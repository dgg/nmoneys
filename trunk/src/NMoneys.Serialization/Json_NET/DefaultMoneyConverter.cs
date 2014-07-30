using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NMoneys.Serialization.Json_NET
{
	public class DefaultMoneyConverter : JsonConverter
	{
		private readonly CurrencyStyle _style;

		public DefaultMoneyConverter(CurrencyStyle style = CurrencyStyle.Alphabetic)
		{
			_style = style;
		}

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			var instance = (Money) value;

			IMoneyWriter @default = new DefaultMoneyWriter(instance, _style, serializer.ContractResolver);
			@default.WriteTo(writer);
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			var token = JToken.ReadFrom(reader);

			IMoneyReader @default = new DefaultMoneyReader(_style, serializer.ContractResolver);

			return new Money(
				@default.ReadAmount(token),
				@default.ReadCurrencyCode(token));
		}

		public override bool CanConvert(Type objectType)
		{
			return objectType == typeof (Money);
		}
	}
}