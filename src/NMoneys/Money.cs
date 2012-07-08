using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using NMoneys.Support;

namespace NMoneys
{
	/// <summary>
	/// A monetary quantity in a given currency
	/// </summary>
	[Serializable]
	[XmlRoot(Namespace = Serialization.Data.NAMESPACE, ElementName = Serialization.Data.Money.ROOT_NAME, DataType = Serialization.Data.Money.DATA_TYPE, IsNullable = false)]
	public partial struct Money : ISerializable, IXmlSerializable
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
		/// <exception cref="InvalidEnumArgumentException"><paramref name="currency"/> is not defined.</exception>
		public static Money Zero(CurrencyIsoCode currency)
		{
			return new Money(decimal.Zero, currency);
		}

		/// <summary>
		/// Creates and initializes an array of <see cref="Money"/> with <see cref="decimal.Zero"/> quantity and the specified currency.
		/// </summary>
		/// <param name="currency">The <see cref="CurrencyCode"/> of each monetary quantity.</param>
		/// <param name="numberOfElements">The number of elements in the array.</param>
		/// <returns>An array of <see cref="Money"/> instances with zero <see cref="Amount"/> and the specified <paramref name="currency"/>.</returns>
		/// <seealso cref="Money.Zero(CurrencyIsoCode)"/>
		/// <exception cref="InvalidEnumArgumentException"><paramref name="currency"/> is not defined.</exception>
		/// <exception cref="OverflowException"><paramref name="numberOfElements"/> is not a valid array length.</exception>
		public static Money[] Zero(CurrencyIsoCode currency, int numberOfElements)
		{
			return initArray(numberOfElements, () => Zero(currency));
		}

		private static Money[] initArray(int length, Func<Money> aMoney)
		{
			var results = new Money[length];
			// instead of execute the delegate once per iteration, we execute it once and assign it multiple times as
			// it is a value object
			var instance = aMoney();
			for (int i = 0; i < results.Length; i++)
			{
				results[i] = instance;
			}
			return results;
		}

		/// <summary>
		/// Creates an instance of <see cref="Money"/> with <see cref="decimal.Zero"/> quantity and the specified currency.
		/// </summary>
		/// <param name="currency">The incarnation of the <see cref="CurrencyCode"/>.</param>
		/// <returns>An <see cref="Money"/> instance with zero <see cref="Amount"/> and the specified <paramref name="currency"/>.</returns>
		/// <seealso cref="Money(decimal, Currency)"/>
		/// <exception cref="ArgumentNullException"><paramref name="currency"/> is null.</exception>
		public static Money Zero(Currency currency)
		{
			return new Money(decimal.Zero, currency);
		}

		/// <summary>
		/// Creates and initializes an array of <see cref="Money"/> with <see cref="decimal.Zero"/> quantity and the specified currency.
		/// </summary>
		/// <param name="currency">The incarnation of the <see cref="CurrencyCode"/> for each monetary quantity.</param>
		/// <param name="numberOfElements">The number of elements in the array.</param>
		/// <returns>An array of <see cref="Money"/> instances with zero <see cref="Amount"/> and the specified <paramref name="currency"/>.</returns>
		/// <seealso cref="Money.Zero(Currency)"/>
		/// <exception cref="ArgumentNullException"><paramref name="currency"/> is null.</exception>
		/// <exception cref="OverflowException"><paramref name="numberOfElements"/> is not a valid array length.</exception>
		public static Money[] Zero(Currency currency, int numberOfElements)
		{
			return initArray(numberOfElements, () => Zero(currency));
		}

		/// <summary>
		/// Creates an instance of <see cref="Money"/> with <see cref="decimal.Zero"/> quantity and the specified currency.
		/// </summary>
		/// <param name="threeLetterIsoCode">Textual representation of the ISO 4217 <see cref="CurrencyCode"/>.</param>
		/// <returns>An <see cref="Money"/> instance with zero <see cref="Amount"/> and the specified <paramref name="threeLetterIsoCode"/>.</returns>
		/// <seealso cref="Money(decimal, string)"/>
		/// <exception cref="ArgumentNullException"><paramref name="threeLetterIsoCode"/> is null.</exception>
		/// <exception cref="InvalidEnumArgumentException"><paramref name="threeLetterIsoCode"/> is not defined.</exception>
		public static Money Zero(string threeLetterIsoCode)
		{
			return new Money(decimal.Zero, threeLetterIsoCode);
		}

