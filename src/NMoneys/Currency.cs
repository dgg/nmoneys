using System.Globalization;
using System.Xml.Serialization;
using NMoneys.Support;

namespace NMoneys;

///<summary>
/// Represents a currency unit such as Euro or American Dollar.
///</summary>
public sealed partial class Currency
{
	#region properties

	/// <summary>
	/// The ISO 4217 code of the <see cref="Currency"/>
	/// </summary>
	public CurrencyIsoCode IsoCode { get; private set; }

	/// <summary>
	/// The numeric ISO 4217 code of the <see cref="Currency"/>
	/// </summary>
	public short NumericCode => IsoCode.NumericCode();

	/// <summary>
	/// Returns a padded three digit string representation of the <see cref="NumericCode"/>.
	/// </summary>
	public string PaddedNumericCode => IsoCode.PaddedNumericCode();

	/// <summary>
	/// Gets the name, in English, of the <see cref="Currency"/>.
	/// </summary>
	[XmlIgnore]
	public string EnglishName { get; private set; }

	/// <summary>
	/// Gets the currency symbol associated with the <see cref="Currency"/>.
	/// </summary>
	[XmlIgnore]
	public string Symbol { get; private set; }

	/// <summary>
	/// Textual representation of the ISO 4217 code
	/// </summary>
	public string AlphabeticCode { get; private set; }

	/// <summary>
	/// Textual representation of the ISO 4217 code
	/// </summary>
	[XmlIgnore]
	public string IsoSymbol { get; private set; }

	/// <summary>
	/// Indicates the number of decimal places to use in currency values.
	/// </summary>
	[XmlIgnore]
	public byte SignificantDecimalDigits { get; private set; }

	/// <summary>
	/// Represents the smallest amount that can be represented for the currency according to its <see cref="SignificantDecimalDigits"/>.
	/// </summary>
	[XmlIgnore]
	public decimal MinAmount => SmallPowerOfTen.Negative(SignificantDecimalDigits);

	/// <summary>
	/// Gets the name of the currency formatted in the native language of the country/region where the currency is used.
	/// </summary>
	[XmlIgnore]
	public string NativeName { get; private set; }

	/// <summary>
	/// Gets the string to use as the decimal separator in currency values.
	/// </summary>
	[XmlIgnore]
	public string DecimalSeparator { get; private set; }

	/// <summary>
	/// Gets the string that separates groups of digits to the left of the decimal in currency values.
	/// </summary>
	[XmlIgnore]
	public string GroupSeparator { get; private set; }

	/// <summary>
	/// Gets the number of digits in each group to the left of the decimal in currency values.
	/// </summary>
	[XmlIgnore]
#pragma warning disable CA1819
	public byte[] GroupSizes { get; private set; }
#pragma warning restore CA1819

	/// <summary>
	/// Gets format pattern for negative currency values.
	/// </summary>
	/// <remarks>For more information about this pattern see <see cref="NumberFormatInfo.CurrencyNegativePattern"/>.</remarks>
	[XmlIgnore]
	public int NegativePattern { get; private set; }

	/// <summary>
	/// Gets the format pattern for positive currency values.
	/// </summary>
	/// <remarks>For more information about this pattern see <see cref="NumberFormatInfo.CurrencyPositivePattern"/>.</remarks>
	[XmlIgnore]
	public int PositivePattern { get; private set; }

	/// <summary>
	/// Defines how numeric values are formatted and displayed, depending on the culture related to the <see cref="Currency"/>.
	/// </summary>
	[XmlIgnore]
	public NumberFormatInfo FormatInfo { get; private set; }

	/// <summary>
	/// Indicates whether the currency is legal tender or it has been obsoleted
	/// </summary>
	[XmlIgnore]
	public bool IsObsolete { get; private set; }

	/// <summary>
	/// Represents the character reference for the currency
	/// </summary>
	/// <remarks>Not all currencies have an character reference.
	/// For those who does not have one, a <c>null</c> instance is returned.</remarks>
	[XmlIgnore]
	public CharacterReference? Entity { get; private set; } = null;

	/// <summary>
	/// Gets the default currency symbol.
	/// </summary>
	public static readonly string DefaultSymbol = CultureInfo.InvariantCulture.NumberFormat.CurrencySymbol;

	#endregion

	#region ctors

#pragma warning disable CS8618
	[Obsolete("serialization")]
	private Currency()
	{
	}

	internal Currency(CurrencyIsoCode code, ICurrencyInfo info)
	{
		setAllFields(code, info);
	}
#pragma warning restore CS8618

	private void setAllFields(CurrencyIsoCode code, ICurrencyInfo info)
	{
		IsoCode = code;
		AlphabeticCode = IsoSymbol = code.ToString();
		EnglishName = info.EnglishName;
		NativeName = info.NativeName;
		Symbol = info.Symbol;
		SignificantDecimalDigits = info.SignificantDecimalDigits;
		DecimalSeparator = info.DecimalSeparator;
		GroupSeparator = info.GroupSeparator;
		GroupSizes = info.GroupSizes;
		PositivePattern = info.PositivePattern;
		NegativePattern = info.NegativePattern;
		IsObsolete = info.IsObsolete;
		if (info.CodePoint.HasValue)
		{
			Entity = new CharacterReference(info.CodePoint.Value, info.EntityName);
		}

		FormatInfo = CurrencyInfo.ToFormatInfo(info);
	}

	#endregion

	private static Currencies Currencies { get; } = new();
}
