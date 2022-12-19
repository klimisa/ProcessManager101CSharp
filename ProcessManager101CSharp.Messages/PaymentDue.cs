namespace ProcessManager101CSharp.Messages;

public class PaymentDue
{
    public decimal Amount { get; set; }

    public Guid CorrelationId { get; set; }
}