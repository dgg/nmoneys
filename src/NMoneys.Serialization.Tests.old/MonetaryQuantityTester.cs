using NMoneys.Extensions;
using NMoneys.Serialization.Entity_Framework;
using NUnit.Framework;
using Testing.Commons;
using Testing.Commons.NUnit.Constraints;

namespace NMoneys.Serialization.Tests
{
	[TestFixture]
	public class MonetaryQuantityTester
	{
		#region construction

		[Test]
		public void Ctor_InstanceWithMoneyValues()
		{
			MonetaryQuantity subject = new MonetaryQuantity(42.74m.Eur());

			Assert.That(subject, Must.Have.Property<MonetaryQuantity>(q => q.Amount, Is.EqualTo(42.74m)) &
				Must.Have.Property<MonetaryQuantity>(q => q.Currency, Is.EqualTo(CurrencyIsoCode.EUR.AlphabeticCode())));
		}

		[Test]
		public void Factory_Null_Null()
		{
			Assert.That(MonetaryQuantity.From(default(Money?)), Is.Null);
		}

		[Test]
		public void Factory_NotNull_InstanceWithMoneyValues()
		{
			Money? notNull = 42.74m.Eur();
			MonetaryQuantity quantity = MonetaryQuantity.From(notNull);
			Assert.That(quantity, Is.Not.Null);

			Assert.That(quantity, Must.Have.Property<MonetaryQuantity>(q => q.Amount, Is.EqualTo(42.74m)) &
				Must.Have.Property<MonetaryQuantity>(q => q.Currency, Is.EqualTo(CurrencyIsoCode.EUR.AlphabeticCode())));
		}

		[Test]
		public void Factory_NotNullable_InstanceWithMoneyValues()
		{
			Money notNullable = 42.74m.Eur();
			MonetaryQuantity quantity = MonetaryQuantity.From(notNullable);
			Assert.That(quantity, Is.Not.Null);

			Assert.That(quantity, Must.Have.Property<MonetaryQuantity>(q => q.Amount, Is.EqualTo(42.74m)) &
				Must.Have.Property<MonetaryQuantity>(q => q.Currency, Is.EqualTo(CurrencyIsoCode.EUR.AlphabeticCode())));
		}

		#endregion

		#region conversion from Money?

		[Test]
		public void ExplicitConversion_Null_NullInstance()
		{
			MonetaryQuantity @explicit = (MonetaryQuantity) default(Money?);
			Assert.That(@explicit, Is.Null);
		}

		[Test]
		public void ExplicitConversion_NotNull_InstanceWithMoneyValues()
		{
			Money? notNull = 42.74m.Eur();
			MonetaryQuantity quantity = (MonetaryQuantity)notNull;
			Assert.That(quantity, Is.Not.Null);

			Assert.That(quantity, Must.Have.Property<MonetaryQuantity>(q => q.Amount, Is.EqualTo(42.74m)) &
				Must.Have.Property<MonetaryQuantity>(q => q.Currency, Is.EqualTo(CurrencyIsoCode.EUR.AlphabeticCode())));
		}

		[Test]
		public void ExplicitConversion_NotNullable_InstanceWithMoneyValues()
		{
			Money notNullable = 42.74m.Eur();
			MonetaryQuantity quantity = (MonetaryQuantity)notNullable;
			Assert.That(quantity, Is.Not.Null);

			Assert.That(quantity, Must.Have.Property<MonetaryQuantity>(q => q.Amount, Is.EqualTo(42.74m)) &
				Must.Have.Property<MonetaryQuantity>(q => q.Currency, Is.EqualTo(CurrencyIsoCode.EUR.AlphabeticCode())));
		}

		
		#endregion

		#region conversion to Money?

		[Test]
		public void ToMoney_Null_Null()
		{
			MonetaryQuantity @null = null;
			Assert.That(MonetaryQuantity.ToMoney(@null), Is.Null);
		}

		[Test]
		public void ToMoney_NullAmount_Null()
		{
			MonetaryQuantity noAmount = new MonetaryQuantity(null, "XXX");
			Assert.That(MonetaryQuantity.ToMoney(noAmount), Is.Null);
		}


