namespace ProcessManager101CSharp.Messages;

public class SubmitPayment
{
    public Guid OrderId { get; set; }
    public string PaymentType { get; set; }
    public decimal Amount { get; set; }
    public Guid CorrelationId { get; set; }
 
}