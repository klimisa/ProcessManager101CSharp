namespace ProcessManager101CSharp.Customer;

using MassTransit;
using Messages;

public class CustomerConsumer :
    IConsumer<PaymentDue>,
    IConsumer<DrinkReady>
{
    public async Task Consume(ConsumeContext<PaymentDue> context)
    {
        Status.PaymentDue = true;
        await Task.CompletedTask;
    }
    
    public async Task Consume(ConsumeContext<DrinkReady> context)
    {
        var name = context.Message.Name;
        var drink = context.Message.Drink;
        Console.WriteLine($"Hey, {name}, your {drink} is ready.");
        Status.DrinkReady = true;
        await Task.CompletedTask;
    }
}