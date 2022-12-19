namespace ProcessManager101CSharp.Infrastructure.ProcessManager;

public static class Extensions
{
    public static Command Send<TOutput>(this TOutput command) =>
        new Command.Send<TOutput>(command);

    public static Command Publish<TOutput>(this TOutput command) =>
        new Command.Publish<TOutput>(command);
    
    public static Command Reply<TOutput>(this TOutput command) =>
        new Command.Reply<TOutput>(command);
}