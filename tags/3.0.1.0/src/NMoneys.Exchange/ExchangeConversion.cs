using NMoneys.Support;

namespace NMoneys.Exchange
{
	/// <summary>
	/// Implements the conversions of monetary quantities into other monetary quantities with different currencies.
	/// </summary>
	/// <remarks>Throws if the rate cannot be found.</remarks>
	internal class ExchangeConversion : IExchangeConversion
	{
		private readonly IExchangeRateProvider _provider;
		private readonly Money _from;

		public ExchangeConversion(IExchangeRateProvider provider, Money from)
		{
			_provider = provider;
			_from = from;
		}

		public Money To(CurrencyIsoCode to)
		{
			ExchangeRate rate = _provider.Get(_from.CurrencyCode, to);
			return rate.Apply(_from);
		}

		public Money To(Currency to)
		{
			Guard.AgainstNullArgument("to", to);
			ExchangeRate rate = _provider.Get(_from.CurrencyCode, to.IsoCode);
			return rate.Apply(_from);
		}

		public Money From { get { return _from; } }
	}
}