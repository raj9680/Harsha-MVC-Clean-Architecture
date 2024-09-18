using Entities;
using ServiceContracts.Enums;
using System.Runtime.CompilerServices;

namespace ServiceContracts.DTO
{
    /// <summary>
    /// Represents DTO class that is used as return type of most methods of Persons Service
    /// </summary>
    public class PersonResponse
    {
        public Guid PersonID { get; set; }
        public string? PersonName { get; set; }
        public string? Email { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public Guid? CountryID { get; set; }
        public string? Country { get; set; }
        public bool? ReceivesNewsLetter { get; set; }
        public double? Age { get; set; }

        /// <summary>
        /// Compares the current object data with the parameter object
        /// We want to compare values in our app, & Equals() compares object, Hence we are
        /// overriding Equals() method of the same class to compare values.
        /// </summary>
        /// <param name="obj">The PersonResponse object to compare</param>
        /// <returns>True or False, indicating whether all person details are matched with
        /// the specified paramter object</returns>
        public override bool Equals(object? obj)
        {
            if(obj == null) return false;
            if(obj.GetType() != typeof(PersonResponse)) return false;

            PersonResponse person = (PersonResponse)obj;

            return this.PersonID == person.PersonID && this.PersonName == person.PersonName && this.Email == person.Email && this.DateOfBirth == person.DateOfBirth && this.Gender == person.Gender && this.CountryID == person.CountryID && this.ReceivesNewsLetter == person.ReceivesNewsLetter;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        // To return actual data as response instead of class entity objects
        public override string ToString()
        {
            return 
                $"Person ID: {PersonID}, " +
                $"Person Name: {PersonName}, " +
                $"PersonEmail: {Email}, " +
                $"Date of Birth: {DateOfBirth?.ToString("dd MM yyyy")}, " +
                $"Gender: {Gender}, " +
                $"Country ID: {CountryID}, " +
                $"Address: {Country}, " +
                $"Receivd Newsletter: {ReceivesNewsLetter}";
        }

        public PersonUpdateRequest ToPersonUpdateRequest()
        {
            return new PersonUpdateRequest()
            {
                PersonID = PersonID,
                PersonName = PersonName,
                Email = Email,
                DateOfBirth = DateOfBirth,
                CountryID = CountryID,
                Gender = (GenderOptions)Enum.Parse(typeof(GenderOptions), Gender, true)
            };
        }
    }


    /// <summary>
    /// As a part of business logic we have to convert the data from Country object into 
    /// CountryResponse ex: user is trying to retrieve all existing countries from list
    /// countries (List of entity model (Country)). So each of the Country object should be
    /// converted into the country response, so that's why we are adding extension method
    /// called 'ToCountryResponse' which converts the data from Country object into
    /// CountryResponse
    /// 
    /// THis extesnion method is getting added to Person class automatically
    /// </summary>
    public static class PersonResponseExtensions
    {
        /// <summary>
        /// An Extension method to convert object of Person class into PersonResponse class
        /// </summary>
        /// <param name="person">Person object to convert</param>
        /// <returns>Returns the converted PersonResponse object</returns>

        public static PersonResponse ToPersonResponse(this Person person)
        {
            return new PersonResponse()
            {
                PersonID = person.PersonID,
                PersonName = person.PersonName,
                Email = person.Email,
                DateOfBirth = person.DateOfBirth,
                Gender = person.Gender,
                CountryID = person.CountryID,
                ReceivesNewsLetter = person.ReceiveNewsLetters,
                
                // Code to Calculate Age
                Age = (person.DateOfBirth != null) ? Math.Round((DateTime.Now - person.DateOfBirth.Value).TotalDays / 365.25) : null,
                Country = person.Country?.CountryName
            };
        }

    }
}
