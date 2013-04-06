using System;
using System.ComponentModel;
using System.Globalization;
using NMoneys.Support;

namespace NMoneys.Exchange
{
	/// <summary>
	/// Represents the rate at which one currency will be exchanged for another.
	/// </summary>
	/// <remarks>The also named "currency pair" is the quotation of the relative value of a currency unit against the unit of another currency.</remarks>
	public class ExchangeRate : IEquatable<ExchangeRate>
	{
		/// <summary>
		/// Initializes an instance of <see cref="ExchangeRate"/> with the provided information.
		/// </summary>
		/// <param name="from">Base currency, the currency from which the conversion is performed.</param>
		/// <param name="to">Quote currency, the currency which the conversion is performed to.</param>
		/// <param name="rate">A non-negative <see cref="decimal"/> instance representing the relative vaue of <paramref name="from"/> against <paramref name="to"/>.</param>
		/// <example>{from}= EUR, {to}= USD, {rate}=1.2500, represented as "EUR/USD 1.2500" means that one euro is exchanged for 1.2500 US dollars.</example>
		/// <exception cref="InvalidEnumArgumentException"><paramref name="from"/> or <paramref name="to"/> are undefined currencies.</exception>
		/// <exception cref="ArgumentException"><paramref name="rate"/> is negative.</exception>
		public ExchangeRate(CurrencyIsoCode from, CurrencyIsoCode to, decimal rate)
		{
			Guard.AgainstArgument("rate", rate < 0, "Non-negative");
			Enumeration.AssertDefined(from);
			Enumeration.AssertDefined(to);

			From = from;
			To = to;
			Rate = rate;
		}

		/// <summary>
		/// Base currency, the currency from which the conversion is performed.
		/// </summary>
		public CurrencyIsoCode From { get; private set; }

		/// <summary>
		/// Quote currency, the currency which the conversion is performed to.
		/// </summary>
		public CurrencyIsoCode To { get; private set; }

		/// <summary>
		/// A <see cref="decimal"/> instance representing the relative vaue of <see cref="From"/> against <see cref="To"/>.
		/// </summary>
		public decimal Rate { get; private set; }

		/// <summary>
		/// Creates a new <see cref="ExchangeRate"/> in which the order of the currencies has been "swapped" that is, the <see cref="From"/> currency
		/// will become the <see cref="To"/> currency and viceversa and the rate, by default, has been inverted (reciprocated).
		/// </summary>
		/// <remarks>The instance on which the method is called remains unchanged.</remarks>
		/// <returns>A new <see cref="ExchangeRate"/> with swapped currencies and inverted rate.</returns>
		public virtual ExchangeRate Invert()
		{
			return new ExchangeRate(To, From, 1m / Rate);
		}

		/// <summary>
		/// Applies the conversion rate to the provided monetary quantity.
		/// </summary>
		/// <remarks>The quanity to be exchanged need to be compatible with the instance of the rate, that is have the same currency as specified in the <see cref="From"/> property.
		/// <para>By default, the application of the rate to the amount is a simple product as specified in <see cref="decimal.op_Multiply"/>. No further manipulations is performed.</para></remarks>
		/// <param name="from">The monetary quantity to be exchanged.</param>
		/// <returns>A new monetary quantity which has the currency as specified in the <see cref="To"/> and its amount the result of the application of the exchange rate to its previous amount.</returns>
		/// <exception cref="DifferentCurrencyException">The rate cannot be applied to <paramref name="from"/>.</exception>
		public virtual Money Apply(Money from)
		{
			assertCompatibility(from.CurrencyCode);

			return new Money(from.Amount * Rate, To);
		}

		/// <summary>
		/// Asserts whether a currency is compatible with this exchange rate, that is, is the same as <see cref="From"/>.
		/// </summary>
		/// <param name="from">The currency to check its compatibility againsts the base currency.</param>
		/// <exception cref="DifferentCurrencyException"><paramref name="from"/> is not compatible.</exception>
		private void assertCompatibility(CurrencyIsoCode from)
		{
			if (from != From) throw new DifferentCurrencyException(From.AlphabeticCode(), from.AlphabeticCode());
		}

		/// <summary>
		/// Converts a exchange rate to its equivalent string representation.
		/// </summary>
		/// <returns>The string representation of the value of this instance, consisting of the three letter code of the base currency, a forward slash <c>/</c>,
		/// the three letter code of the quote currency and the rate formatted as per the rules of the invariant culture.</returns>
		public override string ToString()
		{
			return string.Format("{0}/{1} {2}", From, To, Rate.ToString(CultureInfo.InvariantCulture));
		}

