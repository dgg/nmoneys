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
	/// <returns>A <see cref="Money"/> with the specified <paramref name="amount"/> and <see cref="Currency.Aud"/>.</returns>
	public static Money Aud(this decimal amount) => new Money(amount, Currency.Aud);

	/// <summary>
	/// Creates an <see cref="Money"/> instance with the specified Currency.
	/// </summary>
	/// <param name="amount">The <see cref="Money.Amount"/> of the monetary quantity.</param>
	/// <returns>A <see cref="Money"/> with the specified <paramref name="amount"/> and <see cref="Currency.Aud"/>.</returns>
	public static Money Aud(this int amount) => new Money(amount, Currency.Aud);

	/// <summary>
	/// Creates an <see cref="Money"/> instance with the specified Currency.
	/// </summary>
	/// <param name="amount">The <see cref="Money.Amount"/> of the monetary quantity.</param>
	/// <returns>A <see cref="Money"/> with the specified <paramref name="amount"/> and <see cref="Currency.Cad"/>.</returns>
	public static Money Cad(this decimal amount) => new(amount, Currency.Cad);

	/// <summary>
	/// Creates an <see cref="Money"/> instance with the specified Currency.
	/// </summary>
	/// <param name="amount">The <see cref="Money.Amount"/> of the monetary quantity.</param>
	/// <returns>A <see cref="Money"/> with the specified <paramref name="amount"/> and <see cref="Currency.Cad"/>.</returns>
	public static Money Cad(this int amount) => new(amount, Currency.Cad);

	/// <summary>
	/// Creates an <see cref="Money"/> instance with the specified Currency.
	/// </summary>
	/// <param name="amount">The <see cref="Money.Amount"/> of the monetary quantity.</param>
	/// <returns>A <see cref="Money"/> with the specified <paramref name="amount"/> and <see cref="Currency.Chf"/>.</returns>
	public static Money Chf(this decimal amount) => new(amount, Currency.Chf);

	/// <summary>
	/// Creates an <see cref="Money"/> instance with the specified Currency.
	/// </summary>
	/// <param name="amount">The <see cref="Money.Amount"/> of the monetary quantity.</param>
	/// <returns>A <see cref="Money"/> with the specified <paramref name="amount"/> and <see cref="Currency.Chf"/>.</returns>
	public static Money Chf(this int amount) => new(amount, Currency.Chf);

	/// <summary>
	/// Creates an <see cref="Money"/> instance with the specified Currency.
	/// </summary>
	/// <param name="amount">The <see cref="Money.Amount"/> of the monetary quantity.</param>
	/// <returns>A <see cref="Money"/> with the specified <paramref name="amount"/> and <see cref="Currency.Cny"/>.</returns>
	public static Money Cny(this decimal amount) => new(amount, Currency.Cny);

	/// <summary>
	/// Creates an <see cref="Money"/> instance with the specified Currency.
	/// </summary>
	/// <param name="amount">The <see cref="Money.Amount"/> of the monetary quantity.</param>
	/// <returns>A <see cref="Money"/> with the specified <paramref name="amount"/> and <see cref="Currency.Cny"/>.</returns>
	public static Money Cny(this int amount) => new(amount, Currency.Cny);

	/// <summary>
	/// Creates an <see cref="Money"/> instance with the specified Currency.
	/// </summary>
	/// <param name="amount">The <see cref="Money.Amount"/> of the monetary quantity.</param>
	/// <returns>A <see cref="Money"/> with the specified <paramref name="amount"/> and <see cref="Currency.Dkk"/>.</returns>
	public static Money Dkk(this decimal amount) => new(amount, Currency.Dkk);

	/// <summary>
	/// Creates an <see cref="Money"/> instance with the specified Currency.
	/// </summary>
	/// <param name="amount">The <see cref="Money.Amount"/> of the monetary quantity.</param>
	/// <returns>A <see cref="Money"/> with the specified <paramref name="amount"/> and <see cref="Currency.Dkk"/>.</returns>
	public static Money Dkk(this int amount) => new(amount, Currency.Dkk);

	/// <summary>
	/// Creates an <see cref="Money"/> instance with the specified Currency.
	/// </summary>
	/// <param name="amount">The <see cref="Money.Amount"/> of the monetary quantity.</param>
	/// <returns>A <see cref="Money"/> with the specified <paramref name="amount"/> and <see cref="Currency.Eur"/>.</returns>
	public static Money Eur(this decimal amount) => new(amount, Currency.Eur);

	/// <summary>
	/// Creates an <see cref="Money"/> instance with the specified Currency.
	/// </summary>
	/// <param name="amount">The <see cref="Money.Amount"/> of the monetary quantity.</param>
	/// <returns>A <see cref="Money"/> with the specified <paramref name="amount"/> and <see cref="Currency.Eur"/>.</returns>
	public static Money Eur(this int amount) => new(amount, Currency.Eur);

	/// <summary>
	/// Creates an <see cref="Money"/> instance with the specified Currency.
	/// </summary>
	/// <param name="amount">The <see cref="Money.Amount"/> of the monetary quantity.</param>
	/// <returns>A <see cref="Money"/> with the specified <paramref name="amount"/> and <see cref="Currency.Gbp"/>.</returns>
	public static Money Gbp(this decimal amount) => new(amount, Currency.Gbp);

	/// <summary>
	/// Creates an <see cref="Money"/> instance with the specified Currency.
	/// </summary>
	/// <param name="amount">The <see cref="Money.Amount"/> of the monetary quantity.</param>
	/// <returns>A <see cref="Money"/> with the specified <paramref name="amount"/> and <see cref="Currency.Gbp"/>.</returns>
	public static Money Gbp(this int amount) => new(amount, Currency.Gbp);

	/// <summary>
	/// Creates an <see cref="Money"/> instance with the specified Currency.
	/// </summary>
	/// <param name="amount">The <see cref="Money.Amount"/> of the monetary quantity.</param>
	/// <returns>A <see cref="Money"/> with the specified <paramref name="amount"/> and <see cref="Currency.Hkd"/>.</returns>
	public static Money Hkd(this decimal amount) => new(amount, Currency.Hkd);

	/// <summary>
	/// Creates an <see cref="Money"/> instance with the specified Currency.
	/// </summary>
	/// <param name="amount">The <see cref="Money.Amount"/> of the monetary quantity.</param>
	/// <returns>A <see cref="Money"/> with the specified <paramref name="amount"/> and <see cref="Currency.Hkd"/>.</returns>
	public static Money Hkd(this int amount) => new(amount, Currency.Hkd);

	/// <summary>
	/// Creates an <see cref="Money"/> instance with the specified Currency.
	/// </summary>
	/// <param name="amount">The <see cref="Money.Amount"/> of the monetary quantity.</param>
	/// <returns>A <see cref="Money"/> with the specified <paramref name="amount"/> and <see cref="Currency.Huf"/>.</returns>
	public static Money Huf(this decimal amount) => new(amount, Currency.Huf);

	/// <summary>
	/// Creates an <see cref="Money"/> instance with the specified Currency.
	/// </summary>
	/// <param name="amount">The <see cref="Money.Amount"/> of the monetary quantity.</param>
	/// <returns>A <see cref="Money"/> with the specified <paramref name="amount"/> and <see cref="Currency.Huf"/>.</returns>
	public static Money Huf(this int amount) => new(amount, Currency.Huf);

	/// <summary>
	/// Creates an <see cref="Money"/> instance with the specified Currency.
	/// </summary>
	/// <param name="amount">The <see cref="Money.Amount"/> of the monetary quantity.</param>
	/// <returns>A <see cref="Money"/> with the specified <paramref name="amount"/> and <see cref="Currency.Inr"/>.</returns>
	public static Money Inr(this decimal amount) => new(amount, Currency.Inr);

	/// <summary>
	/// Creates an <see cref="Money"/> instance with the specified Currency.
	/// </summary>
	/// <param name="amount">The <see cref="Money.Amount"/> of the monetary quantity.</param>
	/// <returns>A <see cref="Money"/> with the specified <paramref name="amount"/> and <see cref="Currency.Inr"/>.</returns>
	public static Money Inr(this int amount) => new(amount, Currency.Inr);

	/// <summary>
	/// Creates an <see cref="Money"/> instance with the specified Currency.
	/// </summary>
	/// <param name="amount">The <see cref="Money.Amount"/> of the monetary quantity.</param>
	/// <returns>A <see cref="Money"/> with the specified <paramref name="amount"/> and <see cref="Currency.Jpy"/>.</returns>
	public static Money Jpy(this decimal amount) => new(amount, Currency.Jpy);

	/// <summary>
	/// Creates an <see cref="Money"/> instance with the specified Currency.
	/// </summary>
	/// <param name="amount">The <see cref="Money.Amount"/> of the monetary quantity.</param>
	/// <returns>A <see cref="Money"/> with the specified <paramref name="amount"/> and <see cref="Currency.Jpy"/>.</returns>
	public static Money Jpy(this int amount) => new(amount, Currency.Jpy);

	/// <summary>
	/// Creates an <see cref="Money"/> instance with the specified Currency.
	/// </summary>
	/// <param name="amount">The <see cref="Money.Amount"/> of the monetary quantity.</param>
	/// <returns>A <see cref="Money"/> with the specified <paramref name="amount"/> and <see cref="Currency.Mxn"/>.</returns>
	public static Money Mxn(this decimal amount) => new(amount, Currency.Mxn);

	/// <summary>
	/// Creates an <see cref="Money"/> instance with the specified Currency.
	/// </summary>
	/// <param name="amount">The <see cref="Money.Amount"/> of the monetary quantity.</param>
	/// <returns>A <see cref="Money"/> with the specified <paramref name="amount"/> and <see cref="Currency.Mxn"/>.</returns>
	public static Money Mxn(this int amount) => new(amount, Currency.Mxn);

	/// <summary>
	/// Creates an <see cref="Money"/> instance with the specified Currency.
	/// </summary>
	/// <param name="amount">The <see cref="Money.Amount"/> of the monetary quantity.</param>
	/// <returns>A <see cref="Money"/> with the specified <paramref name="amount"/> and <see cref="Currency.Myr"/>.</returns>
	public static Money Myr(this decimal amount) => new(amount, Currency.Myr);

	/// <summary>
	/// Creates an <see cref="Money"/> instance with the specified Currency.
	/// </summary>
	/// <param name="amount">The <see cref="Money.Amount"/> of the monetary quantity.</param>
	/// <returns>A <see cref="Money"/> with the specified <paramref name="amount"/> and <see cref="Currency.Myr"/>.</returns>
	public static Money Myr(this int amount) => new(amount, Currency.Myr);

	/// <summary>
	/// Creates an <see cref="Money"/> instance with the specified Currency.
	/// </summary>
	/// <param name="amount">The <see cref="Money.Amount"/> of the monetary quantity.</param>
	/// <returns>A <see cref="Money"/> with the specified <paramref name="amount"/> and <see cref="Currency.Nok"/>.</returns>
	public static Money Nok(this decimal amount) => new(amount, Currency.Nok);

	/// <summary>
	/// Creates an <see cref="Money"/> instance with the specified Currency.
	/// </summary>
	/// <param name="amount">The <see cref="Money.Amount"/> of the monetary quantity.</param>
	/// <returns>A <see cref="Money"/> with the specified <paramref name="amount"/> and <see cref="Currency.Nok"/>.</returns>
	public static Money Nok(this int amount) => new(amount, Currency.Nok);

	/// <summary>
	/// Creates an <see cref="Money"/> instance with the specified Currency.
	/// </summary>
	/// <param name="amount">The <see cref="Money.Amount"/> of the monetary quantity.</param>
	/// <returns>A <see cref="Money"/> with the specified <paramref name="amount"/> and <see cref="Currency.Nzd"/>.</returns>
	public static Money Nzd(this decimal amount) => new(amount, Currency.Nzd);

	/// <summary>
	/// Creates an <see cref="Money"/> instance with the specified Currency.
	/// </summary>
	/// <param name="amount">The <see cref="Money.Amount"/> of the monetary quantity.</param>
	/// <returns>A <see cref="Money"/> with the specified <paramref name="amount"/> and <see cref="Currency.Nzd"/>.</returns>
	public static Money Nzd(this int amount) => new(amount, Currency.Nzd);

	/// <summary>
	/// Creates an <see cref="Money"/> instance with the specified Currency.
	/// </summary>
	/// <param name="amount">The <see cref="Money.Amount"/> of the monetary quantity.</param>
	/// <returns>A <see cref="Money"/> with the specified <paramref name="amount"/> and <see cref="Currency.Rub"/>.</returns>
	public static Money Rub(this decimal amount) => new(amount, Currency.Rub);

	/// <summary>
	/// Creates an <see cref="Money"/> instance with the specified Currency.
	/// </summary>
	/// <param name="amount">The <see cref="Money.Amount"/> of the monetary quantity.</param>
	/// <returns>A <see cref="Money"/> with the specified <paramref name="amount"/> and <see cref="Currency.Rub"/>.</returns>
	public static Money Rub(this int amount) => new(amount, Currency.Rub);

	/// <summary>
	/// Creates an <see cref="Money"/> instance with the specified Currency.
	/// </summary>
	/// <param name="amount">The <see cref="Money.Amount"/> of the monetary quantity.</param>
	/// <returns>A <see cref="Money"/> with the specified <paramref name="amount"/> and <see cref="Currency.Sek"/>.</returns>
	public static Money Sek(this decimal amount) => new(amount, Currency.Sek);

	/// <summary>
	/// Creates an <see cref="Money"/> instance with the specified Currency.
	/// </summary>
	/// <param name="amount">The <see cref="Money.Amount"/> of the monetary quantity.</param>
	/// <returns>A <see cref="Money"/> with the specified <paramref name="amount"/> and <see cref="Currency.Sek"/>.</returns>
	public static Money Sek(this int amount) => new(amount, Currency.Sek);

	/// <summary>
	/// Creates an <see cref="Money"/> instance with the specified Currency.
	/// </summary>
	/// <param name="amount">The <see cref="Money.Amount"/> of the monetary quantity.</param>
	/// <returns>A <see cref="Money"/> with the specified <paramref name="amount"/> and <see cref="Currency.Sgd"/>.</returns>
	public static Money Sgd(this decimal amount) => new(amount, Currency.Sgd);

	/// <summary>
	/// Creates an <see cref="Money"/> instance with the specified Currency.
	/// </summary>
	/// <param name="amount">The <see cref="Money.Amount"/> of the monetary quantity.</param>
	/// <returns>A <see cref="Money"/> with the specified <paramref name="amount"/> and <see cref="Currency.Sgd"/>.</returns>
	public static Money Sgd(this int amount) => new(amount, Currency.Sgd);

	/// <summary>
	/// Creates an <see cref="Money"/> instance with the specified Currency.
	/// </summary>
	/// <param name="amount">The <see cref="Money.Amount"/> of the monetary quantity.</param>
	/// <returns>A <see cref="Money"/> with the specified <paramref name="amount"/> and <see cref="Currency.Thb"/>.</returns>
	public static Money Thb(this decimal amount) => new(amount, Currency.Thb);

	/// <summary>
	/// Creates an <see cref="Money"/> instance with the specified Currency.
	/// </summary>
	/// <param name="amount">The <see cref="Money.Amount"/> of the monetary quantity.</param>
	/// <returns>A <see cref="Money"/> with the specified <paramref name="amount"/> and <see cref="Currency.Thb"/>.</returns>
	public static Money Thb(this int amount) => new(amount, Currency.Thb);

	/// <summary>
	/// Creates an <see cref="Money"/> instance with the specified Currency.
	/// </summary>
	/// <param name="amount">The <see cref="Money.Amount"/> of the monetary quantity.</param>
	/// <returns>A <see cref="Money"/> with the specified <paramref name="amount"/> and <see cref="Currency.Usd"/>.</returns>
	public static Money Usd(this decimal amount) => new(amount, Currency.Usd);

	/// <summary>
	/// Creates an <see cref="Money"/> instance with the specified Currency.
	/// </summary>
	/// <param name="amount">The <see cref="Money.Amount"/> of the monetary quantity.</param>
	/// <returns>A <see cref="Money"/> with the specified <paramref name="amount"/> and <see cref="Currency.Usd"/>.</returns>
	public static Money Usd(this int amount) => new(amount, Currency.Usd);

	/// <summary>
	/// Creates an <see cref="Money"/> instance with the specified Currency.
	/// </summary>
	/// <param name="amount">The <see cref="Money.Amount"/> of the monetary quantity.</param>
	/// <returns>A <see cref="Money"/> with the specified <paramref name="amount"/> and <see cref="Currency.Zar"/>.</returns>
	public static Money Zar(this decimal amount) => new(amount, Currency.Zar);

	/// <summary>
	/// Creates an <see cref="Money"/> instance with the specified Currency.
	/// </summary>
	/// <param name="amount">The <see cref="Money.Amount"/> of the monetary quantity.</param>
	/// <returns>A <see cref="Money"/> with the specified <paramref name="amount"/> and <see cref="Currency.Zar"/>.</returns>
	public static Money Zar(this int amount) => new(amount, Currency.Zar);

	/// <summary>
	/// Creates an <see cref="Money"/> instance with the specified Currency.
	/// </summary>
	/// <param name="amount">The <see cref="Money.Amount"/> of the monetary quantity.</param>
	/// <returns>A <see cref="Money"/> with the specified <paramref name="amount"/> and <see cref="Currency.Xxx"/>.</returns>
	public static Money Xxx(this decimal amount) => new(amount, Currency.Xxx);

	/// <summary>
	/// Creates an <see cref="Money"/> instance with the specified Currency.
	/// </summary>
	/// <param name="amount">The <see cref="Money.Amount"/> of the monetary quantity.</param>
	/// <returns>A <see cref="Money"/> with the specified <paramref name="amount"/> and <see cref="Currency.Xxx"/>.</returns>
	public static Money Xxx(this int amount) => new(amount, Currency.Xxx);

	/// <summary>
	/// Creates an <see cref="Money"/> instance with the specified Currency.
	/// </summary>
	/// <param name="amount">The <see cref="Money.Amount"/> of the monetary quantity.</param>
	/// <returns>A <see cref="Money"/> with the specified <paramref name="amount"/> and <see cref="Currency.Xts"/>.</returns>
	public static Money Xts(this decimal amount) => new(amount, Currency.Xts);

	/// <summary>
	/// Creates an <see cref="Money"/> instance with the specified Currency.
	/// </summary>
	/// <param name="amount">The <see cref="Money.Amount"/> of the monetary quantity.</param>
	/// <returns>A <see cref="Money"/> with the specified <paramref name="amount"/> and <see cref="Currency.Xts"/>.</returns>
	public static Money Xts(this int amount) => new(amount, Currency.Xts);
}
