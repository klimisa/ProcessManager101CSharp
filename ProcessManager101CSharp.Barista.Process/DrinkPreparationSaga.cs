namespace ProcessManager101CSharp.Barista.Process;

using Infrastructure.ProcessManager;

public class DrinkPreparationSaga
{
    public record State
    {
        public record Initial : State;

        public record PreparingDrink(string Drink, string Name) : State;

        public record WaitingForPayment : State;

        public record Completed : State;
    };

    public record Input
    {
        public record NewOrder(Guid CorrelationId, string Name, string Size, string Item) : Input;

        public record PaymentComplete(Guid CorrelationId) : Input;
    }

    public record Output
    {
        public record DrinkReady(Guid CorrelationId, string Drink, string Name) : Output;

        public record PrepareDrink(Guid CorrelationId, string Drink, string Name) : Output;
    }

    public static State Evolve(State state, Event message) =>
        (state, message) switch
        {
            (State.Initial, Event.Received<Input>(Input.NewOrder m)) =>
                new State.PreparingDrink(
                    $"{m.Size}, {m.Name}",
                    m.Name
                ),
            (State.PreparingDrink, Event.Received<Input>(Input.PaymentComplete m)) =>
                new State.Completed(),
            _ => state
        };

    public static IEnumerable<Command> Handle(State state, Input message)
    {
        switch (state, message)
        {
            case (State.Initial, Input.NewOrder m):
                var drink = $"{m.Size} {m.Item}";
                Console.WriteLine($"{drink} for {m.Name}, got it!");
                yield return new Output.PrepareDrink(
                    m.CorrelationId,
                    drink,
                    m.Name
                ).Send();
                break;
            case (State.PreparingDrink s, Input.PaymentComplete m):
                Console.WriteLine($"Payment Complete for '{s.Name}' got it!");
                yield return new Output.DrinkReady(
                    m.CorrelationId,
                    s.Drink,
                    s.Name
                ).Publish();
                yield return new Command.Completed();
                break;
            default: throw new Exception($"%A{message} can not be handled by %A{state}");
        }
    }
}