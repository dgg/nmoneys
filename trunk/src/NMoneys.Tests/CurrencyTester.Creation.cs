using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using NMoneys.Support;
using NMoneys.Tests.CustomConstraints;
using NMoneys.Tests.Support;
using NUnit.Framework;

namespace NMoneys.Tests
{
	[TestFixture]
	public partial class CurrencyTester
	{
		#region Get

		[Test]
		public void Get_UndefinedIsoCode_Exception()
		{
			var notDefined = (CurrencyIsoCode) 5000;
			Assert.That(() => Currency.Get(notDefined), Throws.InstanceOf<InvalidEnumArgumentException>().With.Message.StringContaining("5000"));
		}

		[Test]
		public void Get_DefinedIsoSymbol_CaseInsensitive()
		{
			Assert.That(Currency.Get(CurrencyIsoCode.USD), Is
				.SameAs(Currency.Get("USD"))
				.And.SameAs(Currency.Get("usd"))
				.And.SameAs(Currency.Get("Usd"))
				.And.SameAs(Currency.Get("uSD"))
				.And.SameAs(Currency.Get("uSd")));
		}

		[Test]
		public void Get_UndefinedIsoSymbol_Exception()
		{
			string notDefined = "5000";
			Assert.That(() => Currency.Get(notDefined), Throws.InstanceOf<InvalidEnumArgumentException>().With.Message.StringContaining("5000"));
		}

		[Test]
		public void Get_DefinedCurrencyForCulture_Currency()
		{
			CultureInfo spanish = CultureInfo.GetCultureInfo("es-ES");
			Assert.That(Currency.Get(spanish), Is.SameAs(Currency.Eur));
		}

		[Test]
		public void Get_NoRegionForCulture_Exception()
		{
			CultureInfo neutralSpanish = CultureInfo.GetCultureInfo("es");
			Assert.That(() => Currency.Get(neutralSpanish), Throws.ArgumentException);
		}

		[Test, Platform(Include = "Net-2.0")]
		public void Get_OutdateFrameworkCurrencySymbol_Exception()
		{
			CultureInfo bulgarian = CultureInfo.GetCultureInfo("bg-BG");
			var bulgaria = new RegionInfo(bulgarian.LCID);

			Assert.That(() => Currency.Get(bulgarian), Throws.InstanceOf<InvalidEnumArgumentException>().With.Message.StringContaining("BGL"), "Framework returns wrong ISOCurrencySymbol (BGL instead of BGN)");
			Currency lev = Currency.Get(CurrencyIsoCode.BGN);

			Assert.That(lev.IsoSymbol, Is.Not.EqualTo(bulgaria.ISOCurrencySymbol));
			Assert.That(lev.NativeName, Is.Not.EqualTo(bulgaria.CurrencyNativeName));
		}

		[Test]
		public void Get_RegionWithUpdatedInformation_Currency()
		{
			var denmark = new RegionInfo("DK");
			Assert.That(Currency.Get(denmark), Is.SameAs(Currency.Dkk));
		}

		[Test, Platform(Include = "Net-2.0")]
		public void Get_RegionWithOutdatedInformation_Exception()
		{
			var SerbiaAndMontenegro = new RegionInfo("CS");
			Assert.That(() => Currency.Get(SerbiaAndMontenegro), Throws.InstanceOf<InvalidEnumArgumentException>().With.Message.StringContaining("CSD"));
		}

		[Test]
		public void Get_NotAShortcut_Currency()
		{
			Currency notAShortcut = Currency.Get(CurrencyIsoCode.NAD);
			Assert.That(notAShortcut, Is.SameAs(Currency.Get("NAD")));

			Assert.That(notAShortcut.IsoCode, Is.EqualTo(CurrencyIsoCode.NAD));
			Assert.That(notAShortcut.IsoSymbol, Is.EqualTo("NAD"));
			Assert.That(notAShortcut.Symbol, Is.EqualTo("$"));
			Assert.That(notAShortcut.EnglishName, Is.EqualTo("Namibia Dollar"));
		}

		[Test, TestCaseSource(typeof(Obsolete), "ThreeLetterIsoCodes")]
		public void Get_ObsoleteCurrencyIsoCode_EventRaised(string threeLetterIsoCode)
		{
			Action getObsolete = () => Currency.Get(Enumeration.Parse<CurrencyIsoCode>(threeLetterIsoCode));
			Assert.That(getObsolete, Must.RaiseObsoleteEvent.Once());
		}

		[Test, TestCaseSource(typeof(Obsolete), "ThreeLetterIsoCodes")]
		public void Get_ObsoleteCurrencyCode_EventRaised(string threeLetterIsoCode)
		{
			Action getObsolete = () => Currency.Get(threeLetterIsoCode);
			Assert.That(getObsolete, Must.RaiseObsoleteEvent.Once());
		}

		[Test]
		public void Get_ObsoleteCulture_EventRaised()
		{
			Action getObsolete = () => Currency.Get(CultureInfo.GetCultureInfo("et-EE"));
			Assert.That(getObsolete, Must.RaiseObsoleteEvent.Once());
		}

