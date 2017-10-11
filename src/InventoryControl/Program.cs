using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;

namespace InventoryControl
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureLogging(configuration =>
                {
                    configuration.AddConsole();
                    configuration.AddDebug();
                })
                .UseStartup<Startup>()
                .Build();
    }
}
