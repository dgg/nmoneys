namespace NMoneys.Extensions
{
	/// <summary>
	/// Extensions methods related to monetary quantities.
	/// </summary>
	public static class MoneyExtensions
	{
		#region shortcuts

		/// <summary>
		/// Creates an <see cref="Money"/> instance with the specified currency.
		/// </summary>
		/// <param name="amount">The <see cref="Money.Amount"/> of the monetary quantity.</param>
		/// <returns>A <see cref="Money"/> with the specified <paramref name="amount"/> and <see cref="Currency.Aud"/>.</returns>
		public static Money Aud(this decimal amount) { return new Money(amount, Currency.Aud); }
		/// <summary>
		/// Creates an <see cref="Money"/> instance with the specified currency.
		/// </summary>
		/// <param name="amount">The <see cref="Money.Amount"/> of the monetary quantity.</param>
		/// <returns>A <see cref="Money"/> with the specified <paramref name="amount"/> and <see cref="Currency.Cad"/>.</returns>
		public static Money Cad(this decimal amount) { return new Money(amount, Currency.Cad); }
		/// <summary>
		/// Creates an <see cref="Money"/> instance with the specified currency.
		/// </summary>
		/// <param name="amount">The <see cref="Money.Amount"/> of the monetary quantity.</param>
		/// <returns>A <see cref="Money"/> with the specified <paramref name="amount"/> and <see cref="Currency.Chf"/>.</returns>
		public static Money Chf(this decimal amount) { return new Money(amount, Currency.Chf); }
		/// <summary>
		/// Creates an <see cref="Money"/> instance with the specified currency.
		/// </summary>
		/// <param name="amount">The <see cref="Money.Amount"/> of the monetary quantity.</param>
		/// <returns>A <see cref="Money"/> with the specified <paramref name="amount"/> and <see cref="Currency.Cny"/>.</returns>
		public static Money Cny(this decimal amount) { return new Money(amount, Currency.Cny); }
		/// <summary>
		/// Creates an <see cref="Money"/> instance with the specified currency.
		/// </summary>
		/// <param name="amount">The <see cref="Money.Amount"/> of the monetary quantity.</param>
		/// <returns>A <see cref="Money"/> with the specified <paramref name="amount"/> and <see cref="Currency.Dkk"/>.</returns>
		public static Money Dkk(this decimal amount) { return new Money(amount, Currency.Dkk); }
		/// <summary>
		/// Creates an <see cref="Money"/> instance with the specified currency.
		/// </summary>
		/// <param name="amount">The <see cref="Money.Amount"/> of the monetary quantity.</param>
		/// <returns>A <see cref="Money"/> with the specified <paramref name="amount"/> and <see cref="Currency.Eur"/>.</returns>
		public static Money Eur(this decimal amount) { return new Money(amount, Currency.Eur); }
		/// <summary>
		/// Creates an <see cref="Money"/> instance with the specified currency.
		/// </summary>
		/// <param name="amount">The <see cref="Money.Amount"/> of the monetary quantity.</param>
		/// <returns>A <see cref="Money"/> with the specified <paramref name="amount"/> and <see cref="Currency.Gbp"/>.</returns>
		public static Money Gbp(this decimal amount) { return new Money(amount, Currency.Gbp); }
		/// <summary>
		/// Creates an <see cref="Money"/> instance with the specified currency.
		/// </summary>
		/// <param name="amount">The <see cref="Money.Amount"/> of the monetary quantity.</param>
		/// <returns>A <see cref="Money"/> with the specified <paramref name="amount"/> and <see cref="Currency.Hkd"/>.</returns>
		public static Money Hkd(this decimal amount) { return new Money(amount, Currency.Hkd); }
		/// <summary>
		/// Creates an <see cref="Money"/> instance with the specified currency.
		/// </summary>
		/// <param name="amount">The <see cref="Money.Amount"/> of the monetary quantity.</param>
		/// <returns>A <see cref="Money"/> with the specified <paramref name="amount"/> and <see cref="Currency.Huf"/>.</returns>
		public static Money Huf(this decimal amount) { return new Money(amount, Currency.Huf); }
		/// <summary>
		/// Creates an <see cref="Money"/> instance with the specified currency.
		/// </summary>
		/// <param name="amount">The <see cref="Money.Amount"/> of the monetary quantity.</param>
		/// <returns>A <see cref="Money"/> with the specified <paramref name="amount"/> and <see cref="Currency.Inr"/>.</returns>
		public static Money Inr(this decimal amount) { return new Money(amount, Currency.Inr); }
		/// <summary>
		/// Creates an <see cref="Money"/> instance with the specified currency.
		/// </summary>
		/// <param name="amount">The <see cref="Money.Amount"/> of the monetary quantity.</param>
		/// <returns>A <see cref="Money"/> with the specified <paramref name="amount"/> and <see cref="Currency.Jpy"/>.</returns>
		public static Money Jpy(this decimal amount) { return new Money(amount, Currency.Jpy); }
		/// <summary>
		/// Creates an <see cref="Money"/> instance with the specified currency.
		/// </summary>
		/// <param name="amount">The <see cref="Money.Amount"/> of the monetary quantity.</param>
		/// <returns>A <see cref="Money"/> with the specified <paramref name="amount"/> and <see cref="Currency.Mxn"/>.</returns>
		public static Money Mxn(this decimal amount) { return new Money(amount, Currency.Mxn); }
		/// <summary>
		/// Creates an <see cref="Money"/> instance with the specified currency.
		/// </summary>
		/// <param name="amount">The <see cref="Money.Amount"/> of the monetary quantity.</param>
		/// <returns>A <see cref="Money"/> with the specified <paramref name="amount"/> and <see cref="Currency.Myr"/>.</returns>
		public static Money Myr(this decimal amount) { return new Money(amount, Currency.Myr); }
		/// <summary>
		/// Creates an <see cref="Money"/> instance with the specified currency.
		/// </summary>
		/// <param name="amount">The <see cref="Money.Amount"/> of the monetary quantity.</param>
		/// <returns>A <see cref="Money"/> with the specified <paramref name="amount"/> and <see cref="Currency.Nok"/>.</returns>
		public static Money Nok(this decimal amount) { return new Money(amount, Currency.Nok); }
		/// <summary>
		/// Creates an <see cref="Money"/> instance with the specified currency.
		/// </summary>
		/// <param name="amount">The <see cref="Money.Amount"/> of the monetary quantity.</param>
		/// <returns>A <see cref="Money"/> with the specified <paramref name="amount"/> and <see cref="Currency.Nzd"/>.</returns>
		public static Money Nzd(this decimal amount) { return new Money(amount, Currency.Nzd); }
		/// <summary>
		/// Creates an <see cref="Money"/> instance with the specified currency.
		/// </summary>
		/// <param name="amount">The <see cref="Money.Amount"/> of the monetary quantity.</param>
		/// <returns>A <see cref="Money"/> with the specified <paramref name="amount"/> and <see cref="Currency.Rub"/>.</returns>
		public static Money Rub(this decimal amount) { return new Money(amount, Currency.Rub); }
		/// <summary>
		/// Creates an <see cref="Money"/> instance with the specified currency.
		/// </summary>
		/// <param name="amount">The <see cref="Money.Amount"/> of the monetary quantity.</param>
		/// <returns>A <see cref="Money"/> with the specified <paramref name="amount"/> and <see cref="Currency.Sek"/>.</returns>
		public static Money Sek(this decimal amount) { return new Money(amount, Currency.Sek); }
		/// <summary>
		/// Creates an <see cref="Money"/> instance with the specified currency.
		/// </summary>
		/// <param name="amount">The <see cref="Money.Amount"/> of the monetary quantity.</param>
		/// <returns>A <see cref="Money"/> with the specified <paramref name="amount"/> and <see cref="Currency.Sgd"/>.</returns>
		public static Money Sgd(this decimal amount) { return new Money(amount, Currency.Sgd); }
		/// <summary>
		/// Creates an <see cref="Money"/> instance with the specified currency.
		/// </summary>
		/// <param name="amount">The <see cref="Money.Amount"/> of the monetary quantity.</param>
		/// <returns>A <see cref="Money"/> with the specified <paramref name="amount"/> and <see cref="Currency.Thb"/>.</returns>
		public static Money Thb(this decimal amount) { return new Money(amount, Currency.Thb); }
		/// <summary>
		/// Creates an <see cref="Money"/> instance with the specified currency.
		/// </summary>
		/// <param name="amount">The <see cref="Money.Amount"/> of the monetary quantity.</param>
		/// <returns>A <see cref="Money"/> with the specified <paramref name="amount"/> and <see cref="Currency.Usd"/>.</returns>
		public static Money Usd(this decimal amount) { return new Money(amount, Currency.Usd); }
		/// <summary>
		/// Creates an <see cref="Money"/> instance with the specified currency.
		/// </summary>
		/// <param name="amount">The <see cref="Money.Amount"/> of the monetary quantity.</param>
		/// <returns>A <see cref="Money"/> with the specified <paramref name="amount"/> and <see cref="Currency.Zar"/>.</returns>
		public static Money Zar(this decimal amount) { return new Money(amount, Currency.Zar); }
		/// <summary>
		/// Creates an <see cref="Money"/> instance with the specified currency.
		/// </summary>
		/// <param name="amount">The <see cref="Money.Amount"/> of the monetary quantity.</param>
		/// <returns>A <see cref="Money"/> with the specified <paramref name="amount"/> and <see cref="Currency.Xxx"/>.</returns>
		public static Money Xxx(this decimal amount) { return new Money(amount, Currency.Xxx); }
		/// <summary>
		/// Creates an <see cref="Money"/> instance with the specified currency.
		/// </summary>
		/// <param name="amount">The <see cref="Money.Amount"/> of the monetary quantity.</param>
		/// <returns>A <see cref="Money"/> with the specified <paramref name="amount"/> and <see cref="Currency.Xts"/>.</returns>
		public static Money Xts(this decimal amount) { return new Money(amount, Currency.Xts); }

		#endregion

		#region aliases

		/// <summary>
		/// Creates an <see cref="Money"/> instance with the specified currency.
		/// </summary>
		/// <param name="amount">The <see cref="Money.Amount"/> of the monetary quantity.</param>
		/// <returns>A <see cref="Money"/> with the specified <paramref name="amount"/> and <see cref="Currency.Euro"/>.</returns>
		public static Money Euros(this decimal amount) { return new Money(amount, Currency.Euro); }
		/// <summary>
		/// Creates an <see cref="Money"/> instance with the specified currency.
		/// </summary>
		/// <param name="amount">The <see cref="Money.Amount"/> of the monetary quantity.</param>
		/// <returns>A <see cref="Money"/> with the specified <paramref name="amount"/> and <see cref="Currency.Dollar"/>.</returns>
		public static Money Dollars(this decimal amount) { return new Money(amount, Currency.Dollar); }
		/// <summary>
		/// Creates an <see cref="Money"/> instance with the specified currency.
		/// </summary>
		/// <param name="amount">The <see cref="Money.Amount"/> of the monetary quantity.</param>
		/// <returns>A <see cref="Money"/> with the specified <paramref name="amount"/> and <see cref="Currency.Pound"/>.</returns>
		public static Money Pounds(this decimal amount) { return new Money(amount, Currency.Pound); }
		
		#endregion

		#region slang ;-)

		/// <summary>
		/// Creates an <see cref="Money"/> instance with the specified currency.
		/// </summary>
		/// <param name="amount">The <see cref="Money.Amount"/> of the monetary quantity.</param>
		/// <returns>A <see cref="Money"/> with the specified <paramref name="amount"/> and <see cref="Currency.Eur"/>.</returns>
		public static Money Lerus(this decimal amount) { return Euros(amount); }
		/// <summary>
		/// Creates an <see cref="Money"/> instance with the specified currency.
		/// </summary>
		/// <param name="amount">The <see cref="Money.Amount"/> of the monetary quantity.</param>
		/// <returns>A <see cref="Money"/> with the specified <paramref name="amount"/> and <see cref="Currency.Usd"/>.</returns>
		public static Money Bucks(this decimal amount) { return Dollars(amount); }
		/// <summary>
		/// Creates an <see cref="Money"/> instance with the specified currency.
		/// </summary>
		/// <param name="amount">The <see cref="Money.Amount"/> of the monetary quantity.</param>
		/// <returns>A <see cref="Money"/> with the specified <paramref name="amount"/> and <see cref="Currency.Gbp"/>.</returns>
		public static Money Quid(this decimal amount) { return Pounds(amount); }

		#endregion
		
		///<summary>Creates a <see cref="Money"/> instance with the specified amount and currency.</summary>
		/// <param name="amount">The <see cref="Money.Amount"/> of the monetary quantity.</param>
		/// <param name="currency">The <see cref="Money.CurrencyCode"/> of the monetary quantity.</param>
		/// <returns>A <see cref="Money"/> with the specified <paramref name="amount"/> and <paramref name="currency"/>.</returns>
		/// <seealso cref="Money(decimal, CurrencyIsoCode)"/>
		public static Money ToMoney(this CurrencyIsoCode currency, decimal amount)
		{
			return new Money(amount, currency);
		}

		///<summary>Creates a <see cref="Money"/> instance with the specified amount and unspecified currency.</summary>
		/// <param name="amount">The <see cref="Money.Amount"/> of the monetary quantity.</param>
		/// <returns>A <see cref="Money"/> with the specified <paramref name="amount"/> and unspecified currency (<see cref="CurrencyIsoCode.XXX"/>).</returns>
		/// <seealso cref="Money(decimal)"/>
		public static Money ToMoney(this decimal amount)
		{
			return new Money(amount);
		}

		///<summary>Creates a <see cref="Money"/> instance with the specified amount and currency.</summary>
		/// <param name="amount">The <see cref="Money.Amount"/> of the monetary quantity.</param>
		/// <param name="currency">The <see cref="Money.CurrencyCode"/> of the monetary quantity.</param>
		/// <returns>A <see cref="Money"/> with the specified <paramref name="amount"/> and <paramref name="currency"/>.</returns>
		/// <seealso cref="Money(decimal, CurrencyIsoCode)"/>
		public static Money ToMoney(this decimal amount, CurrencyIsoCode currency)
		{
			return new Money(amount, currency);
		}

		///<summary>Creates a <see cref="Money"/> instance with the specified amount and currency.</summary>
		/// <param name="amount">The <see cref="Money.Amount"/> of the monetary quantity.</param>
		/// <param name="currency">The incarnation of the <see cref="Money.CurrencyCode"/>.</param>
		/// <returns>A <see cref="Money"/> with the specified <paramref name="amount"/> and <paramref name="currency"/>.</returns>
		/// <seealso cref="Money(decimal, Currency)"/>
		public static Money ToMoney(this decimal amount, Currency currency)
		{
			return new Money(amount, currency);
		}
	}
}
