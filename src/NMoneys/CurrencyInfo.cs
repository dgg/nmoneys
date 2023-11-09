using System.Globalization;
using NMoneys.Support;

namespace NMoneys;

internal interface ICurrencyInfo
{
	public string EnglishName { get; }
	public string NativeName { get; }
	public string Symbol { get; }
	public byte SignificantDecimalDigits { get; }
	public string DecimalSeparator { get; }
	public string GroupSeparator { get; }
	public byte[] GroupSizes { get; }
	public byte NegativePattern { get; }
	public byte PositivePattern { get; }
	public bool IsObsolete { get; }

	public ushort? CodePoint { get; }
	public string? EntityName { get; }
}

internal record CurrencyInfo(
	string EnglishName,
	string NativeName,
	string Symbol,
	byte SignificantDecimalDigits,
	string DecimalSeparator,
	string GroupSeparator,
	byte[] GroupSizes,
	byte NegativePattern,
	byte PositivePattern,
	bool IsObsolete,
	ushort? CodePoint, string? EntityName
	) : ICurrencyInfo
{

	public static NumberFormatInfo ToFormatInfo(ICurrencyInfo info)
	{
		int[] groupSizes = Array.ConvertAll<byte, int>(info.GroupSizes, input => input);
		NumberFormatInfo formatInfo = NumberFormatInfo.ReadOnly(new NumberFormatInfo
		{
			CurrencySymbol = info.Symbol,
			CurrencyDecimalDigits = info.SignificantDecimalDigits,
			CurrencyDecimalSeparator = info.DecimalSeparator,
			CurrencyGroupSeparator = info.GroupSeparator,
			CurrencyGroupSizes = groupSizes,
			CurrencyPositivePattern = info.PositivePattern,
			CurrencyNegativePattern = info.NegativePattern,
			NumberDecimalDigits = info.SignificantDecimalDigits,
			NumberDecimalSeparator = info.DecimalSeparator,
			NumberGroupSeparator = info.GroupSeparator,
			NumberGroupSizes = groupSizes,
			NumberNegativePattern = CurrencyNegativePattern.TranslateToNumberNegativePattern(info.NegativePattern),
		});
		return formatInfo;
	}
}
