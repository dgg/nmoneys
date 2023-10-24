using System;
using NMoneys.Support;
using NUnit.Framework;

namespace NMoneys.Tests.Support
{
	[TestFixture]
	public class EnumerationTester
	{
		#region AssertDefined

		// ReSharper disable AccessToModifiedClosure
		[Test]
		public void AssertDefined_DefinedEnum_NoException()
		{
			Test1 defined = Test1.Value1;
			Assert.That(() => Enumeration.AssertDefined(defined), Throws.Nothing);

			defined = (Test1)1;
			Assert.That(() => Enumeration.AssertDefined(defined), Throws.Nothing);
		}
		// ReSharper restore AccessToModifiedClosure

		[Test]
		public void AssertDefined_NotDefinedEnum_Exception()
		{
			Test1 notDefined = (Test1)50;
			Assert.That(() => Enumeration.AssertDefined(notDefined), Throws.ArgumentException
				.With.Message.Contains("50")
				.And.With.Message.Contains(typeof(Test1).Name));
		}

		[Test]
		public void AssertDefined_DefinedValue_NoException()
		{
			Assert.That(() => Enumeration.AssertDefined<Test1>("Value1"), Throws.Nothing);
		}

		[Test]
		public void AssertDefined_NotDefinedValue_Exception()
		{
			// for strings, use the representation
			Assert.That(() => Enumeration.AssertDefined<Test1>("notDefined"),
				Throws.ArgumentException.With.Message.Contains("notDefined"));
		}

		[Test]
		public void AssertDefined_NumericalValueInsteadOfRepresentation_NoException()
		{
			// for strings, use the representation
			Assert.That(() => Enumeration.AssertDefined<Test1>("1"), Throws.Nothing);
		}

		[Test]
		public void AssertDefined_WrongCase_Exception()
		{
			Assert.That(() => Enumeration.AssertDefined<Test1>("value1"),
				Throws.InstanceOf<ArgumentException>().With.Message.Contains("value1"));
		}

		#endregion

		#region Parse / TryParse

		[Test]
		public void Parse_ExistingValue_Value()
		{
			Assert.That(Enumeration.Parse<AttributeTargets>("Enum"), Is.EqualTo(AttributeTargets.Enum));
		}

		[Test]
		public void Parse_NonExistingValue_Exception()
		{
			Assert.That(() => Enumeration.Parse<AttributeTargets>("nonExisting"), Throws.ArgumentException);
		}

		[Test]
		public void Parse_WrongCaseValue_Exception()
		{
			Assert.That(() => Enumeration.Parse<AttributeTargets>("EnuM"), Throws.ArgumentException);
		}

		[Test]
		public void TryParse_ExistingValue_True()
		{
			AttributeTargets? existing;
			Assert.That(Enumeration.TryParse("Enum", out existing), Is.True);
			Assert.That(existing, Is.EqualTo(AttributeTargets.Enum));
		}

		[Test]
		public void TryParse_ExistingNumericValue_True()
		{
			AttributeTargets? existing;
			Assert.That(Enumeration.TryParse("1", out existing), Is.True);
			Assert.That(existing, Is.EqualTo(AttributeTargets.Assembly));
		}

		[Test]
		public void TryParse_NonExistingValue_False()
		{
			bool parsed = false;
			AttributeTargets? nonExisting = null;

			Assert.DoesNotThrow(() => parsed = Enumeration.TryParse("nonExisting", out nonExisting));
			Assert.That(parsed, Is.False);
			Assert.That(nonExisting, Is.Null);
		}

		[Test]
		public void TryParse_NonExistingNumericValue_False()
		{
			bool parsed = false;
			AttributeTargets? nonExisting = null;

			Assert.DoesNotThrow(() => parsed = Enumeration.TryParse("9999", out nonExisting));
			Assert.That(parsed, Is.False);
			Assert.That(nonExisting, Is.Null);
		}

		[Test]
		public void TryParse_WrongCaseValue_False()
		{
			bool parsed = false;
			AttributeTargets? nonExisting = null;

			Assert.DoesNotThrow(() => parsed = Enumeration.TryParse("EnuM", out nonExisting));
			Assert.That(parsed, Is.False, "case sentitive");
			Assert.That(nonExisting, Is.Null);
		}

