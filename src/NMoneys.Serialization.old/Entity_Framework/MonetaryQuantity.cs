using System;
using System.Collections.Generic;

namespace NMoneys.Serialization.Entity_Framework
{
	public sealed class MonetaryQuantity : IEquatable<MonetaryQuantity>
	{
		[Obsolete("serialization only")]
		private MonetaryQuantity() { }

		public MonetaryQuantity(Money money) :
			this(money.Amount, money.CurrencyCode.AlphabeticCode())
		{ }

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

		public static explicit operator MonetaryQuantity(Money? money)
		{
			return From(money);
		}

		public static explicit operator Money? (MonetaryQuantity quantity)
		{
			return ToMoney(quantity);
		}

		public bool Equals(MonetaryQuantity other)
		{
			if (ReferenceEquals(null, other)) return false;
			if (ReferenceEquals(this, other)) return true;
			return EqualityComparer<Money?>.Default.Equals(ToMoney(this), ToMoney(other));
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != GetType()) return false;
			return Equals((MonetaryQuantity)obj);
		}

		public override int GetHashCode()
		{
			return EqualityComparer<Money?>.Default.GetHashCode(ToMoney(this));
		}

		public static bool operator ==(MonetaryQuantity left, MonetaryQuantity right)
		{
			return Equals(left, right);
		}

		public static bool operator !=(MonetaryQuantity left, MonetaryQuantity right)
		{
			return !Equals(left, right);
		}
	}
}