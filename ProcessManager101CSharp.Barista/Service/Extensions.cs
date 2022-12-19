namespace ProcessManager101CSharp.Barista.Service;

using Process;
using Utile.Messages;
using static Process.DrinkPreparationSaga;
using Command = Infrastructure.ProcessManager.Command;

public static class Extensions
{
    public static IEvent[] ToEvents(
        this IEnumerable<Command> commands,
        Guid correlationId,
        Guid causationId
    ) =>
        commands.Select(cmd => cmd.ToEvent(correlationId, causationId)).ToArray();

    private static IEvent ToEvent(
        this Command command,
        Guid correlationId,
        Guid causationId
    ) =>
        command switch
        {
            Command.Send<Output.PrepareDrink>(Output.PrepareDrink m) => new Sent__PrepareDrink(m.Drink, m.Name,
                correlationId, causationId) { Message = m },
            Command.Publish<Output.DrinkReady>(Output.DrinkReady m) => new Published__DrinkReady(m.Drink, m.Name,
                correlationId, causationId) { Message = m },
            Command.Completed => new Completed(Guid.NewGuid(), Guid.NewGuid()),
            _ => throw new ArgumentOutOfRangeException(nameof(command), command, null)
        };

    public static IEvent ToEvent(
        this Input message,
        Guid correlationId,
        Guid causationId
    ) =>
        message switch
        {
            Input.NewOrder m =>
                new Received__NewOrder(correlationId, causationId) { Message = m },
            Input.PaymentComplete m =>
                new Received__PaymentComplete(correlationId, causationId) { Message = m },
            _ => throw new ArgumentOutOfRangeException(nameof(message), message, null)
        };
}