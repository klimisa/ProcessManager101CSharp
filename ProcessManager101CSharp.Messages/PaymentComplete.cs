namespace ProcessManager101CSharp.Messages;

public class PaymentComplete
{
    public Guid OrderId { get; set; }
    public Guid CorrelationId { get; set; }
    public string PaymentType  { get; set; }
    public decimal Amount  { get; set; }

}