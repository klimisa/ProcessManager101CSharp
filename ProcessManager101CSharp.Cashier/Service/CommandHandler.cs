namespace ProcessManager101CSharp.Cashier.Service;

using Process;
using Utile.Messages;

public static class CommandHandler
{
    public static async Task Handle(
        CashierSagaId sagaId,
        ICashierSagaRepository repository,
        CashierSagaId cashierSagaId,
        Guid correlationId,
        Guid causationId,
        CashierSaga.Input message)
    {
        var state = await repository.GetSaga(sagaId); // expected version
        switch (state, message)
        {
            case (CashierSaga.State.Initial s, CashierSaga.Input.NewOrder m):
                await repository.Create(cashierSagaId,
                    (_, _) => Events(s, m, correlationId, causationId));
                break;
            case (CashierSaga.State.WaitingForPayment s, CashierSaga.Input.PaymentReceived m):
                await repository.Update(cashierSagaId,
                    _ => Events(s, m, correlationId, causationId));
                break;
        }
    }

    private static IEnumerable<IEvent> Events(
        CashierSaga.State state, CashierSaga.Input message,
        Guid correlationId, Guid causationId)
    {
        var commands = CashierSaga.Handle(state, message);
        var commandsToStore = commands.ToEvents(correlationId, causationId);

        var messageToStore =
            message
                .ToEvent(correlationId, causationId);

        var events = new List<IEvent> { messageToStore };

        events.AddRange(commandsToStore);
        return events;
    }
}