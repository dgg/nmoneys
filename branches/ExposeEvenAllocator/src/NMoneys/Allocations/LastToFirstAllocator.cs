using System.Collections.Generic;

namespace NMoneys.Allocation
{
	internal class LastToFirstAllocator : RemainderAllocatorBase
	{
		public override void Allocate(Money remainder, IList<Money> alreadyAllocated)
		{
			int index = alreadyAllocated.Count - 1;
			while (!remainder.IsZero())
			{
				apply(ref remainder, alreadyAllocated, index);
				index--;
			}
		}

		public override Allocation Allocate(Allocation allocatedSoFar)
		{
			int index = allocatedSoFar.Length - 1;
			Allocation beingAllocated = allocatedSoFar;
			while (!beingAllocated.IsComplete && index < beingAllocated.Length)
			{
				beingAllocated = apply(beingAllocated, index);
				index--;
			}
			return beingAllocated;
		}
	}
}
 