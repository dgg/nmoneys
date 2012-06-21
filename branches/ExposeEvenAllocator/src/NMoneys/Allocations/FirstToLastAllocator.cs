using System.Collections.Generic;

namespace NMoneys.Allocations
{
	internal class FirstToLastAllocator : RemainderAllocatorBase
	{
		public override void Allocate(Money remainder, IList<Money> alreadyAllocated)
		{
			int index = 0;
			while (!remainder.IsZero())
			{
				apply(ref remainder, alreadyAllocated, index);
				index++;
			}
		}
		public override Allocation Allocate(Allocation allocatedSoFar)
		{
			int index = 0;
			Allocation beingAllocated = allocatedSoFar;
			while (!beingAllocated.IsComplete && index < beingAllocated.Length)
			{
				beingAllocated = apply(beingAllocated, index);
				index++;
			}
			return beingAllocated;
		}
	}
}