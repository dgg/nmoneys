namespace NMoneys.Allocations;

internal class FirstToLastAllocator : RemainderAllocatorBase
{
	public override Allocation Allocate(Allocation allocatedSoFar)
	{
		uint index = 0;
		Allocation beingAllocated = allocatedSoFar;
		while (!beingAllocated.IsComplete && index < beingAllocated.Count)
		{
			beingAllocated = apply(beingAllocated, index);
			index++;
		}
		return beingAllocated;
	}
}
