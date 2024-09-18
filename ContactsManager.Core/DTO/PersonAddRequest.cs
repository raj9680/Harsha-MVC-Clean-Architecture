using Entities;
using ServiceContracts.Enums;
using System.ComponentModel.DataAnnotations;

namespace ServiceContracts.DTO
{
    /// <summary>
    /// Act as DTO fro inserting a new person
    /// </summary>
    public class PersonAddRequest
    {
        [Required(ErrorMessage="Person name can't be blank")]
        public string? PersonName { get; set; }

        [Required(ErrorMessage = "Email can't be blank")]
        [EmailAddress(ErrorMessage = "Email should be valid")]
  [DataType(DataType.EmailAddress)] // It will help provide basic val + type="Email" in html input
        public string? Email { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }
        public GenderOptions Gender { get; set; }
        public Guid? CountryID { get; set; }
        public bool ReceivesNewsLetter { get; set; }


        /// <summary>
        /// Converting the current object of PersonAddRequest into a new object of Person
        /// type
        /// </summary>
        /// <returns></returns>
        public Person ToPerson()
        {
            return new Person()
            {
                PersonName = this.PersonName,
                Email = this.Email,
                DateOfBirth = this.DateOfBirth,
                Gender = this.Gender.ToString(),
                CountryID = this.CountryID,
                ReceiveNewsLetters = this.ReceivesNewsLetter
            };
        }
    }
}
