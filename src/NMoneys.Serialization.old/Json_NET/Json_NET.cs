using System;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace NMoneys.Serialization.Json_NET
{
	#region converters

	/// <summary>
	/// Converts a <see cref="Money"/> instance to and from JSON in the canonical way.
	/// </summary>
	/// <remarks>The canonical way is the one implemented in NMoneys itself, with an <c>Amount</c>
	/// numeric property and a <c>Currency</c> object with a three-letter code <c>IsoCode"</c> property.
	/// <para>
	/// Property casing must be configured apart from this converter using, for instance, another
	/// <see cref="IContractResolver"/>
	/// </para>
	/// <para>This converter can handle conversion to and from <see cref="Nullable{Money}"/> as well.</para>
	/// </remarks>
	/// <example>
	/// <code>{"Amount" : 123.4, "Currency" : {"IsoCode" : "XXX"}}</code>
	/// <code>{"amount" : 123.4, "currency" : {"isoCode" : "XXX"}}</code>
	/// </example>
	public class CanonicalMoneyConverter : JsonConverter
	{
		private readonly IMoneyWriter _writer;
		private readonly IMoneyReader _reader;

		public CanonicalMoneyConverter()
		{
			_writer = new CanonicalMoneyWriter();
			_reader = new CanonicalMoneyReader();
		}

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			var instance = (Money) value;

			_writer.WriteTo(instance, serializer.ContractResolver, writer);
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			Money? money = default(Money?);
			var token = JToken.ReadFrom(reader);
			if (token.HasValues)
			{
				money = new Money(
					_reader.ReadAmount(token, serializer.ContractResolver),
					_reader.ReadCurrencyCode(token, serializer.ContractResolver));
			}
			return MoneyReader.AdaptNullables(objectType, money);
		}

		public override bool CanConvert(Type objectType)
		{
			return objectType == typeof(Money) || objectType == typeof(Money?);
		}
	}

	/// <summary>
	/// Converts a <see cref="Money"/> instance to and from JSON in a default (standard) way.
	/// </summary>
	/// <remarks>The default (standard) way is the one that a normal serializer would do for a money instance,
	/// with an <c>Amount</c> numeric property and a <c>Currency</c> code. The <c>Currency</c> property can be
	/// serialized either as a string (the default or providing <see cref="CurrencyStyle.Alphabetic"/>) or as
	/// a number (providing <see cref="CurrencyStyle.Numeric"/>).
	/// <para>
	/// Property casing must be configured apart from this converter using, for instance, another
	/// <see cref="IContractResolver"/>
	/// </para>
	/// <para>This converter can handle conversion to and from <see cref="Nullable{Money}"/> as well.</para>
	/// </remarks>
	/// <example>
	/// <code>{"Amount" : 123.4, "Currency" : "XXX"}</code>
	/// <code>{"amount" : 123.4, "currency" : 999}</code>
	/// </example>
	public class DefaultMoneyConverter : JsonConverter
	{
		private readonly IMoneyWriter _writer;
		private readonly IMoneyReader _reader;

		public DefaultMoneyConverter() : this(CurrencyStyle.Alphabetic) { }

		public DefaultMoneyConverter(CurrencyStyle style)
		{
			_writer = new DefaultMoneyWriter(style);
			_reader = new DefaultMoneyReader(style);
		}

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			var instance = (Money) value;

			_writer.WriteTo(instance, serializer.ContractResolver, writer);
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			var token = JToken.ReadFrom(reader);
			Money? money = default(Money?);
			if (token.HasValues)
			{
				money = new Money(
					_reader.ReadAmount(token, serializer.ContractResolver),
					_reader.ReadCurrencyCode(token, serializer.ContractResolver));
			}
			return MoneyReader.AdaptNullables(objectType, money);
		}

		public override bool CanConvert(Type objectType)
		{
			return objectType == typeof(Money) || objectType == typeof(Money?);
		}
	}

	/// <summary>
	/// Converts a <see cref="Money"/> instance from JSON numeric quantities.
	/// </summary>
	/// <remarks>To be used in integration scenarios when no currency can be provided and a default
	/// currency can be used.
	/// <para>
	/// Property casing must be configured apart from this converter using, for instance, another
	/// <see cref="IContractResolver"/>
	/// </para>
	/// </remarks>
	/// <example>
	/// <code>{"Amount" : 123.4}</code>
	/// <code>{"any_other_name" : 123.4, ...}</code>
	/// </example>
	public class CurrencyLessMoneyConverter : JsonConverter
	{
		private readonly IMoneyReader _reader;

		public CurrencyLessMoneyConverter() : this(CurrencyIsoCode.XXX) { }

		public CurrencyLessMoneyConverter(CurrencyIsoCode defaultCurrency)
		{
			_reader = new CurrencyLessMoneyReader(defaultCurrency);
		}

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			throw new NotImplementedException(
				"Deserialization only. Moneys should never be serialized without currency information.");
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			var token = JToken.ReadFrom(reader);
			return new Money(
				_reader.ReadAmount(token, serializer.ContractResolver),
				_reader.ReadCurrencyCode(token, serializer.ContractResolver));
		}

		public override bool CanConvert(Type objectType)
		{
			return objectType == typeof (Money);
		}
	}

	#endregion

	public enum CurrencyStyle : byte
	{
		Alphabetic,
		Numeric
	}

	internal static class ElementName
	{
		private static readonly string _amount = "Amount", _currency = "Currency", _isoCode = "IsoCode";

		private static string name(string element, IContractResolver resolver)
		{
			var @default = resolver as DefaultContractResolver;
			return @default != null ? @default.GetResolvedPropertyName(element) : element;
		}

		public static string Amount(IContractResolver resolver)
		{
			return name(_amount, resolver);
		}

		public static string Currency(IContractResolver resolver)
		{
			return name(_currency, resolver);
		}

		public static string IsoCode(IContractResolver resolver)
		{
			return name(_isoCode, resolver);
		}
	}

	#region support for reading

	internal interface IMoneyReader
	{
		decimal ReadAmount(JToken token, IContractResolver resolver);
		CurrencyIsoCode ReadCurrencyCode(JToken token, IContractResolver resolver);
	}

	internal static class MoneyReader
	{
		internal static object AdaptNullables(Type objectType, Money? money)
		{
			// if nullable, whatever is in money will do
			// it not nullable, we better not return null
			object read = objectType == typeof(Money?) ? money : money.GetValueOrDefault();
			return read;
		}
	}

	internal class DefaultMoneyReader : IMoneyReader
	{
		private readonly CurrencyStyle _style;

		public DefaultMoneyReader(CurrencyStyle style)
		{
			_style = style;
		}

		public decimal ReadAmount(JToken token, IContractResolver resolver)
		{
			return token[ElementName.Amount(resolver)].Value<decimal>();
		}

		public CurrencyIsoCode ReadCurrencyCode(JToken token, IContractResolver resolver)
		{
			JToken currency = token[ElementName.Currency(resolver)];
			return currency.GetValue(_style);
		}
	}

	internal class CanonicalMoneyReader : IMoneyReader
	{
		public decimal ReadAmount(JToken token, IContractResolver resolver)
		{
			return token[ElementName.Amount(resolver)].Value<decimal>();
		}

		public CurrencyIsoCode ReadCurrencyCode(JToken token, IContractResolver resolver)
		{
			JToken isoCode = token[ElementName.Currency(resolver)][ElementName.IsoCode(resolver)];
			return isoCode.GetValue(CurrencyStyle.Alphabetic);
		}
	}

	internal class CurrencyLessMoneyReader : IMoneyReader
	{
		private readonly CurrencyIsoCode _defaultCurrency;

		public CurrencyLessMoneyReader(CurrencyIsoCode defaultCurrency)
		{
			_defaultCurrency = defaultCurrency;
		}

		public decimal ReadAmount(JToken token, IContractResolver resolver)
		{
			return token.Type == JTokenType.Object && token.HasValues
				? token.Values().First().Value<decimal>()
				: token.Value<decimal>();
		}

		public CurrencyIsoCode ReadCurrencyCode(JToken token, IContractResolver resolver)
		{
			return _defaultCurrency;
		}
	}

	internal static class CurrencyStyleReadExtensions
	{
		internal static CurrencyIsoCode GetValue(this JToken token, CurrencyStyle style)
		{
			CurrencyIsoCode value;
			switch (style)
			{
				case CurrencyStyle.Alphabetic:
					var alphabetic = token.Value<string>();
					value = Currency.Code.Parse(alphabetic);
					break;
				case CurrencyStyle.Numeric:
					var numeric = token.Value<short>();
					value = Currency.Code.Cast(numeric);
					break;
				default:
					throw new ArgumentOutOfRangeException("style");
			}
			return value;
		}
	}

	#endregion

	#region support for writing

	internal interface IMoneyWriter
	{
		void WriteTo(Money instance, IContractResolver resolver, JsonWriter writer);
	}

	internal abstract class MoneyWriter : IMoneyWriter
	{
		public void WriteTo(Money instance, IContractResolver resolver, JsonWriter writer)
		{
			writer.WriteStartObject();
			writeAmount(instance, resolver, writer);
			writeCurrency(instance, resolver, writer);
			writer.WriteEndObject();
		}

		protected abstract void writeAmount(Money instance, IContractResolver resolver, JsonWriter writer);
		protected abstract void writeCurrency(Money instance, IContractResolver resolver, JsonWriter writer);
	}

	internal class DefaultMoneyWriter : MoneyWriter
	{
		private readonly CurrencyStyle _style;

		public DefaultMoneyWriter(CurrencyStyle style)
		{
			_style = style;
		}

		protected override void writeAmount(Money instance, IContractResolver resolver, JsonWriter writer)
		{
			writer.WritePropertyName(ElementName.Amount(resolver));
			writer.WriteValue(instance.Amount);
		}

		protected override void writeCurrency(Money instance, IContractResolver resolver, JsonWriter writer)
		{
			writer.WritePropertyName(ElementName.Currency(resolver));
			writer.WriteValue(instance.CurrencyCode, _style);
		}
	}

	internal class CanonicalMoneyWriter : MoneyWriter
	{
		protected override void writeAmount(Money instance, IContractResolver resolver, JsonWriter writer)
		{
			writer.WritePropertyName(ElementName.Amount(resolver));
			writer.WriteValue(instance.Amount);
		}

		protected override void writeCurrency(Money instance, IContractResolver resolver, JsonWriter writer)
		{
			writer.WritePropertyName(ElementName.Currency(resolver));
			writer.WriteStartObject();
			writer.WritePropertyName(ElementName.IsoCode(resolver));
			writer.WriteValue(instance.CurrencyCode.AlphabeticCode());
			writer.WriteEndObject();
		}
	}

	internal static class CurrencyStyleWriteExtensions
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

	#endregion
}
