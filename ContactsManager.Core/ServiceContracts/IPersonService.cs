using ServiceContracts.DTO;
using ServiceContracts.Enums;

namespace ServiceContracts
{
    /// <summary>
    /// Represents business logic for manipulating Person Entity
    /// </summary>
    
    public interface IPersonService
    {
        /// <summary>
        /// Adds new person to the exisiting list of persons
        /// </summary>
        /// <param name="personAddRequest">Person object to add</param>
        /// <returns>Returns same person details, along with newly generated personID
        /// </returns>
        Task<PersonResponse> AddPerson(PersonAddRequest? personAddRequest);

        /// <summary>
        /// Return All Person
        /// </summary>
        /// <returns>Perosn List</returns>
        Task<List<PersonResponse>> GetAllPersons();


        /// <summary>
        /// Return person object based on given person id
        /// </summary>
        /// <param name="personID">Person id to search</param>
        /// <returns>Matching person object</returns>
        Task<PersonResponse?> GetPersonByPersonID(Guid? personID);

         /// <summary>
        /// Return all person objects that matches with the given search field and serchStr
        /// </summary>
        /// <param name="searchBy">Search field to search</param>
        /// <param name="searchString">Search string to search</param>
        /// <returns>Returns all matching persons based on the given search string & search
        /// </returns>
        Task<List<PersonResponse>> GetFilteredPersons(string? searchBy, string? searchString);


        /// <summary>
        /// Returns sorted list of Persons
        /// </summary>
        /// <param name="allPersons">Represents list of persons to sort</param>
        /// <param name="sortBy">Name of the property (key), based on which the person should be sort</param>
        /// <param name="sortorderOptions">ASC or DESc</param>
        /// <returns>Return List Persons after sorting</returns>
        Task<List<PersonResponse>> GetSortedPerson(List<PersonResponse> allPersons, string sortBy, SortOrderOptions sortorderOptions);


        /// <summary>
        /// Updates the specified person details based on the given PersonID
        /// </summary>
        /// <param name="personUpdateRequest">Person details to update including person ID</param>
        /// <returns>Updated Person Details Object after Updation</returns>
        Task<PersonResponse> UpdatePerson(PersonUpdateRequest? personUpdateRequest);

        /// <summary>
        /// Delet person based on given person ID
        /// </summary>
        /// <param name="personID">PersonID to Delete</param>
        /// <returns>Returns true/false when person deleted/not deleted</returns>
        Task<bool> DeletePerson(Guid? personID);

        /// <summary>
        /// Returns persons as CSV
        /// </summary>
        /// <returns>Returns the memory stream with CSV data</returns>
        Task<MemoryStream> GetPersonCSV();
        
        /// <summary>
        /// Returns persons as CSV custom fields
        /// </summary>
        /// <returns>Returns the memory stream with CSV custom fields data</returns>
        Task<MemoryStream> GetPersonCSVCustomFields();

        /// <summary>
        /// Returns persons as Excel
        /// </summary>
        /// <returns>Returns the memory stream with Excel data</returns>
        Task<MemoryStream> GetPersonExcel();
    }
}
