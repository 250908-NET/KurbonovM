using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using BugTrakr.Data;
using BugTrakr.Repositories;
using BugTrakr.Services;
using BugTrakr.Models;
using Serilog;

Env.Load();
var builder = WebApplication.CreateBuilder(args);
var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING") 
    ?? throw new InvalidOperationException("DB_CONNECTION_STRING is not set in environment variables.");

builder.Host.UseSerilog((context, services, configuration) => configuration
    .ReadFrom.Configuration(context.Configuration)
    .ReadFrom.Services(services)
    .Enrich.FromLogContext());

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<BugTrakrDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

var app = builder.Build();

app.UseSerilogRequestLogging();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/", () => "Welcome to BugTrakr API!");

app.MapGet("/users", async (IUserService userService) =>
{
    var users = await userService.GetAllUsersAsync();
    return Results.Ok(users);
});
app.MapPost("/users", async (User user, IUserService userService) =>
{
    await userService.AddUserAsync(user);
    return Results.Created($"/users/{user.UserID}", user);
});
app.MapGet("/users/{id}", async (int id, IUserService userService) =>
{
    var user = await userService.GetUserByIdAsync(id);
    return user is not null ? Results.Ok(user) : Results.NotFound();
});

app.Run();
