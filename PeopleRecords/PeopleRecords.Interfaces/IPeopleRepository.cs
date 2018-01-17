using PeopleRecords.Models;
using System.Collections.Generic;

namespace PeopleRecords.Interfaces
{
    public enum OrderOption
    {
        gender = 0,
        birthdate = 1,
        name = 2
    }

    public interface IPeopleRepository
    {
        /// <summary>
        /// Tries to create Person. Use 0 for Identifier of new People.
        /// </summary>
        /// <param name="person">A Person with an Identifier of 0</param>
        /// <exception cref="ArgumentOutOfRangeException">Identifier was not 0</exception>
        /// <returns>The created Person with the newly initialized Identifier</returns>
        Person CreatePerson(Person person);

        /// <summary>
        /// Reads all People in the repository.
        /// </summary>
        /// <returns>All People</returns>
        IEnumerable<Person> ReadPeople();

        /// <summary>
        /// Reads all People in the repository, using an Order Type.
        /// </summary>
        /// <returns>All People</returns>
        IEnumerable<Person> ReadPeople(OrderOption order);

        /// <summary>
        /// Reads a Person based on their ID.
        /// </summary>
        /// <param name="id">The integer Person identifier.</param>
        /// <exception cref="KeyNotFoundException">No person found with that ID.</exception>
        /// <returns>A Person with a matching ID.</returns>
        Person ReadPerson(int id);

        /// <summary>
        /// Updates a Person based on their ID and the fields given.
        /// </summary>
        /// <param name="target">The update target, with updated fields.</param>
        /// <exception cref="KeyNotFoundException">No person found with that ID.</exception>
        /// <returns>The updated record.</returns>
        Person UpdatePerson(Person target);

        /// <summary>
        /// Deletes the Person based on their ID.
        /// </summary>
        /// <param name="id">The integer Person identifier.</param>
        /// <exception cref="KeyNotFoundException">No person found with that ID.</exception>
        void DeletePerson(int id);
    }
}
