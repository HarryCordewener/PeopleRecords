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

            Assert.ThrowsException<KeyNotFoundException>( () => repo.UpdatePerson(falseUpdate));
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
