

using MassTransit;
using NotificationService.Consum;
using NotificationService.Interfaces;
using NotificationService.Middleware;
using NotificationService.Models.Email;
using NotificationService.Services;
using Scalar.AspNetCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();


builder.Services.AddScoped<IEmailSenderService, EmailSenderService>();

builder.Host.UseSerilog((context, configuration) =>
{
    configuration.ReadFrom.Configuration(context.Configuration)
        .Enrich.FromLogContext()
        .WriteTo.Console();
});


builder.Services.AddMassTransit(busConfigurator =>
{
    busConfigurator.SetKebabCaseEndpointNameFormatter();

    busConfigurator.AddConsumer<EmailNotificationConsumer>();

    busConfigurator.UsingRabbitMq((context, configurator) =>
    {
        configurator.Host(new Uri(builder.Configuration["MessageBroker:Host"]) ?? throw new InvalidOperationException("RabbitMQ host not configured"), h =>
        {
            h.Username(builder.Configuration["MessageBroker:Username"] ?? throw new InvalidOperationException("RabbitMQ user name not configured"));
            h.Password(builder.Configuration["MessageBroker:Password"] ?? throw new InvalidOperationException("RabbitMQ password not configured"));
        });

        configurator.ConfigureEndpoints(context);
    });
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapScalarApiReference();

app.UseMiddleware<CorrelationIdMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
