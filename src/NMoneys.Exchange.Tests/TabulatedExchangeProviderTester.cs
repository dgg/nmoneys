using System.Collections.Generic;
using NUnit.Framework;

namespace NMoneys.Exchange.Tests
{

	[TestFixture]
	public class TabulatedExchangeRateProviderTester
	{
		[Test]
		public void Ctor_DefaultRateFactory_AddInstancesOfExchangeRate()
		{
			var subject = new TabulatedExchangeRateProvider();
			ExchangeRate added = subject.Add(CurrencyIsoCode.AED, CurrencyIsoCode.AFN, 1);

			Assert.That(added.GetType(), Is.SameAs(typeof(ExchangeRate)));
		}

		[Test]
		public void Ctor_CustomRateFactory_AddInstancesOfCustomExchangeRate()
		{
			var subject = new TabulatedExchangeRateProvider((from, to, rate) => new CustomExchangeRate(from, to, rate));
			ExchangeRate added = subject.Add(CurrencyIsoCode.AED, CurrencyIsoCode.AFN, 1);

			Assert.That(added.GetType(), Is.SameAs(typeof(CustomExchangeRate)));
		}

		class CustomExchangeRate : ExchangeRate
		{
			public CustomExchangeRate(CurrencyIsoCode from, CurrencyIsoCode to, decimal rate) : base(from, to, rate) { }
		}

		[Test]
		public void Add_AddsTheRate_WithTheData()
		{
			var subject = new TabulatedExchangeRateProvider();
			ExchangeRate added = subject.Add(CurrencyIsoCode.AED, CurrencyIsoCode.AFN, 1.5m);

			Assert.That(added.From, Is.EqualTo(CurrencyIsoCode.AED));
			Assert.That(added.To, Is.EqualTo(CurrencyIsoCode.AFN));
			Assert.That(added.Rate, Is.EqualTo(1.5m));
		}

		[Test]
		public void Get_ExistingRate_RateReturned()
		{
			var subject = new TabulatedExchangeRateProvider();
			subject.Add(CurrencyIsoCode.AED, CurrencyIsoCode.AFN, 1.5m);

			ExchangeRate existing = subject.Get(CurrencyIsoCode.AED, CurrencyIsoCode.AFN);
			Assert.That(existing.From, Is.EqualTo(CurrencyIsoCode.AED));
			Assert.That(existing.To, Is.EqualTo(CurrencyIsoCode.AFN));
			Assert.That(existing.Rate, Is.EqualTo(1.5m));
		}

		[Test]
		public void Get_NonExistingRate_Exception()
		{
			var subject = new TabulatedExchangeRateProvider();
			subject.Add(CurrencyIsoCode.AED, CurrencyIsoCode.AFN, 1.5m);

			Assert.That(() => subject.Get(CurrencyIsoCode.USD, CurrencyIsoCode.AFN), Throws.InstanceOf<KeyNotFoundException>());
		}

		[Test]
		public void Add_ExistingRates_AreOverwritten()
		{
			var subject = new TabulatedExchangeRateProvider();
			subject.Add(CurrencyIsoCode.AED, CurrencyIsoCode.AFN, 1.5m);
			subject.Add(CurrencyIsoCode.AED, CurrencyIsoCode.AFN, 2m);

			ExchangeRate overwritten = subject.Get(CurrencyIsoCode.AED, CurrencyIsoCode.AFN);
			Assert.That(overwritten.Rate, Is.EqualTo(2m));
		}

		[Test]
		public void Add_InverseRate_NotAdded()
		{
			var subject = new TabulatedExchangeRateProvider();
			subject.Add(CurrencyIsoCode.AED, CurrencyIsoCode.AFN, 1.5m);

			Assert.That(() => subject.Get(CurrencyIsoCode.AFN, CurrencyIsoCode.AED), Throws.InstanceOf<KeyNotFoundException>());
		}

		[Test]
		public void Add_IdentityRatesForBaseCurrency_NotAdded()
		{
			var subject = new TabulatedExchangeRateProvider();
			subject.Add(CurrencyIsoCode.AED, CurrencyIsoCode.AFN, 1.5m);

			Assert.That(() => subject.Get(CurrencyIsoCode.AED, CurrencyIsoCode.AED), Throws.InstanceOf<KeyNotFoundException>());
		}

