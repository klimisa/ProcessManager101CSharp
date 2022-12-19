namespace ProcessManager101CSharp.Infrastructure.Bulloak;

using Utile.Messages;

public class Received<T> : Event
{
    public Received(Guid correlationId, Guid causationId) : base(correlationId, causationId)
    {
    }

    public T Message { get; set; }
}

public class Published<T> : Event
{
    public Published(Guid correlationId, Guid causationId) : base(correlationId, causationId)
    {
    }

    public T Message { get; set; }
}

public class Completed : Event
{
    public Completed(Guid correlationId, Guid causationId) : base(correlationId, causationId)
    {
    }
}

