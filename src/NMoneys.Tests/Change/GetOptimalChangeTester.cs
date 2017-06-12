using System;
using NMoneys.Change;
using NMoneys.Tests.Change.Support;
using NUnit.Framework;
using Testing.Commons;

namespace NMoneys.Tests.Change
{
	[TestFixture]
	public class GetOptimalChangeTester
	{
		[Test]
		[TestCase(0), TestCase(-1)]
		public void GetOptimalChange_NotPositive_Exception(decimal notPositive)
		{
			Denomination[] any = { new Denomination(1m) };

			Assert.That(()=> new Money(notPositive).GetOptimalChange(any), Throws.InstanceOf<ArgumentOutOfRangeException>());
		}

		private static readonly object[] _greedySuboptimal =
		{
			new object[]
			{
				30m, new[] {1m, 15m, 25m}, new[]
				{
					2.x(15)
				}
			},
			new object[]
			{
				40m, new[] {1m, 5m, 10m, 20m, 25m}, new[]
				{
					2.x(20)
				}
			},
			new object[]
			{
				6m, new[] {1m, 3m, 4m}, new[]
				{
					2.x(3)
				}
			},
			new object[]
			{
				63m, new[] {1m, 5m, 10m, 21m, 25m}, new[]
				{
					3.x(21)
				}
			}
		};

		[Test, TestCaseSource(nameof(_greedySuboptimal))]
		public void GetOptimalChange_NonOptimalForGreedy_OptimalSolution(decimal amount, decimal[] denominationValues, QDenomination[] solution)
		{
			var changeable = new Money(amount);

			var optimalSolution = changeable.GetOptimalChange(denominationValues);

			Assert.That(optimalSolution, Must.Be.OptimalChange(solution));
		}
	}
}