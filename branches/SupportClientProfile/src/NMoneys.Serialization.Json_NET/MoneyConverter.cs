using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NMoneys.Serialization.Json_NET
{
	public class MoneyConverter : JsonConverter
	{
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			var money = (Money)value;
			writer.WriteStartObject();
			writeAmount(writer, money);
			writeCurrency(writer, money);
			writer.WriteEndObject();
		}

		private void writeAmount(JsonWriter writer, Money money)
		{
			writer.WritePropertyName(Data.Money.AMOUNT);
			writer.WriteValue(money.Amount);
		}

		private void writeCurrency(JsonWriter writer, Money money)
		{
			writer.WritePropertyName(Data.Currency.ROOT_NAME);
			writer.WriteStartObject();
			writer.WritePropertyName(Data.Currency.ISO_CODE);
			writer.WriteValue(money.CurrencyCode.AlphabeticCode());
			writer.WriteEndObject();
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			var token = JToken.ReadFrom(reader);

			return new Money(
				readAmount(token),
				readCurrency(token));
		}

		private static decimal readAmount(JToken token)
		{
			return token[Data.Money.AMOUNT].Value<decimal>();
		}

		private static string readCurrency(JToken token)
		{
			return token[Data.Money.CURRENCY][Data.Currency.ISO_CODE].Value<string>();
		}

		public override bool CanConvert(Type objectType)
		{
			return objectType == typeof(Money);
		}
	}
}
