using System.Linq;
using NMoneys.Change;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using Testing.Commons;
using Testing.Commons.NUnit.Constraints;

namespace NMoneys.Tests.Change.Support
{
	internal class CompleteChangeConstraint : DelegatingConstraint
	{
		public CompleteChangeConstraint(CurrencyIsoCode code, QDenomination[] denominations)
		{
			Delegate = new ConjunctionConstraint(
				Must.Have.Property(nameof(GetChangeSolution.Remainder), Is.EqualTo(Money.Zero(code))),
				new ConstrainedEnumerable(
					denominations.Select(d => new QuantifiedDenominationConstraint(d))
					));
		}

		protected override ConstraintResult matches(object current)
		{
			return Delegate.ApplyTo(current);
		}
	}
}