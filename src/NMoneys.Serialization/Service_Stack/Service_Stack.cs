using System.Globalization;
using System.Linq;
using ServiceStack.Text;

namespace NMoneys.Serialization.Service_Stack
{
	#region serialization/deserialization functions

	/// <summary>
	/// Converts a <see cref="Money"/> instance to and from JSON in the canonical way.
	/// </summary>
	/// <remarks>The canonical way is the one implemented in NMoneys itself, with an <c>Amount</c>
	/// numeric property and a <c>Currency</c> object with a three-letter code <c>IsoCode"</c> property.
	/// <para>
	/// Property casing must be configured apart from this serializer using, for instance,
	/// <see cref="JsConfig.EmitCamelCaseNames"/>
	/// </para>
	/// </remarks>
	/// <example>
	/// <code>{"Amount" : 123.4, "Currency" : {"IsoCode" : "XXX"}}</code>
	/// <code>{"amount" : 123.4, "currency" : {"isoCode" : "XXX"}}</code>
	/// </example>
	public static class CanonicalMoneySerializer
	{
		public static string Serialize(Money instance)
		{
			var surrogate = new CanonicalSurrogate(instance);
			return JsonSerializer.SerializeToString(surrogate);
		}

		public static Money Deserialize(string json)
		{
			var surrogate = JsonSerializer.DeserializeFromString<CanonicalSurrogate>(json);
			return surrogate.ToMoney();
		}
	}

	/// <summary>
	/// Converts a <see cref="Money"/> instance to and from JSON in a default (standard) way.
	/// </summary>
	/// <remarks>The default (standard) way is the one that a normal serializer would do for a money instance,
	/// with an <c>Amount</c> numeric property and an alphabetical <c>Currency</c> code.
	/// <para>
	/// Property casing must be configured apart from this serializer using, for instance,
	/// <see cref="JsConfig.EmitCamelCaseNames"/>
	/// </para>
	/// </remarks>
	/// <example>
	/// <code>{"Amount" : 123.4, "Currency" : "XXX"}</code>
	/// </example>
	public static class DefaultMoneySerializer
	{
		public static string Serialize(Money instance)
		{
			var surrogate = new DefaultSurrogate(instance);
			return JsonSerializer.SerializeToString(surrogate);
		}

		public static Money Deserialize(string json)
		{
			var proxy = JsonSerializer.DeserializeFromString<DefaultSurrogate>(json);
			return proxy.ToMoney();
		}

		/// <summary>
		/// Converts a <see cref="Money"/> instance to and from JSON in a default (standard) way.
		/// </summary>
		/// <remarks>The default (standard) way is the one that a normal serializer would do for a money instance,
		/// with an <c>Amount</c> numeric property and a numerical <c>Currency</c> code.
		/// <para>
		/// Property casing must be configured apart from this serializer using, for instance,
		/// <see cref="JsConfig.EmitCamelCaseNames"/>
		/// </para>
		/// </remarks>
		/// <example>
		/// <code>{"Amount" : 123.4, "Currency" : 999}</code>
		/// </example>
		public static class Numeric
		{
			public static string Serialize(Money instance)
			{
				var surrogate = new NumericSurrogate(instance);
				return JsonSerializer.SerializeToString(surrogate);
			}

			public static Money Deserialize(string json)
			{
				var proxy = JsonSerializer.DeserializeFromString<NumericSurrogate>(json);
				return proxy.ToMoney();
			}
		}
	}

	/// <summary>
	/// Converts a <see cref="Money"/> instance from JSON numeric quantities.
	/// </summary>
	/// <remarks>To be used in integration scenarios when no currency can be provided and a default
	/// currency can be used.
	/// <para>
	/// Property casing must be configured apart from this serializer using, for instance,
	/// <see cref="JsConfig.EmitCamelCaseNames"/>
	/// </para>
	/// </remarks>
	/// <example>
	/// <code>{"Amount" : 123.4}</code>
	/// <code>{"any_other_name" : 123.4, ...}</code>
	/// </example>
	public static class CurrencyLessMoneySerializer
	{
		public static Money Deserialize(string json)
		{
			JsonObject parsed = JsonObject.Parse(json);
			string instanceValue = parsed.Values.First();
			decimal amount = instanceValue == null
				? JsonSerializer.DeserializeFromString<decimal>(json)
				: decimal.Parse(instanceValue, NumberStyles.Number, CultureInfo.InvariantCulture);

			return new Money(amount);
		}
	}

	#endregion

	#region surrogates

	internal interface ISurrogate
	{
		decimal Amount { get; set; }
		Money ToMoney();
	}

	internal class CanonicalSurrogate : ISurrogate
	{
		public CanonicalSurrogate(Money from)
		{
			Amount = from.Amount;
			Currency = new CurrencySurrogate(from.CurrencyCode);
		}

		public decimal Amount { get; set; }
		public CurrencySurrogate Currency { get; set; }

		public class CurrencySurrogate
		{
			public CurrencySurrogate() { }

			public CurrencySurrogate(CurrencyIsoCode @from)
			{
				IsoCode = @from;
			}

			public CurrencyIsoCode IsoCode { get; set; }
		}

		public Money ToMoney()
		{
			return new Money(Amount, Currency.IsoCode);
		}
	}

	internal class DefaultSurrogate : ISurrogate
	{
		public DefaultSurrogate(Money from)
		{
			Amount = from.Amount;
			Currency = from.CurrencyCode;
		}

		public decimal Amount { get; set; }
		public CurrencyIsoCode Currency { get; set; }

		public Money ToMoney()
		{
			return new Money(Amount, Currency);
		}
	}

	internal class NumericSurrogate : ISurrogate
	{
		public NumericSurrogate(Money from)
		{
			Amount = from.Amount;
			Currency = from.CurrencyCode.NumericCode();
		}
		public decimal Amount { get; set; }
		public short Currency { get; set; }

		public Money ToMoney()
		{
			return new Money(Amount, NMoneys.Currency.Code.Cast(Currency));
		}
	}

	#endregion

}
