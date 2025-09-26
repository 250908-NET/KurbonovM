# Bug Tracker API
A bug tracking app built with ASP.NET Core and Entity Framework Core. 

## Key Features
- Project Management: Create, view, update, and delete projects.

- Bug/Ticket Tracking: Log new bugs or features (tickets) and assign them to specific projects.

- User Assignment: Assign tickets to a specific user (developer) for resolution.

- Relational Data: Utilizes a many-to-many relationship (via a join table) to link Users to Projects (e.g., a user can work on multiple projects, and a project can have multiple users).

## Technologies Used
- ASP.NET Core - web api framework for building the backend.
- Entity Framework Core - an object-relational mapper(ORM) for data access.
- SQL Server - database engine for persistent storage.
- Docker - used to run the SQL Server database in a container for easy setup.
- Swashbuckle.AspNetCore - generates OpenAPI/Swagger documentation for the API.
- Microsoft.AspNetCore.Authentication.JwtBearer - handles JWT token-based authentication.
- DotNetEnv - Loads configuration variables from a .env file.
- Serilog - structured logging to console and files.
- xunit - testing framework.
- Moq & FluentAssertions - mocking dependencies and fluent syntax for writing tests.
- Microsoft.EntityFrameworkCore.InMemory - used for fast, isolated database testing.

## Project Structure
```
PROJECT-1/
├── ERD/                 # Contains the Entity Relationship Diagram (ERD) image/files.
├── src/                 # Main source code for the Bug Tracker API.
│   ├── .config/         
│   ├── Controllers/     # API endpoint classes (handles HTTP requests).
│   ├── DTO/             # Data Transfer Objects (used for request/response bodies).
│   ├── Exceptions/      # Custom exception classes.
│   ├── Logs/            # Runtime log files (configured by Serilog).
│   ├── Migrations/      # EF Core database migration scripts.
│   ├── Models/          # Entity classes (defines the database schema).
│   ├── Repositories/    # Data access logic (implements the Repository Pattern).
│   ├── Services/        # Business logic and domain services.
│   ├── .env             # Environment variables (used by DotNetEnv).
│   ├── .env.example     # Template for .env file.
│   ├── appsettings.json # Primary configuration file.
│   └── Program.cs       # Application entry point and service configuration.
├── tests/               # Unit and Integration test projects.
│   ├── BugTrakr.Tests/  # Test project for the API.
│   │   ├── Controllers/ # Tests for controller logic.
│   │   ├── Data/        # Mocks/tests for database context/data.
│   │   ├── Repositories/# Tests for repository methods.
│   │   └── Services/    # Tests for business logic.
│   └── BugTrakr.Tests.csproj
├── .gitignore           # Specifies intentionally untracked files to ignore.
├── BugTrakr.sln         # Visual Studio Solution file.
├── Commands.md          # Reference file for common CLI commands.
├── Requirements.md      # Detailed system and functional requirements.
├── UserStories.md       # Defines features from a user's perspective.
└── README.md            # This document.          
```

## Prerequisites
- .NET SDK 9.0
- Docker Desktop
- Visual Studio Code
- DBeaver and VSCode mssql extension

## Setup 
The setup assumes you are using Docker for the local SQL Server instance.

1. Run the database.
Launch a local SQL Server container using Docker. This command sets up the database named BugTrackerDB.
```
docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=YourStrongPassword123" \
   -p 1433:1433 --name sqlserver_instance -d mcr.microsoft.com/mssql/server:2022-latest
```
- Note: Replace YourStrongPassword123 with your preferred strong password and update the API configuration accordingly.

- The port 1433 must be available on your machine.


2. Configure the API.

Update the connection string in your .env file(sample given in .env.example), to match your Docker setup.
  

3. Apply database migrations.

First, ensure the EF Core tool is installed locally, then apply the migrations to create the database schema:

```
# Install EF Core CLI tool (if not already local)
dotnet tool install --local dotnet-ef 

# Create the initial migration file (if not already done)
dotnet ef migrations add Initial 

# Apply the migrations to the database
dotnet ef database update
```
   
4. Run the application.

Execute the following command to start the ASP.NET Core API:

```
dotnet run
```
In your terminal, you can click the url(such as `http://localhost:5000`) and test endpoints.  Swagger documentation will be available at `/swagger`

## Usage

```
Resource	  HTTP   	   Endpoint	               Description

Projects	   POST	   /api/projects	        Create a new project.
Projects	   GET	       /api/projects	        Get a list of all projects.
Tickets	    POST	   /api/tickets	        Log a new bug or feature.
Tickets	    GET	    /api/tickets/{id}	Get details for a specific ticket.
Tickets	    PUT	    /api/tickets/{id}	Update a ticket's status (e.g., to 'Closed').
...
```

## Running Tests
To ensure everything is working correctly, run the unit and integration tests:
```
dotnet test
```