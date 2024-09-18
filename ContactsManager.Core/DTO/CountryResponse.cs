using Entities;

namespace ServiceContracts.DTO
{
    /// <summary>
    /// DTO class that is used as return type for most of CountriesService methods
    /// </summary>
    
    public class CountryResponse
    {
        public Guid CountryID { get; set; }
        public string? CountryName { get; set; }


        // Very Important - overrided equal method to compare value isntead of object
        // It compares current object to another object of CountryResponse type and return true, if both values are same; otherwise false
        public override bool Equals(object? obj)
        {
            if (obj == null || obj.GetType() != typeof(CountryResponse)) 
            { 
                return false;
            }

            CountryResponse country_to_compare = (CountryResponse) obj;
            return 
            this.CountryID == country_to_compare.CountryID && this.CountryName == country_to_compare.CountryName;
            // return true if condition matches else return false
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }


    /// <summary>
    /// As a part of business logic we have to convert the data from Country object into 
    /// CountryResponse ex: user is trying to retrieve all existing countries from list
    /// countries (List of entity model (Country)). So each of the Country object should be
    /// converted into the country response, so that's why we are adding extension method
    /// called 'ToCountryResponse' which converts the data from Country object into
    /// CountryResponse
    /// This Extension method is getting added to Country class automatically
    /// </summary>
    
    public static class CountryExtensions
    {
        public static CountryResponse ToCountryResponse(this Country country)
        {
            return new CountryResponse()
            {
                CountryID = country.CountryID,
                CountryName = country.CountryName
            };
        }
    }
}
