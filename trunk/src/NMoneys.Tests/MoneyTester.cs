using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using NMoneys.Extensions;
using NMoneys.Support;
using NMoneys.Tests.CustomConstraints;
using NMoneys.Tests.Support;
using NUnit.Framework;

namespace NMoneys.Tests
{
	[TestFixture]
	public partial class MoneyTester
	{
		private static readonly Money fiver = new Money(5, CurrencyIsoCode.GBP),
			tenner = new Money(10, CurrencyIsoCode.GBP),
			hund = new Money(100, CurrencyIsoCode.DKK),
			nought = new Money(0, CurrencyIsoCode.GBP),
			oweMeQuid = new Money(-1, Currency.Gbp);

		private static readonly decimal twoThirds = 2m / 3;

		#region Ctor

		[Test]
		public void Ctor_Default_CurrencyIsNotDefault_ButUndefined()
		{
			Money @default = new Money();

			Assert.That(@default.CurrencyCode, Is.Not.EqualTo(default(CurrencyIsoCode)).And.EqualTo(CurrencyIsoCode.XXX));
		}

		[Test]
		public void Ctor_Amount_AmountWithNoneCurrency()
		{
			Money defaultMoney = new Money(3m);
			Assert.That(defaultMoney.Amount, Is.EqualTo(3m));
			Assert.That(defaultMoney.CurrencyCode, Is.EqualTo(CurrencyIsoCode.XXX));
		}

		[Test]
		public void Ctor_ExistingIsoCode_PropertiesSet()
		{
			Money tenDollars = new Money(10, CurrencyIsoCode.USD);
			Assert.That(tenDollars.Amount, Is.EqualTo(10m));
			Assert.That(tenDollars.CurrencyCode, Is.EqualTo(CurrencyIsoCode.USD));
		}

		[Test]
		public void Ctor_ObsoleteIsoCode_EventRaised()
		{
			CurrencyIsoCode obsolete = Enumeration.Parse<CurrencyIsoCode>("EEK");
			Action moneyWithObsoleteCurrency = () => new Money(10, obsolete);
			Assert.That(moneyWithObsoleteCurrency, Must.RaiseObsoleteEvent.Once());
		}

		[Test]
		public void Ctor_ExistingIsoSymbol_PropertiesSet()
		{
			Money hundredLerus = new Money(100, "EUR");
			Assert.That(hundredLerus.Amount, Is.EqualTo(100m));
			Assert.That(hundredLerus.CurrencyCode, Is.EqualTo(CurrencyIsoCode.EUR));
		}

		[Test]
		public void Ctor_ObsoleteIsoSymbol_EventRaised()
		{
			Action moneyWithObsoleteCurrency = () => new Money(10, "EEK");
			Assert.That(moneyWithObsoleteCurrency, Must.RaiseObsoleteEvent.Once());
		}

		[Test]
		public void Ctor_NullSymbol_Exception()
		{
			Assert.That(() => new Money(decimal.Zero, (string)null), Throws.InstanceOf<ArgumentNullException>());
		}

		[Test]
		public void Ctor_ObsoleteCurrency_EventRaised()
		{
			Currency obsolete = Currency.Get("EEK");
			Action moneyWithObsoleteCurrency = () => new Money(10, obsolete);
			Assert.That(moneyWithObsoleteCurrency, Must.RaiseObsoleteEvent.Once());
		}

		[Test]
		public void Ctor_Currency_PropertiesSet()
		{
			Assert.That(tenner.Amount, Is.EqualTo(10m));
			Assert.That(tenner.CurrencyCode, Is.EqualTo(CurrencyIsoCode.GBP));
		}

		[Test]
		public void Ctor_NullCurrency_Exception()
		{
			Assert.That(() => new Money(decimal.Zero, (Currency)null), Throws.InstanceOf<ArgumentNullException>());
		}

		[Test]
		public void Ctor_NonExistingIsoCode_Exception()
		{
			CurrencyIsoCode nonExistingCode = (CurrencyIsoCode)(-7);

			Assert.That(() => new Money(decimal.Zero, nonExistingCode), Throws.InstanceOf<InvalidEnumArgumentException>().With.Message.StringContaining("-7"));
		}

		[Test]
		public void Ctor_NonExistingIsoSymbol_PropertiesSet()
		{
			string nonExistentIsoSymbol = "XYZ";
			Assert.That(() => new Money(decimal.Zero, nonExistentIsoSymbol), Throws.InstanceOf<InvalidEnumArgumentException>());
		}

		#endregion

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

		#region Total

		[Test]
		public void Total_AllOfSameCurrency_NewInstanceWithAmountAsSumOfAll()
		{
			Assert.That(Money.Total(2m.Dollars(), 3m.Dollars(), 5m.Dollars()), Must.Be.MoneyWith(10m, Currency.Dollar));
		}

		[Test]
		public void Total_DiffCurrency_NewInstanceWithAmountAsSumOfAll()
		{
			Assert.That(() => Money.Total(2m.Dollars(), 3m.Eur(), 5m.Dollars()), Throws.InstanceOf<DifferentCurrencyException>());
		}

		[Test]
		public void Total_NullMoneys_Exception()
		{
			IEnumerable<Money> nullMoneys = null;

			Assert.That(() => Money.Total(nullMoneys), Throws.InstanceOf<ArgumentNullException>()
				.With.Message.StringContaining("moneys"));
		}

		[Test]
		public void Total_EmptyMoneys_Exception()
		{
			Assert.That(() => Money.Total(), Throws.ArgumentException.With.Message.StringContaining("empty"));
			Assert.That(() => Money.Total(new Money[0]), Throws.ArgumentException.With.Message.StringContaining("empty"));
		}

		[Test]
		public void Total_OnlyOneMoney_AnotherMoneyInstanceWithSameInformation()
		{
			Assert.That(Money.Total(10m.Usd()), Is.EqualTo(10m.Usd()));
		}

