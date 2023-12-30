// ReSharper disable ObjectCreationAsStatement
using NMoneys.Tests.Support;

namespace NMoneys.Tests;

[TestFixture]
public partial class MoneyTester
{
	#region Ctor

		[Test]
		public void Ctor_Default_CurrencyIsNotDefault_ButUndefined()
		{
			Money @default = new Money();

			Assert.That(@default.CurrencyCode, Is.Not.EqualTo(default(CurrencyIsoCode)).And.EqualTo(CurrencyIsoCode.XXX));
		}

		[Test]
		public void Ctor_Amount_AmountWithNoneCurrency()
		{
			Money defaultMoney = new Money(3m);
			Assert.That(defaultMoney.Amount, Is.EqualTo(3m));
			Assert.That(defaultMoney.CurrencyCode, Is.EqualTo(CurrencyIsoCode.XXX));
		}

		[Test]
		public void Ctor_ExistingIsoCode_PropertiesSet()
		{
			Money tenDollars = new Money(10, CurrencyIsoCode.USD);
			Assert.That(tenDollars.Amount, Is.EqualTo(10m));
			Assert.That(tenDollars.CurrencyCode, Is.EqualTo(CurrencyIsoCode.USD));
		}

		[Test]
		public void Ctor_ObsoleteIsoCode_EventRaised()
		{
			CurrencyIsoCode obsolete = Currency.Code.Parse("EEK");
			// ReSharper disable once ObjectCreationAsStatement
			Action moneyWithObsoleteCurrency = () => new Money(10, obsolete);
			Assert.That(moneyWithObsoleteCurrency, Doez.Raise.ObsoleteEvent());
		}

		[Test]
		public void Ctor_ExistingIsoSymbol_PropertiesSet()
		{
			Money hundredLerus = new Money(100, "EUR");
			Assert.That(hundredLerus.Amount, Is.EqualTo(100m));
			Assert.That(hundredLerus.CurrencyCode, Is.EqualTo(CurrencyIsoCode.EUR));
		}

		[Test]
		public void Ctor_ObsoleteIsoSymbol_EventRaised()
		{
			Action moneyWithObsoleteCurrency = () => new Money(10, "EEK");
			Assert.That(moneyWithObsoleteCurrency, Doez.Raise.ObsoleteEvent());
		}

		[Test]
		public void Ctor_NullSymbol_Exception()
		{
			Assert.That(() => new Money(decimal.Zero, (string)null!), Throws.InstanceOf<ArgumentNullException>());
		}

		[Test]
		public void Ctor_ObsoleteCurrency_EventRaised()
		{
			Currency obsolete = Currency.Get("EEK");
			Action moneyWithObsoleteCurrency = () => new Money(10, obsolete);
			Assert.That(moneyWithObsoleteCurrency, Doez.Raise.ObsoleteEvent());
		}

		[Test]
		public void Ctor_Currency_PropertiesSet()
		{
			var tenner = new Money(10, CurrencyIsoCode.GBP);
			Assert.That(tenner.Amount, Is.EqualTo(10m));
			Assert.That(tenner.CurrencyCode, Is.EqualTo(CurrencyIsoCode.GBP));
		}

		[Test]
		public void Ctor_NullCurrency_Exception()
		{
			Assert.That(() => new Money(decimal.Zero, (Currency)null!), Throws.InstanceOf<ArgumentNullException>());
		}

		[Test]
		public void Ctor_NonExistingIsoCode_Exception()
		{
			CurrencyIsoCode nonExistingCode = (CurrencyIsoCode)(1);

			Assert.That(() => new Money(decimal.Zero, nonExistingCode), Throws.InstanceOf<UndefinedCodeException>().With.Message.Contains("1"));
		}

		[Test]
		public void Ctor_NonExistingIsoSymbol_PropertiesSet()
		{
			string nonExistentIsoSymbol = "XYZ";
			Assert.That(() => new Money(decimal.Zero, nonExistentIsoSymbol), Throws.ArgumentException);
		}

		#endregion
}
