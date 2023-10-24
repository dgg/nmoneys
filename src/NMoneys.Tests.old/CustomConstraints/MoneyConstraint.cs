using NUnit.Framework;
using NUnit.Framework.Constraints;
using Testing.Commons;
using Testing.Commons.NUnit.Constraints;

namespace NMoneys.Tests.CustomConstraints
{
	internal class MoneyConstraint : DelegatingConstraint
	{
		public MoneyConstraint(decimal amount, Currency currency)
		{
			Delegate = new AndConstraint(
				Must.Have.Property(nameof(Money.Amount), Is.EqualTo(amount)),
				Must.Have.Property(nameof(Money.CurrencyCode), Is.EqualTo(currency.IsoCode)));
		}
		

		protected override ConstraintResult matches(object current)
		{
			return Delegate.ApplyTo(current);
		}
	}
}
