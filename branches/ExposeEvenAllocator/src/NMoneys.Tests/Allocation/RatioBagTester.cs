using System;
using System.Linq;
using NMoneys.Allocation;
using NUnit.Framework;

namespace NMoneys.Tests.Allocation
{
	[TestFixture]
	public class RatioBagTester
	{
		[Test]
		public void Ctor_Empty_Exception()
		{
			Assert.That(() => new RatioBag(), Throws.InstanceOf<ArgumentOutOfRangeException>()
				.With.Property("ParamName").EqualTo("ratios").And
				.With.Property("ActualValue").EqualTo(0m)
				.With.Message.StringContaining("1.0"));
		}

		[Test]
		public void Ctor_Ordering_Empty_Exception()
		{
			Assert.That(() => new RatioBag(AllocationOrdering.HighToLow), Throws.InstanceOf<ArgumentOutOfRangeException>()
				.With.Property("ParamName").EqualTo("ratios").And
				.With.Property("ActualValue").EqualTo(0m)
				.With.Message.StringContaining("1.0"));
		}
		[Test]
		public void ctor_NullOrdering_Exception()
		{
			AllocationOrdering @null = null;
			Assert.That(() => new RatioBag(@null), Throws.InstanceOf<ArgumentNullException>()
				.With.Property("ParamName").EqualTo("ordering"));
		}

		[Test]
		public void Ctor_InvalidRatio_Exception()
		{
			Assert.That(() => new RatioBag(0m, -.5m, 1.5m), Throws.InstanceOf<ArgumentOutOfRangeException>()
				.With.Property("ParamName").EqualTo("ratios").And
				.With.Property("ActualValue").EqualTo(-.5m).And
				.With.Message.StringContaining("[0..1]"));
		}

		[Test]
		public void Ctor_Ordering_InvalidRatio_Exception()
		{
			Assert.That(() => new RatioBag(AllocationOrdering.LowToHigh, 0m, -.5m, 1.5m), Throws.InstanceOf<ArgumentOutOfRangeException>()
				.With.Property("ParamName").EqualTo("ratios").And
				.With.Property("ActualValue").EqualTo(-.5m).And
				.With.Message.StringContaining("[0..1]"));
		}

		[Test]
		public void Ctor_NotSummingOne_Exception()
		{
			Assert.That(() => new RatioBag(.3m, .2m), Throws.InstanceOf<ArgumentOutOfRangeException>()
				.With.Property("ParamName").EqualTo("ratios").And
				.With.Property("ActualValue").EqualTo(.5m)
				.With.Message.StringContaining("1.0"));
		}

		[Test]
		public void Ctor_Ordering_NotSummingOne_Exception()
		{
			Assert.That(() => new RatioBag(AllocationOrdering.Random, .3m, .2m), Throws.InstanceOf<ArgumentOutOfRangeException>()
				.With.Property("ParamName").EqualTo("ratios").And
				.With.Property("ActualValue").EqualTo(.5m)
				.With.Message.StringContaining("1.0"));
		}

		[Test]
		public void Ctor_SummingOne_CollectionOfRatios()
		{
			var bag = new RatioBag(0.5m, .4m, .05m, .05m);

			Assert.That(bag, Has.Count.EqualTo(4).And.EqualTo(new[] { 0.5m, .4m, .05m, .05m }));
			Assert.That(bag[0], Is.EqualTo(.5m));
			Assert.That(bag[1], Is.EqualTo(.4m));
			Assert.That(bag[2], Is.EqualTo(.05m));
			Assert.That(bag[3], Is.EqualTo(.05m));
		}

		[Test]
		public void Ctor_AnArrayCanBeUsedToPassRatios()
		{
			var bag = new RatioBag(new[] { 0.5m, .4m, .05m, .05m });

			Assert.That(bag, Is.EqualTo(new[] { 0.5m, .4m, .05m, .05m }));
		}

		[Test]
		public void AsIs_UnalteredCollectionOfRatios()
		{
			var bag = new RatioBag(AllocationOrdering.AsIs, 0.5m, .4m, .05m, .05m);

			Assert.That(bag, Is.EqualTo(new[] { 0.5m, .4m, .05m, .05m }));
			Assert.That(bag[0], Is.EqualTo(.5m));
			Assert.That(bag[1], Is.EqualTo(.4m));
			Assert.That(bag[2], Is.EqualTo(.05m));
			Assert.That(bag[3], Is.EqualTo(.05m));
		}

		[Test]
		public void HightToLow_OrderedCollectionOfRatios()
		{
			var bag = new RatioBag(AllocationOrdering.HighToLow, 0.4m, .05m, .5m, .05m);

			Assert.That(bag, Is.EqualTo(new[] { 0.5m, .4m, .05m, .05m }));
			Assert.That(bag[0], Is.EqualTo(.5m));
			Assert.That(bag[1], Is.EqualTo(.4m));
			Assert.That(bag[2], Is.EqualTo(.05m));
			Assert.That(bag[3], Is.EqualTo(.05m));
		}

		[Test]
		public void LowToHigh_InverseOrderedCollectionOfRatios()
		{
			var bag = new RatioBag(AllocationOrdering.LowToHigh, 0.4m, .05m, .5m, .05m);

			Assert.That(bag, Is.EqualTo(new[] { .05m, .05m, .4m, 0.5m }));
			Assert.That(bag[0], Is.EqualTo(.05m));
			Assert.That(bag[1], Is.EqualTo(.05m));
			Assert.That(bag[2], Is.EqualTo(.4m));
			Assert.That(bag[3], Is.EqualTo(.5m));
		}

		[Test]
		public void CustomOrdering_OrderedCollectionOfRatios()
		{
			AllocationOrdering reverse = AllocationOrdering.Custom(a => a.Reverse());

			var bag = new RatioBag(reverse, 0.4m, .05m, .3m, .05m, .2m);

			Assert.That(bag, Is.EqualTo(new[] { .2m, .05m, .3m, .05m, .4m }));
			Assert.That(bag[0], Is.EqualTo(.2m));
			Assert.That(bag[1], Is.EqualTo(.05m));
			Assert.That(bag[2], Is.EqualTo(.3m));
			Assert.That(bag[3], Is.EqualTo(.05m));
			Assert.That(bag[4], Is.EqualTo(.4m));
		}

		[Test]
		public void Original_ReturnsRatiosAsEntered()
		{
			AllocationOrdering reverse = AllocationOrdering.Custom(a => a.Reverse());

			var bag = new RatioBag(reverse, 0.4m, .05m, .3m, .05m, .2m);

			Assert.That(bag, Is.EqualTo(new[] { .2m, .05m, .3m, .05m, .4m }));
			Assert.That(bag.Original, Is.EqualTo(new[] { 0.4m, .05m, .3m, .05m, .2m }));
		}
	}
}
