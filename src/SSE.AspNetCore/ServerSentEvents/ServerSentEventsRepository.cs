namespace SSE.AspNetCore.ServerSentEvents
{
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Repository for <see cref="ServerSentEventsClient"/> instances
    /// </summary>
    public class ServerSentEventsRepository
    {
        private readonly ILoggerFactory loggerFactory;
        private readonly ConcurrentDictionary<string, ServerSentEventsClient> clients;

        /// <summary>
        /// Initialises a new instance of the <see cref="ServerSentEventsRepository"/> class.
        /// </summary>
        public ServerSentEventsRepository(ILoggerFactory loggerFactory)
        {
            this.loggerFactory = loggerFactory;
            this.clients = new ConcurrentDictionary<string, ServerSentEventsClient>();
        }

        /// <summary>
        /// Get all SSE clients
        /// </summary>
        /// <returns>All subscribed clients</returns>
        public IReadOnlyCollection<ServerSentEventsClient> GetAll()
        {
            return this.clients.Values.ToList().AsReadOnly();
        }

        /// <summary>
        /// Got a client by ID
        /// </summary>
        /// <param name="id">Client ID</param>
        /// <returns>Matching client</returns>
        /// <exception cref="KeyNotFoundException">Unknown ID</exception>
        public ServerSentEventsClient GetById(string id)
        {
            return this.clients[id];
        }

        /// <summary>
        /// Create a new client
        /// </summary>
        /// <param name="id">Client ID</param>
        /// <param name="stream">Wrapped stream</param>
        /// <param name="cancellationToken">(Optional) Cancellation token</param>
        /// <returns>New client</returns>
        public ServerSentEventsClient Create(string id, Stream stream, CancellationToken cancellationToken = default(CancellationToken))
        {
            var client = new ServerSentEventsClient(this.loggerFactory.CreateLogger<ServerSentEventsClient>(), id, stream, cancellationToken);
            this.clients[id] = client;
            return client;
        }

        /// <summary>
        /// Remove a subscribed client
        /// </summary>
        /// <param name="client">Client to remove</param>
        public void Remove(ServerSentEventsClient client)
        {
            this.clients.TryRemove(client.Id, out var _);
        }
    }
}
