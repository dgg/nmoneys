namespace NMoneys.Extensions;

/// <summary>
/// Extensions methods related to monetary quantities.
/// </summary>
public static partial class MoneyExtensions
{
	/// <summary>
	/// Creates an <see cref="Money"/> instance with the specified Currency.
	/// </summary>
	/// <param name="amount">The <see cref="Money.Amount"/> of the monetary quantity.</param>
	/// <returns>A <see cref="Money"/> with the specified <paramref name="amount"/> and <see cref="Currency.Euro"/>.</returns>
	public static Money Euros(this decimal amount) => new(amount, Currency.Euro);

	/// <summary>
	/// Creates an <see cref="Money"/> instance with the specified Currency.
	/// </summary>
	/// <param name="amount">The <see cref="Money.Amount"/> of the monetary quantity.</param>
	/// <returns>A <see cref="Money"/> with the specified <paramref name="amount"/> and <see cref="Currency.Euro"/>.</returns>
	public static Money Euros(this int amount) => new(amount, Currency.Euro);

	/// <summary>
	/// Creates an <see cref="Money"/> instance with the specified Currency.
	/// </summary>
	/// <param name="amount">The <see cref="Money.Amount"/> of the monetary quantity.</param>
	/// <returns>A <see cref="Money"/> with the specified <paramref name="amount"/> and <see cref="Currency.Dollar"/>.</returns>
	public static Money Dollars(this decimal amount) => new(amount, Currency.Dollar);

	/// <summary>
	/// Creates an <see cref="Money"/> instance with the specified Currency.
	/// </summary>
	/// <param name="amount">The <see cref="Money.Amount"/> of the monetary quantity.</param>
	/// <returns>A <see cref="Money"/> with the specified <paramref name="amount"/> and <see cref="Currency.Dollar"/>.</returns>
	public static Money Dollars(this int amount) => new(amount, Currency.Dollar);

	/// <summary>
	/// Creates an <see cref="Money"/> instance with the specified Currency.
	/// </summary>
	/// <param name="amount">The <see cref="Money.Amount"/> of the monetary quantity.</param>
	/// <returns>A <see cref="Money"/> with the specified <paramref name="amount"/> and <see cref="Currency.Pound"/>.</returns>
	public static Money Pounds(this decimal amount) => new(amount, Currency.Pound);

	/// <summary>
	/// Creates an <see cref="Money"/> instance with the specified Currency.
	/// </summary>
	/// <param name="amount">The <see cref="Money.Amount"/> of the monetary quantity.</param>
	/// <returns>A <see cref="Money"/> with the specified <paramref name="amount"/> and <see cref="Currency.Pound"/>.</returns>
	public static Money Pounds(this int amount) => new(amount, Currency.Pound);
}
