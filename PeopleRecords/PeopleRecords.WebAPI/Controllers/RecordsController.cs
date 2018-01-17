using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using PeopleRecords.Interfaces;
using PeopleRecords.Models;
using PeopleRecords.DataAccess;
using System.Linq;
using System;

namespace PeopleRecords.WebAPI.Controllers
{
    [Route("api/[controller]")]
    public class RecordsController : Controller
    {
        IPeopleRepository PeopleRepository { get; }

        public RecordsController(IPeopleRepository peopleRepository)
        {
            PeopleRepository = peopleRepository;
        }

        // GET api/records
        [HttpGet]
        public IEnumerable<Person> Get()
        {
            return PeopleRepository.ReadPeople();
        }

        [HttpGet("{order:regex(^(name|birthdate|gender)$)}")]
        public IEnumerable<Person> GetByOrder(string order)
        {
            object result;
            if(!System.Enum.TryParse(typeof(OrderOption), order, out result)) throw new ArgumentOutOfRangeException(order);
            return PeopleRepository.ReadPeople((OrderOption)result);
        }


        // GET api/records/5
        [HttpGet("{id:int}")]
        public Person Get(int id)
        {
            return PeopleRepository.ReadPerson(id);
        }

        // POST api/records
        [HttpPost("json")]
        public Person PostJson([FromBody]Person value)
        {
            return PeopleRepository.CreatePerson(value);
        }

        /// <summary>
        /// Required api endpoint #1.
        /// </summary>
        [HttpPost]
        public Person Post([FromBody]string lineValue)
        {
            LineReader.ImportFileIntoRepository(new[] { lineValue }, PeopleRepository);
            return PeopleRepository.ReadPeople().Last();
        }

        // PUT api/records/5
        [HttpPut("{id}")]
        public Person Put(int id, [FromBody]Person value)
        {
            var person = new Person(id, value);
            Person updated = null;

            if (id == value.PersonId)
            {
               updated = PeopleRepository.UpdatePerson(person);
            }

            return updated;
        }

        // DELETE api/records/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            PeopleRepository.DeletePerson(id);
        }
    }
}
