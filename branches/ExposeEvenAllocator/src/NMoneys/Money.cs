using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using NMoneys.Allocation;
using NMoneys.Extensions;
using NMoneys.Support;

namespace NMoneys
{
	/// <summary>
	/// A monetary quantity in a given currency
	/// </summary>
	[Serializable]
	[XmlRoot(Namespace = Serialization.Data.NAMESPACE, ElementName = Serialization.Data.Money.ROOT_NAME, DataType = Serialization.Data.Money.DATA_TYPE, IsNullable = false)]
	public struct Money : IEquatable<Money>, IComparable, IComparable<Money>, ICloneable, ISerializable, IXmlSerializable
	{
		#region .ctor

		/// <summary>
		/// Creates an instance of <see cref="Money"/> with the <paramref name="amount"/> provided
		/// and the unspecified (<see cref="CurrencyIsoCode.XXX"/>) currency.
		/// </summary>
		/// <param name="amount">The <see cref="Amount"/> of the monetary quantity.</param>
		public Money(decimal amount) : this(amount, CurrencyIsoCode.XXX) { }

		/// <summary>
		/// Creates an instance of <see cref="Money"/> with the <paramref name="amount"/> provided
		/// and the specified <paramref name="currency"/>.
		/// </summary>
		/// <param name="amount">The <see cref="Amount"/> of the monetary quantity.</param>
		/// <param name="currency">The <see cref="CurrencyCode"/> of the monetary quantity.</param>
		public Money(decimal amount, CurrencyIsoCode currency)
			: this()
		{
			setAllFields(amount, currency);
		}

		internal Money(decimal amount, Currency currency, ObsoleteCurrencyEventBehavior eventBehavior)
			: this()
		{
			setAllFields(amount, currency.IsoCode, eventBehavior);
		}

		/// <summary>
		/// Creates an instance of <see cref="Money"/> with the <paramref name="amount"/> provided
		/// and the specified <paramref name="currency"/>.
		/// </summary>
		/// <param name="amount">The <see cref="Amount"/> of the monetary quantity.</param>
		/// <param name="currency">The incarnation of the <see cref="CurrencyCode"/>.</param>
		/// <exception cref="ArgumentNullException">If <paramref name="currency"/> is null.</exception>
		public Money(decimal amount, Currency currency) : this(amount, Guard.AgainstNullArgument("currency", currency, c => c.IsoCode)) { }

		/// <summary>
		/// Creates an instance of <see cref="Money"/> with the <paramref name="amount"/> provided
		/// and <see cref="Currency"/> the specified <paramref name="threeLetterIsoCode"/>.
		/// </summary>
		/// <param name="amount">The <see cref="Amount"/> of the monetary quantity.</param>
		/// <param name="threeLetterIsoCode">Textual representation of the ISO 4217 <see cref="CurrencyCode"/>.</param>
		public Money(decimal amount, string threeLetterIsoCode) : this(amount, Currency.Code.ParseArgument(threeLetterIsoCode, "threeLetterIsoCode")) { }

		/// <summary>
		/// Creates an instance of <see cref="Money"/> based on the information provided by <paramref name="money"/>.
		/// </summary>
		/// <param name="money">A <see cref="Money"/> instance from which capture the values from.</param>
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
			setAllFields(amount, currency, ObsoleteCurrencyEventBehavior.Raise);
		}

		private void setAllFields(decimal amount, CurrencyIsoCode currency, ObsoleteCurrencyEventBehavior eventBehavior)
		{
			Enumeration.AssertDefined(currency);
			if (eventBehavior == ObsoleteCurrencyEventBehavior.Raise) Currency.RaiseIfObsolete(currency);

			Amount = amount;
			CurrencyCode = currency;
		}

		#endregion

		#region creation methods

		/// <summary>
		/// Creates an <see cref="Money"/> instance with the specified <see cref="Amount"/> and the <see cref="CurrencyCode"/> from the region
		/// associated with the current culture.
		/// </summary>
		/// <remarks>The current culture is calculated as the value of <see cref="CultureInfo.CurrentCulture"/>.
		/// <para>There might be cases that the framework will provide non-standard or out-dated information for
		/// the current culture. In this case it might be possible that an exception is thrown even if the region
		/// corresponding to the current culture can be created.</para>
		/// </remarks>
		/// <param name="amount">The <see cref="Amount"/> of the monetary quantity.</param>
		/// <returns>An instance of <see cref="Money"/> with the <paramref name="amount"/> specified and the currency associated to the current culture.</returns>
		/// /// <exception cref="ArgumentException">The current is either an invariant or custom, or a <see cref="RegionInfo"/> cannot be instantiated from it.</exception>
		/// <exception cref="System.ComponentModel.InvalidEnumArgumentException">The ISO symbol associated to the current culture does not exist in the <see cref="CurrencyIsoCode"/> enumeration.</exception>
		/// <exception cref="MisconfiguredCurrencyException">The currency associated to the current culture has not been properly configured by the library implementor. Please, log a issue.</exception>
		public static Money ForCurrentCulture(decimal amount)
		{
			return ForCulture(amount, CultureInfo.CurrentCulture);
		}

