using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace NMoneys.Serialization.Json_NET
{
	internal interface IMoneyWriter
	{
		void WriteTo(JsonWriter writer);
	}

	internal abstract class MoneyWriter : MoneyOperator, IMoneyWriter
	{
		protected MoneyWriter(IContractResolver resolver) : base(resolver) { }

		public void WriteTo(JsonWriter writer)
		{
			writer.WriteStartObject();
			writeAmount(writer);
			writeCurrency(writer);
			writer.WriteEndObject();
		}

		protected abstract void writeAmount(JsonWriter writer);
		protected abstract void writeCurrency(JsonWriter writer);
	}

	internal class DefaultMoneyWriter : MoneyWriter
	{
		private readonly Money _instance;
		private readonly CurrencyStyle _style;

		public DefaultMoneyWriter(Money instance, CurrencyStyle style, IContractResolver resolver)
			: base(resolver)
		{
			_instance = instance;
			_style = style;
		}

		protected override void writeAmount(JsonWriter writer)
		{
			writer.WritePropertyName(_amount);
			writer.WriteValue(_instance.Amount);
		}

		protected override void writeCurrency(JsonWriter writer)
		{
			writer.WritePropertyName(_currency);
			writer.WriteValue(_instance.CurrencyCode, _style);
		}
	}

	internal class CanonicalMoneyWriter : MoneyWriter
	{
		private readonly Money _instance;

		public CanonicalMoneyWriter(Money instance, IContractResolver resolver)
			: base(resolver)
		{
			_instance = instance;
		}

		protected override void writeAmount(JsonWriter writer)
		{
			writer.WritePropertyName(_amount);
			writer.WriteValue(_instance.Amount);
		}

		protected override void writeCurrency(JsonWriter writer)
		{
			writer.WritePropertyName(_currency);
			writer.WriteStartObject();
			writer.WritePropertyName(_isoCode);
			writer.WriteValue(_instance.CurrencyCode.AlphabeticCode());
			writer.WriteEndObject();
		}
	}

	internal static partial class CurrencyStyleExtensions
	{
		internal static void WriteValue(this JsonWriter writer, CurrencyIsoCode currency, CurrencyStyle style)
		{
			switch (style)
			{
				case CurrencyStyle.Alphabetic:
					writer.WriteValue(currency.AlphabeticCode());
					break;
				case CurrencyStyle.Numeric:
					writer.WriteValue(currency.NumericCode());
					break;
				default:
					throw new ArgumentOutOfRangeException("style");
			}
		}
	}
}