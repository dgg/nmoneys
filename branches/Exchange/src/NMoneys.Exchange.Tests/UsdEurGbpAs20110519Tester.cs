using NUnit.Framework;

namespace NMoneys.Exchange.Tests
{
	[TestFixture]
	public class UsdEurGbpAs20110519Tester
	{
		private UsdEurGbpAs20110519 _subject;

		[TestFixtureSetUp]
		public void initSubject()
		{
			_subject = new UsdEurGbpAs20110519();
		}

		[TestCaseSource(typeof(ComplexProviderData), "fromEUR")]
		public void Get_FromEUR_CorrectRate(CurrencyIsoCode to, decimal rate)
		{
			ExchangeRate fromEuro = _subject.Get(CurrencyIsoCode.EUR, to);

			Assert.That(fromEuro.From, Is.EqualTo(CurrencyIsoCode.EUR));
			Assert.That(fromEuro.To, Is.EqualTo(to));
			Assert.That(fromEuro.Rate, Is.EqualTo(rate));
		}

		[TestCaseSource(typeof(ComplexProviderData), "fromEURInverse")]
		public void Get_FromEURInverse_CorrectRate(CurrencyIsoCode to, decimal rate)
		{
			ExchangeRate fromEuro = _subject.Get(CurrencyIsoCode.EUR, to);
			ExchangeRate inversed = fromEuro.Inverse();

			Assert.That(inversed.From, Is.EqualTo(to));
			Assert.That(inversed.To, Is.EqualTo(CurrencyIsoCode.EUR));
			Assert.That(inversed.Rate, Is.EqualTo(rate).Within(0.00001m), "within 5 decimals");
		}

		[TestCaseSource(typeof(ComplexProviderData), "toEUR")]
		public void Get_ToEUR_CorrectRate(CurrencyIsoCode from, decimal rate)
		{
			ExchangeRate toEuro = _subject.Get(from, CurrencyIsoCode.EUR);

			Assert.That(toEuro.From, Is.EqualTo(from));
			Assert.That(toEuro.To, Is.EqualTo(CurrencyIsoCode.EUR));
			Assert.That(toEuro.Rate, Is.EqualTo(rate));
		}

		[TestCaseSource(typeof(ComplexProviderData), "toEURInverse")]
		public void Get_ToEURInverse_CorrectRate(CurrencyIsoCode from, decimal rate)
		{
			ExchangeRate toEuro = _subject.Get(from, CurrencyIsoCode.EUR);
			ExchangeRate inversed = toEuro.Inverse();

			Assert.That(inversed.From, Is.EqualTo(CurrencyIsoCode.EUR));
			Assert.That(inversed.To, Is.EqualTo(from));
			Assert.That(inversed.Rate, Is.EqualTo(rate).Within(0.00001m), "within 5 decimals");
		}
	}

	[TestFixture]
	public class UsdEurGbpAs20110519_Using5DecimalsArithmeticTester
	{
		private UsdEurGbpAs20110519_Using5DecimalsArithmetic _subject;

		[TestFixtureSetUp]
		public void initSubject()
		{
			_subject = new UsdEurGbpAs20110519_Using5DecimalsArithmetic();
		}

		[TestCaseSource(typeof(ComplexProviderData), "fromEUR")]
		public void Get_FromEUR_CorrectRate(CurrencyIsoCode to, decimal rate)
		{
			ExchangeRate fromEuro = _subject.Get(CurrencyIsoCode.EUR, to);

			Assert.That(fromEuro.From, Is.EqualTo(CurrencyIsoCode.EUR));
			Assert.That(fromEuro.To, Is.EqualTo(to));
			Assert.That(fromEuro.Rate, Is.EqualTo(rate));
		}

		[TestCaseSource(typeof(ComplexProviderData), "fromEURInverse")]
		public void Get_FromEURInverse_CorrectRate(CurrencyIsoCode to, decimal rate)
		{
			ExchangeRate fromEuro = _subject.Get(CurrencyIsoCode.EUR, to);
			ExchangeRate inversed = fromEuro.Inverse();

			Assert.That(inversed.From, Is.EqualTo(to));
			Assert.That(inversed.To, Is.EqualTo(CurrencyIsoCode.EUR));
			Assert.That(inversed.Rate, Is.EqualTo(rate), "within 5 decimals");
		}

		[TestCaseSource(typeof(ComplexProviderData), "toEUR")]
		public void Get_ToEUR_CorrectRate(CurrencyIsoCode from, decimal rate)
		{
			ExchangeRate toEuro = _subject.Get(from, CurrencyIsoCode.EUR);

			Assert.That(toEuro.From, Is.EqualTo(from));
			Assert.That(toEuro.To, Is.EqualTo(CurrencyIsoCode.EUR));
			Assert.That(toEuro.Rate, Is.EqualTo(rate));
		}

		[TestCaseSource(typeof(ComplexProviderData), "toEURInverse")]
		public void Get_ToEURInverse_CorrectRate(CurrencyIsoCode from, decimal rate)
		{
			ExchangeRate toEuro = _subject.Get(from, CurrencyIsoCode.EUR);
			ExchangeRate inversed = toEuro.Inverse();

			Assert.That(inversed.From, Is.EqualTo(CurrencyIsoCode.EUR));
			Assert.That(inversed.To, Is.EqualTo(from));
			Assert.That(inversed.Rate, Is.EqualTo(rate).Within(0.00001m), "within 5 decimals");
		}
	}
}
