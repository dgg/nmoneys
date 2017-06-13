using System.Globalization;
using NMoneys.Change;
using NUnit.Framework;
using Testing.Commons.Globalization;

namespace NMoneys.Tests.Change
{
	[TestFixture]
	public class QuantifiedDenominationTester
	{
		[Test]
		public void ToString_ValueString_AsPerCurrentCulture()
		{
			var threeOfPointTwo = new QuantifiedDenomination(new Denomination(.2m), 3u);

			using (CultureReseter.Set("en-US"))
			{
				Assert.That(threeOfPointTwo.ToString(), Is.EqualTo("< 3 * 0.2 >"), "US decimals with a dot");
			}

			using (CultureReseter.Set("da-DK"))
			{
				Assert.That(threeOfPointTwo.ToString(), Is.EqualTo("< 3 * 0,2 >"), "DK decimals with a comma");
			}
		}

		[Test]
		public void ToString_CanReceiveCustomFormatsAndProviders()
		{
			var threeOfPointTwo = new QuantifiedDenomination(new Denomination(.2m), 3u);
			var snailDecimalSeparator = (NumberFormatInfo)CultureInfo.InvariantCulture.NumberFormat.Clone();
			snailDecimalSeparator.NumberDecimalSeparator = "@";

			Assert.That(threeOfPointTwo.ToString(".000", snailDecimalSeparator), Is.EqualTo("< 3 * @200 >"));
		}
	}
}