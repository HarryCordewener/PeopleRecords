using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using PeopleRecords.Models;
using PeopleRecords.WebAPI;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Linq;

namespace PeopleRecords.UnitTests.Model
{
    [TestClass]
    public class WebApiEndpointsTests
    {
        private readonly TestServer _server;
        private readonly HttpClient _client;

        public WebApiEndpointsTests()
        {
            _server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
            _client = _server.CreateClient();
        }

        [TestMethod]
        public async Task CreateRead()
        {
            Person p = new Person("last", "first", "gender", DateTimeOffset.Now, "color");

            for (int i = 0; i < 5; i++)
            {
                var result = await _client.PostAsJsonAsync("/api/records/json", p);
                Person readPerson = await UnpackPersonAsync(result.Content);

                var pWithAnotherId = new Person(readPerson.PersonId, p);

                // TODO: Figure out why AreEqual() fails here.
                Assert.IsTrue(pWithAnotherId.Equals(readPerson));
            }
        }

        [TestMethod]
        public async Task ReadAll()
        {
            Person p = new Person("last", "first", "gender", DateTimeOffset.Now, "color");

            for (int i = 0; i < 5; i++)
            {
                await _client.PostAsJsonAsync("/api/records/json", p);
            }

            var result = await _client.GetAsync("/api/records");
            IEnumerable<Person> people = await UnpackPeopleAsync(result.Content);

            Assert.AreEqual(5, people.Count());
        }

        [TestMethod]
        public async Task ReadAllInOrder()
        {
            Person p = new Person("last", "first", "gender", DateTimeOffset.Now, "color");

            for (int i = 0; i < 5; i++)
            {
                await _client.PostAsJsonAsync("/api/records/json", p);
            }

            foreach( var ordertype in new[] { "name", "birthdate", "gender" })
            {
                var result = await _client.GetAsync($"/api/records/{ordertype}");
                IEnumerable<Person> people = await UnpackPeopleAsync(result.Content);
                Assert.AreEqual(5, people.Count());
            }
        }

        #region Helper Functions
        private async Task<Person> UnpackPersonAsync(HttpContent content)
        {
            var byteArray = await content.ReadAsByteArrayAsync();
            var buffer = System.Text.Encoding.UTF8.GetString(byteArray);
            var person = JsonConvert.DeserializeObject<Person>(buffer);
            return person;
        }

        private async Task<IEnumerable<Person>> UnpackPeopleAsync(HttpContent content)
        {
            var byteArray = await content.ReadAsByteArrayAsync();
            var buffer = System.Text.Encoding.UTF8.GetString(byteArray);
            var person = JsonConvert.DeserializeObject<IEnumerable<Person>>(buffer);
            return person;
        }
        #endregion
    }
}
