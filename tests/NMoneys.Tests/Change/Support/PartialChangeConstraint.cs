using NMoneys.Change;
using NUnit.Framework.Constraints;
using Testing.Commons.NUnit.Constraints;

namespace NMoneys.Tests.Change.Support;

internal class PartialChangeConstraint : DelegatingConstraint
{
	public PartialChangeConstraint(Money remainder, uint totalCount, QDenomination[] denominations)
	{
		Delegate = new ConjunctionConstraint(
			Haz.Prop(nameof(ChangeSolution.IsSolution), Is.True),
			Haz.Prop(nameof(ChangeSolution.IsPartial), Is.True),
			Haz.Prop(nameof(ChangeSolution.Remainder), Is.EqualTo(remainder)),
			Haz.Prop(nameof(ChangeSolution.TotalCount), Is.EqualTo(totalCount)),
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
	internal static PartialChangeConstraint PartialChange(Money remainder, uint totalCount, params QDenomination[] denominations)
	{
		return new PartialChangeConstraint(remainder, totalCount, denominations);
	}
}
