using System;
using System.Globalization;
using NMoneys.Allocations;
using NUnit.Framework;
using Testing.Commons.Globalization;

namespace NMoneys.Tests.Allocations
{
	[TestFixture]
	public partial class RatioTester
	{
		[TestCaseSource(nameof(properValues))]
		public void Ctor_ProperRatio_ValueSet(decimal properValue)
		{
			var proper = new Ratio(properValue);

			Assert.That(proper.Value, Is.EqualTo(properValue));
		}

		private static readonly decimal[] properValues = {0m, .001m, .5m, .73m, 1m};

		[Test]
		public void Ctor_NegativeRatio_Exception()
		{
			using (CultureReseter.Set(CultureInfo.InvariantCulture))
			{ 
				Assert.That(() => new Ratio(-0.3m), Throws.InstanceOf<ArgumentOutOfRangeException>()
					.With.Property("ParamName").EqualTo("value").And
					.With.Property("ActualValue").EqualTo(-0.3m)
					.With.Message.Contains("0")
					.With.Message.Contains("1"));
			}
		}

		[Test]
		public void Ctor_BigRatio_Exception()
		{
			using (CultureReseter.Set(CultureInfo.InvariantCulture))
			{ 
				Assert.That(() => new Ratio(2.3m), Throws.InstanceOf<ArgumentOutOfRangeException>()
					.With.Property("ParamName").EqualTo("value").And
					.With.Property("ActualValue").EqualTo(2.3m)
					.With.Message.Contains("0")
					.With.Message.Contains("1"));
			}
		}

		[Test]
		public void Apply_MultipliesAmount()
		{
			Assert.That(new Ratio(.1m).ApplyTo(100), Is.EqualTo(10));
			
		}
	}
}