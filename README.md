# Identd
A simple server library for communicating over the Identd protocol defined in RFC1413.

The server opens up port 113 and listens for incoming messages to respond to.

Users of this library are expected to manage the lifetime of the server and the number of requests to respond to in their own applications.
