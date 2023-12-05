namespace NMoneys.Allocations;

internal abstract class RemainderAllocatorBase : IRemainderAllocator
{
	public abstract Allocation Allocate(Allocation allocatedSoFar);

	/// <summary>
	/// Distributes the minimal amount to the specified result.
	/// </summary>
	/// <remarks>Immutable operation: creates other instance of allocation.</remarks>
	protected static Allocation apply(Allocation allocation, int index)
	{
		Money[] results = allocation
			.Select((m, i) => i != index ? m : m + m.MinValue)
			.ToArray();

		return new Allocation(allocation.Allocatable, results);
	}
}
