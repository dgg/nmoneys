using System;
using NMoneys.Extensions;
using NUnit.Framework;

namespace NMoneys.Exchange.Tests
{
	[TestFixture]
	public class ExtendedConversionsTester
	{
		// Implement alternative provider
		class RateOfTwoProvider : IExchangeRateProvider
		{
			public ExchangeRate Get(CurrencyIsoCode from, CurrencyIsoCode to)
			{
				return new ExchangeRate(from, to, 2);
			}

			public bool TryGet(CurrencyIsoCode from, CurrencyIsoCode to, out ExchangeRate rate)
			{
				rate = Get(from, to);
				return true;
			}
		}

		[TestFixtureSetUp]
		public void setupRateOfTwoProvider()
		{
			ExchangeRateProvider.Provider = () => new RateOfTwoProvider();
		}

		[TestFixtureTearDown]
		public void resetProvider()
		{
			ExchangeRateProvider.Provider = ExchangeRateProvider.Default;
		}

		[Test]
		public void Convert_To_CurrencyCode_DoubledConversion()
		{
			Money oewMeThirteenEuro = 13m.Usd().Convert().To(CurrencyIsoCode.EUR);

			Assert.That(oewMeThirteenEuro.Amount, Is.EqualTo(26m), "the rate of two exchange provider multiplies the amount by two and changes the currency");
			Assert.That(oewMeThirteenEuro.CurrencyCode, Is.EqualTo(CurrencyIsoCode.EUR));
		}

		[Test]
		public void Convert_To_Currency_DoubledConversion()
		{
			Money oewMeThirteenEuro = 13m.Usd().Convert().To(Currency.Euro);

			Assert.That(oewMeThirteenEuro.Amount, Is.EqualTo(26m), "the rate of two exchange provider multiplies the amount by two and changes the currency");
			Assert.That(oewMeThirteenEuro.CurrencyCode, Is.EqualTo(CurrencyIsoCode.EUR));
		}

		[Test]
		public void Convert_To_NullCurrency_Exception()
		{
			Assert.That(() => 13m.Usd().Convert().To(null), Throws.InstanceOf<ArgumentNullException>());
		}

		[Test]
		public void TryConvert_To_CurrencyCode_DoubledConversion()
		{
			Money? oewMeThirteenEuro = 13m.Usd().TryConvert().To(CurrencyIsoCode.EUR);

			Assert.That(oewMeThirteenEuro.HasValue, Is.True);
			Assert.That(oewMeThirteenEuro.GetValueOrDefault().Amount, Is.EqualTo(26m), "the rate of two exchange provider multiplies the amount by two and changes the currency");
			Assert.That(oewMeThirteenEuro.GetValueOrDefault().CurrencyCode, Is.EqualTo(CurrencyIsoCode.EUR));
		}

		[Test]
		public void TryConvert_To_Currency_DoubledConversion()
		{
			Money? oewMeThirteenEuro = 13m.Usd().TryConvert().To(Currency.Euro);

			Assert.That(oewMeThirteenEuro.HasValue, Is.True);
			Assert.That(oewMeThirteenEuro.GetValueOrDefault().Amount, Is.EqualTo(26m), "the rate of two exchange provider multiplies the amount by two and changes the currency");
			Assert.That(oewMeThirteenEuro.GetValueOrDefault().CurrencyCode, Is.EqualTo(CurrencyIsoCode.EUR));
		}

		[Test]
		public void TryConvert_To_NullCurrency_NegatedConversion()
		{
			Money? oewMeThirteenEuro = 13m.Usd().TryConvert().To(null);

			Assert.That(oewMeThirteenEuro.HasValue, Is.False, "the default safe converter ignores null currency instances");
			Assert.That(oewMeThirteenEuro.GetValueOrDefault(), Is.EqualTo(default(Money)));
		}
	}
}
