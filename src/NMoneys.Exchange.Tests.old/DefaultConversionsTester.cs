using System;
using NMoneys.Extensions;
using NUnit.Framework;

namespace NMoneys.Exchange.Tests
{
	[TestFixture]
	public class DefaultConversionsTester
	{
		[Test]
		public void Convert_To_CurrencyCode_DefaultConvertion()
		{
			Money thirteenEuro = 13m.Usd().Convert().To(CurrencyIsoCode.EUR);

			Assert.That(thirteenEuro.Amount, Is.EqualTo(13m), "the default exchange provider merely changes the currency");
			Assert.That(thirteenEuro.CurrencyCode, Is.EqualTo(CurrencyIsoCode.EUR));
		}

		[Test]
		public void Convert_To_Currency_DefaultConversion()
		{
			Money thirteenEuro = 13m.Usd().Convert().To(Currency.Euro);

			Assert.That(thirteenEuro.Amount, Is.EqualTo(13m), "the default exchange provider merely changes the currency");
			Assert.That(thirteenEuro.CurrencyCode, Is.EqualTo(CurrencyIsoCode.EUR));
		}

		[Test]
		public void Convert_To_NullCurrency_Exception()
		{
			Assert.That(() => 13m.Usd().Convert().To(null), Throws.InstanceOf<ArgumentNullException>());
		}

		[Test]
		public void TryConvert_To_CurrencyCode_DefaultConversion()
		{
			Money? thirteenEuro = 13m.Usd().TryConvert().To(CurrencyIsoCode.EUR);

			Assert.That(thirteenEuro.HasValue, Is.True);
			Assert.That(thirteenEuro.GetValueOrDefault().Amount, Is.EqualTo(13m), "the default exchange provider merely changes the currency");
			Assert.That(thirteenEuro.GetValueOrDefault().CurrencyCode, Is.EqualTo(CurrencyIsoCode.EUR));
		}

		[Test]
		public void TryConvert_To_Currency_DefaultConversion()
		{
			Money? thirteenEuro = 13m.Usd().TryConvert().To(Currency.Euro);

			Assert.That(thirteenEuro.HasValue, Is.True);
			Assert.That(thirteenEuro.GetValueOrDefault().Amount, Is.EqualTo(13m), "the default exchange provider merely changes the currency");
			Assert.That(thirteenEuro.GetValueOrDefault().CurrencyCode, Is.EqualTo(CurrencyIsoCode.EUR));
		}

		[Test]
		public void TryConvert_To_NullCurrency_DefaultConversion()
		{
			Money? thirteenEuro = 13m.Usd().TryConvert().To(null);

			Assert.That(thirteenEuro.HasValue, Is.False, "the default safe converter ignores null currency instances");
			Assert.That(thirteenEuro.GetValueOrDefault(), Is.EqualTo(default(Money)));
		}
	}
}
