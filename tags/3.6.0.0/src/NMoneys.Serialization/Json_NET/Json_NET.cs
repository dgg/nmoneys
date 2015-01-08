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
	/// </remarks>
	/// <example>
	/// <code>{"Amount" : 123.4, "Currency" : {"IsoCode" : "XXX"}}</code>
	/// <code>{"amount" : 123.4, "currency" : {"isoCode" : "XXX"}}</code>
	/// </example>
	public class CanonicalMoneyConverter : JsonConverter
	{
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			var money = (Money) value;

			IMoneyWriter canonical = new CanonicalMoneyWriter(money, serializer.ContractResolver);
			canonical.WriteTo(writer);
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			var token = JToken.ReadFrom(reader);

			var canonical = new CanonicalMoneyReader(serializer.ContractResolver as DefaultContractResolver);

			return new Money(
				canonical.ReadAmount(token),
				canonical.ReadCurrencyCode(token));
		}

		public override bool CanConvert(Type objectType)
		{
			return objectType == typeof (Money);
		}
	}

	/// <summary>
	/// Converts a <see cref="Nullable{Money}"/> instance to and from JSON in the canonical way.
	/// </summary>
	/// <remarks>The canonical way is the one implemented in NMoneys itself, with an <c>Amount</c>
	/// numeric property and a <c>Currency</c> object with a three-letter code <c>IsoCode"</c> property.
	/// <para>
	/// Property casing must be configured apart from this converter using, for instance, another
	/// <see cref="IContractResolver"/>
	/// </para>
	/// </remarks>
	/// <example>
	/// <code>{"Amount" : 123.4, "Currency" : {"IsoCode" : "XXX"}}</code>
	/// <code>{"amount" : 123.4, "currency" : {"isoCode" : "XXX"}}</code>
	/// </example>
	public class CanonicalNullableMoneyConverter : JsonConverter
	{
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			var money = (Money?)value;

			if (money.HasValue)
			{
				IMoneyWriter canonical = new CanonicalMoneyWriter(money.Value, serializer.ContractResolver);
				canonical.WriteTo(writer);
			}
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			var token = JToken.ReadFrom(reader);
			Money? read = default(Money?);
			if (token.HasValues)
			{

				var canonical = new CanonicalMoneyReader(serializer.ContractResolver as DefaultContractResolver);

				read = new Money(
					canonical.ReadAmount(token),
					canonical.ReadCurrencyCode(token));
			}
			return read;
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
	/// </remarks>
	/// <example>
	/// <code>{"Amount" : 123.4, "Currency" : "XXX"}</code>
	/// <code>{"amount" : 123.4, "currency" : 999}</code>
	/// </example>
	public class DefaultMoneyConverter : JsonConverter
	{
		private readonly CurrencyStyle _style;

		public DefaultMoneyConverter() : this(CurrencyStyle.Alphabetic)
		{
		}

		public DefaultMoneyConverter(CurrencyStyle style)
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

	/// <summary>
	/// Converts a <see cref="Nullable{Money}"/> instance to and from JSON in a default (standard) way.
	/// </summary>
	/// <remarks>The default (standard) way is the one that a normal serializer would do for a money instance,
	/// with an <c>Amount</c> numeric property and a <c>Currency</c> code. The <c>Currency</c> property can be
	/// serialized either as a string (the default or providing <see cref="CurrencyStyle.Alphabetic"/>) or as
	/// a number (providing <see cref="CurrencyStyle.Numeric"/>).
	/// <para>
	/// Property casing must be configured apart from this converter using, for instance, another
	/// <see cref="IContractResolver"/>
	/// </para>
	/// </remarks>
	/// <example>
	/// <code>{"Amount" : 123.4, "Currency" : "XXX"}</code>
	/// <code>{"amount" : 123.4, "currency" : 999}</code>
	/// </example>
	public class DefaultNullableMoneyConverter : JsonConverter
	{
		private readonly CurrencyStyle _style;

		public DefaultNullableMoneyConverter()
			: this(CurrencyStyle.Alphabetic)
		{
		}

		public DefaultNullableMoneyConverter(CurrencyStyle style)
		{
			_style = style;
		}

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			var instance = (Money?)value;
			if (instance.HasValue)
			{
				IMoneyWriter @default = new DefaultMoneyWriter(instance.Value, _style, serializer.ContractResolver);
				@default.WriteTo(writer);
			}
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			var token = JToken.ReadFrom(reader);
			Money? read = default(Money?);
			if (token.HasValues)
			{
				IMoneyReader @default = new DefaultMoneyReader(_style, serializer.ContractResolver);

				read = new Money(
					@default.ReadAmount(token),
					@default.ReadCurrencyCode(token));
			}
			return read;
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
		private readonly CurrencyIsoCode _defaultCurrency;

		public CurrencyLessMoneyConverter() : this(CurrencyIsoCode.XXX) { }

		public CurrencyLessMoneyConverter(CurrencyIsoCode defaultCurrency)
		{
			_defaultCurrency = defaultCurrency;
		}

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			throw new NotImplementedException(
				"Deserialization only. Moneys should never be serialized without currency information.");
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			var token = JToken.ReadFrom(reader);

			IMoneyReader @default = new CurrencyLessMoneyReader(_defaultCurrency, serializer.ContractResolver);

			return new Money(
				@default.ReadAmount(token),
				@default.ReadCurrencyCode(token));
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

	internal abstract class MoneyOperator
	{
		protected readonly string _amount, _currency, _money, _isoCode;
		protected MoneyOperator(IContractResolver resolver)
		{
			_amount = "Amount";
			_currency = "Currency";
			_money = "Money";
			_isoCode = "IsoCode";

			var contractResolver = resolver as DefaultContractResolver;
			if (contractResolver != null)
			{
				_amount = contractResolver.GetResolvedPropertyName(_amount);
				_currency = contractResolver.GetResolvedPropertyName(_currency);
				_isoCode = contractResolver.GetResolvedPropertyName(_isoCode);
			}
		}
	}

	#region support for reading

	internal interface IMoneyReader
	{
		decimal ReadAmount(JToken token);
		CurrencyIsoCode ReadCurrencyCode(JToken token);
	}

	internal class DefaultMoneyReader : MoneyOperator, IMoneyReader
	{
		private readonly CurrencyStyle _style;

		public DefaultMoneyReader(CurrencyStyle style, IContractResolver resolver)
			: base(resolver)
		{
			_style = style;
		}

		public decimal ReadAmount(JToken token)
		{
			return token[_amount].Value<decimal>();
		}

		public CurrencyIsoCode ReadCurrencyCode(JToken token)
		{
			JToken currency = token[_currency];
			return currency.GetValue(_style);
		}
	}

	internal class CanonicalMoneyReader : MoneyOperator, IMoneyReader
	{
		public CanonicalMoneyReader(IContractResolver resolver) : base(resolver)
		{
		}

		public decimal ReadAmount(JToken token)
		{
			return token[_amount].Value<decimal>();
		}

		public CurrencyIsoCode ReadCurrencyCode(JToken token)
		{
			JToken isoCode = token[_currency][_isoCode];
			return isoCode.GetValue(CurrencyStyle.Alphabetic);
		}
	}

	internal class CurrencyLessMoneyReader : MoneyOperator, IMoneyReader
	{
		private readonly CurrencyIsoCode _defaultCurrency;

		public CurrencyLessMoneyReader(CurrencyIsoCode defaultCurrency, IContractResolver resolver) : base(resolver)
		{
			_defaultCurrency = defaultCurrency;
		}

		public decimal ReadAmount(JToken token)
		{
			return token.Type == JTokenType.Object && token.HasValues
				? token.Values().First().Value<decimal>()
				: token.Value<decimal>();
		}

		public CurrencyIsoCode ReadCurrencyCode(JToken token)
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
		void WriteTo(JsonWriter writer);
	}

	internal abstract class MoneyWriter : MoneyOperator, IMoneyWriter
	{
		protected MoneyWriter(IContractResolver resolver) : base(resolver)
		{
		}

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
