using System;
using NMoneys.Allocations;
using NUnit.Framework;

namespace NMoneys.Tests.Allocations
{
	[TestFixture]
	public partial class RatioBagTester
	{
		[Test]
		public void Ctor_EmptyRatios_Exception()
		{
			Assert.That(() => new RatioBag(new Ratio[] { }), Throws.InstanceOf<ArgumentOutOfRangeException>()
				.With.Property("ParamName").EqualTo("ratios").And
				.With.Property("ActualValue").EqualTo(0m)
				.With.Message.StringContaining("1.0"));
		}

		[Test]
		public void Ctor_EmptyValues_Exception()
		{
			Assert.That(() => new RatioBag(new decimal[] { }), Throws.InstanceOf<ArgumentOutOfRangeException>()
				.With.Property("ParamName").EqualTo("ratios").And
				.With.Property("ActualValue").EqualTo(0m)
				.With.Message.StringContaining("1.0"));
		}

		[Test]
		public void Ctor_InvalidRatioValue_Exception()
		{
			Assert.That(() => new RatioBag(-.5m), Throws.InstanceOf<ArgumentOutOfRangeException>()
				.With.Property("ActualValue").EqualTo(-.5m).And
				.With.Message.StringContaining("[0..1]"));
		}

		[Test]
		public void Ctor_RatiosDoNotSumOne_Exception()
		{
			Assert.That(() => new RatioBag(new Ratio(.3m), new Ratio(.2m)), Throws.InstanceOf<ArgumentOutOfRangeException>()
				.With.Property("ParamName").EqualTo("ratios").And
				.With.Property("ActualValue").EqualTo(.5m)
				.With.Message.StringContaining("1.0"));
		}

		[Test]
		public void Ctor_RatioValuesDoNotSumOne_Exception()
		{
			Assert.That(() => new RatioBag(.3m, .2m), Throws.InstanceOf<ArgumentOutOfRangeException>()
				.With.Property("ParamName").EqualTo("ratios").And
				.With.Property("ActualValue").EqualTo(.5m)
				.With.Message.StringContaining("1.0"));
		}

		[Test]
		public void Ctor_ValuesSummingOne_NoException()
		{
			Assert.That(() => new RatioBag(0.5m, .4m, .05m, .05m), Throws.Nothing);
		}

		[Test]
		public void Ctor_RatiosSummingOne_NoException()
		{
			TestDelegate ratioCtor = () =>
				new RatioBag(
					new Ratio(0.5m),
					new Ratio(.4m),
					new Ratio(.05m),
					new Ratio(.05m));
			Assert.That(ratioCtor, Throws.Nothing);
		}

		[Test]
		public void Ctor_ArrayOfValues_InstanceCreated()
		{
			var arrayOfValues = new[] { 0.5m, .4m, .05m, .05m };
			Assert.That(()=> new RatioBag(arrayOfValues), Throws.Nothing);
		}

		[Test]
		public void Ctor_ArrayOfRatios_InstanceCreated()
		{
			var arrayOfRatios = new[]
			{
				new Ratio(0.5m),
				new Ratio(.4m),
				new Ratio(.05m),
				new Ratio(.05m)
			};
			Assert.That(() => new RatioBag(arrayOfRatios), Throws.Nothing);
		}
	}
}
