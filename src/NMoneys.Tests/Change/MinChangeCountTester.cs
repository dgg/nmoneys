using System;
using NMoneys.Change;
using NMoneys.Extensions;
using NUnit.Framework;

namespace NMoneys.Tests.Change
{
	[TestFixture]
	public class MinChangeCountTester
	{
		[Test]
		[TestCase(0), TestCase(-1)]
		public void MinChangeCount_NotPositive_Exception(decimal notPositive)
		{
			Denomination[] any = { new Denomination(1) };
			Assert.That(() => new Money(notPositive).MinChangeCount(any), Throws.InstanceOf<ArgumentOutOfRangeException>());
		}

		[Test]
		public void MinChangeCount_NoDenominations_Zero()
		{
			Assert.That(5m.Usd().MinChangeCount(new Denomination[0]), Is.EqualTo(0));
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
			new object[] {63m, new decimal[]{ 1, 5, 10, 21, 25 }, 3u},
		};

		[Test, TestCaseSource(nameof(_minChangeCountSamples))]
		public void MinChangeCount_PossibleChange_AccordingToSamples(decimal amount, decimal[] denominationValues, uint count)
		{
			Money money = new Money(amount);
			Assert.That(money.MinChangeCount(denominationValues), Is.EqualTo(count));
		}
	}
}