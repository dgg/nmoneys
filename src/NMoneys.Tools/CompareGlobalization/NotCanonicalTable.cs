using System.Linq;
using ConsoleTables;
using NMoneys.Support;

namespace NMoneys.Tools.CompareGlobalization
{
	internal static class NotCanonicalTable
	{
		public static void Print(GlobalizationCurrencyInfo[] globalizationInfo, CurrencyInfo[] configurationInfo)
		{
			ConsoleTable notCanonical = new ConsoleTable("Code", "Globalization");

			var notDecorated = configurationInfo
				.Where(i => !Enumeration.HasAttribute<CurrencyIsoCode, CanonicalCultureAttribute>(i.Code))
				.ToArray();

			foreach (var fromConfiguration in notDecorated)
			{
				var fromGlobalization = globalizationInfo.Where(i => Currency.Code.Comparer.Equals(i.Info.Code, fromConfiguration.Code)).ToArray();
				for (int i = 0; i < fromGlobalization.Length; i++)
				{
					if (i == 0)
					{
						notCanonical.AddRow(fromConfiguration.Code, string.Empty);
					}
					string fromGlobalizationColumn = $"{fromGlobalization[i].Culture.Name} [{fromGlobalization[i].Culture.EnglishName}]";
					notCanonical.AddRow(string.Empty, fromGlobalizationColumn);
				}
			}

			notCanonical.Write(Format.Alternative);
		}
	}
}