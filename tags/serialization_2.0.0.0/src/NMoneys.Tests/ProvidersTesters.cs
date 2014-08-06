using System;
using NMoneys.Support;
using NMoneys.Tests.CustomConstraints;
using NMoneys.Tests.Support;
using NUnit.Framework;

namespace NMoneys.Tests
{
	[TestFixture]
	public class EmbeddedXmlProviderTester
	{
		[Test]
		public void Get_Euro_EuroCurrencyInfo()
		{
			var subject = new EmbeddedXmlProvider();

			CurrencyInfo info = subject.Get(CurrencyIsoCode.EUR);
			Assert.That(info, Must.Be.CurrencyInfo()
				.WithCode(CurrencyIsoCode.EUR)
				.WithEnglishName("Euro")
				.WithNativeName("Euro")
				.WithSymbol("€")
				.WithSignificantDecimalDigits(2)
				.WithDecimalSeparator(",")
				.WithGroupSeparator(".")
				.WithGroupSizes(new[] { 3 })
				.WithPositivePattern(3)
				.WithNegativePattern(8));
		}

		[Test]
		public void Get_DanishKrone_DanishKroneCurrencyInfo()
		{
			var subject = new EmbeddedXmlProvider();

			CurrencyInfo info = subject.Get(CurrencyIsoCode.DKK);
			Assert.That(info, Must.Be.CurrencyInfo()
				.WithCode(CurrencyIsoCode.DKK)
				.WithEnglishName("Danish Krone")
				.WithNativeName("Dansk krone")
				.WithSymbol("kr")
				.WithSignificantDecimalDigits(2)
				.WithDecimalSeparator(",")
				.WithGroupSeparator(".")
				.WithGroupSizes(new[] { 3 })
				.WithPositivePattern(2)
				.WithNegativePattern(12));
		}

		[Test]
		public void Get_UndefinedCurrency_Exception()
		{
			CurrencyIsoCode notDefined = (CurrencyIsoCode)(-1);
			Assert.That(() => new EmbeddedXmlProvider().Get(notDefined),
				Throws.InstanceOf<MisconfiguredCurrencyException>().With.Message.StringContaining("-1"));
		}
	}

	[TestFixture]
	public class EmbeddedXmlInitializerTester
	{
		[Test]
		public void Get_Euro_EuroCurrencyInfo()
		{
			using (var subject = new EmbeddedXmlInitializer())
			{
				CurrencyInfo info = subject.Get(CurrencyIsoCode.EUR);
				Assert.That(info, Must.Be.CurrencyInfo()
					.WithCode(CurrencyIsoCode.EUR)
					.WithEnglishName("Euro")
					.WithNativeName("Euro")
					.WithSymbol("€")
					.WithSignificantDecimalDigits(2)
					.WithDecimalSeparator(",")
					.WithGroupSeparator(".")
					.WithGroupSizes(new[] { 3 })
					.WithPositivePattern(3)
					.WithNegativePattern(8));
			}
		}

		[Test]
		public void Get_DanishKrone_DanishKroneCurrencyInfo()
		{
			using (var subject = new EmbeddedXmlInitializer())
			{
				CurrencyInfo info = subject.Get(CurrencyIsoCode.DKK);
				Assert.That(info, Must.Be.CurrencyInfo()
					.WithCode(CurrencyIsoCode.DKK)
					.WithEnglishName("Danish Krone")
					.WithNativeName("Dansk krone")
					.WithSymbol("kr")
					.WithSignificantDecimalDigits(2)
					.WithDecimalSeparator(",")
					.WithGroupSeparator(".")
					.WithGroupSizes(new[] { 3 })
					.WithPositivePattern(2)
					.WithNegativePattern(12));
			}
		}

		[Test]
		public void Get_UndefinedCurrency_Exception()
		{
			CurrencyIsoCode notDefined = (CurrencyIsoCode)(-1);
			using (var subject = new EmbeddedXmlInitializer())
			{
				Assert.That(() => subject.Get(notDefined),
					Throws.InstanceOf<MisconfiguredCurrencyException>().With.Message.StringContaining("-1"));
			}
		}

		[Test, Category("Performance")]
		public void MultipleCallsToInitializer_AreFasterThan_MultipleCallsToProvider()
		{
			var provider = new EmbeddedXmlProvider();
			TimeSpan providerTime = ActionTimer.Time(() =>
			{
				foreach (var code in Enumeration.GetValues<CurrencyIsoCode>())
				{
					provider.Get(code);
				}
			});

			TimeSpan initializerTime = ActionTimer.Time(() =>
			{
				using (var initializer = new EmbeddedXmlInitializer())
				{
					foreach (var code in Enumeration.GetValues<CurrencyIsoCode>())
					{
						initializer.Get(code);
					}
				}
			});

			Assert.That(initializerTime, Is.LessThan(providerTime));
		}
	}
}
