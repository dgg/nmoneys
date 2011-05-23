namespace NMoneys.Exchange
{
	public interface IExchangeConversion
	{
		Money To(CurrencyIsoCode to);
		Money To(Currency to);
	}
}