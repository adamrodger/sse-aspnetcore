namespace SSE.AspNetCore.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using ServerSentEvents;

    /// <summary>
    /// Controller for managing events
    /// </summary>
    [Route("api/[controller]")]
    public class EventsController : Controller
    {
        private readonly ServerSentEventsRepository repository;

        /// <summary>
        /// Initialises a new instance of the <see cref="EventsController"/> class.
        /// </summary>
        /// <param name="repository">SSE client repository</param>
        public EventsController(ServerSentEventsRepository repository)
        {
            this.repository = repository;
        }

        /// <summary>
        /// Publish a new event to all subscribed clients
        /// </summary>
        /// <param name="sse">Event definition</param>
        /// <returns>Action result</returns>
        [HttpPost]
        public async Task<IActionResult> PostEvent(ServerSentEvent sse)
        {
            var clients = this.repository.GetAll();

            if (clients.Any())
            {
                var tasks = clients.Select(c => c.SendEvent(sse));
                await Task.WhenAll(tasks);
            }

            return this.Ok();
        }
    }
}
