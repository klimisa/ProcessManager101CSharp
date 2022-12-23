namespace ProcessManager101CSharp.Infrastructure.ProcessManager;

public class Event
{
    public class Received<TInput> : Event
    {
        public Received(TInput @in)
        {
            In = @in;
        }

        public TInput In { get; init; }

        public void Deconstruct(out TInput @in)
        {
            @in = In;
        }
    }

    public class Published<TOutput> : Event
    {
        public Published(TOutput @out)
        {
            Out = @out;
        }

        public TOutput Out { get; init; }

        public void Deconstruct(out TOutput @out)
        {
            @out = Out;
        }
    }

    public class Replied<TOutput> : Event
    {
        public Replied(TOutput @out)
        {
            Out = @out;
        }

        public TOutput Out { get; init; }

        public void Deconstruct(out TOutput @out)
        {
            @out = Out;
        }
    }

    public class Completed : Event
    {
    }
}

public record Command
{
    public record Reply<TOutput>(TOutput TOut) : Command;

    public record Send<TOutput>(TOutput TOut) : Command;

    public record Schedule<TOutput>(TOutput TOut, TimeSpan OnTime) : Command;

    public record Publish<TOutput>(TOutput TOut) : Command;

    public record Complete : Command;
}
