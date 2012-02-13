using System.Collections.Generic;
using NMoneys.Allocators;

namespace NMoneys.Tests.Support
{
	internal class RemainderAllocatorSpy : IRemainderAllocator
	{
		public bool AskedToAllocate { get; private set; }
		public void Allocate(Money remainder, IList<decimal> alreadyAllocated)
		{
			AskedToAllocate = true;
			// need to do the real thing to fulfill contract
			RemainderAllocator.FirstToLast.Allocate(remainder, alreadyAllocated);
		}
	}

	internal class RogueRemainderAllocator : IRemainderAllocator
	{
		public void Allocate(Money remainder, IList<decimal> alreadyAllocated)
		{
 			// does not touch the amounts already allocated, thus not doing its job
		}
	}
}
