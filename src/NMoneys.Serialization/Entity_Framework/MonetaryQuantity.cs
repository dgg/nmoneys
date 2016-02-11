using System;

namespace NMoneys.Serialization.Entity_Framework
{
	// TODO: implement IConvertible
	public class MonetaryQuantity
	{
		[Obsolete("serialization only")]
		private MonetaryQuantity() { }

		public MonetaryQuantity(Money money) :
			this(money.Amount, money.CurrencyCode.AlphabeticCode()) { }

		public MonetaryQuantity(decimal? amount, string currency)
		{
			Amount = amount;
			Currency = currency;
		}

		public string Currency { get; private set; }
		public decimal? Amount { get; private set; }

		public static MonetaryQuantity From(Money? money)
		{
			return money.HasValue ? new MonetaryQuantity(money.Value) : null;
		}

		public static Money? ToMoney(MonetaryQuantity quantity)
		{
			Money? money = default(Money?);
			if (quantity?.Amount != null)
			{
				string currency = string.IsNullOrWhiteSpace(quantity.Currency) ?
					CurrencyIsoCode.XXX.AlphabeticCode() :
					quantity.Currency;
				money = new Money(quantity.Amount.Value, currency);
			}
			return money;
		}

		public static implicit operator MonetaryQuantity(Money? money)
		{
			return From(money);
		}

		public static explicit operator Money? (MonetaryQuantity quantity)
		{
			return ToMoney(quantity);
		}
	}
}