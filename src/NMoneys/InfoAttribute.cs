using System.Reflection;

namespace NMoneys;

[AttributeUsage(AttributeTargets.Field)]
internal sealed class InfoAttribute : Attribute, ICurrencyInfo
{
	// See the attribute guidelines at
	//  http://go.microsoft.com/fwlink/?LinkId=85236
	public InfoAttribute(
		string englishName, string nativeName,
		string symbol,
		byte significantDecimalDigits,
		string decimalSeparator, string groupSeparator, byte[] groupSizes,
		byte negativePattern, byte positivePattern,
		bool isObsolete = false,
		ushort codePoint = 0,
		string? entityName = null
		)
	{
		EnglishName = englishName;
		NativeName = nativeName;
		Symbol = symbol;
		SignificantDecimalDigits = significantDecimalDigits;
		DecimalSeparator = decimalSeparator;
		GroupSeparator = groupSeparator;
		GroupSizes = groupSizes;
		NegativePattern = negativePattern;
		PositivePattern = positivePattern;
		IsObsolete = isObsolete;
		CodePoint = codePoint == 0 ? null: codePoint;
		EntityName = entityName;
	}

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

	public CurrencyInfo MergeWith(CurrencyConfiguration configuration)
	{
		CurrencyInfo merged = new CurrencyInfo(
			EnglishName: configuration.EnglishName ?? EnglishName,
			NativeName: configuration.NativeName ?? NativeName,
			Symbol: configuration.Symbol?? Symbol,
			SignificantDecimalDigits: configuration.SignificantDecimalDigits ?? SignificantDecimalDigits,
			DecimalSeparator: configuration.DecimalSeparator ?? DecimalSeparator,
			GroupSeparator: configuration.GroupSeparator ?? GroupSeparator,
			GroupSizes: configuration.GroupSizes ?? GroupSizes,
			NegativePattern: configuration.NegativePattern ?? NegativePattern,
			PositivePattern: configuration.PositivePattern ?? PositivePattern,
			IsObsolete: configuration.IsObsolete ?? IsObsolete,
			CodePoint: configuration.Reference.Item1 ?? CodePoint,
			EntityName: configuration.Reference.Item2 ?? EntityName
		);

		return merged;
	}

	public static InfoAttribute GetFrom(CurrencyIsoCode code)
	{
		FieldInfo field = code.FieldOf();
		InfoAttribute attribute = field.GetCustomAttribute<InfoAttribute>() ?? throw MisconfiguredCurrencyException.ForCode(code);
		return attribute;
	}

	public static bool TryGetFrom(CurrencyIsoCode code, out InfoAttribute? attribute)
	{
		FieldInfo field = code.FieldOf();
		attribute = field.GetCustomAttribute<InfoAttribute>();
		return attribute != null;
	}
}
