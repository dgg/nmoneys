using System;
using System.ComponentModel;
using System.Globalization;
using NMoneys.Tests.CustomConstraints;
using NUnit.Framework;
using Testing.Commons;
using Testing.Commons.Globalization;

namespace NMoneys.Tests
{
	// PlatformAttribute is not present in NUnits netstandard build, but since the tests marked 
	// as platform-dependent cannot happen in a core runtime (they apply to old .NET 2 versions), 
	// they can be safely ignored
	[TestFixture]
	public partial class MoneyTester
	{
		#region ForCulture

		[Test, Platform(Include = "Net-2.0")]
		public void ForCulture_OutdatedCulture_Exception()
		{
			Assert.That(() => Money.ForCulture(decimal.Zero, CultureInfo.GetCultureInfo("bg-BG")),
				Throws.InstanceOf<InvalidEnumArgumentException>().With.Message.Contains("BGL"),
				"Framework returns wrong ISOCurrencySymbol (BGL instead of BGN)");
		}

		[Test, Platform(Include = "Net-2.0")]
		public void ForCulture_CultureWithObsoleteCulture_EventRaised()
		{
			Action moneyWithObsoleteCurrency = () => Money.ForCulture(decimal.Zero, CultureInfo.GetCultureInfo("et-EE"));
			Assert.That(moneyWithObsoleteCurrency, Must.Raise.ObsoleteEvent());
		}

		#endregion

		#region ForCurrentCulture

		[Test, Platform(Include = "Net-2.0")]
		public void ForCurrentCulture_OutdatedCulture_Exception()
		{
			using (CultureReseter.Set("bg-BG"))
			{
				Assert.That(() => Money.ForCurrentCulture(decimal.Zero),
					Throws.InstanceOf<InvalidEnumArgumentException>().With.Message.Contains("BGL"),
					"Framework returns wrong ISOCurrencySymbol (BGL instead of BGN)");
			}
		}

		[Test, Platform(Include = "Net-2.0")]
		public void ForCurrentCulture_CultureWithObsoleteCulture_EventRaised()
		{
			using (CultureReseter.Set("ee-EE"))
			{
				Action moneyWithObsoleteCurrency = () => Money.ForCurrentCulture(decimal.Zero);
				Assert.That(moneyWithObsoleteCurrency, Must.Raise.ObsoleteEvent());
			}
		}

		#endregion
	}
}