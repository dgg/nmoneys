using System.Diagnostics.Contracts;

namespace NMoneys;

public  partial struct Money
{
	/// <summary>
	/// Converts the numeric value of the <see cref="Amount"/> to its equivalent string representation using an instance of the <see cref="Currency"/>
	/// identified by <see cref="CurrencyCode"/> for culture-specific format information.
	/// </summary>
	/// <remarks>The return value is formatted with the currency numeric format specifier ("C").</remarks>
	/// <returns>The string representation of the value of this instance as specified by the <c>"Currency"</c> format specifier.</returns>
	[Pure]
	public override string ToString()
	{
#pragma warning disable CA1305
		return ToString("C");
#pragma warning restore CA1305
	}

	/// <summary>
	/// Converts the numeric value of the <see cref="Amount"/> to its equivalent string representation using an instance of the <see cref="Currency"/>
	/// identified by <see cref="CurrencyCode"/> for culture-specific format information.
	/// </summary>
	/// <param name="format">A numeric format string</param>
	/// <returns>The string representation of the value of this instance as specified by the format specifier and an instance of the <see cref="Currency"/>
	/// identified by <see cref="CurrencyCode"/> as the provider.</returns>
	[Pure]
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
	[Pure]
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
	[Pure]
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
		[Pure]
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
		[Pure]
		public string Format(string format, IFormatProvider provider)
		{
			Currency currency = Currency.Get(CurrencyCode);
			return string.Format(provider, format, Amount, currency.Symbol, currency.IsoCode, currency.EnglishName, currency.NativeName);
		}
}
