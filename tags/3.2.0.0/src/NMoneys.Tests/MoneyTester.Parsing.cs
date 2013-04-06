using System;
using System.Collections.Generic;
using System.Globalization;
using NMoneys.Tests.CustomConstraints;
using NUnit.Framework;

namespace NMoneys.Tests
{
	[TestFixture]
	public partial class MoneyTester
	{
		#region Parse

		[TestCaseSource("positiveAmounts")]
		public void Parse_CurrencyStyle_PositiveAmount_MoneyParsed(string positiveAmount, Currency currency, decimal amount)
		{
			Money parsed = Money.Parse(positiveAmount, currency);

			Assert.That(parsed, Must.Be.MoneyWith(amount, currency));
		}

		internal static TestCaseData[] positiveAmounts = new[]
		{
			new TestCaseData("$1.5", Currency.Dollar, 1.5m).SetName("one-and-the-half dollars"),
			new TestCaseData("£5.75", Currency.Pound, 5.75m).SetName("five-and-three-quarters pounds"),
			new TestCaseData("10 €", Currency.Euro, 10m).SetName("ten euros"),
			new TestCaseData("kr 100", Currency.Dkk, 100m).SetName("hundrede kroner"),
			new TestCaseData("¤1.2", Currency.None, 1.2m).SetName("one point two, no currency")
		};

		[TestCaseSource("negativeAmounts")]
		public void Parse_CurrencyStyle_NegativeAmount_MoneyParsed(string negativeAmount, Currency currency, decimal amount)
		{
			Money parsed = Money.Parse(negativeAmount, currency);

			Assert.That(parsed, Must.Be.MoneyWith(amount, currency));
		}

		internal static TestCaseData[] negativeAmounts = new[]
		{
			new TestCaseData("($1.5)", Currency.Dollar, -1.5m).SetName("owe one-and-the-half dollars"),
			new TestCaseData("-£5.75", Currency.Pound, -5.75m).SetName("oew five-and-three-quarters pounds"),
			new TestCaseData("-10 €", Currency.Euro, -10m).SetName("owe ten euros"),
			new TestCaseData("kr -100", Currency.Dkk, -100m).SetName("owe hundrede kroner"),
			new TestCaseData("(¤1.2)", Currency.None, -1.2m).SetName("owe one point two, no currency")
		};

		[Test]
		public void Parse_CurrencyStyle_MoreSignificantDecimalDigits_PrecisionMaintained()
		{
			Money parsed = Money.Parse("$1.5678", Currency.Usd);

			Assert.That(parsed, Must.Be.MoneyWith(1.5678m, Currency.Usd));
		}

		[Test]
		public void Parse_CurrencyStyle_CurrenciesWithNoDecimals_MaintainsAmountButNoRepresented()
		{
			Money moreThanOneYen = Money.Parse("¥1.49", Currency.Jpy);
			Assert.That(moreThanOneYen, Must.Be.MoneyWith(1.49m, Currency.Jpy));
			Assert.That(moreThanOneYen.ToString(), Is.EqualTo("¥1"));

			Money lessThanTwoYen = Money.Parse("¥1.5", Currency.Jpy);
			Assert.That(lessThanTwoYen, Must.Be.MoneyWith(1.5m, Currency.Jpy));
			Assert.That(lessThanTwoYen.ToString(), Is.EqualTo("¥2"));
		}

		[TestCaseSource("flexibleParsing")]
		public void Parse_CurrencyStyle_IsAsFlexibleAsParsingDecimals(string flexibleParse, Currency currency, decimal amount)
		{
			Money parsed = Money.Parse(flexibleParse, currency);

			Assert.That(parsed, Must.Be.MoneyWith(amount, currency));
		}

		internal static TestCaseData[] flexibleParsing = new[]
		{
			new TestCaseData("(€1.5)", Currency.Euro, -15m).SetName("dollar style"),
			new TestCaseData("-€5,75", Currency.Euro, -5.75m).SetName("pound style"),
			new TestCaseData("-10", Currency.Euro, -10m).SetName("negative euro style without no symbol"),
			new TestCaseData("€ -100", Currency.Euro, -100m).SetName("negative danish style"),
			new TestCaseData("(1.2)", Currency.Euro, -12m).SetName("negative dollar style with no symbol")
		};

		[Test]
		public void Parse_Default_IncorrectFormat_Exception()
		{
			Assert.That(() => Money.Parse("not a Number", Currency.None), Throws.InstanceOf<FormatException>());
			Assert.That(() => Money.Parse("1--", Currency.None), Throws.InstanceOf<FormatException>());
		}

		[Test]
		public void Parse_WithStyle_AllowsIgnoringCurrencySymbols()
		{
			Money parsed = Money.Parse("3", NumberStyles.Number, Currency.Nok);

			Assert.That(parsed, Must.Be.MoneyWith(3m, Currency.Nok));
		}

		[Test]
		public void Parse_WithStyle_AllowsHavingStardardSignedAmounts()
		{
			// in usd, negative amounts go between parenthesis, we can override that behavior
			Money parsed = Money.Parse("-73", NumberStyles.Number, Currency.Usd);

			Assert.That(parsed, Must.Be.MoneyWith(-73m, Currency.Usd));
		}

