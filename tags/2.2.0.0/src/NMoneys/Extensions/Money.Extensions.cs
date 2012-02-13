using System.Collections.Generic;
using System.Linq;
using NMoneys.Support;

namespace NMoneys.Extensions
{
	/// <summary>
	/// Extensions methods related to monetary quantities.
	/// </summary>
	public static class MoneyExtensions
	{
		#region shortcuts

		/// <summary>
		/// Creates an <see cref="Money"/> instance with the specified NMoneys.Currency.
		/// </summary>
		/// <param name="amount">The <see cref="Money.Amount"/> of the monetary quantity.</param>
		/// <returns>A <see cref="Money"/> with the specified <paramref name="amount"/> and <see cref="NMoneys.Currency.Aud"/>.</returns>
		public static Money Aud(this decimal amount) { return new Money(amount, NMoneys.Currency.Aud); }
		/// <summary>
		/// Creates an <see cref="Money"/> instance with the specified NMoneys.Currency.
		/// </summary>
		/// <param name="amount">The <see cref="Money.Amount"/> of the monetary quantity.</param>
		/// <returns>A <see cref="Money"/> with the specified <paramref name="amount"/> and <see cref="NMoneys.Currency.Cad"/>.</returns>
		public static Money Cad(this decimal amount) { return new Money(amount, NMoneys.Currency.Cad); }
		/// <summary>
		/// Creates an <see cref="Money"/> instance with the specified NMoneys.Currency.
		/// </summary>
		/// <param name="amount">The <see cref="Money.Amount"/> of the monetary quantity.</param>
		/// <returns>A <see cref="Money"/> with the specified <paramref name="amount"/> and <see cref="NMoneys.Currency.Chf"/>.</returns>
		public static Money Chf(this decimal amount) { return new Money(amount, NMoneys.Currency.Chf); }
		/// <summary>
		/// Creates an <see cref="Money"/> instance with the specified NMoneys.Currency.
		/// </summary>
		/// <param name="amount">The <see cref="Money.Amount"/> of the monetary quantity.</param>
		/// <returns>A <see cref="Money"/> with the specified <paramref name="amount"/> and <see cref="NMoneys.Currency.Cny"/>.</returns>
		public static Money Cny(this decimal amount) { return new Money(amount, NMoneys.Currency.Cny); }
		/// <summary>
		/// Creates an <see cref="Money"/> instance with the specified NMoneys.Currency.
		/// </summary>
		/// <param name="amount">The <see cref="Money.Amount"/> of the monetary quantity.</param>
		/// <returns>A <see cref="Money"/> with the specified <paramref name="amount"/> and <see cref="NMoneys.Currency.Dkk"/>.</returns>
		public static Money Dkk(this decimal amount) { return new Money(amount, NMoneys.Currency.Dkk); }
		/// <summary>
		/// Creates an <see cref="Money"/> instance with the specified NMoneys.Currency.
		/// </summary>
		/// <param name="amount">The <see cref="Money.Amount"/> of the monetary quantity.</param>
		/// <returns>A <see cref="Money"/> with the specified <paramref name="amount"/> and <see cref="NMoneys.Currency.Eur"/>.</returns>
		public static Money Eur(this decimal amount) { return new Money(amount, NMoneys.Currency.Eur); }
		/// <summary>
		/// Creates an <see cref="Money"/> instance with the specified NMoneys.Currency.
		/// </summary>
		/// <param name="amount">The <see cref="Money.Amount"/> of the monetary quantity.</param>
		/// <returns>A <see cref="Money"/> with the specified <paramref name="amount"/> and <see cref="NMoneys.Currency.Gbp"/>.</returns>
		public static Money Gbp(this decimal amount) { return new Money(amount, NMoneys.Currency.Gbp); }
		/// <summary>
		/// Creates an <see cref="Money"/> instance with the specified NMoneys.Currency.
		/// </summary>
		/// <param name="amount">The <see cref="Money.Amount"/> of the monetary quantity.</param>
		/// <returns>A <see cref="Money"/> with the specified <paramref name="amount"/> and <see cref="NMoneys.Currency.Hkd"/>.</returns>
		public static Money Hkd(this decimal amount) { return new Money(amount, NMoneys.Currency.Hkd); }
		/// <summary>
		/// Creates an <see cref="Money"/> instance with the specified NMoneys.Currency.
		/// </summary>
		/// <param name="amount">The <see cref="Money.Amount"/> of the monetary quantity.</param>
		/// <returns>A <see cref="Money"/> with the specified <paramref name="amount"/> and <see cref="NMoneys.Currency.Huf"/>.</returns>
		public static Money Huf(this decimal amount) { return new Money(amount, NMoneys.Currency.Huf); }
		/// <summary>
		/// Creates an <see cref="Money"/> instance with the specified NMoneys.Currency.
		/// </summary>
		/// <param name="amount">The <see cref="Money.Amount"/> of the monetary quantity.</param>
		/// <returns>A <see cref="Money"/> with the specified <paramref name="amount"/> and <see cref="NMoneys.Currency.Inr"/>.</returns>
		public static Money Inr(this decimal amount) { return new Money(amount, NMoneys.Currency.Inr); }
		/// <summary>
		/// Creates an <see cref="Money"/> instance with the specified NMoneys.Currency.
		/// </summary>
		/// <param name="amount">The <see cref="Money.Amount"/> of the monetary quantity.</param>
		/// <returns>A <see cref="Money"/> with the specified <paramref name="amount"/> and <see cref="NMoneys.Currency.Jpy"/>.</returns>
		public static Money Jpy(this decimal amount) { return new Money(amount, NMoneys.Currency.Jpy); }
		/// <summary>
		/// Creates an <see cref="Money"/> instance with the specified NMoneys.Currency.
		/// </summary>
		/// <param name="amount">The <see cref="Money.Amount"/> of the monetary quantity.</param>
		/// <returns>A <see cref="Money"/> with the specified <paramref name="amount"/> and <see cref="NMoneys.Currency.Mxn"/>.</returns>
		public static Money Mxn(this decimal amount) { return new Money(amount, NMoneys.Currency.Mxn); }
		/// <summary>
		/// Creates an <see cref="Money"/> instance with the specified NMoneys.Currency.
		/// </summary>
		/// <param name="amount">The <see cref="Money.Amount"/> of the monetary quantity.</param>
		/// <returns>A <see cref="Money"/> with the specified <paramref name="amount"/> and <see cref="NMoneys.Currency.Myr"/>.</returns>
		public static Money Myr(this decimal amount) { return new Money(amount, NMoneys.Currency.Myr); }
		/// <summary>
		/// Creates an <see cref="Money"/> instance with the specified NMoneys.Currency.
		/// </summary>
		/// <param name="amount">The <see cref="Money.Amount"/> of the monetary quantity.</param>
		/// <returns>A <see cref="Money"/> with the specified <paramref name="amount"/> and <see cref="NMoneys.Currency.Nok"/>.</returns>
		public static Money Nok(this decimal amount) { return new Money(amount, NMoneys.Currency.Nok); }
		/// <summary>
		/// Creates an <see cref="Money"/> instance with the specified NMoneys.Currency.
		/// </summary>
		/// <param name="amount">The <see cref="Money.Amount"/> of the monetary quantity.</param>
		/// <returns>A <see cref="Money"/> with the specified <paramref name="amount"/> and <see cref="NMoneys.Currency.Nzd"/>.</returns>
		public static Money Nzd(this decimal amount) { return new Money(amount, NMoneys.Currency.Nzd); }
		/// <summary>
		/// Creates an <see cref="Money"/> instance with the specified NMoneys.Currency.
		/// </summary>
		/// <param name="amount">The <see cref="Money.Amount"/> of the monetary quantity.</param>
		/// <returns>A <see cref="Money"/> with the specified <paramref name="amount"/> and <see cref="NMoneys.Currency.Rub"/>.</returns>
		public static Money Rub(this decimal amount) { return new Money(amount, NMoneys.Currency.Rub); }
		/// <summary>
		/// Creates an <see cref="Money"/> instance with the specified NMoneys.Currency.
		/// </summary>
		/// <param name="amount">The <see cref="Money.Amount"/> of the monetary quantity.</param>
		/// <returns>A <see cref="Money"/> with the specified <paramref name="amount"/> and <see cref="NMoneys.Currency.Sek"/>.</returns>
		public static Money Sek(this decimal amount) { return new Money(amount, NMoneys.Currency.Sek); }
		/// <summary>
		/// Creates an <see cref="Money"/> instance with the specified NMoneys.Currency.
		/// </summary>
		/// <param name="amount">The <see cref="Money.Amount"/> of the monetary quantity.</param>
		/// <returns>A <see cref="Money"/> with the specified <paramref name="amount"/> and <see cref="NMoneys.Currency.Sgd"/>.</returns>
		public static Money Sgd(this decimal amount) { return new Money(amount, NMoneys.Currency.Sgd); }
		/// <summary>
		/// Creates an <see cref="Money"/> instance with the specified NMoneys.Currency.
		/// </summary>
		/// <param name="amount">The <see cref="Money.Amount"/> of the monetary quantity.</param>
		/// <returns>A <see cref="Money"/> with the specified <paramref name="amount"/> and <see cref="NMoneys.Currency.Thb"/>.</returns>
		public static Money Thb(this decimal amount) { return new Money(amount, NMoneys.Currency.Thb); }
		/// <summary>
		/// Creates an <see cref="Money"/> instance with the specified NMoneys.Currency.
		/// </summary>
		/// <param name="amount">The <see cref="Money.Amount"/> of the monetary quantity.</param>
		/// <returns>A <see cref="Money"/> with the specified <paramref name="amount"/> and <see cref="NMoneys.Currency.Usd"/>.</returns>
		public static Money Usd(this decimal amount) { return new Money(amount, NMoneys.Currency.Usd); }
		/// <summary>
		/// Creates an <see cref="Money"/> instance with the specified NMoneys.Currency.
		/// </summary>
		/// <param name="amount">The <see cref="Money.Amount"/> of the monetary quantity.</param>
		/// <returns>A <see cref="Money"/> with the specified <paramref name="amount"/> and <see cref="NMoneys.Currency.Zar"/>.</returns>
		public static Money Zar(this decimal amount) { return new Money(amount, NMoneys.Currency.Zar); }
		/// <summary>
		/// Creates an <see cref="Money"/> instance with the specified NMoneys.Currency.
		/// </summary>
		/// <param name="amount">The <see cref="Money.Amount"/> of the monetary quantity.</param>
		/// <returns>A <see cref="Money"/> with the specified <paramref name="amount"/> and <see cref="NMoneys.Currency.Xxx"/>.</returns>
		public static Money Xxx(this decimal amount) { return new Money(amount, NMoneys.Currency.Xxx); }
		/// <summary>
		/// Creates an <see cref="Money"/> instance with the specified NMoneys.Currency.
		/// </summary>
		/// <param name="amount">The <see cref="Money.Amount"/> of the monetary quantity.</param>
		/// <returns>A <see cref="Money"/> with the specified <paramref name="amount"/> and <see cref="NMoneys.Currency.Xts"/>.</returns>
		public static Money Xts(this decimal amount) { return new Money(amount, NMoneys.Currency.Xts); }

