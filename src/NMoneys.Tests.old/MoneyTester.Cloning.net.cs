using NUnit.Framework;

namespace NMoneys.Tests
{
	[TestFixture]
	public partial class MoneyTester
	{
		[Test]
		public void Clone_IClonable_ObjectOfCorrectType()
		{
			object anotherFiver = fiver.Clone();

			Assert.That(anotherFiver, Is.TypeOf<Money>().And.Not.SameAs(fiver));

			Assert.That(((Money)anotherFiver).Amount, Is.EqualTo(fiver.Amount));
			Assert.That(((Money)anotherFiver).CurrencyCode, Is.EqualTo(fiver.CurrencyCode));
		}
	}
}