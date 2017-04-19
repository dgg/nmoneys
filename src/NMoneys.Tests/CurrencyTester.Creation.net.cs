using System;
using System.ComponentModel;
using System.Globalization;
using NMoneys.Tests.CustomConstraints;
using NUnit.Framework;
using Testing.Commons;

namespace NMoneys.Tests
{
	// PlatformAttribute is not present in NUnits netstandard build, but since the tests marked 
	// as platform-dependent cannot happen in a core runtime (they apply to old .NET 2 versions), 
	// they can be safely ignored
	[TestFixture]
	public partial class CurrencyTester
	{
		#region Get

		[Test, Platform(Include = "Net-2.0")]
		public void Get_OutdateFrameworkCurrencySymbol_Exception()
		{
			CultureInfo bulgarian = CultureInfo.GetCultureInfo("bg-BG");
			var bulgaria = new RegionInfo(bulgarian.LCID);

			Assert.That(() => Currency.Get(bulgarian), Throws.InstanceOf<InvalidEnumArgumentException>().With.Message.Contains("BGL"), "Framework returns wrong ISOCurrencySymbol (BGL instead of BGN)");
			Currency lev = Currency.Get(CurrencyIsoCode.BGN);

			Assert.That(lev.IsoSymbol, Is.Not.EqualTo(bulgaria.ISOCurrencySymbol));
			Assert.That(lev.NativeName, Is.Not.EqualTo(bulgaria.CurrencyNativeName));
		}

		[Test, Platform(Include = "Net-2.0")]
		public void Get_RegionWithOutdatedInformation_Exception()
		{
			var SerbiaAndMontenegro = new RegionInfo("CS");
			Assert.That(() => Currency.Get(SerbiaAndMontenegro), Throws.InstanceOf<InvalidEnumArgumentException>().With.Message.Contains("CSD"));
		}

		[Test, Platform(Include = "Net-2.0", Reason = "updated culture uses non-deprecated currency")]
		public void Get_ObsoleteCulture_EventRaised()
		{
			Action getObsolete = () => Currency.Get(CultureInfo.GetCultureInfo("et-EE"));
			Assert.That(getObsolete, Must.Raise.ObsoleteEvent());
		}

		[Test, Platform(Include = "Net-2.0", Reason = "updated culture uses non-deprecated currency")]
		public void Get_ObsoleteRegion_EventRaised()
		{
			Action getObsolete = () => Currency.Get(new RegionInfo("EE"));
			Assert.That(getObsolete, Must.Raise.ObsoleteEvent());
		}

		#endregion

		#region TryGet

		[Test, Platform(Include = "Net-2.0")]
		public void TryGet_OutdateFrameworkCurrencySymbol_False()
		{
			CultureInfo bulgarian = CultureInfo.GetCultureInfo("bg-BG");
			var bulgaria = new RegionInfo(bulgarian.LCID);

			Currency lev;
			Assert.That(Currency.TryGet(bulgarian, out lev), Is.False, "Framework returns wrong ISOCurrencySymbol (BGL instead of BGN)");
			Assert.That(lev, Is.Null);

			Currency.TryGet(CurrencyIsoCode.BGN, out lev);

			Assert.That(lev.IsoSymbol, Is.Not.EqualTo(bulgaria.ISOCurrencySymbol));
			Assert.That(lev.NativeName, Is.Not.EqualTo(bulgaria.CurrencyNativeName));
		}

		[Test, Platform(Include = "Net-2.0")]
		public void TryGet_RegionWithOutdatedInformation_False()
		{
			var SerbiaAndMontenegro = new RegionInfo("CS");
			Currency rsd;
			Assert.That(Currency.TryGet(SerbiaAndMontenegro, out rsd), Is.False);
			Assert.That(rsd, Is.Null);
		}

		[Test, Platform(Include = "Net-2.0", Reason = "updated culture uses non-deprecated currency")]
		public void TryGet_ObsoleteCulture_EventRaised()
		{
			Currency c;
			Action tryGetObsolete = () => Currency.TryGet(CultureInfo.GetCultureInfo("et-EE"), out c);
			Assert.That(tryGetObsolete, Must.Raise.ObsoleteEvent());
		}

		[Test, Platform(Include = "Net-2.0", Reason = "updated culture uses non-deprecated currency")]
		public void TryGet_ObsoleteRegion_EventRaised()
		{
			Currency c;
			Action tryGetObsolete = () => Currency.TryGet(new RegionInfo("EE"), out c);
			Assert.That(tryGetObsolete, Must.Raise.ObsoleteEvent());
		}

		#endregion
	}
}