		#endregion

		#region property value checking

		[Test]
		public void IsZero_TrueWhenZero()
		{
			Assert.That(fiver.IsZero(), Is.False);
			Assert.That(nought.IsZero(), Is.True);
			Assert.That(oweMeQuid.IsZero(), Is.False);
		}

		[Test]
		public void IsPositive_TrueWhenPositive()
		{
			Assert.That(fiver.IsPositive(), Is.True);
			Assert.That(nought.IsPositive(), Is.False);
			Assert.That(oweMeQuid.IsPositive(), Is.False);
		}

		[Test]
		public void IsPositiveOrZero_TruenWhenPositiveOrZero()
		{
			Assert.That(fiver.IsPositiveOrZero(), Is.True);
			Assert.That(nought.IsPositiveOrZero(), Is.True);
			Assert.That(oweMeQuid.IsPositiveOrZero(), Is.False);
		}

		[Test]
		public void IsNegative_TrueWhenNegative()
		{
			Assert.That(fiver.IsNegative(), Is.False);
			Assert.That(nought.IsNegative(), Is.False);
			Assert.That(oweMeQuid.IsNegative(), Is.True);
		}

		[Test]
		public void IsNegativeOrZero_TrueWhenNegativeOrZero()
		{
			Assert.That(fiver.IsNegativeOrZero(), Is.False);
			Assert.That(nought.IsNegativeOrZero(), Is.True);
			Assert.That(oweMeQuid.IsNegativeOrZero(), Is.True);
		}

		[TestCaseSource("sameCurrency")]
		public void HasSameCurrency_SameCurrency_True(Money subject, Money argument)
		{
			Assert.That(subject.HasSameCurrencyAs(argument), Is.True);
		}

		[TestCaseSource("differentCurrency")]
		public void HasSameCurrency_DifferentCurrency_False(Money subject, Money argument)
		{
			Assert.That(subject.HasSameCurrencyAs(argument), Is.False);
		}

		[TestCaseSource("sameCurrency")]
		public void AssertSameCurrency_SameCurrency_NoException(Money subject, Money argument)
		{
			Assert.That(() => subject.AssertSameCurrency(argument), Throws.Nothing);
		}

		[TestCaseSource("differentCurrency")]
		public void AssertSameCurrency_DifferentCurrency_Exception(Money subject, Money argument)
		{
			Assert.That(() => subject.AssertSameCurrency(argument), Throws.InstanceOf<DifferentCurrencyException>());
		}

#pragma warning disable 169
		private static readonly object[] sameCurrency = new object[]
		{
			new object[] {fiver, fiver},
			new object[] {fiver, tenner},			
			new object[] {tenner, fiver},
		};

		private static readonly object[] differentCurrency = new object[]
		{
			new object[] {fiver, hund },
			new object[] {hund, fiver },
		};
#pragma warning restore 169

		#endregion

		#region operators

		[Test]
		public void UnaryNegate_AnotherMoneyWithNegativeAmount()
		{
			Money oweYouFive = -fiver;
			Assert.That(oweYouFive, Is.Not.SameAs(fiver));
			Assert.That(oweYouFive.Amount, Is.EqualTo(-(fiver.Amount)));
			Assert.That(oweYouFive.CurrencyCode, Is.EqualTo(fiver.CurrencyCode));

			Money youOweMeFive = -oweYouFive;
			Assert.That(youOweMeFive.Amount, Is.EqualTo(fiver.Amount));
		}

		[Test]
		public void Add_SameCurrency_AnotherMoneyWithAddedUpamount()
		{
			Money fifteenQuid = fiver + tenner;

			Assert.That(fifteenQuid, Is.Not.SameAs(fiver).And.Not.SameAs(tenner));
			Assert.That(fifteenQuid.Amount, Is.EqualTo(15));
			Assert.That(fifteenQuid.CurrencyCode, Is.EqualTo(CurrencyIsoCode.GBP));
		}

		[Test]
		public void Add_DifferentCurrency_Exception()
		{
			Money money;
			Assert.That(() => money = fiver + hund, Throws.InstanceOf<DifferentCurrencyException>());
		}

		[Test]
		public void Substract_SameCurrency_AnotherMoneyWithAddedUpamount()
		{
			Money anotherFiver = tenner - fiver;

			Assert.That(anotherFiver, Is.Not.SameAs(fiver).And.Not.SameAs(tenner));
			Assert.That(anotherFiver.Amount, Is.EqualTo(5));
			Assert.That(anotherFiver.CurrencyCode, Is.EqualTo(CurrencyIsoCode.GBP));
		}

		[Test]
		public void Substract_TennerMinusFiver_OweMeMoney()
		{
			Money oweFiver = fiver - tenner;
			Assert.That(oweFiver.Amount, Is.EqualTo(-5));
		}

		[Test]
		public void Substract_DifferentCurrency_Exception()
		{
			Money money;
			Assert.That(() => money = fiver - hund, Throws.InstanceOf<DifferentCurrencyException>());
		}

		#endregion

		#region Arithmetic

		[Test]
		public void Negate_AnotherMoneyWithNegativeAmount()
		{
			Money oweYouFive = fiver.Negate();
			Assert.That(oweYouFive, Is.Not.SameAs(fiver));
			Assert.That(oweYouFive.Amount, Is.EqualTo(-(fiver.Amount)));
			Assert.That(oweYouFive.CurrencyCode, Is.EqualTo(fiver.CurrencyCode));

			Money youOweMeFive = oweYouFive.Negate();
			Assert.That(youOweMeFive.Amount, Is.EqualTo(fiver.Amount));
		}