		/// <summary>
		/// Creates an <see cref="Money"/> instance with the specified <see cref="Amount"/> and the <see cref="CurrencyCode"/> from the region
		/// associated with the provided <paramref name="culture"/>.
		/// </summary>
		/// <remarks>There might be cases that the framework will provided non-standard or out-dated information for
		/// the given <paramref name="culture"/>. In this case it might be possible that an exception is thrown even if the region
		/// corresponding to the <paramref name="culture"/> can be created.</remarks>
		/// <param name="amount">The <see cref="Amount"/> of the monetary quantity.</param>
		/// <param name="culture">A <see cref="CultureInfo"/> from which retrieve the associated currency.</param>
		/// <returns>An instance of <see cref="Money"/> with the <paramref name="amount"/> specified and the currency associated to the specified <paramref name="culture"/>.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="culture"/> is null.</exception>
		/// <exception cref="ArgumentException"><paramref name="culture"/> is either an invariant, custom or neutral culture, or a <see cref="RegionInfo"/> cannot be instantiated from it.</exception>
		/// <exception cref="System.ComponentModel.InvalidEnumArgumentException">The ISO symbol associated to the <paramref name="culture"/> does not exist in the <see cref="CurrencyIsoCode"/> enumeration.</exception>
		/// <exception cref="MisconfiguredCurrencyException">The currency associated to the <paramref name="culture"/> has not been properly configured by the library implementor. Please, log a issue.</exception>
		public static Money ForCulture(decimal amount, CultureInfo culture)
		{
			Guard.AgainstNullArgument("culture", culture);
			return new Money(amount, Currency.Get(culture), ObsoleteCurrencyEventBehavior.Ignore);
		}

		/// <summary>
		/// Creates an instance of <see cref="Money"/> with <see cref="decimal.Zero"/> quantity and the unspecified currency.
		/// </summary>
		/// <returns>An <see cref="Money"/> instance with zero <see cref="Amount"/> and unspecified currency (<see cref="CurrencyIsoCode.XXX"/>).</returns>
		/// <seealso cref="Money(decimal)"/>
		public static Money Zero()
		{
			return new Money(decimal.Zero);
		}

		/// <summary>
		/// Creates an instance of <see cref="Money"/> with <see cref="decimal.Zero"/> quantity and the specified currency.
		/// </summary>
		/// <param name="currency">The <see cref="CurrencyCode"/> of the monetary quantity.</param>
		/// <returns>An <see cref="Money"/> instance with zero <see cref="Amount"/> and the specified <paramref name="currency"/>.</returns>
		/// <seealso cref="Money(decimal, CurrencyIsoCode)"/>
		public static Money Zero(CurrencyIsoCode currency)
		{
			return new Money(decimal.Zero, currency);
		}

		/// <summary>
		/// Creates an instance of <see cref="Money"/> with <see cref="decimal.Zero"/> quantity and the specified currency.
		/// </summary>
		/// <param name="currency">The incarnation of the <see cref="CurrencyCode"/>.</param>
		/// <returns>An <see cref="Money"/> instance with zero <see cref="Amount"/> and the specified <paramref name="currency"/>.</returns>
		/// <seealso cref="Money(decimal, Currency)"/>
		public static Money Zero(Currency currency)
		{
			return new Money(decimal.Zero, currency);
		}

		/// <summary>
		/// Creates an instance of <see cref="Money"/> with <see cref="decimal.Zero"/> quantity and the specified currency.
		/// </summary>
		/// <param name="threeLetterIsoCode">Textual representation of the ISO 4217 <see cref="CurrencyCode"/>.</param>
		/// <returns>An <see cref="Money"/> instance with zero <see cref="Amount"/> and the specified <paramref name="threeLetterIsoCode"/>.</returns>
		/// <seealso cref="Money(decimal, string)"/>
		public static Money Zero(string threeLetterIsoCode)
		{
			return new Money(decimal.Zero, threeLetterIsoCode);
		}

		/// <summary>
		/// Creates an instance of <see cref="Money"/> with the specified amount and the unspecified currency.
		/// </summary>
		/// <remarks>The <see cref="Amount"/> is a whole number only.
		/// Thus 'XXX 20' can be intialised, but not the value 'XXX 20.32'.</remarks>
		/// <param name="amountMajor">The <see cref="Amount"/> in the major division of the currency.</param>
		/// <returns>A <see cref="Money"/> with the specified <paramref name="amountMajor"/> and unspecified currency (<see cref="CurrencyIsoCode.XXX"/>).</returns>
		/// <seealso cref="Money(decimal)"/>
		public static Money ForMajor(long amountMajor)
		{
			return ForMajor(amountMajor, CurrencyIsoCode.XXX);
		}

