using Entities;

namespace ServiceContracts.DTO
{
    /// <summary>
    /// DTO class for adding a new country
    /// </summary>
    public class CountryAddRequest
    {
        public string? CountryName { get; set; }


        // It will create & return an object oh the country class, It converts the existing CountryAddRequest object into a new object of Country class  
        public Country ToCountry()
        {
            return new Country() { CountryName = CountryName };
        }
    }
}
