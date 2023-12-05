using NMoneys.Allocations;
using NUnit.Framework.Constraints;
using Testing.Commons.NUnit.Constraints;

namespace NMoneys.Tests.Allocations.Support;

public class CompleteAllocationConstraint : DelegatingConstraint
{
	public Money Allocated { get; set; }

	protected override ConstraintResult matches(object current)
	{
		Delegate = Doez.Satisfy.Conjunction(
			Haz.Prop(nameof(Allocation.IsComplete), Is.True),
			Haz.Prop(nameof(Allocation.IsQuasiComplete), Is.False),
			Haz.Prop(nameof(Allocation.TotalAllocated), Is.EqualTo(Allocated)),
			Haz.Prop(nameof(Allocation.Remainder), Is.EqualTo(Money.Zero(Allocated.CurrencyCode))));

		return Delegate.ApplyTo(current);
	}
}

public partial class Iz : Is
{
	public static CompleteAllocationConstraint Complete(Action<CompleteAllocationConstraint> setup)
	{
		var complete = new CompleteAllocationConstraint();
		setup(complete);
		return complete;
	}
}
