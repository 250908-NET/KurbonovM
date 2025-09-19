using System;
using Serilog;


namespace ToDoApp
{
    class Program
    {
        static void Main(string[] args)
        {
            // Serilog - popular logging library for .NET
            // dotnet add <project.csproj> package <package_name>
            // dotnet add ToDoApp.csproj package Serilog.AspNetCore

            // logging sinks - where the logs go
            // console, file, database, remote server, etc.

            var builder = WebApplication.CreateBuilder(args); // Create a builder for the web application

            // configure logging before we "build" the app
            // dotnet add package Serilog.sink.console
            Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateLogger();
            builder.Host.UseSerilog(); // Use Serilog for logging


            var app = builder.Build(); // Build the web application
            app.MapGet("/", (ILogger<Program> logger) =>
            {
                logger.LogInformation("Hello World endpoint was called"); // Log an information message
                return "Hello, World!"; // Return a simple response 
            });


            // HTTP requests - response have body, and headers
            // HEAD  request is like GET but it does not have body
            // HEAD, OPTIONS, DELETE requests do not have body

            app.Run(); // Run the web application


            // LOGGING - record the function/activity/behaviors/events of an application
            // events - requests, responses, system/application status, errors, wornings, crashes/shutdown, startup
            // levels of logging: 
            // Trace - most detailed level, used for diagnosing issues - everything that happens!
            // Debug - less detailed than Trace, used for debugging purposes
            // Information - general information about the application's operation
            // ----- this is the last "everything is ok" level -----
            // Warning - indicates a potential issue or unexpected behavior
            // Error - indicates a significant issue that affects the application's functionality
            // Critical - indicates a severe issue that requires immediate attention

        }
    }
}
