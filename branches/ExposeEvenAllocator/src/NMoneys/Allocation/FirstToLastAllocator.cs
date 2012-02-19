using System.Collections.Generic;

namespace NMoneys.Allocation
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
	}
}