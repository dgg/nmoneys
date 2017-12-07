using System.Text;
using NMoneys.Support.Ext;

namespace NMoneys
{
	internal sealed class CurrencyInfo
	{
		public CurrencyInfo(CurrencyIsoCode code, string englishName, string nativeName, string tokenizedSymbol, int significantDecimalDigits, string decimalSeparator, string groupSeparator, string tokenizedGroupSizes, int positivePattern, int negativePattern, bool obsolete, CharacterReference entity)
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

			Obsolete = obsolete;
			Entity = entity;
		}

		public CurrencyIsoCode Code { get; }
		public string EnglishName { get; }
		public string NativeName { get; }

		public string Symbol { get; }
		public int SignificantDecimalDigits { get; }
		public string DecimalSeparator { get; }
		public string GroupSeparator { get; }
		public int[] GroupSizes { get; }
		public int PositivePattern { get; }
		public int NegativePattern { get; }
		public bool Obsolete { get; }
		public CharacterReference Entity { get; }


		/// <inheritdoc />
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
			printDecimals(sb);
			printGroups(sb);
			printPatterns(sb);
			sb.AppendFormat("Obsolete {0}", Obsolete);
			sb.AppendLine();
			return sb.ToString();
		}

		private void printPatterns(StringBuilder sb)
		{
			sb.AppendFormat("PositivePattern {0}", PositivePattern);
			sb.AppendLine();
			sb.AppendFormat("NegativePattern {0}", NegativePattern);
			sb.AppendLine();
		}

		private void printGroups(StringBuilder sb)
		{
			sb.AppendFormat("GroupSeparator {0}", GroupSeparator);
			sb.AppendLine();
			sb.Append("GroupSizes ");
			sb.Append("[");
			sb.Append(GroupSizes.ToDelimitedString());
			sb.Append("]");
			sb.AppendLine();
		}

		private void printDecimals(StringBuilder sb)
		{
			sb.AppendFormat("SignificantDecimalDigits {0}", SignificantDecimalDigits);
			sb.AppendLine();
			sb.AppendFormat("DecimalSeparator {0}", DecimalSeparator);
			sb.AppendLine();
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
