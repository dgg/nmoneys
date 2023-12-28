namespace NMoneys.Allocations;

internal class LastToFirstAllocator : RemainderAllocatorBase
{
	public override Allocation Allocate(Allocation allocatedSoFar)
	{
		uint index = allocatedSoFar.Count - 1;
		Allocation beingAllocated = allocatedSoFar;
		while (!beingAllocated.IsComplete && index < beingAllocated.Count)
		{
			beingAllocated = apply(beingAllocated, index);
			index--;
		}
		return beingAllocated;
	}
}