		#endregion

		#region aliases

		/// <summary>
		/// Creates an <see cref="Money"/> instance with the specified NMoneys.Currency.
		/// </summary>
		/// <param name="amount">The <see cref="Money.Amount"/> of the monetary quantity.</param>
		/// <returns>A <see cref="Money"/> with the specified <paramref name="amount"/> and <see cref="NMoneys.Currency.Euro"/>.</returns>
		public static Money Euros(this decimal amount) { return new Money(amount, NMoneys.Currency.Euro); }
		/// <summary>
		/// Creates an <see cref="Money"/> instance with the specified NMoneys.Currency.
		/// </summary>
		/// <param name="amount">The <see cref="Money.Amount"/> of the monetary quantity.</param>
		/// <returns>A <see cref="Money"/> with the specified <paramref name="amount"/> and <see cref="NMoneys.Currency.Dollar"/>.</returns>
		public static Money Dollars(this decimal amount) { return new Money(amount, NMoneys.Currency.Dollar); }
		/// <summary>
		/// Creates an <see cref="Money"/> instance with the specified NMoneys.Currency.
		/// </summary>
		/// <param name="amount">The <see cref="Money.Amount"/> of the monetary quantity.</param>
		/// <returns>A <see cref="Money"/> with the specified <paramref name="amount"/> and <see cref="NMoneys.Currency.Pound"/>.</returns>
		public static Money Pounds(this decimal amount) { return new Money(amount, NMoneys.Currency.Pound); }
		
