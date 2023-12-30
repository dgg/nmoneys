using NMoneys.Allocations;
using NUnit.Framework.Constraints;
using Testing.Commons.NUnit.Constraints;

namespace NMoneys.Tests.Allocations.Support;

public class IncompleteAllocationConstraint : DelegatingConstraint
{
	public Money Allocated { get; set; }
	public Money Remainder { get; set; }

	protected override ConstraintResult matches(object current)
	{
		Delegate = Haz.Prop(nameof(Allocation.IsComplete), Is.False) &
		           Haz.Prop(nameof(Allocation.IsQuasiComplete), Is.False) &
		           Haz.Prop(nameof(Allocation.TotalAllocated), Is.EqualTo(Allocated)) &
		           Haz.Prop(nameof(Allocation.Remainder), Is.EqualTo(Remainder));

		return Delegate.ApplyTo(current);
	}
}

public partial class Iz : Is
{
	public static IncompleteAllocationConstraint Incomplete(Action<IncompleteAllocationConstraint> setup)
	{
		var incomplete = new IncompleteAllocationConstraint();
		setup(incomplete);
		return incomplete;
	}
}
