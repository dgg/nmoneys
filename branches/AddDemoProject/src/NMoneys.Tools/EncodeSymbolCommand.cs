using System;
using NMoneys.Support;

namespace NMoneys.Tools
{
	internal class EncodeSymbolCommand : Command
	{
		protected override void DoExecute()
		{
			while (true)
			{
				Console.Write("\nInsert unicode text (Ctrl+C to exit): ");
				WL(UnicodeSymbol.FromSymbol(RL()).TokenizedCodePoints.Replace(" ", " | "));
			}
		}
	}
}
