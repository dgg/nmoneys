namespace NMoneys.Exchange
{
	/// <summary>
	/// Allows conversions of monetary quantities into other monetary quantities with different currencies.
	/// </summary>
	public interface IExchangeSafeConversion
	{
		/// <summary>
		/// Converts a monetary quantity into another monetary with the currency specified by <paramref name="to"/>.
		/// </summary>
		/// <remarks>Implementations of the interface should throw when conversion cannot be performed.</remarks>
		/// <param name="to">Target currency of the conversion.</param>
		/// <returns>A <see cref="Money"/> instance with the <see cref="Money.CurrencyCode"/> specified by <paramref name="to"/> 
		/// and the <see cref="Money.Amount"/> corresponding to the conversion; or null if the conversion could not be performed.</returns>
		Money? To(CurrencyIsoCode to);

		/// <summary>
		/// Converts a monetary quantity into another monetary with the currency specified by <paramref name="to"/>.
		/// </summary>
		/// <remarks>Implementations of the interface should throw when conversion cannot be performed.</remarks>
		/// <param name="to">Target currency of the conversion.</param>
		/// <returns>A <see cref="Money"/> instance with the <see cref="Money.CurrencyCode"/> specified by <paramref name="to"/> 
		/// and the <see cref="Money.Amount"/> corresponding to the conversion; or null of the conversion could not be performed.</returns>
		Money? To(Currency to);

		/// <summary>
		/// Allows access to the instance the extensions method was invoked on.
		/// </summary>
		Money From { get; }
	}
}