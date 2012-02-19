using System.Collections.Generic;

namespace NMoneys.Allocation
{
	internal abstract class RemainderAllocatorBase : IRemainderAllocator
	{
		public abstract void Allocate(Money remainder, IList<decimal> alreadyAllocated);

		/// <summary>
		/// Distributes the minimal amount to the specified result. 
		/// </summary>
		internal protected void apply(ref Money remainder, IList<decimal> results, int i)
		{
			results[i] += remainder.MinValue.Amount;
			remainder -= remainder.MinValue;
		}
	}
}