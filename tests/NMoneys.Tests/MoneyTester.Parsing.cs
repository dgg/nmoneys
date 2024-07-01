using System.Globalization;
using NMoneys.Extensions;

namespace NMoneys.Tests;

using Iz = Support.Iz;

public partial class MoneyTester
{
	#region Parse

	#region quantity

	private static TestCaseData[] quantities = new[]
	{
		new TestCaseData("XTS 50", CurrencyIsoCode.XTS, 50m)
			.SetName("positive integral"),
		new TestCaseData("XXX -50", CurrencyIsoCode.XXX, -50m)
			.SetName("negative integral"),
		new TestCaseData("GBP 5.75", CurrencyIsoCode.GBP, 5.75m)
			.SetName("positive fractional"),
		new TestCaseData("EUR -5.765", CurrencyIsoCode.EUR, -5.765m)
			.SetName("negative fractional"),
		new TestCaseData("AUD 1.000000000000000001", CurrencyIsoCode.AUD, 1.000000000000000001m)
			.SetName("many decimals")
	};

	[Test, TestCaseSource(nameof(quantities))]
	public void Parse_Quantity_MoneyCreatedAsPerSpec(string quantity, CurrencyIsoCode currency, decimal amount)
	{
		Money parsed = Money.Parse(quantity);
		Assert.That(parsed, Is.EqualTo(new Money(amount, currency)));
	}

	[Test]
	public void Parse_ExponentialQuantity_MoneyParsed()
	{
		Money parsed = Money.Parse("USD 1E6");
		// cannot be written by .AsQuantity(), but can be parse
		Assert.That(parsed, Is.EqualTo(1000000m.Usd()));
	}

	[Test]
	public void Parse_ThousandsQuantity_Exception()
	{
		Assert.That(() => Money.Parse("XXX 1,000.1"), Throws.InstanceOf<FormatException>());
	}

	[Test]
	public void Parse_TrailingSignQuantity_Exception()
	{
		Assert.That(() => Money.Parse("USD 1-"), Throws.InstanceOf<FormatException>());
	}

	[Test]
	public void Parse_UndefinedCurrency_Exception()
	{
		Assert.That(() => Money.Parse("LOL 1"), Throws.InstanceOf<ArgumentException>());
	}

	[Test]
	public void Parse_MissingAmount_Exception()
	{
		Assert.That(() => Money.Parse("USD"), Throws.InstanceOf<ArgumentOutOfRangeException>());
	}

	#endregion

	[TestCaseSource(nameof(positiveAmounts))]
	public void Parse_CurrencyStyle_PositiveAmount_MoneyParsed(string positiveAmount, Currency currency, decimal amount)
	{
		Money parsed = Money.Parse(positiveAmount, currency);

		Assert.That(parsed, Iz.MoneyWith(amount, currency));
	}

	private static TestCaseData[] positiveAmounts = new[]
	{
		new TestCaseData("$1.5", Currency.Dollar, 1.5m).SetName("one-and-the-half dollars"),
		new TestCaseData("£5.75", Currency.Pound, 5.75m).SetName("five-and-three-quarters pounds"),
		new TestCaseData("10 €", Currency.Euro, 10m).SetName("ten euros"),
		new TestCaseData("kr. 100", Currency.Dkk, 100m).SetName("hundrede kroner"),
		new TestCaseData("¤1.2", Currency.None, 1.2m).SetName("one point two, no currency")
	};

	[TestCaseSource(nameof(negativeAmounts))]
	public void Parse_CurrencyStyle_NegativeAmount_MoneyParsed(string negativeAmount, Currency currency, decimal amount)
	{
		Money parsed = Money.Parse(negativeAmount, currency);

		Assert.That(parsed, Iz.MoneyWith(amount, currency));
	}

	private static TestCaseData[] negativeAmounts = new[]
	{
		new TestCaseData("($1.5)", Currency.Dollar, -1.5m).SetName("owe one-and-the-half dollars"),
		new TestCaseData("-£5.75", Currency.Pound, -5.75m).SetName("oew five-and-three-quarters pounds"),
		new TestCaseData("-10 €", Currency.Euro, -10m).SetName("owe ten euros"),
		new TestCaseData("kr. -100", Currency.Dkk, -100m).SetName("owe hundrede kroner"),
		new TestCaseData("(¤1.2)", Currency.None, -1.2m).SetName("owe one point two, no currency")
	};

	[Test]
	public void Parse_CurrencyStyle_MoreSignificantDecimalDigits_PrecisionMaintained()
	{
		Money parsed = Money.Parse("$1.5678", Currency.Usd);

		Assert.That(parsed, Iz.MoneyWith(1.5678m, Currency.Usd));
	}

