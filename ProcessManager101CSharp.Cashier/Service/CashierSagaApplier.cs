namespace ProcessManager101CSharp.Cashier.Service;

using BullOak.Repositories.Appliers;
using Infrastructure.ProcessManager;
using Process;

public class CashierSagaApplier:
    IApplyEvent<IHoldCashierSagaState, Received__NewOrder>,
    IApplyEvent<IHoldCashierSagaState, Received__PaymentReceived>,
    IApplyEvent<IHoldCashierSagaState, Replied__PaymentDue>,
    IApplyEvent<IHoldCashierSagaState, Published__PaymentComplete>,
    IApplyEvent<IHoldCashierSagaState, Completed>
{
    public IHoldCashierSagaState Apply(IHoldCashierSagaState state, Received__NewOrder @event)
    {
        state.CashierSagaSagaId = CashierSagaId.From(@event.Id);
        state.SagaState = CashierSaga.Evolve(
            new CashierSaga.State.Initial(),
            new Event.Received<CashierSaga.Input>(
                new CashierSaga.Input.NewOrder(
                    @event.Id,
                    @event.CorrelationId,
                    @event.Message.Name,
                    @event.Message.Size,
                    @event.Message.Item
                )
            )
        );
        return state;
    }

    public IHoldCashierSagaState Apply(IHoldCashierSagaState state, Received__PaymentReceived @event)
    {
        state.SagaState = CashierSaga.Evolve(
            new CashierSaga.State.Initial(),
            new Event.Received<CashierSaga.Input>(
                new CashierSaga.Input.PaymentReceived(
                    @event.Id,
                    @event.CorrelationId,
                    @event.Message.PaymentType,
                    @event.Message.Amount
                )
            )
        );
        return state;
    }

    public IHoldCashierSagaState Apply(IHoldCashierSagaState state, Replied__PaymentDue @event)
    {
        return state;
    }

    public IHoldCashierSagaState Apply(IHoldCashierSagaState state, Published__PaymentComplete @event)
    {
        return state;
    }

    public IHoldCashierSagaState Apply(IHoldCashierSagaState state, Completed @event)
    {
        return state;
    }
}