using System;
using System.Globalization;
using NMoneys.Change;
using NUnit.Framework;
using Testing.Commons.Globalization;

namespace NMoneys.Tests.Change
{
	[TestFixture]
	public class DenominationTester
	{
		[Test]
		public void Ctor_Positive_SetValue()
		{
			var subject = new Denomination(5m);

			Assert.That(subject.Value, Is.EqualTo(5m));
		}

		[Test]
		[TestCase(0), TestCase(-1)]
		public void Ctor_NotPositive_Exception(decimal notPositive)
		{
			Assert.That(() => new Denomination(notPositive), Throws.InstanceOf<ArgumentOutOfRangeException>());
		}

		[Test]
		public void DefaultCtor_YieldsNonDefaultValue()
		{
			var subject = new Denomination();

			Assert.That(subject.Value, Is.Not.EqualTo(default(decimal)).And.EqualTo(1));
		}

		[Test]
		public void ToString_ValueString_AsPerCurrentCulture()
		{
			var pointTwo = new Denomination(.2m);

			using (CultureReseter.Set("en-US"))
			{
				Assert.That(pointTwo.ToString(), Is.EqualTo("0.2"), "US decimals with a dot");
			}

			using (CultureReseter.Set("da-DK"))
			{
				Assert.That(pointTwo.ToString(), Is.EqualTo("0,2"), "DK decimals with a comma");
			}
		}

		[Test]
		public void ToString_CanReceiveCustomFormatsAndProviders()
		{
			var pointTwo = new Denomination(.2m);
			var snailDecimalSeparator = (NumberFormatInfo)CultureInfo.InvariantCulture.NumberFormat.Clone();
			snailDecimalSeparator.NumberDecimalSeparator = "@";

			Assert.That(pointTwo.ToString(".000", snailDecimalSeparator), Is.EqualTo("@200"));
		}
	}
}