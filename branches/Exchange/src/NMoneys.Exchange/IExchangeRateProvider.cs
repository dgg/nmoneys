namespace NMoneys.Exchange
{
	public interface IExchangeRateProvider
	{
		ExchangeRate Get(CurrencyIsoCode from, CurrencyIsoCode to);
		bool TryGet(CurrencyIsoCode from, CurrencyIsoCode to, out ExchangeRate rate);
	}
}