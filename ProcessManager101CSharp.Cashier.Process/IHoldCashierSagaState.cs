namespace ProcessManager101CSharp.Cashier.Process;

public interface IHoldCashierSagaState
{
    public CashierSagaId CashierSagaSagaId { get; set; }
    public CashierSaga.State SagaState { get; set; }
}