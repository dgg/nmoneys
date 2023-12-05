using System.Globalization;
using NMoneys.Allocations;

namespace NMoneys.Tests.Allocations;

[TestFixture]
public partial class RatioCollectionTester
{
	[Test, SetCulture("en-US")]
	public void ToString_US_PipeSeparatedRatiosAsPerCurrentCulture()
	{
		var bag = new RatioCollection(0.5m, .4m, .05m, .05m);

		Assert.That(bag.ToString(), Is.EqualTo("< 0.5 | 0.4 | 0.05 | 0.05 >"), "US decimals with a dot");
	}

	[Test, SetCulture("da-DK")]
	public void ToString_DK_PipeSeparatedRatiosAsPerCurrentCulture()
	{
		var bag = new RatioCollection(0.5m, .4m, .05m, .05m);

		Assert.That(bag.ToString(), Is.EqualTo("< 0,5 | 0,4 | 0,05 | 0,05 >"), "DK decimals with a comma");
	}

	[Test, SetCulture("")]
	public void ToString_SingleRatio_PaddedOne()
	{
		var singleBag = new RatioCollection(1m);

		Assert.That(singleBag.ToString(), Is.EqualTo("< 1 >"));
	}

	[Test, SetCulture("")]
	public void ToString_CustomFormatsAndProviders_AppliesToRatios()
	{
		var bag = new RatioCollection(0.5m, .4m, .05m, .05m);
		var snailDecimalSeparator = (NumberFormatInfo)CultureInfo.InvariantCulture.NumberFormat.Clone();
		snailDecimalSeparator.NumberDecimalSeparator = "@";

		Assert.That(bag.ToString(".000", snailDecimalSeparator), Is.EqualTo("< @500 | @400 | @050 | @050 >"));
	}
}
