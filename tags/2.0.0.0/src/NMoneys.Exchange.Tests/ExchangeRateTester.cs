using System;
using System.Collections;
using System.ComponentModel;
using NMoneys.Extensions;
using NMoneys.Tests.CustomConstraints;
using NUnit.Framework;

namespace NMoneys.Exchange.Tests
{
	[TestFixture]
	public class ExchangeRateTester
	{
		#region ctor

		[Test]
		public void Ctor_SetsProperties()
		{
			var subject = new ExchangeRate(CurrencyIsoCode.EUR, CurrencyIsoCode.USD, 1.25m);

			Assert.That(subject.From, Is.EqualTo(CurrencyIsoCode.EUR));
			Assert.That(subject.To, Is.EqualTo(CurrencyIsoCode.USD));
			Assert.That(subject.Rate, Is.EqualTo(1.25m));
		}

		[Test]
		public void Ctor_NegativeRate_Exception()
		{
			Assert.That(() => new ExchangeRate(CurrencyIsoCode.XXX, CurrencyIsoCode.XTS, -0.1m), Throws.ArgumentException);
		}

		[TestCaseSource("undefinedCurrencies")]
		public void Ctor_UndefinedCurrency_Exception(CurrencyIsoCode from, CurrencyIsoCode to)
		{
			Assert.That(() => new ExchangeRate(from, to, 1m), Throws.InstanceOf<InvalidEnumArgumentException>());
		}

		protected IEnumerable undefinedCurrencies
		{
			get
			{
				yield return new[] { CurrencyIsoCode.USD, ((CurrencyIsoCode)1) };
				yield return new[] { ((CurrencyIsoCode)1), CurrencyIsoCode.USD };
				yield return new[] { ((CurrencyIsoCode)1), ((CurrencyIsoCode)1) };
			}
		}

		#endregion

		[Test]
		public void Apply_CompatibleInstance_CorrectAmountAndCurrency()
		{
			var subject = new ExchangeRate(CurrencyIsoCode.EUR, CurrencyIsoCode.USD, 1.25m);
			Money compatible = 10m.Eur();

			Assert.That(subject.Apply(compatible), Must.Be.MoneyWith(12.5m, Currency.Usd));
		}

		[Test]
		public void Apply_IncompatibleInstance_Exception()
		{
			var subject = new ExchangeRate(CurrencyIsoCode.EUR, CurrencyIsoCode.USD, 1.25m);
			Money incompatible = 10m.Cad();

			Assert.That(() => subject.Apply(incompatible), Throws
				.InstanceOf<DifferentCurrencyException>()
				.With.Message.StringContaining("EUR")
				.And.Message.StringContaining("CAD"));
		}

		[Test]
		public void Invert_SwappedCurrencies_AndInvertedRate()
		{
			var subject = new ExchangeRate(CurrencyIsoCode.EUR, CurrencyIsoCode.USD, 1.25m);
			ExchangeRate inverted = subject.Invert();

			Assert.That(inverted.From, Is.EqualTo(CurrencyIsoCode.USD));
			Assert.That(inverted.To, Is.EqualTo(CurrencyIsoCode.EUR));
			Assert.That(inverted.Rate, Is.EqualTo(0.8m));
		}

		[Test]
		public void Invert_IsPure()
		{
			var subject = new ExchangeRate(CurrencyIsoCode.EUR, CurrencyIsoCode.USD, 1.25m);
			ExchangeRate inverted = subject.Invert();

			Assert.That(inverted, Is.Not.SameAs(subject));

			Assert.That(subject.From, Is.EqualTo(CurrencyIsoCode.EUR));
			Assert.That(subject.To, Is.EqualTo(CurrencyIsoCode.USD));
			Assert.That(subject.Rate, Is.EqualTo(1.25m));
		}

		[Test]
		public void ToString_ConvertsToStringRepresentation()
		{
			var subject = new ExchangeRate(CurrencyIsoCode.EUR, CurrencyIsoCode.USD, 1.25m);

			Assert.That(subject.ToString(), Is.EqualTo("EUR/USD 1.25"));
		}

