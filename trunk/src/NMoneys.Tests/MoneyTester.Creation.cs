using System;
using System.ComponentModel;
using System.Globalization;
using NMoneys.Extensions;
using NMoneys.Tests.CustomConstraints;
using NUnit.Framework;

namespace NMoneys.Tests
{
	[TestFixture]
	public partial class MoneyTester
	{
		#region ForCulture

		[Test]
		public void ForCulture_DefinedCulture_PropertiesSet()
		{
			Money twentyBucks = Money.ForCulture(20, CultureInfo.GetCultureInfo("en-US"));
			Assert.That(twentyBucks.Amount, Is.EqualTo(20m));
			Assert.That(twentyBucks.CurrencyCode, Is.EqualTo(CurrencyIsoCode.USD));
		}

		[Test, Platform(Include = "Net-2.0")]
		public void ForCulture_OutdatedCulture_Exception()
		{
			Assert.That(() => Money.ForCulture(decimal.Zero, CultureInfo.GetCultureInfo("bg-BG")),
				Throws.InstanceOf<InvalidEnumArgumentException>().With.Message.StringContaining("BGL"),
				"Framework returns wrong ISOCurrencySymbol (BGL instead of BGN)");
		}

		[Test, Platform(Include = "Net-2.0")]
		public void ForCulture_CultureWithObsoleteCulture_EventRaised()
		{
			Action moneyWithObsoleteCurrency = () => Money.ForCulture(decimal.Zero, CultureInfo.GetCultureInfo("et-EE"));
			Assert.That(moneyWithObsoleteCurrency, Must.RaiseObsoleteEvent.Once());
		}

		[Test]
		public void ForCulture_NullDefaultCulture_Exception()
		{
			Assert.Throws<ArgumentNullException>(() => Money.ForCulture(decimal.Zero, null));
		}

		#endregion

		#region ForCurrentCulture

		[Test, SetCulture("da-DK")]
		public void ForCurrentCulture_DefinedCulture_PropertiesSet()
		{
			Money subject = Money.ForCurrentCulture(100);
			Assert.That(subject.Amount, Is.EqualTo(100m));
			Assert.That(subject.CurrencyCode, Is.EqualTo(CurrencyIsoCode.DKK));
		}

		[Test, Platform(Include = "Net-2.0"), SetCulture("bg-BG")]
		public void ForCurrentCulture_OutdatedCulture_Exception()
		{
			Assert.That(() => Money.ForCurrentCulture(decimal.Zero),
				Throws.InstanceOf<InvalidEnumArgumentException>().With.Message.StringContaining("BGL"),
				"Framework returns wrong ISOCurrencySymbol (BGL instead of BGN)");
		}

		[Test, Platform(Include = "Net-2.0"), SetCulture("et-EE")]
		public void ForCurrentCulture_CultureWithObsoleteCulture_EventRaised()
		{
			Action moneyWithObsoleteCurrency = () => Money.ForCurrentCulture(decimal.Zero);
			Assert.That(moneyWithObsoleteCurrency, Must.RaiseObsoleteEvent.Once());
		}

		#endregion

		#region Major/Minor

		[Test]
		public void ForMajor_Currency()
		{
			Assert.That(Currency.Gbp.SignificantDecimalDigits, Is.EqualTo(2));
			Assert.That(Money.ForMajor(234, Currency.Gbp), Must.Be.MoneyWith(234, Currency.Gbp));
		}

		[TestCaseSource("forMinorAllowingDecimals")]
		public void ForMinor_Currency_AllowingDecimals_AmountShiftedAsManyDigitsAsCurrencySpecifies(Currency currency, int decimalDigits, long minorAmount, decimal amount)
		{
			Assert.That(currency.SignificantDecimalDigits, Is.EqualTo(decimalDigits));
			Assert.That(Money.ForMinor(minorAmount, currency), Must.Be.MoneyWith(amount, currency));
		}

#pragma warning disable 169
		private static TestCaseData[] forMinorAllowingDecimals = new[]
		{
			new TestCaseData(Currency.Gbp, 2, 234, 2.34m).SetName("bigger than cent factor"),
			new TestCaseData(Currency.Gbp, 2, 34, .34m).SetName("same as cent factor"),
			new TestCaseData(Currency.Gbp, 2, 4, .04m).SetName("less than cent factor"),
			new TestCaseData(Currency.Get(CurrencyIsoCode.BHD), 3, 2345, 2.345m).SetName("bigger than cent factor"),
			new TestCaseData(Currency.Get(CurrencyIsoCode.BHD), 3, 234, .234m).SetName("same as cent factor"),
			new TestCaseData(Currency.Get(CurrencyIsoCode.BHD), 3, 34, .034m).SetName("less than cent factor"),
		};
#pragma warning restore 169

