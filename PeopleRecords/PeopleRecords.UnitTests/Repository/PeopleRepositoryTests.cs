using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PeopleRecords.DataAccess;
using PeopleRecords.Interfaces;
using PeopleRecords.Models;
using Microsoft.Extensions.Logging;
using Moq;

namespace PeopleRecords.UnitTests.Repository
{
    [TestClass]
    public class PeopleRepositoryTests
    {
        Mock<ILogger<InMemoryPeopleRepository>> logger = new Mock<ILogger<InMemoryPeopleRepository>>();

        [TestInitialize]
        public void Initialize()
        {
            logger.Reset();
        }

        [TestMethod]
        public void BaseConstructorHasEmptyList()
        {
            IPeopleRepository repo = new InMemoryPeopleRepository(logger.Object);
            Assert.IsFalse(repo.ReadPeople().Any());
        }

        [TestMethod]
        public void ReadAllSortedByName()
        {
            var people = new List<Person>()
            {
                new Person("d", "e", "f", DateTimeOffset.Parse("04-02-1987 00:00"), "g"),
                new Person("c", "d", "e", DateTimeOffset.Parse("03-02-1987 00:00"), "f"),
                new Person("b", "c", "d", DateTimeOffset.Parse("02-02-1987 00:00"), "e"),
                new Person("a", "b", "c", DateTimeOffset.Parse("01-02-1987 00:00"), "d")
            };

            IPeopleRepository repo = new InMemoryPeopleRepository(logger.Object);

            // This will insert it in the wrong order: ascending instead of descending.
            foreach (var item in people.OrderBy(x => x.FirstName))
            {
                repo.CreatePerson(item);
            }

            List<Person> orderedPeople = repo.ReadPeople(OrderOption.name).ToList();

            for (int i = 0; i < 4; i++)
            {
                Assert.AreEqual(people[i].FavoriteColor, orderedPeople[i].FavoriteColor);
                Assert.AreEqual(people[i].FirstName, orderedPeople[i].FirstName);
                Assert.AreEqual(people[i].LastName, orderedPeople[i].LastName);
                Assert.AreEqual(people[i].DateOfBirth, orderedPeople[i].DateOfBirth);
                Assert.AreEqual(people[i].Gender, orderedPeople[i].Gender);
            }
        }

        [TestMethod]
        public void ReadAllSortedByGender()
        {
            var people = new List<Person>()
            {
                new Person("a", "e", "f", DateTimeOffset.Parse("04-02-1987 00:00"), "g"),
                new Person("b", "d", "f", DateTimeOffset.Parse("03-02-1987 00:00"), "f"),
                new Person("c", "c", "m", DateTimeOffset.Parse("02-02-1987 00:00"), "e"),
                new Person("d", "b", "m", DateTimeOffset.Parse("01-02-1987 00:00"), "d")
            };

            IPeopleRepository repo = new InMemoryPeopleRepository(logger.Object);

            // This will insert it in the wrong order: descending last names in stead of ascending.
            foreach (var item in people.OrderByDescending(x => x.FirstName))
            {
                repo.CreatePerson(item);
            }

            List<Person> orderedPeople = repo.ReadPeople(OrderOption.gender).ToList();

            for (int i = 0; i < 4; i++)
            {
                Assert.AreEqual(people[i].FavoriteColor, orderedPeople[i].FavoriteColor);
                Assert.AreEqual(people[i].FirstName, orderedPeople[i].FirstName);
                Assert.AreEqual(people[i].LastName, orderedPeople[i].LastName);
                Assert.AreEqual(people[i].DateOfBirth, orderedPeople[i].DateOfBirth);
                Assert.AreEqual(people[i].Gender, orderedPeople[i].Gender);
            }
        }

        [TestMethod]
        public void ReadAllSortedByBirthDate()
        {
            var people = new List<Person>()
            {
                new Person("d", "e", "f", DateTimeOffset.Parse("01-02-1987 00:00"), "g"),
                new Person("c", "d", "e", DateTimeOffset.Parse("02-02-1987 00:00"), "f"),
                new Person("b", "c", "d", DateTimeOffset.Parse("03-02-1987 00:00"), "e"),
                new Person("a", "b", "c", DateTimeOffset.Parse("04-02-1987 00:00"), "d")
            };

            IPeopleRepository repo = new InMemoryPeopleRepository(logger.Object);

            // This will insert it in the wrong order: ascending instead of descending.
            foreach (var item in people.OrderByDescending(x => x.DateOfBirth))
            {
                repo.CreatePerson(item);
            }

            List<Person> orderedPeople = repo.ReadPeople(OrderOption.birthdate).ToList();

            for (int i = 0; i < 4; i++)
            {
                Assert.AreEqual(people[i].FavoriteColor, orderedPeople[i].FavoriteColor);
                Assert.AreEqual(people[i].FirstName, orderedPeople[i].FirstName);
                Assert.AreEqual(people[i].LastName, orderedPeople[i].LastName);
                Assert.AreEqual(people[i].DateOfBirth, orderedPeople[i].DateOfBirth);
                Assert.AreEqual(people[i].Gender, orderedPeople[i].Gender);
            }
        }

