using System;
using NMoneys.Allocations;
using NUnit.Framework;

namespace NMoneys.Tests.Allocation
{
	[TestFixture]
	public partial class RatioTester
	{
		[TestCaseSource("properValues")]
		public void Ctor_ProperRatio_ValueSet(decimal properValue)
		{
			var proper = new Ratio(properValue);

			Assert.That(proper.Value, Is.EqualTo(properValue));
		}

		protected decimal[] properValues = new[] {0m, .001m, .5m, .73m, 1m};

		[Test, SetCulture("")]
		public void Ctor_NegativeRatio_Exception()
		{
			Assert.That(() => new Ratio(-0.3m), Throws.InstanceOf<ArgumentOutOfRangeException>()
				.With.Property("ParamName").EqualTo("value").And
				.With.Property("ActualValue").EqualTo(-0.3m)
				.With.Message.StringContaining("0")
				.With.Message.StringContaining("1"));
		}

		[Test, SetCulture("")]
		public void Ctor_BigRatio_Exception()
		{
			Assert.That(() => new Ratio(2.3m), Throws.InstanceOf<ArgumentOutOfRangeException>()
				.With.Property("ParamName").EqualTo("value").And
				.With.Property("ActualValue").EqualTo(2.3m)
				.With.Message.StringContaining("0")
				.With.Message.StringContaining("1"));
		}

		[Test]
		public void Apply_MultipliesAmount()
		{
			Assert.That(new Ratio(.1m).ApplyTo(100), Is.EqualTo(10));
			
		}
	}
}