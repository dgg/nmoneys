using NUnit.Framework;

namespace NMoneys.Tests
{
	[TestFixture]
	public partial class CurrencyIsoCodeTester
	{
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
	}
}
