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
		public void ChangeCount_Zero_OneWayOfChoosingNoAmount()
		{
			Denomination[] any = { new Denomination(1) };
			Assert.That(Money.Zero().ChangeCount(any), Is.EqualTo(1u));
		}

		[Test]
		public void ChangeCount_NotPossibleToChange_Zero()
		{
			Assert.That(7m.Xxx().ChangeCount(4m, 2m), Is.EqualTo(0u));
		}

		private static readonly object[] _changeCountSamples = 
		{
			new object[] {4m, new decimal[]{1, 2, 3}, 4u},
			new object[] {10m, new decimal[]{2, 5, 3, 6}, 5u},
			new object[] {6m, new decimal[]{1, 3, 4}, 4u},
			new object[] {5m, new decimal[]{1, 2, 3}, 5u},
		};

		[Test, TestCaseSource(nameof(_changeCountSamples))]
		public void ChangeCount_PossibleChange_AccordingToSamples(decimal amount, decimal[] denominationValues, uint count)
		{
			Money money = new Money(amount);
			Assert.That(money.ChangeCount(denominationValues), Is.EqualTo(count));
		}

		[Test]
		public void ChangeCount_NoDenominations_Zero()
		{
			Assert.That(5m.Usd().ChangeCount(new Denomination[0]), Is.EqualTo(0));
		}
	}
}