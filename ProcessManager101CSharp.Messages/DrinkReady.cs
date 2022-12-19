namespace ProcessManager101CSharp.Messages;

public class DrinkReady
{
    public Guid CorrelationId { get; set; }
    public string Drink { get; set; }
    public string Name { get; set; }
}