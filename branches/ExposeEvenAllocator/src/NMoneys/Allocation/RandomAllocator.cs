using System;
using System.Collections.Generic;
using System.Linq;

namespace NMoneys.Allocation
{
	internal class RandomAllocator : RemainderAllocatorBase
	{
		public override void Allocate(Money remainder, IList<Money> alreadyAllocated)
		{
			var rnd = new Random();

			// the number of recipients that can get an incremental share
			var numberOfRecipients = (int)(remainder.Amount / remainder.MinValue.Amount);

			// make a list of all indexes, order it randomly
			// and take the number of indexes we need
			var indexes = Enumerable
				.Range(0, alreadyAllocated.Count)
				.OrderBy(x => rnd.Next())
				.Take(numberOfRecipients);

			foreach (var i in indexes)
			{
				apply(ref remainder, alreadyAllocated, i);
			}
		}
	}
}