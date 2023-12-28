using NMoneys.Allocations;

namespace NMoneys.Tests.Allocations;

[TestFixture]
public partial class RatioTester
{
	[Test, SetCulture("en-US")]
	public void ToString_US_InvariantFormatting()
	{
		var pointTwo = new Ratio(.2m);

		Assert.That(pointTwo.ToString(), Is.EqualTo("Ratio { 0.2 }"), "US decimals with a dot");
	}

	[Test, SetCulture("da-DK")]
	public void ToString_DK_InvariantFormatting()
	{
		var pointTwo = new Ratio(.2m);

		Assert.That(pointTwo.ToString(), Is.EqualTo("Ratio { 0.2 }"), "DK decimals with a comma");
	}
}
