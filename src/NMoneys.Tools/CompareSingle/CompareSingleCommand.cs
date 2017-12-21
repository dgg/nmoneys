using System;
using System.IO;
using GoCommando;
using NMoneys.Support;

namespace NMoneys.Tools.CompareSingle
{
	[Command("cmpSingle")]
	public class CompareSingleCommand : ICommand
	{
		[Parameter("currency", "c")]
		[Description("Currency to compare against ISO and canonical globalization")]
		public string CurrencyCode { get; set; }

		[Parameter("isoFile", "f", optional: true)]
		[Description("Path to the offline file that contains ISO information in case we are short on network")]
		public string IsoFilePath { get; set; }

		public void Run()
		{
			CurrencyIsoCode currency = Currency.Code.Parse(CurrencyCode);

			CanonicalCultureAttribute attribute = ensureCanonical(currency);
			ICurrencyInfoProvider provider = new EmbeddedXmlProvider();
			CurrencyInfo fromConfiguration = provider.Get(currency);
			Console.WriteLine(Environment.NewLine);
			new GlobalizationDiscrepanciesTable().Print(attribute.Culture(), fromConfiguration);

			FileInfo cache = Path.IsPathRooted(IsoFilePath) ? new FileInfo(IsoFilePath) : null;
			new IsoDiscrepanciesTable().Print(currency, cache);
		}

		private CanonicalCultureAttribute ensureCanonical(CurrencyIsoCode currency)
		{
			
			if (!Enumeration.TryGetAttribute(currency, out CanonicalCultureAttribute attribute))
			{
				writeError($"Currency '{currency}' not decorated with '{nameof(CanonicalCultureAttribute)}'.");
				Environment.Exit(-1);
			}
			return attribute;
		}

		private void writeError(string message)
		{
			ConsoleColor prev = Console.ForegroundColor;
			Console.ForegroundColor = ConsoleColor.Red;
			Console.Error.WriteLine(message);
			Console.ForegroundColor = prev;
		}
	}
}