using System;
using System.Collections.Generic;
using System.Linq;

namespace NMoneys.Allocation
{
	public class AllocationOrdering
	{
		private readonly Func<IEnumerable<decimal>, IEnumerable<decimal>> _sorter;

		internal IEnumerable<decimal> Order(IEnumerable<decimal> sortable)
		{
			return _sorter(sortable);
		}

		private AllocationOrdering(Func<IEnumerable<decimal>, IEnumerable<decimal>> sorter)
		{
			_sorter = sorter;
		}

		public static AllocationOrdering AsIs = new AllocationOrdering(a => a);
		public static AllocationOrdering LowToHigh = new AllocationOrdering(a => a.OrderBy(x => x));
		public static AllocationOrdering HighToLow = new AllocationOrdering(a => a.OrderByDescending(x => x));
		public static AllocationOrdering Random = new AllocationOrdering(a =>
			{
				var rnd = new Random();
				return a.OrderBy(x => rnd.Next());
			});

		public static AllocationOrdering Custom(Func<IEnumerable<decimal>, IEnumerable<decimal>> sorter)
		{
			return new AllocationOrdering(sorter);
		}
	}
}