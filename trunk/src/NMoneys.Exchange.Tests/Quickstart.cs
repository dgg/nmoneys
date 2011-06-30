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

		#region extend conversion operations

		[Test]
		public void Creating_New_ConversionOperations()
		{
			var hundredDollars = new Money(100m, CurrencyIsoCode.USD);

			var hundredEuros = hundredDollars.Convert().Using(1m).To(CurrencyIsoCode.EUR);

			Assert.That(hundredEuros.Amount, Is.EqualTo(100m));
			Assert.That(hundredEuros.CurrencyCode, Is.EqualTo(CurrencyIsoCode.EUR));
		}

		#endregion
	}

	internal static class ExtendedConversions
	{
		public static UsingImplementor Using(this IExchangeConversion conversion, decimal rate)
		{
			return new UsingImplementor(conversion.From, rate);
		}

		public class UsingImplementor
		{
			private readonly Money _from;
			private readonly decimal _rate;

			public UsingImplementor(Money from, decimal rate)
			{
				_from = from;
				_rate = rate;
			}

			public Money To(CurrencyIsoCode to)
			{
				var rateCalculator = new ExchangeRate(_from.CurrencyCode, to, _rate);
				return rateCalculator.Apply(_from);	
			}
		}
	}
}
