using System;
using System.Collections.Generic;
using System.Linq;
using NMoneys.Allocations;
using NMoneys.Extensions;
using NMoneys.Tests.Support;
using NUnit.Framework;

namespace NMoneys.Tests
{
	[TestFixture]
	public partial class MoneyTester
	{
		[TestCaseSource("ProvidedAllocators")]
		public void Allocate_FairAllocation_EveryoneGetTheSameQuantity(IRemainderAllocator allocator)
		{
			Allocation allocated = 8m.Usd().Allocate(4, allocator);

			Assert.That(allocated, Is.EqualTo(new[] { 2m.Usd(), 2m.Usd(), 2m.Usd(), 2m.Usd() }));
		}

		internal IEnumerable<IRemainderAllocator> ProvidedAllocators
		{
			get
			{
				yield return RemainderAllocator.FirstToLast;
				yield return RemainderAllocator.LastToFirst;
				yield return RemainderAllocator.Random;
			}
		}

		[Test]
		public void Allocate_FairAllocation_RemainderNotAsked()
		{
			var spy = new RemainderAllocatorSpy();
			8m.Gbp().Allocate(4, spy);

			Assert.That(spy.AskedToAllocate, Is.False);

			spy = new RemainderAllocatorSpy();
			8m.Gbp().Allocate(4, spy);

			Assert.That(spy.AskedToAllocate, Is.False);
		}

		[Test]
		public void Allocate_UnfairAllocation_FirstToLast_FirstAmountsAreBigger()
		{
			Allocation allocated = 8.3m.Usd().Allocate(4, RemainderAllocator.FirstToLast);

			Assert.That(allocated, Is.EqualTo(new[] { 2.08m.Usd(), 2.08m.Usd(), 2.07m.Usd(), 2.07m.Usd() }));
		}

		[Test]
		public void Allocate_UnfairAllocation_LastToFirst_LastAmountsAreBigger()
		{
			Allocation allocated = 8.3m.Usd().Allocate(4, RemainderAllocator.LastToFirst);

			Assert.That(allocated, Is.EqualTo(new[] { 2.07m.Usd(), 2.07m.Usd(), 2.08m.Usd(), 2.08m.Usd() }));
		}

		[Test]
		public void Allocate_UnfairAllocation_Random_SomeoneGetsMore()
		{
			Allocation allocated = 8.3m.Eur().Allocate(4, RemainderAllocator.Random);

			Assert.That(allocated.Aggregate((a, b) => a + b), Is.EqualTo(8.3m.Eur()));
		}

		[TestCase(-1)]
		[TestCase(0)]
		public void Allocate_NotEnoughRecipients_Exception(int notEnoughRecipients)
		{
			Assert.That(() => 8m.Usd().Allocate(notEnoughRecipients, RemainderAllocator.Random),
				Throws.InstanceOf<ArgumentOutOfRangeException>());
		}

		[TestCaseSource("ProvidedAllocators")]
		public void Allocate_SingleAllocation_SameQuantity(IRemainderAllocator remainder)
		{
			Allocation allocated = 8.3m.Eur().Allocate(1, remainder);

			Assert.That(allocated, Is.EqualTo(new[] { 8.3m.Eur() }));
		}

		[Test]
		public void Allocate_RemainderAllocatorDidNotAllocateAllTheRemainder_IncompleteAllocation()
		{
			IRemainderAllocator rogue = new RogueRemainderAllocator();

			Allocation allocated = 8.3m.Gbp().Allocate(4, rogue);
			Assert.That(allocated.IsComplete, Is.False);
			Assert.That(allocated.IsQuasiComplete, Is.False);
			Assert.That(allocated.TotalAllocated, Is.EqualTo(8.28m.Gbp()));
		}

		[Test]
		public void Allocate_WholeCurrencies_WholeAllocationsArePossible()
		{
			// 1 yen is the minimal amount, there are no subdivisions
			Allocation allocated = 34m.Jpy().Allocate(4, RemainderAllocator.FirstToLast);

			Assert.That(allocated, Is.EqualTo(new[] { 9m.Jpy(), 9m.Jpy(), 8m.Jpy(), 8m.Jpy() }));
		}

		[Test]
		public void Allocate_NotEnoughToEveryone_FirstToLast_LastOnesGetLess()
		{
			var threeYen = 3m.Jpy();
			Allocation allocated = threeYen.Allocate(4, RemainderAllocator.FirstToLast);

			Assert.That(allocated, Is.EqualTo(new[] { 1m.Jpy(), 1m.Jpy(), 1m.Jpy(), 0m.Jpy() }));
		}

		[Test]
		public void Allocate_NotEnoughToEveryoneAfterEvenAllocation_LastToFirst_FirstOnesGetLess()
		{
			var threeYen = 3m.Jpy();
			Allocation allocated = threeYen.Allocate(2, RemainderAllocator.LastToFirst);

			Assert.That(allocated, Is.EqualTo(new[] { 1m.Jpy(), 2m.Jpy() }));
		}

		[Test]
		public void Allocate_NotEvenTheMinimumForEveryone_QuasiCompleteAllocation()
		{
			var notEvenAYen = 0.3m.Jpy();

			Allocation allocated = notEvenAYen.Allocate(2, RemainderAllocator.FirstToLast);

			Assert.That(allocated.IsComplete, Is.False);
			Assert.That(allocated.IsQuasiComplete, Is.True);
			Assert.That(allocated.TotalAllocated, Is.EqualTo(0m.Jpy()));
		}

		[Test]
		public void Allocate_ResidualRemainders_Exception()
		{
			var residualRemainder = 100.001m.Usd();

			Allocation allocated = residualRemainder.Allocate(2, RemainderAllocator.FirstToLast);

			Assert.That(allocated.IsComplete, Is.False);
			Assert.That(allocated.IsQuasiComplete, Is.True);
			Assert.That(allocated.TotalAllocated, Is.EqualTo(100m.Usd()));
		}
	}
}