		#endregion

		#region slang ;-)

		/// <summary>
		/// Creates an <see cref="Money"/> instance with the specified NMoneys.Currency.
		/// </summary>
		/// <param name="amount">The <see cref="Money.Amount"/> of the monetary quantity.</param>
		/// <returns>A <see cref="Money"/> with the specified <paramref name="amount"/> and <see cref="NMoneys.Currency.Eur"/>.</returns>
		public static Money Lerus(this decimal amount) { return Euros(amount); }
		/// <summary>
		/// Creates an <see cref="Money"/> instance with the specified NMoneys.Currency.
		/// </summary>
		/// <param name="amount">The <see cref="Money.Amount"/> of the monetary quantity.</param>
		/// <returns>A <see cref="Money"/> with the specified <paramref name="amount"/> and <see cref="NMoneys.Currency.Usd"/>.</returns>
		public static Money Bucks(this decimal amount) { return Dollars(amount); }
		/// <summary>
		/// Creates an <see cref="Money"/> instance with the specified NMoneys.Currency.
		/// </summary>
		/// <param name="amount">The <see cref="Money.Amount"/> of the monetary quantity.</param>
		/// <returns>A <see cref="Money"/> with the specified <paramref name="amount"/> and <see cref="NMoneys.Currency.Gbp"/>.</returns>
		public static Money Quid(this decimal amount) { return Pounds(amount); }