		/// <summary>
		/// Creates and initializes an array of <see cref="Money"/> with <see cref="decimal.Zero"/> quantity and the specified currency.
		/// </summary>
		/// <param name="threeLetterIsoCode">Textual representation of the ISO 4217 <see cref="CurrencyCode"/> for each monetary quantity.</param>
		/// <param name="numberOfElements">The number of elements in the array.</param>
		/// <returns>An array of <see cref="Money"/> instances with zero <see cref="Amount"/> and the specified <paramref name="threeLetterIsoCode"/>.</returns>
		/// <seealso cref="Money.Zero(string)"/>
		/// <exception cref="OverflowException"><paramref name="numberOfElements"/> is not a valid array length.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="threeLetterIsoCode"/> is null.</exception>
		/// <exception cref="InvalidEnumArgumentException"><paramref name="threeLetterIsoCode"/> is not defined.</exception>
		public static Money[] Zero(string threeLetterIsoCode, int numberOfElements)
		{
			return initArray(numberOfElements, ()=> Zero(threeLetterIsoCode));
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
		/// Creates and initializes an array of <see cref="Money"/> with <paramref name="amount"/> quantity and the specified currency.
		/// </summary>
		/// <param name="amount">The <see cref="Amount"/> of each monetary quantity.</param>
		/// <param name="currency">The <see cref="CurrencyCode"/> of each monetary quantity.</param>
		/// <param name="numberOfElements">The number of elements in the array.</param>
		/// <returns>An array of <see cref="Money"/> instances with <paramref name="amount"/> and the specified <paramref name="currency"/>.</returns>
		/// <seealso cref="Money(decimal, CurrencyIsoCode)"/>
		/// <exception cref="InvalidEnumArgumentException"><paramref name="currency"/> is not defined.</exception>
		/// <exception cref="OverflowException"><paramref name="numberOfElements"/> is not a valid array length.</exception>
		public static Money[] Some(decimal amount, CurrencyIsoCode currency, int numberOfElements)
		{
			return initArray(numberOfElements, () => new Money(amount, currency));
		}

		/// <summary>
		/// Creates and initializes an array of <see cref="Money"/> with <paramref name="amount"/> quantity and the specified currency.
		/// </summary>
		/// <param name="amount">The <see cref="Amount"/> of each monetary quantity.</param>
		/// <param name="currency">The incarnation of the <see cref="CurrencyCode"/> for each monetary quantity.</param>
		/// <param name="numberOfElements">The number of elements in the array.</param>
		/// <returns>An array of <see cref="Money"/> instances with <paramref name="amount"/> and the specified <paramref name="currency"/>.</returns>
		/// <seealso cref="Money(decimal, Currency)"/>
		/// <exception cref="InvalidEnumArgumentException"><paramref name="currency"/> is not defined.</exception>
		/// <exception cref="OverflowException"><paramref name="numberOfElements"/> is not a valid array length.</exception>
		public static Money[] Some(decimal amount, Currency currency, int numberOfElements)
		{
			return initArray(numberOfElements, () => new Money(amount, currency));
		}

		/// <summary>
		/// Creates and initializes an array of <see cref="Money"/> with <paramref name="amount"/> quantity and the specified currency.
		/// </summary>
		/// <param name="amount">The <see cref="Amount"/> of each monetary quantity.</param>
		/// <param name="threeLetterIsoCode">Textual representation of the ISO 4217 <see cref="CurrencyCode"/> for each monetary quantity.</param>
		/// <param name="numberOfElements">The number of elements in the array.</param>
		/// <returns>An array of <see cref="Money"/> instances with <paramref name="amount"/> and the specified <paramref name="threeLetterIsoCode"/>.</returns>
		/// <seealso cref="Money(decimal, string)"/>
		/// <exception cref="OverflowException"><paramref name="numberOfElements"/> is not a valid array length.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="threeLetterIsoCode"/> is null.</exception>
		/// <exception cref="InvalidEnumArgumentException"><paramref name="threeLetterIsoCode"/> is not defined.</exception>
		public static Money[] Some(decimal amount, string threeLetterIsoCode, int numberOfElements)
		{
			return initArray(numberOfElements, () => new Money(amount, threeLetterIsoCode));
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
		/// Returns a value indicating whether each of the <paramref name="moneys"/> has the same currency as the instance.
		/// </summary>
		/// <param name="moneys">Collection of <see cref="Money"/> instances to check against.</param>
		/// <returns>true if <see cref="CurrencyCode"/> is equal to each of <paramref name="moneys"/>'s; otherwise, false.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="moneys"/> is null.</exception>
		public bool HasSameCurrencyAs(IEnumerable<Money> moneys)
		{
			Guard.AgainstNullArgument("moneys", moneys);
			var self = this;
			return moneys.All(self.HasSameCurrencyAs);
		}

		/// <summary>
		/// Checks whether the <paramref name="money"/> has the same currency as the instance, throwing an exception if that is not the case.
		/// </summary>
		/// <param name="money"><see cref="Money"/> instance to check against.</param>
		/// <exception cref="DifferentCurrencyException"><paramref name="money"/> has a different currency from the instance's.</exception>
		public void AssertSameCurrency(Money money)
		{
			if (!HasSameCurrencyAs(money)) throw new DifferentCurrencyException(CurrencyCode.ToString(), money.CurrencyCode.ToString());
		}

		/// <summary>
		/// Checks whether each of the <paramref name="moneys"/> has the same currency as the instance, throwing an exception if that is not the case.
		/// </summary>
		/// <param name="moneys">Collection of <see cref="Money"/> instance to check against.</param>
		/// <exception cref="DifferentCurrencyException">At least one of the <paramref name="moneys"/> has a different currency from the instance's.</exception>
		public void AssertSameCurrency(IEnumerable<Money> moneys)
		{
			Guard.AgainstNullArgument("arg", moneys);
			foreach (var money in moneys)
			{
				if (!HasSameCurrencyAs(money)) throw new DifferentCurrencyException(CurrencyCode.ToString(), money.CurrencyCode.ToString());	
			}
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
	}
}
