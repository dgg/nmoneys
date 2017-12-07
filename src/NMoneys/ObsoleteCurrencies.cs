using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
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
			var obsoleteCodes = Enumeration.GetValues<CurrencyIsoCode>()
				.Where(Enumeration.HasAttribute<CurrencyIsoCode, ObsoleteAttribute>);

			_set = new HashSet<CurrencyIsoCode>(
				obsoleteCodes,
				Currency.Code.Comparer);
		}

		[Pure]
		public static bool IsObsolete(CurrencyIsoCode code)
		{
			return _set.Contains(code);
		}

		[Pure]
		public static bool IsObsolete(Currency currency)
		{
			return currency == null || IsObsolete(currency.IsoCode);
		}

		[Pure]
		public static ushort Count => Convert.ToUInt16(_set.Count);
	}
}