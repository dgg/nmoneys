using NUnit.Framework.Constraints;
using Testing.Commons.NUnit.Constraints;

namespace NMoneys.Tests.CustomConstraints
{
	internal class CharacterReferenceConstraint : CustomConstraint<CharacterReference>
	{
		public CharacterReferenceConstraint(string entityName, string entityNumber)
		{
			_inner = new LambdaPropertyConstraint<CharacterReference>(r => r.EntityName, new EqualConstraint(entityName)) &
				new LambdaPropertyConstraint<CharacterReference>(r => r.EntityNumber, new EqualConstraint(entityNumber));
		}

		protected override bool matches(CharacterReference current)
		{
			return _inner.Matches(current);
		}
	}
}
