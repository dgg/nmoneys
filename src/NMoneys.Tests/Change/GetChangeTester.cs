using System;
using System.Linq;
using NMoneys.Change;
using NMoneys.Extensions;
using NMoneys.Tests.Change.Support;
using NUnit.Framework;
using Testing.Commons;

namespace NMoneys.Tests.Change
{
	[TestFixture]
	public class GetChangeTester
	{
		[Test]
		[TestCase(0), TestCase(-1)]
		public void GetChange_NotPositive_Exception(decimal notPositive)
		{
			Denomination[] any = { new Denomination(1m) };

			Assert.That(()=> new Money(notPositive).GetChange(any), Throws.InstanceOf<ArgumentOutOfRangeException>());
		}

		[Test]
		public void GetChange_NoDenominations_EmptySolution()
		{
			var subject = new Money(5m);

			var emptySolution = subject.GetChange(new Denomination[0]);

			Assert.That(emptySolution, Must.Be.NoChange(remainder: subject));
		}

		[Test]
		public void GetChange_NotEnoughToMakeChange_EmptySolution()
		{
			var subject = new Money(3m);
			var emptySolution = subject.GetChange(5m);

			Assert.That(emptySolution, Must.Be.NoChange(remainder: subject));
		}

		[Test]
		public void GetChange_SameDenominationAsAmount_OneDenominationAndNoRemainder()
		{
			var subject = new Money(5m);
			var change = subject.GetChange(5m);

			Assert.That(change, Must.Be.CompleteChange(CurrencyIsoCode.XXX, 
				1.x(5)));
		}

		[Test]
		public void GetChange_ChangePossible_MultipleDenominationsNoRemainder()
		{
			var subject = new Money(5m);
			var wholeSolution = subject.GetChange(1m, 3m, 2m);

			Assert.That(wholeSolution, Must.Be.CompleteChange(CurrencyIsoCode.XXX,
				1.x(3), 1.x(2)));
			Assert.That(wholeSolution, Has.Property(nameof(ChangeSolution.TotalCount)).EqualTo(2));
		}

		private static readonly object[] _greedySuboptimal =
		{
			new object[]
			{
				30m, new[] {1m, 15m, 25m}, new[]
				{
					1.x(25), 5.x(1)
				}
			},
			new object[]
			{
				40m, new[] {1m, 5m, 10m, 20m, 25m}, new[]
				{
					1.x(25), 1.x(10), 1.x(5)
				}
			},
			new object[]
			{
				6m, new[] {1m, 3m, 4m}, new[]
				{
					1.x(4), 2.x(1)
				}
			},
			new object[]
			{
				63m, new[] {1m, 5m, 10m, 21m, 25m}, new[]
				{
					2.x(25), 1.x(10), 3.x(1)
				}
			}
		};

		[Test, TestCaseSource(nameof(_greedySuboptimal))]
		public void GetChange_NonOptimalForGreedy_SubOptimalSolution(decimal amount, decimal[] denominationValues, QDenomination[] solution)
		{
			var subject = new Money(amount);

			var subOptimalSolution = subject.GetChange(denominationValues);

			Assert.That(subOptimalSolution, Must.Be.CompleteChange(CurrencyIsoCode.XXX, solution));
			Assert.That(subOptimalSolution.TotalCount, Is.EqualTo(solution.Sum(s => s.Quantity)));
		}

		[Test]
		public void GetChange_NotCompleteSolution_QuantifiedAndRemainder()
		{
			var subject = new Money(7m);

			var incompleteSolution = subject.GetChange(4m, 2m);

			Assert.That(incompleteSolution, Must.Not.Be.IncompleteChange(1m.Xxx(), 
				1.x(4), 1.x(2)));
			Assert.That(incompleteSolution.TotalCount, Is.EqualTo(2));
		}
	}
}