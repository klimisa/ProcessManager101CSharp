// See https://aka.ms/new-console-template for more information

using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProcessManager101CSharp.Customer;
using ProcessManager101CSharp.Messages;
using Serilog;

var host = AppStartup();

var bus = host.Services.GetRequiredService<IBusControl>();

await bus.StartAsync();

Status.IsNewOrder = true;
Guid id = default;
do
{
    var value = String.Empty;
    if (Status.IsNewOrder)
    {
        Console.WriteLine("Press 'Enter' to create a new order (or quit to exit)");
        Console.Write("> ");
        value = Console.ReadLine();
        id = Guid.NewGuid();
        var order = new NewOrder
        {
            Id = id,
            CorrelationId = Guid.NewGuid(),
            Name = "Sith Lord",
            Size = "Grande",
            Item = "Pumpkin Cream Cold Brew"
        };
        await bus.Publish(order);
        Status.IsNewOrder = false;
    }

    if (Status.PaymentDue)
    {
        Console.WriteLine("Payment due: Would you like to add a tip? [Y/n]");
        Console.Write("> ");
        value = Console.ReadLine();

        if (string.IsNullOrEmpty(value))
        {
            var submitPayment = new SubmitPayment
            {
                OrderId = id,
                CorrelationId = Guid.NewGuid(),
                PaymentType = "CreditCard",
                Amount = 0.2m,
            };

            await bus.Publish(submitPayment);
            Status.PaymentDue = false;
        }
    }

    if (Status.DrinkReady)
    {
        Console.WriteLine("Would you like to have something else? [quit/retry]");
        Console.Write("> ");
        value = Console.ReadLine();
        Status.DrinkReady = false;
    }

    if ("quit".Equals(value, StringComparison.OrdinalIgnoreCase))
        break;

    if ("retry".Equals(value, StringComparison.OrdinalIgnoreCase))
        Status.IsNewOrder = true;

    // if (Status.DrinkReady == false && Status.IsNewOrder == false && Status.PaymentDue == false)
    // {
    //     Console.WriteLine("Thank you!!!");
    //     break;
    // }
} while (true);

await bus.StopAsync();

static void BuildConfig(IConfigurationBuilder builder)
{
    // Check the current directory that the application is running on 
    // Then once the file 'appsetting.json' is found, we are adding it.
    // We add env variables, which can override the configs in appsettings.json
    builder.SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .AddEnvironmentVariables();
}

static IHost AppStartup()
{
    var builder = new ConfigurationBuilder();
    BuildConfig(builder);

    // Specifying the configuration for serilog
    Log.Logger = new LoggerConfiguration()
        .MinimumLevel.Information()
        .WriteTo.Console()
        .CreateBootstrapLogger();

    Log.Logger.Information("Application Starting");

    var host = Host.CreateDefaultBuilder() // Initialising the Host 
        .UseSerilog()
        .ConfigureServices((ctx, services) =>
        {
            services.AddMassTransit(x =>
            {
                x.AddConsumer<CustomerConsumer>();

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
        })
        .Build(); // Build the Host

    return host;
}