		[Test]
		public void ForMinor_Currency_NotAllowingDecimals_AmountStaysAtItis()
		{
			Currency notAllowingDecimals = Currency.Jpy;
			Assert.That(notAllowingDecimals.SignificantDecimalDigits, Is.EqualTo(0));
			Assert.That(Money.ForMinor(234, notAllowingDecimals), Must.Be.MoneyWith(234m, notAllowingDecimals));
		}

		[Test]
		public void MajorAmount_NotDecimalAmount_SameValueForBoth()
		{
			Assert.That(tenner.MajorAmount, Is.EqualTo(10m));
			Assert.That(oweMeQuid.MajorAmount, Is.EqualTo(-1m));
		}

		[Test]
		public void MajorAmount_DecimalAmount_Truncated()
		{
			Assert.That(2.5m.Eur().MajorAmount, Is.EqualTo(2m));
			Assert.That(-2.5m.Eur().MajorAmount, Is.EqualTo(-2m));
		}

		[Test]
		public void MajorIntegralAmount_NotDecimalAmount_SameValueForBoth()
		{
			Assert.That(tenner.MajorIntegralAmount, Is.EqualTo(10L));
			Assert.That(oweMeQuid.MajorIntegralAmount, Is.EqualTo(-1L));
		}

		[Test]
		public void MajorIntegralAmount_DecimalAmount_Truncated()
		{
			Assert.That(2.5m.Eur().MajorIntegralAmount, Is.EqualTo(2L));
			Assert.That(-2.5m.Eur().MajorIntegralAmount, Is.EqualTo(-2L));
		}

		[Test]
		public void MinorAmount_CurrencyWithDecimals_DecimalShifted()
		{
			Assert.That(new Money(2.34m, Currency.Gbp).MinorAmount, Is.EqualTo(234m));
			Assert.That(new Money(-2.34m, Currency.Gbp).MinorAmount, Is.EqualTo(-234m));
			Assert.That(new Money(-2.3m, Currency.Eur).MinorAmount, Is.EqualTo(-230m));
			Assert.That(new Money(12.378m, Currency.Eur).MinorAmount, Is.EqualTo(1237m));
			Assert.That(new Money(5m, Currency.Eur).MinorAmount, Is.EqualTo(500m));
		}

		[Test]
		public void MinorAmount_CurrencyWithoutDecimals_DecimalNotShifted()
		{
			Assert.That(new Money(2.34m, Currency.Jpy).MinorAmount, Is.EqualTo(2m));
			Assert.That(new Money(-2.34m, Currency.Jpy).MinorAmount, Is.EqualTo(-2m));
		}

		[Test]
		public void MinorIntegralAmount_CurrencyWithDecimals_DecimalShifted()
		{
			Assert.That(new Money(2.34m, Currency.Gbp).MinorIntegralAmount, Is.EqualTo(234L));
			Assert.That(new Money(-2.34m, Currency.Gbp).MinorIntegralAmount, Is.EqualTo(-234L));
			Assert.That(new Money(-2.3m, Currency.Eur).MinorIntegralAmount, Is.EqualTo(-230m));
			Assert.That(new Money(12.378m, Currency.Eur).MinorIntegralAmount, Is.EqualTo(1237L));
			Assert.That(new Money(5m, Currency.Eur).MinorIntegralAmount, Is.EqualTo(500L));
		}

		[Test]
		public void MinorIntegralAmount_CurrencyWithoutDecimals_DecimalNotShifted()
		{
			Assert.That(new Money(2.34m, Currency.Jpy).MinorIntegralAmount, Is.EqualTo(2L));
			Assert.That(new Money(-2.34m, Currency.Jpy).MinorIntegralAmount, Is.EqualTo(-2L));
		}

		#endregion
	}
}