		/// <summary>
		/// Creates an instance of <see cref="Money"/> with the specified amount and the specified currency.
		/// </summary>
		/// <remarks>The <see cref="Amount"/> is a whole number only.
		/// Thus 'USD 20' can be intialised, but not the value 'USD 20.32'.</remarks>
		/// <param name="amountMajor">The <see cref="Amount"/> in the major division of the currency.</param>
		/// <param name="currency">The <see cref="CurrencyCode"/> of the monetary quantity.</param>
		/// <returns>A <see cref="Money"/> with the specified <paramref name="amountMajor"/> and <paramref name="currency"/>.</returns>
		/// <seealso cref="Money(decimal)"/>
		public static Money ForMajor(long amountMajor, CurrencyIsoCode currency)
		{
			return ForMajor(amountMajor, Currency.Get(currency));
		}

		/// <summary>
		/// Creates an instance of <see cref="Money"/> from an amount in major units of the specified <paramref name="currency"/>.
		/// </summary>
		/// <remarks>The <see cref="Amount"/> is a whole number. Thus 'USD 20' can be initialized, but not 'USD 20.32'.</remarks>
		/// <param name="amountMajor">The <see cref="Amount"/> in the major division of the currency.</param>
		/// <param name="currency">The incarnation of the <see cref="CurrencyCode"/>.</param>
		/// <returns>A <see cref="Money"/> with the specified <paramref name="amountMajor"/> and <paramref name="currency"/>.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="currency"/> is null.</exception>
		public static Money ForMajor(long amountMajor, Currency currency)
		{
			Guard.AgainstNullArgument("currency", currency);
			return new Money(decimal.Truncate(amountMajor), currency);
		}

		/// <summary>
		/// Creates an instance of <see cref="Money"/> from an amount in major units of the specified <paramref name="threeLetterIsoCode"/>.
		/// </summary>
		/// <remarks>The <see cref="Amount"/> is a whole number. Thus 'USD 20' can be initialized, but not 'USD 20.32'.</remarks>
		/// <param name="amountMajor">The <see cref="Amount"/> in the major division of the currency.</param>
		/// <param name="threeLetterIsoCode">Textual representation of the ISO 4217 <see cref="CurrencyCode"/>.</param>
		/// <returns>A <see cref="Money"/> with the specified <paramref name="amountMajor"/> and <paramref name="threeLetterIsoCode"/>.</returns>
		public static Money ForMajor(long amountMajor, string threeLetterIsoCode)
		{
			return ForMajor(amountMajor, Currency.Get(threeLetterIsoCode));
		}

		/// <summary>
		/// Creates an instance of <see cref="Money"/> from an amount in major units of the unspecified currency.
		/// </summary>
		/// <remarks>Allows the creation of an instance with an amount expressed in terms of the minor unit of the unspecified currency.
		/// For the unspecified currency the input to this method represents cents.</remarks>
		/// <param name="amountMinor">The <see cref="Amount"/> in the minor division of the currency.</param>
		/// <returns>A <see cref="Money"/> with the specified <paramref name="amountMinor"/> and unspecified currency (<see cref="CurrencyIsoCode.XXX"/>).</returns>
		public static Money ForMinor(long amountMinor)
		{
			return ForMinor(amountMinor, CurrencyIsoCode.XXX);
		}

		/// <summary>
		/// Creates an instance of <see cref="Money"/> from an amount in major units of the specified currency.
		/// </summary>
		/// <remarks>Allows creating an instance with an amount expressed in terms of the minor unit of the currency.
		/// <para>For example, when constructing 'US Dollars', the <paramref name="amountMinor"/> represents 'cents'.</para>
		/// <para>When the currency has zero decimal places, <see cref="MajorAmount"/> and <see cref="MinorAmount"/> are the same.</para>
		/// </remarks>
		/// /// <example>Money.ForMinor(CurrencyIsoCode.USD, 2595) creates an instance of 'USD 29.95'</example>
		/// <param name="amountMinor">The <see cref="Amount"/> in the minor division of the currency.</param>
		/// <param name="currency">The <see cref="CurrencyCode"/> of the monetary quantity.</param>
		/// <returns>A <see cref="Money"/> with the specified <paramref name="amountMinor"/> and <paramref name="currency"/>.</returns>
		public static Money ForMinor(long amountMinor, CurrencyIsoCode currency)
		{
			return ForMinor(amountMinor, Currency.Get(currency));
		}

