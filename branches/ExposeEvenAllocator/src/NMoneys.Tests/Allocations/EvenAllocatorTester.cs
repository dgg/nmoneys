using NMoneys.Allocations;
using NMoneys.Extensions;
using NUnit.Framework;

namespace NMoneys.Tests.Allocations
{
	[TestFixture]
	public class EvenAllocatorTester
	{
		[Test]
		public void Allocate_NotEvenTheMinimun_NoException()
		{
			var notEvenAYen = 0.3m.Jpy();

			Money allocated;
			Assert.That(() => new EvenAllocator(notEvenAYen).Allocate(2, out allocated), Throws.Nothing);
		}

		[Test]
		public void Allocate_NotEvenTheMinimun_NoAllocation()
		{
			var notEvenAYen = 0.3m.Jpy();

			Money allocated;
			Assert.That(new EvenAllocator(notEvenAYen).Allocate(2, out allocated), Is.EqualTo(new[] { 0m.Jpy(), 0m.Jpy() }));
			Assert.That(allocated, Is.EqualTo(Money.Zero(Currency.Jpy)));
		}

		[Test]
		public void Allocate_SomethingForEveryone_EveryoneSameAmount()
		{
			var threeYen = 3m.Jpy();
			Money allocated;
			Money[] allocation = new EvenAllocator(threeYen).Allocate(2, out allocated);

			Assert.That(allocation, Is.EqualTo(new[] { 1m.Jpy(), 1m.Jpy() }));
			Assert.That(allocated, Is.EqualTo(2m.Jpy()));
		}

		[Test]
		public void Allocate_NotEnoughToEveryone_NoAllocation()
		{
			var threeYen = 3m.Jpy();
			Money allocated;
			Money[] allocation = new EvenAllocator(threeYen).Allocate(4, out allocated);

			Assert.That(allocation, Is.EqualTo(new[] { 0m.Jpy(), 0m.Jpy(), 0m.Jpy(), 0m.Jpy() }));
			Assert.That(allocated, Is.EqualTo(0m.Jpy()));
		}

		[Test]
		public void Allocate_FairAllocation_EveryoneGetsTheSameWithNoRemainder()
		{
			Money allocated;
			Money[] fair = new EvenAllocator(8m.Usd()).Allocate(4, out allocated);

			Assert.That(fair, Is.EqualTo(new[] { 2m.Usd(), 2m.Usd(), 2m.Usd(), 2m.Usd() }));
			Assert.That(allocated, Is.EqualTo(8m.Usd()));
		}

		[Test]
		public void Allocate_UnfairAllocation_EveryoneGetsTheSameAmountWithARemainder()
		{
			Money allocated;
			Money[] unfair = new EvenAllocator(8.3m.Usd()).Allocate(4, out allocated);

			Assert.That(unfair, Is.EqualTo(new[] { 2.07m.Usd(), 2.07m.Usd(), 2.07m.Usd(), 2.07m.Usd() }));
			Assert.That(allocated, Is.EqualTo(8.28m.Usd()));
		}
	}
}
