# AutoService Manager API

AutoService Manager API is an ASP.NET Core Web API project for managing automotive service workflows, including customers, vehicles, technicians, service orders, and repair operations.

This project was built as a backend portfolio project focused on real-world API design, SQL Server persistence, service-layer architecture, business validation, audit tracking, pagination, filtering, and maintainable code structure.

## Tech Stack

- ASP.NET Core Web API
- C#
- Entity Framework Core
- SQL Server / LocalDB
- Swagger / OpenAPI
- Visual Studio
- Git / GitHub

## Main Features

- Customer management
- Vehicle management linked to customers
- Technician management
- Service order creation and status handling
- Repair operation management
- Labor amount calculation
- Service order total recalculation
- Audit tracking fields
- Pagination and search filters
- Global exception handling middleware
- Swagger documentation

## Project Structure

```txt
AutoServiceManager.Api
│
├── Common
│   ├── ApiResponse.cs
│   ├── AuditableEntity.cs
│   ├── PagedRequest.cs
│   └── PagedResult.cs
│
├── Controllers
│   ├── CustomersController.cs
│   ├── VehiclesController.cs
│   ├── TechniciansController.cs
│   ├── ServiceOrdersController.cs
│   └── OperationsController.cs
│
├── Data
│   └── AppDbContext.cs
│
├── DTOs
│   ├── Customers
│   ├── Vehicles
│   ├── Technicians
│   ├── ServiceOrders
│   └── Operations
│
├── Entities
│   ├── Customer.cs
│   ├── Vehicle.cs
│   ├── Technician.cs
│   ├── ServiceOrder.cs
│   └── ServiceOrderOperation.cs
│
├── Enums
│   ├── ServiceOrderStatus.cs
│   └── OperationStatus.cs
│
├── Middleware
│   └── ExceptionHandlingMiddleware.cs
│
├── Services
│   ├── Interfaces
│   ├── CustomerService.cs
│   ├── VehicleService.cs
│   ├── TechnicianService.cs
│   ├── ServiceOrderService.cs
│   └── OperationService.cs
│
└── Program.cs
Database

The project uses SQL Server LocalDB by default.

Connection string example:

{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=AutoServiceManagerDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
  }
}
Main Endpoints
Customers
GET    /api/Customers
GET    /api/Customers/{id}
POST   /api/Customers
PUT    /api/Customers/{id}
DELETE /api/Customers/{id}
Vehicles
GET    /api/customers/{customerId}/vehicles
GET    /api/vehicles/{id}
POST   /api/customers/{customerId}/vehicles
PUT    /api/vehicles/{id}
DELETE /api/vehicles/{id}
Technicians
GET    /api/Technicians
GET    /api/Technicians/{id}
POST   /api/Technicians
PUT    /api/Technicians/{id}
PATCH  /api/Technicians/{id}/deactivate
Service Orders
GET    /api/ServiceOrders
GET    /api/ServiceOrders/{id}
POST   /api/ServiceOrders
PUT    /api/ServiceOrders/{id}/status
POST   /api/ServiceOrders/{id}/close
Operations
GET    /api/service-orders/{serviceOrderId}/operations
GET    /api/operations/{id}
POST   /api/service-orders/{serviceOrderId}/operations
PUT    /api/operations/{id}
DELETE /api/operations/{id}
Business Rules
A service order can only be closed when all related operations are completed or cancelled.
Operations cannot be added, updated, or deleted when the related service order is closed.
Labor amount is calculated using labor hours and labor rate.
Service order totals are recalculated when operations are created, updated, or deleted.
Technicians must be active before being assigned to an operation.
Audit fields are automatically updated when records are created or modified.
Pagination and Filtering

Several endpoints support pagination and search filters.

Example:

GET /api/Customers?pageNumber=1&pageSize=10&searchTerm=john

Example for service orders:

GET /api/ServiceOrders?pageNumber=1&pageSize=10&status=2&vehicleId=1&searchTerm=SO-

Example for operations:

GET /api/service-orders/1/operations?pageNumber=1&pageSize=10&status=3&technicianId=1&searchTerm=oil
Sample Requests

Create a customer:

{
  "firstName": "John",
  "lastName": "Carter",
  "email": "john.carter@example.com",
  "phone": "+15550101010"
}

Create a vehicle:

{
  "vin": "1HGCM82633A004352",
  "make": "Honda",
  "model": "Accord",
  "year": 2022,
  "plateNumber": "ABC-1234",
  "unitNumber": "UNIT-001"
}

Create a technician:

{
  "fullName": "Alex Turner",
  "email": "alex.turner@example.com"
}

Create a service order:

{
  "vehicleId": 1
}

Create an operation:

{
  "technicianId": 1,
  "opCode": "LOF",
  "description": "Oil and filter change",
  "laborHours": 1.5,
  "laborRate": 95
}
How to Run Locally
Clone the repository.
git clone https://github.com/lasantosX/AutoServiceManager.Api.git
Open the solution in Visual Studio.
Restore NuGet packages.
dotnet restore
Apply database migrations.
dotnet ef database update
Run the API.
dotnet run
Open Swagger.
https://localhost:{port}/

Swagger is configured as the default page in Development mode.

Future Improvements
JWT authentication and role-based authorization
Seed data for local testing
Unit tests and integration tests
Docker support
CI/CD pipeline with GitHub Actions
Report endpoints for technician performance and revenue summaries
More advanced validation using FluentValidation
Notes

This project is designed as a clean backend API sample that demonstrates practical ASP.NET Core development patterns, relational data modeling, Entity Framework Core usage, business rule validation, pagination, filtering, audit tracking, and API documentation.