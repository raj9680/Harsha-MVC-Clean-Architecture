using Entities;

namespace RepositoryContracts
{
    /// <summary>
    /// Represents data access logic for managing Country entity
    /// </summary>
    public interface ICountriesRepository
    {
        /// <summary>
        /// Adds a new country object to the data store
        /// </summary>
        /// <param name="country">country object to add</param>
        /// <returns>Returns the country object after adding it to the data store</returns>
        Task<Country> AddCountry(Country country);

        /// <summary>
        /// Returns all countries from data store
        /// </summary>
        /// <returns>All Countires from the table</returns>
        Task<List<Country>> GetAllCountries();

        /// <summary>
        /// Return matching Country based on countryID object, else null
        /// </summary>
        /// <param name="countryID">Country ID to search</param>
        /// <returns>Matching Country Object or null</returns>
        Task<Country> GetCountryById(Guid countryID);

        /// <summary>
        /// Return a country object based on matching country name
        /// </summary>
        /// <param name="countryName">country name</param>
        /// <returns>Matching country name or null</returns>
        Task<Country> GetCountryByName(string countryName);
    }
}
