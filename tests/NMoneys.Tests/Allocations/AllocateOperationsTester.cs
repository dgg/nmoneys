using NMoneys.Allocations;
using NMoneys.Extensions;

namespace NMoneys.Tests.Allocations;

[TestFixture]
public class AllocateOperationsTester
{
	[Test]
	public void Allocate_FairAllocation_CompleteWithZeroRemainder()
	{
		Allocation fair = 40m.Eur().Allocate(4);

		Assert.That(fair.IsComplete, Is.True);
		Assert.That(fair.Remainder, Is.EqualTo(0m.Eur()));
		Assert.That(fair.TotalAllocated, Is.EqualTo(40m.Eur()));
		Assert.That(fair, Has.Count.EqualTo(4));
		Assert.That(fair, Is.EqualTo(new []
		{
			10m.Eur(), 10m.Eur(), 10m.Eur(), 10m.Eur()
		}));
	}

	[Test]
	public void Allocate_UnfairAllocation_CompleteWithZeroRemainder()
	{
		Allocation unfair = 40m.Eur().Allocate(3, RemainderAllocator.LastToFirst);

		Assert.That(unfair.IsComplete, Is.True);
		Assert.That(unfair.Remainder, Is.EqualTo(0m.Eur()));
		Assert.That(unfair.TotalAllocated, Is.EqualTo(40m.Eur()));
		Assert.That(unfair, Has.Count.EqualTo(3));
		Assert.That(unfair, Is.EqualTo(new []
		{
			13.33m.Eur(), 13.33m.Eur(), 13.34m.Eur()
		}));
	}

	[Test]
	public void Allocate_FoemmelsConundrum_FirstToLast()
	{
		Allocation solution = .05m.Usd().Allocate(new RatioCollection(.3m, 0.7m));

		Console.WriteLine(solution);
		Assert.That(solution.IsComplete, Is.True);
		Assert.That(solution.Remainder, Is.EqualTo(0m.Usd()));
		Assert.That(solution.TotalAllocated, Is.EqualTo(.05m.Usd()));
		Assert.That(solution, Has.Count.EqualTo(2));
		Assert.That(solution, Is.EqualTo(new []
		{
			.02m.Usd(), .03m.Usd()
		}));
	}

	[Test]
	public void Allocate_FoemmelsConundrum_LastToFirst()
	{
		Allocation solution = .05m.Usd().Allocate(new RatioCollection(.3m, 0.7m), RemainderAllocator.LastToFirst);

		Console.WriteLine(solution);
		Assert.That(solution.IsComplete, Is.True);
		Assert.That(solution.Remainder, Is.EqualTo(0m.Usd()));
		Assert.That(solution.TotalAllocated, Is.EqualTo(.05m.Usd()));
		Assert.That(solution, Has.Count.EqualTo(2));
		Assert.That(solution, Is.EqualTo(new []
		{
			.01m.Usd(), .04m.Usd()
		}));
	}
}
