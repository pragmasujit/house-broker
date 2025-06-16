# House Broker

**House Broker** is a modern real estate listing and management application built with clean architecture principles. It allows users to browse, create, update, and manage property listings with rich domain validation and business rules enforcement.

## Features

- Create and manage property listings with detailed attributes (title, price, currency, property type, images, address, etc.)
- Domain-driven design ensuring business rules and validation at the core
- Support for multiple property types and flexible pricing
- API endpoints for listing retrieval, filtering, and CRUD operations
- Clean separation of concerns via layered architecture (Domain, Application, Infrastructure, API)
- Secure and scalable with ASP.NET Core and Entity Framework Core

## Technology Stack

- **Backend:** .NET 9 / ASP.NET Core Web API
- **Architecture:** Clean Architecture, Domain-Driven Design (DDD), CQRS with MediatR
- **Persistence:** Entity Framework Core with SQL Server
- **Validation:** FluentValidation, Custom Domain Validation Exceptions
- **Testing:** nUnit, Moq for unit and integration tests
- **Authentication:** ASP.NET Identity
- **Other:** Specification Pattern, Logging with Microsoft.Extensions.Logging

## Getting Started

### Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/9.0)
- SQL Server or any other supported database
- IDE such as Visual Studio, Visual Studio Code, or JetBrains Rider

### Installation

1. Clone the repository

   ```bash
   git clone https://github.com/pragmasujit/house-broker.git
   cd house-broker/HouseBroker
2. Update appsettings.json DefaultConnection
3. Update Migrations
   ```bash
   dotnet ef database update  --project ../HouseBroker.Infrastructure

4. Ready to go
   ```bash
   dotnet run
5. Navigate to /scalar/v1
6. Data seeds work out of the box. Defaut user with broker permission - user - User@123
