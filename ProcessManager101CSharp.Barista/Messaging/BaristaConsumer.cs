namespace ProcessManager101CSharp.Barista.Messaging;

using MassTransit;
using Process;
using Messages;
using Service;

public class BaristaConsumer :
    IConsumer<NewOrder>,
    IConsumer<PrepareDrink>,
    IConsumer<PaymentComplete>
{
    private IDrinkPreparationSagaRepository repository;

    public BaristaConsumer(IDrinkPreparationSagaRepository repository)
    {
        this.repository = repository;
    }

    public async Task Consume(ConsumeContext<NewOrder> context)
    {
        var correlationId = context.Message.CorrelationId;
        var sagaId = DrinkPreparationSagaId.From(context.Message.Id);
        var name = context.Message.Name;
        var size = context.Message.Size;
        var item = context.Message.Item;
      
        var message = new DrinkPreparationSaga.Input.NewOrder(correlationId, name, size, item); // Added to metadata
        await CommandHandler.Handle(sagaId, repository, sagaId, correlationId, correlationId, message);  // hide the details of create or update behind an append method
    }



    public Task Consume(ConsumeContext<PrepareDrink> context)
    {
        var name = context.Message.Name;
        var drink = context.Message.Drink;
        
        Console.WriteLine($"{drink} for {name}, got it!");
        
        for (int i = 0; i < 10; i++)
        {
            Thread.Sleep(i * 200);
            Console.WriteLine("[wwhhrrrr....psssss...chrhrhrhrrr]");
        }
        
        Console.WriteLine($"I've got a {drink} ready for {name}!");
        return Task.CompletedTask;
    }

    public async Task Consume(ConsumeContext<PaymentComplete> context)
    {
        var correlationId = Guid.NewGuid();
        var causationId = Guid.NewGuid();

        var sagaId = DrinkPreparationSagaId.From(context.Message.OrderId);
        var message = new DrinkPreparationSaga.Input.PaymentComplete(correlationId); // Added to metadata

        // hide the details of create or update behind an append method
        await CommandHandler.Handle(sagaId, repository, sagaId, correlationId, causationId, message);
    }
}