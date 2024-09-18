using Microsoft.AspNetCore.Http;
using ServiceContracts.DTO;

namespace ServiceContracts
{
    /// <summary>
    /// Represents business logic for manipulating Country entity
    /// </summary>
    
    public interface ICountryService
    {
        /// <summary>
        /// Adds a country onject to the list of countries
        /// </summary>
        /// <param name="countryAddRequest">Country object to be add</param>
        /// <returns>Return Country object as CountryResponse after adding it (including newly generated country id)</returns>
        Task<CountryResponse> AddCountry(CountryAddRequest? countryAddRequest);


        /// <summary>
        /// Return all countries from the list
        /// </summary>
        /// <returns>Return all countries from the list as List<CountryResponse></returns>
        Task<List<CountryResponse>> GetAllCountries();

        /// <summary>
        /// Return Country object based on given country ID
        /// </summary>
        /// <param name="countryID">CountryID (guid) to search</param>
        /// <returns>Matching country as CountryResponse object</returns>
        Task<CountryResponse?> GetCountryByCountryID(Guid? countryID);

        /// <summary>
        /// Upload countries from excel file into database
        /// </summary>
        /// <param name="formFile">Excel file containing lists of countries</param>
        /// <returns>Returns number of countries inserted into DB</returns>
        Task<int> UploadCountriesFromExcelFile(IFormFile formFile);
    }
}
