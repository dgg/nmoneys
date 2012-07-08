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
	}
}