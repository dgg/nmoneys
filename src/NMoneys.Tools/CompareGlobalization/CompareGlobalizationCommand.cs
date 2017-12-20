using System.Linq;
using GoCommando;
using NMoneys.Support;

namespace NMoneys.Tools.CompareGlobalization
{
	[Command("cmpGlobal")]
	[Description("Compares the information provided by System.Globalization against NMoneys implementation")]
	internal class CompareGlobalizationCommand : ICommand
	{
		public void Run()
		{
			GlobalizationCurrencyInfo[] globalizationInfo = GlobalizationCurrencyInfo
				.LoadFromGlobalization()
				.ToArray();

			CurrencyInfo[] configurationInfo;
			using (var initializer = new EmbeddedXmlInitializer())
			{

				configurationInfo = Enumeration.GetValues<CurrencyIsoCode>().Select(c => initializer.Get(c)).ToArray();
			}

			/*ConsoleTable discrepancies = new ConsoleTable("Code", "Globalization", "Configuration");
			ConsoleTable nonDecorated = new ConsoleTable("Code", "Globalization", "Configuration (overwritten)");*/

			
			MultiCanonicalTable.Print(globalizationInfo, configurationInfo);

			/*printNonCanonical(globalizationInfo, configurationInfo);
			printDiscrepancies(globalizationInfo, configurationInfo);

				foreach (GlobalizationCurrencyInfo fromGlobalization in globalizationInfo)
				{
					CurrencyInfo fromConfiguration = initializer.Get(fromGlobalization.Info.Code);
					if (!fromGlobalization.Equals(fromConfiguration))
					{
						Enumeration.TryGetAttribute(fromConfiguration.Code, out CanonicalCultureAttribute canonicalAttr);
						CultureInfo canonicalCulture = canonicalAttr?.Culture();
						if (canonicalCulture != null && !canonicalCulture.Name.Equals(fromGlobalization.Culture.Name))
						{
							string globalization = $"{fromGlobalization.Culture.Name} [{fromGlobalization.Culture.EnglishName}]";
							string overwritten = canonicalAttr.Overwritten ? "*" : string.Empty;
							string configuration = $"{canonicalCulture.Name} [{canonicalCulture.EnglishName}] {overwritten}";
							nonDecorated.AddRow(fromGlobalization.Info.Code, globalization, configuration);
						}
						else
						{
							var differences = fromGlobalization.Compare(fromConfiguration);
							if (differences != null)
							{
								discrepancies.AddRow(fromConfiguration.Code, string.Empty, string.Empty);
								foreach (var diff in differences)
								{
									discrepancies.AddRow("   " + diff.PropertyName, diff.Globalization, diff.Configuration);
								}
							}
						}
					}
				}

				nonDecorated.Write(Format.Alternative);
				discrepancies.Write(Format.Alternative);*/
			}
	}
}