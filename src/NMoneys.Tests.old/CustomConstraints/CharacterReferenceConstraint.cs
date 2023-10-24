using NUnit.Framework.Constraints;
using Testing.Commons;
using Testing.Commons.NUnit.Constraints;

namespace NMoneys.Tests.CustomConstraints
{
	internal class CharacterReferenceConstraint : DelegatingConstraint
	{
		public CharacterReferenceConstraint(string entityName, string entityNumber)
		{
			Delegate = Must.Have.Property(nameof(CharacterReference.EntityName), new EqualConstraint(entityName)) &
				Must.Have.Property(nameof(CharacterReference.EntityNumber), new EqualConstraint(entityNumber));
		}

		protected override ConstraintResult matches(object current)
		{
			return Delegate.ApplyTo(current);
		}
	}
}
