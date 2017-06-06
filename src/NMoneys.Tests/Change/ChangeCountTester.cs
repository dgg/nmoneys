using NMoneys.Change;
using NMoneys.Extensions;
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

		[Test]
		public void ChangeCount_NotPossibleToChange_Zero()
		{
			Money notCompletelyChangeable = 7m.Xxx();
			Denomination[] denominations = { new Denomination(4m), new Denomination(2m) };
			Assert.That(notCompletelyChangeable.ChangeCount(denominations), Is.EqualTo(0u));
		}
	}
}