		[Test]
		public void Plus_SameCurrency_AnotherMoneyWithAddedUpamount()
		{
			Money fifteenQuid = fiver.Plus(tenner);

			Assert.That(fifteenQuid, Is.Not.SameAs(fiver).And.Not.SameAs(tenner));
			Assert.That(fifteenQuid.Amount, Is.EqualTo(15));
			Assert.That(fifteenQuid.CurrencyCode, Is.EqualTo(CurrencyIsoCode.GBP));
		}

		[Test]
		public void Plus_DifferentCurrency_Exception()
		{
			Money money;
			Assert.That(() => money = fiver.Plus(hund), Throws.InstanceOf<DifferentCurrencyException>());
		}

		[Test]
		public void Minus_SameCurrency_AnotherMoneyWithAddedUpamount()
		{
			Money anotherFiver = tenner.Minus(fiver);

			Assert.That(anotherFiver, Is.Not.SameAs(fiver).And.Not.SameAs(tenner));
			Assert.That(anotherFiver.Amount, Is.EqualTo(5));
			Assert.That(anotherFiver.CurrencyCode, Is.EqualTo(CurrencyIsoCode.GBP));
		}

		[Test]
		public void Minus_TennerMinusFiver_OweMeMoney()
		{
			Money oweFiver = fiver.Minus(tenner);
			Assert.That(oweFiver.Amount, Is.EqualTo(-5));
		}

		[Test]
		public void Minus_DifferentCurrency_Exception()
		{
			Money money;
			Assert.That(() => money = fiver.Minus(hund), Throws.InstanceOf<DifferentCurrencyException>());
		}

		[Test]
		public void Abs_GetsPositiveAmount()
		{
			Assert.That(fiver.Abs().Amount, Is.EqualTo(fiver.Amount));

			Assert.That((fiver - tenner).Abs().Amount, Is.EqualTo(5));
		}

		#region TruncateToSignificantDecimalDigits

		[Test]
		public void TruncateToSignificantDecimalDigits_CurrencyWith2Decimals_AmountHas2Decimals()
		{
			Money subject = new Money(twoThirds, CurrencyIsoCode.XXX);

			Assert.That(subject.Amount, Is.Not.EqualTo(0.66m), "has more decimals");
			Assert.That(subject.TruncateToSignificantDecimalDigits().Amount, Is.EqualTo(0.66m));
		}

		[Test]
		public void TruncateToSignificantDecimalDigits_CultureWithSameCurrency_CultureApplied()
		{
			Money subject = new Money(twoThirds, CurrencyIsoCode.EUR);
			NumberFormatInfo spanishFormatting = CultureInfo.GetCultureInfo("es-ES").NumberFormat;

			Assert.That(subject.Amount, Is.Not.EqualTo(0.66m),
						"the raw amount has more decimals");
			Assert.That(subject.TruncateToSignificantDecimalDigits(spanishFormatting).Amount, Is.EqualTo(0.66m),
						"in Spain, 2 decimals are used for currencies");
		}

		[Test]
		public void TruncateToSignificantDecimalDigits_CultureWithDifferentCurrency_CultureApplied()
		{
			Money subject = new Money(twoThirds, CurrencyIsoCode.GBP);
			NumberFormatInfo spanishFormatting = CultureInfo.GetCultureInfo("es-ES").NumberFormat;

			Assert.That(subject.Amount, Is.Not.EqualTo(0.66m),
						"the raw amount has more decimals");
			Assert.That(subject.TruncateToSignificantDecimalDigits(spanishFormatting).Amount, Is.EqualTo(0.66m),
						"in Spain, 2 decimals are used for currencies");
		}

		#endregion

		#region Truncate

		[TestCaseSource("truncate")]
		public void Truncate_Spec(decimal amount, decimal truncatedAmount)
		{
			var subject = new Money(amount, CurrencyIsoCode.XTS);

			Money truncated = subject.Truncate();
			Assert.That(truncated.Amount, Is.EqualTo(truncatedAmount));
			Assert.That(truncated.CurrencyCode, Is.EqualTo(CurrencyIsoCode.XTS));
		}

#pragma warning disable 169
		private static readonly object truncate = new[]
		{
			new [] {0m, 0m},
			new []{123.456m, 123m},
			new[]{-123.456m, -123m},
			new[]{-123.0000000m, -123m},
			new[]{-9999999999.9999999999m, -9999999999m}
		};
#pragma warning restore 169

		#endregion

		#region Round

		[TestCaseSource("roundToNearestInt")]
		public void RoundToNearestInt_Spec(decimal amount, decimal roundedAmount)
		{
			var subject = new Money(amount, CurrencyIsoCode.XTS);

			Money rounded = subject.RoundToNearestInt();
			Assert.That(rounded, Is.Not.SameAs(subject));
			Assert.That(rounded.CurrencyCode, Is.EqualTo(CurrencyIsoCode.XTS));
			Assert.That(rounded.Amount, Is.EqualTo(roundedAmount));
		}

#pragma warning disable 169
		private static readonly TestCaseData[] roundToNearestInt = new[]
		{
			new TestCaseData(twoThirds, 1m),
			new TestCaseData(0.5m, 0m).SetName("the closest even number is 0"),
			new TestCaseData(1.5m, 2m).SetName("the closest even number is 2"),
			new TestCaseData(1.499999m, 1m).SetName("the closest number is 1")
		};
#pragma warning restore 169

