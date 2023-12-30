using NMoneys.Allocations;
using NUnit.Framework.Constraints;
using Testing.Commons.NUnit.Constraints;

namespace NMoneys.Tests.Allocations.Support;

public class NoAllocationConstraint : DelegatingConstraint
{
	public Money Remainder { get; set; }

	protected override ConstraintResult matches(object current)
	{
		Delegate = Doez.Satisfy.Conjunction(
			Haz.Prop(nameof(Allocation.IsComplete), Is.False),
			Haz.Prop(nameof(Allocation.IsQuasiComplete), Is.False),
			Haz.Prop(nameof(Allocation.TotalAllocated), Is.EqualTo(Money.Zero(Remainder.CurrencyCode))),
			Haz.Prop(nameof(Allocation.Remainder), Is.EqualTo(Remainder)));

		return Delegate.ApplyTo(current);
	}
}

public partial class Iz : Is
{
	public static NoAllocationConstraint NoAllocation(Action<NoAllocationConstraint> setup)
	{
		var no = new NoAllocationConstraint();
		setup(no);
		return no;
	}
}
