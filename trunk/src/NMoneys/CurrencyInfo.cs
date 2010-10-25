using System.Text;
using NMoneys.Support.Ext;

namespace NMoneys
{
	internal sealed class CurrencyInfo
	{
		public CurrencyInfo(CurrencyIsoCode code, string englishName, string nativeName, string tokenizedSymbol, int significantDecimalDigits, string decimalSeparator, string groupSeparator, string tokenizedGroupSizes, int positivePattern, int negativePattern)
		{
			Code = code;
			EnglishName = englishName;
			NativeName = nativeName;

			Symbol = Support.UnicodeSymbol.FromTokenizedCodePoints(tokenizedSymbol).Symbol;
			SignificantDecimalDigits = significantDecimalDigits;
			DecimalSeparator = decimalSeparator;
			GroupSeparator = groupSeparator;
			GroupSizes = Support.GroupSizes.FromTokenizedSizes(tokenizedGroupSizes).Sizes;
			PositivePattern = positivePattern;
			NegativePattern = negativePattern;
		}

		public CurrencyIsoCode Code { get; private set; }
		public string EnglishName { get; private set; }
		public string NativeName { get; private set; }

		public string Symbol { get; private set; }
		public int SignificantDecimalDigits { get; private set; }
		public string DecimalSeparator { get; private set; }
		public string GroupSeparator { get; private set; }
		public int[] GroupSizes { get; private set; }
		public int PositivePattern { get; private set; }
		public int NegativePattern { get; private set; }

		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendFormat("Code {0}", Code);
			sb.AppendLine();
			sb.AppendFormat("EnglishName {0}", EnglishName);
			sb.AppendLine();
			sb.AppendFormat("NativeName {0}", NativeName);
			sb.AppendLine();
			sb.AppendFormat("Symbol {0}", Symbol);
			sb.AppendLine();
			sb.AppendFormat("SignificantDecimalDigits {0}", SignificantDecimalDigits);
			sb.AppendLine();
			sb.AppendFormat("DecimalSeparator {0}", DecimalSeparator);
			sb.AppendLine();
			sb.AppendFormat("GroupSeparator {0}", GroupSeparator);
			sb.AppendLine();
			sb.Append("GroupSizes ");
			sb.Append("[");
			sb.Append(GroupSizes.ToDelimitedString());
			sb.Append("]");
			sb.AppendLine();
			sb.AppendFormat("PositivePattern {0}", PositivePattern);
			sb.AppendLine();
			sb.AppendFormat("NegativePattern {0}", NegativePattern);
			sb.AppendLine();
			return sb.ToString();
		}

		internal static ICurrencyInfoProvider CreateProvider()
		{
			return new EmbeddedXmlProvider();
		}

		internal static ICurrencyInfoInitializer CreateInitializer()
		{
			return new EmbeddedXmlInitializer();
		}
	}
}