		[Test]
		public void Get_ObsoleteRegion_EventRaised()
		{
			Action getObsolete = () => Currency.Get(new RegionInfo("EE"));
			Assert.That(getObsolete, Must.RaiseObsoleteEvent.Once());
		}

		#endregion

		#region TryGet

		[Test]
		public void TryGet_UndefinedIsoCode_False()
		{
			var notDefined = (CurrencyIsoCode) 5000;
			Currency tried;
			Assert.That(Currency.TryGet(notDefined, out tried), Is.False);
			Assert.That(tried, Is.Null);
		}

		[Test]
		public void TryGet_UndefinedIsoSymbol_False()
		{
			string notDefined = "5000";
			Currency tried;
			Assert.That(Currency.TryGet(notDefined, out tried), Is.False);
			Assert.That(tried, Is.Null);
		}

		[Test]
		public void TryGet_NullSymbol_False()
		{
			Currency tried;
			Assert.That(Currency.TryGet((string) null, out tried), Is.False);
			Assert.That(tried, Is.Null);
		}

		[Test]
		public void TryGet_DefinedCurrencyForCulture_True()
		{
			CultureInfo spanish = CultureInfo.GetCultureInfo("es-ES");
			Currency tried;
			Assert.That(Currency.TryGet(spanish, out tried), Is.True);
			Assert.That(tried, Is.SameAs(Currency.Eur));
		}

		[Test]
		public void TryGet_NoRegionForCulture_False()
		{
			CultureInfo neutralSpanish = CultureInfo.GetCultureInfo("es");
			Currency tried;
			Assert.That(Currency.TryGet(neutralSpanish, out tried), Is.False);
			Assert.That(tried, Is.Null);
		}

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

		[Test]
		public void TryGet_RegionWithUpdatedInformation_True()
		{
			var denmark = new RegionInfo("DK");
			Currency dkk;

			Assert.That(Currency.TryGet(denmark, out dkk), Is.True);
			Assert.That(dkk, Is.SameAs(Currency.Dkk));
		}

		[Test, Platform(Include = "Net-2.0")]
		public void TryGet_RegionWithOutdatedInformation_False()
		{
			var SerbiaAndMontenegro = new RegionInfo("CS");
			Currency rsd;
			Assert.That(Currency.TryGet(SerbiaAndMontenegro, out rsd), Is.False);
			Assert.That(rsd, Is.Null);
		}

		[Test, TestCaseSource(typeof(Obsolete), "ThreeLetterIsoCodes")]
		public void TryGet_ObsoleteCurrencyIsoCode_EventRaised(string threeLetterIsoCode)
		{
			Currency c;
			Action tryGetObsolete = () => Currency.TryGet(Enumeration.Parse<CurrencyIsoCode>(threeLetterIsoCode), out c);
			Assert.That(tryGetObsolete, Must.RaiseObsoleteEvent.Once());
		}

		[Test, TestCaseSource(typeof(Obsolete), "ThreeLetterIsoCodes")]
		public void TryGet_ObsoleteCurrencyCode_EventRaised(string threeLetterIsoCode)
		{
			Currency c;
			Action tryGetObsolete = () => Currency.TryGet(threeLetterIsoCode, out c);
			Assert.That(tryGetObsolete, Must.RaiseObsoleteEvent.Once());
		}

		[Test]
		public void TryGet_ObsoleteCulture_EventRaised()
		{
			Currency c;
			Action tryGetObsolete = () => Currency.TryGet(CultureInfo.GetCultureInfo("et-EE"), out c);
			Assert.That(tryGetObsolete, Must.RaiseObsoleteEvent.Once());
		}

		[Test]
		public void TryGet_ObsoleteRegion_EventRaised()
		{
			Currency c;
			Action tryGetObsolete = () => Currency.TryGet(new RegionInfo("EE"), out c);
			Assert.That(tryGetObsolete, Must.RaiseObsoleteEvent.Once());
		}

		#endregion

		#region FindAll

		[Test]
		public void FindAll_GetsAllCurrencies()
		{
			Currency[] allCurrencies = Currency.FindAll().ToArray();
			CurrencyIsoCode[] allCodes = Enumeration.GetValues<CurrencyIsoCode>();

			Assert.That(allCurrencies.Select(c => c.IsoCode), Is.EquivalentTo(allCodes));
		}

		[Test]
		public void FindAll_CanBeUsedForLinq()
		{
			Func<Currency, bool> currenciesWithDollarSymbol = c => c.Symbol.Equals("$", StringComparison.Ordinal);
			Assert.That(Currency.FindAll().Where(currenciesWithDollarSymbol), Is.Not.Empty);
		}

		[Test]
		public void FindAll_ReturnsObsoleteCurrencies()
		{
			Action iterateAllCurrencies = () => Currency.FindAll().ToArray();
			Assert.That(iterateAllCurrencies, Must.RaiseObsoleteEvent.Times(ObsoleteCurrencies.Count));
		}

		#endregion

	}
}