		/// <summary>
		/// Creates an instance of <see cref="Money"/> from an amount in major units of the specified currency.
		/// </summary>
		/// <remarks>Allows creating an instance with an amount expressed in terms of the minor unit of the currency.
		/// <para>For example, when constructing 'US Dollars', the <paramref name="amountMinor"/> represents 'cents'.</para>
		/// <para>When the currency has zero decimal places, <see cref="MajorAmount"/> and <see cref="MinorAmount"/> are the same.</para>
		/// </remarks>
		/// <example>Money.ForMinor(Currency.Usd, 2595) creates an instance of 'USD 29.95'</example>
		/// <param name="amountMinor">The <see cref="Amount"/> in the minor division of the currency.</param>
		/// <param name="currency">The incarnation of the <see cref="CurrencyCode"/>.</param>
		/// <returns>A <see cref="Money"/> with the specified <paramref name="amountMinor"/> and <paramref name="currency"/>.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="currency"/> is null.</exception>
		public static Money ForMinor(long amountMinor, Currency currency)
		{
			Guard.AgainstNullArgument("currency", currency);

			return new Money(
				decimal.Divide(amountMinor, PowerOfTen.Positive(currency)),
currency);
		}

		/// <summary>
		/// Creates an instance of <see cref="Money"/> from an amount in major units of the specified currency.
		/// </summary>
		/// <remarks>Allows creating an instance with an amount expressed in terms of the minor unit of the currency.
		/// <para>For example, when constructing 'US Dollars', the <paramref name="amountMinor"/> represents 'cents'.</para>
		/// <para>When the currency has zero decimal places, <see cref="MajorAmount"/> and <see cref="MinorAmount"/> are the same.</para>
		/// </remarks>
		/// <example>Money.ForMinor(Currency.Usd, 2595) creates an instance of 'USD 29.95'</example>
		/// <param name="amountMinor">The <see cref="Amount"/> in the minor division of the currency.</param>
		/// <param name="threeLetterIsoCode">Textual representation of the ISO 4217 <see cref="CurrencyCode"/>.</param>
		/// <returns>A <see cref="Money"/> with the specified <paramref name="amountMinor"/> and <paramref name="threeLetterIsoCode"/>.</returns>
		public static Money ForMinor(long amountMinor, string threeLetterIsoCode)
		{
			return ForMinor(amountMinor, Currency.Get(threeLetterIsoCode));
		}

		/// <summary>
		/// Creates an instance of <see cref="Money"/> witht the total value of an array.
		/// </summary>
		/// <remarks>All moneys have to have the same currency, otherwise and exception will be thrown.</remarks>
		/// <param name="moneys">A not null and not empty array of moneys.</param>
		/// <returns>An <see cref="Money"/> instance which <see cref="Amount"/> is the sum of all amounts of the moneys in the array,
		/// and <see cref="Currency"/> the same as all the moneys in the array.</returns>
		/// <exception cref="ArgumentNullException">If <paramref name="moneys"/> is null.</exception>
		/// <exception cref="ArgumentException">If <paramref name="moneys"/> is empty.</exception>
		/// <exception cref="DifferentCurrencyException">If any of the currencies of <paramref name="moneys"/> differ.</exception>
		public static Money Total(params Money[] moneys)
		{
			return Total((IEnumerable<Money>)moneys);
		}

		/// <summary>
		/// Creates an instance of <see cref="Money"/> witht the total value of an collection of moneys.
		/// </summary>
		/// <remarks></remarks>
		/// <param name="moneys">A not null and not empty collection of moneys.</param>
		/// <returns>An <see cref="Money"/> instance which <see cref="Amount"/> is the sum of all amounts of the moneys in the collection,
		/// and <see cref="Currency"/> the same as all the moneys in the collection.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="moneys"/> is null.</exception>
		/// <exception cref="ArgumentException"><paramref name="moneys"/> is empty.</exception>
		/// <exception cref="DifferentCurrencyException">If any of the currencies of <paramref name="moneys"/> differ.</exception>
		public static Money Total(IEnumerable<Money> moneys)
		{
			Guard.AgainstNullArgument("moneys", moneys);
			Guard.AgainstArgument("moneys", !moneys.Any(), "The collection of moneys cannot be empty.");

			return moneys.Aggregate((a, b) => a + b);
		}

		#endregion

		/// <summary>
		/// DO NOT USE the field directly. Use <see cref="CurrencyCode"/>.
		/// </summary>
		[Obsolete]
		private CurrencyIsoCode? _currencyCode;

#pragma warning disable 612,618
		/// <summary>
		/// The ISO 4217 code of the currency of a monetary quantity.
		/// </summary>
		public CurrencyIsoCode CurrencyCode
		{
			get
			{
				return _currencyCode.GetValueOrDefault(CurrencyIsoCode.XXX);
			}
			private set
			{
				_currencyCode = value;
			}
		}
#pragma warning restore 612,618

		/// <summary>
		/// The amount of a monetary quantity
		/// </summary>
		public decimal Amount { get; private set; }

