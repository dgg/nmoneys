using NMoneys.Support;

namespace NMoneys.Extensions;

/// <summary>
/// Extensions methods related to monetary quantities.
/// </summary>
public static partial class MoneyExtensions
{
	///<summary>Creates a <see cref="Money"/> instance with the specified amount and Currency.</summary>
	/// <param name="amount">The <see cref="Money.Amount"/> of the monetary quantity.</param>
	/// <param name="currency">The <see cref="Money.CurrencyCode"/> of the monetary quantity.</param>
	/// <returns>A <see cref="Money"/> with the specified <paramref name="amount"/> and <paramref name="currency"/>.</returns>
	/// <seealso cref="Money(decimal, CurrencyIsoCode)"/>
	public static Money ToMoney(this CurrencyIsoCode currency, decimal amount) => new(amount, currency);

	///<summary>Creates a <see cref="Money"/> instance with the specified amount and Currency.</summary>
	/// <param name="amount">The <see cref="Money.Amount"/> of the monetary quantity.</param>
	/// <param name="currency">The <see cref="Money.CurrencyCode"/> of the monetary quantity.</param>
	/// <returns>A <see cref="Money"/> with the specified <paramref name="amount"/> and <paramref name="currency"/>.</returns>
	/// <seealso cref="Money(decimal, CurrencyIsoCode)"/>
	public static Money ToMoney(this CurrencyIsoCode currency, int amount) => new(amount, currency);

	///<summary>Creates a <see cref="Money"/> instance with the specified amount and unspecified Currency.</summary>
	/// <param name="amount">The <see cref="Money.Amount"/> of the monetary quantity.</param>
	/// <returns>A <see cref="Money"/> with the specified <paramref name="amount"/> and unspecified currency (<see cref="CurrencyIsoCode.XXX"/>).</returns>
	/// <seealso cref="Money(decimal, CurrencyIsoCode)"/>
	public static Money ToMoney(this decimal amount) => new(amount);

	///<summary>Creates a <see cref="Money"/> instance with the specified amount and unspecified Currency.</summary>
	/// <param name="amount">The <see cref="Money.Amount"/> of the monetary quantity.</param>
	/// <returns>A <see cref="Money"/> with the specified <paramref name="amount"/> and unspecified currency (<see cref="CurrencyIsoCode.XXX"/>).</returns>
	/// <seealso cref="Money(decimal, CurrencyIsoCode)"/>
	public static Money ToMoney(this int amount) => new(amount);

	///<summary>Creates a <see cref="Money"/> instance with the specified amount and Currency.</summary>
	/// <param name="amount">The <see cref="Money.Amount"/> of the monetary quantity.</param>
	/// <param name="currency">The <see cref="Money.CurrencyCode"/> of the monetary quantity.</param>
	/// <returns>A <see cref="Money"/> with the specified <paramref name="amount"/> and <paramref name="currency"/>.</returns>
	/// <seealso cref="Money(decimal, CurrencyIsoCode)"/>
	public static Money ToMoney(this decimal amount, CurrencyIsoCode currency) => new(amount, currency);

	///<summary>Creates a <see cref="Money"/> instance with the specified amount and Currency.</summary>
	/// <param name="amount">The <see cref="Money.Amount"/> of the monetary quantity.</param>
	/// <param name="currency">The <see cref="Money.CurrencyCode"/> of the monetary quantity.</param>
	/// <returns>A <see cref="Money"/> with the specified <paramref name="amount"/> and <paramref name="currency"/>.</returns>
	/// <seealso cref="Money(decimal, CurrencyIsoCode)"/>
	public static Money ToMoney(this int amount, CurrencyIsoCode currency) => new(amount, currency);

