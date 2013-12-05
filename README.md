EventSourcing101
================

Actual running code* of an event sourced domain model (*as compared to the snippets fed by most ES pundits)
## Features demonstrated

* DDD appropriate repository implementation
* Append / Apply pattern within the domain model
* Application of facts to child entities of an aggregate
* Read models
* Domain event handlers participating in a unit of work
* Some basic approval tests to guide the shape of the domain

* * * *
## Features not demonstrated

* Fact storage (simple to wire up with NEventStore + RavenDb)
* Merging fact streams
* Resolution of concurrency issues
