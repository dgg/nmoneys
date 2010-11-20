using System;
using System.Globalization;
using NMoneys.Support;

namespace NMoneys.Tools
{
	internal class ExtractInfoFromCulture : Command
	{
		protected override void DoExecute()
		{
			WL(Environment.Version);
			while (true)
			{
				Console.Write("\nInsert culture info name (Ctrl+C to exit): ");
				CultureInfo cultureInfo = CultureInfo.GetCultureInfo(RL());
				RegionInfo info2 = new RegionInfo(cultureInfo.LCID);
				WL("ri.CurrencyEnglishName: " + info2.CurrencyEnglishName);
				WL("ri.CurrencyNativeName: " + info2.CurrencyNativeName);
				NumberFormatInfo numberFormat = cultureInfo.NumberFormat;
				WL("nf.CurrencySymbol: " + numberFormat.CurrencySymbol);
				WL("nf.CurrencyDecimalDigits: " + numberFormat.CurrencyDecimalDigits);
				WL("nf.CurrencyDecimalSeparator: " + numberFormat.CurrencyDecimalSeparator);
				WL("nf.CurrencyGroupSeparator: " + numberFormat.CurrencyGroupSeparator);
				GroupSizes sizes = GroupSizes.FromSizes(numberFormat.CurrencyGroupSizes);
				WL("nf.CurrencyGroupSizes: " + sizes.TokenizedSizes);
				WL("nf.CurrencyPositivePattern: " + numberFormat.CurrencyPositivePattern);
				WL("nf.CurrencyNegativePattern: " + numberFormat.CurrencyNegativePattern);
			}
		}
	}
}
