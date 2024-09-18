using Entities;
using System.Linq.Expressions;

namespace RepositoryContracts
{
    /// <summary>
    /// Returns data access logic for managing Person entity
    /// </summary>
    public interface IPersonsRepository
    {
        /// <summary>
        /// It adds person and return person object
        /// </summary>
        /// <param name="person">Takes person object as param</param>
        /// <returns>Return person object after adding or null</returns>
        Task<Person> AddPerson(Person person);

        /// <summary>
        /// Return List of all person as object
        /// </summary>
        /// <returns>List of all persons</returns>
        Task<List<Person>> GetAllPersons();

        /// <summary>
        /// Return person object based on given person ID
        /// </summary>
        /// <param name="personID">Takes personID as parameter</param>
        /// <returns>Returns person object based on matching ID</returns>
        Task<Person> GetPersonByPersonId(Guid personID);

        /// <summary>
        /// Return all person object based on the given expression
        /// </summary>
        /// <param name="predicate">LINQ expression to check</param>
        /// <returns>All matching person with givenr condition</returns>
        Task<List<Person>> GetFilteredPersons(Expression<Func<Person, bool>> predicate);

        /// <summary>
        /// Deletes person based on personID
        /// </summary>
        /// <param name="personID">Person ID to search</param>
        /// <returns>Return true, if the deletion is successful otherwise false</returns>
        Task<bool> DeletePersonByPersonID(Guid personID);

        /// <summary>
        /// updates a person object - person name and other details based
        /// on given person ID
        /// </summary>
        /// <param name="person">object of person to uddate</param>
        /// <returns>Return updated person object</returns>
        Task<Person> UpdatePerson(Person person);
    }
}
