using GoCommando;
using NMoneys.Support;
using static System.Console;

namespace NMoneys.Tools
{
	[Command("encode")]
	[Description("Extracts the codepoints that make up a given string")]
	internal class EncodeCommand : ICommand
	{
		[Parameter("text", "t")]
		[Description("Unicode text to encode")]
		public string Text { get; set; }

		public void Run()
		{
			WriteLine(UnicodeSymbol.FromSymbol(Text).TokenizedCodePoints.Replace(" ", " | "));
		}
	}
}