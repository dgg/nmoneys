using NMoneys.Allocations;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using Testing.Commons;
using Testing.Commons.NUnit.Constraints;

namespace NMoneys.Tests.CustomConstraints
{
	public class NoAllocationConstraint : DelegatingConstraint
	{
		public Money Remainder { get; set; }

		protected override ConstraintResult matches(object current)
		{
			Delegate = Must.Satisfy.Conjunction(
				Must.Have.Property(nameof(Allocation.IsComplete), Is.False),
				Must.Have.Property(nameof(Allocation.IsQuasiComplete), Is.False),
				Must.Have.Property(nameof(Allocation.TotalAllocated), Is.EqualTo(Money.Zero(Remainder.CurrencyCode))),
				Must.Have.Property(nameof(Allocation.Remainder), Is.EqualTo(Remainder)));

			return Delegate.ApplyTo(current);
		}
	}
}