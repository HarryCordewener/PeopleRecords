using Microsoft.VisualStudio.TestTools.UnitTesting;
using PeopleRecords.Models;

namespace PeopleRecords.UnitTests.Model
{
    [TestClass]
    public class PersonModelTests
    {
        [TestMethod]
        public void BaseConstructorSetsIdToZero()
        {
            var person = new Person("first", "last", "male", System.DateTimeOffset.Now, "blue");
            Assert.AreEqual(0, person.PersonId, "PersonId should be initialized to 0.");
        }

        [TestMethod]
        public void CopyConstructorSetsDifferentPersonId()
        {
            var person = new Person("first", "last", "male", System.DateTimeOffset.Now, "blue");
            var newPerson = new Person(5, person);
            Assert.AreNotEqual(0, newPerson.PersonId);
            Assert.AreEqual(5, newPerson.PersonId);
            Assert.AreNotSame(person, newPerson);
        }
    }
}
