using NMoneys.Tests.Support;
using NUnit.Framework;

namespace NMoneys.Tests
{
	[TestFixture]
	public partial class MoneyTester
	{
		[Test]
		public void CompareTo_NonGenericDifferentTypes()
		{
			using (CultureReseter.Set("en-US"))
			{
				Assert.That(() => fiver.CompareTo("asd"), Throws.ArgumentException);
				Assert.That(() => fiver.CompareTo(5), Throws.ArgumentException, "no implicit comparison");
				Assert.That(() => fiver.CompareTo(5m), Throws.ArgumentException, "no implicit comparison");
			}

			using (CultureReseter.Set("en-GB"))
			{
				Assert.That(() => fiver.CompareTo("asd"), Throws.ArgumentException);
				Assert.That(() => fiver.CompareTo(5), Throws.ArgumentException, "no implicit comparison");
				Assert.That(() => fiver.CompareTo(5m), Throws.ArgumentException, "no implicit comparison");
			}
		}

		[Test]
		public void CompareTo_Null_Positive()
		{
			Assert.That(fiver.CompareTo(null), Is.GreaterThan(0));
		}

		[Test]
		public void CompareTo_Generic()
		{
			Assert.That(fiver.CompareTo(tenner), Is.LessThan(0));
			Assert.That(tenner.CompareTo(fiver), Is.GreaterThan(0));
			Assert.That(fiver.CompareTo(fiver), Is.EqualTo(0));

			var anotherFiver = new Money(5, Currency.Gbp);
			Assert.That(fiver.CompareTo(anotherFiver), Is.EqualTo(0));
			Assert.That(anotherFiver.CompareTo(fiver), Is.EqualTo(0));

			Assert.That(() => fiver.CompareTo(hund), Throws.InstanceOf<DifferentCurrencyException>());
			Assert.That(() => hund.CompareTo(fiver), Throws.InstanceOf<DifferentCurrencyException>());
		}

		[Test]
		public void ComparisionOperators_Generic()
		{
			var anotherFiver = new Money(5, CurrencyIsoCode.GBP);

			Assert.That(fiver > anotherFiver, Is.False);
			Assert.That(fiver > tenner, Is.False);
			Assert.That(tenner > fiver, Is.True);

			Assert.That(fiver < anotherFiver, Is.False);
			Assert.That(fiver < tenner, Is.True);
			Assert.That(tenner < fiver, Is.False);

			Assert.That(fiver >= anotherFiver, Is.True);
			Assert.That(fiver >= tenner, Is.False);
			Assert.That(tenner >= fiver, Is.True);

			Assert.That(fiver <= anotherFiver, Is.True);
			Assert.That(fiver <= tenner, Is.True);
			Assert.That(tenner <= fiver, Is.False);
		}
	}
}