using System.Diagnostics.Contracts;
using System.Globalization;

namespace NMoneys;

public partial struct Money
{
	/// <summary>
	/// Converts the string representation of a monetary quantity (<see cref="Money.AsQuantity()"/>) to its <see cref="Money"/> equivalent.
	/// </summary>
	/// <exception cref="ArgumentException">The currency value does not contain <see cref="CurrencyIsoCode"/> enumeration information.</exception>
	/// <exception cref="FormatException">The amount value is not in the correct <see cref="Decimal"/> format.</exception>
	/// <exception cref="UndefinedCodeException">The currency value does not contain a defined <see cref="CurrencyIsoCode"/>.</exception>
	/// <exception cref="ArgumentOutOfRangeException">Components of the representation are missing.</exception>
	/// <returns>Monetary quantity equivalent to its representation.</returns>
	[Pure]
	public static Money Parse(string quantity)
	{
		ReadOnlySpan<char> span = quantity.AsSpan();
		CurrencyIsoCode currency = Enum.Parse<CurrencyIsoCode>(span[..3], ignoreCase: false);
		currency.AssertDefined();
		decimal amount = decimal.Parse(span[4..],
			style: NumberStyles.Float,
			provider: NumberFormatInfo.InvariantInfo);
		return new Money(amount, currency);
	}

	/// <summary>
	/// Tries to converts the string representation of a monetary quantity (<see cref="Money.AsQuantity()"/>) to its <see cref="Money"/> equivalent.
	/// </summary>
	/// <param name="quantity"></param>
	/// <param name="money">When this method returns, contains the monetary quantity <see cref="Money"/> equivalent to its representation contained in <paramref name="quantity"/>,
	/// if the conversion succeeded, or <c>null</c> if the conversion failed.</param>
	/// <returns><c>true</c> if <paramref name="quantity"/> was converted successfully; otherwise, <c>false</c>.</returns>
	[Pure]
	public static bool TryParse(string quantity, out Money? money)
	{
		ReadOnlySpan<char> span = quantity.AsSpan();
		try
		{
			if (Enum.TryParse(span[..3], out CurrencyIsoCode currencyCode) &&
			    currencyCode.CheckDefined() &&
			    decimal.TryParse(span[4..], NumberStyles.Float, NumberFormatInfo.InvariantInfo, out decimal amount))
			{
				money = new Money(amount, currencyCode);
				return true;
			}
		}
#pragma warning disable CA1031
		catch
		{
			// span indexing operation can throw
		}
#pragma warning restore CA1031

		money = null;
		return false;
	}

	/// <summary>
	/// Converts the string representation of a monetary quantity to its <see cref="Money"/> equivalent
	/// using the <see cref="Currency"/> style and the specified currency as format information.
	/// </summary>
	/// <remaks>This method assumes <paramref name="s"/> to have a <see cref="Currency"/> style.</remaks>
	/// <param name="s">The string representation of the monetary quantity to convert.</param>
	/// <param name="currency">Expected currency of <paramref name="s"/> that provides format information.</param>
	/// <returns>The <see cref="Money"/> equivalent to the monetary quantity contained in <paramref name="s"/> as specified by the <paramref name="currency"/>.</returns>
	/// <exception cref="FormatException"><paramref name="s"/> is not in the correct format.</exception>
	/// <exception cref="OverflowException"><paramref name="s"/> represents a monetary quantity less than <see cref="decimal.MinValue"/> or greater than <see cref="decimal.MaxValue"/>.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="s"/> is null.</exception>
	/// <seealso cref="decimal.Parse(string, NumberStyles, IFormatProvider)" />
	[Pure]
	public static Money Parse(string s, Currency currency)
	{
		return Parse(s, NumberStyles.Currency, currency);
	}

	/// <summary>
	/// Converts the string representation of a monetary quantity to its <see cref="Money"/> equivalent
	/// using the specified style and the specified currency as format information.
	/// </summary>
	/// <remarks>Use this method when <paramref name="s"/> is suspected not to have a <see cref="NumberStyles.Currency"/> style or more control over the operation is needed.</remarks>
	/// <param name="s">The string representation of the monetary quantity to convert.</param>
	/// <param name="style">A bitwise combination of <see cref="NumberStyles"/> values that indicates the style elements that can be present in <paramref name="s"/>.
	/// A typical value to specify is <see cref="NumberStyles.Number"/>.</param>
	/// <param name="currency">Expected currency of <paramref name="s"/> that provides format information.</param>
	/// <returns>The <see cref="Money"/> equivalent to the monetary quantity contained in <paramref name="s"/> as specified by <paramref name="style"/> and <paramref name="currency"/>.</returns>
	/// <exception cref="FormatException"><paramref name="s"/> is not in the correct format.</exception>
	/// <exception cref="OverflowException"><paramref name="s"/> represents a monetary quantity less than <see cref="decimal.MinValue"/> or greater than <see cref="decimal.MaxValue"/>.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="s"/> is null.</exception>
	/// <exception cref="ArgumentException"><paramref name="style"/> is not a <see cref="NumberStyles"/> value
	/// <para>-or-</para>
	/// <paramref name="style"/> is the <see cref="NumberStyles.AllowHexSpecifier"/> value.
	/// </exception>
	/// <seealso cref="decimal.Parse(string, NumberStyles, IFormatProvider)" />
	[Pure]
	public static Money Parse(string s, NumberStyles style, Currency currency)
	{
		decimal amount = decimal.Parse(s, style, currency);

		return new Money(amount, currency);
	}

