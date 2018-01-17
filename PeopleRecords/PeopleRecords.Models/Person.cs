using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace PeopleRecords.Models
{
    /// <summary>
    ///  last name, first name, gender, date of birth and favorite 
    /// </summary>
    public class Person : IEquatable<Person>
    {
        public int PersonId { get; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Gender { get; set; }
        public DateTimeOffset DateOfBirth { get; set; }
        public string FavoriteColor { get; set; }

        /// <summary>
        /// Generates a person with the information provided. Defaults the PersonId to 0.
        /// </summary>
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
        /// Generates a person with the information provided.
        /// </summary>
        /// <param name="personId">The record's Identifier</param>
        /// <param name="lastName">The record's Last Name</param>
        /// <param name="firstName">The record's First Name</param>
        /// <param name="gender">The record's Gender</param>
        /// <param name="dateOfBirth">The record's Date of Birth</param>
        /// <param name="favoriteColor">The record's Favorite color</param>
        public Person(int personId, string lastName, string firstName, string gender, DateTimeOffset dateOfBirth, string favoriteColor)
        {
            PersonId = personId;
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

        public Person()
        {
        }

        public override bool Equals(Object obj)
        {
            // Check for null values and compare run-time types.
            if (obj == null || GetType() != obj.GetType())
                return false;

            Person other = (Person)obj;
            return PersonId == other.PersonId &&
                LastName == other.LastName &&
                FirstName == other.FirstName &&
                Gender == other.Gender &&
                DateOfBirth.Equals(other.DateOfBirth) &&
                FavoriteColor == other.FavoriteColor;
        }

        public bool Equals(Person other)
        {
            return other != null &&
                   PersonId == other.PersonId &&
                   LastName == other.LastName &&
                   FirstName == other.FirstName &&
                   Gender == other.Gender &&
                   DateOfBirth.Equals(other.DateOfBirth) &&
                   FavoriteColor == other.FavoriteColor;
        }

        public override int GetHashCode()
        {
            var hashCode = 1781705854;
            hashCode = hashCode * -1521134295 + PersonId.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(LastName);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(FirstName);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Gender);
            hashCode = hashCode * -1521134295 + EqualityComparer<DateTimeOffset>.Default.GetHashCode(DateOfBirth);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(FavoriteColor);
            return hashCode;
        }

        public override string ToString()
        {
            return $"{LastName} | {FirstName} | {Gender} | {DateOfBirth.ToString("M/D/YYYY")} | {FavoriteColor}";
        }
    }
}
