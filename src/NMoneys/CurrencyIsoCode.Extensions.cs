using System.Globalization;

namespace NMoneys
{
	/// <summary>
	/// Contains extension to the type <see cref="CurrencyIsoCode"/>
	/// </summary>
	public static class IsoCodeExtensions
	{
		private const string EQUAL = " = ";

		/// <summary>
		/// Returns a combination of the ISO 4217 code and its numeric value, separated by the equals sign '<code>=</code>'.
		/// </summary>
		public static string AsValuePair(this CurrencyIsoCode isoCode)
		{
			return isoCode + EQUAL + isoCode.NumericCode();
		}

		/// <summary>
		/// The numeric ISO 4217 code of the <see cref="CurrencyIsoCode"/>
		/// </summary>
		public static short NumericCode(this CurrencyIsoCode isoCode)
		{
			return (short)isoCode;
		}

		/// <summary>
		/// Returns a padded three digit string representation of the <see cref="NumericCode"/>.
		/// </summary>
		public static string PaddedNumericCode(this CurrencyIsoCode isoCode)
		{
			return isoCode.NumericCode().ToString("000", CultureInfo.InvariantCulture);
		}

		/// <summary>
		/// The alphabetic ISO 4217 code of the <see cref="CurrencyIsoCode"/>
		/// </summary>
		public static string AlphabeticCode(this CurrencyIsoCode isoCode)
		{
			return isoCode.ToString();
		}
	}
}
