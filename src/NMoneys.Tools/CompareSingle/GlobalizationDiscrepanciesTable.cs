using System.Globalization;
using System.Linq;
using NMoneys.Tools.CompareGlobalization;

namespace NMoneys.Tools.CompareSingle
{
	internal class GlobalizationDiscrepanciesTable : DiscrepanciesTable
	{
		public GlobalizationDiscrepanciesTable() : base("Property", "Configuration", "Globalization") { }

		public void Print(CultureInfo canonical, CurrencyInfo fromConfiguration)
		{
			GlobalizationCurrencyInfo fromGlobalization = loadGlobalizationCurrency(canonical);

			DiffCollection diffs = fromGlobalization.Compare(fromConfiguration);
			if (diffs != null)
			{
				foreach (var diff in diffs)
				{
					AddRow(diff.PropertyName, diff.Configuration, diff.Globalization);
				}
			}
			MaybeWrite("discrepancies with System.Globalization");
		}

		private GlobalizationCurrencyInfo loadGlobalizationCurrency(CultureInfo canonical)
		{
			GlobalizationCurrencyInfo globalizationInfo = GlobalizationCurrencyInfo
				.LoadFromGlobalization()
				.Single(i => i.Culture.Equals(canonical));

			return globalizationInfo;
		}
	}
}