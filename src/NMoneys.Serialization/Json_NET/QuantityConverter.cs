using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NMoneys.Serialization.Json_NET;

/// <summary>
/// Converts an monetary quantity <see cref="NMoneys.Money"/> to and from JSON.
/// </summary>
/// <remarks>
/// <para>The serialized quantity would look something like: <c>"XTS 0"</c>.</para>
/// </remarks>
public class QuantityConverter : JsonConverter<Money>
{
	/// <inheritdoc />
	public override Money ReadJson([NotNull] JsonReader reader,
		Type objectType, Money existingValue,
		bool hasExistingValue, [NotNull] JsonSerializer serializer)
	{
		JToken strToken = JToken.ReadFrom(reader);
		if (strToken.Type != JTokenType.String)
		{
			throw new JsonSerializationException($"Expected token '{JTokenType.String}', but got '{strToken.Type}'.");
		}

		string quantity = strToken.Value<string>() ?? throw new JsonSerializationException($"Quantity cannot be null.");

		Money parsed = Money.Parse(quantity);
		return parsed;
	}

	/// <inheritdoc />
	public override void WriteJson([NotNull] JsonWriter writer, Money value, [NotNull] JsonSerializer serializer)
	{
		writer.WriteValue(value.AsQuantity());
	}
}