		/// <summary>
		/// Gets the amount in major units
		/// </summary>
		/// <remarks>This method returns the monetary amount in terms of the major units of the currency, truncating the <see cref="Amount"/> if necessary.
		/// <para>For example, 'EUR 2.35' will return a major amount of 2, since EUR has 2 significant decimal values. 
		/// 'BHD -1.345' will return -1.</para></remarks>
		public decimal MajorAmount
		{
			get { return Truncate().Amount; }
		}

		/// <summary>
		/// Gets the amount in major units as a <see cref="long"/>.
		/// </summary>
		/// <remarks>This property returns the monetary amount in terms of the major units of the currency, truncating the amount if necessary.
		/// <para>For example, 'EUR 2.35' will return a major amount of 2, since EUR has 2 significant decimal values. 
		/// 'BHD -1.345' will return -1.</para></remarks>
		public long MajorIntegralAmount
		{
			get { return Convert.ToInt64(MajorAmount); }
		}

		/// <summary>
		/// Gets the amount in minor units.
		/// </summary>
		/// <remarks>This property return the monetary amount in terms of the minor units of the currency, truncating the amount if necessary.
		/// <para>For example, 'EUR 2.35' will return a minor amount of 235, since EUR has 2 significant decimal values. 
		/// 'BHD -1.345' will return -1345.</para></remarks>
		public decimal MinorAmount
		{
			get
			{
				Currency currency = Currency.Get(CurrencyCode);
				return decimal.Truncate(decimal.Multiply(Amount, PowerOfTen.Positive(currency)));
			}
		}

		/// <summary>
		/// Gets the amount in minor units as a <see cref="long"/>.
		/// </summary>
		/// <remarks>This property return the monetary amount in terms of the minor units of the currency, truncating the amount if necessary.
		/// <para>For example, 'EUR 2.35' will return a minor amount of 235, since EUR has 2 significant decimal values. 
		/// 'BHD -1.345' will return -1345.</para></remarks>
		public long MinorIntegralAmount
		{
			get { return Convert.ToInt64(MinorAmount); }
		}

		/// <summary>
		/// Represents the smallest quantity that couldn be represented using the currency corresponding to <see cref="CurrencyCode"/>.
		/// <seealso cref="Currency.MinAmount"/>
		/// <seealso cref="Currency.SignificantDecimalDigits"/>
		/// </summary>
		public Money MinValue
		{
			get
			{
				Currency currency = Currency.Get(CurrencyCode);
				return new Money(currency.MinAmount, currency);
			}
		}

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
		/// <paramref name="provider"/>.</returns>
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
		/// and <paramref name="provider"/>.</returns>
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

		/// <summary>
		/// Indicates whether the current <see cref="Money"/> is equal to another <see cref="Money"/>.
		/// </summary>
		/// <returns>
		/// true if the current instance has equal <see cref="Amount"/> and <see cref="Currency"/>as the <paramref name="other"/> parameter;
		/// otherwise, false.
		/// </returns>
		/// <param name="other">An money to compare with this instance.</param>
		public bool Equals(Money other)
		{
			return Equals(other.CurrencyCode, CurrencyCode) && other.Amount == Amount;
		}


