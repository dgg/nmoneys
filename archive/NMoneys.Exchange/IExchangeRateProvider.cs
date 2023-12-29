namespace NMoneys.Exchange
{
	/// <summary>
	/// Provides means to obtain applicable rates for exchange opeerations.
	/// </summary>
	public interface IExchangeRateProvider
	{
		/// <summary>
		/// Provides an applicable rate for exchange operations.
		/// </summary>
		/// <remarks>Implementations may throw in case an applicable rate cannot be provided.</remarks>
		/// <param name="from">Base currency, the currency from which the conversion is performed.</param>
		/// <param name="to">Quote currency, the currency which the conversion is performed to.</param>
		/// <returns>A rate at which one currency will be exchanged for another.</returns>
		ExchangeRate Get(CurrencyIsoCode from, CurrencyIsoCode to);

		/// <summary>
		/// Provides an applicable rate for exchange operations.
		/// </summary>
		/// <remarks>Implementations should not throw in case an applicable rate cannot be provided.</remarks>
		/// <param name="from">Base currency, the currency from which the conversion is performed.</param>
		/// <param name="to">Quote currency, the currency which the conversion is performed to.</param>
		/// <param name="rate">A rate at which one currency will be exchanged for another or null if one cannot be provided.</param>
		/// <returns>true if an applicable rate can be provided; otherwise, false.</returns>
		bool TryGet(CurrencyIsoCode from, CurrencyIsoCode to, out ExchangeRate rate);
	}
}
