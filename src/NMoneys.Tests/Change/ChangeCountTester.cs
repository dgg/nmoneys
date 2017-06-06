using NMoneys.Change;
using NUnit.Framework;

namespace NMoneys.Tests.Change
{
	[TestFixture]
	public class ChangeCountTester
	{
		[Test]
		public void ChangeCount_Zero_OneWayOfChoosingNoDenominations()
		{
			var zero = Money.Zero();

			Denomination[] any = { new Denomination(1) };

			Assert.That(zero.ChangeCount(any), Is.EqualTo(1u));
		}
	}
}