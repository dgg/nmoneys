using System;

namespace NMoneys.Exchange
{
	/// <summary>
	/// Container for conversion extension methods and <see cref="IExchangeRateProvider"/> factories.
	/// </summary>
	public static class ExchangeRateProvider
	{
		private static readonly Func<IExchangeRateProvider> _default = () => new ProviderOfIdentities();

		/// <summary>
		/// Default rate provider.
		/// </summary>
		/// <remarks>Return a provider that always return identity rates: rates with the rate of one.</remarks>
		public static Func<IExchangeRateProvider> Default { get { return _default; } }

		/// <summary>
		/// Gets or sets the provider to be used when performing exchange conversions.
		/// </summary>
		public static Func<IExchangeRateProvider> Provider = Default;

		/// <summary>
		/// Gives acccess to exchange operations of <see cref="Money"/> instances.
		/// </summary>
		/// <remarks>Operations accessed through this method can throw if an applicable rate cannot be provided.</remarks>
		/// <param name="from">Monetary quantity to be exchanged.</param>
		/// <returns>A <see cref="IExchangeConversion"/> that allows performing exchange operations.</returns>
		public static IExchangeConversion Convert(this Money from)
		{
			return new ExchangeConversion(Provider(), from);
		}

		/// <summary>
		/// Gives acccess to exchange operations of <see cref="Money"/> instances.
		/// </summary>
		/// <remarks>Operations accessed through this method should not throw if an applicable rate cannot be provided.</remarks>
		/// <param name="from">Monetary quantity to be exchanged.</param>
		/// <returns>A <see cref="IExchangeSafeConversion"/> that allows performing exchange operations.</returns>
		public static IExchangeSafeConversion TryConvert(this Money from)
		{
			return new ExchangeSafeConversion(Provider(), from);
		}
	}
}
