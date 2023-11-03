namespace NMoneys.Tests;

[TestFixture]
public class CurrencyIsoCodeTester
{
	#region extensions

	[Test]
	public void NumericCode_HoldsTheValueOfTheCode()
	{
		Assert.That(CurrencyIsoCode.USD.NumericCode(), Is.EqualTo(840));
		Assert.That(CurrencyIsoCode.EUR.NumericCode(), Is.EqualTo(978));
	}

	[Test]
	public void PaddedNumericCode_ThreeDigitedValue_NoLeadingZeros()
	{
		Assert.That(CurrencyIsoCode.USD.PaddedNumericCode(), Is.EqualTo("840"));
	}

	[Test]
	public void PaddedNumericCode_TwoDigitedValue_OneLeadingZero()
	{
		Assert.That(CurrencyIsoCode.BZD.PaddedNumericCode(), Is.EqualTo("084"));
	}

	[Test]
	public void PaddedNumericCode_OneDigitedValue_OneLeadingZero()
	{
		Assert.That(CurrencyIsoCode.ALL.PaddedNumericCode(), Is.EqualTo("008"));
	}

	[Test]
	public void AlphabeticCode_HoldsTheNameOfTheCode()
	{
		Assert.That(CurrencyIsoCode.USD.AlphabeticCode(), Is.EqualTo("USD"));
		Assert.That(CurrencyIsoCode.EUR.AlphabeticCode(), Is.EqualTo("EUR"));
	}

	[Test]
	public void AsValuePair_StringRepresentation()
	{
		Assert.That(CurrencyIsoCode.USD.AsValuePair(), Is.EqualTo("USD = 840"));
		Assert.That(CurrencyIsoCode.EUR.AsValuePair(), Is.EqualTo("EUR = 978"));
	}

	[Test]
	public void AsCurrency_GetsTheCorrespondingCurrency()
	{
		Assert.That(CurrencyIsoCode.USD.AsCurrency(), Is.SameAs(Currency.Usd));
	}

	[Test]
	public void Equals_ProvidesFastComparison()
	{
		Assert.That(CurrencyIsoCode.USD.Equals(CurrencyIsoCode.EUR), Is.False);
	}

	[Test]
	public void AssertDefined_Defined_NoException()
	{
		Assert.DoesNotThrow(()=> CurrencyIsoCode.EUR.AssertDefined());
	}

	[Test]
	public void AssertDefined_NotDefined_Exception()
	{
		CurrencyIsoCode notDefined = (CurrencyIsoCode)1;
		Assert.That(()=> notDefined.AssertDefined(), Throws.InstanceOf<UndefinedCodeException>());
	}
	[Test]
	public void CheckDefined_Defined_True()
	{
		Assert.That(CurrencyIsoCode.EUR.CheckDefined(), Is.True);
	}

	[Test]
	public void CheckDefined_NotDefined_Exception()
	{
		CurrencyIsoCode notDefined = (CurrencyIsoCode)1;
		Assert.That(notDefined.CheckDefined(), Is.False);
	}

	#endregion

	[Test]
	public void AllCurrencies_Decorated()
	{
		Assert.Fail();
	}
}
