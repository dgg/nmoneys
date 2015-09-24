using NMoneys.Allocations;
using NUnit.Framework;
using Testing.Commons.NUnit.Constraints;

namespace NMoneys.Tests.CustomConstraints
{
	public class CompleteAllocationConstraint : CustomConstraint<Allocation>
	{
		public Money Allocated { get; set; }

		protected override bool matches(Allocation current)
		{
			_inner = new LambdaPropertyConstraint<Allocation>(a => a.IsComplete, Is.True) &
			new LambdaPropertyConstraint<Allocation>(a => a.IsQuasiComplete, Is.False) &
			new LambdaPropertyConstraint<Allocation>(a => a.TotalAllocated, Is.EqualTo(Allocated)) &
			new LambdaPropertyConstraint<Allocation>(a => a.Remainder, Is.EqualTo(Money.Zero(Allocated.CurrencyCode)));

			return _inner.Matches(current);
		}
	}
}