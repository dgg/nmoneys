using System.Collections.Generic;

namespace NMoneys.Exchange.Tests
{
	internal class UsdEurGbpAs20110519 : IExchangeRateProvider
	{
		private readonly IDictionary<CurrencyIsoCode, ExchangeRate> _usdBase, _eurBase, _gbpBase;
		public UsdEurGbpAs20110519()
		{
			_usdBase = new Dictionary<CurrencyIsoCode, ExchangeRate>(3)
			{
				{CurrencyIsoCode.USD, new ExchangeRate(CurrencyIsoCode.USD, CurrencyIsoCode.USD, 1m)},
				{CurrencyIsoCode.EUR, new ExchangeRate(CurrencyIsoCode.USD, CurrencyIsoCode.EUR, 0.70155m)},
				{CurrencyIsoCode.GBP, new ExchangeRate(CurrencyIsoCode.USD, CurrencyIsoCode.GBP, 0.61860m)}
			};

			_eurBase = new Dictionary<CurrencyIsoCode, ExchangeRate>(3)
			{
				{CurrencyIsoCode.EUR, new ExchangeRate(CurrencyIsoCode.EUR, CurrencyIsoCode.EUR, 1m)},
				{CurrencyIsoCode.USD, new ExchangeRate(CurrencyIsoCode.EUR, CurrencyIsoCode.USD, 1.42542m)},
				{CurrencyIsoCode.GBP, new ExchangeRate(CurrencyIsoCode.EUR, CurrencyIsoCode.GBP, 0.88176m)}
			};

			_gbpBase = new Dictionary<CurrencyIsoCode, ExchangeRate>(3)
			{
				{CurrencyIsoCode.GBP, new ExchangeRate(CurrencyIsoCode.GBP, CurrencyIsoCode.GBP, 1m)},
				{CurrencyIsoCode.EUR, new ExchangeRate(CurrencyIsoCode.GBP, CurrencyIsoCode.EUR, 1.13409m)},
				{CurrencyIsoCode.USD, new ExchangeRate(CurrencyIsoCode.GBP, CurrencyIsoCode.USD, 1.61656m)}
			};
		}

		public ExchangeRate Get(CurrencyIsoCode from, CurrencyIsoCode to)
		{
			switch (from)
			{
				case CurrencyIsoCode.EUR:
					return _eurBase[to];
				case CurrencyIsoCode.USD:
					return _usdBase[to];
				case CurrencyIsoCode.GBP:
					return _gbpBase[to];
				default:
					throw new KeyNotFoundException("Only conversions from/to EUR-GBP-USD allowed");
			}
		}

		public bool TryGet(CurrencyIsoCode from, CurrencyIsoCode to, out ExchangeRate rate)
		{
			switch (from)
			{
				case CurrencyIsoCode.EUR:
					return _eurBase.TryGetValue(to, out rate);
				case CurrencyIsoCode.USD:
					return _usdBase.TryGetValue(to, out rate);
				case CurrencyIsoCode.GBP:
					return _gbpBase.TryGetValue(to, out rate);
				default:
					rate = null;
					return false;
			}
		}
	}
}