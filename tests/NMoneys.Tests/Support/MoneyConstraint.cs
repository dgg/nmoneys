using NUnit.Framework.Constraints;
using Testing.Commons.NUnit.Constraints;

namespace NMoneys.Tests.Support;

public class MoneyConstraint : DelegatingConstraint
{
	public MoneyConstraint(decimal amount, Currency currency)
	{
		Delegate = new AndConstraint(
			Haz.Prop(nameof(Money.Amount), Is.EqualTo(amount)),
			Haz.Prop(nameof(Money.CurrencyCode), Is.EqualTo(currency.IsoCode)));
	}


	protected override ConstraintResult matches(object current)
	{
		return Delegate.ApplyTo(current);
	}
}
