namespace ProcessManager101CSharp.Retort;

using Barista.Process;
using Cashier.Process;
using MassTransit;
using Messages;
using Serilog;
using Utile.EventStore.Projections;

public class DrinkPreparationSagaProjection : IProjection
{
    //notes: Abstraction of "Published__" will publish to the bus 

    private readonly IBus bus;

    public DrinkPreparationSagaProjection(IBus bus)
    {
        this.bus = bus;
    }

    public async Task Project(Envelope envelope)
    {
        try
        {
            switch (envelope.EventData)
            {
                case Sent__PrepareDrink m:
                    Log.Information(nameof(Sent__PrepareDrink));
                    await bus.Publish(new PrepareDrink
                    {
                        Name = m.Name,
                        Drink = m.Drink,
                        CorrelationId = m.CorrelationId
                    });
                    break;
                case Published__DrinkReady m:
                    Log.Information(nameof(Published__DrinkReady));
                    await bus.Publish(new DrinkReady
                    {
                        Name = m.Name,
                        Drink = m.Drink,
                        CorrelationId = m.CorrelationId
                    });
                    break;
                case Replied__PaymentDue m:
                    Log.Information(nameof(Replied__PaymentDue));
                    await bus.Publish(new PaymentDue
                    {
                        Amount = m.Message.Amount,
                        CorrelationId = m.CorrelationId
                    });
                    break;
                case Published__PaymentComplete m:
                    Log.Information(nameof(Published__PaymentComplete));
                    await bus.Publish(new PaymentComplete
                    {
                        OrderId = m.Message.OrderId,
                        PaymentType = $"{m.Message.PaymentType}",
                        Amount = m.Message.Amount,
                        CorrelationId = m.CorrelationId
                    });
                    break;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}