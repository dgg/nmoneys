using System.Linq;
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

		private static readonly object[] changeCountSamples = 
		{
			new object[] {4m, new decimal[]{1, 2, 3}, 4u},
			new object[] {10m, new decimal[]{2, 5, 3, 6}, 5u},
			new object[] {6m, new decimal[]{1, 3, 4}, 4u},
			new object[] {5m, new decimal[]{1, 2, 3}, 5u},
		};

		[Test, TestCaseSource(nameof(changeCountSamples))]
		public void ChangeCount_PossibleChange_AccordingToSamples(decimal amount, decimal[] denominationValues, uint count)
		{
			Money money = new Money(amount);
			Denomination[] denominations = denominationValues.Select(d => new Denomination(d)).ToArray();

			Assert.That(money.ChangeCount(denominations), Is.EqualTo(count));
		}

		[Test]
		public void ChangeCount_NoDenominations_Zero()
		{
			Assert.That(5m.Usd().ChangeCount(), Is.EqualTo(0));
		}
	}
}