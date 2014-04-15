using NMoneys.Allocations;

namespace NMoneys.Tests.Support
{
	internal class RemainderAllocatorSpy : IRemainderAllocator
	{
		public bool AskedToAllocate { get; private set; }
		
		public Allocation Allocate(Allocation allocationSoFar)
		{
			AskedToAllocate = true;
			// need to do the real thing to fulfill contract
			return RemainderAllocator.FirstToLast.Allocate(allocationSoFar);
		}
	}

	internal class RogueRemainderAllocator : IRemainderAllocator
	{
		public Allocation Allocate(Allocation allocationSoFar)
		{
			// does not touch the amounts already allocated, thus not doing its job
			return allocationSoFar;
		}
	}
}
