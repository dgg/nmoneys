using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using NMoneys.Support;

namespace NMoneys
{
	[Serializable]
	[XmlRoot(Namespace = Serialization.Data.NAMESPACE, ElementName = Serialization.Data.Money.ROOT_NAME, DataType = Serialization.Data.Money.DATA_TYPE, IsNullable = false)]
	public struct Money : IEquatable<Money>, IComparable, IComparable<Money>, ICloneable, ISerializable, IXmlSerializable
	{
		#region .ctor

		public Money(decimal amount) : this(amount, CurrencyIsoCode.XXX) { }

		public Money(decimal amount, CurrencyIsoCode currency)
			: this()
		{
			setAllFields(amount, currency);
		}

		public Money(decimal amount, Currency currency) : this(amount, currency.IsoCode) { }

		public Money(decimal amount, string threeLetterIsoCode) : this(amount, Enumeration.Parse<CurrencyIsoCode>(threeLetterIsoCode)) { }

		public Money(Money money)
			: this()
		{
			setAllFields(money.Amount, money.CurrencyCode);
		}

		/// <summary>
		/// Initializes a new instace of <see cref="Money"/> with serialized data
		/// </summary>
		/// <param name="info">The <see cref="SerializationInfo"/> that holds the serialized object data about the <see cref="Money"/>.</param>
		/// <param name="context">The <see cref="StreamingContext"/> that contains contextual information about the source or destination.</param>
		private Money(SerializationInfo info, StreamingContext context) :
			this(info.GetDecimal(Serialization.Data.Money.AMOUNT), (CurrencyIsoCode)info.GetValue(Serialization.Data.Money.CURRENCY, typeof(CurrencyIsoCode))) { }

		private void setAllFields(decimal amount, CurrencyIsoCode currency)
		{
			Enumeration.AssertDefined(currency);
			Amount = amount;
			CurrencyCode = currency;
		}

		#endregion

		#region creation methods

		public static Money ForCurrentCulture(decimal amount)
		{
			return ForCulture(amount, CultureInfo.CurrentCulture);
		}

		public static Money ForCulture(decimal amount, CultureInfo culture)
		{
			Guard.AgainstNullArgument("culture", culture);
			return new Money(amount, Currency.Get(culture));
		}

		#endregion

		//[XmlIgnore]
		public CurrencyIsoCode CurrencyCode { get; private set; }
		//[XmlIgnore]
		public decimal Amount { get; private set; }

		#region formatting

		/// <summary>
		/// Converts the numeric value of the <see cref="Amount"/> to its equivalent string representation using an instance of the <see cref="Currency"/>
		/// identified by <see cref="CurrencyCode"/> for culture-specific format information.
		/// </summary>
		/// <remarks>The return value is formatted with the currency numeric format specifier ("C").</remarks>
		/// <returns>The string representation of the value of this instance as specified by the <c>"Currency"</c> format specifier.</returns>
		public override string ToString()
		{
			return ToString("C");
		}

		/// <summary>
		/// Converts the numeric value of the <see cref="Amount"/> to its equivalent string representation using an instance of the <see cref="Currency"/>
		/// identified by <see cref="CurrencyCode"/> for culture-specific format information.
		/// </summary>
		/// <param name="format">A numeric format string</param>
		/// <returns>The string representation of the value of this instance as specified by the format specifier and an instance of the <see cref="Currency"/>
		/// identified by <see cref="CurrencyCode"/> as the provider.</returns>
		public string ToString(string format)
		{
			Currency currency = Currency.Get(CurrencyCode);
			return Amount.ToString(format, currency);
		}


		/// <summary>
		/// Converts the numeric value of the <see cref="Amount"/> to its equivalent string representation using <paramref name="provider"/>
		/// for culture-specific format information.
		/// </summary>
		/// <param name="provider">An object that supplies culture-specific formatting information.</param>
		/// <returns>The string representation of the value of this instance as specified by the<c>"Currency"</c> format specifier and 
		/// <paramref name="provider>"/>.</returns>
		public string ToString(IFormatProvider provider)
		{
			return ToString("C", provider);
		}

		/// <summary>
		/// Converts the numeric value of the <see cref="Amount"/> to its equivalent string representation using the specified <paramref name="format"/>
		/// and culture-specific format information.
		/// </summary>
		/// <param name="format">A numeric format string</param>
		/// <param name="provider">An object that supplies culture-specific formatting information.</param>
		/// <returns>The string representation of the value of this instance as specified by <paramref name="format"/>the<c>"Currency"</c> format specifier and 
		/// and <paramref name="provider>"/>.</returns>
		public string ToString(string format, IFormatProvider provider)
		{
			return Amount.ToString(format, provider);
		}

