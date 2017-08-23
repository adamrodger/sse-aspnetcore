namespace SSE.AspNetCore.ServerSentEvents
{
    using System.IO;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// A client subscribed to receive SSEs
    /// </summary>
    public class ServerSentEventsClient
    {
        private readonly ILogger<ServerSentEventsClient> log;
        private readonly Stream stream;
        private readonly CancellationToken cancellationToken;

        /// <summary>
        /// Client ID
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// Initialises a new instance of the <see cref="ServerSentEventsClient"/> class.
        /// </summary>
        /// <param name="log">Logger</param>
        /// <param name="id">Client ID</param>
        /// <param name="stream">Wrapped stream on which to publish messages</param>
        /// <param name="cancellationToken">Cancellation token</param>
        public ServerSentEventsClient(ILogger<ServerSentEventsClient> log, string id, Stream stream, CancellationToken cancellationToken = default(CancellationToken))
        {
            this.log = log;
            this.Id = id;
            this.stream = stream;
            this.cancellationToken = cancellationToken;
        }

        /// <summary>
        /// Send an event to the client
        /// </summary>
        /// <param name="sse">Server Sent Event</param>
        /// <returns>Async/await task</returns>
        public async Task SendEvent(ServerSentEvent sse)
        {
            if (this.cancellationToken.IsCancellationRequested)
            {
                return;
            }

            string eventString = sse.ToEventStreamContent();
            this.log.LogDebug($"Sending event to client {this.Id}: {eventString}");

            byte[] bytes = Encoding.UTF8.GetBytes(eventString);
            await this.stream.WriteAsync(bytes, 0, bytes.Length, this.cancellationToken).ConfigureAwait(false);
            this.log.LogDebug($"Successfully sent event to client {this.Id}");
        }
    }
}
