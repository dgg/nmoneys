using NMoneys.Change;
using NUnit.Framework.Constraints;
using Testing.Commons.NUnit.Constraints;

namespace NMoneys.Tests.Change.Support;

public class NotOptimalChangeConstraint : DelegatingConstraint
{
	public NotOptimalChangeConstraint()
	{
		Delegate = new ConjunctionConstraint(
			Haz.Prop(nameof(OptimalChangeSolution.IsSolution), Is.False),
			Is.Empty,
			Haz.Prop(nameof(OptimalChangeSolution.Count), Is.EqualTo(0)));
	}

	protected override ConstraintResult matches(object current)
	{
		return Delegate.ApplyTo(current);
	}
}

public partial class Iz : Is
{
	internal static NotOptimalChangeConstraint NotOptimalChange()
	{
		return new NotOptimalChangeConstraint();
	}
}
