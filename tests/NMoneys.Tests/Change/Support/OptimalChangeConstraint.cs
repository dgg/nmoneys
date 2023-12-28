using NMoneys.Change;
using NUnit.Framework.Constraints;
using Testing.Commons.NUnit.Constraints;

namespace NMoneys.Tests.Change.Support;

public class OptimalChangeConstraint : DelegatingConstraint
{
	public OptimalChangeConstraint(QDenomination[] denominations)
	{
		Delegate = new ConjunctionConstraint(
			Haz.Prop(nameof(OptimalChangeSolution.IsSolution), Is.True),
			new ConstrainedEnumerableConstraint(
				denominations.Select(d => new QuantifiedDenominationConstraint(d))
			));
	}

	protected override ConstraintResult matches(object current)
	{
		return Delegate.ApplyTo(current);
	}
}

public partial class Iz : Is
{
	internal static OptimalChangeConstraint OptimalChange(params QDenomination[] denominations)
	{
		return new OptimalChangeConstraint(denominations);
	}
}
