using System;
using System.ComponentModel;
using NMoneys.Tests.CustomConstraints;
using NUnit.Framework;

namespace NMoneys.Tests
{
	[TestFixture]
	public partial class MoneyTester
	{
		[Test]
		public void Zero_NoneCurrency()
		{
			Assert.That(Money.Zero(), Must.Be.MoneyWith(decimal.Zero, Currency.None));
		}

		[Test]
		public void Zero_ExistingIsoCode_PropertiesSet()
		{
			Assert.That(Money.Zero(CurrencyIsoCode.USD), Must.Be.MoneyWith(decimal.Zero, Currency.Usd));
		}

		[Test]
		public void Zero_Array_ExistingIsoCode_CollectionOfZeroes()
		{
			Assert.That(Money.Zero(CurrencyIsoCode.USD, 3), Has.Length.EqualTo(3).And
				.All.Matches(Must.Be.MoneyWith(decimal.Zero, Currency.Usd)));
		}

		[Test]
		public void Zero_ExistingIsoSymbol_PropertiesSet()
		{
			Assert.That(Money.Zero("EUR"), Must.Be.MoneyWith(decimal.Zero, Currency.Euro));
		}

		[Test]
		public void Zero_Array_ExistingIsoSymbol_CollectionOfZeroes()
		{
			Assert.That(Money.Zero("EUR", 2), Has.Length.EqualTo(2).And
				.All.Matches(Must.Be.MoneyWith(decimal.Zero, Currency.Euro)));
		}

		[Test]
		public void Zero_NullSymbol_Exception()
		{
			Assert.That(() => Money.Zero((string)null), Throws.InstanceOf<ArgumentNullException>());
		}

		[Test]
		public void Zero_Currency_PropertiesSet()
		{
			Assert.That(Money.Zero(Currency.Gbp), Must.Be.MoneyWith(decimal.Zero, Currency.Gbp));
		}

		[Test]
		public void Zero_NullCurrency_Exception()
		{
			Assert.That(() => Money.Zero((Currency)null), Throws.InstanceOf<ArgumentNullException>());
		}

		[Test]
		public void Zero_NonExistingIsoCode_Exception()
		{
			var nonExistingCode = (CurrencyIsoCode)(-7);

			Assert.That(() => Money.Zero(nonExistingCode), Throws.InstanceOf<InvalidEnumArgumentException>().With.Message.StringContaining("-7"));
		}

		[Test]
		public void Zero_NonExistingIsoSymbol_PropertiesSet()
		{
			string nonExistentIsoSymbol = "XYZ";
			Assert.That(() => Money.Zero(nonExistentIsoSymbol), Throws.InstanceOf<InvalidEnumArgumentException>());
		}

		[Test]
		public void Zero_Array_InvalidLength_Exception()
		{
			Assert.That(() => Money.Zero(CurrencyIsoCode.EUR, -1), Throws.InstanceOf<OverflowException>());
		}

		[Test]
		public void Zero_Array_ZeroLength_Empty()
		{
			Assert.That(Money.Zero(Currency.Hkd, 0), Is.Empty);
		}
	}
}
