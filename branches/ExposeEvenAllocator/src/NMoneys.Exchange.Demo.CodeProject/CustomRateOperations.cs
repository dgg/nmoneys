using NMoneys.Extensions;
using NUnit.Framework;

namespace NMoneys.Exchange.Demo.CodeProject
{
	[TestFixture]
	public class CustomRateOperations
	{
		public class CustomRateArithmetic : ExchangeRate
		{
			public CustomRateArithmetic(CurrencyIsoCode from, CurrencyIsoCode to, decimal rate) : base(from, to, rate) { }

			public override Money Apply(Money from)
			{
				// instead of this useless "return 0" policy one can implement rounding policies, for instance
				return new Money(0m, To);
			}
		}

		public class CustomArithmeticProvider : IExchangeRateProvider
		{
			public ExchangeRate Get(CurrencyIsoCode from, CurrencyIsoCode to)
			{
				return new CustomRateArithmetic(from, to, 1m);
			}

			public bool TryGet(CurrencyIsoCode from, CurrencyIsoCode to, out ExchangeRate rate)
			{
				rate = new CustomRateArithmetic(from, to, 1m);
				return true;
			}
		}

		[Test]
		public void Use_CustomArithmeticProvider()
		{
			var customProvider = new CustomArithmeticProvider();

			ExchangeRateProvider.Factory = () => customProvider;

			var zeroDollars = 10m.Eur().Convert().To(CurrencyIsoCode.USD);
			Assert.That(zeroDollars, Is.EqualTo(0m.Usd()));

			// go back to default
			ExchangeRateProvider.Factory = ExchangeRateProvider.Default;
		}
	}
}
