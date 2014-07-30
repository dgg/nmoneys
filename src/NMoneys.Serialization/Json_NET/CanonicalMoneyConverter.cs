using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace NMoneys.Serialization.Json_NET
{
	/// <summary>
	/// Converts a <see cref="Money"/> instance to and from JSON.
	/// </summary>
	/// <remarks>Json.Net already performs custom serialization of <see cref="Currency"/> and <see cref="CurrencyIsoCode"/> so no converter needed.</remarks>
	public class CanonicalMoneyConverter : JsonConverter
	{
		/// <summary>
		/// Writes the custom JSON representation of the object.
		/// </summary>
		/// <param name="writer">The <see cref="JsonWriter"/> to write to.</param>
		/// <param name="value">The value.</param>
		/// <param name="serializer">The calling serializer.</param>
		/// <exception cref="InvalidCastException"><paramref name="value"/> is not of type <see cref="Money"/>.</exception>
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			var money = (Money)value;

			IMoneyWriter canonical = new CanonicalMoneyWriter(money, serializer.ContractResolver);
			canonical.WriteTo(writer);
		}

		/// <summary>
		/// Reads the custom JSON representation of the object.
		/// </summary>
		/// <param name="reader">The <see cref="JsonReader"/> to read from.</param>
		/// <param name="objectType">Type of the object.</param>
		/// <param name="existingValue">The existing value of object being read.</param>
		/// <param name="serializer">The calling serializer.</param>
		/// <returns>
		/// The <see cref="Money"/> value.
		/// </returns>
		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			var token = JToken.ReadFrom(reader);

			var canonical = new CanonicalMoneyReader(serializer.ContractResolver as DefaultContractResolver);

			return new Money(
				canonical.ReadAmount(token),
				canonical.ReadCurrencyCode(token));
		}

		/// <summary>
		/// Determines whether this instance can convert the specified object type.
		/// </summary>
		/// <param name="objectType">Type of the object.</param>
		/// <returns>
		/// <c>true</c> if this instance can convert the specified object type; otherwise, <c>false</c>.
		/// </returns>
		public override bool CanConvert(Type objectType)
		{
			return objectType == typeof(Money);
		}
	}
}