		[TestCaseSource("roundToNearestInt_WithMode")]
		public void RoundToNearestInt_WithMode_Spec(decimal amount, MidpointRounding mode, decimal roundedAmount)
		{
			var subject = new Money(amount, CurrencyIsoCode.XTS);

			Money rounded = subject.RoundToNearestInt(mode);
			Assert.That(rounded, Is.Not.SameAs(subject));
			Assert.That(rounded.CurrencyCode, Is.EqualTo(CurrencyIsoCode.XTS));
			Assert.That(rounded.Amount, Is.EqualTo(roundedAmount));
		}

#pragma warning disable 169
		private static readonly TestCaseData[] roundToNearestInt_WithMode = new[]
		{
			new TestCaseData(twoThirds, MidpointRounding.ToEven, 1m),
			new TestCaseData(twoThirds, MidpointRounding.AwayFromZero, 1m),
			new TestCaseData(0.5m, MidpointRounding.ToEven, 0m).SetName("the closest even number is 0"),
			new TestCaseData(0.5m, MidpointRounding.AwayFromZero, 1m).SetName("the closest number away from zero is 1"),
			new TestCaseData(1.5m, MidpointRounding.ToEven, 2m).SetName("the closest number is 2"),
			new TestCaseData(1.5m, MidpointRounding.AwayFromZero, 2m),
			new TestCaseData(1.499999m, MidpointRounding.ToEven, 1m).SetName("the closest number is 1"),
			new TestCaseData(1.499999m, MidpointRounding.AwayFromZero, 1m)
		};
#pragma warning restore 169

		[TestCaseSource("round")]
		public void Round_Spec(CurrencyIsoCode currency, decimal amount, decimal roundedAmount)
		{
			var subject = new Money(amount, currency);

			Money rounded = subject.Round();
			Assert.That(rounded, Is.Not.SameAs(subject));
			Assert.That(rounded.CurrencyCode, Is.EqualTo(currency));
			Assert.That(rounded.Amount, Is.EqualTo(roundedAmount));
		}

#pragma warning disable 169
		private static readonly TestCaseData[] round = new[]
		{
			new TestCaseData(CurrencyIsoCode.XTS, 2.345m, 2.34m).SetName("2 decimal places"),
			new TestCaseData(CurrencyIsoCode.USD, 2.345m, 2.34m).SetName("2 decimal places"),
			new TestCaseData(CurrencyIsoCode.JPY, 2.345m, 2m).SetName("0 decimal places"),
			
			new TestCaseData(CurrencyIsoCode.XTS, 2.355m, 2.36m).SetName("2 decimal places"),
			new TestCaseData(CurrencyIsoCode.USD, 2.355m, 2.36m).SetName("2 decimal places"),
			new TestCaseData(CurrencyIsoCode.JPY, 2.355m, 2m).SetName("0 decimal places"),
			
			new TestCaseData(CurrencyIsoCode.XTS, twoThirds, 0.67m).SetName("2 decimal places"),
			new TestCaseData(CurrencyIsoCode.USD, twoThirds, 0.67m).SetName("2 decimal places"),
			new TestCaseData(CurrencyIsoCode.JPY, twoThirds, 1m).SetName("0 decimal places"),
			
			new TestCaseData(CurrencyIsoCode.XTS, 0.5m, 0.50m).SetName("2 decimal places"),
			new TestCaseData(CurrencyIsoCode.USD, 0.5m, 0.50m).SetName("2 decimal places"),
			new TestCaseData(CurrencyIsoCode.JPY, 0.5m, 0m).SetName("0 decimal places"),
			
			new TestCaseData(CurrencyIsoCode.XTS, 1.5m, 1.50m).SetName("2 decimal places"),
			new TestCaseData(CurrencyIsoCode.USD, 1.5m, 1.50m).SetName("2 decimal places"),
			new TestCaseData(CurrencyIsoCode.JPY, 1.5m, 2m).SetName("0 decimal places"),
			
			new TestCaseData(CurrencyIsoCode.XTS, 1.499999m, 1.50m).SetName("2 decimal places"),
			new TestCaseData(CurrencyIsoCode.USD, 1.499999m, 1.50m).SetName("2 decimal places"),
			new TestCaseData(CurrencyIsoCode.JPY, 1.499999m, 1m).SetName("0 decimal places"),
		};
#pragma warning restore 169

