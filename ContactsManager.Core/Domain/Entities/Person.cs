using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    public class Person
    {
        [Key]
        public Guid PersonID { get; set; }

        [StringLength(40)] // nvarchar(40)
        public string? PersonName { get; set; }

        [StringLength(40)]
        public string? Email { get; set; }
        public DateTime? DateOfBirth { get; set; }

        [StringLength(10)]
        public string? Gender { get; set; }

        // Unique-Identifier
        public Guid? CountryID { get; set; }

        // bit
        public bool? ReceiveNewsLetters { get; set; }

        public string? TIN { get; set; }


        // relationship
        [ForeignKey("CountryID")]
        public Country? Country { get; set; }

        public override string ToString()
        {
            return $"Person Id: {PersonID}, Person Name: {PersonName}, Email: {Email}, DOB: {DateOfBirth}, Gender: {Gender}, Country: {Country?.CountryName}, ReceiveNewsLetter: {ReceiveNewsLetters}";
        }

    }
}
