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
            var person = new Person("last", "first", "male", System.DateTimeOffset.Now, "blue");
            Assert.AreEqual(0, person.PersonId, "PersonId should be initialized to 0.");
        }

        [TestMethod]
        public void CopyConstructorSetsDifferentPersonId()
        {
            var person = new Person("last", "first", "male", System.DateTimeOffset.Now, "blue");
            var newPerson = new Person(5, person);
            Assert.AreNotEqual(0, newPerson.PersonId);
            Assert.AreEqual(5, newPerson.PersonId);
            Assert.AreNotSame(person, newPerson);
        }

        [TestMethod]
        public void Equality()
        {
            var time = System.DateTimeOffset.Now;
            var personSame1 = new Person("last", "first", "male", time, "blue");
            var personSame2 = new Person("last", "first", "male", time, "blue");
            var personDifferent = new Person("fakelast", "fakefirst", "joe", System.DateTimeOffset.UtcNow, "red");
            Assert.IsTrue(personSame1.Equals(personSame2));
            Assert.IsTrue(personSame1.GetHashCode() == personSame2.GetHashCode());
            Assert.IsFalse(personSame1.Equals(personDifferent));
            Assert.IsFalse(personSame1.Equals(null));
            Assert.IsFalse(personSame1.Equals("blah"));
            Assert.IsFalse(personSame1.GetHashCode() == personDifferent.GetHashCode());
        }

        [TestMethod]
        public void StringFormatIsCorrect()
        {
            var person = new Person("last", "first", "male", System.DateTimeOffset.Parse("01/24/1987"), "blue");
            Assert.AreEqual("last | first | male | 1/24/1987 | blue", person.ToString());
        }
    }
}