	[Test]
	public void Parse_CurrencyStyle_CurrenciesWithNoDecimals_MaintainsAmountButNoRepresented()
	{
		Money moreThanOneYen = Money.Parse("¥1.49", Currency.Jpy);
		Assert.That(moreThanOneYen, Iz.MoneyWith(1.49m, Currency.Jpy));
		Assert.That(moreThanOneYen.ToString(), Is.EqualTo("¥1"));

		Money lessThanTwoYen = Money.Parse("¥1.5", Currency.Jpy);
		Assert.That(lessThanTwoYen, Iz.MoneyWith(1.5m, Currency.Jpy));
		Assert.That(lessThanTwoYen.ToString(), Is.EqualTo("¥2"));
	}

	[TestCaseSource(nameof(flexibleParsing))]
	public void Parse_CurrencyStyle_IsAsFlexibleAsParsingDecimals(string flexibleParse, Currency currency,
		decimal amount)
	{
		Money parsed = Money.Parse(flexibleParse, currency);

		Assert.That(parsed, Iz.MoneyWith(amount, currency));
	}

	private static TestCaseData[] flexibleParsing = new[]
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

		Assert.That(parsed, Iz.MoneyWith(3m, Currency.Nok));
	}

	[Test]
	public void Parse_WithStyle_AllowsHavingStandardSignedAmounts()
	{
		// in usd, negative amounts go between parenthesis, we can override that behavior
		Money parsed = Money.Parse("-73", NumberStyles.Number, Currency.Usd);

		Assert.That(parsed, Iz.MoneyWith(-73m, Currency.Usd));
	}

	[Test]
	public void Parse_WithStyle_IncorrectFormat_Exception()
	{
		Assert.That(() => Money.Parse("AF", NumberStyles.Integer, Currency.None), Throws.InstanceOf<FormatException>());
		Assert.That(() => Money.Parse("1e-1", NumberStyles.HexNumber, Currency.None),
			Throws.InstanceOf<ArgumentException>());
	}

	[Test, Combinatorial]
	public void Parse_CanParseAllWrittenCurrencies(
		[ValueSource(nameof(nonFractionalAmounts))]
		decimal amount,
		[ValueSource(nameof(allCurrencies))] Currency currency)
	{
		Money beforeWritting = new Money(amount, currency);
		Money afterParsing = Money.Parse(beforeWritting.ToString(), currency);

		Assert.That(beforeWritting, Is.EqualTo(afterParsing));
	}

	// only non-fractional amounts are chosen to not deal with rounding problems for currencies that do not support decimals
	private static decimal[] nonFractionalAmounts = { 123m, -123m };

	private static IEnumerable<Currency> allCurrencies = Currency.FindAll()
		// NPR makes parsing tests fail
		.Where(c => !Currency.Code.Comparer.Equals(c.IsoCode, CurrencyIsoCode.NPR));

	#endregion

	#region TryParse

	#region quantity

	[Test, TestCaseSource(nameof(quantities))]
	public void TryParse_Quantity_MoneyCreatedAsPerSpec(string quantity, CurrencyIsoCode currency, decimal amount)
	{
		Assert.That(Money.TryParse(quantity, out Money? parsed), Is.True);
		Assert.That(parsed, Is.EqualTo(new Money(amount, currency)));
	}

	[Test]
	public void TryParse_ExponentialQuantity_MoneyParsed()
	{
		// cannot be written by .AsQuantity(), but can be parsed
		Assert.That(Money.TryParse("USD 1E6", out Money? parsed), Is.True);
		Assert.That(parsed, Is.EqualTo(1000000m.Usd()));
	}

	[Test]
	public void TryParse_ThousandsQuantity_False()
	{
		Assert.That(Money.TryParse("XXX 1,000.1", out Money? parsed), Is.False);
		Assert.That(parsed, Is.Null);
	}

	[Test]
	public void TryParse_TrailingSignQuantity_False()
	{
		Assert.That(Money.TryParse("USD 1-", out Money? parsed), Is.False);
		Assert.That(parsed, Is.Null);
	}

	[Test]
	public void TryParse_UndefinedCurrency_Exception()
	{
		Assert.That(Money.TryParse("LOL 1", out Money? parsed), Is.False);
		Assert.That(parsed, Is.Null);
	}

	[Test]
	public void Parse_MissingAmount_False()
	{
		Assert.That(Money.TryParse("USD", out Money? parsed), Is.False);
		Assert.That(parsed, Is.Null);
	}

	#endregion

	[TestCaseSource(nameof(positiveAmounts))]
	public void TryParse_CurrencyStyle_PositiveAmount_MoneyParsed(string positiveAmount, Currency currency,
		decimal amount)
	{
		Assert.That(Money.TryParse(positiveAmount, currency, out Money? parsed), Is.True);
		Assert.That(parsed, Iz.MoneyWith(amount, currency));
	}

	[TestCaseSource(nameof(negativeAmounts))]
	public void TryParse_CurrencyStyle_NegativeAmount_MoneyParsed(string negativeAmount, Currency currency,
		decimal amount)
	{
		Assert.That(Money.TryParse(negativeAmount, currency, out Money? parsed), Is.True);
		Assert.That(parsed, Iz.MoneyWith(amount, currency));
	}

	[Test]
	public void TryParse_CurrencyStyle_MoreSignificantDecimalDigits_PrecisionMaintained()
	{
		Assert.That(Money.TryParse("$1.5678", Currency.Usd, out Money? parsed), Is.True);
		Assert.That(parsed, Iz.MoneyWith(1.5678m, Currency.Usd));
	}

	[Test]
	public void TryParse_CurrencyStyle_CurrenciesWithNoDecimals_MaintainsAmountButNoRepresented()
	{
		Money.TryParse("¥1.49", Currency.Jpy, out Money? moreThanOneYen);
		Assert.That(moreThanOneYen, Iz.MoneyWith(1.49m, Currency.Jpy));
		Assert.That(moreThanOneYen.ToString(), Is.EqualTo("¥1"));

		Money.TryParse("¥1.5", Currency.Jpy, out Money? lessThanTwoYen);
		Assert.That(lessThanTwoYen, Iz.MoneyWith(1.5m, Currency.Jpy));
		Assert.That(lessThanTwoYen.ToString(), Is.EqualTo("¥2"));
	}

	[TestCaseSource(nameof(flexibleParsing))]
	public void TryParse_CurrencyStyle_IsAsFlexibleAsParsingDecimals(string flexibleParse, Currency currency,
		decimal amount)
	{
		Assert.That(Money.TryParse(flexibleParse, currency, out Money? parsed), Is.True);
		Assert.That(parsed, Iz.MoneyWith(amount, currency));
	}

	[Test]
	public void TryParse_Default_IncorrectFormat_False()
	{
		Assert.That(Money.TryParse("not a Number", Currency.None, out Money? notParsed), Is.False);
		Assert.That(notParsed, Is.Null);
		Assert.That(Money.TryParse("1--", Currency.None, out notParsed), Is.False);
		Assert.That(notParsed, Is.Null);
	}

	[Test]
	public void TryParse_WithStyle_AllowsIgnoringCurrencySymbols()
	{
		Assert.That(Money.TryParse("3", NumberStyles.Number, Currency.Nok, out Money? parsed), Is.True);
		Assert.That(parsed, Iz.MoneyWith(3m, Currency.Nok));
	}

	[Test]
	public void TryParse_WithStyle_AllowsHavingStandardSignedAmounts()
	{
		// in usd, negative amounts go between parenthesis, we can override that behavior
		Assert.That(Money.TryParse("-73", NumberStyles.Number, Currency.Usd, out Money? parsed), Is.True);
		Assert.That(parsed, Iz.MoneyWith(-73m, Currency.Usd));
	}

	[Test]
	public void TryParse_WithStyle_IncorrectFormat_False()
	{
		Assert.That(Money.TryParse("¤1.4", NumberStyles.Integer, Currency.None, out Money? notParsed), Is.False);
		Assert.That(notParsed, Is.Null);
		Assert.That(Money.TryParse("1e-1", NumberStyles.Currency, Currency.None, out Money? notParsedEither), Is.False);
		Assert.That(notParsedEither, Is.Null);
	}

	[Test, Combinatorial]
	public void TryParse_CanParseAllWrittenCurrencies(
		[ValueSource(nameof(nonFractionalAmounts))]
		decimal amount,
		[ValueSource(nameof(allCurrencies))] Currency currency)
	{
		Money beforeWritting = new Money(amount, currency);
		Money.TryParse(beforeWritting.ToString(), currency, out Money? afterParsing);

		Assert.That(beforeWritting, Is.EqualTo(afterParsing));
	}

	#region Issue 28. Support detachment of parsing logic from currency

	[Test]
	public void TryParse_ParsingDetachedFromCurrency_FullControlOverParsing()
	{
		// currency does the parsing and the money construction: dot is not a decimal separator
		Money.TryParse("€1000.00", Currency.Eur, out Money? parsed);
		Assert.That(parsed, Is.EqualTo(100000m.Eur()));

		// NumberFormatInfo used for parsing, currency for building the money instance
		NumberFormatInfo parser = CultureInfo.GetCultureInfo("en-GB").NumberFormat;
		Money.TryParse("€1000.00", NumberStyles.Currency, parser, Currency.Eur, out parsed);
		Assert.That(parsed, Is.EqualTo(1000m.Eur()));
	}

	#endregion

	#endregion
}
