namespace SSE.AspNetCore.ServerSentEvents
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// Extension methods for adding Server Sent Events middleware to ASP.Net Core
    /// </summary>
    public static class ServerSentEventsExtensions
    {
        /// <summary>
        /// Add Server Sent Events required services
        /// </summary>
        /// <param name="services">Service collection</param>
        /// <returns>Same service collection</returns>
        public static IServiceCollection AddServerSentEvents(this IServiceCollection services)
        {
            services.AddSingleton<ServerSentEventsRepository>();
            return services;
        }

        /// <summary>
        /// Use Server Sent Events middleware
        /// </summary>
        /// <param name="app">Application builder</param>
        /// <returns>Same application builder</returns>
        public static IApplicationBuilder UseServerSentEvents(this IApplicationBuilder app)
        {
            app.UseMiddleware<ServerSentEventsMiddleware>();
            return app;
        }
    }
}