		[TestCaseSource("round_WithMode")]
		public void Round_WithMode_Spec(CurrencyIsoCode currency, MidpointRounding mode, decimal amount, decimal roundedAmount)
		{
			var subject = new Money(amount, currency);

			Money rounded = subject.Round(mode);
			Assert.That(rounded, Is.Not.SameAs(subject));
			Assert.That(rounded.CurrencyCode, Is.EqualTo(currency));
			Assert.That(rounded.Amount, Is.EqualTo(roundedAmount));
		}

#pragma warning disable 169
		private static readonly object[] round_WithMode = new object[]
		{
		    new object[] {CurrencyIsoCode.XTS, MidpointRounding.ToEven, 2.345m, 2.34m},
		    new object[] {CurrencyIsoCode.XTS, MidpointRounding.AwayFromZero, 2.345m, 2.35m},
		    new object[] {CurrencyIsoCode.USD, MidpointRounding.ToEven, 2.345m, 2.34m},
		    new object[] {CurrencyIsoCode.USD, MidpointRounding.AwayFromZero, 2.345m, 2.35m},
		    new object[] {CurrencyIsoCode.JPY, MidpointRounding.ToEven, 2.345m, 2m},
		    new object[] {CurrencyIsoCode.JPY, MidpointRounding.AwayFromZero, 2.345m, 2m},

		    new object[] {CurrencyIsoCode.XTS, MidpointRounding.ToEven, 2.355m, 2.36m},
		    new object[] {CurrencyIsoCode.XTS, MidpointRounding.AwayFromZero, 2.355m, 2.36m},
		    new object[] {CurrencyIsoCode.USD, MidpointRounding.ToEven, 2.355m, 2.36m},
		    new object[] {CurrencyIsoCode.USD, MidpointRounding.AwayFromZero, 2.355m, 2.36m},
		    new object[] {CurrencyIsoCode.JPY, MidpointRounding.ToEven, 2.355m, 2m},
		    new object[] {CurrencyIsoCode.JPY, MidpointRounding.AwayFromZero, 2.355m, 2m},

		    new object[] {CurrencyIsoCode.XTS, MidpointRounding.ToEven, twoThirds, 0.67m},	
		    new object[] {CurrencyIsoCode.XTS, MidpointRounding.AwayFromZero, twoThirds, 0.67m},	
		    new object[] {CurrencyIsoCode.USD, MidpointRounding.ToEven, twoThirds, 0.67m},	
		    new object[] {CurrencyIsoCode.USD, MidpointRounding.AwayFromZero, twoThirds, 0.67m},	
		    new object[] {CurrencyIsoCode.JPY, MidpointRounding.ToEven, twoThirds, 1m},		
		    new object[] {CurrencyIsoCode.JPY, MidpointRounding.AwayFromZero, twoThirds, 1m},		

		    new object[] {CurrencyIsoCode.XTS, MidpointRounding.ToEven, 0.5m, 0.50m},		
		    new object[] {CurrencyIsoCode.XTS, MidpointRounding.AwayFromZero, 0.5m, 0.50m},		
		    new object[] {CurrencyIsoCode.USD, MidpointRounding.ToEven, 0.5m, 0.50m},		
		    new object[] {CurrencyIsoCode.USD, MidpointRounding.AwayFromZero, 0.5m, 0.50m},		
		    new object[] {CurrencyIsoCode.JPY, MidpointRounding.ToEven, 0.5m, 0m},			
		    new object[] {CurrencyIsoCode.JPY, MidpointRounding.AwayFromZero, 0.5m, 1m},			

		    new object[] {CurrencyIsoCode.XTS, MidpointRounding.ToEven, 1.5m, 1.50m},		
		    new object[] {CurrencyIsoCode.XTS, MidpointRounding.AwayFromZero, 1.5m, 1.50m},		
		    new object[] {CurrencyIsoCode.USD, MidpointRounding.ToEven, 1.5m, 1.50m},		
		    new object[] {CurrencyIsoCode.USD, MidpointRounding.AwayFromZero, 1.5m, 1.50m},		
		    new object[] {CurrencyIsoCode.JPY, MidpointRounding.ToEven, 1.5m, 2m},			
		    new object[] {CurrencyIsoCode.JPY, MidpointRounding.AwayFromZero, 1.5m, 2m},			

		    new object[] {CurrencyIsoCode.XTS, MidpointRounding.ToEven, 1.499999m, 1.50m},	
		    new object[] {CurrencyIsoCode.XTS, MidpointRounding.AwayFromZero, 1.499999m, 1.50m},	
		    new object[] {CurrencyIsoCode.USD, MidpointRounding.ToEven, 1.499999m, 1.50m},	
		    new object[] {CurrencyIsoCode.USD, MidpointRounding.AwayFromZero, 1.499999m, 1.50m},	
		    new object[] {CurrencyIsoCode.JPY, MidpointRounding.ToEven, 1.499999m, 1m},		
		    new object[] {CurrencyIsoCode.JPY, MidpointRounding.AwayFromZero, 1.499999m, 1m},		
		};
#pragma warning restore 169

		[TestCaseSource("round_WithDecimals")]
		public void Round_WithDecimals_Spec(int decimals, decimal amount, decimal roundedAmount)
		{
			var subject = new Money(amount, CurrencyIsoCode.XTS);

			Money rounded = subject.Round(decimals);
			Assert.That(rounded, Is.Not.SameAs(subject));
			Assert.That(rounded.CurrencyCode, Is.EqualTo(CurrencyIsoCode.XTS));
			Assert.That(rounded.Amount, Is.EqualTo(roundedAmount));
		}

#pragma warning disable 169
		private static readonly object[] round_WithDecimals = new object[]
		{
		    new object[] {2, 2.345m, 2.34m},
		    new object[] {4, 2.345m, 2.345m},
		    new object[] {0, 2.345m, 2m},

		    new object[] {2, 2.355m, 2.36m},
		    new object[] {4, 2.355m, 2.355m},
		    new object[] {0, 2.355m, 2m},

		    new object[] {2, twoThirds, 0.67m},
		    new object[] {3, twoThirds, 0.667m},
		    new object[] {0, twoThirds, 1m},

		    new object[] {2, 0.5m, 0.50m},
		    new object[] {3, 0.5m, 0.5m},
		    new object[] {0, 0.5m, 0m},

		    new object[] {2, 1.5m, 1.50m},
		    new object[] {3, 1.5m, 1.50m},
		    new object[] {0, 1.5m, 2m},

		    new object[] {2, 1.499999m, 1.50m},
		    new object[] {3, 1.499999m, 1.5m},
		    new object[] {0, 1.499999m, 1m},
		};
#pragma warning restore 169

