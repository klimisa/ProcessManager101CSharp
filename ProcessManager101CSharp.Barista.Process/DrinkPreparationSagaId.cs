namespace ProcessManager101CSharp.Barista.Process;

using System;
using System.Text.Json;

[System.Text.Json.Serialization.JsonConverter(typeof(DrinkPreparationSagaIdJsonTextConverter))]
public record DrinkPreparationSagaId
{
    public Guid Id { get; }

    public DrinkPreparationSagaId(Guid id)
    {
        if (id == default)
            throw new ArgumentNullException(nameof(id), "Drink preparation saga id cannot be empty");

        Id = id;
    }

    public static implicit operator Guid(DrinkPreparationSagaId self) => self.Id;
    public static explicit operator DrinkPreparationSagaId(Guid id) => new(id);
    public static DrinkPreparationSagaId From(Guid id) => new(id);
    public static DrinkPreparationSagaId New() => new(Guid.NewGuid());

    public override string ToString() => $"{Id}";
}

public class
    DrinkPreparationSagaIdJsonTextConverter : System.Text.Json.Serialization.JsonConverter<DrinkPreparationSagaId>
{
    public override DrinkPreparationSagaId Read(ref Utf8JsonReader reader, Type typeToConvert,
        JsonSerializerOptions options)
        => DrinkPreparationSagaId.From(
            Guid.Parse(reader.GetString() ?? throw new InvalidOperationException()));

    public override void Write(Utf8JsonWriter writer, DrinkPreparationSagaId value, JsonSerializerOptions options)
        => writer.WriteStringValue(value.Id);
}