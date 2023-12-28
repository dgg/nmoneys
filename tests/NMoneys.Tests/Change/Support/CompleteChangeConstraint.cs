using NMoneys.Change;
using NUnit.Framework.Constraints;
using Testing.Commons.NUnit.Constraints;

namespace NMoneys.Tests.Change.Support;

internal class CompleteChangeConstraint : DelegatingConstraint
{
	public CompleteChangeConstraint(uint totalCount, QDenomination[] denominations)
	{
		Delegate = new ConjunctionConstraint(
			Haz.Prop(nameof(ChangeSolution.IsSolution), Is.True),
			Haz.Prop(nameof(ChangeSolution.IsPartial), Is.False),
			Haz.Prop(nameof(ChangeSolution.Remainder), Is.Null),
			Haz.Prop(nameof(ChangeSolution.Count), Is.EqualTo(denominations.Length)),
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
	internal static CompleteChangeConstraint CompleteChange(uint totalCount,
		params QDenomination[] denominations)
	{
		return new CompleteChangeConstraint(totalCount, denominations);
	}
}
