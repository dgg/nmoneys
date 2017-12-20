using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using ConsoleTables;
using NMoneys.Support;

namespace NMoneys.Tools.CompareGlobalization
{
	internal static class DiscrepanciesTable
	{
		public static void Print(GlobalizationCurrencyInfo[] globalizationInfo, CurrencyInfo[] configurationInfo)
		{
			ConsoleTable discrepancies = new ConsoleTable("Code", "Configuration", "Globalization");

			Dictionary<string, GlobalizationCurrencyInfo> globalizationMap = globalizationInfo.ToDictionary(i => i.Culture.Name, i => i, StringComparer.Ordinal);

			var decoratedFromConfiguration = configurationInfo
				.Select(i =>
				{
					Enumeration.TryGetAttribute(i.Code, out CanonicalCultureAttribute canonicalAttr);
					return new { Canonical = canonicalAttr, FromConfiguration = i };
				})
				.Where(a => a.Canonical != null);

			foreach (var fromConfiguration in decoratedFromConfiguration)
			{
				var fromGlobalization = globalizationMap[fromConfiguration.Canonical.Name];
				var differences = fromGlobalization.Compare(fromConfiguration.FromConfiguration);
				if (differences != null)
				{
					CultureInfo canonicalCulture = fromConfiguration.Canonical.Culture();
					string overwritten = fromConfiguration.Canonical.Overwritten ? "*" : string.Empty;
					string configurationColumn = $"{canonicalCulture.Name} [{canonicalCulture.EnglishName}] {overwritten}";
					discrepancies.AddRow(fromGlobalization.Info.Code, configurationColumn, string.Empty);
					foreach (var diff in differences)
					{
						discrepancies.AddRow("   " + diff.PropertyName, diff.Configuration, diff.Globalization);
					}
				}
			}

			discrepancies.Write(Format.Alternative);
		}
	}
}