using System.Globalization;
using NMoneys.Allocations;
using NMoneys.Tests.Support;
using NUnit.Framework;

namespace NMoneys.Tests.Allocations
{
	[TestFixture]
	public partial class RatioCollectionTester
	{
		[Test]
		public void ToString_PipeSeparatedRatiosAsPerCurrentCulture()
		{
			var bag = new RatioCollection(0.5m, .4m, .05m, .05m);

			using (CultureReseter.Set("en-US"))
			{
				Assert.That(bag.ToString(), Is.EqualTo("< 0.5 | 0.4 | 0.05 | 0.05 >"), "US decimals with a dot");
			}

			using (CultureReseter.Set("da-DK"))
			{
				Assert.That(bag.ToString(), Is.EqualTo("< 0,5 | 0,4 | 0,05 | 0,05 >"), "DK decimals with a comma");
			}
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
}