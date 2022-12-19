namespace ProcessManager101CSharp.Messages;

public class PrepareDrink
{
    public string Name { get; set; }
    public string Drink { get; set; }
    public Guid CorrelationId { get; set; }
}