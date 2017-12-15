using System.Globalization;
using GoCommando;
using NMoneys.Support;
using static System.Console;

namespace NMoneys.Tools
{
	[Command("info")]
	[Description("Extracts the currency relevant information for the given culture name")]
	internal class ExtractInfoCommand : ICommand
    {
	    [Parameter("culture", "c")]
	    [Description("Name of the culture info")]
		public string Culture { get; set; }

		public void Run()
	    {
			CultureInfo ci = CultureInfo.GetCultureInfo(Culture);
		    RegionInfo ri = new RegionInfo(ci.LCID);
		    WriteLine("ri.CurrencyEnglishName: " + ri.CurrencyEnglishName);
		    WriteLine("ri.CurrencyNativeName: " + ri.CurrencyNativeName);
		    NumberFormatInfo numberFormat = ci.NumberFormat;
		    WriteLine("nf.CurrencySymbol: " + numberFormat.CurrencySymbol);
		    WriteLine("nf.CurrencyDecimalDigits: " + numberFormat.CurrencyDecimalDigits);
		    WriteLine("nf.CurrencyDecimalSeparator: " + numberFormat.CurrencyDecimalSeparator);
		    WriteLine("nf.CurrencyGroupSeparator: " + numberFormat.CurrencyGroupSeparator);
		    GroupSizes sizes = GroupSizes.FromSizes(numberFormat.CurrencyGroupSizes);
		    WriteLine("nf.CurrencyGroupSizes: " + sizes.TokenizedSizes);
		    WriteLine("nf.CurrencyPositivePattern: " + numberFormat.CurrencyPositivePattern);
		    WriteLine("nf.CurrencyNegativePattern: " + numberFormat.CurrencyNegativePattern);
		}
    }
}
