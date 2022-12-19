namespace ProcessManager101CSharp.Messages;

public class NewOrder
{
    public Guid Id { get; set; }
    public Guid CorrelationId { get; set; }
    public string Name { get; set; }
    public string Size { get; set; }
    public string Item { get; set; }
}