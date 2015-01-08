using System;
using System.Linq;

namespace NMoneys.Allocations
{
	internal class RandomAllocator : RemainderAllocatorBase
	{
		public override Allocation Allocate(Allocation allocatedSoFar)
		{
			var rnd = new Random();

			// the number of recipients that can get an incremental share
			Money remainder = allocatedSoFar.Remainder;
			var numberOfRecipients = (int)(remainder.Amount / remainder.MinValue.Amount);

			// make a list of all indexes, order it randomly
			// and take the number of indexes we need
			var indexes = Enumerable
				.Range(0, allocatedSoFar.Length)
				.OrderBy(x => rnd.Next())
				.Take(numberOfRecipients);

			return indexes.Aggregate(allocatedSoFar, apply);
		}
	}
}