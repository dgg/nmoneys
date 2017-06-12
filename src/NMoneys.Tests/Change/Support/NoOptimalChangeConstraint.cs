using NMoneys.Change;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using Testing.Commons;
using Testing.Commons.NUnit.Constraints;

namespace NMoneys.Tests.Change.Support
{
	internal class NoOptimalChangeConstraint : DelegatingConstraint
	{
		public NoOptimalChangeConstraint()
		{
			Delegate = new ConjunctionConstraint(
				Is.Empty,
				Must.Have.Property(nameof(OptimalChangeSolution.Count), Is.EqualTo(0)));
		}

		protected override ConstraintResult matches(object current)
		{
			return Delegate.ApplyTo(current);
		}
	}
}