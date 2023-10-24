using System.Globalization;
using NMoneys.Change;
using NMoneys.Extensions;
using NUnit.Framework;
using Testing.Commons.Globalization;

namespace NMoneys.Tests.Change
{
	[TestFixture]
	public class OptimalChangeSolutionTester
	{
		[Test]
		public void ToString_ValueString_AsPerCurrentCulture()
		{
			var subject = .5m.Xxx().MakeOptimalChange(.1m, .3m, .2m);

			using (CultureReseter.Set("en-US"))
			{
				Assert.That(subject.ToString(), Is.EqualTo("< < 1 * 0.3 > | < 1 * 0.2 > >"), "US decimals with a dot");
			}

			using (CultureReseter.Set("da-DK"))
			{
				Assert.That(subject.ToString(), Is.EqualTo("< < 1 * 0,3 > | < 1 * 0,2 > >"), "DK decimals with a comma");
			}
		}

		[Test]
		public void ToString_CanReceiveCustomFormatsAndProviders()
		{
			var subject = .5m.Xxx().MakeOptimalChange(.5m, .3m, .2m);

			var snailDecimalSeparator = (NumberFormatInfo)CultureInfo.InvariantCulture.NumberFormat.Clone();
			snailDecimalSeparator.NumberDecimalSeparator = "@";

			Assert.That(subject.ToString(".000", snailDecimalSeparator), Is.EqualTo("< < 1 * @500 > >"));
		}
	}
}
 