		[Test]
		public void TryParse_EmptyValue_False()
		{
			bool parsed = false;
			AttributeTargets? nonExisting = null;

			Assert.DoesNotThrow(() => parsed = Enumeration.TryParse(string.Empty, out nonExisting));
			Assert.That(parsed, Is.False);
			Assert.That(nonExisting, Is.Null);

			Assert.DoesNotThrow(() => parsed = Enumeration.TryParse(null, out nonExisting));
			Assert.That(parsed, Is.False);
			Assert.That(nonExisting, Is.Null);
		}

		#endregion

		#region GetAttribute / TryGetAttribute

		[Test]
		public void GetAttribute_ExistingAttribute_Instance()
		{
			DisplayNameAttribute subject = Enumeration.GetAttribute<Test1, DisplayNameAttribute>(Test1.Value3);
			Assert.That(subject, Is.Not.Null);
			Assert.That(subject.DisplayName, Is.EqualTo("displayName : Test1 - Value3"));
		}

		[Test]
		public void GetAttribute_NonExistingAttribute_Exception()
		{
			Assert.That(() => Enumeration.GetAttribute<Test1, DisplayNameAttribute>(Test1.Value1), Throws.InstanceOf<InvalidOperationException>());
		}

		[Test]
		public void TryGetAttribute_ExistingAttribute_True()
		{
			DisplayNameAttribute attribute;

			Assert.That(Enumeration.TryGetAttribute(Test1.Value3, out attribute), Is.True);
			Assert.That(attribute.DisplayName, Is.EqualTo("displayName : Test1 - Value3"));
		}

		[Test]
		public void TryGetAttribute_NonExistingAttribute_False()
		{
			DisplayNameAttribute attribute = new DisplayNameAttribute("asd");
			bool success = true;

			Assert.DoesNotThrow(() => success = Enumeration.TryGetAttribute(Test1.Value1, out attribute));
			Assert.That(success, Is.False);
			Assert.That(attribute, Is.Null);
		}

		#endregion

		#region Compare

		[Test]
		public void Comparer_Equal_True()
		{
			Test1 x = Test1.Value1, y = Test1.Value1;
			Assert.That(Enumeration.Comparer<Test1>.Instance.Equals(x, y), Is.True);
		}

		[Test]
		public void Comparer_NotEqual_False()
		{
			Test1 x = Test1.Value1, y = (Test1)(-1);
			Assert.That(Enumeration.Comparer<Test1>.Instance.Equals(x, y), Is.False);
		}

		[Test]
		public void Comparer_NotEnum_Throws()
		{
			Assert.That(() => Enumeration.Comparer<int>.Instance, Throws.InstanceOf<TypeInitializationException>().With
				.InnerException.TypeOf<ArgumentException>());
		}

		#endregion

		[Test]
		public void GetValues_GetsAllValuesAsTyped()
		{
			Test1[] values = Enumeration.GetValues<Test1>();
			Assert.That(values, Is.EqualTo(new[] { Test1.Value1, Test1.Value2, Test1.Value3 }));
		}

		#region Issue 16. Case sensitivity. Parsing methods are case sensitive

		[Test]
		public void Parse_IsCaseSensitive()
		{
			Assert.That(() => Enumeration.Parse<AttributeTargets>("ENUM"), Throws.ArgumentException);
			Assert.That(Enumeration.Parse<AttributeTargets>("Enum"), Is.EqualTo(AttributeTargets.Enum));
		}

		[Test]
		public void TryParse_IsCaseSensitive()
		{
			AttributeTargets? notParsed;
			Assert.That(Enumeration.TryParse("ENUM", out notParsed), Is.False);
			Assert.That(notParsed, Is.Null);
		}

		#endregion
	}

	[AttributeUsage(AttributeTargets.Field)]
	internal sealed class DisplayNameAttribute : Attribute
	{
		public DisplayNameAttribute(string displayName)
		{
			_displayName = displayName;
		}

		private readonly string _displayName;
		public string DisplayName => _displayName;
	}

	public enum Test1
	{
		[System.Diagnostics.CodeAnalysis.SuppressMessage("cate", "check")]
		Value1,
		Value2,
		[DisplayName("displayName : Test1 - Value3")]
		Value3
	}
}
