using System;
using System.ComponentModel;
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
			Assert.That(() => Enumeration.AssertDefined(notDefined), Throws.InstanceOf<InvalidEnumArgumentException>()
				.With.Property("Message").StringContaining("50")
				.And.With.Property("Message").StringContaining(typeof(Test1).Name));
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
				Throws.InstanceOf<InvalidEnumArgumentException>().With.Property("Message").StringContaining("notDefined"));
		}

		[Test]
		public void AssertDefined_NumericalValueInsteadOfRepresentation_Exception()
		{
			// for strings, use the representation
			Assert.That(() => Enumeration.AssertDefined<Test1>("1"),
				Throws.InstanceOf<InvalidEnumArgumentException>().With.Property("Message").StringContaining("1"));
		}

		[Test]
		public void AssertDefined_WrongCase_Exception()
		{
			Assert.That(() => Enumeration.AssertDefined<Test1>("value1"),
				Throws.InstanceOf<InvalidEnumArgumentException>().With.Property("Message").StringContaining("value1"));
		}

		#endregion

		#region Parse / TryParse

		[Test]
		public void Parse_ExistingValue_Value()
		{
			Assert.That(Enumeration.Parse<PlatformID>("Unix"), Is.EqualTo(PlatformID.Unix));
		}

		[Test]
		public void Parse_NonExistingValue_Exception()
		{
			Assert.That(() => Enumeration.Parse<PlatformID>("nonExisting"), Throws.InstanceOf<InvalidEnumArgumentException>());
		}

		[Test]
		public void Parse_WrongCaseValue_Exception()
		{
			Assert.That(() => Enumeration.Parse<PlatformID>("UniX"), Throws.InstanceOf<InvalidEnumArgumentException>());
		}

		[Test]
		public void TryParse_ExistingValue_True()
		{
			PlatformID? existing;
			Assert.That(Enumeration.TryParse("Unix", out existing), Is.True);
			Assert.That(existing, Is.EqualTo(PlatformID.Unix));
		}

		[Test]
		public void TryParse_NonExistingValue_False()
		{
			bool parsed = false;
			PlatformID? nonExisting = null;

			Assert.DoesNotThrow(() => parsed = Enumeration.TryParse("nonExisting", out nonExisting));
			Assert.That(parsed, Is.False);
			Assert.That(nonExisting, Is.Null);
		}

		[Test]
		public void TryParse_WrongCaseValue_False()
		{
			bool parsed = false;
			PlatformID? nonExisting = null;

			Assert.DoesNotThrow(() => parsed = Enumeration.TryParse("UnIx", out nonExisting));
			Assert.That(parsed, Is.False, "case sentitive");
			Assert.That(nonExisting, Is.Null);
		}

		[Test]
		public void TryParse_EmptyValue_False()
		{
			bool parsed = false;
			PlatformID? nonExisting = null;

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
			Assert.That(Enumeration.Comparer<Test1>().Equals(x, y), Is.True);
		}

		[Test]
		public void Comparer_NotEqual_False()
		{
			Test1 x = Test1.Value1, y = (Test1)(-1);
			Assert.That(Enumeration.Comparer<Test1>().Equals(x, y), Is.False);
		}

		[Test]
		public void Comparer_NotEnum_Throws()
		{
			Assert.That(() => Enumeration.Comparer<int>(), Throws.ArgumentException);
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
			Assert.That(() => Enumeration.Parse<PlatformID>("UNIX"), Throws.InstanceOf<InvalidEnumArgumentException>());
			Assert.That(Enumeration.Parse<PlatformID>("Unix"), Is.EqualTo(PlatformID.Unix));
		}

		[Test]
		public void TryParse_IsCaseSensitive()
		{
			PlatformID? notParsed;
			Assert.That(Enumeration.TryParse("UNIX", out notParsed), Is.False);
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
		public string DisplayName
		{
			get { return _displayName; }
		}
	}

	public enum Test1
	{
		[System.ComponentModel.Description("Test1 - Value1")]
		Value1,
		Value2,
		[DisplayName("displayName : Test1 - Value3")]
		Value3
	}
}
