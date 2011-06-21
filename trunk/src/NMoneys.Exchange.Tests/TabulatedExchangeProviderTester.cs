using NUnit.Framework;

namespace NMoneys.Exchange.Tests
{
	[TestFixture]
	public class TabulatedExchangeProviderTester_DirectBuild_DefaultConversionArithmetic
	{
		private TabulatedExchangeRateProvider as20110519;

		[TestFixtureSetUp]
		public void initSubject()
		{
			as20110519 = new TabulatedExchangeRateProvider();
			as20110519.Add(CurrencyIsoCode.USD, CurrencyIsoCode.USD, 1m);
			as20110519.Add(CurrencyIsoCode.USD, CurrencyIsoCode.EUR, 0.70155m);
			as20110519.Add(CurrencyIsoCode.USD, CurrencyIsoCode.GBP, 0.61860m);

			as20110519.Add(CurrencyIsoCode.EUR, CurrencyIsoCode.EUR, 1m);
			as20110519.Add(CurrencyIsoCode.EUR, CurrencyIsoCode.USD, 1.42542m);
			as20110519.Add(CurrencyIsoCode.EUR, CurrencyIsoCode.GBP, 0.88176m);

			as20110519.Add(CurrencyIsoCode.GBP, CurrencyIsoCode.GBP, 1m);
			as20110519.Add(CurrencyIsoCode.GBP, CurrencyIsoCode.EUR, 1.13409m);
			as20110519.Add(CurrencyIsoCode.GBP, CurrencyIsoCode.USD, 1.61656m);
		}

		[TestCaseSource(typeof(ComplexProviderData), "fromEUR")]
		public void Get_FromEUR_CorrectRate(CurrencyIsoCode to, decimal rate)
		{
			ExchangeRate fromEuro = as20110519.Get(CurrencyIsoCode.EUR, to);

			Assert.That(fromEuro.From, Is.EqualTo(CurrencyIsoCode.EUR));
			Assert.That(fromEuro.To, Is.EqualTo(to));
			Assert.That(fromEuro.Rate, Is.EqualTo(rate), "no error margin is needed");
		}

		[TestCaseSource(typeof(ComplexProviderData), "fromEURInverse")]
		public void Get_FromEURInverse_CorrectRate(CurrencyIsoCode to, decimal rate)
		{
			ExchangeRate fromEuro = as20110519.Get(CurrencyIsoCode.EUR, to);
			ExchangeRate inversed = fromEuro.Invert();

			Assert.That(inversed.From, Is.EqualTo(to));
			Assert.That(inversed.To, Is.EqualTo(CurrencyIsoCode.EUR));
			Assert.That(inversed.Rate, Is.EqualTo(rate)
				.Within(0.00001m), "within 5 decimals error as Invert() uses decimal arithmetic");
		}

		[TestCaseSource(typeof(ComplexProviderData), "toEUR")]
		public void Get_ToEUR_CorrectRate(CurrencyIsoCode from, decimal rate)
		{
			ExchangeRate toEuro = as20110519.Get(from, CurrencyIsoCode.EUR);

			Assert.That(toEuro.From, Is.EqualTo(from));
			Assert.That(toEuro.To, Is.EqualTo(CurrencyIsoCode.EUR));
			Assert.That(toEuro.Rate, Is.EqualTo(rate));
		}

		[TestCaseSource(typeof(ComplexProviderData), "toEURInverse")]
		public void Get_ToEURInverse_CorrectRate(CurrencyIsoCode from, decimal rate)
		{
			ExchangeRate toEuro = as20110519.Get(from, CurrencyIsoCode.EUR);
			ExchangeRate inversed = toEuro.Invert();

			Assert.That(inversed.From, Is.EqualTo(CurrencyIsoCode.EUR));
			Assert.That(inversed.To, Is.EqualTo(from));
			Assert.That(inversed.Rate, Is.EqualTo(rate)
				.Within(0.00001m), "within 5 decimals error as Invert() used decimal arithmetic");
		}
	}

	[TestFixture]
	public class TabulatedExchangeProviderTester_SimplerBuild_DefaultConversionArithmetic
	{
		private TabulatedExchangeRateProvider as20110519;

		[TestFixtureSetUp]
		public void initSubject()
		{
			as20110519 = new TabulatedExchangeRateProvider();
			as20110519.MultiAdd(CurrencyIsoCode.USD, CurrencyIsoCode.EUR, 0.70155m);
			as20110519.MultiAdd(CurrencyIsoCode.USD, CurrencyIsoCode.GBP, 0.61860m);
			as20110519.MultiAdd(CurrencyIsoCode.GBP, CurrencyIsoCode.EUR, 1.13409m);
		}

		[TestCaseSource(typeof(ComplexProviderData), "fromEUR")]
		public void Get_FromEUR_CorrectRate(CurrencyIsoCode to, decimal rate)
		{
			ExchangeRate fromEuro = as20110519.Get(CurrencyIsoCode.EUR, to);

			Assert.That(fromEuro.From, Is.EqualTo(CurrencyIsoCode.EUR));
			Assert.That(fromEuro.To, Is.EqualTo(to));
			Assert.That(fromEuro.Rate, Is.EqualTo(rate)
				.Within(0.00001m), "as it was built from USD, EUR direct rates have more decimals");
		}

		[TestCaseSource(typeof(ComplexProviderData), "fromEURInverse")]
		public void Get_FromEURInverse_CorrectRate(CurrencyIsoCode to, decimal rate)
		{
			ExchangeRate fromEuro = as20110519.Get(CurrencyIsoCode.EUR, to);
			ExchangeRate inversed = fromEuro.Invert();

			Assert.That(inversed.From, Is.EqualTo(to));
			Assert.That(inversed.To, Is.EqualTo(CurrencyIsoCode.EUR));
			Assert.That(inversed.Rate, Is.EqualTo(rate)
				.Within(0.00001m), "within 5 decimals due to precision of Invert()");
		}

