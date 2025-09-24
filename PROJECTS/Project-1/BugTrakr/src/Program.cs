using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using BugTrakr.Data;
using BugTrakr.Repositories;
using BugTrakr.Services;
using Serilog;

Env.Load();
var builder = WebApplication.CreateBuilder(args);
var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING") 
    ?? throw new InvalidOperationException("DB_CONNECTION_STRING is not set in environment variables.");

builder.Host.UseSerilog((context, services, configuration) => configuration
    .ReadFrom.Configuration(context.Configuration)
    .ReadFrom.Services(services)
    .Enrich.FromLogContext());

builder.Services.AddControllers(); // register controllers
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<BugTrakrDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

var app = builder.Build();

app.UseSerilogRequestLogging();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers(); // map controller routes

app.Run();
