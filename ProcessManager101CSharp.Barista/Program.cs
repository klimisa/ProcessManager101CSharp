using MassTransit;
using ProcessManager101CSharp.Barista.Messaging;
using ProcessManager101CSharp.Barista.Process;
using ProcessManager101CSharp.Barista.Repository;
using ProcessManager101CSharp.Barista.Service;
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
      x.AddConsumer<BaristaConsumer>();

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
   .AddBullOak(typeof(DrinkPreparationSagaApplier))
   .AddSingleton<IDrinkPreparationSagaRepository, DrinkPreparationSagaRepository>();

var app = builder.Build();

app.MapGet("/", () => "Barista App");

app.Run();