		[TestCaseSource("round_WithDecimalsAndMode")]
		public void Round_WithDecimalsAndMode_Spec(int decimals, MidpointRounding mode, decimal amount, decimal roundedAmount)
		{
			var subject = new Money(amount, CurrencyIsoCode.XTS);

			Money rounded = subject.Round(decimals, mode);
			Assert.That(rounded, Is.Not.SameAs(subject));
			Assert.That(rounded.CurrencyCode, Is.EqualTo(CurrencyIsoCode.XTS));
			Assert.That(rounded.Amount, Is.EqualTo(roundedAmount));
		}

#pragma warning disable 169
		private static readonly object[] round_WithDecimalsAndMode = new object[]
		{
			new object[] {2, MidpointRounding.ToEven, 2.345m, 2.34m},
		    new object[] {2, MidpointRounding.AwayFromZero, 2.345m, 2.35m},
		    new object[] {3, MidpointRounding.ToEven, 2.345m, 2.345m},
		    new object[] {3, MidpointRounding.AwayFromZero, 2.345m, 2.345m},
		    new object[] {0, MidpointRounding.ToEven, 2.345m, 2m},
		    new object[] {0, MidpointRounding.AwayFromZero, 2.345m, 2m},

		    new object[] {2, MidpointRounding.ToEven, 2.355m, 2.36m},
		    new object[] {2, MidpointRounding.AwayFromZero, 2.355m, 2.36m},
		    new object[] {3, MidpointRounding.ToEven, 2.355m, 2.355m},
		    new object[] {3, MidpointRounding.AwayFromZero, 2.355m, 2.355m},
		    new object[] {0, MidpointRounding.ToEven, 2.355m, 2m},
		    new object[] {0, MidpointRounding.AwayFromZero, 2.355m, 2m},

		    new object[] {2, MidpointRounding.ToEven, twoThirds, 0.67m},	
		    new object[] {2, MidpointRounding.AwayFromZero, twoThirds, 0.67m},	
		    new object[] {3, MidpointRounding.ToEven, twoThirds, 0.667m},	
		    new object[] {3, MidpointRounding.AwayFromZero, twoThirds, 0.667m},	
		    new object[] {0, MidpointRounding.ToEven, twoThirds, 1m},		
		    new object[] {0, MidpointRounding.AwayFromZero, twoThirds, 1m},		

		    new object[] {2, MidpointRounding.ToEven, 0.5m, 0.50m},		
		    new object[] {2, MidpointRounding.AwayFromZero, 0.5m, 0.50m},		
		    new object[] {3, MidpointRounding.ToEven, 0.5m, 0.50m},		
		    new object[] {3, MidpointRounding.AwayFromZero, 0.5m, 0.50m},		
		    new object[] {0, MidpointRounding.ToEven, 0.5m, 0m},			
		    new object[] {0, MidpointRounding.AwayFromZero, 0.5m, 1m},			

		    new object[] {2, MidpointRounding.ToEven, 1.5m, 1.50m},		
		    new object[] {2, MidpointRounding.AwayFromZero, 1.5m, 1.50m},		
		    new object[] {3, MidpointRounding.ToEven, 1.5m, 1.50m},		
		    new object[] {3, MidpointRounding.AwayFromZero, 1.5m, 1.50m},		
		    new object[] {0, MidpointRounding.ToEven, 1.5m, 2m},			
		    new object[] {0, MidpointRounding.AwayFromZero, 1.5m, 2m},			

		    new object[] {2, MidpointRounding.ToEven, 1.499999m, 1.50m},	
		    new object[] {2, MidpointRounding.AwayFromZero, 1.499999m, 1.50m},	
		    new object[] {3, MidpointRounding.ToEven, 1.499999m, 1.50m},	
		    new object[] {3, MidpointRounding.AwayFromZero, 1.499999m, 1.50m},	
		    new object[] {0, MidpointRounding.ToEven, 1.499999m, 1m},		
		    new object[] {0, MidpointRounding.AwayFromZero, 1.499999m, 1m},		
		};
#pragma warning restore 169

		#endregion

		#region Floor

		[TestCaseSource("floor")]
		public void Floor_Spec(decimal amount, decimal flooredAmount)
		{
			var subject = new Money(amount, CurrencyIsoCode.XTS);

			Money truncated = subject.Floor();
			Assert.That(truncated.Amount, Is.EqualTo(flooredAmount));
			Assert.That(truncated.CurrencyCode, Is.EqualTo(CurrencyIsoCode.XTS));
		}

#pragma warning disable 169
		private static readonly object floor = new[]
		{
			new[] {0m, 0m},
			new[] {123.456m, 123m},
			new[] {-123.456m, -124m},
			new[] {-123.0000000m, -123m},
			new[] {-9999999999.9999999999m, -10000000000m}
		};
#pragma warning restore 169

		#endregion

		#region Perform

		[Test]
		public void Perform_Unary_UnaryOperationPerformed()
		{
			bool performed = false;
			Func<decimal, decimal> unary = d =>
											{
												performed = true;
												return d;
											};

			decimal amount = 2m;
			var subject = new Money(amount, CurrencyIsoCode.XTS);
			Money result = subject.Perform(unary);

			Assert.That(performed, Is.True);
			Assert.That(result, Is.Not.SameAs(subject));
			Assert.That(result.Amount, Is.EqualTo(amount));
			Assert.That(result.CurrencyCode, Is.EqualTo(CurrencyIsoCode.XTS));
		}

		[Test]
		public void Perform_BinarySameCurrency_BinaryOperationPerformed()
		{
			bool performed = false;
			Func<decimal, decimal, decimal> binary = (x, y) =>
														{
															performed = true;
															return x * y;
														};

			Money subject = new Money(2m, CurrencyIsoCode.XTS),
				  operand = new Money(3m, CurrencyIsoCode.XTS);

			Money result = subject.Perform(operand, binary);

			Assert.That(performed, Is.True);
			Assert.That(result, Is.Not.SameAs(subject).And.Not.SameAs(operand));
			Assert.That(result.Amount, Is.EqualTo(6m));
			Assert.That(result.CurrencyCode, Is.EqualTo(CurrencyIsoCode.XTS));
		}

		[Test]
		public void Perform_BinaryDifferentCurrency_Exception()
		{
			bool performed = false;
			Func<decimal, decimal, decimal> binary = (x, y) =>
														{
															performed = true;
															return x * y;
														};

			Money subject = new Money(2m, CurrencyIsoCode.XTS),
				  operand = new Money(2m, CurrencyIsoCode.XXX);

			Assert.That(() => subject.Perform(operand, binary), Throws.InstanceOf<DifferentCurrencyException>());
			Assert.That(performed, Is.False);
		}

		#endregion

		#region HasDecimals

		[Test]
		public void HasDecimals_Integer_False()
		{
			Assert.That(new Money(3m, Currency.Usd).HasDecimals, Is.False);
		}

		[Test]
		public void HasDecimals_NotInteger_False()
		{
			Assert.That(new Money(3.0000001m, Currency.Usd).HasDecimals, Is.True);
		}

		#endregion

