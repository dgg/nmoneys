using System.Globalization;
using NMoneys.Allocations;
using NMoneys.Tests.Support;
using NUnit.Framework;

namespace NMoneys.Tests.Allocations
{
	[TestFixture]
	public partial class RatioTester
	{
		[Test]
		public void ToString_ValueString_AsPerCurrentCulture()
		{
			var pointTwo = new Ratio(.2m);

			using (CultureReseter.Set("en-US"))
			{
				Assert.That(pointTwo.ToString(), Is.EqualTo("0.2"), "US decimals with a dot");
			}

			using (CultureReseter.Set("da-DK"))
			{
				Assert.That(pointTwo.ToString(), Is.EqualTo("0,2"), "DK decimals with a comma");
			}
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
}