		/// <summary>
		/// Replaces the format item in a specified <code>string</code> with information from the <see cref="Currency"/>
		/// identified by the instance's <see cref="CurrencyCode"/>.
		/// An instance of the <see cref="Currency"/> identified by <see cref="CurrencyCode"/> will be supplying culture-specific formatting information.
		/// </summary>
		/// <remarks>
		/// The following table describes the tokens that will be replaced in the <paramref name="format"/>:
		/// <list type="table">
		/// <listheader>
		/// <term>Token</term>
		/// <description>Description</description>
		/// </listheader>
		/// <item>
		/// <term>{0}</term>
		/// <description>This token represents the <see cref="Amount"/> of the current instance.</description>
		/// </item>
		/// <item>
		/// <term>{1}</term>
		/// <description>This token represents the <see cref="Currency.Symbol"/> for the currency of the current instance</description>
		/// </item>
		/// <item>
		/// <term>{2}</term>
		/// <description>This token represents the <see cref="Currency.IsoCode"/> for the currency of the current instance</description>
		/// </item>
		/// <item>
		/// <term>{3}</term>
		/// <description>This token represents the <see cref="Currency.EnglishName"/> for the currency of the current instance</description>
		/// </item>
		/// <item>
		/// <term>{4}</term>
		/// <description>This token represents the <see cref="Currency.NativeName"/> for the currency of the current instance</description>
		/// </item>
		/// </list>
		/// </remarks>
		/// <param name="format">A composite format string that can contain tokens to be replaced by properties of the <see cref="Currency"/>
		/// identified by the instance's <see cref="CurrencyCode"/>.</param>
		/// <returns>A copy of <paramref name="format"/> in which the format items have been replaced by the string representation of the corresponding tokens.</returns>
		public string Format(string format)
		{
			Currency currency = Currency.Get(CurrencyCode);
			return string.Format(currency, format, Amount, currency.Symbol, currency.IsoCode, currency.EnglishName, currency.NativeName);
		}

		/// <summary>
		/// Replaces the format item in a specified <code>string</code> with information from the <see cref="Currency"/>
		/// identified by the instance's <see cref="CurrencyCode"/>.
		/// The <paramref name="provider"/> will be supplying culture-specific formatting information.
		/// </summary>
		/// <remarks>
		/// The following table describes the tokens that will be replaced in the <paramref name="format"/>:
		/// <list type="table">
		/// <listheader>
		/// <term>Token</term>
		/// <description>Description</description>
		/// </listheader>
		/// <item>
		/// <term>{0}</term>
		/// <description>This token represents the <see cref="Amount"/> of the current instance.</description>
		/// </item>
		/// <item>
		/// <term>{1}</term>
		/// <description>This token represents the <see cref="Currency.Symbol"/> for the currency of the current instance</description>
		/// </item>
		/// <item>
		/// <term>{2}</term>
		/// <description>This token represents the <see cref="Currency.IsoCode"/> for the currency of the current instance</description>
		/// </item>
		/// <item>
		/// <term>{3}</term>
		/// <description>This token represents the <see cref="Currency.EnglishName"/> for the currency of the current instance</description>
		/// </item>
		/// <item>
		/// <term>{4}</term>
		/// <description>This token represents the <see cref="Currency.NativeName"/> for the currency of the current instance</description>
		/// </item>
		/// </list>
		/// </remarks>
		/// <param name="format">A composite format string that can contain tokens to be replaced by properties of the <see cref="Currency"/>
		/// identified by the instance's <see cref="CurrencyCode"/>.</param>
		/// <param name="provider">An object that supplies culture-specific formatting information.</param>
		/// <returns>A copy of <paramref name="format"/> in which the format items have been replaced by the string representation of the corresponding tokens.</returns>
		public string Format(string format, IFormatProvider provider)
		{
			Currency currency = Currency.Get(CurrencyCode);
			return string.Format(provider, format, Amount, currency.Symbol, currency.IsoCode, currency.EnglishName, currency.NativeName);
		}

		#endregion

		#region equality

		public bool Equals(Money other)
		{
			return Equals(other.CurrencyCode, CurrencyCode) && other.Amount == Amount;
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (obj.GetType() != typeof(Money)) return false;
			return Equals((Money)obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				return (CurrencyCode.GetHashCode() * 397) ^ Amount.GetHashCode();
			}
		}