		#endregion

		#region serialization

		[Test]
		public void CanBe_BinarySerialized()
		{
			Assert.That(new Money(3.757m), Must.Be.BinarySerializable<Money>(m => Is.EqualTo(m)));
		}

		[Test]
		public void BinarySerialization_ObsoleteCurrency_RaisesEvent()
		{
			Money obsolete = new Money(2m, "EEK");
			using (var serializer = new OneGoBinarySerializer<Money>())
			{
				serializer.Serialize(obsolete);
				Action deserializeObsolete = () => serializer.Deserialize();
				Assert.That(deserializeObsolete, Must.RaiseObsoleteEvent.Once());
			}
		}

		[Test]
		public void CanBe_XmlSerialized()
		{
			Assert.That(new Money(3.757m), Must.Be.XmlSerializable<Money>());
		}

		[Test]
		public void CanBe_XmlDeserializable()
		{
			string serializedMoney =
				"<money xmlns=\"urn:nmoneys\">" +
				"<amount>3.757</amount>" +
				"<currency><isoCode>XXX</isoCode></currency>" +
				"</money>";
			Assert.That(serializedMoney, Must.Be.XmlDeserializableInto(new Money(3.757m)));
		}

		[Test]
		public void XmlSerialization_ObsoleteCurrency_RaisesEvent()
		{
			Money obsolete = new Money(2m, "EEK");
			using (var serializer = new OneGoXmlSerializer<Money>())
			{
				serializer.Serialize(obsolete);
				Action deserializeObsolete = () => serializer.Deserialize();
				Assert.That(deserializeObsolete, Must.RaiseObsoleteEvent.Once());
			}
		}

		[Test]
		public void CanBe_XamlSerialized()
		{
			XamlSerializer serializer = new XamlSerializer();
			string xaml = serializer.Serialize(new Money(3.757m));
			Assert.DoesNotThrow(() => serializer.Deserialize<Money>(xaml));

			Assert.Inconclusive("Properties are not serialized, though.");
		}

		[Test]
		public void CanBe_DataContractSerialized()
		{
			Assert.That(new Money(3.757m), Must.Be.DataContractSerializable<Money>());
		}

		[Test]
		public void CanBe_DataContractDeserializable()
		{
			string serializedMoney =
				"<money xmlns=\"urn:nmoneys\">" +
				"<amount>3.757</amount>" +
				"<currency><isoCode>XXX</isoCode></currency>" +
				"</money>";
			Assert.That(serializedMoney, Must.Be.DataContractDeserializableInto(new Money(3.757m)));
		}

		[Test]
		public void DataContractSerialization_ObsoleteCurrency_RaisesEvent()
		{
			Money obsolete = new Money(2m, "EEK");
			using (var serializer = new OneGoDataContractSerializer<Money>())
			{
				serializer.Serialize(obsolete);
				Action deserializeObsolete = () => serializer.Deserialize();
				Assert.That(deserializeObsolete, Must.RaiseObsoleteEvent.Once());
			}
		}

		[Test]
		public void CannotBe_DataContractJsonSerialized()
		{
			Assert.That(new Money(3.757m), Must.Not.Be.DataContractJsonSerializable<Money>());
		}

		[Test]
		public void CanBe_JsonSerialized()
		{
			Assert.That(new Money(3.757m), Must.Be.JsonSerializable<Money>(m => Is.EqualTo(m)));
		}

		[Test]
		public void CanBe_JsonDeserializable()
		{
			string serializedMoney = "{\"amount\":3.757,\"currency\":{\"isoCode\":\"XXX\"}}";
			Assert.That(serializedMoney, Must.Be.JsonDeserializableInto(new Money(3.757m)));
		}

		[Test]
		public void JsonSerialization_ObsoleteCurrency_RaisesEvent()
		{
			Money obsolete = new Money(2m, "EEK");
			using (var serializer = new OneGoJsonSerializer<Money>())
			{
				serializer.Serialize(obsolete);
				Action deserializeObsolete = () => serializer.Deserialize();
				Assert.That(deserializeObsolete, Must.RaiseObsoleteEvent.Once());
			}
		}

		#endregion

		#region non Zero-Based enumeration behaviors

		[Test]
		public void CurrencyDependentMethod_CanBeUsedWithDefaultInstance()
		{
			Money @default = new Money();

			Assert.That(() => @default.ToString(), Throws.Nothing);
			Assert.That(@default.CurrencyCode, Is.EqualTo(CurrencyIsoCode.XXX));
		}

		[Test]
		public void CannotBeCreated_WithDefaultCode_AsItIsUndefined()
		{
			Assert.That(() => new Money(2, default(CurrencyIsoCode)), Throws.InstanceOf<InvalidEnumArgumentException>());
		}

		[Test]
		public void DefaultInstances_HaveDefinedCode_InsteadOfUndefined()
		{
			Money? @null = null;

			Assert.That(@null.GetValueOrDefault().CurrencyCode, Is.EqualTo(CurrencyIsoCode.XXX));
		}

		[Test]
		public void Moneycontainer_CanHaveUnitializedMembers()
		{
			var p = new MoneyContainer();
			var someMoney = new Money(0m, Currency.Dkk);

			Assert.That(p.Money, Is.Not.EqualTo(someMoney));
		}

		public class MoneyContainer
		{
			public Money Money { get; set; }
		}

		[Test]
		public void BinarySerialization_OfDefaultInstance_StoresAndDeserializesNoCurrency()
		{
			var @default = new Money();

			var serializer = new OneGoBinarySerializer<Money>();
			serializer.Serialize(@default);

			Money deserialized = serializer.Deserialize();

			Assert.That(deserialized, Must.Be.MoneyWith(0m, Currency.Xxx));
		}

