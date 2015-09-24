using NMoneys.Allocations;
using NUnit.Framework;
using Testing.Commons.NUnit.Constraints;

namespace NMoneys.Tests.CustomConstraints
{
	public class QuasiCompleteAllocationConstraint : DelegatingConstraint<Allocation>
	{
		public Money Allocated { get; set; }
		public Money Remainder { get; set; }

		protected override bool matches(Allocation current)
		{
			Delegate = new LambdaPropertyConstraint<Allocation>(a => a.IsComplete, Is.False) &
				new LambdaPropertyConstraint<Allocation>(a => a.IsQuasiComplete, Is.True) &
				new LambdaPropertyConstraint<Allocation>(a => a.TotalAllocated, Is.EqualTo(Allocated)) &
				new LambdaPropertyConstraint<Allocation>(a => a.Remainder, Is.EqualTo(Remainder));

			return Delegate.Matches(current);
		}
	}
}