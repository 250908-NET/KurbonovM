# Used commands

## create a new project
- dotnet new webapi -n BugTrakr

## add packages to the project
- dotnet add package Microsoft.EntityFrameworkCore.Tools
- dotnet add package Microsoft.EntityFrameworkCore.Design
- dotnet add package Microsoft.EntityFrameworkCore.SqlServer
- dotnet add package Microsoft.AspNetCore.Mvc.Testing
- dotnet add package xunit
- dotnet add package Moq

## tool install for dotnet
- dotnet add package Swashbuckle.AspNetCore
- dotnet new tool-manifest
- dotnet tool install --local dotnet -ef
- dotnet add package Serilog.AspNetCore
- dotnet add package Serilog.Sinks.Console
- dotnet add package Serilog.Sinks.File
- dotnet add package DotNetEnv

## mssql in docker
- docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=**********" -p 1433:1433 --name sqlserver_instance -d mcr.microsoft.com/mssql/server:2022-latest

## database migrations
dotnet ef migrations add Initial
dotnet ef database update


## set up tests
- (cd BugTrakr)
dotnet new xunit -n BugTrakr.Tests
- (cd BugTrakr.Tests)
dotnet add reference ../src/BugTrakr.csproj

## run tests
- dotnet test