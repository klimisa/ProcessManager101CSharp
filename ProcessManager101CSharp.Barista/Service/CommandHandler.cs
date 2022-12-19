namespace ProcessManager101CSharp.Barista.Service;

using Process;
using Utile.Messages;

public static class CommandHandler
{
    public static async Task Handle(
        DrinkPreparationSagaId sagaId,
        IDrinkPreparationSagaRepository repository,
        DrinkPreparationSagaId drinkPreparationSagaId,
        Guid correlationId,
        Guid causationId,
        DrinkPreparationSaga.Input message)
    {
        var state = await repository.GetSaga(sagaId); // expected version
        switch (state, message)
        {
            case (DrinkPreparationSaga.State.Initial s, DrinkPreparationSaga.Input.NewOrder m):
                await repository.Create(drinkPreparationSagaId,
                    (_, _) => Events(s, m, correlationId, causationId));
                break;
            case (DrinkPreparationSaga.State.PreparingDrink s, DrinkPreparationSaga.Input.PaymentComplete m):
               
                await repository.Update(drinkPreparationSagaId,
                    _ => Events(s, m, correlationId, causationId));
                break;
        }
    }

    private static IEnumerable<IEvent> Events(
        DrinkPreparationSaga.State state, DrinkPreparationSaga.Input message,
        Guid correlationId, Guid causationId)
    {
        var commands = DrinkPreparationSaga.Handle(state, message);
        var commandsToStore = commands.ToEvents(correlationId, causationId);

        var messageToStore =
            message
                .ToEvent(correlationId, causationId);

        var events = new List<IEvent> { messageToStore };

        events.AddRange(commandsToStore);
        return events;
    }
}