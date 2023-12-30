namespace NMoneys;

/// <summary>
/// Provides data for the <see cref="Currency.ObsoleteCurrency"/> event.
/// </summary>
public sealed class ObsoleteCurrencyEventArgs : EventArgs
{
	/// <summary>
	/// Initializes a new instance of the <see cref="ObsoleteCurrencyEventArgs"/> class.
	/// </summary>
	/// <param name="code"></param>
	public ObsoleteCurrencyEventArgs(CurrencyIsoCode code)
	{
		Code = code;
	}

	/// <summary>
	/// ISO code of the obsolete currency.
	/// </summary>
	public CurrencyIsoCode Code { get; init; }
}