		[Test, SetCulture("es-ES")]
		public void ToString_CultureIndependant()
		{
			var subject = new ExchangeRate(CurrencyIsoCode.EUR, CurrencyIsoCode.USD, 1.25m);

			Assert.That(subject.ToString(), Is.EqualTo("EUR/USD 1.25"), "invariant number representation");
			Assert.That(1.25m.ToString(), Is.EqualTo("1,25"), "even decimals use commas in spanish");
		}

		#region equality

		readonly ExchangeRate fiberUnit = new ExchangeRate(CurrencyIsoCode.EUR, CurrencyIsoCode.USD, decimal.One),
			anotherFiberUnit = new ExchangeRate(CurrencyIsoCode.EUR, CurrencyIsoCode.USD, decimal.One),
			fundsUnit = new ExchangeRate(CurrencyIsoCode.USD, CurrencyIsoCode.CAD, decimal.One),
			fiberDouble = new ExchangeRate(CurrencyIsoCode.EUR, CurrencyIsoCode.USD, 2m);

		[Test]
		public void Equality_SameCurrenciesAndRate_True()
		{

			Assert.That(fiberUnit.Equals(fiberUnit), Is.True);
			Assert.That(fiberUnit.Equals(anotherFiberUnit), Is.True);
			Assert.That(anotherFiberUnit.Equals(fiberUnit), Is.True);
			Assert.That(fiberUnit == anotherFiberUnit, Is.True);
			Assert.That(anotherFiberUnit == fiberUnit, Is.True);
		}

		[Test]
		public void Equality_DifferentRateOrCurrencies_False()
		{
			Assert.That(fiberUnit.Equals(fundsUnit), Is.False);
			Assert.That(fundsUnit.Equals(fiberUnit), Is.False);
			Assert.That(fiberUnit == fundsUnit, Is.False);
			Assert.That(fundsUnit == fiberUnit, Is.False);

			Assert.That(fiberUnit.Equals(fiberDouble), Is.False);
			Assert.That(fiberDouble.Equals(fiberUnit), Is.False);
			Assert.That(fiberUnit == fiberDouble, Is.False);
			Assert.That(fiberDouble == fiberUnit, Is.False);
		}

		[Test]
		public void Equality_DifferentTypes()
		{
			Assert.That(fiberUnit.Equals("asd"), Is.False);
			Assert.That("asd".Equals(fiberUnit), Is.False);
			Assert.That(fiberUnit.Equals(5), Is.False);
			Assert.That(5.Equals(fiberUnit), Is.False);
		}

		[Test]
		public void Inequality_SameCurrenciesAndRate_False()
		{
			Assert.That(fiberUnit != anotherFiberUnit, Is.False);
			Assert.That(anotherFiberUnit != fiberUnit, Is.False);
		}

		[Test]
		public void Inequality_DifferentCurrenciesOrRate_True()
		{
			Assert.That(fiberUnit != fundsUnit, Is.True);
			Assert.That(fundsUnit != fiberUnit, Is.True);

			Assert.That(fiberUnit != fiberDouble, Is.True);
			Assert.That(fiberDouble != fiberUnit, Is.True);
		}

		#endregion

		#region Parse

		[Test]
		public void Parse_InverseOf_ToString()
		{
			var subject = new ExchangeRate(CurrencyIsoCode.EUR, CurrencyIsoCode.USD, 1.25m);

			Assert.That(ExchangeRate.Parse(subject.ToString()), Is.EqualTo(subject));
		}

		[Test]
		public void Parse_CaseInsensitiveCurrencies()
		{
			ExchangeRate parsed = ExchangeRate.Parse("eur/USd 1.25");

			Assert.That(parsed.From, Is.EqualTo(CurrencyIsoCode.EUR));
			Assert.That(parsed.To, Is.EqualTo(CurrencyIsoCode.USD));
			Assert.That(parsed.Rate, Is.EqualTo(1.25m));
		}

