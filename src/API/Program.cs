using Application.Common.Behaviors;
using Application.Features.Articles.Commands;
using Application.Interfaces;
using FluentValidation;
using Infrastructure.DomainEventHandlers;
using Infrastructure.Messaging;
using Infrastructure.Persistence;
using Infrastructure.Services;
using Infrastructure.SignalR;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DbContext
//builder.Services.AddDbContext<AppDbContext>(options =>
  //  options.UseInMemoryDatabase("StockDb"));
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
Console.WriteLine($"Connection string: {connectionString}");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddScoped<IApplicationDbContext>(sp => sp.GetRequiredService<AppDbContext>());

// MediatR
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(CreateArticleCommand).Assembly);
    cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
    cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
});

// Validators (scan)
builder.Services.AddValidatorsFromAssembly(typeof(CreateArticleCommand).Assembly);


// AutoMapper
builder.Services.AddAutoMapper(typeof(Application.Mappings.MappingProfile));

// SignalR
builder.Services.AddSignalR();

// Kafka Producer
//builder.Services.AddSingleton<IMessageProducer, KafkaProducer>();

// Ajouter un producteur factice 
builder.Services.AddSingleton<IMessageProducer>(new NullMessageProducer());

// Notification Service
builder.Services.AddScoped<INotificationService, NotificationService>();

// Domain Event Handlers
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(StockUpdatedEventHandler).Assembly));

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AngularApp", policy =>
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials());
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AngularApp");
app.UseAuthorization();
app.MapControllers();
app.MapHub<StockHub>("/stockHub");

app.Run();
