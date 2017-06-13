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
		public CompleteChangeConstraint(uint totalCount, QDenomination[] denominations)
		{
			Delegate = new ConjunctionConstraint(
				Must.Have.Property(nameof(ChangeSolution.IsSolution), Is.True),
				Must.Have.Property(nameof(ChangeSolution.IsComplete), Is.True),
				Must.Have.Property(nameof(ChangeSolution.Remainder), Is.Null),
				Must.Have.Property(nameof(ChangeSolution.Count), Is.EqualTo(denominations.Length)),
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