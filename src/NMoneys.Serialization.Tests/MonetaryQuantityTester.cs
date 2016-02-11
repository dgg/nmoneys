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

		[Test]
		public void ImplicitConversion_Null_NullInstance()
		{
			MonetaryQuantity @implicit = default(Money?);
			Assert.That(@implicit, Is.Null);
		}

		[Test]
		public void ImplicitConversion_NotNull_InstanceWithMoneyValues()
		{
			Money? notNull = 42.74m.Eur();
			MonetaryQuantity quantity = notNull;
			Assert.That(quantity, Is.Not.Null);

			Assert.That(quantity, Must.Have.Property<MonetaryQuantity>(q => q.Amount, Is.EqualTo(42.74m)) &
				Must.Have.Property<MonetaryQuantity>(q => q.Currency, Is.EqualTo(CurrencyIsoCode.EUR.AlphabeticCode())));
		}

		[Test]
		public void ImplictConversion_NotNullable_InstanceWithMoneyValues()
		{
			Money notNullable = 42.74m.Eur();
			MonetaryQuantity quantity = notNullable;
			Assert.That(quantity, Is.Not.Null);

			Assert.That(quantity, Must.Have.Property<MonetaryQuantity>(q => q.Amount, Is.EqualTo(42.74m)) &
				Must.Have.Property<MonetaryQuantity>(q => q.Currency, Is.EqualTo(CurrencyIsoCode.EUR.AlphabeticCode())));
		}

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
	}
}