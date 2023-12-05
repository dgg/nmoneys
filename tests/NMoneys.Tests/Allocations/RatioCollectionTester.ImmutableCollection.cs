using NMoneys.Allocations;

namespace NMoneys.Tests.Allocations;

[TestFixture]
public partial class RatioCollectionTester
{
	[Test]
	public void Count_NumberOfRatiosInTheBag()
	{
		var bag = new RatioCollection(0.5m, .4m, .05m, .05m);

		Assert.That(bag, Has.Count.EqualTo(4));
	}

	[Test]
	public void RatiosCanBeEnumerated()
	{
		var bag = new RatioCollection(0.5m, .4m, .05m, .05m);

		Assert.That(bag, Is.EqualTo(new[]
		{
			new Ratio(.5m),
			new Ratio(.4m),
			new Ratio(.05m),
			new Ratio(.05m)
		}));


		Assert.That(bag[0], Is.InstanceOf<Ratio>().With.Property("Value").EqualTo(.5m));
		Assert.That(bag[1], Is.InstanceOf<Ratio>().With.Property("Value").EqualTo(.4m));
		Assert.That(bag[2], Is.InstanceOf<Ratio>().With.Property("Value").EqualTo(.05m));
		Assert.That(bag[3], Is.InstanceOf<Ratio>().With.Property("Value").EqualTo(.05m));
	}

	[Test]
	public void RatiosCanIndexed()
	{
		var bag = new RatioCollection(0.5m, .4m, .05m, .05m);

		Assert.That(bag[0], Is.InstanceOf<Ratio>().With.Property("Value").EqualTo(.5m));
		Assert.That(bag[1], Is.InstanceOf<Ratio>().With.Property("Value").EqualTo(.4m));
		Assert.That(bag[2], Is.InstanceOf<Ratio>().With.Property("Value").EqualTo(.05m));
		Assert.That(bag[3], Is.InstanceOf<Ratio>().With.Property("Value").EqualTo(.05m));
	}
}
