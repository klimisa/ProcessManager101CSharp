namespace ProcessManager101CSharp.Cashier.Process;

using Infrastructure.ProcessManager;

public class CashierSaga
{
    public record State
    {
        public record Initial : State;

        public record WaitingForPayment : State;

        public record Completed : State;
    }

    public record Input
    {
        public record NewOrder(Guid Id, Guid CorrelationId, string Name, string Size, string Item) : Input;


        public record PaymentReceived(Guid OrderId, Guid CorrelationId, PaymentType PaymentType, decimal Amount) : Input;
    }

    public record Output
    {
        public record PaymentDue(Guid OrderId, Guid CorrelationId, decimal Amount) : Output;

        public record PaymentComplete(Guid OrderId, Guid CorrelationId, PaymentType PaymentType, decimal Amount) : Output;
    }

    public static State Evolve(State state, Event message) =>
        (state, message) switch
        {
            (State.Initial, Event.Received<Input>(Input.NewOrder m)) =>
                new State.WaitingForPayment(
                ),
            (State.WaitingForPayment, Event.Received<Input>(Input.PaymentReceived m)) =>
                new State.Completed(),
            _ => state
        };

    public static IEnumerable<Command> Handle(State state, Input message)
    {
        switch (state, message)
        {
            case (State.Initial, Input.NewOrder m):
                Console.WriteLine($"I've received an order for a {m.Size} {m.Item} for {m.Name}.");
                yield return new Output.PaymentDue(
                    m.Id,
                    m.CorrelationId,
                    GetPriceForSize(m.Size)
                ).Reply();
                break;
            case (State.WaitingForPayment s, Input.PaymentReceived m):
                yield return new Output.PaymentComplete(
                    m.OrderId,
                    m.CorrelationId,
                    m.PaymentType,
                    m.Amount).Publish();
                yield return new Command.Completed();
                break;
            default: throw new Exception($"%A{message} can not be handled by %A{state}");
        }
    }

    private static decimal GetPriceForSize(string size) =>
        size.ToLower() switch
        {
            "tall" => 3.25m,
            "grande" => 4.00m,
            "venti" => 4.75m,
            _ => throw new Exception($"We don't have that size ({size})")
        };
}