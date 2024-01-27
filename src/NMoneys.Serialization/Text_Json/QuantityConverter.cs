using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace NMoneys.Serialization.Text_Json;

/// <summary>
/// Converts an monetary quantity <see cref="NMoneys.Money"/> to and from JSON.
/// </summary>
/// <remarks>
/// <para>The serialized quantity would look something like: <c>"XXX 0"</c>.</para>
/// </remarks>
public class QuantityConverter : JsonConverter<Money>
{
	public override Money Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		JsonNode strNode = JsonNode.Parse(ref reader) ?? throw new JsonException($"Expected '{nameof(JsonValue)}' but was 'null'.");;
		JsonValue value = strNode.AsValue();
		string quantity = value.GetValue<string>();
		Money parsed = Money.Parse(quantity);
		return parsed;
	}

	public override void Write([NotNull] Utf8JsonWriter writer, Money value, [NotNull] JsonSerializerOptions options)
	{
		writer.WriteStringValue(value.AsQuantity());
	}
}
