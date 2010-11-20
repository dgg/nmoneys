using System.Globalization;

namespace NMoneys.Extensions
{
	public static class MoneyExtensions
	{

		#region shortcuts

		public static Money Aud(this decimal amount) { return new Money(amount, Currency.Aud); }
		public static Money Cad(this decimal amount) { return new Money(amount, Currency.Cad); }
		public static Money Chf(this decimal amount) { return new Money(amount, Currency.Chf); }
		public static Money Cny(this decimal amount) { return new Money(amount, Currency.Cny); }
		public static Money Dkk(this decimal amount) { return new Money(amount, Currency.Dkk); }
		public static Money Eur(this decimal amount) { return new Money(amount, Currency.Eur); }
		public static Money Gbp(this decimal amount) { return new Money(amount, Currency.Gbp); }
		public static Money Hkd(this decimal amount) { return new Money(amount, Currency.Hkd); }
		public static Money Huf(this decimal amount) { return new Money(amount, Currency.Huf); }
		public static Money Inr(this decimal amount) { return new Money(amount, Currency.Inr); }
		public static Money Jpy(this decimal amount) { return new Money(amount, Currency.Jpy); }
		public static Money Mxn(this decimal amount) { return new Money(amount, Currency.Mxn); }
		public static Money Myr(this decimal amount) { return new Money(amount, Currency.Myr); }
		public static Money Nok(this decimal amount) { return new Money(amount, Currency.Nok); }
		public static Money Nzd(this decimal amount) { return new Money(amount, Currency.Nzd); }
		public static Money Rub(this decimal amount) { return new Money(amount, Currency.Rub); }
		public static Money Sek(this decimal amount) { return new Money(amount, Currency.Sek); }
		public static Money Sgd(this decimal amount) { return new Money(amount, Currency.Sgd); }
		public static Money Thb(this decimal amount) { return new Money(amount, Currency.Thb); }
		public static Money Usd(this decimal amount) { return new Money(amount, Currency.Usd); }
		public static Money Zar(this decimal amount) { return new Money(amount, Currency.Zar); }
		public static Money Xxx(this decimal amount) { return new Money(amount, Currency.Xxx); }
		public static Money Xts(this decimal amount) { return new Money(amount, Currency.Xts); }

		#endregion

		#region aliases

		public static Money Euros(this decimal amount) { return new Money(amount, Currency.Euro); }
		public static Money Dollars(this decimal amount) { return new Money(amount, Currency.Dollar); }
		public static Money Pounds(this decimal amount) { return new Money(amount, Currency.Pound); }
		
		#endregion

		#region slang ;-)

		public static Money Lerus(this decimal amount) { return Euros(amount); }
		public static Money Bucks(this decimal amount) { return Dollars(amount); }
		public static Money Quid(this decimal amount) { return Pounds(amount); }

		#endregion
		
		public static Money ToMoney(this CurrencyIsoCode currency, decimal amount)
		{
			return new Money(amount, currency);
		}

		public static Money ToMoney(this decimal amount)
		{
			return new Money(amount);
		}

		public static Money ToMoney(this decimal amount, CurrencyIsoCode currency)
		{
			return new Money(amount, currency);
		}

		public static Money ToMoney(this decimal amount, Currency currency)
		{
			return new Money(amount, currency);
		}
	}
}
