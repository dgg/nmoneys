using System;
using System.Diagnostics.Contracts;
using System.Globalization;
using NMoneys.Support;

namespace NMoneys
{
	public partial struct Money
	{
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
		/// <exception cref="ArgumentException">The ISO symbol associated to the current culture does not exist in the <see cref="CurrencyIsoCode"/> enumeration.</exception>
		/// <exception cref="MisconfiguredCurrencyException">The currency associated to the current culture has not been properly configured by the library implementor. Please, log a issue.</exception>
		[Pure]
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
		/// <exception cref="ArgumentException">The ISO symbol associated to the <paramref name="culture"/> does not exist in the <see cref="CurrencyIsoCode"/> enumeration.</exception>
		/// <exception cref="MisconfiguredCurrencyException">The currency associated to the <paramref name="culture"/> has not been properly configured by the library implementor. Please, log a issue.</exception>
		[Pure]
		public static Money ForCulture(decimal amount, CultureInfo culture)
		{
			Guard.AgainstNullArgument(nameof(culture), culture);
			return new Money(amount, Currency.Get(culture), ObsoleteCurrencyEventBehavior.Ignore);
		}

		/// <summary>
		/// Creates an instance of <see cref="Money"/> with <see cref="decimal.Zero"/> quantity and the unspecified currency.
		/// </summary>
		/// <returns>An <see cref="Money"/> instance with zero <see cref="Amount"/> and unspecified currency (<see cref="CurrencyIsoCode.XXX"/>).</returns>
		/// <seealso cref="Money(decimal)"/>
		[Pure]
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
		/// <exception cref="ArgumentException"><paramref name="currency"/> is not defined.</exception>
		[Pure]
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
		/// <exception cref="ArgumentException"><paramref name="currency"/> is not defined.</exception>
		/// <exception cref="OverflowException"><paramref name="numberOfElements"/> is not a valid array length.</exception>
		[Pure]
		public static Money[] Zero(CurrencyIsoCode currency, int numberOfElements)
		{
			return initArray(numberOfElements, () => Zero(currency));
		}

		[Pure]
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
		[Pure]
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
		[Pure]
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
		/// <exception cref="ArgumentException"><paramref name="threeLetterIsoCode"/> is not defined.</exception>
		[Pure]
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
		/// <exception cref="ArgumentException"><paramref name="threeLetterIsoCode"/> is not defined.</exception>
		[Pure]
		public static Money[] Zero(string threeLetterIsoCode, int numberOfElements)
		{
			return initArray(numberOfElements, () => Zero(threeLetterIsoCode));
		}

		/// <summary>
		/// Creates an instance of <see cref="Money"/> with the specified amount and the unspecified currency.
		/// </summary>
		/// <remarks>The <see cref="Amount"/> is a whole number only.
		/// Thus 'XXX 20' can be intialised, but not the value 'XXX 20.32'.</remarks>
		/// <param name="amountMajor">The <see cref="Amount"/> in the major division of the currency.</param>
		/// <returns>A <see cref="Money"/> with the specified <paramref name="amountMajor"/> and unspecified currency (<see cref="CurrencyIsoCode.XXX"/>).</returns>
		/// <seealso cref="Money(decimal)"/>
		[Pure]
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
		[Pure]
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
		[Pure]
		public static Money ForMajor(long amountMajor, Currency currency)
		{
			Guard.AgainstNullArgument(nameof(currency), currency);
			return new Money(decimal.Truncate(amountMajor), currency);
		}

		/// <summary>
		/// Creates an instance of <see cref="Money"/> from an amount in major units of the specified <paramref name="threeLetterIsoCode"/>.
		/// </summary>
		/// <remarks>The <see cref="Amount"/> is a whole number. Thus 'USD 20' can be initialized, but not 'USD 20.32'.</remarks>
		/// <param name="amountMajor">The <see cref="Amount"/> in the major division of the currency.</param>
		/// <param name="threeLetterIsoCode">Textual representation of the ISO 4217 <see cref="CurrencyCode"/>.</param>
		/// <returns>A <see cref="Money"/> with the specified <paramref name="amountMajor"/> and <paramref name="threeLetterIsoCode"/>.</returns>
		[Pure]
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
		[Pure]
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
		[Pure]
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
		[Pure]
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
		[Pure]
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
		/// <exception cref="ArgumentException"><paramref name="currency"/> is not defined.</exception>
		/// <exception cref="OverflowException"><paramref name="numberOfElements"/> is not a valid array length.</exception>
		[Pure]
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
		/// <exception cref="ArgumentException"><paramref name="currency"/> is not defined.</exception>
		/// <exception cref="OverflowException"><paramref name="numberOfElements"/> is not a valid array length.</exception>
		[Pure]
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
		/// <exception cref="ArgumentException"><paramref name="threeLetterIsoCode"/> is not defined.</exception>
		[Pure]
		public static Money[] Some(decimal amount, string threeLetterIsoCode, int numberOfElements)
		{
			return initArray(numberOfElements, () => new Money(amount, threeLetterIsoCode));
		}
	}
}
