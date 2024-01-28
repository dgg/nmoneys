using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace NMoneys.Serialization.Json_NET;

/// <summary>
/// Converts an monetary quantity <see cref="NMoneys.Money"/> to and from JSON.
/// </summary>
/// <remarks>
/// <para>The serialized quantity would look something like: <c>{"Amount": 0, "Currency": "XXX"}</c>.</para>
/// <para>Provides better JSON pointers for deserialization errors: better suited when not in control of serialization.</para>
/// </remarks>
public class DescriptiveMoneyConverter : JsonConverter<Money>
{
	private readonly bool _forceStringEnum;

	/// <param name="forceStringEnum">Ignore enum value configuration and force string representation of the
	/// <see cref="Money.CurrencyCode"/> when serializing.</param>
	public DescriptiveMoneyConverter(bool forceStringEnum = false)
	{
		_forceStringEnum = forceStringEnum;
	}

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
		string amountName = resolver?.GetResolvedPropertyName("Amount") ?? "amount";
		writer.WritePropertyName(amountName);
		writer.WriteValue(amount);
	}

	private void writeCurrency(CurrencyIsoCode currency,
		JsonWriter writer, JsonSerializer serializer, DefaultContractResolver? resolver)
	{
		// non-pascal if "weird" resolver
		string currencyName = resolver?.GetResolvedPropertyName("Currency") ?? "currency";
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

	#region read

	/// <inheritdoc />
	public override Money ReadJson([NotNull] JsonReader reader,
		Type objectType, Money existingValue,
		bool hasExistingValue, [NotNull] JsonSerializer serializer)
	{
		DefaultContractResolver? resolver = serializer.ContractResolver as DefaultContractResolver;

		// no need to read StartObject token (already read)

		decimal amount = readAmount(reader, resolver);
		CurrencyIsoCode currency = readCurrency(reader, serializer, resolver);

		// but EndObject needs to be read
		readEndObject(reader);

		return new Money(amount, currency);
	}

	private static decimal readAmount(JsonReader reader, DefaultContractResolver? resolver)
	{
		readProperty(reader);
		ensurePropertyName("Amount", reader, resolver);
		decimal amount = readAmountValue(reader);
		return amount;
	}

	private static CurrencyIsoCode readCurrency(JsonReader reader, JsonSerializer serializer, DefaultContractResolver? resolver)
	{
		readProperty(reader);
		ensurePropertyName("Currency", reader, resolver);
		CurrencyIsoCode currency = readCurrencyValue(reader, serializer);
		return currency;
	}

	private static void readEndObject(JsonReader reader)
	{
		bool read = reader.Read();
		if (!read || reader.TokenType != JsonToken.EndObject)
		{
			throw buildException($"Expected token type '{JsonToken.EndObject}', but got '{reader.TokenType}'.", reader);
		}
	}

	private static JsonSerializationException buildException(string message, JsonReader reader, Exception? inner = null)
	{
		IJsonLineInfo? info = reader as IJsonLineInfo;
		JsonSerializationException exception = (info == null || info.HasLineInfo()) ?
			new JsonSerializationException(message) :
			new JsonSerializationException(message, reader.Path, info.LineNumber, info.LinePosition, inner);
		return exception;
	}


	private static void readProperty(JsonReader reader)
	{
		bool isRead = reader.Read();
		if (!isRead || reader.TokenType != JsonToken.PropertyName)
		{
			throw buildException($"Expected token type '{JsonToken.PropertyName}', but got '{reader.TokenType}'.", reader);
		}
	}

	private static void ensurePropertyName(string pascalSingleName, JsonReader reader, DefaultContractResolver? resolver)
	{
#pragma warning disable CA1308
		string propName = resolver?.GetResolvedPropertyName(pascalSingleName) ?? pascalSingleName.ToLowerInvariant();
#pragma warning restore CA1308
		bool matchAmount = StringComparer.Ordinal.Equals(reader.Value, propName);
		if (!matchAmount)
		{
			throw buildException($"Expected property '{propName}', but got '{reader.Value}'.", reader);
		}
	}

	private static decimal readAmountValue(JsonReader reader)
	{
		try
		{
			var amount = reader.ReadAsDecimal();
			if (!amount.HasValue)
			{
				throw buildException("Amount should not be nullable.", reader);
			}

			return amount.Value;
		}
		catch (Exception ex)
		{
			throw buildException("Could not read amount value.", reader, ex);
		}
	}

	private static CurrencyIsoCode readCurrencyValue(JsonReader reader, JsonSerializer serializer)
	{
		bool read = reader.Read();
		if (!read)
		{
			throw buildException("Expected value token type.", reader);
		}

		CurrencyIsoCode currency = serializer.Deserialize<CurrencyIsoCode>(reader);
		ensureDefined(currency, reader);

		return currency;
	}

	private static void ensureDefined(CurrencyIsoCode maybeCurrency, JsonReader reader)
	{
		try
		{
			maybeCurrency.AssertDefined();
		}
		catch (Exception ex)
		{
			throw buildException($"Currency '{maybeCurrency}' not defined.", reader, ex);
		}
	}


	#endregion
}
