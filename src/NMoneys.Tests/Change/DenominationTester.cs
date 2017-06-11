using System;
using NMoneys.Change;
using NUnit.Framework;

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
	}
}