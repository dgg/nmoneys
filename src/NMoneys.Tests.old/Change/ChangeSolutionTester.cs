using System.Globalization;
using NMoneys.Change;
using NUnit.Framework;
using Testing.Commons.Globalization;

namespace NMoneys.Tests.Change
{
	[TestFixture]
	public class ChangeSolutionTester
	{
		[Test]
		public void ToString_ValueString_AsPerCurrentCulture()
		{
			var denominations = new[]{ new Denomination(2), new Denomination(.2m), new Denomination(.2m) };
			var subject = new ChangeSolution(denominations, Money.Zero(CurrencyIsoCode.EUR));

			using (CultureReseter.Set("en-US"))
			{
				Assert.That(subject.ToString(), Is.EqualTo("< < 1 * 2 > | < 2 * 0.2 > >"), "US decimals with a dot");
			}

			using (CultureReseter.Set("da-DK"))
			{
				Assert.That(subject.ToString(), Is.EqualTo("< < 1 * 2 > | < 2 * 0,2 > >"), "DK decimals with a comma");
			}
		}

		[Test]
		public void ToString_CanReceiveCustomFormatsAndProviders()
		{
			var denominations = new[] { new Denomination(2), new Denomination(.2m), new Denomination(.2m) };

			var subject = new ChangeSolution(denominations, Money.Zero(CurrencyIsoCode.EUR));
			var snailDecimalSeparator = (NumberFormatInfo)CultureInfo.InvariantCulture.NumberFormat.Clone();
			snailDecimalSeparator.NumberDecimalSeparator = "@";

			Assert.That(subject.ToString(".000", snailDecimalSeparator), Is.EqualTo("< < 1 * 2@000 > | < 2 * @200 > >"));
		}

		[Test]
		public void ToString_NoRemainder_OnlyDenominations()
		{
			var subject = new ChangeSolution(new[] { new Denomination(2) }, 
				Money.Zero(CurrencyIsoCode.EUR));

			using (CultureReseter.Set(CultureInfo.InvariantCulture))
			{
				Assert.That(subject.ToString(), Is.EqualTo("< < 1 * 2 > >"));
			}
		}

		[Test]
		public void ToString_Remainder_DenominationsAndRemainder()
		{
			var subject = new ChangeSolution(new[] { new Denomination(2) },
				new Money(5, CurrencyIsoCode.EUR));

			using (CultureReseter.Set(CultureInfo.InvariantCulture))
			{
				Assert.That(subject.ToString(), Is.EqualTo("< < 1 * 2 > > + < 5,00 € >"));
			}
		}
	}
}