using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace NMoneys.Serialization.Json_NET;

/// <summary>
/// Converts an monetary quantity <see cref="NMoneys.Money"/> to and from JSON.
/// </summary>
/// <remarks>
/// <para>The serialized quantity would look something like: <c>{"Amount": 0, "Currency": "XXX"}</c>.</para>
/// <para>Provides limited JSON pointers for deserialization errors: better suited when in control of serialization.</para>
/// </remarks>
public class MoneyConverter : JsonConverter<Money>
{
	private readonly bool _forceStringEnum;

	/// <param name="forceStringEnum">Ignore enum value configuration and force string representation of the
	/// <see cref="Money.CurrencyCode"/> when serializing.</param>
	public MoneyConverter(bool forceStringEnum = false)
	{
		_forceStringEnum = forceStringEnum;
	}

	#region read

	/// <inheritdoc />
	public override Money ReadJson([NotNull] JsonReader reader,
		Type objectType, Money existingValue,
		bool hasExistingValue, [NotNull] JsonSerializer serializer)
	{
		JObject obj = readObject(reader);
		decimal amount = getAmount(obj);
		CurrencyIsoCode currency = getCurrency(obj);
		return new Money(amount, currency);
	}

	private static JObject readObject(JsonReader reader)
	{
		JToken objToken = JToken.ReadFrom(reader);
		if (objToken.Type != JTokenType.Object)
		{
			throw new JsonSerializationException($"Expected token '{JTokenType.Object}', but got '{objToken.Type}'.");
		}

		return (JObject)objToken;
	}

	private static decimal getAmount(JObject obj)
	{
		string propName = "Amount";
		JProperty amountProp = getProperty(obj, propName);
		decimal? amount = amountProp.Value.Value<decimal>();
		return amount ?? throw new JsonSerializationException($"'{propName}' cannot be null.");
	}

	private static CurrencyIsoCode getCurrency(JObject obj)
	{
		string propName = "Currency";
		JProperty currencyProp = getProperty(obj, propName);
		var currency = currencyProp.Value.ToObject<CurrencyIsoCode>();
		currency.AssertDefined();
		return currency;
	}

	private static JProperty getProperty(JObject obj, string singleWordPropName)
	{
		// since props are single-word, case ignoring cover most common: pascal, camel, snake, kebab
		JProperty? amountProp = obj.Property(singleWordPropName, StringComparison.OrdinalIgnoreCase) ??
		                        throw new JsonSerializationException($"Missing property '{singleWordPropName}'.");
		return amountProp;
	}

	#endregion

	#region write

	/// <inheritdoc />
	public override void WriteJson([NotNull] JsonWriter writer, Money value, [NotNull] JsonSerializer serializer)
	{
		DefaultContractResolver? resolver = serializer.ContractResolver as DefaultContractResolver;

		writer.WriteStartObject();

		writeAmount(value.Amount, writer, resolver);
		writeCurrency(value.CurrencyCode, writer, serializer, resolver);

		writer.WriteEndObject();
	}

	private static void writeAmount(decimal amount, JsonWriter writer, DefaultContractResolver? resolver)
	{
		// non-pascal if "weird" resolver
		string amountName = resolver?.GetResolvedPropertyName("Amount") ?? "Amount";
		writer.WritePropertyName(amountName);
		writer.WriteValue(amount);
	}

	private void writeCurrency(CurrencyIsoCode currency,
		JsonWriter writer, JsonSerializer serializer, DefaultContractResolver? resolver)
	{
		// non-pascal if "weird" resolver
		string currencyName = resolver?.GetResolvedPropertyName("Currency") ?? "Currency";
		writer.WritePropertyName(currencyName);
		if (_forceStringEnum)
		{
			// ignore configured enum value convention, string it is
			writer.WriteValue(currency.ToString());
		}
		else
		{
			// follow configured enum value convention
			serializer.Serialize(writer, currency, typeof(CurrencyIsoCode));
		}
	}

	#endregion
}
