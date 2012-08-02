using System;
using System.Globalization;

namespace NMoneys
{
	public partial struct Money
	{
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
		/// A typical value to specify is <see cref="NumberStyles.Number"/>.</param>
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
		/// <seealso cref="decimal.TryParse(string, NumberStyles, IFormatProvider, out decimal)" />
		public static bool TryParse(string s, NumberStyles style, Currency currency, out Money? money)
		{
			decimal amount;
			bool result = decimal.TryParse(s, style, currency, out amount);
			money = result ? new Money(amount, currency) : (Money?)null;
			return result;
		}
	}
}
