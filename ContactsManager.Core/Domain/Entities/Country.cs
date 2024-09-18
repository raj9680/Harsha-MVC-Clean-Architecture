using System.ComponentModel.DataAnnotations;

namespace Entities
{
    /// <summary>
    /// Domain Model for Country
    /// </summary>
    public class Country
    {
        [Key]
        public Guid CountryID { get; set; }
        public string? CountryName { get; set; }

        // Below persons property will load all the person property with specific country ID
        public virtual ICollection<Person>? Persons { get; set; }
    }
}
