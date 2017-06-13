using System;
using NMoneys.Change;
using NMoneys.Tests.Change.Support;
using NUnit.Framework;
using Testing.Commons;

namespace NMoneys.Tests.Change
{
	[TestFixture]
	public class MakeOptimalChangeTester
	{
		[Test]
		[TestCase(0), TestCase(-1)]
		public void MakeOptimalChange_NotPositive_Exception(decimal notPositive)
		{
			Denomination[] any = { new Denomination(1m) };

			Assert.That(()=> new Money(notPositive).MakeOptimalChange(any), Throws.InstanceOf<ArgumentOutOfRangeException>());
		}

		[Test]
		public void MakeOptimalChange_NoDenominations_EmptySolution()
		{
			var subject = new Money(5m);

			var emptySolution = subject.MakeOptimalChange(new Denomination[0]);

			Assert.That(emptySolution, Must.Be.NoChange());
		}

		[Test]
		public void MakeOptimalChange_NotEnoughToMakeChange_EmptySolution()
		{
			var subject = new Money(3m);
			var emptySolution = subject.MakeOptimalChange(5m);

			Assert.That(emptySolution, Must.Be.NoChange());
		}

		[Test]
		public void MakeOptimalChange_IdentityChange_OneDenominationAndNoRemainder()
		{
			var subject = new Money(5m);
			var change = subject.MakeOptimalChange(5m);

			Assert.That(change, Must.Be.OptimalChange(1.x(5)));
		}

		[Test]
		public void MakeOptimalChange_ChangePossible_MultipleDenominationsNoRemainder()
		{
			var subject = new Money(5m);
			var wholeSolution = subject.MakeOptimalChange(1m, 3m, 2m);

			Assert.That(wholeSolution, Must.Be.OptimalChange(1.x(3), 1.x(2)));
			Assert.That(wholeSolution, Has.Property(nameof(ChangeSolution.TotalCount)).EqualTo(2));
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
		public void MakeOptimalChange_NonOptimalForGreedy_OptimalSolution(decimal amount, decimal[] denominationValues, QDenomination[] solution)
		{
			var changeable = new Money(amount);

			var optimalSolution = changeable.MakeOptimalChange(denominationValues);

			Assert.That(optimalSolution, Must.Be.OptimalChange(solution));
		}

		[Test]
		public void MakeOptimalChange_NotCompleteSolution_EmptySolution()
		{
			var subject = new Money(7m);

			var incomplete = subject.MakeOptimalChange(4m, 2m);

			Assert.That(incomplete, Must.Be.NoChange());
		}
	}
}