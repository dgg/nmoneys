using System.Collections.Generic;
using NMoneys.Support;

namespace NMoneys
{
	internal sealed class ObsoleteCurrencies
	{
		private readonly HashSet<CurrencyIsoCode> _set;
		private ObsoleteCurrencies()
		{
#pragma warning disable 612,618
			_set = new HashSet<CurrencyIsoCode>(FastEnumComparer<CurrencyIsoCode>.Instance)
			{
				CurrencyIsoCode.EEK
			};
#pragma warning restore 612,618
		}

		public bool IsObsolete(CurrencyIsoCode code)
		{
			return _set.Contains(code);
		}

		public bool IsObsolete(Currency currency)
		{
			return currency == null || IsObsolete(currency.IsoCode);
		}

		public static ObsoleteCurrencies Instance
		{
			get
			{
				return Nested.instance;
			}
		}

		// needed for lazy initialized singleton
		class Nested
		{
			// Explicit static constructor to tell C# compiler
			// not to mark type as beforefieldinit
			static Nested()
			{
			}

			internal static readonly ObsoleteCurrencies instance = new ObsoleteCurrencies();
		}
	}
}