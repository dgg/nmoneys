using NMoneys.Change;

namespace NMoneys.Tests.Change;

[TestFixture]
public class OptimalChangeSolutionTester
{
	[Test]
	public void ToString_NoSolution_FalseAndEmptyDenominations()
	{
		var subject = new Money(3m);
		var emptySolution = subject.MakeOptimalChange(5m);

		Assert.That(emptySolution.ToString(), Is.EqualTo(
			"OptimalChangeSolution { IsSolution = False Denominations = [] }"
		));
	}

	[Test]
	public void ToString_CompleteSolution_TrueAndDenominations()
	{
		var subject = new Money(5m);
		var wholeSolution = subject.MakeOptimalChange(1m, 3m, 2m);

		Assert.That(wholeSolution.ToString(), Is.EqualTo(
			"OptimalChangeSolution { IsSolution = True Denominations = [ 1 * 3 | 1 * 2 ] }"
		));
	}

	[Test]
	public void ToString_IncompleteSolution_FalseAndEmptyDenominations()
	{
		var subject = new Money(7m);

		var incomplete = subject.MakeOptimalChange(4m, 2m);

		Assert.That(incomplete.ToString(), Is.EqualTo(
			"OptimalChangeSolution { IsSolution = False Denominations = [] }"
		));
	}
}
