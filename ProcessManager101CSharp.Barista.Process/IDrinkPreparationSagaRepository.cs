namespace ProcessManager101CSharp.Barista.Process;

using Utile.EventStore.Domain;

public interface IDrinkPreparationSagaRepository : IEventRepository<DrinkPreparationSagaId, IHoldDrinkPreparationSagaState>
{
    public Task<DrinkPreparationSaga.State> GetSaga(DrinkPreparationSagaId id);
}