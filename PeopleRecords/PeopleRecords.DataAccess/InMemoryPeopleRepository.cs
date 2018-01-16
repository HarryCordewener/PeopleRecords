using PeopleRecords.Interfaces;
using System;
using PeopleRecords.Models;
using System.Collections.Generic;
namespace PeopleRecords.DataAccess
{
    public class InMemoryPeopleRepository : IPeopleRepository
    {
        private Dictionary<int, Person> People { get; }
        int IncrementingCounter { get; set; } = 0;

        public InMemoryPeopleRepository()
        {
            People = new Dictionary<int, Person>();
        }

        public Person CreatePerson(Person person)
        {
            if (person.PersonId != 0) throw new ArgumentOutOfRangeException(nameof(person.PersonId));
            var IdentifiedPerson = new Person(IncrementingCounter, person);
            People.Add(IncrementingCounter, IdentifiedPerson);
            IncrementingCounter++;
            return IdentifiedPerson;
        }

        public void DeletePerson(int id)
        {
            if (!People.ContainsKey(id)) throw new KeyNotFoundException();
            People.Remove(id);
        }

        public IEnumerable<Person> ReadPeople()
        {
            foreach (var kvPair in People)
            {
                yield return kvPair.Value;
            }
        }

        public Person ReadPerson(int id)
        {
            if (!People.ContainsKey(id)) throw new KeyNotFoundException();
            return People[id];
        }

        public Person UpdatePerson(Person target)
        {
            if (!People.ContainsKey(target.PersonId)) throw new KeyNotFoundException();
            People[target.PersonId] = target;
            return target;
        }
    }
}
