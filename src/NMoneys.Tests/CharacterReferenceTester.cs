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
			Assert.That(reference.IsEmpty, Is.False);
		}

		public object[] Spec = new[]
		{
			new object[]{ "&cent;", "&cent;", "cent", "&#162;", 162, "¢"},
			new object[]{ "cent", "&cent;", "cent", "&#162;", 162, "¢"},
			new object[]{ "&pound;", "&pound;", "pound", "&#163;", 163, "£"},
			new object[]{ "pound", "&pound;", "pound", "&#163;", 163, "£"},
			new object[]{ "&curren;", "&curren;", "curren", "&#164;", 164, "¤"},
			new object[]{ "curren", "&curren;", "curren", "&#164;", 164, "¤"},
			new object[]{ "&yen;", "&yen;", "yen", "&#165;", 165, "¥"},
			new object[]{ "yen", "&yen;", "yen", "&#165;", 165, "¥"},
			new object[]{ "&fnof;", "&fnof;", "fnof", "&#402;", 402, "ƒ"},
			new object[]{ "fnof", "&fnof;", "fnof", "&#402;", 402, "ƒ"},
			new object[]{ "&euro;", "&euro;", "euro", "&#8364;", 8364, "€"},
			new object[]{ "euro", "&euro;", "euro", "&#8364;", 8364, "€"},
		};

		[Test]
		public void Ctor_NullName_Exception()
		{
			Assert.That(()=> new CharacterReference(null), Throws.InstanceOf<ArgumentNullException>()
				.With.Message.StringContaining("entityName"));
		}

		[Test]
		public void Empty_AccordingToItsSpec()
		{
			Assert.That(CharacterReference.Empty.EntityName, Is.Empty);
			Assert.That(CharacterReference.Empty.EntityNumber, Is.EqualTo("&#00;"));
			Assert.That(CharacterReference.Empty.SimpleName, Is.Empty);
			Assert.That(CharacterReference.Empty.CodePoint, Is.EqualTo(0));
			Assert.That(CharacterReference.Empty.Character, Is.Empty);
			Assert.That(CharacterReference.Empty.IsEmpty, Is.True);
		}

		[Test]
		public void Equals_Null_False()
		{
			Assert.That(new CharacterReference("pound").Equals(null), Is.False);
		}

		[Test]
		public void Equals_Same_True()
		{
			var pound = new CharacterReference("pound");
			Assert.That(pound.Equals(pound), Is.True);
		}

		[Test]
		public void Equals_NotAReference_False()
		{
			Assert.That(new CharacterReference("pound").Equals("pound"), Is.False);
		}

		[Test]
		public void Equals_Equal_True()
		{
			Assert.That(new CharacterReference("pound").Equals(new CharacterReference("pound")), Is.True);
		}

		[Test]
		public void Equals_NotEqual_False()
		{
			Assert.That(new CharacterReference("pound").Equals(new CharacterReference("curren")), Is.False);
		}

		[Test]
		public void Equality_BothNull_True()
		{
			CharacterReference one = null, two = null;
			Assert.That(one == two, Is.True);
		}

		[Test]
		public void Equality_OneNull_False()
		{
			CharacterReference notNull = new CharacterReference("pound");
			Assert.That(notNull == null, Is.False);
			Assert.That(null == notNull, Is.False);
		}

		[Test]
		public void Equality_Equal_True()
		{
			CharacterReference pound = new CharacterReference("pound"), anotherPound= new CharacterReference("pound");
			Assert.That(pound == anotherPound, Is.True);
		}

		[Test]
		public void Equality_NotEqual_False()
		{
			CharacterReference pound = new CharacterReference("pound"), cent = new CharacterReference("cent");
			Assert.That(pound == cent, Is.False);
		}

		[Test]
		public void Inequality_BothNull_False()
		{
			CharacterReference one = null, two = null;
			Assert.That(one != two, Is.False);
		}

		[Test]
		public void Inequality_OneNull_True()
		{
			CharacterReference notNull = new CharacterReference("pound");
			Assert.That(notNull != null, Is.True);
			Assert.That(null != notNull, Is.True);
		}

		[Test]
		public void Inequality_Equal_False()
		{
			CharacterReference pound = new CharacterReference("pound"), anotherPound = new CharacterReference("pound");
			Assert.That(pound != anotherPound, Is.False);
		}

		[Test]
		public void Inequality_NotEqual_True()
		{
			CharacterReference pound = new CharacterReference("pound"), cent = new CharacterReference("cent");
			Assert.That(pound != cent, Is.True);
		}
	}
}
