using NMoneys.Allocations;
using NUnit.Framework;

namespace NMoneys.Tests.CustomConstraints
{
	public class NoAllocationConstraint : CustomConstraint<Allocation>
	{
		public Money Remainder { get; set; }

		protected override bool matches(Allocation current)
		{
			_inner = new PropertyConstraint<Allocation>(a => a.IsComplete, Is.False) &
			new PropertyConstraint<Allocation>(a => a.IsQuasiComplete, Is.False) &
			new PropertyConstraint<Allocation>(a => a.TotalAllocated, Is.EqualTo(Money.Zero(Remainder.CurrencyCode))) &
			new PropertyConstraint<Allocation>(a => a.Remainder, Is.EqualTo(Remainder));

			return _inner.Matches(current);
		}
	}
}