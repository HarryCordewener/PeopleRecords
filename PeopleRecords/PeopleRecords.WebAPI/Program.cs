using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PeopleRecords.WebAPI
{
    public class Program
    {
        private static ServiceProvider provider;

        public static void Main(string[] args)
        {
            var task = Task.Run( ()=> BuildWebHost(args).Run() );

            // Lame, but let's do this for now, for simplicity's sake. We can technically BuildWebHost send a Program Signal out to do this.
            Thread.Sleep(2000);
            Console.Clear();
            Console.ReadLine();
            var console = new ConsoleApp(provider, args);
            console.Run().GetAwaiter().GetResult();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .ConfigureServices(x => provider = x.BuildServiceProvider())
                .Build();
    }
}
