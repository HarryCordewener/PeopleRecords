using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PeopleRecords.DataAccess;
using PeopleRecords.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PeopleRecords.WebAPI
{
    public class Program
    {
        private static ServiceProvider Provider;

        public static void Main(string[] args)
        {
            var task = Task.Run( ()=> BuildWebHost(new string[] { }).Run() );

            // Lame, but let's do this for now, for simplicity's sake. We can technically BuildWebHost send a Program Signal out to do this.
            Thread.Sleep(2000);
            Console.Clear();
            var console = new ConsoleApp(Provider.GetRequiredService<IPeopleRepository>(), args);
            console.Run().GetAwaiter().GetResult();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureServices(ConfigureMyServices)
                .UseStartup<Startup>()
                .Build();

        public static void ConfigureMyServices(IServiceCollection services)
        {
            services.AddMvc().AddJsonOptions(options =>
            {
                options.SerializerSettings.Formatting = Formatting.Indented;
            });
            services.AddLogging();

            // We need to take this approach, otherwise the console app and the webapi endpoint somehow end up with two different instances, 
            // even though we indicated it should add a Singleton, and thus they should both be using the same bloody thing.
            // This may have to do with going across App Domain boundaries / threads, the way we are launching things above.
            ILogger<InMemoryPeopleRepository> logger = services.BuildServiceProvider().GetRequiredService<ILogger<InMemoryPeopleRepository>>();
            var peopleRepo = new InMemoryPeopleRepository(logger);
            services.AddSingleton(typeof(IPeopleRepository), peopleRepo);
            Provider = services.BuildServiceProvider();
        }
    }
}
