using System.Linq;
using GoCommando;
using NMoneys.Support;

namespace NMoneys.Tools.CompareGlobalization
{
	[Command("cmpGlobal")]
	[Description("Compares the information provided by System.Globalization against NMoneys implementation")]
	internal class CompareGlobalizationCommand : ICommand
	{
		[Parameter("all", "a", optional: true)]
		[Description("Show all of the tables")]
		public bool All { get; set; }

		[Parameter("multiCanonical", "m", optional: true)]
		[Description("Show currencies implemented in multiple configuration cultures")]
		public bool MultiCanonical { get; set; }

		[Parameter("discrepancies", "d", optional: true)]
		[Description("Show discrepancies between currencies and the canonical implementation in globalization")]
		public bool Discrepancies { get; set; }

		[Parameter("notCanonical", "n", optional: true)]
		[Description("Show currencies not decorated with the canonical attribute but for which there is a globalization culture that implements it")]
		public bool NotCanonical { get; set; }

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

			if (MultiCanonical || All)
			{
				MultiCanonicalTable.Print(globalizationInfo, configurationInfo);
			}
			if (Discrepancies || All)
			{
				DiscrepanciesTable.Print(globalizationInfo, configurationInfo);
			}
			if (NotCanonical || All)
			{
				NotCanonicalTable.Print(globalizationInfo, configurationInfo);
			}
			
		}
	}
}
