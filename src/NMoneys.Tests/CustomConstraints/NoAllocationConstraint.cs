using NMoneys.Allocations;
using NUnit.Framework;
using Testing.Commons.NUnit.Constraints;

namespace NMoneys.Tests.CustomConstraints
{
	public class NoAllocationConstraint : CustomConstraint<Allocation>
	{
		public Money Remainder { get; set; }

		protected override bool matches(Allocation current)
		{
			_inner = new LambdaPropertyConstraint<Allocation>(a => a.IsComplete, Is.False) &
			new LambdaPropertyConstraint<Allocation>(a => a.IsQuasiComplete, Is.False) &
			new LambdaPropertyConstraint<Allocation>(a => a.TotalAllocated, Is.EqualTo(Money.Zero(Remainder.CurrencyCode))) &
			new LambdaPropertyConstraint<Allocation>(a => a.Remainder, Is.EqualTo(Remainder));

			return _inner.Matches(current);
		}
	}
}