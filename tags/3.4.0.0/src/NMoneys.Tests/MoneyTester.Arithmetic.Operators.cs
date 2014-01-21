using NMoneys.Extensions;
using NUnit.Framework;

namespace NMoneys.Tests
{
	[TestFixture]
	public partial class MoneyTester
	{
		[Test]
		public void opUnaryNegation_AnotherMoneyWithNegativeAmount()
		{
			Money oweYouFive = -fiver;
			Assert.That(oweYouFive, Is.Not.SameAs(fiver));
			Assert.That(oweYouFive.Amount, Is.EqualTo(-(fiver.Amount)));
			Assert.That(oweYouFive.CurrencyCode, Is.EqualTo(fiver.CurrencyCode));

			Money youOweMeFive = -oweYouFive;
			Assert.That(youOweMeFive.Amount, Is.EqualTo(fiver.Amount));
		}

		#region addition

		[Test]
		public void opAddition_SameCurrency_AnotherMoneyWithAddedUpamount()
		{
			Money fifteenQuid = fiver + tenner;

			Assert.That(fifteenQuid, Is.Not.SameAs(fiver).And.Not.SameAs(tenner));
			Assert.That(fifteenQuid.Amount, Is.EqualTo(15));
			Assert.That(fifteenQuid.CurrencyCode, Is.EqualTo(CurrencyIsoCode.GBP));
		}

		[Test]
		public void opAddition_DifferentCurrency_Exception()
		{
			Money money;
			Assert.That(() => money = fiver + hund, Throws.InstanceOf<DifferentCurrencyException>());
		}

		#endregion

		#region substraction

		[Test]
		public void opSubstraction_SameCurrency_AnotherMoneyWithAddedUpamount()
		{
			Money anotherFiver = tenner - fiver;

			Assert.That(anotherFiver, Is.Not.SameAs(fiver).And.Not.SameAs(tenner));
			Assert.That(anotherFiver.Amount, Is.EqualTo(5));
			Assert.That(anotherFiver.CurrencyCode, Is.EqualTo(CurrencyIsoCode.GBP));
		}

		[Test]
		public void opSubstraction_TennerMinusFiver_OweMeMoney()
		{
			Money oweFiver = fiver - tenner;
			Assert.That(oweFiver.Amount, Is.EqualTo(-5));
		}

		[Test]
		public void opSubstraction_DifferentCurrency_Exception()
		{
			Money money;
			Assert.That(() => money = fiver - hund, Throws.InstanceOf<DifferentCurrencyException>());
		}

		#endregion

		#region multiplication

		[Test]
		public void opMultiply_MultipliesAmount()
		{
			Money oweMeFour = 2m.Dkk() * -2;

			Assert.That(oweMeFour.Amount, Is.EqualTo(-4m));
			Assert.That(oweMeFour.CurrencyCode, Is.EqualTo(CurrencyIsoCode.DKK));
		}


		[Test]
		public void opMultiply_SupportsAllIntegralTypes()
		{
			Money money = new Money(1m, CurrencyIsoCode.EUR), min, max;

			byte b = byte.MaxValue;
			sbyte sb = sbyte.MinValue;
			max = money * b; //  * long
			min = money * sb; // * long
			Assert.That(min.Amount, Is.EqualTo(sbyte.MinValue));
			Assert.That(max.Amount, Is.EqualTo(byte.MaxValue));

			short s = short.MinValue;
			ushort us = ushort.MaxValue;
			min = money * s; // * long
			max = money * us; // * long
			Assert.That(min.Amount, Is.EqualTo(short.MinValue));
			Assert.That(max.Amount, Is.EqualTo(ushort.MaxValue));

			int i = int.MinValue;
			uint ui = uint.MaxValue;
			min = money * i; // * long
			max = money * ui; //  * long
			Assert.That(min.Amount, Is.EqualTo(int.MinValue));
			Assert.That(max.Amount, Is.EqualTo(uint.MaxValue));

			long l = long.MinValue;
			min = money* l; // * long
			max = money * long.MaxValue; // * long
			Assert.That(min.Amount, Is.EqualTo(long.MinValue));
			Assert.That(max.Amount, Is.EqualTo(long.MaxValue));
		}

		#endregion
	}
}