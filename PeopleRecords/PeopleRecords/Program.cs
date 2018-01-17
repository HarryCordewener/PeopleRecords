using Microsoft.Extensions.DependencyInjection;
using PeopleRecords.DataAccess;
using PeopleRecords.Interfaces;
using PeopleRecords.Models;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PeopleRecords
{
    public class ConsoleApp
    {
        IPeopleRepository PeopleRepository { get; set; }
        string[] FileLocations { get; }

        public ConsoleApp(ServiceProvider provider, string[] args)
        {
            if (provider == null) throw new ArgumentNullException(nameof(provider));
            var peopleRepo = provider.GetService(typeof(IPeopleRepository));
            if(args.Count() != 3)
            {
                throw new ArgumentOutOfRangeException(nameof(args));
            }

            foreach( string fileLocation in args)
            {
                if (!File.Exists(fileLocation))
                {
                    throw new ArgumentOutOfRangeException(nameof(args), "File does not exist.");
                }
            }

            FileLocations = args;
        }

        public async Task Run()
        {
            try
            {
                foreach(string fileLocation in FileLocations)
                {
                    var fileLines = await File.ReadAllLinesAsync(fileLocation);
                    LineReader.ImportFileIntoRepository(fileLines, PeopleRepository);
                }
            }
            catch
            {
                throw;
            }

            do
            {
                Console.WriteLine("Select an ordering method, or quit: name, birthdate, gender, quit");
                var order = Console.ReadLine();
                if (order == "quit") break;
                if (System.Enum.TryParse(typeof(OrderOption), order, out object result))
                {
                    foreach(Person person in PeopleRepository.ReadPeople((OrderOption)result))
                    {
                        Console.WriteLine(person);
                    }
                    continue;
                }
                else 
                {
                    Console.WriteLine("Invalid entry.");
                    continue;
                }
            } while (true);
        }
    }
}
