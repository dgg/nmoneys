namespace NMoneys.Allocations
{
	internal class LastToFirstAllocator : RemainderAllocatorBase
	{
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
 