		[Test]
		public void XmlSerialization_OfDefaultInstance_StoresAndDeserializesNoCurrency()
		{
			var @default = new Money();

			var serializer = new OneGoXmlSerializer<Money>();
			serializer.Serialize(@default);

			Money deserialized = serializer.Deserialize();

			Assert.That(deserialized, Must.Be.MoneyWith(0m, Currency.Xxx));
		}

		[Test]
		public void DatacontractSerialization_OfDefaultInstance_StoresAndDeserializesNoCurrency()
		{
			var @default = new Money();

			var serializer = new OneGoDataContractSerializer<Money>();
			serializer.Serialize(@default);

			Money deserialized = serializer.Deserialize();

			Assert.That(deserialized, Must.Be.MoneyWith(0m, Currency.Xxx));
		}

		[Test]
		public void JsonSerialization_OfDefaultInstance_StoresAndDeserializesNoCurrency()
		{
			var @default = new Money();

			var serializer = new OneGoJsonSerializer<Money>();
			serializer.Serialize(@default);

			Money deserialized = serializer.Deserialize();

			Assert.That(deserialized, Must.Be.MoneyWith(0m, Currency.Xxx));
		}

		#endregion

		[Test, Explicit("WARNING: THIS TEST MIGH FAIL. Rerun the test to solve it.")]
		public void ExploratoryTesting_OnPerformance()
		{
			Stopwatch watch = new Stopwatch();
			ActionTimer.Time(watch, () =>
			{
				var c = new WithoutEnsure().NonZero;
			},
			1000000);
			long withoutEnsure = watch.ElapsedTicks;
			Debug.WriteLine("withoutEnsure: " + withoutEnsure);

			watch.Reset();
			ActionTimer.Time(watch, () =>
			{
				var c = new WithEnsure().NonZero;
			},
			1000000);
			long withEnsure = watch.ElapsedTicks;
			Debug.WriteLine("withEnsure: " + withEnsure);

			watch.Reset();
			ActionTimer.Time(watch, () =>
			{
				var c = new WithNullable().NonZero;
			}, 1000000);
			long withNullable = watch.ElapsedTicks;
			Debug.WriteLine("withNullable: " + withNullable);

			Assert.That(withoutEnsure, Is.LessThan(withEnsure).And.LessThan(withNullable),
				"doing nothing is the fastest, but it does allow a undefined value in the enumeration.");
			Assert.That(withEnsure, Is.GreaterThan(withNullable),
				"having a custom method to ensure the defined value is more expensive than using a nullable private field");
		}

		private enum NonZero
		{
			One = 1,
			Two = 2
		}

		private struct WithoutEnsure
		{
			public WithoutEnsure(decimal number, NonZero nonZero)
				: this()
			{
				Number = number;
				NonZero = nonZero;
			}

			public NonZero NonZero { get; private set; }

			public decimal Number { get; private set; }
		}

		private struct WithEnsure
		{
			public WithEnsure(decimal number, NonZero nonZero)
				: this()
			{
				Number = number;
				NonZero = nonZero;
			}

			private NonZero _nonZero;
			public NonZero NonZero
			{
				get
				{
					ensureNoDefault();
					return _nonZero;
				}
				private set { _nonZero = value; }
			}

			private void ensureNoDefault()
			{
				if (_nonZero.Equals(default(NonZero)))
				{
					_nonZero = NonZero.One;
				}
			}

			private decimal Number { get; set; }
		}

		private struct WithNullable
		{
			public WithNullable(decimal number, NonZero nonZero)
				: this()
			{
				Number = number;
				NonZero = nonZero;
			}

			private NonZero? _nonZero;
			public NonZero NonZero { get { return _nonZero.GetValueOrDefault(NonZero.One); } private set { _nonZero = value; } }

			public decimal Number { get; private set; }

			public decimal Amount { get; private set; }
		}

		#region Issue 16. Case sensitivity. Money instances can be obtained by any casing of the IsoCode (Alphbetic code)

		[Test]
		public void ctor_IsoCode_IsCaseInsensitive()
		{
			Money upper = new Money(100, "EUR");
			Money mixed = new Money(100, "eUr");

			Assert.That(upper, Is.EqualTo(mixed));
		}

		[Test]
		public void XmlDeserialization_IsCaseInsensitive()
		{
			string serializedMoney =
				"<money xmlns=\"urn:nmoneys\">" +
				"<amount>3.757</amount>" +
				"<currency><isoCode>xXx</isoCode></currency>" +
				"</money>";
			Assert.That(serializedMoney, Must.Be.XmlDeserializableInto(new Money(3.757m)));
		}

		[Test]
		public void DataContractDeserialization_IsCaseInsensitive()
		{
			string serializedMoney =
				"<money xmlns=\"urn:nmoneys\">" +
				"<amount>3.757</amount>" +
				"<currency><isoCode>xXx</isoCode></currency>" +
				"</money>";
			Assert.That(serializedMoney, Must.Be.DataContractDeserializableInto(new Money(3.757m)));
		}

		[Test]
		public void JsonDeserialization_IsCaseInsensitive()
		{
			string serializedMoney = "{\"amount\":3.757,\"currency\":{\"isoCode\":\"xXx\"}}";
			Assert.That(serializedMoney, Must.Be.JsonDeserializableInto(new Money(3.757m)));
		}

		#endregion

		[Test]
		public void OperatingOnStrings_IsPossible_IfCurrenciesAreKnownUpfront()
		{
			Money poundQuantity = Money.Parse("£100.50", Currency.Gbp);
			Money yenQuantity = Money.Parse("¥ 1,000", Currency.Jpy);

			Func<decimal, decimal> halfDiscount = q => q * .5m;

			Assert.That(poundQuantity.Perform(halfDiscount), Must.Be.MoneyWith(50.25m, Currency.Gbp));
			Assert.That(yenQuantity.Perform(halfDiscount), Must.Be.MoneyWith(500, Currency.Jpy));
		}
	}
}
