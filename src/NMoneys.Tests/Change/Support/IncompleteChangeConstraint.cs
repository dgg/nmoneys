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
		public IncompleteChangeConstraint(Money remainder, uint totalCount, QDenomination[] denominations)
		{
			Delegate = new ConjunctionConstraint(
				Must.Have.Property(nameof(ChangeSolution.IsSolution), Is.True),
				Must.Have.Property(nameof(ChangeSolution.IsComplete), Is.False),
				Must.Have.Property(nameof(ChangeSolution.Remainder), Is.EqualTo(remainder)),
				Must.Have.Property(nameof(ChangeSolution.TotalCount), Is.EqualTo(totalCount)),
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