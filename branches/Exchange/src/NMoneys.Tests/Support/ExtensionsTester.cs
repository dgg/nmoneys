using System;
using System.Collections.Generic;
using System.Globalization;
using NUnit.Framework;
using NMoneys.Support.Ext;

namespace NMoneys.Tests.Support
{
	[TestFixture]
	public class ExtensionsTester
	{
		#region FromUnicodes

		[Test]
		public void FromUnicodes_NullUnicodes_Empty()
		{
			string[] @null = null;

			Assert.That(@null.FromUnicodes(), Is.Empty);
		}

		[Test]
		public void FromUnicodes_EmptyUnicodes_Empty()
		{
			string[] @null = new string[0];

			Assert.That(@null.FromUnicodes(), Is.Empty);
		}

		[TestCase(new[] { "36" }, "$")]
		[TestCase(new[] { "8364" }, "€")]
		[TestCase(new[] { "36", "8364" }, "$€")]
		[TestCase(new[] { "36", "8364", "65" }, "$€A")]
		public void FromUnicodes_UnicodesArray_StringFromUnicode(string[] unicodes, string expected)
		{
			Assert.That(unicodes.FromUnicodes(), Is.EqualTo(expected));
		}

		[TestCase(new[] { "notAnUnicode" }, null)]
		[TestCase(new[] { "1,2" }, null)]
		public void FromUnicodes_WrongUnicode_Exception(string[] wrongUnicodes, string toAvoidCompilationError)
		{
			Assert.That(() => wrongUnicodes.FromUnicodes(), Throws.InstanceOf<FormatException>());
		}

		#endregion

		[TestCase(null, true)]
		[TestCase("", true)]
		[TestCase("abc", false)]
		public void IsEmpty_Combinations(string input, bool isEmpty)
		{
			Assert.That(input.IsEmpty(), Is.EqualTo(isEmpty));
		}

		#region ToDelimitedString

		[Test]
		public void ToDelimitedString_PopulatedCollections_DelimitedString()
		{
			var enumerable = new[] { 1, 2, 3, 4 };
			Func<int, string> doubleToStringFunction = i => (i * 2).ToString();
			Assert.That(enumerable.ToDelimitedString(doubleToStringFunction), Is.EqualTo("2, 4, 6, 8"));
		}

		[Test]
		public void ToDelimitedString_NullEnumerable_Null()
		{
			IEnumerable<string> @null = null;
			Assert.That(@null.ToDelimitedString(s => string.Empty), Is.Null);
		}

		[Test]
		public void ToDelimitedString_EmptyEnumerable_Empty()
		{
			IEnumerable<string> empty = new string[0];
			Assert.That(empty.ToDelimitedString(s => string.Empty), Is.Empty);
		}

		#endregion

		#region TranslateNegativePattern

		[TestCase(0, 0)]
		[TestCase(1, 1)]
		[TestCase(2, 1)]
		[TestCase(3, 3)]
		[TestCase(4, 0)]
		[TestCase(5, 1)]
		[TestCase(6, 3)]
		[TestCase(7, 3)]
		[TestCase(8, 1)]
		[TestCase(9, 2)]
		[TestCase(10, 4)]
		[TestCase(11, 3)]
		[TestCase(12, 1)]
		[TestCase(13, 3)]
		public void TranslateNegativePattern_MaintainsLayout(int currencyNegativePattern, int numberNegativePattern)
		{
			NumberFormatInfo nf = new NumberFormatInfo
			{
				CurrencyNegativePattern = currencyNegativePattern,
				NumberNegativePattern = numberNegativePattern,
				// to allow similar numbers and currencies
				CurrencySymbol = string.Empty,
				CurrencyDecimalDigits = 0,
				NumberDecimalDigits = 0
			};

			string currencyFormatted = (-1).ToString("C", nf).Trim();
			string numberFormatted = (-1).ToString("N", nf).Trim();

			Assert.That(currencyFormatted, Is.EqualTo(numberFormatted));
		}

		[TestCase(14, 0)]
		[TestCase(15, 0)]
		public void TranslateNegativePattern_MaintainsLayout_InSpecialCases(int currencyNegativePattern, int numberNegativePattern)
		{
			NumberFormatInfo nf = new NumberFormatInfo
			{
				CurrencyNegativePattern = currencyNegativePattern,
				NumberNegativePattern = numberNegativePattern,
				// to allow similar numbers and currencies
				CurrencySymbol = string.Empty,
				CurrencyDecimalDigits = 0,
				NumberDecimalDigits = 0
			};

			// an extra space is added between the number and the currency symbol
			// it cannot be properly translated without further manipulation
			string currencyFormatted = (-1).ToString("C", nf).Replace(" ", string.Empty);
			string numberFormatted = (-1).ToString("N", nf).Replace(" ", string.Empty);

			Assert.That(currencyFormatted, Is.EqualTo(numberFormatted));
		}

		#endregion


	}
}
