namespace SSE.AspNetCore.ServerSentEvents
{
    using System.Text;

    /// <summary>
    /// Body for a Server Sent Event
    /// </summary>
    /// <remarks>See https://www.w3.org/TR/2009/WD-eventsource-20091029/ </remarks>
    public class ServerSentEvent
    {
        private const string EmptyData = ":";
        private const int NoRetry = -1;
        
        /// <summary>
        /// Represents an empty event - e.g. used to generate a heartbeat event
        /// </summary>
        public static readonly ServerSentEvent Empty = new ServerSentEvent(data: EmptyData);

        /// <summary>
        /// Event ID
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Event Type
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Event Data
        /// </summary>
        public string Data { get; set; }

        /// <summary>
        /// Event Retry Time (in milliseconds)
        /// </summary>
        public int Retry { get; set; }

        /// <summary>
        /// Initialises a new instance of the <see cref="ServerSentEvent"/> class.
        /// </summary>
        public ServerSentEvent() : this(string.Empty, string.Empty, string.Empty, NoRetry)
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="ServerSentEvent"/> class.
        /// </summary>
        /// <param name="id">Event ID</param>
        /// <param name="type">Event Type</param>
        /// <param name="data">Event Data</param>
        /// <param name="retry">Event retry interval (in milliseconds)</param>
        public ServerSentEvent(string id = "", string type = "", string data = "", int retry = NoRetry)
        {
            this.Id = id;
            this.Type = type;
            this.Data = string.IsNullOrWhiteSpace(data) ? EmptyData : data;
            this.Retry = retry;
        }

        /// <summary>
        /// Serialise the event to a properly-formatted event stream body
        /// </summary>
        /// <returns>Event stream content</returns>
        public string ToEventStreamContent()
        {
            var builder = new StringBuilder();

            if (!string.IsNullOrWhiteSpace(this.Id))
            {
                builder.Append($"id: {this.Id}\n");
            }

            if (!string.IsNullOrWhiteSpace(this.Id))
            {
                builder.Append($"type: {this.Type}\n");
            }

            if (this.Retry > 0)
            {
                builder.Append($"retry: {this.Retry}\n");
            }

            builder.Append(this.Data.Equals(EmptyData)
                                ? $"{EmptyData}"
                                : $"data: {this.Data}");
            builder.Append("\n\n");

            return builder.ToString();
        }
    }
}
