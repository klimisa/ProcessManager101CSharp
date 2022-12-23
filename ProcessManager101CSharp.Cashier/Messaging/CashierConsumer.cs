namespace ProcessManager101CSharp.Cashier.Messaging;

using MassTransit;
using Process;
using Service;
using Messages;

public class CashierConsumer :
    IConsumer<NewOrder>,
    IConsumer<SubmitPayment>
{
    private readonly ICashierSagaRepository repository;
    private readonly IBus bus;

    public CashierConsumer(ICashierSagaRepository repository, IBus bus)
    {
        this.repository = repository;
        this.bus = bus;
    }

    public async Task Consume(ConsumeContext<NewOrder> context)
    {
        var correlationId = context.Message.CorrelationId;
        var sagaId = CashierSagaId.From(context.Message.Id);
        var message =
            new CashierSaga.Input.NewOrder(
                context.Message.Id,
                correlationId,
                context.Message.Name,
                context.Message.Size,
                context.Message.Item); // Added to metadata

       // What is my order? Go and pick your strategy?
       // Execute strategy.
       //
        // hide the details of create or update behind an append method
        await CommandHandler.Handle(sagaId, repository, sagaId, correlationId, correlationId, message);
    }

    public async Task Consume(ConsumeContext<SubmitPayment> context)
    {
        var amount = context.Message.Amount;
        Console.WriteLine(amount > 0 ? "Thanks for the tip!" : "What are you, some kind of charity case?");

        var paymentType = Enum.Parse<PaymentType>(context.Message.PaymentType);
        Console.WriteLine($"Received a payment of {paymentType}");

        if (paymentType == PaymentType.CreditCard)
        {
            Console.Write("Authorizing Card...");
            Thread.Sleep(4000);
            Console.WriteLine("done!");
        }

        var correlationId = context.Message.CorrelationId;
        var sagaId = CashierSagaId.From(context.Message.OrderId);
        var message =
            new CashierSaga.Input.PaymentReceived(
                context.Message.OrderId,
                correlationId,
                paymentType,
                context.Message.Amount);

        // hide the details of create or update behind an append method
        await CommandHandler.Handle(sagaId, repository, sagaId, correlationId, correlationId, message);
    }
}
