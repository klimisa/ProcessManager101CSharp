namespace ProcessManager101CSharp.Barista.Service;

using BullOak.Repositories.Appliers;
using Process;
using Infrastructure.ProcessManager;

public class DrinkPreparationSagaApplier :
    IApplyEvent<IHoldDrinkPreparationSagaState, Received__NewOrder>,
    IApplyEvent<IHoldDrinkPreparationSagaState, Received__PaymentComplete>,
    IApplyEvent<IHoldDrinkPreparationSagaState, Sent__PrepareDrink>,
    IApplyEvent<IHoldDrinkPreparationSagaState, Published__DrinkReady>,
    IApplyEvent<IHoldDrinkPreparationSagaState, Completed>
{
    public IHoldDrinkPreparationSagaState Apply(IHoldDrinkPreparationSagaState state, Received__NewOrder @event)
    {
        state.SagaState = DrinkPreparationSaga.Evolve(
            new DrinkPreparationSaga.State.Initial(),
            new Event.Received<DrinkPreparationSaga.Input>(
                new DrinkPreparationSaga.Input.NewOrder(
                    @event.CorrelationId,
                    @event.Message.Name,
                    @event.Message.Size,
                    @event.Message.Item
                )
            )
        );
        return state;
    }

    public IHoldDrinkPreparationSagaState Apply(IHoldDrinkPreparationSagaState state, Received__PaymentComplete @event)
    {
        state.SagaState = DrinkPreparationSaga.Evolve(
            state.SagaState,
            new Event.Received<DrinkPreparationSaga.Input>(
                new DrinkPreparationSaga.Input.PaymentComplete(
                    @event.CorrelationId
                )
            )
        );
        return state;
    }

    public IHoldDrinkPreparationSagaState Apply(IHoldDrinkPreparationSagaState state, Published__DrinkReady @event)
    {
        return state;
    }

    public IHoldDrinkPreparationSagaState Apply(IHoldDrinkPreparationSagaState state, Completed @event)
    {
        return state;
    }

    public IHoldDrinkPreparationSagaState Apply(IHoldDrinkPreparationSagaState state, Sent__PrepareDrink @event)
    {
        return state;
    }
}