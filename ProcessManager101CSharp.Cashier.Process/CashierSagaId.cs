namespace ProcessManager101CSharp.Cashier.Process;

using System;
using System.Text.Json;

[System.Text.Json.Serialization.JsonConverter(typeof(CashierSagaIdJsonTextConverter))]
public record CashierSagaId
{
    public Guid Id { get; }

    public CashierSagaId(Guid id)
    {
        if (id == default)
            throw new ArgumentNullException(nameof(id), "Drink preparation saga id cannot be empty");

        Id = id;
    }

    public static implicit operator Guid(CashierSagaId self) => self.Id;
    public static explicit operator CashierSagaId(Guid id) => new(id);
    public static CashierSagaId From(Guid id) => new(id);
    public static CashierSagaId New() => new(Guid.NewGuid());

    public override string ToString() => $"{Id}";
}

public class
    CashierSagaIdJsonTextConverter : System.Text.Json.Serialization.JsonConverter<CashierSagaId>
{
    public override CashierSagaId Read(ref Utf8JsonReader reader, Type typeToConvert,
        JsonSerializerOptions options)
        => CashierSagaId.From(
            Guid.Parse(reader.GetString() ?? throw new InvalidOperationException()));

    public override void Write(Utf8JsonWriter writer, CashierSagaId value, JsonSerializerOptions options)
        => writer.WriteStringValue(value.Id);
}