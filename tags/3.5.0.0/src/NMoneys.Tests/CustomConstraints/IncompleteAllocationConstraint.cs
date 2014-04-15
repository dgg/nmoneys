using NMoneys.Allocations;
using NUnit.Framework;

namespace NMoneys.Tests.CustomConstraints
{
	public class IncompleteAllocationConstraint : CustomConstraint<Allocation>
	{
		public Money Allocated { get; set; }
		public Money Remainder { get; set; }

		protected override bool matches(Allocation current)
		{
			_inner = new PropertyConstraint<Allocation>(a => a.IsComplete, Is.False) &
			new PropertyConstraint<Allocation>(a => a.IsQuasiComplete, Is.False) &
			new PropertyConstraint<Allocation>(a => a.TotalAllocated, Is.EqualTo(Allocated)) &
			new PropertyConstraint<Allocation>(a => a.Remainder, Is.EqualTo(Remainder));

			return _inner.Matches(current);
		}
	}
}