using MassTransit;
using ProcessManager101CSharp.Cashier.Messaging;
using ProcessManager101CSharp.Cashier.Process;
using ProcessManager101CSharp.Cashier.Repository;
using ProcessManager101CSharp.Cashier.Service;
using Serilog;
using Utile.EventStore;
using Utile.EventStore.BullOak;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .CreateBootstrapLogger();

var builder = WebApplication.CreateBuilder(args);
builder.Host
    .UseSerilog()
    .ConfigureServices((ctx, services) =>
    {
        services.AddMassTransit(x =>
        {
            x.AddConsumer<CashierConsumer>();

            x.UsingRabbitMq((context,cfg) =>
            {
                cfg.Host(new Uri("rabbitmq://localhost:5672/ParcelVision.Retail"), 
                    configurator =>
                    {
                        configurator.Username("pvRetailDev");
                        configurator.Password("pvRetailDev");
                    });
                cfg.ConfigureEndpoints(context);
            });
        });
    });

var configuration = builder.Configuration;
var services = builder.Services;

services
    .AddSingleton(configuration.GetSection("EventStore").Get<EventStoreOptions>())
    .AddEventStore()
    .AddBullOak(typeof(CashierSagaApplier))
    .AddSingleton<ICashierSagaRepository, CashierSagaRepository>();

var app = builder.Build();

app.MapGet("/", () => "Cashier App");

app.Run();