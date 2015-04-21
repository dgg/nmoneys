using System.Globalization;
using NMoneys.Extensions;
using NUnit.Framework;

namespace NMoneys.Tests
{
	[TestFixture]
	public partial class MoneyTester
	{
		[Test]
		public void ToString_Default_CurrencyFormat()
		{
			var subject = new Money(5, CurrencyIsoCode.GBP);
			Assert.That(subject.ToString(), Is.EqualTo("£5.00"));

			subject = new Money(5, CurrencyIsoCode.DKK);
			Assert.That(subject.ToString(), Is.EqualTo("kr 5,00"), "Kroner symbol DOES not include dot. Mistake in Vista and newer.");

			subject = new Money(5, CurrencyIsoCode.USD);
			Assert.That(subject.ToString(), Is.EqualTo("$5.00"));

			subject = new Money(5, CurrencyIsoCode.EUR);
			Assert.That(subject.ToString(), Is.EqualTo("5,00 €"));
		}

		[Test]
		public void ToString_WithCustomFormat_CurrencyDependentFormat()
		{
			var subject = new Money(1000, Currency.Gbp);
			Assert.That(subject.ToString("C"), Is.EqualTo("£1,000.00"));
			Assert.That(subject.ToString("N"), Is.EqualTo("1,000.00"));

			subject = new Money(1000, CurrencyIsoCode.DKK);
			Assert.That(subject.ToString("C"), Is.EqualTo("kr 1.000,00"), "Kroner symbol DOES not include dot. Mistake in Vista and newer.");
			Assert.That(subject.ToString("00.000"), Is.EqualTo("1000,000"));
		}

		[Test]
		public void ToString_CulturesWithSameCurrencyAndDifferentFormat_FormatHonored()
		{
			var subject = new Money(1000, CurrencyIsoCode.EUR);

			Assert.That(subject.ToString(), Is.EqualTo("1.000,00 €"));
			Assert.That(subject.ToString(CultureInfo.GetCultureInfo("fr-FR")),
				Is.Not.EqualTo(subject.ToString()),
				"Germany has different format than France");
		}

		[Test]
		public void ToString_CultureWithDifferentCurrency_FormatWithNewCulture()
		{
			var subject = new Money(1000, CurrencyIsoCode.EUR);

			Assert.That(subject.ToString(CultureInfo.GetCultureInfo("en-GB")), Is.EqualTo("£1,000.00"));
		}

		[Test]
		public void Format_PatternWith2Placeholders_RichFormatting()
		{
			var subject = new Money(5, CurrencyIsoCode.USD);

			Assert.That(subject.Format("{0} {1}"), Is.EqualTo("5 $"));
			Assert.That(subject.Format("{1} {0:00.00}"), Is.EqualTo("$ 05.00"));

			subject = new Money(1000, CurrencyIsoCode.SEK);
			Assert.That(subject.Format("{1}. {0:000}"), Is.EqualTo("kr. 1000"));
		}

		[Test]
		public void Format_PatternWith1PlaceHolder_CurrencySymbolIgnored()
		{
			var subject = new Money(5, CurrencyIsoCode.USD);

			Assert.That(subject.Format("{0}"), Is.EqualTo("5"));
			Assert.That(subject.Format("{0:00.00}"), Is.EqualTo("05.00"));

			subject = new Money(1000, CurrencyIsoCode.SEK);
			Assert.That(subject.Format(". {0:000}"), Is.EqualTo(". 1000"));
		}

		[Test]
		public void Format_SameCurrencyCulture_Richformatting()
		{
			var subject = new Money(1500, CurrencyIsoCode.EUR);
			string format = "{1} {0:#,#.00}";
			Assert.That(subject.Format(format), Is.EqualTo("€ 1.500,00"));
		}

		[Test]
		public void Format_CulturesWithSameCurrencyAndDifferentFormat_FormatHonored()
		{
			var subject = new Money(1500, CurrencyIsoCode.EUR);
			string format = "{1} {0:#,#.00}";

			Assert.That(subject.Format(format, CultureInfo.GetCultureInfo("fr-FR")),
				Is.Not.EqualTo(subject.Format(format)),
				"Germany has different format than France");
		}

		[Test]
		public void Format_DifferentCurrencyCulture_RichFormatting()
		{
			var subject = new Money(1500, CurrencyIsoCode.EUR);
			string format = "{1} {0:#,#.00}";
			Assert.That(subject.Format(format, CultureInfo.GetCultureInfo("en-US")), Is.EqualTo("€ 1,500.00"));
		}

		#region Issue 22. Default number formatting. Default does not include group separation

		[Test]
		public void Format_AmountWithoutFormatting_NumberWithoutSperators()
		{
			Assert.That(1500m.Dkk().Format("{0}"), Is.EqualTo("1500"));
		}

		[Test]
		public void Format_AmountWithCustomFormatting_NumberWithSperators()
		{
			Assert.That(1500m.Dkk().Format("{0:N}"), Is.EqualTo("1.500,00"));
		}

		#endregion

		#region Issue 29. Allow empty currency symbols

		[Test]
		public void ToString_SymbolLessCurrency_NoSymbol()
		{
			Money positive = new Money(12.25m, CurrencyIsoCode.CVE),
				negative = new Money(-12.25m, CurrencyIsoCode.CVE);

			Assert.That(positive.ToString(), Is.EqualTo("12$25"));
			Assert.That(negative.ToString(), Is.EqualTo("-12$25"));
		}

		#endregion

		#region change SEK formatting

		[Test]
		public void ToString_BigSEK_ThousandSeparatorIsNotDotAnymore()
		{
			var big = new Money(12345.67m, Currency.Sek);

			Assert.That(big.ToString(), Is.EqualTo("12 345,67 kr"));
		}

		#endregion
	}
}
