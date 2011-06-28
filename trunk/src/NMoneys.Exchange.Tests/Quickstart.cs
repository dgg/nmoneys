using NUnit.Framework;

namespace NMoneys.Exchange.Tests
{
	[TestFixture]
	public class Quickstart
	{
		[Test]
		public void Default_Conversions()
		{
			var tenEuro = new Money(10m, CurrencyIsoCode.EUR);

			var tenDollar = tenEuro.Convert().To(CurrencyIsoCode.USD);
			var tenPounds = tenEuro.Convert().To(Currency.Gbp);
		}

		#region configuring a provider

		[Test]
		public void Configuring_Provider()
		{
			var customProvider = new TabulatedExchangeRateProvider();
			customProvider.Add(CurrencyIsoCode.EUR, CurrencyIsoCode.USD, 0);

			ExchangeRateProvider.Factory = () => customProvider;

			var tenEuro = new Money(10m, CurrencyIsoCode.EUR);
			var zeroDollars = tenEuro.Convert().To(CurrencyIsoCode.USD);

			// go back to default
			ExchangeRateProvider.Factory = ExchangeRateProvider.Default;
		}

		#endregion

		#region using custom arithmetic

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

		public class CustomRateArithmetic : ExchangeRate
		{
			public CustomRateArithmetic(CurrencyIsoCode from, CurrencyIsoCode to, decimal rate) : base(from, to, rate) { }

			public override Money Apply(Money from)
			{
				return new Money(0m, To);
			}
		}

		#endregion
	}
}