        [TestMethod]
        public void CreateFailsOnNonZeroPersonId()
        {
            IPeopleRepository repo = new InMemoryPeopleRepository(logger.Object);
            var person = new Person("first", "last", "male", System.DateTimeOffset.Now, "blue");
            var newPerson = new Person(5, person);

            repo.CreatePerson(person);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => repo.CreatePerson(newPerson));
        }

        [TestMethod]
        public void CreateDoesNotFailOnSamePerson()
        {
            IPeopleRepository repo = new InMemoryPeopleRepository(logger.Object);
            var person = new Person("first", "last", "male", System.DateTimeOffset.Now, "blue");

            repo.CreatePerson(person);
            repo.CreatePerson(person);

            Assert.AreEqual(2, repo.ReadPeople().Count());
        }

        [TestMethod]
        public void ReadFailsOnNonExistantId()
        {
            IPeopleRepository repo = new InMemoryPeopleRepository(logger.Object);
            var person = new Person("first", "last", "male", System.DateTimeOffset.Now, "blue");

            Person created = repo.CreatePerson(person);

            Assert.ThrowsException<KeyNotFoundException>(() => repo.ReadPerson(created.PersonId + 1));
        }

        [TestMethod]
        public void ReadSucceedsOnExistantId()
        {
            IPeopleRepository repo = new InMemoryPeopleRepository(logger.Object);
            var person = new Person("first", "last", "male", System.DateTimeOffset.Now, "blue");

            Person created = repo.CreatePerson(person);
            Person read = repo.ReadPerson(created.PersonId);

            Assert.AreSame(created, read);
        }

        [TestMethod]
        public void UpdateFailsOnNonExistantId()
        {
            IPeopleRepository repo = new InMemoryPeopleRepository(logger.Object);
            var person = new Person("first", "last", "male", System.DateTimeOffset.Now, "blue");

            Person created = repo.CreatePerson(person);
            created.FavoriteColor = "green";

            Person falseUpdate = new Person(created.PersonId + 1, created);

            Assert.ThrowsException<KeyNotFoundException>(() => repo.UpdatePerson(falseUpdate));
        }

        [TestMethod]
        public void UpdateSucceeds()
        {
            IPeopleRepository repo = new InMemoryPeopleRepository(logger.Object);
            var person = new Person("first", "last", "male", System.DateTimeOffset.Now, "blue");

            Person created = repo.CreatePerson(person);

            created.FavoriteColor = "green";
            Person updated = repo.UpdatePerson(created);

            Assert.AreNotEqual(person, updated);
            Assert.AreNotEqual(person.FavoriteColor, updated.FavoriteColor);
            Assert.AreEqual(person.FirstName, updated.FirstName);
            Assert.AreEqual(person.LastName, updated.LastName);
            Assert.AreEqual(person.DateOfBirth, updated.DateOfBirth);
            Assert.AreEqual(person.Gender, updated.Gender);
            Assert.AreEqual(person.PersonId, updated.PersonId);
        }

        [TestMethod]
        public void DeleteFailsOnNonExistantId()
        {
            IPeopleRepository repo = new InMemoryPeopleRepository(logger.Object);
            var person = new Person("first", "last", "male", System.DateTimeOffset.Now, "blue");

            Person created = repo.CreatePerson(person);

            Assert.ThrowsException<KeyNotFoundException>(() => repo.DeletePerson(created.PersonId + 1));

            Assert.AreEqual(1, repo.ReadPeople().Count());
        }

        [TestMethod]
        public void DeleteSucceedsOnExistantId()
        {
            IPeopleRepository repo = new InMemoryPeopleRepository(logger.Object);
            var person = new Person("first", "last", "male", System.DateTimeOffset.Now, "blue");

            Person created = repo.CreatePerson(person);

            repo.DeletePerson(created.PersonId);

            Assert.IsFalse(repo.ReadPeople().Any());
        }
    }
}
