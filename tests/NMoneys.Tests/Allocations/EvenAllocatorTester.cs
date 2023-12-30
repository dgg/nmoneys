using NMoneys.Allocations;
using NMoneys.Extensions;
using Iz = NMoneys.Tests.Allocations.Support.Iz;

namespace NMoneys.Tests.Allocations;

[TestFixture]
public class EvenAllocatorTester
{
	[Test]
		public void Allocate_NotEvenTheMinimum_NoException()
		{
			var notEvenAYen = 0.3m.Jpy();

			Assert.That(() => new EvenAllocator(notEvenAYen).Allocate(2), Throws.Nothing);
		}

		[Test]
		public void Allocate_NotEvenTheMinimum_QuasiCompleteAllocation()
		{
			var notEvenAYen = 0.3m.Jpy();

			var incomplete = new EvenAllocator(notEvenAYen).Allocate(2);
			Assert.That(incomplete, Is.EqualTo(new[] { 0m.Jpy(), 0m.Jpy() }));

			Assert.That(incomplete, Iz.QuasiComplete(a =>
			{
				a.Allocated = 0m.Jpy();
				a.Remainder = .3m.Jpy();
			}));
		}

		[Test]
		public void Allocate_SomethingForEveryone_IncompleteWithEveryoneSameAmount()
		{
			var threeYen = 3m.Jpy();
			var allocation = new EvenAllocator(threeYen).Allocate(2);

			Assert.That(allocation, Is.EqualTo(new[] { 1m.Jpy(), 1m.Jpy() }));

			Assert.That(allocation, Iz.Incomplete(a =>
			{
				a.Allocated = 2m.Jpy();
				a.Remainder = 1m.Jpy();
			}));
		}

		[Test]
		public void Allocate_NotEnoughToEveryone_NoAllocation()
		{
			var threeYen = 3m.Jpy();
			var allocation = new EvenAllocator(threeYen).Allocate(4);

			Assert.That(allocation, Is.EqualTo(new[] { 0m.Jpy(), 0m.Jpy(), 0m.Jpy(), 0m.Jpy() }));

			Assert.That(allocation, Iz.NoAllocation(a =>
			{
				a.Remainder = 3m.Jpy();
			}));
		}

		[Test]
		public void Allocate_FairAllocation_CompleteWithEveryoneSameAmount()
		{
			var fair = new EvenAllocator(8m.Usd()).Allocate(4);

			Assert.That(fair, Is.EqualTo(new[] { 2m.Usd(), 2m.Usd(), 2m.Usd(), 2m.Usd() }));

			Assert.That(fair, Iz.Complete(a =>
			{
				a.Allocated = 8m.Usd();
			}));
		}

		[Test]
		public void Allocate_UnfairAllocation_IncompleteWithEveryoneSameAmountWithARemainder()
		{
			var unfair = new EvenAllocator(8.3m.Usd()).Allocate(4);

			Assert.That(unfair, Is.EqualTo(new[] { 2.07m.Usd(), 2.07m.Usd(), 2.07m.Usd(), 2.07m.Usd() }));

			Assert.That(unfair, Iz.Incomplete(a =>
			{
				a.Allocated = 8.28m.Usd();
				a.Remainder = .02m.Usd();
			}));
		}
}
