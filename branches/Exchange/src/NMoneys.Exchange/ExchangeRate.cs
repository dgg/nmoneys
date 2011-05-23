using System.Globalization;

namespace NMoneys.Exchange
{
	public class ExchangeRate
	{
		public ExchangeRate(CurrencyIsoCode from, CurrencyIsoCode to, decimal rate)
		{
			From = from;
			To = to;
			Rate = rate;
		}

		public CurrencyIsoCode From { get; private set; }
		public CurrencyIsoCode To { get; private set; }
		public decimal Rate { get; private set; }

		public virtual ExchangeRate Inverse()
		{
			return new ExchangeRate(To, From, 1m / Rate);
		}

		public virtual Money Apply(Money from)
		{
			AssertCompatibility(from.CurrencyCode);

			return new Money(from.Amount * Rate, To);
		}

		protected virtual void AssertCompatibility(CurrencyIsoCode from)
		{
			if (from != From) throw new DifferentCurrencyException(From.AlphabeticCode(), from.AlphabeticCode());
		}

		public override string ToString()
		{
			return From + " --> " + To + " @ " + Rate.ToString(CultureInfo.InvariantCulture);
		}
	}
}