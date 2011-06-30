using NUnit.Framework;

namespace NMoneys.Tests.CustomConstraints
{
	[TestFixture]
	public class PropertyConstraintTester
	{
		[Test]
		public void Match_MatchingPropertyOfSameType_True()
		{
			var sameType = new PropertyConstraintSubject(3);
			var subject = new PropertyConstraint<PropertyConstraintSubject>(p => p.Property, Is.EqualTo(3));

			Assert.That(subject.Matches(sameType), Is.True);
		}

		[Test]
		public void Match_MatchingPropertyOfDifferentType_True()
		{
			var differentType = new ContainsPropertySubject();
			var subject = new PropertyConstraint<PropertyConstraintSubject>(p => p.Property, Is.Not.Null);

			Assert.That(subject.Matches(differentType), Is.True);
		}

		[Test]
		public void Match_NonExistingProperty_Exception()
		{
			var subject = new PropertyConstraint<PropertyConstraintSubject>(p => p.Property, Is.Not.Null);

			var doesNotContainProperty = new DoesNotContainPropertySubject();
			Assert.That(()=> subject.Matches(doesNotContainProperty), Throws.ArgumentException);
		}

		[Test]
		public void Match_NotMatching_InformativeMessage()
		{
			var notMatching = new PropertyConstraintSubject(3);
			TextMessageWriter writer = new TextMessageWriter();
			var subject = new PropertyConstraint<PropertyConstraintSubject>(p => p.Property, Is.EqualTo(4));
			subject.Matches(notMatching);
			subject.WriteMessageTo(writer);
			Assert.That(writer.ToString(), Is.StringContaining("Property").And.StringContaining("3").And.StringContaining("4"));
		}

		internal class PropertyConstraintSubject
		{
			public PropertyConstraintSubject(int property)
			{
				Property = property;
			}

			public int Property { get; set; }
		}

		internal class ContainsPropertySubject
		{
			public int Property { get; set; }
		}

		internal class DoesNotContainPropertySubject
		{
			public int AnotherProperty { get; set; }
		}
	}

}
