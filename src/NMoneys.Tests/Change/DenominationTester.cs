using System;
using NMoneys.Change;
using NUnit.Framework;

namespace NMoneys.Tests.Change
{
	[TestFixture]
	public class DenominationTester
	{
		[Test]
		public void Ctor_WithValue()
		{
			var subject = new Denomination(5m);

			Assert.That(subject.Value, Is.EqualTo(5m));
		}

		[Test]
		public void Ctor_Negative_Exception()
		{
			Assert.That(() => new Denomination(-1m), Throws.InstanceOf<ArgumentOutOfRangeException>());
		}

		[Test]
		public void Ctor_Zero_Exception()
		{
			Assert.That(() => new Denomination(0m), Throws.InstanceOf<ArgumentOutOfRangeException>());
		}
	}
}