	/// <summary>
	/// Converts the string representation of a monetary quantity to its <see cref="Money"/> equivalent
	/// using <see cref="NumberStyles.Currency"/> and the provided currency as format information.
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
	[Pure]
	public static bool TryParse(string s, Currency currency, out Money? money)
	{
		return TryParse(s, NumberStyles.Currency, currency, out money);
	}

	/// <summary>
	/// Converts the string representation of a monetary quantity to its <see cref="Money"/> equivalent
	/// using the specified style and the provided currency as format information.
	/// A return value indicates whether the conversion succeeded or failed.
	/// </summary>
	/// <param name="s">The string representation of the monetary quantity to convert.</param>
	/// <param name="style">A bitwise combination of enumeration values that indicates the permitted format of <paramref name="s"/>.
	/// A typical value to specify is <see cref="NumberStyles.Number"/>.</param>
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
	/// <exception cref="ArgumentNullException"><paramref name="currency"/> is <c>null</c>.</exception>
	/// <seealso cref="decimal.TryParse(string, NumberStyles, IFormatProvider, out decimal)" />
	[Pure]
	public static bool TryParse(string s, NumberStyles style, Currency currency, out Money? money)
	{
		ArgumentNullException.ThrowIfNull(currency, nameof(currency));
		money = tryParse(s, style, currency.FormatInfo, currency.IsoCode);
		return money.HasValue;
	}

	/// <summary>
	/// Converts the string representation of a monetary quantity to its <see cref="Money" /> equivalent
	/// using the specified style, the <paramref name="numberFormatInfo" /> and the symbol of the provided
	/// <paramref name="currency" /> as format information.
	/// A return value indicates whether the conversion succeeded or failed.
	/// </summary>
	/// <param name="s">The string representation of the monetary quantity to convert.</param>
	/// <param name="style">A bitwise combination of enumeration values that indicates the permitted format of <paramref name="s"/>.
	/// A typical value to specify is <see cref="NumberStyles.Currency"/>.</param>
	/// <param name="numberFormatInfo">
	/// The format info for the textual representation of the currency. This may be different from
	/// how the currency would normally be represented in the country that uses it. For example, the
	/// <see cref="NumberFormatInfo" /> for en-GB would
	/// allow the string "€10,000.00" for <see cref="Currency.Eur" />, even though this would normally be written as
	/// "10.000,00 €"
	/// </param>
	/// <param name="currency">Resultant currency of <paramref name="s" /> (only symbol is used for formatting).</param>
	/// <param name="money">
	/// When this method returns, contains the <see cref="Money" /> that is equivalent to the monetary quantity
	/// contained in <paramref name="s" />,
	/// if the conversion succeeded, or is null if the conversion failed.
	/// The conversion fails if the <paramref name="s" /> parameter is null, is not in a format compliant with currency
	/// style,
	/// or represents a number less than <see cref="Decimal.MinValue" /> or greater than
	/// <see cref="Decimal.MaxValue" />.
	/// This parameter is passed uninitialized.
	/// </param>
	/// <returns> true if s was converted successfully; otherwise, false. </returns>
	/// <exception cref="ArgumentNullException"><paramref name="numberFormatInfo"/> or <paramref name="currency"/> is <c>null</c>.</exception>
	[Pure]
	public static bool TryParse(string s, NumberStyles style, NumberFormatInfo numberFormatInfo, Currency currency,
		out Money? money)
	{
		ArgumentNullException.ThrowIfNull(numberFormatInfo, nameof(numberFormatInfo));
		ArgumentNullException.ThrowIfNull(currency, nameof(currency));

		var mergedNumberFormatInfo = (NumberFormatInfo)numberFormatInfo.Clone();
		mergedNumberFormatInfo.CurrencySymbol = currency.Symbol;

		money = tryParse(s, style, mergedNumberFormatInfo, currency.IsoCode);

		return money.HasValue;
	}

	private static Money? tryParse(string s, NumberStyles style, NumberFormatInfo formatProvider,
		CurrencyIsoCode currency)
	{
		decimal amount;
		bool result = decimal.TryParse(s, style, formatProvider, out amount);
		return result ? new Money(amount, currency) : null;
	}
}
