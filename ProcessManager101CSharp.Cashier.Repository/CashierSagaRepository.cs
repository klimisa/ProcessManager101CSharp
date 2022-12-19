namespace ProcessManager101CSharp.Cashier.Repository;

using BullOak.Repositories.EventStore;
using BullOak.Repositories.Repository;
using Process;
using Utile.EventStore.Repository;

public class CashierSagaRepository
    : BaseEventRepository<CashierSagaId, IHoldCashierSagaState>,
        ICashierSagaRepository
{
    protected override string StreamPrefix => "CashierSaga";

    public CashierSagaRepository
    (
        IReadQueryModels<string, IHoldCashierSagaState> readRepository,
        IStartSessions<string, IHoldCashierSagaState> writeRepository
    ) : base(readRepository, writeRepository)
    {
    }

    public async Task<CashierSaga.State> GetSaga(CashierSagaId id)
    {
        var saga = await Read(id);
        return saga is null
            ? new CashierSaga.State.Initial()
            : saga.SagaState;
    }
}