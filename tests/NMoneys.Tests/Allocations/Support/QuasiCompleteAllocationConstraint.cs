using NMoneys.Allocations;
using NUnit.Framework.Constraints;
using Testing.Commons.NUnit.Constraints;

namespace NMoneys.Tests.Allocations.Support;

public class QuasiCompleteAllocationConstraint : DelegatingConstraint
{
	public Money Allocated { get; set; }
	public Money Remainder { get; set; }

	protected override ConstraintResult matches(object current)
	{
		Delegate =  Doez.Satisfy.Conjunction(

			Haz.Prop(nameof(Allocation.IsComplete), Is.False),
			Haz.Prop(nameof(Allocation.IsQuasiComplete), Is.True),
			Haz.Prop(nameof(Allocation.TotalAllocated), Is.EqualTo(Allocated)),
			Haz.Prop(nameof(Allocation.Remainder), Is.EqualTo(Remainder)));

		return Delegate.ApplyTo(current);
	}
}

public partial class Iz : Is
{
	public static QuasiCompleteAllocationConstraint QuasiComplete(Action<QuasiCompleteAllocationConstraint> setup)
	{
		var quasiComplete = new QuasiCompleteAllocationConstraint();
		setup(quasiComplete);
		return quasiComplete;
	}
}