		public static bool operator ==(Money left, Money right)
		{
			return Equals(left, right);
		}

		public static bool operator !=(Money left, Money right)
		{
			return !Equals(left, right);
		}

		#endregion

		#region comparison

		/// <summary>
		/// Compares the current instance with another object of the same type and returns an integer that indicates whether the current instance precedes,
		/// follows, or occurs in the same position in the sort order as the other object.
		/// </summary>
		/// <param name="obj">An object to compare with this instance. </param>
		/// <returns>
		/// A 32-bit signed integer that indicates the relative order of the objects being compared. The return value has these meanings:
		/// <list type="table">
		/// <listheader>
		/// <term>Value</term>
		/// <description>Meaning</description>
		/// </listheader>
		/// <item>
		/// <term>Less than zero</term>
		/// <description>This instance is less than <paramref name="obj"/>.</description>
		/// </item>
		/// <item>
		/// <term>Zero</term>
		/// <description>This instance is equal to <paramref name="obj"/>.</description>
		/// </item>
		/// <item>
		/// <term>Greater than zero</term>
		/// <description>This instance is greater than <paramref name="obj"/>.</description>
		/// </item>
		/// </list>
		/// </returns>
		/// <exception cref="T:System.ArgumentException"><paramref name="obj"/> is not a <see cref="Money"/>.</exception>
		public int CompareTo(object obj)
		{
			if (obj == null)
			{
				return 1;
			}
			if (!(obj is Money))
			{
				throw new ArgumentException("obj", string.Format("Argument must be of type {0}.", typeof(Money).Name));
			}
			return CompareTo((Money)obj);
		}

		/// <summary>
		/// Compares the current <see cref="Amount"/> with the one for another <see cref="Money"/>.
		/// </summary>
		/// <remarks>Both instances must have the same <see cref="CurrencyCode"/> in order to be compared, otherwise an exception will be thrown.</remarks>
		/// <param name="other">An <see cref="Money"/> to compare with this object.</param>
		/// <returns>
		/// A 32-bit signed integer that indicates the relative order of the <c>amounts</c> being compared. The return value has the following meanings: 
		/// <list type="table">
		/// <listheader>
		/// <term>Value</term>
		/// <description>Meaning</description>
		/// </listheader>
		/// <item>
		/// <term>Less than zero</term>
		/// <description>This <see cref="Amount"/> is less than <paramref name="other"/>'s.</description>
		/// </item>
		/// <item>
		/// <term>Zero</term>
		/// <description>This <see cref="Amount"/> is equal to <paramref name="other"/>'s.</description>
		/// </item>
		/// <item>
		/// <term>Greater than zero</term>
		/// <description>This <see cref="Amount"/> is greater than <paramref name="other"/>'s.</description>
		/// </item>
		/// </list>
		/// </returns>
		/// <exception cref="DifferentCurrencyException">If <paramref name="other"/> does not have the same <see cref="CurrencyCode"/>.</exception>
		public int CompareTo(Money other)
		{
			AssertSameCurrency(other);

			return Amount.CompareTo(other.Amount);
		}

		/// <summary>
		/// Returns a value indicating whether a specified <see cref="Money"/> is greater than another specified <see cref="Money"/>.
		/// </summary>
		/// <param name="left">The first value to compare.</param>
		/// <param name="right">The second value to compare.</param>
		/// <returns>true if <paramref name="left"/> is greater than <paramref name="right"/>; otherwise, false.</returns>
		public static bool operator >(Money left, Money right)
		{
			return left.CompareTo(right) > 0;
		}

		/// <summary>
		/// Returns a value indicating whether a specified <see cref="Money"/> is less than another specified <see cref="Money"/>.
		/// </summary>
		/// <param name="left">The first value to compare.</param>
		/// <param name="right">The second value to compare.</param>
		/// <returns>true if <paramref name="left"/> is less than <paramref name="right"/>; otherwise, false.</returns>
		public static bool operator <(Money left, Money right)
		{
			return left.CompareTo(right) < 0;
		}

		/// <summary>
		/// Returns a value indicating whether a specified <see cref="Money"/> is greater than or equal to another specified <see cref="Money"/>.
		/// </summary>
		/// <param name="left">The first value to compare.</param>
		/// <param name="right">The second value to compare.</param>
		/// <returns>true if <paramref name="left"/> is greater than or equal to <paramref name="right"/>; otherwise, false.</returns>
		public static bool operator >=(Money left, Money right)
		{
			return left.CompareTo(right) >= 0;
		}

