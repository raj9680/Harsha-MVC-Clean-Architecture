using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Entities
{
    public class PersonsDbContext: DbContext
    {
        public PersonsDbContext(DbContextOptions<PersonsDbContext> options) : base(options)
        {
            
        }

        public DbSet<Person> Persons { get; set; }
        public DbSet<Country> Countries { get; set; }

        // Bind above DbSets to corresponsing tables
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Country>().ToTable("TblCountries");
            modelBuilder.Entity<Person>().ToTable("TblPersons");

            // Seed Data
            //modelBuilder.Entity<Country>().HasData(new Country()
            //{
            //    CountryID = Guid.NewGuid(),
            //    CountryName = "India"
            //});

            // Seed Data - Countries using Json file
            string countriesJson = File.ReadAllText("countries.json");

            List<Country>? countries = JsonSerializer.Deserialize<List<Country>>(countriesJson);

            foreach (Country item in countries)
            {
                modelBuilder.Entity<Country>().HasData(item);
            }

            // Seed Data - Persons using Json file
            string personsJson = File.ReadAllText("persons.json");

            List<Person>? persons = JsonSerializer.Deserialize<List<Person>>(personsJson);

            foreach (Person item in persons)
            {
                modelBuilder.Entity<Person>().HasData(item);
            }

            // Fluent API
            modelBuilder.Entity<Person>().Property(temp => temp.TIN)
                .HasColumnName("TaxIdNumber")
                .HasColumnType("varchar(8)")
                .HasDefaultValue("TID20220");

            // For Unique Constraits
            //modelBuilder.Entity<Person>()
            //    .HasIndex(t => t.TIN).IsUnique();

            // Check Constraint - IMPORTANT , off due to conflict
            modelBuilder.Entity<Person>()
                .HasCheckConstraint("CHK_TIN", "len([TaxIdNumber])=8");

            // Table Relations
            // This will create FK relation of Country Table as CountryID in person table - not recommended 
            
            //modelBuilder.Entity<Person>(entity =>
            //{
            //    entity.HasOne<Country>(c => c.Country).WithMany(p => p.Persons).HasForeignKey(p => p.CountryID);
            //});
        }

        // For SP - {08F1963C-0D48-4A63-B923-739483A7D8A7}
        public List<Person> sp_GetAllPersons()
        {
            // After changes in Table
            //return Persons.FromSqlRaw("Execute [dbo].[GetAllPersons]").ToList();
            
            return Persons.FromSqlRaw("Execute [dbo].[GetAllPersons]").ToList();
        }


        public int sp_InsertPerson(Person person)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@PersonID",person.PersonID),
                new SqlParameter("@PersonName", person.PersonName),
                new SqlParameter("@Email", person.Email),
                new SqlParameter("@DateOfBirth", person.DateOfBirth),
                new SqlParameter("@Gender", person.Gender),
                new SqlParameter("@CountryID", person.CountryID),
                new SqlParameter("@ReceiveNewsLetters", person.ReceiveNewsLetters)
            };

            // It will return numbers of row effected i.e int hence return type if fun is also int
            return Database.ExecuteSqlRaw("EXECUTE [dbo].[Insert_Person] @PersonID, @PersonName,@Email, @DateOfBirth,@Gender, @CountryID,@ReceiveNewsLetters", parameters);
        }
    }
}
