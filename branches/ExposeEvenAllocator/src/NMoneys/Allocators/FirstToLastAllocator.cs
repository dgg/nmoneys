using System.Collections.Generic;

namespace NMoneys.Allocators
{
	internal class FirstToLastAllocator : RemainderAllocatorBase
	{
		public override void Allocate(Money remainder, IList<decimal> alreadyAllocated)
		{
			int index = 0;
			while (!remainder.IsZero())
			{
				apply(ref remainder, alreadyAllocated, index);
				index++;
			}
		}
	}
}