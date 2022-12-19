namespace ProcessManager101CSharp.Cashier.Service;

using Process;
using Utile.Messages;
using static Process.CashierSaga;
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
            Command.Reply<Output.PaymentDue>(Output.PaymentDue m) => 
                new Replied__PaymentDue(correlationId, causationId) {Message = m},
            Command.Publish<Output.PaymentComplete>(Output.PaymentComplete m) => 
                new Published__PaymentComplete(correlationId, causationId) {Message = m},
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
            Input.NewOrder m => new Received__NewOrder(correlationId, causationId) { Message = m },
            Input.PaymentReceived m => new Received__PaymentReceived(correlationId, causationId) { Message = m },
            _ => throw new ArgumentOutOfRangeException(nameof(message), message, null)
        };
}