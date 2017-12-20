using System.Globalization;
using System.Linq;
using ConsoleTables;
using NMoneys.Support;

namespace NMoneys.Tools.CompareGlobalization
{
	internal static class MultiCanonicalTable
	{
		public static void Print(GlobalizationCurrencyInfo[] globalizationInfo, CurrencyInfo[] configurationInfo)
		{
			ConsoleTable multiCanonical = new ConsoleTable("Code", "Canonical", "Globalization", "Equal to Canonical?");

			var configurationMap = configurationInfo.ToDictionary(i => i.Code, Currency.Code.Comparer);

			var cmp = new AlphaComparer();
			foreach (var group in globalizationInfo.GroupBy(i => i.Info.Code).OrderBy(g => g.Key, cmp))
			{
				GlobalizationCurrencyInfo[] fromGlobalization = group.ToArray();
				if (fromGlobalization.Length > 1)
				{
					CurrencyInfo fromConfiguration = configurationMap[group.Key];
					Enumeration.TryGetAttribute(fromConfiguration.Code, out CanonicalCultureAttribute canonicalAttr);
					CultureInfo canonicalCulture = canonicalAttr?.Culture();
					if (canonicalCulture != null)
					{
						string overwritten = canonicalAttr.Overwritten ? "*" : string.Empty;
						string canonicalColumn = $"{canonicalCulture.Name} [{canonicalCulture.EnglishName}] {overwritten}";

						multiCanonical.AddRow(group.Key, canonicalColumn, string.Empty, string.Empty);
						foreach (var notCanonical in fromGlobalization.Where(i => !i.Culture.Equals(canonicalCulture)))
						{
							string notCanonicalColumn = $"{notCanonical.Culture.Name} [{notCanonical.Culture.EnglishName}]";
							string equalityColumn = notCanonical.Equals(fromConfiguration) ? "=" : "!=";
							multiCanonical.AddRow(string.Empty, string.Empty, notCanonicalColumn, equalityColumn);
						}
					}
				}
			}

			multiCanonical.Write(Format.Alternative);
		}
	}
}