		#endregion

		#region ToMoney

		///<summary>Creates a <see cref="Money"/> instance with the specified amount and NMoneys.Currency.</summary>
		/// <param name="amount">The <see cref="Money.Amount"/> of the monetary quantity.</param>
		/// <param name="currency">The <see cref="Money.CurrencyCode"/> of the monetary quantity.</param>
		/// <returns>A <see cref="Money"/> with the specified <paramref name="amount"/> and <paramref name="currency"/>.</returns>
		/// <seealso cref="Money(decimal, CurrencyIsoCode)"/>
		public static Money ToMoney(this CurrencyIsoCode currency, decimal amount)
		{
			return new Money(amount, currency);
		}

		///<summary>Creates a <see cref="Money"/> instance with the specified amount and unspecified NMoneys.Currency.</summary>
		/// <param name="amount">The <see cref="Money.Amount"/> of the monetary quantity.</param>
		/// <returns>A <see cref="Money"/> with the specified <paramref name="amount"/> and unspecified currency (<see cref="CurrencyIsoCode.XXX"/>).</returns>
		/// <seealso cref="Money(decimal)"/>
		public static Money ToMoney(this decimal amount)
		{
			return new Money(amount);
		}

		///<summary>Creates a <see cref="Money"/> instance with the specified amount and NMoneys.Currency.</summary>
		/// <param name="amount">The <see cref="Money.Amount"/> of the monetary quantity.</param>
		/// <param name="currency">The <see cref="Money.CurrencyCode"/> of the monetary quantity.</param>
		/// <returns>A <see cref="Money"/> with the specified <paramref name="amount"/> and <paramref name="currency"/>.</returns>
		/// <seealso cref="Money(decimal, CurrencyIsoCode)"/>
		public static Money ToMoney(this decimal amount, CurrencyIsoCode currency)
		{
			return new Money(amount, currency);
		}

		///<summary>Creates a <see cref="Money"/> instance with the specified amount and NMoneys.Currency.</summary>
		/// <param name="amount">The <see cref="Money.Amount"/> of the monetary quantity.</param>
		/// <param name="currency">The incarnation of the <see cref="Money.CurrencyCode"/>.</param>
		/// <returns>A <see cref="Money"/> with the specified <paramref name="amount"/> and <paramref name="currency"/>.</returns>
		/// <seealso cref="Money(decimal, NMoneys.Currency)"/>
		public static Money ToMoney(this decimal amount, Currency currency)
		{
			return new Money(amount, currency);
		}

		///<summary>Creates an array of <see cref="Money"/> instances all with the specified currency and the corresponding amount.</summary>
		/// <param name="amounts">Each of the <see cref="Money.Amount"/> of the monetary quantity.</param>
		/// <param name="currency">The <see cref="Money.CurrencyCode"/> of the monetary quantity.</param>
		/// <returns>An array of <see cref="Money"/> with the same length as <paramref name="amounts"/> and each member with the specified
		/// amount and <paramref name="currency"/>.</returns>
		/// <seealso cref="ToMoney(decimal, CurrencyIsoCode)"/>
		public static Money[] ToMoney(this IEnumerable<decimal> amounts, CurrencyIsoCode currency)
		{
			return amounts.Select(x => x.ToMoney(currency)).ToArray();
		}

		///<summary>Creates an array of <see cref="Money"/> instances all with the specified currency and the corresponding amount.</summary>
		/// <param name="amounts">Each of the <see cref="Money.Amount"/> of the monetary quantity.</param>
		/// <param name="currency">The incarnation of the <see cref="Money.CurrencyCode"/>.</param>
		/// <returns>An array of <see cref="Money"/> with the same length as <paramref name="amounts"/> and each member with the specified
		/// amount and <paramref name="currency"/>.</returns>
		/// <seealso cref="ToMoney(decimal, NMoneys.Currency)"/>
		public static Money[] ToMoney(this IEnumerable<decimal> amounts, Currency currency)
		{
			return ToMoney(amounts, Guard.AgainstNullArgument("currency", currency, c => c.IsoCode));
		}

		#endregion

		/// <summary>
		/// The currency instance of <paramref name="money"/>.
		/// </summary>
		/// <returns>The instance of <see cref="Currency"/> represented by <see cref="Money.CurrencyCode"/>.</returns>
		public static Currency Currency(this Money money)
		{
			return NMoneys.Currency.Get(money.CurrencyCode);
		}
	}
}
