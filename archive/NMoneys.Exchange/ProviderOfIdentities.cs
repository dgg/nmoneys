namespace NMoneys.Exchange
{
	/// <summary>
	/// Null object pattern implementor of <see cref="IExchangeRateProvider"/>
	/// </summary>
	internal class ProviderOfIdentities : IExchangeRateProvider
	{
		/// <returns>Returns an identity rate: a rate of one.</returns>
		public ExchangeRate Get(CurrencyIsoCode from, CurrencyIsoCode to)
		{
			return ExchangeRate.Identity(from, to);
		}

		/// <returns>Returns an identity rate: a rate of one.</returns>
		public bool TryGet(CurrencyIsoCode from, CurrencyIsoCode to, out ExchangeRate rate)
		{
			rate = Get(from, to);
			return true;
		}
	}
}
