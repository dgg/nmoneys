using System.IO;
using System.Linq;
using NMoneys.Tools.CompareIso;

namespace NMoneys.Tools.CompareSingle
{
	internal class IsoDiscrepanciesTable : DiscrepanciesTable
	{
		public IsoDiscrepanciesTable() : base("Alpha-Code", "Name / ISO Name", "Minor units / ISO Units") { }

		public void Print(CurrencyIsoCode currency, FileInfo file)
		{
			IsoCurrency fromIso = loadIsoCurrency(file, currency);
			
			pushDiff(fromIso, Currency.Get(fromIso.AlphabeticalCode));

			MaybeWrite("discrepancies with iso.org");
		}

		private void pushDiff(IsoCurrency fromIso, Currency fromConfiguration)
		{
			CurrencyDiff diff = CurrencyDiff.Diff(fromConfiguration, fromIso);
			if (diff != null)
			{
				AddRow(fromIso.AlphabeticalCode, diff.Name, diff.MinorUnits);
			}
		}

		private IsoCurrency loadIsoCurrency(FileInfo file, CurrencyIsoCode currency)
		{
			var loader = new IsoCurrenciesLoader();
			var collection = file?.Exists ?? false ?
				loader.LoadFrom(file.OpenRead()) :
				loader.LoadFrom(IsoCurrenciesLoader.IsoUrl);

			short currencyCode = currency.NumericCode();
			return collection.Single(c => c.NumericCode.Value.GetValueOrDefault() == currencyCode);
		}
	}
}