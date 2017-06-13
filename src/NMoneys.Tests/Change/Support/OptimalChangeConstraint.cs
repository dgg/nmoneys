using System.Linq;
using NMoneys.Change;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using Testing.Commons;
using Testing.Commons.NUnit.Constraints;

namespace NMoneys.Tests.Change.Support
{
	internal class OptimalChangeConstraint : DelegatingConstraint
	{
		public OptimalChangeConstraint(QDenomination[] denominations)
		{
			Delegate = new ConjunctionConstraint(
				Must.Have.Property(nameof(OptimalChangeSolution.IsSolution), Is.True),
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