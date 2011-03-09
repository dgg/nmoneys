using System;
using System.Globalization;
using System.Linq;

namespace NMoneys.Support
{
	/// <summary>
	/// Holds the different way a unicode encoded curency symbol can be represented.
	/// </summary>
	internal class UnicodeSymbol : TokenizedValue
	{
		/// <summary>
		/// Collection of Unicode code points for each character of the symbol
		/// </summary>
		/// <remarks>Used to construct the unicode <see cref="Symbol"/>.</remarks>
		public int[] CodePoints { get; private set; }

		/// <summary>
		/// Space-tokenized collection of code points (as strings) for each character of the symbol.
		/// </summary>
		/// <remarks>Used to represent a complex symbol in the <see cref="CurrencyInfo"/> storage.</remarks>
		public string TokenizedCodePoints { get; private set; }

		/// <summary>
		/// Complex Unicode symbol.
		/// </summary>
		public string Symbol { get; private set; }

		private UnicodeSymbol(int[] codePoints, string symbol)
		{
			CodePoints = codePoints;
			TokenizedCodePoints = join(codePoints.Select(cp => cp.ToString(CultureInfo.InvariantCulture)).ToArray());
			Symbol = symbol;
		}

		/// <summary>
		/// Creates an instance of <see cref="UnicodeSymbol"/> from the space-tokenized collection of
		/// integers that represent each character in the symbol.
		/// </summary>
		/// <param name="tokenizedCodePoints">Space-tokenized collection of code points (as strings) for each character of the symbol.</param>
		/// <returns>Instace with the different representations of a complex Unicode symbol.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="tokenizedCodePoints"/> is null.</exception>
		public static UnicodeSymbol FromTokenizedCodePoints(string tokenizedCodePoints)
		{
			Guard.AgainstNullArgument("tokenizedCodePoints", tokenizedCodePoints);

			int[] codePoints = 
				split(tokenizedCodePoints)
				.Select(cp => Convert.ToInt32(cp.Trim(), CultureInfo.InvariantCulture))
				.ToArray();
			string symbol = string.Empty;
			Array.ForEach(codePoints, cp => symbol += char.ConvertFromUtf32(cp));
			return new UnicodeSymbol(codePoints, symbol);
		}

		/// <summary>
		/// Creates an instance of <see cref="UnicodeSymbol"/> from a complex Unicode symbol.
		/// </summary>
		/// <param name="symbol">Complex unicode symbol.</param>
		/// <returns>Instace with the different representations of a complex Unicode symbol.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="symbol"/> is null.</exception>
		public static UnicodeSymbol FromSymbol(string symbol)
		{
			Guard.AgainstNullArgument("symbol", symbol);
			Guard.AgainstArgument("symbol", string.IsNullOrEmpty(symbol));

			int[] codePoints = new int[symbol.Length];
			for (int i = 0; i < symbol.Length; i++)
			{
				codePoints[i] = char.ConvertToUtf32(symbol, i);
			}
			
			return new UnicodeSymbol(codePoints, symbol);
		}
	}
}
