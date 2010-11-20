using System;
using Mono.Options;

namespace NMoneys.Tools
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine(Environment.Version);
			bool help = true;

			OptionSet set = new OptionSet()
				.Add("i", "Extracts the currency relevat information for the given culture name",
					option =>
					{
						help = false;
						new ExtractCurrencyInfoCommand().Execute();
					})
				.Add("e", "Extracts the codepoints that make up a given string",
					option =>
					{
						help = false;
						new EncodeSymbolCommand().Execute();
					})
				.Add("c", "Compares the information provided by the culture in .Net with the information provided in the config file",
					option =>
					{
						help = false;
						new CompareWithGlobalizationCommand().Execute();
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
