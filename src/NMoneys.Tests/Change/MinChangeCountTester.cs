using NMoneys.Change;
using NMoneys.Extensions;
using NUnit.Framework;

namespace NMoneys.Tests.Change
{
	[TestFixture]
	public class MinChangeCountTester
	{
		[Test]
		public void MinChangeCount_Zero_NoWayOfChoosingNoAmount()
		{
			Denomination[] any = { new Denomination(1) };
			Assert.That(Money.Zero().MinChangeCount(any), Is.EqualTo(0));
		}

		[Test]
		public void MinChangeCount_NotPossibleToChange_Zero()
		{
			Assert.That(7m.Xxx().MinChangeCount(4m, 2m), Is.EqualTo(0u));
		}

		private static readonly object[] _minChangeCountSamples = 
		{
			new object[] {4m, new decimal[]{1, 2, 3}, 2u},
			new object[] {10m, new decimal[]{2, 5, 3, 6}, 2u},
			new object[] {6m, new decimal[]{1, 3, 4}, 2u},
			new object[] {5m, new decimal[]{1, 2, 3}, 2u},
		};

		[Test, TestCaseSource(nameof(_minChangeCountSamples))]
		public void MinChangeCount_PossibleChange_AccordingToSamples(decimal amount, decimal[] denominationValues, uint count)
		{
			Money money = new Money(amount);
			Assert.That(money.MinChangeCount(denominationValues), Is.EqualTo(count));
		}

		[Test]
		public void MinChangeCount_NoDenominations_Zero()
		{
			Assert.That(5m.Usd().MinChangeCount(new Denomination[0]), Is.EqualTo(0));
		}
	}
}