		[Test]
		public void ToMoney_NullCurrency_MissingCurrency()
		{
			MonetaryQuantity noCurrency = new MonetaryQuantity(12, null);
			Money? money = MonetaryQuantity.ToMoney(noCurrency);
			Assert.That(money.HasValue, Is.True);
			Assert.That(money.Value.CurrencyCode, Is.EqualTo(CurrencyIsoCode.XXX));
		}

		[Test]
		public void MoneyExplicitConversion_Null_Null()
		{
			MonetaryQuantity @null = null;
			Money? money = (Money?)@null;
			Assert.That(money, Is.Null);
		}

		[Test]
		public void MoneyExplicitConversion_NullAmount_Null()
		{
			MonetaryQuantity noAmount = new MonetaryQuantity(null, "XXX");
			Money? money = (Money?) noAmount;
			Assert.That(money, Is.Null);
		}

		[Test]
		public void MoneyExplicitConversion_NullCurrency_MissingCurrency()
		{
			MonetaryQuantity noCurrency = new MonetaryQuantity(12, null);
			Money? money = (Money?) noCurrency;
			Assert.That(money.HasValue, Is.True);
			Assert.That(money.Value.CurrencyCode, Is.EqualTo(CurrencyIsoCode.XXX));
		}

		#endregion

		[Test]
		public void Equality_SameCurrencyAndAmount_True()
		{
			MonetaryQuantity fiver = new MonetaryQuantity(5m.Gbp()), 
				anotherFiver = (MonetaryQuantity)new Money(5, Currency.Gbp);

			Assert.That(fiver.Equals(fiver), Is.True);
			Assert.That(fiver.Equals(anotherFiver), Is.True);
			Assert.That(anotherFiver.Equals(fiver), Is.True);
			Assert.That(fiver == anotherFiver, Is.True);
			Assert.That(anotherFiver == fiver, Is.True);
		}

		[Test]
		public void Equality_DifferentAmountOrCurrency_False()
		{
			MonetaryQuantity fiver = new MonetaryQuantity(5m.Gbp()),
				tenner = (MonetaryQuantity)10m.Gbp(), hund = (MonetaryQuantity)100m.Dkk();

			Assert.That(fiver.Equals(tenner), Is.False);
			Assert.That(tenner.Equals(fiver), Is.False);
			Assert.That(fiver == tenner, Is.False);
			Assert.That(tenner == fiver, Is.False);

			Assert.That(fiver.Equals(hund), Is.False);
			Assert.That(hund.Equals(fiver), Is.False);
			Assert.That(fiver == hund, Is.False);
			Assert.That(hund == fiver, Is.False);
		}

		[Test]
		public void Equality_DifferentTypes()
		{
			MonetaryQuantity fiver = new MonetaryQuantity(5m.Gbp());

			Assert.That(fiver.Equals("gbp"), Is.False);
			Assert.That("GBP".Equals(fiver), Is.False);
			Assert.That(fiver.Equals(5m), Is.False);
			Assert.That(5m.Equals(fiver), Is.False);
		}

		[Test]
		public void Inequality_SameCurrencyAndAmount_False()
		{
			MonetaryQuantity fiver = new MonetaryQuantity(5m.Gbp()),
				anotherFiver = (MonetaryQuantity)new Money(5, Currency.Gbp);

			Assert.That(fiver != anotherFiver, Is.False);
			Assert.That(anotherFiver != fiver, Is.False);
		}

		[Test]
		public void Inequality_DifferentAmountOrCurrency_True()
		{
			MonetaryQuantity fiver = new MonetaryQuantity(5m.Gbp()),
				tenner = (MonetaryQuantity)10m.Gbp(), hund = (MonetaryQuantity)100m.Dkk();

			Assert.That(fiver != tenner, Is.True);
			Assert.That(tenner != fiver, Is.True);

			Assert.That(fiver != hund, Is.True);
			Assert.That(hund != fiver, Is.True);
		}

		[Test]
		public void Equality_NoCurrencyAndMissingCurrent_Equals(
			[Values(null, "", " ", "  ")]string missing)
		{
			MonetaryQuantity noCurrency = (MonetaryQuantity)12m.Xxx(),
				missingCurrency = new MonetaryQuantity(12m, missing);

			Assert.That(noCurrency, Is.EqualTo(missingCurrency));
		}
	}
}