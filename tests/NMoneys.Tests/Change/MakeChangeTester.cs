using NMoneys.Change;
using NMoneys.Extensions;
using NMoneys.Tests.Change.Support;
using Iz = NMoneys.Tests.Change.Support.Iz;

namespace NMoneys.Tests.Change;

[TestFixture]
public class MakeChangeTester
{
	[Test]
	[TestCase(0), TestCase(-1)]
	public void MakeChange_NotPositive_Exception(decimal notPositive)
	{
		Denomination[] any = { new(1m) };

		Assert.That(() => new Money(notPositive).MakeChange(any), Throws.InstanceOf<ArgumentOutOfRangeException>());
	}

	[Test]
	public void MakeChange_NoDenominations_EmptySolution()
	{
		var subject = new Money(5m);

		var emptySolution = subject.MakeChange(Array.Empty<Denomination>());

		Assert.That(emptySolution, Iz.NoChange(remainder: subject));
	}

	[Test]
	public void MakeChange_NullDenominations_Exception()
	{
		Denomination[] @null = null!;
		Assert.That(() => 5m.Usd().MakeChange(@null), Throws.ArgumentNullException);
	}

	[Test]
	public void MakeChange_NotEnoughToMakeChange_EmptySolution()
	{
		var subject = new Money(3m);
		var emptySolution = subject.MakeChange(5m);

		Assert.That(emptySolution, Iz.NoChange(remainder: subject));
	}

	[Test]
	public void MakeChange_IdentityChange_OneDenominationAndNoRemainder()
	{
		var subject = new Money(5m);
		var change = subject.MakeChange(5m);

		Assert.That(change, Iz.CompleteChange(1, 1.x(5)));
	}

	[Test]
	public void MakeChange_ChangePossible_MultipleDenominationsNoRemainder()
	{
		var subject = new Money(5m);
		var wholeSolution = subject.MakeChange(1m, 3m, 2m);

		Assert.That(wholeSolution, Iz.CompleteChange(2, 1.x(3), 1.x(2)));
	}

	private static readonly object[] _greedySuboptimal =
	{
			new object[]
			{
				30m, new[] {1m, 15m, 25m}, 6u, new[]
				{
					1.x(25), 5.x(1)
				}
			},
			new object[]
			{
				40m, new[] {1m, 5m, 10m, 20m, 25m}, 3u, new[]
				{
					1.x(25), 1.x(10), 1.x(5)
				}
			},
			new object[]
			{
				6m, new[] {1m, 3m, 4m}, 3u, new[]
				{
					1.x(4), 2.x(1)
				}
			},
			new object[]
			{
				63m, new[] {1m, 5m, 10m, 21m, 25m}, 6u, new[]
				{
					2.x(25), 1.x(10), 3.x(1)
				}
			}
		};

	[Test, TestCaseSource(nameof(_greedySuboptimal))]
	public void MakeChange_NonOptimalForGreedy_SubOptimalSolution(decimal amount, decimal[] denominationValues, uint totalCount, QDenomination[] solution)
	{
		var subject = new Money(amount);

		var subOptimalSolution = subject.MakeChange(denominationValues);

		Assert.That(subOptimalSolution, Iz.CompleteChange(totalCount, solution));
	}

	[Test]
	public void MakeChange_NotCompleteSolution_QuantifiedAndRemainder()
	{
		var subject = new Money(7m);

		var incompleteSolution = subject.MakeChange(4m, 2m);

		Assert.That(incompleteSolution, Iz.PartialChange(1m.Xxx(), 2,
			1.x(4), 1.x(2)));
		Assert.That(incompleteSolution.TotalCount, Is.EqualTo(2));
	}
}
