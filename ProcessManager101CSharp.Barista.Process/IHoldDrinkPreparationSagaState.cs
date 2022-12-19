namespace ProcessManager101CSharp.Barista.Process;

public interface IHoldDrinkPreparationSagaState
{
    public DrinkPreparationSagaId DrinkPreparationSagaId { get; set; }
    public DrinkPreparationSaga.State SagaState { get; set; }
}