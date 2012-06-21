using System.Collections.Generic;
using System.Linq;

namespace NMoneys.Allocations
{
	internal abstract class RemainderAllocatorBase : IRemainderAllocator
	{
		public abstract void Allocate(Money remainder, IList<Money> alreadyAllocated);
		public abstract Allocation Allocate(Allocation allocatedSoFar);

		/// <summary>
		/// Distributes the minimal amount to the specified result. 
		/// </summary>
		internal protected void apply(ref Money remainder, IList<Money> results, int i)
		{
			results[i] += remainder.MinValue;
			remainder -= remainder.MinValue;
		}

		/// <summary>
		/// Immutable operation: creates other instance of allocation.
		/// </summary>
		internal protected Allocation apply(Allocation allocation, int index)
		{
			Money[] results = allocation
				.Select((m, i) => i != index ? m : m + m.MinValue)
				.ToArray();
			
			return new Allocation(allocation.Allocatable, results);
		}
	}
}