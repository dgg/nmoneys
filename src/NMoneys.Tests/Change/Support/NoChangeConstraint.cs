using NMoneys.Change;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using Testing.Commons;
using Testing.Commons.NUnit.Constraints;

namespace NMoneys.Tests.Change.Support
{
	internal class NoChangeConstraint : DelegatingConstraint
	{
		public NoChangeConstraint(Money remainder)
		{
			Delegate = new ConjunctionConstraint(
				Must.Have.Property(nameof(ChangeSolution.IsSolution), Is.False)/*,
				Is.Empty,
				Must.Have.Property(nameof(ChangeSolution.Count), Is.EqualTo(0)),
				Must.Have.Property(nameof(ChangeSolution.Remainder), Is.EqualTo(remainder))*/);
		}

		protected override ConstraintResult matches(object current)
		{
			return Delegate.ApplyTo(current);
		}
	}
}