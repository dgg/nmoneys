using NUnit.Framework;

namespace NMoneys.Tests.Change
{
	[TestFixture]
	public class DenominationTester
	{
		[Test]
		public void Ctor_WithValue()
		{
			var subject = new Denomination(5m);

			Assert.That(subject.Value, Is.EqualTo(5m));
		}
	}
}