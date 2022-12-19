using FluentMigrator.Runner;
using MassTransit;
using OpenTelemetry.Trace;
using ProcessManager101CSharp.Retort;
using ProcessManager101CSharp.Infrastructure.Postgres;
using ProcessManager101CSharp.Barista.Process;
using ProcessManager101CSharp.Cashier.Process;
using Serilog;
using Utile.EventStore;
using Utile.EventStore.Projections;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .CreateBootstrapLogger();

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
var services = builder.Services;

var postgresOptions = configuration.GetSection("Postgres").Get<PostgresOptions>();
var PostgresMigratorOptions = configuration.GetSection("PostgresMigrator").Get<PostgresOptions>();
var postgresOptionsList = new List<PostgresOptions> { postgresOptions, PostgresMigratorOptions };

builder.Host
    .UseSerilog()
    .ConfigureServices((ctx, services) =>
{
    services.AddMassTransit(x =>
    {
        x.UsingRabbitMq((context, cfg) =>
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


services
    .AddFluentMigratorCore()
    .ConfigureRunner(rb => rb
        .AddPostgres()
        .WithGlobalConnectionString(postgresOptions.GetConnectionString())
        .ScanIn(typeof(Migrations.InitialSchema).Assembly)
        .For.Migrations());

services.AddSingleton<IConnectionFactory>(_ =>
    new PostgreSqlConnectionFactory(postgresOptions.GetConnectionString()));

services
    .AddSingleton(postgresOptionsList)
    .AddHostedService<DatabaseMigrator>();

builder.Services
    .AddSingleton(TracerProvider.Default.GetTracer("FUCK OFF SIMON"))
    .AddSingleton(configuration.GetSection("EventStore").Get<EventStoreOptions>())
    .AddSingleton<ICheckpointStore, PostgreSqlCheckpointStore>()
    .AddEventStore()
    .AddProjections((p, sp) =>
    {
        TypeMapper.ScanIn(
            typeof(DrinkPreparationSaga).Assembly,
            typeof(CashierSaga).Assembly);

        p.Add("DrinkPreparationSaga",
            new List<IProjection>
            {
                new DrinkPreparationSagaProjection(
                    sp.GetRequiredService<IBus>()
                    )
            });
    });

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();