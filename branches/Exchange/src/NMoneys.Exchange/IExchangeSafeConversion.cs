namespace NMoneys.Exchange
{
	public interface IExchangeSafeConversion
	{
		Money? To(CurrencyIsoCode to);
		Money? To(Currency to);
	}
}