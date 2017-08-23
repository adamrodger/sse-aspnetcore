using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SSE.AspNetCore
{
    using ServerSentEvents;

    /// <summary>
    /// ASP.Net Core startup class
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Application configuration
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// Initialises a new instance of the <see cref="Startup"/> class.
        /// </summary>
        /// <param name="configuration">Application configuration</param>
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        /// <summary>
        /// Add services to the service container
        /// </summary>
        /// <param name="services">Service collection</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddServerSentEvents()
                    .AddMvc();
        }

        /// <summary>
        /// Configure the HTTP request pipeline
        /// </summary>
        /// <param name="app">Application builder</param>
        /// <param name="env">Hosting environment</param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles()
               .UseDefaultFiles()
               .UseServerSentEvents()
               .UseMvc();
        }
    }
}
