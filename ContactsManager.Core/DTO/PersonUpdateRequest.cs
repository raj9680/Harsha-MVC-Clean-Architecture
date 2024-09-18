using Entities;
using ServiceContracts.Enums;
using System.ComponentModel.DataAnnotations;

namespace ServiceContracts.DTO
{
    public class PersonUpdateRequest
    {
        /// <summary>
        /// Act as DTO fro updating existing person
        /// </summary>

        [Required(ErrorMessage = "Person ID can't be blank")]
        public Guid PersonID { get; set; }

        [Required(ErrorMessage = "Person name can't be blank")]
            public string? PersonName { get; set; }

            [Required(ErrorMessage = "Email can't be blank")]
            [EmailAddress(ErrorMessage = "Email should be valid")]
            public string? Email { get; set; }

            public DateTime? DateOfBirth { get; set; }
            public GenderOptions? Gender { get; set; }
            public Guid? CountryID { get; set; }
            public bool ReceiveNewsLetters { get; set; }


        /// <summary>
        /// Converting the current object of PersonUpdateRequest into a new object of Person
        /// type
        /// </summary>
        /// <returns></returns>
        public Person ToPerson()
            {
                return new Person()
                {
                    PersonID = PersonID,
                    PersonName = this.PersonName,
                    Email = this.Email,
                    DateOfBirth = this.DateOfBirth,
                    Gender = this.Gender.ToString(),
                    CountryID = this.CountryID,
                    ReceiveNewsLetters = this.ReceiveNewsLetters
                };
            }
    }
}
