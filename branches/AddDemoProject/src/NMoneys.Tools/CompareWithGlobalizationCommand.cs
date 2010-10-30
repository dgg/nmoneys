using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using NMoneys.Support;
using NMoneys.Tools.Support;
using NUnit.Framework;

namespace NMoneys.Tools
{
	internal class CompareWithGlobalizationCommand : Command
	{
		protected override void DoExecute()
		{
			compareWithConfiguration(
				CultureInfo.GetCultures(CultureTypes.AllCultures)
				.Where(c => !c.IsNeutralCulture && !c.Equals(CultureInfo.InvariantCulture))
				.Select(CultureCurrencyInfo.TryBuild)
				.Where(ci => ci != null));
		}

		private void compareWithConfiguration(IEnumerable<CultureCurrencyInfo> fromGlobalization)
		{
			var writer = new TextMessageWriter();
			using (var initializer = new EmbeddedXmlInitializer())
			{
				foreach (var global in fromGlobalization)
				{
					CurrencyInfo notGlobal = initializer.Get(global.Info.Code);
					var comparer = new CurrencyInfoComparer(global.Culture);
					if (!comparer.Equals(global.Info, notGlobal))
					{
						var differences = comparer.ExtendedEquals(global.Info, notGlobal);
						displayDifferences(global, notGlobal, differences, writer);
					}
				}
			}
		}

		private void displayDifferences(CultureCurrencyInfo fromGlobalization, CurrencyInfo fromConfiguration, IEnumerable<KeyValuePair<string, Pair>> differences, TextMessageWriter writer)
		{
			displayCultureInformation(writer, fromGlobalization, fromConfiguration);
			
			foreach(var diff in differences)
			{
				displayDifference(writer, diff);
			}
			
			writer.Flush();
			WL(writer.ToString());
			StringBuilder sb = writer.GetStringBuilder();
			sb.Remove(0, sb.Length);
			RL();
		}

		private void displayCultureInformation(TextMessageWriter writer, CultureCurrencyInfo fromGlobalization, CurrencyInfo fromConfiguration)
		{
			CanonicalCultureAttribute attr;
			CultureInfo configurationCulture = null;
			if (Enumeration.TryGetAttribute(fromConfiguration.Code, out attr))
			{
				configurationCulture = attr.Culture();
			}
			writer.Write("Globalizaton Currency {0} for culture {1} [{2}] ", fromGlobalization.Info.Code, fromGlobalization.Culture.Name, fromGlobalization.Culture.EnglishName);
			writer.WriteLine("differs from Configuration Currency {0}", fromConfiguration.Code);
			writer.WriteMessageLine(1, "Canonical Culture {0}, {1}",
				configurationCulture != null ? configurationCulture.Name : "NONE",
				configurationCulture != null ? attr.Overwritten.ToString() : "NA");
		}

		private static void displayDifference(TextMessageWriter writer, KeyValuePair<string, Pair> difference)
		{
			writer.Write("\t");
			writer.Write(difference.Key);
			writer.Write(" --> ");
			writer.Write("globalization: ");
			writer.WriteExpectedValue(difference.Value.First);
			writer.Write("\t");
			writer.Write("configuration: ");
			writer.WriteActualValue(difference.Value.Second);
			writer.WriteLine();
		}
	}
}
