using ConsoleTables;
using GoCommando;
using static System.Console;

namespace NMoneys.Tools.CompareIso
{
	[Command("cmpIso")]
	[Description("Compares the information provided by the ISO.org page against NMoneys implementation")]
    internal class CompareIsoCommand : ICommand
    {
		public void Run()
	    {
		    var loader = new IsoCurrenciesLoader();
		    IsoCurrenciesCollection isoCurrencies = loader.LoadFrom(IsoCurrenciesLoader.IsoUrl);
		    ImplementedCurrenciesCollection implemented = new ImplementedCurrenciesCollection()
			    .AddRange(Currency.FindAll());

		    ConsoleTable implementedOnly = new ConsoleTable("Alpha-Code", "Name", "Obsolete?");
		    ConsoleTable isoOnly = new ConsoleTable("Alpha-Code", "Name", "Minor units");
		    ConsoleTable discrepancies = new ConsoleTable("Alpha-Code", "Name / ISO Name", "Minor units / ISO Units");

		    foreach (IsoCurrency iso in isoCurrencies)
		    {
			    Currency currency = implemented[iso.NumericCode.Value.GetValueOrDefault()];
			    if (currency != null)
			    {
				    CurrencyDiff diff = CurrencyDiff.Diff(currency, iso);
				    if (diff != null)
				    {
					    discrepancies.AddRow(currency.AlphabeticCode, diff.Name, diff.MinorUnits);
				    }
			    }
			    else
			    {
				    isoOnly.AddRow(iso.AlphabeticalCode, iso.Name, iso.MinorUnits.Value);
			    }
		    }

		    foreach (var currency in implemented.Except(isoCurrencies))
		    {
			    implementedOnly.AddRow(currency.AlphabeticCode, currency.EnglishName, currency.IsObsolete);
		    }

		    WriteLine("The following currencies are defined only in the implemented set:");
		    implementedOnly.Write(Format.Alternative);

		    WriteLine("The following currencies are defined only in the ISO web:");
		    isoOnly.Write(Format.Alternative);

		    WriteLine("The following currencies have different information in the ISO web than the one implemented:");
		    discrepancies.Write(Format.Alternative);
		}
    }
}
