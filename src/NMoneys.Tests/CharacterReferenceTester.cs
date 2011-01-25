using System;
using NUnit.Framework;

namespace NMoneys.Tests
{
	[TestFixture]
	public class CharacterReferenceTester
	{
		[Test, TestCaseSource("Spec")]
		public void CanBeConstructed_WithSimpleOrEntityNames(string ctorName, string entityName, string simpleName, string entityNumber, int codePoint, string character)
		{
			var reference = new CharacterReference(ctorName);
			Assert.That(reference.EntityName, Is.EqualTo(entityName));
			Assert.That(reference.EntityNumber, Is.EqualTo(entityNumber));
			Assert.That(reference.SimpleName, Is.EqualTo(simpleName));
			Assert.That(reference.CodePoint, Is.EqualTo(codePoint));
			Assert.That(reference.Character, Is.EqualTo(character));
		}

		public object[] Spec = new[]
		{
			new object[]{ "&pound;", "&pound;", "pound", "&#163;", 163, "£"},
			new object[]{ "pound", "&pound;", "pound", "&#163;", 163, "£"},
			new object[]{ "&curren;", "&curren;", "curren", "&#164;", 164, "¤"},
			new object[]{ "curren", "&curren;", "curren", "&#164;", 164, "¤"}
		};

		[Test]
		public void Ctor_NullName_Exception()
		{
			Assert.That(()=> new CharacterReference(null), Throws.InstanceOf<ArgumentNullException>()
				.With.Message.StringContaining("entityName"));
		}
	}
}