		/// <summary>
		/// Converts the string representation of a exhange rate to its <see cref="ExchangeRate"/> representation.
		/// </summary>
		/// <param name="rateRepresentation">A string containing a rate to convert.</param>
		/// <returns>A <see cref="ExchangeRate"/> instance equivalent to the rate represented by <paramref name="rateRepresentation"/>.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="rateRepresentation"/> is null.</exception>
		/// <exception cref="FormatException"><paramref name="rateRepresentation"/> is not in the correct format.</exception>
		public static ExchangeRate Parse(string rateRepresentation)
		{
			Guard.AgainstNullArgument("rate", rateRepresentation);

			try
			{
				string from = rateRepresentation.Substring(0, 3),
					to = rateRepresentation.Substring(4, 3),
					rate = rateRepresentation.Substring(8);

				return new ExchangeRate(
					Currency.Code.Parse(from),
					Currency.Code.Parse(to),
					decimal.Parse(rate, CultureInfo.InvariantCulture));
			}
			catch (Exception ex)
			{
				throw new FormatException("Input string was not in a correct format: {three_letter_isocode}/{three_letter_isocode} {rate}", ex);
			}
		}

		/// <summary>
		/// Creates an instance of an identity exchange rate with the parameters provided.
		/// </summary>
		/// <remarks>An identity exchange rate is one which rate is 1, as such applying it to some monetary quantity merely changes the currency while leaving its amount unchanged.</remarks>
		/// <param name="from">Base currency, the currency from which the conversion is performed.</param>
		/// <param name="to">Quote currency, the currency which the conversion is performed to.</param>
		/// <returns>An identity exchange rate for the currencies provided.</returns>
		public static ExchangeRate Identity(CurrencyIsoCode from, CurrencyIsoCode to)
		{
			return new ExchangeRate(from, to, decimal.One);
		}

		/// <summary>
		/// Creates an instance of an identity exchange rate with the parameters provided.
		/// </summary>
		/// <remarks>An identity exchange rate is one which rate is 1, as such applying it to some monetary quantity merely changes the currency while leaving its amount unchanged.</remarks>
		/// <param name="single">This currency will become  both the base currency, and the quote currency.</param>
		/// <returns>An identity exchange rate for the currency provided.</returns>
		public static ExchangeRate Identity(CurrencyIsoCode single)
		{
			return new ExchangeRate(single, single, decimal.One);
		}

		#region equality

		/// <summary>
		/// Indicates whether the current exchange rate is equal to another exchange rate.
		/// </summary>
		/// <returns>
		/// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
		/// </returns>
		/// <param name="other">An object to compare with this rate.</param>
		public bool Equals(ExchangeRate other)
		{
			if (ReferenceEquals(null, other)) return false;
			if (ReferenceEquals(this, other)) return true;
			return Equals(other.From, From) && Equals(other.To, To) && other.Rate == Rate;
		}

		/// <summary>
		/// Determines whether the specified <see cref="object"/> is equal to the current <see cref="ExchangeRate"/>.
		/// </summary>
		/// <returns>
		/// true if the specified <see cref="object"/> is equal to the current <see cref="ExchangeRate"/>; otherwise, false.
		/// </returns>
		/// <param name="obj">The <see cref="object"/> to compare with the current <see cref="ExchangeRate"/>.</param>
		/// <exception cref="NullReferenceException">The <paramref name="obj"/> parameter is null.</exception>
		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != typeof (ExchangeRate)) return false;
			return Equals((ExchangeRate) obj);
		}

		/// <summary>
		/// Serves as a hash function for a particular type. 
		/// </summary>
		/// <returns>
		/// A hash code for the current <see cref="ExchangeRate"/>.
		/// </returns>
		public override int GetHashCode()
		{
			unchecked
			{
				int result = From.GetHashCode();
				result = (result*397) ^ To.GetHashCode();
				result = (result*397) ^ Rate.GetHashCode();
				return result;
			}
		}

		/// <summary>
		/// Returns a value indicating whether two instances of <see cref="ExchangeRate"/> are equal.
		/// </summary>
		/// <param name="left">The first value to compare.</param>
		/// <param name="right">The second value to compare.</param>
		/// <returns>true if <paramref name="left"/> and <paramref name="right"/> are equal; otherwise, false.</returns>
		public static bool operator ==(ExchangeRate left, ExchangeRate right)
		{
			return Equals(left, right);
		}

		/// <summary>
		/// Returns a value indicating whether two instances of <see cref="ExchangeRate"/> are not equal.
		/// </summary>
		/// <param name="left">The first value to compare.</param>
		/// <param name="right">The second value to compare.</param>
		/// <returns>true if <paramref name="left"/> and <paramref name="right"/> are not equal; otherwise, false.</returns>
		public static bool operator !=(ExchangeRate left, ExchangeRate right)
		{
			return !Equals(left, right);
		}

		#endregion
	}
}