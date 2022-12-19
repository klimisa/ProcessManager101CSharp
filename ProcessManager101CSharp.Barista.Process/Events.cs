namespace ProcessManager101CSharp.Barista.Process;

using Infrastructure.Bulloak;

public class Received__NewOrder : Received<DrinkPreparationSaga.Input.NewOrder>
{
    public Received__NewOrder(Guid correlationId, Guid causationId)
        : base(correlationId, causationId)
    {
    }
}

public class Received__PaymentComplete : Received<DrinkPreparationSaga.Input.PaymentComplete>
{
    public Received__PaymentComplete(Guid correlationId, Guid causationId)
        : base(correlationId, causationId)
    {
    }
}

public class Published__DrinkReady : Published<DrinkPreparationSaga.Output.DrinkReady>
{
    public string Drink { get; set; }
    public string Name { get; set; }

    public Published__DrinkReady(string Drink, string Name, Guid correlationId, Guid causationId)
        : base(correlationId, causationId)
    {
        this.Drink = Drink;
        this.Name = Name;
    }
}

public class Sent__PrepareDrink : Published<DrinkPreparationSaga.Output.PrepareDrink>
{
    public string Drink { get; set; }
    public string Name { get; set; }

    public Sent__PrepareDrink(string Drink, string Name, Guid correlationId, Guid causationId)
        : base(correlationId, causationId)
    {
        this.Drink = Drink;
        this.Name = Name;
    }
}

public class Completed : Infrastructure.Bulloak.Completed
{
    public Completed(Guid correlationId, Guid causationId) : base(correlationId, causationId)
    {
    }
}