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
	public static Money Aud(this decimal amount)
	{
		return new Money(amount, Currency.Aud);
	}

	/// <summary>
	/// Creates an <see cref="Money"/> instance with the specified Currency.
	/// </summary>
	/// <param name="amount">The <see cref="Money.Amount"/> of the monetary quantity.</param>
	/// <returns>A <see cref="Money"/> with the specified <paramref name="amount"/> and <see cref="Currency.Cad"/>.</returns>
	public static Money Cad(this decimal amount)
	{
		return new Money(amount, Currency.Cad);
	}

	/// <summary>
	/// Creates an <see cref="Money"/> instance with the specified Currency.
	/// </summary>
	/// <param name="amount">The <see cref="Money.Amount"/> of the monetary quantity.</param>
	/// <returns>A <see cref="Money"/> with the specified <paramref name="amount"/> and <see cref="Currency.Chf"/>.</returns>
	public static Money Chf(this decimal amount)
	{
		return new Money(amount, Currency.Chf);
	}

	/// <summary>
	/// Creates an <see cref="Money"/> instance with the specified Currency.
	/// </summary>
	/// <param name="amount">The <see cref="Money.Amount"/> of the monetary quantity.</param>
	/// <returns>A <see cref="Money"/> with the specified <paramref name="amount"/> and <see cref="Currency.Cny"/>.</returns>
	public static Money Cny(this decimal amount)
	{
		return new Money(amount, Currency.Cny);
	}

	/// <summary>
	/// Creates an <see cref="Money"/> instance with the specified Currency.
	/// </summary>
	/// <param name="amount">The <see cref="Money.Amount"/> of the monetary quantity.</param>
	/// <returns>A <see cref="Money"/> with the specified <paramref name="amount"/> and <see cref="Currency.Dkk"/>.</returns>
	public static Money Dkk(this decimal amount)
	{
		return new Money(amount, Currency.Dkk);
	}

	/// <summary>
	/// Creates an <see cref="Money"/> instance with the specified Currency.
	/// </summary>
	/// <param name="amount">The <see cref="Money.Amount"/> of the monetary quantity.</param>
	/// <returns>A <see cref="Money"/> with the specified <paramref name="amount"/> and <see cref="Currency.Eur"/>.</returns>
	public static Money Eur(this decimal amount)
	{
		return new Money(amount, Currency.Eur);
	}

	/// <summary>
	/// Creates an <see cref="Money"/> instance with the specified Currency.
	/// </summary>
	/// <param name="amount">The <see cref="Money.Amount"/> of the monetary quantity.</param>
	/// <returns>A <see cref="Money"/> with the specified <paramref name="amount"/> and <see cref="Currency.Gbp"/>.</returns>
	public static Money Gbp(this decimal amount)
	{
		return new Money(amount, Currency.Gbp);
	}

	/// <summary>
	/// Creates an <see cref="Money"/> instance with the specified Currency.
	/// </summary>
	/// <param name="amount">The <see cref="Money.Amount"/> of the monetary quantity.</param>
	/// <returns>A <see cref="Money"/> with the specified <paramref name="amount"/> and <see cref="Currency.Hkd"/>.</returns>
	public static Money Hkd(this decimal amount)
	{
		return new Money(amount, Currency.Hkd);
	}

	/// <summary>
	/// Creates an <see cref="Money"/> instance with the specified Currency.
	/// </summary>
	/// <param name="amount">The <see cref="Money.Amount"/> of the monetary quantity.</param>
	/// <returns>A <see cref="Money"/> with the specified <paramref name="amount"/> and <see cref="Currency.Huf"/>.</returns>
	public static Money Huf(this decimal amount)
	{
		return new Money(amount, Currency.Huf);
	}

	/// <summary>
	/// Creates an <see cref="Money"/> instance with the specified Currency.
	/// </summary>
	/// <param name="amount">The <see cref="Money.Amount"/> of the monetary quantity.</param>
	/// <returns>A <see cref="Money"/> with the specified <paramref name="amount"/> and <see cref="Currency.Inr"/>.</returns>
	public static Money Inr(this decimal amount)
	{
		return new Money(amount, Currency.Inr);
	}

	/// <summary>
	/// Creates an <see cref="Money"/> instance with the specified Currency.
	/// </summary>
	/// <param name="amount">The <see cref="Money.Amount"/> of the monetary quantity.</param>
	/// <returns>A <see cref="Money"/> with the specified <paramref name="amount"/> and <see cref="Currency.Jpy"/>.</returns>
	public static Money Jpy(this decimal amount)
	{
		return new Money(amount, Currency.Jpy);
	}

	/// <summary>
	/// Creates an <see cref="Money"/> instance with the specified Currency.
	/// </summary>
	/// <param name="amount">The <see cref="Money.Amount"/> of the monetary quantity.</param>
	/// <returns>A <see cref="Money"/> with the specified <paramref name="amount"/> and <see cref="Currency.Mxn"/>.</returns>
	public static Money Mxn(this decimal amount)
	{
		return new Money(amount, Currency.Mxn);
	}

	/// <summary>
	/// Creates an <see cref="Money"/> instance with the specified Currency.
	/// </summary>
	/// <param name="amount">The <see cref="Money.Amount"/> of the monetary quantity.</param>
	/// <returns>A <see cref="Money"/> with the specified <paramref name="amount"/> and <see cref="Currency.Myr"/>.</returns>
	public static Money Myr(this decimal amount)
	{
		return new Money(amount, Currency.Myr);
	}

	/// <summary>
	/// Creates an <see cref="Money"/> instance with the specified Currency.
	/// </summary>
	/// <param name="amount">The <see cref="Money.Amount"/> of the monetary quantity.</param>
	/// <returns>A <see cref="Money"/> with the specified <paramref name="amount"/> and <see cref="Currency.Nok"/>.</returns>
	public static Money Nok(this decimal amount)
	{
		return new Money(amount, Currency.Nok);
	}

	/// <summary>
	/// Creates an <see cref="Money"/> instance with the specified Currency.
	/// </summary>
	/// <param name="amount">The <see cref="Money.Amount"/> of the monetary quantity.</param>
	/// <returns>A <see cref="Money"/> with the specified <paramref name="amount"/> and <see cref="Currency.Nzd"/>.</returns>
	public static Money Nzd(this decimal amount)
	{
		return new Money(amount, Currency.Nzd);
	}

	/// <summary>
	/// Creates an <see cref="Money"/> instance with the specified Currency.
	/// </summary>
	/// <param name="amount">The <see cref="Money.Amount"/> of the monetary quantity.</param>
	/// <returns>A <see cref="Money"/> with the specified <paramref name="amount"/> and <see cref="Currency.Rub"/>.</returns>
	public static Money Rub(this decimal amount)
	{
		return new Money(amount, Currency.Rub);
	}

	/// <summary>
	/// Creates an <see cref="Money"/> instance with the specified Currency.
	/// </summary>
	/// <param name="amount">The <see cref="Money.Amount"/> of the monetary quantity.</param>
	/// <returns>A <see cref="Money"/> with the specified <paramref name="amount"/> and <see cref="Currency.Sek"/>.</returns>
	public static Money Sek(this decimal amount)
	{
		return new Money(amount, Currency.Sek);
	}

	/// <summary>
	/// Creates an <see cref="Money"/> instance with the specified Currency.
	/// </summary>
	/// <param name="amount">The <see cref="Money.Amount"/> of the monetary quantity.</param>
	/// <returns>A <see cref="Money"/> with the specified <paramref name="amount"/> and <see cref="Currency.Sgd"/>.</returns>
	public static Money Sgd(this decimal amount)
	{
		return new Money(amount, Currency.Sgd);
	}

	/// <summary>
	/// Creates an <see cref="Money"/> instance with the specified Currency.
	/// </summary>
	/// <param name="amount">The <see cref="Money.Amount"/> of the monetary quantity.</param>
	/// <returns>A <see cref="Money"/> with the specified <paramref name="amount"/> and <see cref="Currency.Thb"/>.</returns>
	public static Money Thb(this decimal amount)
	{
		return new Money(amount, Currency.Thb);
	}

	/// <summary>
	/// Creates an <see cref="Money"/> instance with the specified Currency.
	/// </summary>
	/// <param name="amount">The <see cref="Money.Amount"/> of the monetary quantity.</param>
	/// <returns>A <see cref="Money"/> with the specified <paramref name="amount"/> and <see cref="Currency.Usd"/>.</returns>
	public static Money Usd(this decimal amount)
	{
		return new Money(amount, Currency.Usd);
	}

	/// <summary>
	/// Creates an <see cref="Money"/> instance with the specified Currency.
	/// </summary>
	/// <param name="amount">The <see cref="Money.Amount"/> of the monetary quantity.</param>
	/// <returns>A <see cref="Money"/> with the specified <paramref name="amount"/> and <see cref="Currency.Zar"/>.</returns>
	public static Money Zar(this decimal amount)
	{
		return new Money(amount, Currency.Zar);
	}

	/// <summary>
	/// Creates an <see cref="Money"/> instance with the specified Currency.
	/// </summary>
	/// <param name="amount">The <see cref="Money.Amount"/> of the monetary quantity.</param>
	/// <returns>A <see cref="Money"/> with the specified <paramref name="amount"/> and <see cref="Currency.Xxx"/>.</returns>
	public static Money Xxx(this decimal amount)
	{
		return new Money(amount, Currency.Xxx);
	}

	/// <summary>
	/// Creates an <see cref="Money"/> instance with the specified Currency.
	/// </summary>
	/// <param name="amount">The <see cref="Money.Amount"/> of the monetary quantity.</param>
	/// <returns>A <see cref="Money"/> with the specified <paramref name="amount"/> and <see cref="Currency.Xts"/>.</returns>
	public static Money Xts(this decimal amount)
	{
		return new Money(amount, Currency.Xts);
	}
}
