namespace ProcessManager101CSharp.Cashier.Process;

using Utile.EventStore.Domain;

public interface ICashierSagaRepository: IEventRepository<CashierSagaId, IHoldCashierSagaState>
{
    public Task<CashierSaga.State> GetSaga(CashierSagaId id);
}