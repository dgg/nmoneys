namespace NMoneys.Exchange
{
	/// <summary>
	/// Implements the conversions of monetary quantities into other monetary quantities with different currencies.
	/// </summary>
	/// <remarks>Does not throw if the rate cannot be found.</remarks>
	internal class ExchangeSafeConversion : IExchangeSafeConversion
	{
		private readonly IExchangeRateProvider _provider;
		private readonly Money _from;

		public ExchangeSafeConversion(IExchangeRateProvider provider, Money from)
		{
			_provider = provider;
			_from = from;
		}

		public Money? To(CurrencyIsoCode to)
		{
			Money? converted = null;
			ExchangeRate rate;
			if (_provider.TryGet(_from.CurrencyCode, to, out rate))
			{
				converted = rate.Apply(_from);
			}
			return converted;
		}

		public Money? To(Currency to)
		{
			Money? converted = null;
			ExchangeRate rate;
			if (to != null && _provider.TryGet(_from.CurrencyCode, to.IsoCode, out rate))
			{
				converted = rate.Apply(_from);
			}
			return converted;
		}

		public Money From { get { return _from; } }
	}
}