using System;
using NMoneys.Change;
using NMoneys.Extensions;
using NUnit.Framework;

namespace NMoneys.Tests.Change
{
	[TestFixture]
	public class CountWaysToMakeChangeTester
	{
		[Test]
		[TestCase(0), TestCase(-1)]
		public void ChangeCount_NotPositive_Exception(decimal notPositive)
		{
			Denomination[] any = { new Denomination(1) };
			Assert.That(() => new Money(notPositive).CountWaysToMakeChange(any), Throws.InstanceOf<ArgumentOutOfRangeException>());
		}

		[Test]
		public void ChangeCount_NoDenominations_Zero()
		{
			Assert.That(5m.Usd().CountWaysToMakeChange(new Denomination[0]), Is.EqualTo(0));
		}

		[Test]
		public void ChangeCount_NotPossibleToChange_Zero()
		{
			Assert.That(7m.Xxx().CountWaysToMakeChange(4m, 2m), Is.EqualTo(0u));
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
			Assert.That(money.CountWaysToMakeChange(denominationValues), Is.EqualTo(count));
		}
	}
}