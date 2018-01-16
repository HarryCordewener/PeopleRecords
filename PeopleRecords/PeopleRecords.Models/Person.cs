using System;

namespace PeopleRecords.Models
{
    /// <summary>
    ///  last name, first name, gender, date of birth and favorite 
    /// </summary>
    public class Person
    {
        public int PersonId { get; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Gender { get; set; }
        public DateTimeOffset DateOfBirth { get; set; }
        public string FavoriteColor { get; set; }

        /// <summary>
        /// Generates a person with the information provided.
        /// </summary>
        /// <param name="personId">The record's Identifier, defaults to 0</param>
        /// <param name="lastName">The record's Last Name</param>
        /// <param name="firstName">The record's First Name</param>
        /// <param name="gender">The record's Gender</param>
        /// <param name="dateOfBirth">The record's Date of Birth</param>
        /// <param name="favoriteColor">The record's Favorite color</param>
        public Person(string lastName, string firstName, string gender, DateTimeOffset dateOfBirth, string favoriteColor)
        {
            PersonId = 0;
            LastName = lastName;
            FirstName = firstName;
            Gender = gender;
            DateOfBirth = dateOfBirth;
            FavoriteColor = favoriteColor;
        }

        /// <summary>
        /// Creates an instance of a person with a specific Identifier.
        /// </summary>
        /// <param name="Identifier">The PersonId for the Person</param>
        /// <param name="person">The Person to copy from</param>
        public Person(int Identifier, Person person)
        {
            PersonId = Identifier;
            LastName = person.LastName;
            FirstName = person.FirstName;
            Gender = person.Gender;
            DateOfBirth = person.DateOfBirth;
            FavoriteColor = person.FavoriteColor;
        }
    }
}
