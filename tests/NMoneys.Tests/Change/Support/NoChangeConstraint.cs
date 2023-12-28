using NMoneys.Change;
using NUnit.Framework.Constraints;
using Testing.Commons.NUnit.Constraints;

namespace NMoneys.Tests.Change.Support;

internal class NoChangeConstraint : DelegatingConstraint
{
	public NoChangeConstraint(Money remainder)
	{
		Delegate = new ConjunctionConstraint(
			Haz.Prop(nameof(ChangeSolution.IsSolution), Is.False),
			Is.Empty,
			Haz.Prop(nameof(ChangeSolution.Count), Is.EqualTo(0)),
			Haz.Prop(nameof(ChangeSolution.Remainder), Is.EqualTo(remainder)));
	}

	protected override ConstraintResult matches(object current)
	{
		return Delegate.ApplyTo(current);
	}
}

public partial class Iz : Is
{
	internal static NoChangeConstraint NoChange(Money remainder)
	{
		return new NoChangeConstraint(remainder);
	}
}
