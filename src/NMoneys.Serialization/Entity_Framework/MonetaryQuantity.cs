using System;

namespace NMoneys.Serialization.Entity_Framework
{
	public class MonetaryQuantity
	{
		[Obsolete("serialization only")]
		private MonetaryQuantity() { }

		public MonetaryQuantity(Money money)
		{
			Currency = money.CurrencyCode.AlphabeticCode();
			Amount = money.Amount;
		}

		public string Currency { get; private set; }
		public decimal? Amount { get; private set; }

		public static MonetaryQuantity From(Money? money)
		{
			return money.HasValue ? new MonetaryQuantity(money.Value) : null;
		}
	}
}