		/// <summary>
		/// Returns a value indicating whether a specified <see cref="Money"/> is less than or equal to another specified <see cref="Money"/>.
		/// </summary>
		/// <param name="left">The first value to compare.</param>
		/// <param name="right">The second value to compare.</param>
		/// <returns>true if <paramref name="left"/> is less than or equal to <paramref name="right"/>; otherwise, false.</returns>
		public static bool operator <=(Money left, Money right)
		{
			return left.CompareTo(right) <= 0;
		}

		/// <summary>
		/// Returns a value indicating whether the <see cref="Amount"/> is strictly less than <see cref="decimal.Zero"/>.
		/// </summary>
		/// <returns>true if <see cref="Amount"/> is less than <see cref="decimal.Zero"/>; otherwise, false.</returns>
		public bool IsNegative()
		{
			return Amount < decimal.Zero;
		}

		/// <summary>
		/// Returns a value indicating whether the <see cref="Amount"/> is stricly greater than <see cref="decimal.Zero"/>.
		/// </summary>
		/// <returns>true if <see cref="Amount"/> is greater than <see cref="decimal.Zero"/>; otherwise, false.</returns>
		public bool IsPositive()
		{
			return Amount > decimal.Zero;
		}

		/// <summary>
		/// Returns a value indicating whether the <see cref="Amount"/> is stricly equal to<see cref="decimal.Zero"/>.
		/// </summary>
		/// <returns>true if <see cref="Amount"/> is equal to <see cref="decimal.Zero"/>; otherwise, false.</returns>
		public bool IsZero()
		{
			return Amount.Equals(decimal.Zero);
		}

		#endregion

		#region cloning

		/// <summary>
		/// Creates a new object that is a copy of the current instance.
		/// </summary>
		/// <returns>
		/// A new object that is a copy of this instance.
		/// </returns>
		public object Clone()
		{
			return new Money(Amount, CurrencyCode);
		}

		#endregion

		#region currency checking

		/// <summary>
		/// Returns a value indicating whether the <paramref name="money"/> has the same currency as the instance.
		/// </summary>
		/// <param name="money"><see cref="Money"/> instance to check against.</param>
		/// <returns>true if <see cref="CurrencyCode"/> is equal to <paramref name="money"/>'s; otherwise, false.</returns>
		public bool HasSameCurrencyAs(Money money)
		{
			return money.CurrencyCode.Equals(CurrencyCode);
		}

		/// <summary>
		/// Checks whether the <paramref name="money"/> has the same currency as the instance, throwing an exception if that is not the case.
		/// </summary>
		/// <param name="money"><see cref="Money"/> instance to check against.</param>
		/// <exception cref="DifferentCurrencyException"></exception>
		public void AssertSameCurrency(Money money)
		{
			if (!HasSameCurrencyAs(money)) throw new DifferentCurrencyException(CurrencyCode.ToString(), money.CurrencyCode.ToString());
		}

		private static void assertSameCurrency(Money first, Money second)
		{
			if (!first.HasSameCurrencyAs(second))
			{
				throw new DifferentCurrencyException(first.CurrencyCode.ToString(), second.CurrencyCode.ToString());
			}
		}

		#endregion

		#region operators

		/// <summary>
		/// Negates the value of the specified <see cref="Money"/> operand.
		/// </summary>
		/// <param name="money">The value to negate.</param>
		/// <returns>A <see cref="Money"/> with the <see cref="Amount"/> of <paramref name="money"/>, but multiplied by negative one (-1).</returns>
		public static Money operator -(Money money)
		{
			return new Money(-(money.Amount), money.CurrencyCode);
		}

		/// <summary>
		/// Adds two specified <see cref="Money"/> values.
		/// </summary>
		/// <remarks>Both instances must have the same <see cref="CurrencyCode"/> in order to be added, otherwise an exception will be thrown.</remarks>
		/// <param name="first">The first value to add.</param>
		/// <param name="second">The second value to add.</param>
		/// <returns>A <see cref="Money"/> with <see cref="Amount"/> as the sum of <paramref name="first"/> and <paramref name="second"/> amounts
		/// and the same <see cref="CurrencyCode"/> as any of the arguments.</returns>
		/// <exception cref="DifferentCurrencyException">If <paramref name="first"/> does not have the same <see cref="CurrencyCode"/>
		/// as <paramref name="second"/>.</exception>
		/// <exception cref="OverflowException">The <see cref="Amount"/> of the result is less than
		/// <see cref="decimal.MinValue"/> or greater than <see cref="decimal.MaxValue"/>.</exception>
		public static Money operator +(Money first, Money second)
		{
			assertSameCurrency(first, second);
			return new Money(first.Amount + second.Amount, first.CurrencyCode);
		}

