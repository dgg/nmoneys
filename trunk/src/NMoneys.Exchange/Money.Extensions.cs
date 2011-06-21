using System;

namespace NMoneys.Exchange
{
	public static class ExchangeRateProvider
	{
		private static readonly Func<IExchangeRateProvider> _default = () => new NoOpProvider();
		public static Func<IExchangeRateProvider> Default { get { return _default; } }

		public static Func<IExchangeRateProvider> Factory = Default;

		public static IExchangeConversion Convert(this Money from)
		{
			return new ExchangeConversion(Factory(), from);
		}
		
		public static IExchangeSafeConversion TryConvert(this Money from)
		{
			return new ExchangeSafeConversion(Factory(), from);
		}
	}
}
