using NMoneys.Allocations;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using Testing.Commons;
using Testing.Commons.NUnit.Constraints;

namespace NMoneys.Tests.CustomConstraints
{
	public class CompleteAllocationConstraint : DelegatingConstraint
	{
		public Money Allocated { get; set; }

		protected override ConstraintResult matches(object current)
		{
			Delegate = Must.Satisfy.Conjunction(
				Must.Have.Property(nameof(Allocation.IsComplete), Is.True),
				Must.Have.Property(nameof(Allocation.IsQuasiComplete), Is.False),
				Must.Have.Property(nameof(Allocation.TotalAllocated), Is.EqualTo(Allocated)),
				Must.Have.Property(nameof(Allocation.Remainder), Is.EqualTo(Money.Zero(Allocated.CurrencyCode))));

			return Delegate.ApplyTo(current);
		}
	}
}