		[Test]
		public void Add_IdentityRatesForQuoteCurrency_NotAdded()
		{
			var subject = new TabulatedExchangeRateProvider();
			subject.Add(CurrencyIsoCode.AED, CurrencyIsoCode.AFN, 1.5m);

			Assert.That(() => subject.Get(CurrencyIsoCode.AFN, CurrencyIsoCode.AFN), Throws.InstanceOf<KeyNotFoundException>());
		}

		[Test]
		public void TryGet_ExistingRate_RateReturned()
		{
			var subject = new TabulatedExchangeRateProvider();
			subject.Add(CurrencyIsoCode.AED, CurrencyIsoCode.AFN, 1.5m);

			ExchangeRate existing;
			Assert.That(subject.TryGet(CurrencyIsoCode.AED, CurrencyIsoCode.AFN, out existing), Is.True);
			Assert.That(existing.From, Is.EqualTo(CurrencyIsoCode.AED));
			Assert.That(existing.To, Is.EqualTo(CurrencyIsoCode.AFN));
			Assert.That(existing.Rate, Is.EqualTo(1.5m));
		}

		[Test]
		public void TryGet_NonExistingRate_NoException()
		{
			var subject = new TabulatedExchangeRateProvider();

			ExchangeRate existing = new ExchangeRate(CurrencyIsoCode.AED, CurrencyIsoCode.AED, 1);

			Assert.That(subject.TryGet(CurrencyIsoCode.AED, CurrencyIsoCode.AFN, out existing), Is.False);
			Assert.That(existing, Is.Null);
		}

		[Test]
		public void MultiAdd_ExistingRates_AreOverwritten()
		{
			var subject = new TabulatedExchangeRateProvider();
			subject.Add(CurrencyIsoCode.AED, CurrencyIsoCode.AFN, 1.5m);
			subject.MultiAdd(CurrencyIsoCode.AED, CurrencyIsoCode.AFN, 2m);

			ExchangeRate overwritten = subject.Get(CurrencyIsoCode.AED, CurrencyIsoCode.AFN);
			Assert.That(overwritten.Rate, Is.EqualTo(2m));
		}

		[Test]
		public void MultiAdd_ExistingInverseRates_AreOverwritten()
		{
			var subject = new TabulatedExchangeRateProvider();
			subject.Add(CurrencyIsoCode.AFN, CurrencyIsoCode.AED, 1.5m);
			subject.MultiAdd(CurrencyIsoCode.AED, CurrencyIsoCode.AFN, 2m);

			ExchangeRate overwritten = subject.Get(CurrencyIsoCode.AFN, CurrencyIsoCode.AED);
			Assert.That(overwritten.Rate, Is.EqualTo(.5m));
		}

		[Test]
		public void MultiAdd_InverseRate_Added()
		{
			var subject = new TabulatedExchangeRateProvider();
			subject.MultiAdd(CurrencyIsoCode.AED, CurrencyIsoCode.AFN, 1.5m);

			var inverse = subject.Get(CurrencyIsoCode.AFN, CurrencyIsoCode.AED);
			Assert.That(inverse.From, Is.EqualTo(CurrencyIsoCode.AFN));
			Assert.That(inverse.To, Is.EqualTo(CurrencyIsoCode.AED));
			Assert.That(inverse.Rate, Is.EqualTo(0.66m).Within(0.0067m));
		}

		[Test]
		public void MultiAdd_IdentityRatesForBaseCurrency_Added()
		{
			var subject = new TabulatedExchangeRateProvider();
			subject.MultiAdd(CurrencyIsoCode.AED, CurrencyIsoCode.AFN, 1.5m);

			var identity = subject.Get(CurrencyIsoCode.AED, CurrencyIsoCode.AED);
			Assert.That(identity.From, Is.EqualTo(CurrencyIsoCode.AED));
			Assert.That(identity.To, Is.EqualTo(CurrencyIsoCode.AED));
			Assert.That(identity.Rate, Is.EqualTo(1m));
		}

		[Test]
		public void MultiAdd_IdentityRatesForQuoteCurrency_Added()
		{
			var subject = new TabulatedExchangeRateProvider();
			subject.MultiAdd(CurrencyIsoCode.AED, CurrencyIsoCode.AFN, 1.5m);

			var identity = subject.Get(CurrencyIsoCode.AFN, CurrencyIsoCode.AFN);
			Assert.That(identity.From, Is.EqualTo(CurrencyIsoCode.AFN));
			Assert.That(identity.To, Is.EqualTo(CurrencyIsoCode.AFN));
			Assert.That(identity.Rate, Is.EqualTo(1m));
		}
	}


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
