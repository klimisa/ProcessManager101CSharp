namespace ProcessManager101CSharp.Barista.Repository;

using BullOak.Repositories.EventStore;
using BullOak.Repositories.Repository;
using Process;
using Utile.EventStore.Repository;

public class DrinkPreparationSagaRepository
    : BaseEventRepository<DrinkPreparationSagaId, IHoldDrinkPreparationSagaState>,
        IDrinkPreparationSagaRepository
{
    protected override string StreamPrefix => "DrinkPreparationSaga";

    public DrinkPreparationSagaRepository
    (
        IReadQueryModels<string, IHoldDrinkPreparationSagaState> readRepository,
        IStartSessions<string, IHoldDrinkPreparationSagaState> writeRepository
    ) : base(readRepository, writeRepository)
    {
    }

    public async Task<DrinkPreparationSaga.State> GetSaga(DrinkPreparationSagaId id)
    {
        var saga = await Read(id);
        return saga is null
            ? new DrinkPreparationSaga.State.Initial()
            : saga.SagaState;
    }
}