		/// <summary>
		/// Substracts one specified <see cref="Money"/> from another.
		/// </summary>
		/// <remarks>Both instances must have the same <see cref="CurrencyCode"/> in order to be substracted, otherwise an exception will be thrown.</remarks>
		/// <param name="first">The minuend.</param>
		/// <param name="second">The subtrahend.</param>
		/// <returns>A <see cref="Money"/> with <see cref="Amount"/> as the result of substracting <paramref name="second"/> from <paramref name="first"/> amounts
		/// and the same <see cref="CurrencyCode"/> as any of the arguments.</returns>
		/// <exception cref="DifferentCurrencyException">If <paramref name="first"/> does not have the same <see cref="CurrencyCode"/>
		/// as <paramref name="second"/>.</exception>
		/// <exception cref="OverflowException">The <see cref="Amount"/> of the result is less than
		/// <see cref="decimal.MinValue"/> or greater than <see cref="decimal.MaxValue"/>.</exception>
		public static Money operator -(Money first, Money second)
		{
			assertSameCurrency(first, second);
			return new Money(first.Amount - second.Amount, first.CurrencyCode);
		}

		#endregion

		#region arithmetic operations

		/// <summary>
		/// Adds two specified <see cref="Money"/> values.
		/// </summary>
		/// <remarks><paramref name="money"/> must have the same <see cref="CurrencyCode"/> as this instance, otherwise an exception will be thrown.</remarks>
		/// <param name="money">The value to add.</param>
		/// <returns>A <see cref="Money"/> with <see cref="Amount"/> as the sum of <paramref name="money"/> amount and this <see cref="Amount"/>
		/// and the same <see cref="CurrencyCode"/> this instance.</returns>
		/// <exception cref="DifferentCurrencyException">If <paramref name="money"/> does not have the same <see cref="CurrencyCode"/> as this instance.</exception>
		/// <exception cref="OverflowException">The <see cref="Amount"/> of the result is less than  <see cref="decimal.MinValue"/> or greater than <see cref="decimal.MaxValue"/>.</exception>
		public Money Plus(Money money)
		{
			return this + money;
		}

		/// <summary>
		/// Substracts one specified <see cref="Money"/> from another.
		/// </summary>
		/// <remarks><paramref name="money"/> must have the same <see cref="CurrencyCode"/> as this instance, otherwise an exception will be thrown.</remarks>
		/// <param name="money">The subtrahend.</param>
		/// <returns>A <see cref="Money"/> with <see cref="Amount"/> as the result of substracting <paramref name="money"/>'s <see cref="Amount"/> from this instance's
		/// and the same <see cref="CurrencyCode"/> as this instance.</returns>
		/// <exception cref="DifferentCurrencyException">If <paramref name="money"/> does not have the same <see cref="CurrencyCode"/> as this instance.</exception>
		/// <exception cref="OverflowException">The <see cref="Amount"/> of the result is less than <see cref="decimal.MinValue"/> or greater than <see cref="decimal.MaxValue"/>.</exception>
		public Money Minus(Money money)
		{
			return this - money;
		}

		/// <summary>
		/// Returns the absolute value of a <see cref="Money"/>.
		/// </summary>
		/// <remarks>The absolute value of a <see cref="Money"/> is another <see cref="Money"/> which <see cref="Amount"/> is the numeric value without its sign.
		/// For example, the absolute value of both $1.2 and ($1.2) is $1.2.</remarks>
		/// <returns>A <see cref="Money"/> with <see cref="Amount"/> as the absolute value of this instance's.</returns>
		public Money Abs()
		{
			return new Money(Math.Abs(Amount), CurrencyCode);
		}

		/// <summary>
		/// Returns the result of multiplying this instance of <see cref="Money"/> by negative one.
		/// </summary>
		/// <returns>A <see cref="Money"/> with the <see cref="Amount"/> of this instance's, but the opposite sign.</returns>
		public Money Negate()
		{
			return new Money(decimal.Negate(Amount), CurrencyCode);
		}

