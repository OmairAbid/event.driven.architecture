# Implementation of DDD, CQRS and Event Sourcing

This project leverages event-driven architecture to improve maintainability in systems with complex business domains. It is built using ASP.NET Core and applies Domain-Driven Design, along with the CQRS and Event Sourcing patterns. A fictional business case, shaped by an event storming workshop, serves as the foundation for the project. The focus is on using well-established principles and frameworks to handle intricate business logic effectively.


## Features

- ASP.NET Core 5.0 Web API application
- Clean Architecture implementation with applying SOLID principles
- Domain-driven Design (DDD)
- xUnit-based Test-driven Design (TDD)
- Hexagonal architecture a.k.a. Ports & Adapters (Domain, Application and Framework Layers)
- CQRS implementation on Commands, Queries and Projections
- MediatR implementation (Request- and Notification-Handling, Pipeline-Behaviour for Logging, Metrics and Authentication)
- Swagger Open API endpoint
- Dockerfile and Docker Compose (YAML) file for environmental setup
- EventStoreDB Repository and custom Client for storing of events
- MongoDB Repository for performance-optimized querying
- Authentication Service based on ASP.NET Core Idenity
- Email Service for account verification
- Shared Kernel class library implementation for DDD & Event Sourcing