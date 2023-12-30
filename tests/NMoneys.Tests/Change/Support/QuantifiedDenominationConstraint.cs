using NMoneys.Change;
using NUnit.Framework.Constraints;
using Testing.Commons.NUnit.Constraints;

namespace NMoneys.Tests.Change.Support;

internal class QuantifiedDenominationConstraint : DelegatingConstraint
{
	public QuantifiedDenominationConstraint(QDenomination denomination) : this(denomination.Quantity, denomination.Denomination) { }
	public QuantifiedDenominationConstraint(uint quantity, decimal denominationValue)
	{
		Delegate = new ConjunctionConstraint(
			Haz.Prop(nameof(QuantifiedDenomination.Quantity), Is.EqualTo(quantity)),
			Haz.Prop(nameof(QuantifiedDenomination.Denomination), Haz.Prop(nameof (Denomination.Value), Is.EqualTo(denominationValue))));
	}
	protected override ConstraintResult matches(object current)
	{
		return Delegate.ApplyTo(current);
	}
}