	///<summary>Creates a <see cref="Money"/> instance with the specified amount and Currency.</summary>
	/// <param name="amount">The <see cref="Money.Amount"/> of the monetary quantity.</param>
	/// <param name="currency">The incarnation of the <see cref="Money.CurrencyCode"/>.</param>
	/// <returns>A <see cref="Money"/> with the specified <paramref name="amount"/> and <paramref name="currency"/>.</returns>
	/// <seealso cref="Money(decimal, NMoneys.Currency)"/>
	public static Money ToMoney(this decimal amount, Currency currency) => new(amount, currency);

	///<summary>Creates a <see cref="Money"/> instance with the specified amount and Currency.</summary>
	/// <param name="amount">The <see cref="Money.Amount"/> of the monetary quantity.</param>
	/// <param name="currency">The incarnation of the <see cref="Money.CurrencyCode"/>.</param>
	/// <returns>A <see cref="Money"/> with the specified <paramref name="amount"/> and <paramref name="currency"/>.</returns>
	/// <seealso cref="Money(decimal, NMoneys.Currency)"/>
	public static Money ToMoney(this int amount, Currency currency) => new(amount, currency);

	///<summary>Creates an array of <see cref="Money"/> instances all with the specified currency and the corresponding amount.</summary>
	/// <param name="amounts">Each of the <see cref="Money.Amount"/> of the monetary quantity.</param>
	/// <param name="currency">The <see cref="Money.CurrencyCode"/> of the monetary quantity.</param>
	/// <returns>An array of <see cref="Money"/> with the same length as <paramref name="amounts"/> and each member with the specified
	/// amount and <paramref name="currency"/>.</returns>
	/// <seealso cref="ToMoney(decimal, CurrencyIsoCode)"/>
	public static Money[] ToMoney(this IEnumerable<decimal> amounts, CurrencyIsoCode currency) =>
		amounts.Select(x => x.ToMoney(currency)).ToArray();

	///<summary>Creates an array of <see cref="Money"/> instances all with the specified currency and the corresponding amount.</summary>
	/// <param name="amounts">Each of the <see cref="Money.Amount"/> of the monetary quantity.</param>
	/// <param name="currency">The <see cref="Money.CurrencyCode"/> of the monetary quantity.</param>
	/// <returns>An array of <see cref="Money"/> with the same length as <paramref name="amounts"/> and each member with the specified
	/// amount and <paramref name="currency"/>.</returns>
	/// <seealso cref="ToMoney(decimal, CurrencyIsoCode)"/>
	public static Money[] ToMoney(this IEnumerable<int> amounts, CurrencyIsoCode currency) =>
		amounts.Select(x => x.ToMoney(currency)).ToArray();

	///<summary>Creates an array of <see cref="Money"/> instances all with the specified currency and the corresponding amount.</summary>
	/// <param name="amounts">Each of the <see cref="Money.Amount"/> of the monetary quantity.</param>
	/// <param name="currency">The incarnation of the <see cref="Money.CurrencyCode"/>.</param>
	/// <returns>An array of <see cref="Money"/> with the same length as <paramref name="amounts"/> and each member with the specified
	/// amount and <paramref name="currency"/>.</returns>
	/// <seealso cref="ToMoney(decimal, NMoneys.Currency)"/>
	public static Money[] ToMoney(this IEnumerable<decimal> amounts, Currency currency) =>
		ToMoney(amounts, Ensuring.NotNull(nameof(currency), currency, c => c.IsoCode));

	///<summary>Creates an array of <see cref="Money"/> instances all with the specified currency and the corresponding amount.</summary>
	/// <param name="amounts">Each of the <see cref="Money.Amount"/> of the monetary quantity.</param>
	/// <param name="currency">The incarnation of the <see cref="Money.CurrencyCode"/>.</param>
	/// <returns>An array of <see cref="Money"/> with the same length as <paramref name="amounts"/> and each member with the specified
	/// amount and <paramref name="currency"/>.</returns>
	/// <seealso cref="ToMoney(decimal, NMoneys.Currency)"/>
	public static Money[] ToMoney(this IEnumerable<int> amounts, Currency currency) =>
		ToMoney(amounts, Ensuring.NotNull(nameof(currency), currency, c => c.IsoCode));
}
