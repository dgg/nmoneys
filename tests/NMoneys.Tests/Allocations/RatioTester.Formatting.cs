using System.Globalization;
using NMoneys.Allocations;

namespace NMoneys.Tests.Allocations;

[TestFixture]
public partial class RatioTester
{
	[Test, SetCulture("en-US")]
	public void ToString_ValueString_USFormatting()
	{
		var pointTwo = new Ratio(.2m);

		Assert.That(pointTwo.ToString(), Is.EqualTo("0.2"), "US decimals with a dot");
	}

	[Test, SetCulture("da-DK")]
	public void ToString_ValueString_DKFormatting()
	{
		var pointTwo = new Ratio(.2m);

		Assert.That(pointTwo.ToString(), Is.EqualTo("0,2"), "DK decimals with a comma");
	}

	[Test]
	public void ToString_CanReceiveCustomFormatsAndProviders()
	{
		var pointTwo = new Ratio(.2m);
		var snailDecimalSeparator = (NumberFormatInfo)CultureInfo.InvariantCulture.NumberFormat.Clone();
		snailDecimalSeparator.NumberDecimalSeparator = "@";

		Assert.That(pointTwo.ToString(".000", snailDecimalSeparator), Is.EqualTo("@200"));
	}
}
