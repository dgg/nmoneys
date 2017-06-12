using System.Linq;
using NMoneys.Change;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using Testing.Commons;
using Testing.Commons.NUnit.Constraints;

namespace NMoneys.Tests.Change.Support
{
	internal class IncompleteChangeConstraint : DelegatingConstraint
	{
		public IncompleteChangeConstraint(Money remainder, QDenomination[] denominations)
		{
			Delegate = new ConjunctionConstraint(
				Must.Have.Property(nameof(ChangeSolution.Remainder), Is.EqualTo(remainder)),
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