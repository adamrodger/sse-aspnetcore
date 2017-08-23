using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace SSE.AspNetCore
{
    /// <summary>
    /// ASP.Net Core bootstrapper
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Start the application
        /// </summary>
        /// <param name="args">Command line arguments</param>
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        /// <summary>
        /// Build the ASP.net Core web host
        /// </summary>
        /// <param name="args">Command line arguments</param>
        /// <returns>Web host</returns>
        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}
