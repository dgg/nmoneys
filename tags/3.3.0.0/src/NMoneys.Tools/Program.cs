using System;
using Mono.Options;

namespace NMoneys.Tools
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine(Environment.Version);
			Console.ReadLine();
			bool help = true;

			OptionSet set = new OptionSet()
				.Add("eic", "Extracts the currency relevant information for the given culture name",
					option =>
					{
						help = false;
						new ExtractInfoFromCulture().Execute();
					})
				.Add("es", "Extracts the codepoints that make up a given string",
					option =>
					{
						help = false;
						new EncodeSymbol().Execute();
					})
				.Add("cg", "Compares the information provided by the culture in .Net with the information provided in the config file",
					option =>
					{
						help = false;
						new CompareWithGlobalization().Execute();
					})
				.Add("sw", "Scraped the information contained in the ISO.org web page for the 4217 standard",
					option =>
					{
						help = false;
						new ScrapeWebsite().Execute();
					})
				.Add("h|?|help", "Displays Help", opt => help = true);

			try
			{
				set.Parse(args);
			}
			catch (OptionException)
			{
				showHelp(set);
			}
			if (help) showHelp(set);

		}

		private static void showHelp(OptionSet set)
		{
			set.WriteOptionDescriptions(Console.Error);
			Environment.Exit(-1);
		}
	}
}
