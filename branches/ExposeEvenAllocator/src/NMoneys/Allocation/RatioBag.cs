using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NMoneys.Support;

namespace NMoneys.Allocation
{
	/// <summary>
	/// Maintains a list of ratios suitable for use in an allocation when the
	/// sum of all items is exactly equal to one (100%). No individual it may
	/// be less than zero or greater than one.
	/// </summary>
	public class RatioBag : IEnumerable<decimal>
	{
		#region Creation

		private readonly decimal[] _ratios, _original;

		public RatioBag(AllocationOrdering ordering, params decimal[] ratios)
		{
			Guard.AgainstNullArgument("ordering", ordering);
			new Range<decimal>(0m.Close(), 1m.Close()).AssertArgument("ratios", ratios);
			assertAllocatable(ratios);

			Ordering = ordering;
			_original = ratios;
			_ratios = Ordering.Order(ratios).ToArray();
		}

		private void assertAllocatable(IEnumerable<decimal> ratios)
		{
			decimal sum = ratios.Sum();
			if (!sum.Equals(1))
			{
				throw new ArgumentOutOfRangeException("ratios", sum, "Ratios have to sum up 1.0.");
			}
		}

		public RatioBag(params decimal[] ratios) : this(AllocationOrdering.AsIs, ratios) { }

		#endregion

		#region collection-like methods

		public IEnumerator<decimal> GetEnumerator() { return ((IEnumerable<decimal>)_ratios).GetEnumerator(); }
		IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }

		public int Count { get { return _ratios.Length; } }

		public decimal this[int index]
		{
			get { return _ratios[index]; }
		}

		#endregion

		public override string ToString()
		{
			return "<" + string.Join(", ", _ratios.Select(r => r.ToString()).ToArray()) + ">";
		}

		#region Ordering

		public AllocationOrdering Ordering { get; private set; }

		public decimal[] Original { get { return _original; } }

		#endregion



	}
}
