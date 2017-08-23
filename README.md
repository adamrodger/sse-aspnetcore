Server Sent Events in ASP.Net Core Example
==========================================

This example shows how to use Server Sent Events (SSE) with ASP.Net Core to push events from
the server back to a browser asynchronously.

Start the service with:

```bash
cd src/SSE.AspNetCore
dotnet run
```

Then navigate to `http://localhost:5000/` from a browser to see the SSEs being received and
actioned.

You can post a new event which should be broadcast to all clients using:

```bash
curl -H "Content-Type: application/json" -X POST -d '{"Data":"test message"}' http://localhost:5000/api/events
```

## TODO

- Empty heartbeat message every 15s to keep connections alive
- Stop the request thread being blocked on new subscriptions
- Better logging
- Unit tests
