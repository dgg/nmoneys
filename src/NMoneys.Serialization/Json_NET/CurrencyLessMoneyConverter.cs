using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NMoneys.Serialization.Json_NET
{
	public class CurrencyLessMoneyConverter : JsonConverter
	{
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			throw new NotImplementedException("Deserialization only. Moneys should never be serialized without currency information.");
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			var token = JToken.ReadFrom(reader);

			IMoneyReader @default = new CurrencyLessMoneyReader(serializer.ContractResolver);

			return new Money(
				@default.ReadAmount(token),
				@default.ReadCurrencyCode(token));
		}

		public override bool CanConvert(Type objectType)
		{
			return objectType == typeof(Money);
		}
	}
}