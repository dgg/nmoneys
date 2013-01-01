using System.Collections.Generic;
using NMoneys.Support;

namespace NMoneys
{
	/// <summary>
	/// Maintains a list of obsolete currencies
	/// </summary>
	internal static class ObsoleteCurrencies
	{
		private static readonly HashSet<CurrencyIsoCode> _set;
		static ObsoleteCurrencies()
		{
#pragma warning disable 612,618
			_set = new HashSet<CurrencyIsoCode>(FastEnumComparer<CurrencyIsoCode>.Instance)
			{
				CurrencyIsoCode.EEK,
				CurrencyIsoCode.ZMK
			};
#pragma warning restore 612,618
		}

		public static bool IsObsolete(CurrencyIsoCode code)
		{
			return _set.Contains(code);
		}

		public static bool IsObsolete(Currency currency)
		{
			return currency == null || IsObsolete(currency.IsoCode);
		}
	}
}