using System.Collections.Generic;
using NMoneys.Allocations;

namespace NMoneys.Tests.Support
{
	internal class RemainderAllocatorSpy : IRemainderAllocator
	{
		public bool AskedToAllocate { get; private set; }
		public void Allocate(Money remainder, IList<Money> alreadyAllocated)
		{
			AskedToAllocate = true;
			// need to do the real thing to fulfill contract
			RemainderAllocator.FirstToLast.Allocate(remainder, alreadyAllocated);
		}

		public Allocations.Allocation Allocate(Allocations.Allocation allocationSoFar)
		{
			throw new System.NotImplementedException();
		}
	}

	internal class RogueRemainderAllocator : IRemainderAllocator
	{
		public void Allocate(Money remainder, IList<Money> alreadyAllocated)
		{
 			// does not touch the amounts already allocated, thus not doing its job
		}

		public Allocations.Allocation Allocate(Allocations.Allocation allocationSoFar)
		{
			return allocationSoFar;
		}
	}
}