		/// <summary>
		/// Indicates whether this instance and a specified object are equal.
		/// </summary>
		/// <returns>
		/// true if <paramref name="obj"/> and this instance are the same type and represent the same value; otherwise, false.
		/// </returns>
		/// <param name="obj">Another object to compare to.</param>
		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (obj.GetType() != typeof(Money)) return false;
			return Equals((Money)obj);
		}

		/// <summary>
		/// Returns the hash code for this instance.
		/// </summary>
		/// <returns>
		/// A 32-bit signed integer that is the hash code for this instance.
		/// </returns>
		public override int GetHashCode()
		{
			unchecked
			{
				return (CurrencyCode.GetHashCode() * 397) ^ Amount.GetHashCode();
			}
		}

		/// <summary>
		/// Returns a value indicating whether two instances of <see cref="Money"/> are equal.
		/// </summary>
		/// <param name="left">The first value to compare.</param>
		/// <param name="right">The second value to compare.</param>
		/// <returns>true if <paramref name="left"/> and <paramref name="right"/> are equal; otherwise, false.</returns>
		public static bool operator ==(Money left, Money right)
		{
			return Equals(left, right);
		}

		/// <summary>
		/// Returns a value indicating whether two instances of <see cref="Money"/> are not equal.
		/// </summary>
		/// <param name="left">The first value to compare.</param>
		/// <param name="right">The second value to compare.</param>
		/// <returns>true if <paramref name="left"/> and <paramref name="right"/> are not equal; otherwise, false.</returns>
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
		/// Returns a value indicating whether the <see cref="Amount"/> is stricly equal to <see cref="decimal.Zero"/>.
		/// </summary>
		/// <returns>true if <see cref="Amount"/> is equal to <see cref="decimal.Zero"/>; otherwise, false.</returns>
		public bool IsZero()
		{
			return Amount.Equals(decimal.Zero);
		}

		/// <summary>
		/// Returns a value indicating whether the <see cref="Amount"/> is less than or equal to <see cref="decimal.Zero"/>.
		/// </summary>
		/// <returns>true if <see cref="Amount"/> is less or equal than <see cref="decimal.Zero"/>; otherwise, false.</returns>
		/// <seealso cref="IsNegative()"/>
		/// <seealso cref="IsZero()"/>
		public bool IsNegativeOrZero()
		{
			return IsNegative() || IsZero();
		}

		/// <summary>
		/// Returns a value indicating whether the <see cref="Amount"/> is greater than or equal to <see cref="decimal.Zero"/>.
		/// </summary>
		/// <returns>true if <see cref="Amount"/> is greater or equal than <see cref="decimal.Zero"/>; otherwise, false.</returns>
		/// <seealso cref="IsPositive()"/>
		/// <seealso cref="IsZero()"/>
		public bool IsPositiveOrZero()
		{
			return IsPositive() || IsZero();
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
		public static Money Add(Money first, Money second)
		{
			return first + second;
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
		public static Money Subtract(Money first, Money second)
		{
			return first - second;
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
			return new Money(truncateAmountFor(currency.SignificantDecimalDigits), CurrencyCode);
		}

		/// <summary>
		/// Truncates the <see cref="Amount"/> to the number of significant decimal digits specified by <paramref name="numberFormat"/>.
		/// </summary>
		/// <param name="numberFormat">Specifies the number of significant decimal digits.</param>
		/// <returns>A <see cref="Money"/> with the <see cref="Amount"/> truncated to the significant number of decimal digits of <paramref name="numberFormat"/>.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="numberFormat"/> is null.</exception>
		public Money TruncateToSignificantDecimalDigits(NumberFormatInfo numberFormat)
		{
			Guard.AgainstNullArgument("numberFormat", numberFormat);

			return new Money(truncateAmountFor(numberFormat.CurrencyDecimalDigits), CurrencyCode);
		}

		private decimal truncateAmountFor(int numberOfDecimals)
		{
			uint centFactor = PowerOfTen.Positive(numberOfDecimals);
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
		/// <exception cref="ArgumentNullException"><paramref name="binaryOperation"/> is null.</exception>
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
		/// <exception cref="ArgumentNullException"><paramref name="unaryOperation"/> is null.</exception>
		public Money Perform(Func<decimal, decimal> unaryOperation)
		{
			Guard.AgainstNullArgument("unaryOperation", unaryOperation);

			return new Money(unaryOperation(Amount), CurrencyCode);
		}

		/// <summary>
		/// Specifies whether the <see cref="Amount"/> is not a whole number.
		/// </summary>
		public bool HasDecimals
		{
			get
			{
				decimal truncated = decimal.Truncate(Amount);
				return Amount - truncated != decimal.Zero;
			}
		}

		/// <summary>
		/// Allocates the sum of money fully and 'fairly', delegating the distribution of whichever remainder after
		/// allocating the highest fair amount amongst the recipients to the provided <paramref name="allocator"/>.
		/// </summary>
		/// <remarks>
		/// <para>
		/// A sum of money that can be allocated to each recipient exactly evenly is inherently 'fair'. For example, a US
		/// Dollar split four (4) ways leaves each recipient with 25 cents.</para>
		/// <para>
		/// A US Dollar split three (3) ways cannot be distributed evenly and is therefore inherently 'unfair'. The
		/// best we can do is minimize the amount of the remainder (in this case a cent) and allocate it in a way
		/// that seems random and thus fair to the recipients.</para>
		/// <para>The precision to use for rounding will be the <see cref="Currency.SignificantDecimalDigits"/> 
		/// of the currency represented by <see cref="CurrencyCode"/>.</para>
		/// </remarks>
		/// <param name="numberOfRecipients">The number of times to split up the total.</param>
		/// <param name="allocator">The <see cref="IRemainderAllocator"/> that will distribute the remainder after an even split.</param>
		/// <returns>The results of the allocation as an array with a length equal to <paramref name="numberOfRecipients"/>.</returns>
		/// <exception cref="ArithmeticException">The <paramref name="allocator"/> did not distributed all the remainder.</exception>
		/// <seealso cref="IRemainderAllocator"/>
		public Money[] Allocate(int numberOfRecipients, IRemainderAllocator allocator)
		{
			new Range<int>(1.Close(), int.MaxValue.Close()).AssertArgument("numberOfRecipients", numberOfRecipients);

			Money totalAllocated;
			Money[] allocated = new EvenAllocator(this)
				.Allocate(numberOfRecipients, out totalAllocated);

			allocateRemainderIfNeeded(ref totalAllocated, allocator, allocated);

			assertAllocatedWhole(totalAllocated);
			return allocated;
		}

		private void allocateRemainderIfNeeded(ref Money totalAllocated, IRemainderAllocator allocator, Money[] results)
		{
			Money remainder = this - totalAllocated;
			if (remainder.Amount > 0)
			{
				allocator.Allocate(remainder, results);
				totalAllocated = Total(results);
			}
		}

		private void assertAllocatedWhole(Money totalAllocated)
		{
			if (!totalAllocated.Equals(this)) throw new ArithmeticException("The total amount was not fully allocated");
		}

		#endregion

		#region serialization

		/// <summary>
		/// Populates a <see cref="SerializationInfo"/> with the data needed to serialize the target object.
		/// </summary>
		/// <param name="info">The <see cref="System.Runtime.Serialization.SerializationInfo"/> to populate with data.</param>
		/// <param name="context">The destination (see <see cref="T:System.Runtime.Serialization.StreamingContext"/>) for this serialization.</param>
		/// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue(Serialization.Data.Money.AMOUNT, Amount);
			info.AddValue(Serialization.Data.Money.CURRENCY, CurrencyCode, typeof(Currency));
		}

		/// <summary>
		/// This method is reserved and should not be used.
		/// When implementing the <see cref="IXmlSerializable"/> interface, you should return null from this method, and instead, if specifying a custom schema is required, apply the <see cref="T:System.Xml.Serialization.XmlSchemaProviderAttribute"/> to the class.
		/// </summary>
		/// <returns>null</returns>
		[Obsolete("deprecated, use SchemaProviders instead")]
		XmlSchema IXmlSerializable.GetSchema()
		{
			return null;
		}

		/// <summary>
		/// Returns the XML schema applied for serialization.
		/// </summary>
		/// <param name="xs">A cache of XML Schema definition language (XSD) schemas.</param>
		/// <returns>Represents the complexType element from XML Schema as specified by the <paramref name="xs"/>.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="xs"/> is null.</exception>
		public static XmlSchemaComplexType GetSchema(XmlSchemaSet xs)
		{
			Guard.AgainstNullArgument("xs", xs);

			XmlSchemaComplexType complex = null;
			var schemaSerializer = new XmlSerializer(typeof(XmlSchema));
			using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(Serialization.Data.ResourceName))
			{
				if (stream != null)
				{
					var schema = (XmlSchema)schemaSerializer.Deserialize(new XmlTextReader(stream));
					xs.Add(schema);
					var name = new XmlQualifiedName(Serialization.Data.Money.DATA_TYPE, Serialization.Data.NAMESPACE);
					complex = (XmlSchemaComplexType)schema.SchemaTypes[name];
				}
			}
			return complex;
		}

		/// <summary>
		/// Generates an object from its XML representation.
		/// </summary>
		/// <param name="reader">The <see cref="XmlReader"/> stream from which the object is deserialized.</param>
		/// <exception cref="ArgumentNullException"><paramref name="reader"/> is null.</exception>
		public void ReadXml(XmlReader reader)
		{
			Guard.AgainstNullArgument("reader", reader);

			reader.ReadStartElement();
			setAllFields(reader.ReadElementContentAsDecimal(Serialization.Data.Money.AMOUNT, Serialization.Data.NAMESPACE),
				Currency.ReadXmlData(reader));
			reader.ReadEndElement();
		}

		/// <summary>
		/// Converts an object into its XML representation.
		/// </summary>
		/// <param name="writer">The <see cref="XmlWriter"/> stream to which the object is serialized.</param>
		/// <exception cref="ArgumentNullException"><paramref name="writer"/> is null.</exception>
		public void WriteXml(XmlWriter writer)
		{
			Guard.AgainstNullArgument("writer", writer);

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

		/// <summary>
		/// Converts the string representation of a monetary quantity to its <see cref="Money"/> equivalent
		/// using the <see cref="NumberStyles.Currency"/> style and the specified currency as format information.
		/// </summary>
		/// <remaks>This method assumes <paramref name="s"/> to have a <see cref="NumberStyles.Currency"/> style.</remaks>
		/// <param name="s">The string representation of the monetary quantity to convert.</param>
		/// <param name="currency">Expected currency of <paramref name="s"/> that provides format information.</param>
		/// <returns>The <see cref="Money"/> equivalent to the monetary quantity contained in <paramref name="s"/> as specified by the <paramref name="currency"/>.</returns>
		/// <exception cref="FormatException"><paramref name="s"/> is not in the correct format.</exception>
		/// <exception cref="OverflowException"><paramref name="s"/> representes a montary quantity less than <see cref="decimal.MinValue"/> or greater than <see cref="decimal.MaxValue"/>.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="s"/> is null.</exception>
		/// <seealso cref="decimal.Parse(string, NumberStyles, IFormatProvider)" />
		public static Money Parse(string s, Currency currency)
		{
			return Parse(s, NumberStyles.Currency, currency);
		}

		/// <summary>
		/// Converts the string representation of a monetary quantity to its <see cref="Money"/> equivalent
		/// using the specified style and the specified currency as format information.
		/// </summary>
		/// <remarks>Use this method when <paramref name="s"/> is supected not to have a <see cref="NumberStyles.Currency"/> style or more control over the operation is needed.</remarks>
		/// <param name="s">The string representation of the monetary quantity to convert.</param>
		/// <param name="style">A bitwise combination of <see cref="NumberStyles"/> values that indicates the style elements that can be present in <paramref name="s"/>.
		/// A typical value to specify is <see cref="Number"/>.</param>
		/// <param name="currency">Expected currency of <paramref name="s"/> that provides format information.</param>
		/// <returns>The <see cref="Money"/> equivalent to the monetary quantity contained in <paramref name="s"/> as specified by <paramref name="style"/> and <paramref name="currency"/>.</returns>
		/// <exception cref="FormatException"><paramref name="s"/> is not in the correct format.</exception>
		/// <exception cref="OverflowException"><paramref name="s"/> representes a montary quantity less than <see cref="decimal.MinValue"/> or greater than <see cref="decimal.MaxValue"/>.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="s"/> is null.</exception>
		/// <exception cref="ArgumentException"><paramref name="style"/> is not a <see cref="NumberStyles"/> value
		/// <para>-or-</para>
		/// <paramref name="style"/> is the <see cref="NumberStyles.AllowHexSpecifier"/> value.
		/// </exception>
		/// <seealso cref="decimal.Parse(string, NumberStyles, IFormatProvider)" />
		public static Money Parse(string s, NumberStyles style, Currency currency)
		{
			decimal amount = decimal.Parse(s, style, currency);

			return new Money(amount, currency);
		}

		/// <summary>
		/// Converts the string representation of a monetary quantity to its <see cref="Money"/> equivalent
		/// using <see cref="NumberStyles.Currency"/> and the provided currency as format infomation.
		/// A return value indicates whether the conversion succeeded or failed.
		/// </summary>
		/// <param name="s">The string representation of the monetary quantity to convert.</param>
		/// <param name="currency">Expected currency of <paramref name="s"/> that provides format information.</param>
		/// <param name="money">When this method returns, contains the <see cref="Money"/> that is equivalent to the monetary quantity contained in <paramref name="s"/>,
		/// if the conversion succeeded, or is null if the conversion failed.
		/// The conversion fails if the <paramref name="s"/> parameter is null, is not in a format compliant with currency style,
		/// or represents a number less than <see cref="decimal.MinValue"/> or greater than <see cref="decimal.MaxValue"/>.
		/// This parameter is passed uninitialized. </param>
		/// <returns>true if s was converted successfully; otherwise, false.</returns>
		/// <seealso cref="decimal.TryParse(string, NumberStyles, IFormatProvider, out decimal)" />
		public static bool TryParse(string s, Currency currency, out Money? money)
		{
			return TryParse(s, NumberStyles.Currency, currency, out money);
		}

		/// <summary>
		/// Converts the string representation of a monetary quantity to its <see cref="Money"/> equivalent
		/// using the specified style and the provided currency as format infomation.
		/// A return value indicates whether the conversion succeeded or failed.
		/// </summary>
		/// <param name="s">The string representation of the monetary quantity to convert.</param>
		/// <param name="style">A bitwise combination of enumeration values that indicates the permitted format of <paramref name="s"/>.
		/// A typical value to specify is <see cref="Number"/>.</param>
		/// <param name="currency">Expected currency of <paramref name="s"/> that provides format information.</param>
		/// <param name="money">When this method returns, contains the <see cref="Money"/> that is equivalent to the monetary quantity contained in <paramref name="s"/>,
		/// if the conversion succeeded, or is null if the conversion failed.
		/// The conversion fails if the <paramref name="s"/> parameter is null, is not in a format compliant with currency style,
		/// or represents a number less than <see cref="decimal.MinValue"/> or greater than <see cref="decimal.MaxValue"/>.
		/// This parameter is passed uninitialized. </param>
		/// <returns>true if s was converted successfully; otherwise, false.</returns>
		/// <exception cref="ArgumentException"><paramref name="style"/> is not a <see cref="NumberStyles"/> value
		/// <para>-or-</para>
		/// <paramref name="style"/> is the <see cref="NumberStyles.AllowHexSpecifier"/> value.
		/// </exception>
		/// <seealso cref="decimal.TryParse(string, NumberStyles, IFormatProvider, out decimal)" />
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
