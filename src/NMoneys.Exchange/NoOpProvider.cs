namespace NMoneys.Exchange
{
	internal class NoOpProvider : IExchangeRateProvider
	{
		public ExchangeRate Get(CurrencyIsoCode from, CurrencyIsoCode to)
		{
			return new ExchangeRate(from, to, 1m);
		}

		public bool TryGet(CurrencyIsoCode from, CurrencyIsoCode to, out ExchangeRate rate)
		{
			rate = Get(from, to);
			return true;
		}
	}
}