using System.Collections.Generic;

namespace NMoneys.Allocators
{
	internal class LastToFirstAllocator : RemainderAllocatorBase
	{
		public override void Allocate(Money remainder, IList<decimal> alreadyAllocated)
		{
			int index = alreadyAllocated.Count - 1;
			while (!remainder.IsZero())
			{
				apply(ref remainder, alreadyAllocated, index);
				index--;
			}
		}
	}
}
 