		/// <summary>
		/// Truncates the <see cref="Amount"/> to the number of significant decimal digits specified by the <see cref="Currency"/>
		/// identified this <see cref="CurrencyCode"/>.
		/// </summary>
		/// <returns>A <see cref="Money"/> with the <see cref="Amount"/> truncated to the significant number of decimal digits of its currency.</returns>
		public Money TruncateToSignificantDecimalDigits()
		{
			Currency currency = Currency.Get(CurrencyCode);
			return new Money(truncateToSignificantDecimalDigits(currency.SignificantDecimalDigits), CurrencyCode);
		}

		/// <summary>
		/// Truncates the <see cref="Amount"/> to the number of significant decimal digits specified by <paramref name="numberFormat"/>.
		/// </summary>
		/// <param name="numberFormat">Specifies the number of significant decimal digits.</param>
		/// <returns>A <see cref="Money"/> with the <see cref="Amount"/> truncated to the significant number of decimal digits of <paramref name="numberFormat"/>.</returns>
		/// <exception cref="ArgumentNullException">If <paramref name="numberFormat"/> is null.</exception>
		public Money TruncateToSignificantDecimalDigits(NumberFormatInfo numberFormat)
		{
			Guard.AgainstNullArgument("numberFormat", numberFormat);
			return new Money(truncateToSignificantDecimalDigits(numberFormat.CurrencyDecimalDigits), CurrencyCode);
		}

		private static readonly int[] _cents = new[] { 1, 10, 100, 1000 };
		private decimal truncateToSignificantDecimalDigits(int decimalDigits)
		{
			// 10^significantDecimalDigits
			int centFactor = _cents[decimalDigits];

			decimal truncatedAmount = (decimal)((long)Math.Truncate(Amount * centFactor)) / centFactor;
			return truncatedAmount;
		}

		
		/// <summary>
		/// Returns the integral digits of this instance of <see cref="Money"/>; any fractional digits are discarded.
		/// </summary>
		/// <remarks>This method rounds <see cref="Amount"/> toward zero, to the nearest whole number, which corresponds to discarding any digits after the decimal point.</remarks>
		/// <returns>A <see cref="Money"/> with <see cref="Amount"/> the result of <see cref="Amount"/> rounded toward zero, to the nearest whole number.</returns>
		public Money Truncate()
		{
			return new Money(decimal.Truncate(Amount), CurrencyCode);
		}

		/// <summary>
		/// Rounds <see cref="Amount"/> to the nearest integer.
		/// </summary>
		/// <returns>
		/// A <see cref="Money"/> with an integer <see cref="Amount"/> that is nearest to the old value.
		/// <para>If <see cref="Amount"/> is halfway between two integers, one of which is even and the other odd, the even number is chosen.</para>
		/// </returns>
		public Money RoundToNearestInt()
		{
			return new Money(decimal.Round(Amount), CurrencyCode);
		}

		/// <summary>
		/// Rounds <see cref="Amount"/> to the nearest integer. A parameter specifies how to round the value if it is midway between two other numbers.
		/// </summary>
		/// <param name="mode">A value that specifies how to round <see cref="Amount"/> if it is midway between two other numbers.</param>
		/// <returns>
		/// A <see cref="Money"/> with an integer <see cref="Amount"/> that is nearest to the previous <see cref="Amount"/> value.
		/// <para>If <see cref="Amount"/> is halfway between two numbers, one of which is even and the other odd, the mode parameter determines which of the two numbers is chosen.</para>
		/// </returns>
		public Money RoundToNearestInt(MidpointRounding mode)
		{
			return new Money(decimal.Round(Amount, mode), CurrencyCode);
		}

		/// <summary>
		/// Rounds <see cref="Amount"/> to a the number of decimal places specified by the <see cref="Currency"/> identified by <see cref="CurrencyCode"/>.
		/// </summary>
		/// <returns>
		/// A <see cref="Money"/> with the <see cref="Amount"/> equivalent to previous <see cref="Amount"/> rounded to the number of decimal places specified by its <see cref="CurrencyCode"/>.
		/// </returns>
		/// <remarks>
		/// When <see cref="Amount"/> is exactly halfway between two rounded values, the resultant <see cref="Amount"/> is the rounded value that has an even digit in the far right decimal position. For example, when rounded to two <see cref="Amount"/>s, the value <c>2.345</c> becomes <c>2.34</c> and the value <c>2.355</c> becomes 2.36. This process is known as rounding toward even, or rounding to nearest.
		/// <para>The behavior of this method follows IEEE Standard 754, section 4. This kind of rounding is sometimes called rounding to nearest or banker's rounding.</para>
		/// </remarks>
		public Money Round()
		{
			Currency currency = Currency.Get(CurrencyCode);
			return Round(currency.SignificantDecimalDigits);
		}

