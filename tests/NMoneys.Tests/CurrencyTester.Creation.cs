namespace NMoneys.Tests;

[TestFixture]
public partial class CurrencyTester
{
	[Test]
	public void Get_UndefinedCurrency_Exception()
	{
		var notDefined = (CurrencyIsoCode)5000;
		Assert.That(() => Currency.Get(notDefined), Throws
			.InstanceOf<UndefinedCodeException>()
			.With.Message.Contains("5000")
		);
	}

	[Test]
	public void Initialize_AllCurrenciesGetInitialized()
	{
		Assert.DoesNotThrow(Currency.InitializeAllCurrencies);
	}

	[Test]
	public void NumericValue_HoldsTheValueOfTheCode()
	{
		Assert.That(Currency.Get("XXX").NumericCode, Is.EqualTo(999));
		Assert.That(Currency.Get("XTS").NumericCode, Is.EqualTo(963));
	}

	[Test]
	public void PaddedNumericValue_ThreeDigitedValue_NoLeadingZeros()
	{
		Assert.That(Currency.Get("XTS").PaddedNumericCode, Is.EqualTo("963"));
	}

	[Test]
	public void PaddedNumericValue_TwoDigitedValue_OneLeadingZero()
	{
		Assert.That(Currency.Get(CurrencyIsoCode.BZD).PaddedNumericCode, Is.EqualTo("084"));
	}

	[Test]
	public void PaddedNumericValue_OneDigitedValue_OneLeadingZero()
	{
		Assert.That(Currency.Get(CurrencyIsoCode.ALL).PaddedNumericCode, Is.EqualTo("008"));
	}
}