		[TestCaseSource(typeof(ComplexProviderData), "toEUR")]
		public void Get_ToEUR_CorrectRate(CurrencyIsoCode from, decimal rate)
		{
			ExchangeRate toEuro = as20110519.Get(from, CurrencyIsoCode.EUR);

			Assert.That(toEuro.From, Is.EqualTo(from));
			Assert.That(toEuro.To, Is.EqualTo(CurrencyIsoCode.EUR));
			Assert.That(toEuro.Rate, Is.EqualTo(rate));
		}

		[TestCaseSource(typeof(ComplexProviderData), "toEURInverse")]
		public void Get_ToEURInverse_CorrectRate(CurrencyIsoCode from, decimal rate)
		{
			ExchangeRate toEuro = as20110519.Get(from, CurrencyIsoCode.EUR);
			ExchangeRate inversed = toEuro.Invert();

			Assert.That(inversed.From, Is.EqualTo(CurrencyIsoCode.EUR));
			Assert.That(inversed.To, Is.EqualTo(from));
			Assert.That(inversed.Rate, Is.EqualTo(rate)
				.Within(0.00001m), "within 5 decimals due to precision of Invert()");
		}
	}

	[TestFixture]
	public class TabulatedExchangeProviderTester_DirectBuild_CustomConversionArithmetic
	{
		public class FiveDecimalsArithmetic : ExchangeRate
		{
			public FiveDecimalsArithmetic(CurrencyIsoCode from, CurrencyIsoCode to, decimal rate)
				: base(from, to, rate) { }

			public override ExchangeRate Invert()
			{
				return new FiveDecimalsArithmetic(To, From, decimal.Round(1m / Rate, 5));
			}

			public override Money Apply(Money from)
			{
				Money applied = base.Apply(from);
				return applied.Round(5);
			}
		}

		private TabulatedExchangeRateProvider as20110519;

		[TestFixtureSetUp]
		public void initSubject()
		{
			as20110519 = new TabulatedExchangeRateProvider((from, to, rate) => new FiveDecimalsArithmetic(from, to, rate));
			as20110519.Add(CurrencyIsoCode.USD, CurrencyIsoCode.USD, 1m);
			as20110519.Add(CurrencyIsoCode.USD, CurrencyIsoCode.EUR, 0.70155m);
			as20110519.Add(CurrencyIsoCode.USD, CurrencyIsoCode.GBP, 0.61860m);

			as20110519.Add(CurrencyIsoCode.EUR, CurrencyIsoCode.EUR, 1m);
			as20110519.Add(CurrencyIsoCode.EUR, CurrencyIsoCode.USD, 1.42542m);
			as20110519.Add(CurrencyIsoCode.EUR, CurrencyIsoCode.GBP, 0.88176m);

			as20110519.Add(CurrencyIsoCode.GBP, CurrencyIsoCode.GBP, 1m);
			as20110519.Add(CurrencyIsoCode.GBP, CurrencyIsoCode.EUR, 1.13409m);
			as20110519.Add(CurrencyIsoCode.GBP, CurrencyIsoCode.USD, 1.61656m);
		}

		[TestCaseSource(typeof(ComplexProviderData), "fromEUR")]
		public void Get_FromEUR_CorrectRate(CurrencyIsoCode to, decimal rate)
		{
			ExchangeRate fromEuro = as20110519.Get(CurrencyIsoCode.EUR, to);

			Assert.That(fromEuro.From, Is.EqualTo(CurrencyIsoCode.EUR));
			Assert.That(fromEuro.To, Is.EqualTo(to));
			Assert.That(fromEuro.Rate, Is.EqualTo(rate));
		}

		[TestCaseSource(typeof(ComplexProviderData), "fromEURInverse")]
		public void Get_FromEURInverse_CorrectRate(CurrencyIsoCode to, decimal rate)
		{
			ExchangeRate fromEuro = as20110519.Get(CurrencyIsoCode.EUR, to);
			ExchangeRate inversed = fromEuro.Invert();

			Assert.That(inversed.From, Is.EqualTo(to));
			Assert.That(inversed.To, Is.EqualTo(CurrencyIsoCode.EUR));
			Assert.That(inversed.Rate, Is.EqualTo(rate)
				.Within(0.00001),
				"for some reason, despite using 5-digits arithmetic 1.13409m is represented as 1.1341m so a error marging is needed");
		}

		[TestCaseSource(typeof(ComplexProviderData), "toEUR")]
		public void Get_ToEUR_CorrectRate(CurrencyIsoCode from, decimal rate)
		{
			ExchangeRate toEuro = as20110519.Get(from, CurrencyIsoCode.EUR);

			Assert.That(toEuro.From, Is.EqualTo(from));
			Assert.That(toEuro.To, Is.EqualTo(CurrencyIsoCode.EUR));
			Assert.That(toEuro.Rate, Is.EqualTo(rate));
		}

		[TestCaseSource(typeof(ComplexProviderData), "toEURInverse")]
		public void Get_ToEURInverse_CorrectRate(CurrencyIsoCode from, decimal rate)
		{
			ExchangeRate toEuro = as20110519.Get(from, CurrencyIsoCode.EUR);
			ExchangeRate inversed = toEuro.Invert();

			Assert.That(inversed.From, Is.EqualTo(CurrencyIsoCode.EUR));
			Assert.That(inversed.To, Is.EqualTo(from));
			Assert.That(inversed.Rate, Is.EqualTo(rate));
		}
	}
}
