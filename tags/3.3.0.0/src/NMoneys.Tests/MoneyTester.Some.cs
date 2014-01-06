using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using NMoneys.Tests.CustomConstraints;
using NUnit.Framework;

namespace NMoneys.Tests
{
	[TestFixture]
	public partial class MoneyTester
	{
		[Test]
		public void Some_ExistingIsoCode_CollectionOfMoneys()
		{
			Assert.That(Money.Some(5m, CurrencyIsoCode.USD, 3), Has.Length.EqualTo(3).And
				.All.Matches(Must.Be.MoneyWith(5m, Currency.Usd)));
		}

		[Test]
		public void Some_ExistingIsoSymbol_CollectionOfMoneys()
		{
			Assert.That(Money.Some(10m, "EUR", 2), Has.Length.EqualTo(2).And
				.All.Matches(Must.Be.MoneyWith(10m, Currency.Euro)));
		}

		[Test]
		public void Some_NullSymbol_Exception()
		{
			Assert.That(() => Money.Some(13, (string)null, 2), Throws.InstanceOf<ArgumentNullException>());
		}

		[Test]
		public void Some_Currency_CollectionOfMoneys()
		{
			Assert.That(Money.Zero(Currency.Gbp), Must.Be.MoneyWith(decimal.Zero, Currency.Gbp));
		}

		[Test]
		public void Some_NullCurrency_Exception()
		{
			Assert.That(() => Money.Some(13, (Currency)null, 2), Throws.InstanceOf<ArgumentNullException>());
		}

		[Test]
		public void Some_NonExistingIsoCode_Exception()
		{
			var nonExistingCode = (CurrencyIsoCode)(-7);

			Assert.That(() => Money.Some(13, nonExistingCode, 2), Throws.InstanceOf<InvalidEnumArgumentException>().With.Message.StringContaining("-7"));
		}

		[Test]
		public void Some_NonExistingIsoSymbol_Exception()
		{
			string nonExistentIsoSymbol = "XYZ";
			Assert.That(() => Money.Some(13, nonExistentIsoSymbol, 2), Throws.InstanceOf<InvalidEnumArgumentException>());
		}

		[Test]
		public void Some_InvalidLength_Exception()
		{
			Assert.That(() => Money.Some(13, CurrencyIsoCode.EUR, -1), Throws.InstanceOf<OverflowException>());
		}

		[Test]
		public void Some_ZeroLength_Empty()
		{
			Assert.That(Money.Some(1000, Currency.Hkd, 0), Is.Empty);
		}
	}
}
