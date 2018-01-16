using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PeopleRecords.Interfaces;
using PeopleRecords.DataAccess;
using PeopleRecords.Models;
using System.Threading.Tasks;
using System.Linq;

namespace PeopleRecords.UnitTests
{
    [TestClass]
    public class LineReaderTests
    {
        [TestMethod]
        public void TestLineSuccess()
        {
            IPeopleRepository repo = new InMemoryPeopleRepository();

            // There is a discrepancy that gets introduced here, if we don't parse the stringified value.
            DateTimeOffset nowTime = DateTimeOffset.Now;
            string now = nowTime.ToString();
            nowTime = DateTimeOffset.Parse(now);

            var testPerson = new Person("last", "first", "gender", nowTime, "favoriteColor");

            string[] lines = new string[] {
                $"last,first,gender,favoriteColor,{now}",
                $"last, first, gender, favoriteColor, {now}",
                $"last|first|gender|favoriteColor|{now}",
                $"last| first| gender| favoriteColor| {now}",
                $"last first gender favoriteColor  {now}",
                $"last  first  gender  favoriteColor  {now}",
            };

            LineReader.ImportFileIntoRepositoryAsync(Task.FromResult(lines), repo).GetAwaiter().GetResult();

            Assert.AreEqual(6, repo.ReadPeople().Count());
            foreach (var person in repo.ReadPeople())
            {
                Assert.AreEqual(testPerson.LastName, person.LastName);
                Assert.AreEqual(testPerson.FirstName, person.FirstName);
                Assert.AreEqual(testPerson.Gender, person.Gender);
                Assert.AreEqual(testPerson.FavoriteColor, person.FavoriteColor);
                Assert.AreEqual(testPerson.DateOfBirth, person.DateOfBirth);
            }
        }

        [TestMethod]
        public void TestLineFailsOnBadLine_WrongDelimiter()
        {
            IPeopleRepository repo = new InMemoryPeopleRepository();

            // There is a discrepancy that gets introduced here, if we don't parse the stringified value.
            DateTimeOffset nowTime = DateTimeOffset.Now;
            string now = nowTime.ToString();
            nowTime = DateTimeOffset.Parse(now);

            string[] lines = new string[] {
                $"last-first-gender-favoriteColor-{now}",
            };

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => LineReader.ImportFileIntoRepositoryAsync(Task.FromResult(lines), repo).GetAwaiter().GetResult());
        }

        [TestMethod]
        public void TestLineFailsOnBadLine_ShortLine()
        {
            IPeopleRepository repo = new InMemoryPeopleRepository();

            // There is a discrepancy that gets introduced here, if we don't parse the stringified value.
            DateTimeOffset nowTime = DateTimeOffset.Now;
            string now = nowTime.ToString();
            nowTime = DateTimeOffset.Parse(now);

            string[] lines = new string[] {
                $"last-first-gender-favoriteColor",
            };

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => LineReader.ImportFileIntoRepositoryAsync(Task.FromResult(lines), repo).GetAwaiter().GetResult());
        }
    }
}
