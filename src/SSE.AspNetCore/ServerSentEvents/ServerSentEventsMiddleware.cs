namespace SSE.AspNetCore.ServerSentEvents
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Middleware to handling incoming Server Sent Event subscriptions
    /// </summary>
    public class ServerSentEventsMiddleware
    {
        private const string SseContentType = "text/event-stream";

        private readonly RequestDelegate next;
        private readonly ILogger<ServerSentEventsMiddleware> log;
        private readonly ServerSentEventsRepository repository;

        /// <summary>
        /// Initialises a new instance of the <see cref="ServerSentEventsMiddleware"/> class.
        /// </summary>
        /// <param name="next">Next handler in the ASP.Net Core request pipeline. Invoked if the incoming request is not an SSE subscription</param>
        /// <param name="log">Logger</param>
        /// <param name="repository">SSE clients repository</param>
        public ServerSentEventsMiddleware(RequestDelegate next, ILogger<ServerSentEventsMiddleware> log, ServerSentEventsRepository repository)
        {
            this.next = next;
            this.log = log;
            this.repository = repository;
        }

        /// <summary>
        /// Invoke the middleware
        /// </summary>
        /// <param name="context">Request/response context</param>
        /// <returns>Async/await task</returns>
        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Headers["Accept"].Equals(SseContentType))
            {
                context.Response.ContentType = SseContentType;

                // register the client for future events
                string id = context.Request.Query.ContainsKey("id") ? context.Request.Query["id"].ToString() : Guid.NewGuid().ToString();
                this.log.LogInformation($"Subscribing new SSE client with id {id}");
                CancellationToken cancellationToken = context.RequestAborted;
                var client = this.repository.Create(id, context.Response.Body, cancellationToken);

                // Establish the connection
                await client.SendEvent(ServerSentEvent.Empty).ConfigureAwait(false);
                this.log.LogInformation($"SSE connection successfully established to client {id}");

                // wait for the browser to disconnect
                cancellationToken.WaitHandle.WaitOne();

                // TODO: Is it possible to stop the request returning without blocking the current thread like this...?
                /*
                var tcs = new TaskCompletionSource<bool>();
                cancellationToken.Register(s => ((TaskCompletionSource<bool>)s).SetResult(true), tcs);
                await tcs.Task;
                */

                this.log.LogInformation($"Unsubscribing client {id}");
                this.repository.Remove(client);
                return;
            }

            await this.next(context);
        }
    }
}