		[TestCase("ZZZ/USD 1")]
		[TestCase("USD/ZZZ 1")]
		public void Parse_UndefinedCurrencies_Exception(string undefinedCurrency)
		{
			Assert.That(() => ExchangeRate.Parse(undefinedCurrency), Throws.InstanceOf<FormatException>());
		}

		[Test]
		public void Parse_NotARate_Exception()
		{
			Assert.That(() => ExchangeRate.Parse("USD/EUR notARate"), Throws.InstanceOf<FormatException>());
		}

		[Test]
		public void Parse_NegativeRate_Exception()
		{
			Assert.That(() => ExchangeRate.Parse("USD/EUR -1"), Throws.InstanceOf<FormatException>());
		}

		[Test]
		public void Parse_DifferentCurrencySeparator_NotImportant()
		{
			ExchangeRate parsed = ExchangeRate.Parse("EUR-USD 1.25");

			Assert.That(parsed.From, Is.EqualTo(CurrencyIsoCode.EUR));
			Assert.That(parsed.To, Is.EqualTo(CurrencyIsoCode.USD));
			Assert.That(parsed.Rate, Is.EqualTo(1.25m));
		}

		[Test]
		public void Parse_MissingRate_Exception()
		{
			Assert.That(() => ExchangeRate.Parse("USD/EUR  "), Throws.InstanceOf<FormatException>());
		}

		#endregion

		#region ExchangeRate extensibility points

		class ExtendsApply : ExchangeRate
		{
			public ExtendsApply(CurrencyIsoCode from, CurrencyIsoCode to, decimal rate) : base(from, to, rate) { }

			// truncate to significant digits
			public override Money Apply(Money from)
			{
				return base.Apply(from).TruncateToSignificantDecimalDigits();
			}
		}

		[Test]
		public void ExchangeRate_CanBeExtendedTo_SupportCustomRateArithmetic()
		{
			var stock = new ExchangeRate(CurrencyIsoCode.EUR, CurrencyIsoCode.USD, 1.11111m);
			var truncating = new ExtendsApply(CurrencyIsoCode.EUR, CurrencyIsoCode.USD, 1.11111m);

			Assert.That(stock.Apply(1m.Eur()), Is.Not.EqualTo(truncating.Apply(1m.Eur())));
		}

		class ExtendsInvert : ExchangeRate
		{
			public ExtendsInvert(CurrencyIsoCode from, CurrencyIsoCode to, decimal rate) : base(from, to, rate) { }

			// levaes things as they are
			public override ExchangeRate Invert()
			{
				return this;
			}
		}

		[Test]
		public void ExchangeRate_CanBeExtendedTo_SupportCustomInversions()
		{
			var stock = new ExchangeRate(CurrencyIsoCode.EUR, CurrencyIsoCode.USD, 1.11111m);
			var noOp = new ExtendsInvert(CurrencyIsoCode.EUR, CurrencyIsoCode.USD, 1.11111m);

			Assert.That(stock.Invert(), Is.Not.EqualTo(noOp.Invert()));
		}

		#endregion

		[Test]
		public void Identity_TwoCurrencies_BuildsAnIdentityRate()
		{
			var identity = ExchangeRate.Identity(CurrencyIsoCode.EUR, CurrencyIsoCode.USD);

			Assert.That(identity.From, Is.EqualTo(CurrencyIsoCode.EUR));
			Assert.That(identity.To, Is.EqualTo(CurrencyIsoCode.USD));
			Assert.That(identity.Rate, Is.EqualTo(1m));
		}

		[Test]
		public void Identity_SingleCurrency_BuildsAnIdentityRateForItself()
		{
			var identity = ExchangeRate.Identity(CurrencyIsoCode.EUR);

			Assert.That(identity.From, Is.EqualTo(CurrencyIsoCode.EUR));
			Assert.That(identity.To, Is.EqualTo(CurrencyIsoCode.EUR));
			Assert.That(identity.Rate, Is.EqualTo(1m));
		}
	}
}