		/// <summary>
		/// Rounds <see cref="Amount"/> to a precision specified by the <see cref="Currency"/> identified by <see cref="CurrencyCode"/>.
		/// <paramref name="mode"/> specifies how to round the value if it is midway between two other numbers.
		/// </summary>
		/// <param name="mode">A value that specifies how to round <see cref="Amount"/> if it is midway between two other numbers.</param>
		/// <returns>
		/// A <see cref="Money"/> with <see cref="Amount"/> that is nearest to the previous <see cref="Amount"/> with a precision equal to <see cref="Currency.SignificantDecimalDigits"/>.
		/// If <see cref="Amount"/> is halfway between two numbers, one of which is even and the other odd, the mode parameter determines which of the two numbers is chosen.
		/// If the precision of <see cref="Amount"/> is less than <see cref="Currency.SignificantDecimalDigits"/>, <see cref="Amount"/> remains unchanged.
		/// </returns>
		/// <remarks>
		/// The <see cref="Currency.SignificantDecimalDigits"/> specifies the number of significant decimal places in the return value and ranges from 0 to 28.
		/// <para>If <see cref="Currency.SignificantDecimalDigits"/> is zero, an integer is returned.</para>
		/// <para>The behavior of this method follows IEEE Standard 754, section 4. This kind of rounding is sometimes called rounding to nearest or banker's rounding.</para>
		/// <para>If <see cref="Currency.SignificantDecimalDigits"/> is zero, this kind of rounding is sometimes called rounding toward zero.</para>
		/// </remarks>
		public Money Round(MidpointRounding mode)
		{
			Currency currency = Currency.Get(CurrencyCode);
			return Round(currency.SignificantDecimalDigits, mode);
		}

		/// <summary>
		/// Rounds <see cref="Amount"/> to a specified number of decimal places.
		/// </summary>
		/// <param name="decimals">A value from 0 to 28 that specifies the number of decimal places to round to.</param>
		/// <returns>
		/// A <see cref="Money"/> with <see cref="Amount"/> rounded to decimals number of decimal places.
		/// </returns>
		/// <remarks>
		/// The behavior of this method follows IEEE Standard 754, section 4. This kind of rounding is sometimes called rounding to nearest or banker's rounding.
		/// </remarks>
		public Money Round(int decimals)
		{
			return new Money(decimal.Round(Amount, decimals), CurrencyCode);
		}

		/// <summary>
		/// Rounds <see cref="Amount"/> to a specified precision. A parameter specifies how to round the value if it is midway between two other numbers.
		/// </summary>
		/// <param name="decimals">A value from 0 to 28 that specifies the number of decimal places to round to.</param>
		/// <param name="mode">A value that specifies how to round <see cref="Amount"/> if it is midway between two other numbers.</param>
		/// <returns>
		/// A <see cref="Money"/> with <see cref="Amount"/> that is nearest to the previous <see cref="Amount"/> with a precision equal to the <paramref name="decimals"/> parameter.
		/// <para>If previous <see cref="Amount"/> is halfway between two numbers, one of which is even and the other odd, the mode parameter determines which of the two numbers is returned.</para>
		/// <para>If the precision of d  is less than decimals, d is returned unchanged.</para>
		/// </returns>
		/// <remarks>
		/// The <paramref name="decimals"/> specifies the number of significant decimal places in the return value and ranges from 0 to 28.
		/// <para>If <paramref name="decimals"/> is zero, an integer is returned.</para>
		/// <para>The behavior of this method follows IEEE Standard 754, section 4. This kind of rounding is sometimes called rounding to nearest or banker's rounding.</para>
		/// <para>If <paramref name="decimals"/> is zero, this kind of rounding is sometimes called rounding toward zero.</para>
		/// </remarks>
		public Money Round(int decimals, MidpointRounding mode)
		{
			return new Money(decimal.Round(Amount, decimals, mode), CurrencyCode);
		}

		/// <summary>
		/// Rounds <see cref="Amount"/> to the closest integer toward negative infinity.
		/// </summary>
		/// <returns>A <see cref="Money"/> which <see cref="Amount"/> is:
		/// <para>if <see cref="Amount"/> has a fractional part, the next whole decimal number toward negative infinity that is less than <see cref="Amount"/>.</para>
		///<para>-or-</para>
		///<para>If <see cref="Amount"/> doesn't have a fractional part, is remains unchanged.</para></returns>
		public Money Floor()
		{
			return new Money(decimal.Floor(Amount), CurrencyCode);
		}

