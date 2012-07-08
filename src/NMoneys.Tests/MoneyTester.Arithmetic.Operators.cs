using System;
using System.Collections.Generic;
using NMoneys.Extensions;
using NMoneys.Tests.CustomConstraints;
using NUnit.Framework;

namespace NMoneys.Tests
{
	[TestFixture]
	public partial class MoneyTester
	{
		[Test]
		public void UnaryNegate_AnotherMoneyWithNegativeAmount()
		{
			Money oweYouFive = -fiver;
			Assert.That(oweYouFive, Is.Not.SameAs(fiver));
			Assert.That(oweYouFive.Amount, Is.EqualTo(-(fiver.Amount)));
			Assert.That(oweYouFive.CurrencyCode, Is.EqualTo(fiver.CurrencyCode));

			Money youOweMeFive = -oweYouFive;
			Assert.That(youOweMeFive.Amount, Is.EqualTo(fiver.Amount));
		}

		[Test]
		public void Add_SameCurrency_AnotherMoneyWithAddedUpamount()
		{
			Money fifteenQuid = fiver + tenner;

			Assert.That(fifteenQuid, Is.Not.SameAs(fiver).And.Not.SameAs(tenner));
			Assert.That(fifteenQuid.Amount, Is.EqualTo(15));
			Assert.That(fifteenQuid.CurrencyCode, Is.EqualTo(CurrencyIsoCode.GBP));
		}

		[Test]
		public void Add_DifferentCurrency_Exception()
		{
			Money money;
			Assert.That(() => money = fiver + hund, Throws.InstanceOf<DifferentCurrencyException>());
		}

		[Test]
		public void Substract_SameCurrency_AnotherMoneyWithAddedUpamount()
		{
			Money anotherFiver = tenner - fiver;

			Assert.That(anotherFiver, Is.Not.SameAs(fiver).And.Not.SameAs(tenner));
			Assert.That(anotherFiver.Amount, Is.EqualTo(5));
			Assert.That(anotherFiver.CurrencyCode, Is.EqualTo(CurrencyIsoCode.GBP));
		}

		[Test]
		public void Substract_TennerMinusFiver_OweMeMoney()
		{
			Money oweFiver = fiver - tenner;
			Assert.That(oweFiver.Amount, Is.EqualTo(-5));
		}

		[Test]
		public void Substract_DifferentCurrency_Exception()
		{
			Money money;
			Assert.That(() => money = fiver - hund, Throws.InstanceOf<DifferentCurrencyException>());
		}

		#region Total

		[Test]
		public void Total_AllOfSameCurrency_NewInstanceWithAmountAsSumOfAll()
		{
			Assert.That(Money.Total(2m.Dollars(), 3m.Dollars(), 5m.Dollars()), Must.Be.MoneyWith(10m, Currency.Dollar));
		}

		[Test]
		public void Total_DiffCurrency_NewInstanceWithAmountAsSumOfAll()
		{
			Assert.That(() => Money.Total(2m.Dollars(), 3m.Eur(), 5m.Dollars()), Throws.InstanceOf<DifferentCurrencyException>());
		}

		[Test]
		public void Total_NullMoneys_Exception()
		{
			IEnumerable<Money> nullMoneys = null;

			Assert.That(() => Money.Total(nullMoneys), Throws.InstanceOf<ArgumentNullException>()
				.With.Message.StringContaining("moneys"));
		}

		[Test]
		public void Total_EmptyMoneys_Exception()
		{
			Assert.That(() => Money.Total(), Throws.ArgumentException.With.Message.StringContaining("empty"));
			Assert.That(() => Money.Total(new Money[0]), Throws.ArgumentException.With.Message.StringContaining("empty"));
		}

		[Test]
		public void Total_OnlyOneMoney_AnotherMoneyInstanceWithSameInformation()
		{
			Assert.That(Money.Total(10m.Usd()), Is.EqualTo(10m.Usd()));
		}

		#endregion
	}
}