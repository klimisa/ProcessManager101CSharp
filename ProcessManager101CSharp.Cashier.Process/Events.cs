namespace ProcessManager101CSharp.Cashier.Process;

using Infrastructure.Bulloak;

public class Received__NewOrder : Received<CashierSaga.Input.NewOrder>
{
    public Received__NewOrder(Guid correlationId, Guid causationId)
        : base(correlationId, causationId)
    {
    }
}

public class Received__PaymentReceived : Received<CashierSaga.Input.PaymentReceived>
{
    public Received__PaymentReceived(Guid correlationId, Guid causationId)
        : base(correlationId, causationId)
    {
    }
}

public class Replied__PaymentDue : Published<CashierSaga.Output.PaymentDue>
{
    public Replied__PaymentDue(Guid correlationId, Guid causationId)
        : base(correlationId, causationId)
    {
    }
}

public class Published__PaymentComplete : Published<CashierSaga.Output.PaymentComplete>
{
    public Published__PaymentComplete(Guid correlationId, Guid causationId)
        : base(correlationId, causationId)
    {
    }
}

public class Completed : Infrastructure.Bulloak.Completed
{
    public Completed(Guid correlationId, Guid causationId) : base(correlationId, causationId)
    {
    }
}