		/// <summary>
		/// Performs the arithmetial operation <paramref name="binaryOperation"/> on <see cref="Amount"/>.
		/// </summary>
		/// <param name="operand">Value which <see cref="Amount"/> serves as the second argument to <paramref name="binaryOperation"/>.</param>
		/// <param name="binaryOperation">Arithmetical operation to perform.</param>
		/// <returns>A <see cref="Money"/> with <see cref="Amount"/> as the result of applying <paramref name="binaryOperation"/> to he old amount and
		/// <paramref name="operand"/>'s amount.</returns>
		/// <exception cref="ArgumentNullException">The <paramref name="binaryOperation"/> is null.</exception>
		public Money Perform(Money operand, Func<decimal, decimal, decimal> binaryOperation)
		{
			Guard.AgainstNullArgument("binaryOperation", binaryOperation);

			AssertSameCurrency(operand);
			return new Money(binaryOperation(Amount, operand.Amount), CurrencyCode);
		}

		/// <summary>
		/// Performs the arithmetial operation <paramref name="unaryOperation"/> on <see cref="Amount"/>.
		/// </summary>
		/// <param name="unaryOperation">Arithmetical operation to perform.</param>
		/// <returns>a <see cref="Money"/> with <see cref="Amount"/> as the result of applying <paramref name="unaryOperation"/> to the previous <see cref="Amount"/>.</returns>
		/// <exception cref="ArgumentNullException">The <paramref name="unaryOperation"/> is null.</exception>
		public Money Perform(Func<decimal, decimal> unaryOperation)
		{
			Guard.AgainstNullArgument("unaryOperation", unaryOperation);

			return new Money(unaryOperation(Amount), CurrencyCode);
		}

		#endregion

		#region serialization

		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue(Serialization.Data.Money.AMOUNT, Amount);
			info.AddValue(Serialization.Data.Money.CURRENCY, CurrencyCode, typeof(Currency));
		}

		[Obsolete("deprecated, use SchemaProviders instead")]
		XmlSchema IXmlSerializable.GetSchema()
		{
			return null;
		}

		public static XmlSchemaComplexType GetSchema(XmlSchemaSet xs)
		{
			XmlSchemaComplexType complex = null;
			XmlSerializer schemaSerializer = new XmlSerializer(typeof(XmlSchema));
			using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(Serialization.Data.ResourceName))
			{
				if (stream != null)
				{
					try
					{
						XmlSchema schema = (XmlSchema)schemaSerializer.Deserialize(new XmlTextReader(stream));
						xs.Add(schema);
						XmlQualifiedName name = new XmlQualifiedName(Serialization.Data.Money.DATA_TYPE, Serialization.Data.NAMESPACE);
						complex = (XmlSchemaComplexType)schema.SchemaTypes[name];
					}
					finally
					{
						stream.Close();
					}
				}
			}
			return complex;
		}

		public void ReadXml(XmlReader reader)
		{
			reader.ReadStartElement();
			Amount = reader.ReadElementContentAsDecimal(Serialization.Data.Money.AMOUNT, Serialization.Data.NAMESPACE);
			CurrencyIsoCode isoCode = Currency.ReadXmlData(reader);
			CurrencyCode = isoCode;
			reader.ReadEndElement();
		}

		public void WriteXml(XmlWriter writer)
		{
			writer.WriteStartElement(Serialization.Data.Money.AMOUNT, Serialization.Data.NAMESPACE);
			writer.WriteValue(Amount);
			writer.WriteEndElement();
			writer.WriteStartElement(Serialization.Data.Currency.ROOT_NAME);
			Currency currency = Currency.Get(CurrencyCode);
			currency.WriteXml(writer);
			writer.WriteEndElement();
		}

		#endregion

		#region parsing

		public static Money Parse(string s, Currency currency)
		{
			return Parse(s, NumberStyles.Currency, currency);
		}

		public static Money Parse(string s, NumberStyles style, Currency currency)
		{
			decimal amount = decimal.Parse(s, style, currency);

			return new Money(amount, currency);
		}

		public static bool TryParse(string s, Currency currency, out Money? money)
		{
			return TryParse(s, NumberStyles.Currency, currency, out money);
		}

		public static bool TryParse(string s, NumberStyles style, Currency currency, out Money? money)
		{
			decimal amount;
			bool result = decimal.TryParse(s, style, currency, out amount);
			money = result ? new Money(amount, currency) : (Money?)null;
			return result;
		}

		#endregion
	}
}