		[Test]
		public void Parse_WithStyle_IncorrectFormat_Exception()
		{
			Assert.That(() => Money.Parse("AF", NumberStyles.Integer, Currency.None), Throws.InstanceOf<FormatException>());
			Assert.That(() => Money.Parse("1e-1", NumberStyles.HexNumber, Currency.None), Throws.InstanceOf<ArgumentException>());

		}

		[Test, Combinatorial]
		public void Parse_CanParseAllWrittenCurrencies(
			[ValueSource("nonFractionalAmounts")]decimal amount,
			[ValueSource("allCurrencies")]Currency currency)
		{
			Money beforeWritting = new Money(amount, currency);
			Money afterParsing = Money.Parse(beforeWritting.ToString(), currency);

			Assert.That(beforeWritting, Is.EqualTo(afterParsing));
		}

		// only non-fractional amounts are chosen to not deal with ronding problems for currencies that do not support decimals
		internal static decimal[] nonFractionalAmounts = new[] { 123m, -123m };

		internal static IEnumerable<Currency> allCurrencies = Currency.FindAll();

		#endregion

		#region TryParse

		[TestCaseSource("positiveAmounts")]
		public void TryParse_CurrencyStyle_PositiveAmount_MoneyParsed(string positiveAmount, Currency currency, decimal amount)
		{
			Money? parsed;
			Assert.That(Money.TryParse(positiveAmount, currency, out parsed), Is.True);
			Assert.That(parsed, Must.Be.MoneyWith(amount, currency));
		}

		[TestCaseSource("negativeAmounts")]
		public void TryParse_CurrencyStyle_NegativeAmount_MoneyParsed(string negativeAmount, Currency currency, decimal amount)
		{
			Money? parsed;
			Assert.That(Money.TryParse(negativeAmount, currency, out parsed), Is.True);
			Assert.That(parsed, Must.Be.MoneyWith(amount, currency));
		}

		[Test]
		public void TryParse_CurrencyStyle_MoreSignificantDecimalDigits_PrecisionMaintained()
		{
			Money? parsed;
			Assert.That(Money.TryParse("$1.5678", Currency.Usd, out parsed), Is.True);
			Assert.That(parsed, Must.Be.MoneyWith(1.5678m, Currency.Usd));
		}

		[Test]
		public void TryParse_CurrencyStyle_CurrenciesWithNoDecimals_MaintainsAmountButNoRepresented()
		{
			Money? moreThanOneYen;

			Money.TryParse("¥1.49", Currency.Jpy, out moreThanOneYen);
			Assert.That(moreThanOneYen, Must.Be.MoneyWith(1.49m, Currency.Jpy));
			Assert.That(moreThanOneYen.ToString(), Is.EqualTo("¥1"));

			Money? lessThanTwoYen;
			Money.TryParse("¥1.5", Currency.Jpy, out lessThanTwoYen);
			Assert.That(lessThanTwoYen, Must.Be.MoneyWith(1.5m, Currency.Jpy));
			Assert.That(lessThanTwoYen.ToString(), Is.EqualTo("¥2"));
		}

		[TestCaseSource("flexibleParsing")]
		public void TryParse_CurrencyStyle_IsAsFlexibleAsParsingDecimals(string flexibleParse, Currency currency, decimal amount)
		{
			Money? parsed;
			Assert.That(Money.TryParse(flexibleParse, currency, out parsed), Is.True);
			Assert.That(parsed, Must.Be.MoneyWith(amount, currency));
		}

		[Test]
		public void TryParse_Default_IncorrectFormat_False()
		{
			Money? notParsed;
			Assert.That(Money.TryParse("not a Number", Currency.None, out notParsed), Is.False);
			Assert.That(notParsed, Is.Null);
			Assert.That(Money.TryParse("1--", Currency.None, out notParsed), Is.False);
			Assert.That(notParsed, Is.Null);
		}

		[Test]
		public void TryParse_WithStyle_AllowsIgnoringCurrencySymbols()
		{
			Money? parsed;

			Assert.That(Money.TryParse("3", NumberStyles.Number, Currency.Nok, out parsed), Is.True);
			Assert.That(parsed, Must.Be.MoneyWith(3m, Currency.Nok));
		}

		[Test]
		public void TryParse_WithStyle_AllowsHavingStardardSignedAmounts()
		{
			// in usd, negative amounts go between parenthesis, we can override that behavior
			Money? parsed;

			Assert.That(Money.TryParse("-73", NumberStyles.Number, Currency.Usd, out parsed), Is.True);
			Assert.That(parsed, Must.Be.MoneyWith(-73m, Currency.Usd));
		}

		[Test]
		public void TryParse_WithStyle_IncorrectFormat_False()
		{
			Money? notParsed;
			Assert.That(Money.TryParse("¤1.4", NumberStyles.Integer, Currency.None, out notParsed), Is.False);
			Assert.That(notParsed, Is.Null);
			Assert.That(Money.TryParse("1e-1", NumberStyles.Currency, Currency.None, out notParsed), Is.False);
			Assert.That(notParsed, Is.Null);
		}

		[Test, Combinatorial]
		public void TryParse_CanParseAllWrittenCurrencies(
			[ValueSource("nonFractionalAmounts")]decimal amount,
			[ValueSource("allCurrencies")]Currency currency)
		{
			Money beforeWritting = new Money(amount, currency);
			Money? afterParsing;
			Money.TryParse(beforeWritting.ToString(), currency, out afterParsing);

			Assert.That(beforeWritting, Is.EqualTo(afterParsing));
		}

		#endregion
	}
}