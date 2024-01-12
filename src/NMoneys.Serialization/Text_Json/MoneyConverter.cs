using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace NMoneys.Serialization.Text_Json;

/// <summary>
/// Converts an monetary quantity <see cref="NMoneys.Money"/> to and from JSON.
/// </summary>
/// <remarks>
/// <para>The serialized quantity would look something like: <c>{"Amount": 0, "Currency": "XXX"}</c>.</para>
/// <para>Provides limited JSON pointers for deserialization errors: better suited when in control of serialization.</para>
/// </remarks>
public class MoneyConverter : JsonConverter<Money>
{
	// forcing enum string argument is not a good idea as deserialization will fail

	#region read

	public override Money Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		JsonObject obj = parseObject(ref reader);

		decimal amount = getAmount(obj);
		CurrencyIsoCode currency = getCurrency(obj, options);

		return new Money(amount, currency);
	}

	private static JsonObject parseObject(ref Utf8JsonReader reader)
	{
		var nodeOpts = new JsonNodeOptions { PropertyNameCaseInsensitive = true };

		JsonNode? objNode = JsonNode.Parse(ref reader, nodeOpts) ?? throw new JsonException($"Expected '{nameof(JsonObject)}' but was 'null'.");
		JsonObject obj = objNode.AsObject();
		return obj;
	}

	private static decimal getAmount(JsonObject obj)
	{
		string propName = "Amount";
		JsonNode amountNode = getProperty(obj, propName);
		var amount = amountNode.GetValue<decimal>();
		return amount;
	}

	private static CurrencyIsoCode getCurrency(JsonObject obj, JsonSerializerOptions options)
	{
		string propName = "Currency";
		JsonNode currencyNode = getProperty(obj, propName);
		var currency = currencyNode.Deserialize<CurrencyIsoCode>(options);
		currency.AssertDefined();
		return currency;
	}

	private static JsonNode getProperty(JsonObject obj, string singleWordPropName)
	{
		if (!obj.TryGetPropertyValue(singleWordPropName, out JsonNode? propertyNode) || propertyNode == null)
		{
			throw new JsonException($"Missing property '{singleWordPropName}'.");
		}

		return propertyNode;
	}

	#endregion

	#region write

	public override void Write([NotNull] Utf8JsonWriter writer, Money value, [NotNull] JsonSerializerOptions options)
	{
		writer.WriteStartObject();

		writeAmount(value.Amount, writer, options);
		writeCurrency(value.CurrencyCode, writer, options);

		writer.WriteEndObject();
	}

	private static void writeAmount(decimal amount, Utf8JsonWriter writer, JsonSerializerOptions options)
	{
		// pascal if no policy override
		string amountName = options.PropertyNamingPolicy?.ConvertName("Amount") ?? "Amount";
		writer.WriteNumber(amountName, amount);
	}

	private static void writeCurrency(CurrencyIsoCode currency, Utf8JsonWriter writer, JsonSerializerOptions options)
	{
		// pascal if no policy override
		string currencyName = options.PropertyNamingPolicy?.ConvertName("Currency") ?? "Currency";

		writer.WritePropertyName(currencyName);
		JsonSerializer.Serialize(writer, currency, options);
	}

	#endregion
}
