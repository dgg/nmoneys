using System.Threading;
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
			Money? @null = default(Money?);
			Assert.That(MonetaryQuantity.From(@null), Is.Null);
		}

		[Test]
		public void Factory_NotNull_InstanceWithMoeyValues()
		{
			Money? notNull = 42.74m.Eur();
			MonetaryQuantity quantity = MonetaryQuantity.From(notNull);
			Assert.That(quantity, Is.Not.Null);

			Assert.That(quantity, Must.Have.Property<MonetaryQuantity>(q => q.Amount, Is.EqualTo(42.74m)) &
				Must.Have.Property<MonetaryQuantity>(q => q.Currency, Is.EqualTo(CurrencyIsoCode.EUR.AlphabeticCode())));
		}
	}
}