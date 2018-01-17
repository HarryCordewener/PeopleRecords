using PeopleRecords.Interfaces;
using PeopleRecords.Models;
using System.Text.RegularExpressions;
using System.IO;
using System.Threading.Tasks;
using System;

namespace PeopleRecords.DataAccess
{
    static public class LineReader
    {
        private static Regex LineMatch = new Regex(@"^" +
            @"(?<LastName>\w+)[\u007c\s,]\s*" +
            @"(?<FirstName>\w+)[\u007c\s,]\s*" +
            @"(?<Gender>\w+)[\u007c\s,]\s*" +
            @"(?<FavoriteColor>\w+)[\u007c\s,]\s*" +
            @"(?<DateOfBirth>.+)" +
            @"$");

        /// <summary>
        /// Asynchronously imports a file into an <see cref="IPeopleRepository"/>. Throws an exception on any failed line, and stops.
        /// </summary>
        /// <param name="filePath">The file path</param>
        /// <param name="peopleRepository">The repository to import into</param>
        /// <returns></returns>
        public static void ImportFileIntoRepository(string[] lines, IPeopleRepository peopleRepository)
        {
            try
            {
                foreach (var line in lines)
                {
                    Person processedLine = TransformLineToPerson(line);
                    peopleRepository.CreatePerson(processedLine);
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Transforms a line item into a Person.
        /// </summary>
        /// <example>
        /// LastName | FirstName | Gender | FavoriteColor | DateOfBirth
        /// </example>
        /// <example>
        /// LastName, FirstName, Gender, FavoriteColor, DateOfBirth
        /// </example>
        /// <example>
        /// LastName FirstName Gender FavoriteColor DateOfBirth
        /// </example>
        /// <param name="line">A line, either comma, space, or pipe-delimited</param>
        /// <returns>A person</returns>
        private static Person TransformLineToPerson(string line)
        {
            DateTimeOffset dateOfBirth;

            var match = LineMatch.Match(line);
            if (!match.Success) throw new ArgumentOutOfRangeException(nameof(line));

            string lastName = match.Groups["LastName"].Value;
            string firstName = match.Groups["FirstName"].Value;
            string gender = match.Groups["Gender"].Value;
            string favoriteColor = match.Groups["FavoriteColor"].Value;
            string dateOfBirthString = match.Groups["DateOfBirth"].Value;

            if (!DateTimeOffset.TryParse(dateOfBirthString, out dateOfBirth)) throw new ArgumentOutOfRangeException("dateOfBirth");

            return new Person(lastName, firstName, gender, dateOfBirth, favoriteColor);
        }
    }
}
