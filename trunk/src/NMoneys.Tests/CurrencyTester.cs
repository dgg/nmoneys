using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows.Markup;
using NMoneys.Support;
using NMoneys.Tests.CustomConstraints;
using NMoneys.Tests.Support;
using NUnit.Framework;

namespace NMoneys.Tests
{
	[TestFixture]
	public class CurrencyTester
	{
		[TestCaseSource("shortcuts_info")]
		public void Shortcuts_MinimalInfoConfigured(Currency shortcut, CurrencyIsoCode isoCode, string isoSymbol, string symbol, string englishName)
		{
			Assert.That(shortcut.IsoCode, Is.EqualTo(isoCode));
			Assert.That(shortcut.IsoSymbol, Is.EqualTo(isoSymbol));
			Assert.That(shortcut.Symbol, Is.EqualTo(symbol));
			Assert.That(shortcut.EnglishName, Is.EqualTo(englishName));
		}

#pragma warning disable 169
		private static readonly object[] shortcuts_info = new object[]
		{
			new object[] {Currency.Aud, CurrencyIsoCode.AUD, "AUD", "$", "Australian Dollar"},
			new object[] {Currency.Cad, CurrencyIsoCode.CAD, "CAD", "$", "Canadian Dollar"},
			// override for 2.0 SFr.
			new object[] {Currency.Chf, CurrencyIsoCode.CHF, "CHF", "Fr.", "Swiss Franc"},
			// overriden for wrong symbol "kr."
			new object[] {Currency.Dkk, CurrencyIsoCode.DKK, "DKK", "kr", "Danish Krone"},
			new object[] {Currency.Eur, CurrencyIsoCode.EUR, "EUR", "€", "Euro"},
			new object[] {Currency.Gbp, CurrencyIsoCode.GBP, "GBP", "£", "UK Pound Sterling"},
			new object[] {Currency.Hkd, CurrencyIsoCode.HKD, "HKD", "HK$", "Hong Kong Dollar"},
			new object[] {Currency.Huf, CurrencyIsoCode.HUF, "HUF", "Ft", "Hungarian Forint"},
			new object[] {Currency.Inr, CurrencyIsoCode.INR, "INR", "रु", "Indian Rupee"},
			new object[] {Currency.Jpy, CurrencyIsoCode.JPY, "JPY", "¥", "Japanese Yen"},
			new object[] {Currency.Mxn, CurrencyIsoCode.MXN, "MXN", "$", "Mexican Peso"},
			// overriden for 2.0 "R"
			new object[] {Currency.Myr, CurrencyIsoCode.MYR, "MYR", "RM", "Malaysian Ringgit"},
			new object[] {Currency.Nok, CurrencyIsoCode.NOK, "NOK", "kr", "Norwegian Krone"},
			new object[] {Currency.Nzd, CurrencyIsoCode.NZD, "NZD", "$", "New Zealand Dollar"},
			new object[] {Currency.Rub, CurrencyIsoCode.RUB, "RUB", "р.", "Russian Ruble"},
			new object[] {Currency.Sek, CurrencyIsoCode.SEK, "SEK", "kr", "Swedish Krona"},
			new object[] {Currency.Sgd, CurrencyIsoCode.SGD, "SGD", "$", "Singapore Dollar"},
			new object[] {Currency.Thb, CurrencyIsoCode.THB, "THB", "฿", "Thai Baht"},
			new object[] {Currency.Usd, CurrencyIsoCode.USD, "USD", "$", "US Dollar"},
			new object[] {Currency.Xts, CurrencyIsoCode.XTS, "XTS", "¤", "Test currency"},
			new object[] {Currency.Xxx, CurrencyIsoCode.XXX, "XXX", "¤", "No currency"},
			new object[] {Currency.Zar, CurrencyIsoCode.ZAR, "ZAR", "R", "South African Rand"},
		};
#pragma warning restore 169

		[TestCaseSource("aliases")]
		public void Aliases_ReferenceShortcut(Currency alias, Currency shortcut)
		{
			Assert.That(alias, Is.SameAs(shortcut));
		}

#pragma warning disable 169
		private static readonly object[] aliases = new object[]
		{
			new object[] {Currency.Euro, Currency.Eur },
			new object[] {Currency.Dollar, Currency.Usd },
			new object[] {Currency.Pound, Currency.Gbp },
			new object[] {Currency.Test, Currency.Xts },
			new object[] {Currency.None, Currency.Xxx },
		};
#pragma warning restore 169

		[TestCaseSource("singleton")]
		public void Shortcuts_Singletons_FromFactoryMethod(Currency shortcut, CurrencyIsoCode isoCode, string isoSymbol)
		{
			Assert.That(shortcut, Is.SameAs(Currency.Get(isoCode)));
			Assert.That(shortcut, Is.SameAs(Currency.Get(isoSymbol)));
			Assert.That(Currency.Get(isoCode), Is.SameAs(Currency.Get(isoSymbol)));

			Currency tried;
			Assert.That(Currency.TryGet(isoCode, out tried), Is.True);
			Assert.That(tried, Is.SameAs(shortcut));

			Assert.That(Currency.TryGet(isoSymbol, out tried), Is.True);
			Assert.That(tried, Is.SameAs(shortcut));
		}

#pragma warning disable 169
		private static readonly object[] singleton = new object[]
		{
			new object[] {Currency.Aud, CurrencyIsoCode.AUD, "AUD"},
			new object[] {Currency.Cad, CurrencyIsoCode.CAD, "CAD"},
			new object[] {Currency.Chf, CurrencyIsoCode.CHF, "CHF"},
			new object[] {Currency.Dkk, CurrencyIsoCode.DKK, "DKK"},
			new object[] {Currency.Eur, CurrencyIsoCode.EUR, "EUR"},
			new object[] {Currency.Gbp, CurrencyIsoCode.GBP, "GBP"},
			new object[] {Currency.Hkd, CurrencyIsoCode.HKD, "HKD"},
			new object[] {Currency.Huf, CurrencyIsoCode.HUF, "HUF"},
			new object[] {Currency.Inr, CurrencyIsoCode.INR, "INR"},
			new object[] {Currency.Jpy, CurrencyIsoCode.JPY, "JPY"},
			new object[] {Currency.Mxn, CurrencyIsoCode.MXN, "MXN"},
			new object[] {Currency.Myr, CurrencyIsoCode.MYR, "MYR"},
			new object[] {Currency.Nok, CurrencyIsoCode.NOK, "NOK"},
			new object[] {Currency.Nzd, CurrencyIsoCode.NZD, "NZD"},
			new object[] {Currency.Rub, CurrencyIsoCode.RUB, "RUB"},
			new object[] {Currency.Sek, CurrencyIsoCode.SEK, "SEK"},
			new object[] {Currency.Sgd, CurrencyIsoCode.SGD, "SGD"},
			new object[] {Currency.Thb, CurrencyIsoCode.THB, "THB"},
			new object[] {Currency.Usd, CurrencyIsoCode.USD, "USD"},
			new object[] {Currency.Xts, CurrencyIsoCode.XTS, "XTS"},
			new object[] {Currency.Xxx, CurrencyIsoCode.XXX, "XXX"},
			new object[] {Currency.Zar, CurrencyIsoCode.ZAR, "ZAR"},
		};
#pragma warning restore 169

		[Test]
		public void Get_UndefinedIsoCode_Exception()
		{
			CurrencyIsoCode notDefined = (CurrencyIsoCode)5000;
			Assert.That(() => Currency.Get(notDefined),
				Throws.InstanceOf<InvalidEnumArgumentException>().With.Message.StringContaining("5000"));
		}

		[Test]
		public void Get_DefinedIsoSymbol_CaseInsensitive()
		{
			Assert.That(Currency.Get(CurrencyIsoCode.USD),
				Is.SameAs(Currency.Get("USD"))
				.And.SameAs(Currency.Get("usd"))
				.And.SameAs(Currency.Get("Usd"))
				.And.SameAs(Currency.Get("uSD"))
				.And.SameAs(Currency.Get("uSd")));
		}

		[Test]
		public void Get_UndefinedIsoSymbol_Exception()
		{
			string notDefined = "5000";
			Assert.That(() => Currency.Get(notDefined),
				Throws.InstanceOf<InvalidEnumArgumentException>().With.Message.StringContaining("5000"));
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
			RegionInfo bulgaria = new RegionInfo(bulgarian.LCID);

			Assert.That(() => Currency.Get(bulgarian), Throws.InstanceOf<InvalidEnumArgumentException>().With.Message.StringContaining("BGL"),
				"Framework returns wrong ISOCurrencySymbol (BGL instead of BGN)");
			Currency lev = Currency.Get(CurrencyIsoCode.BGN);

			Assert.That(lev.IsoSymbol, Is.Not.EqualTo(bulgaria.ISOCurrencySymbol));
			Assert.That(lev.NativeName, Is.Not.EqualTo(bulgaria.CurrencyNativeName));
			Assert.That(lev.Symbol, Is.EqualTo(bulgaria.CurrencySymbol));
		}

		[Test]
		public void Get_RegionWithUpdatedInformation_Currency()
		{
			RegionInfo denmark = new RegionInfo("DK");
			Assert.That(Currency.Get(denmark), Is.SameAs(Currency.Dkk));
		}

		[Test, Platform(Include = "Net-2.0")]
		public void Get_RegionWithOutdatedInformation_Exception()
		{
			RegionInfo SerbioAndMontenegro = new RegionInfo("CS");
			Assert.That(() => Currency.Get(SerbioAndMontenegro), Throws.InstanceOf<InvalidEnumArgumentException>().With.Message.StringContaining("CSD"));
		}

		[Test]
		public void Get_NotAShortcut_Currency()
		{
			Currency notAShortcut = Currency.Get(CurrencyIsoCode.NAD);
			Assert.That(notAShortcut, Is.SameAs(Currency.Get("NAD")));

			Assert.That(notAShortcut.IsoCode, Is.EqualTo(CurrencyIsoCode.NAD));
			Assert.That(notAShortcut.IsoSymbol, Is.EqualTo("NAD"));
			Assert.That(notAShortcut.Symbol, Is.EqualTo("$"));
			Assert.That(notAShortcut.EnglishName, Is.EqualTo("Namibian Dollar"));
		}

		[Test]
		public void TryGet_UndefinedIsoCode_False()
		{
			CurrencyIsoCode notDefined = (CurrencyIsoCode)5000;
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
			Assert.That(Currency.TryGet((string)null, out tried), Is.False);
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
			RegionInfo bulgaria = new RegionInfo(bulgarian.LCID);

			Currency lev;
			Assert.That(Currency.TryGet(bulgarian, out lev), Is.False, "Framework returns wrong ISOCurrencySymbol (BGL instead of BGN)");
			Assert.That(lev, Is.Null);

			Currency.TryGet(CurrencyIsoCode.BGN, out lev);

			Assert.That(lev.IsoSymbol, Is.Not.EqualTo(bulgaria.ISOCurrencySymbol));
			Assert.That(lev.NativeName, Is.Not.EqualTo(bulgaria.CurrencyNativeName));
			Assert.That(lev.Symbol, Is.EqualTo(bulgaria.CurrencySymbol));
		}

		[Test]
		public void TryGet_RegionWithUpdatedInformation_True()
		{
			RegionInfo denmark = new RegionInfo("DK");
			Currency dkk;

			Assert.That(Currency.TryGet(denmark, out dkk), Is.True);
			Assert.That(dkk, Is.SameAs(Currency.Dkk));
		}

		[Test, Platform(Include = "Net-2.0")]
		public void TryGet_RegionWithOutdatedInformation_False()
		{
			RegionInfo SerbioAndMontenegro = new RegionInfo("CS");
			Currency rsd;
			Assert.That(Currency.TryGet(SerbioAndMontenegro, out rsd), Is.False);
			Assert.That(rsd, Is.Null);
		}

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

		#region serialization

		[Test]
		public void CanBe_BinarySerialized()
		{
			Assert.That(Currency.Dollar, Must.Be.BinarySerializable<Currency>(Is.SameAs));
		}

		[Test]
		public void CanBe_XmlSerialized()
		{
			Assert.That(Currency.Dollar, Must.Be.XmlSerializable<Currency>());
		}

		[Test]
		public void CanBe_XmlDeserializable()
		{
			string serializedDollar =
				"<currency xmlns=\"urn:nmoneys\">" +
				"<isoCode>USD</isoCode>" +
				"</currency>";
			Assert.That(serializedDollar, Must.Be.XmlDeserializableInto(Currency.Dollar));
		}

		[Test]
		public void CannotBe_XamlSerialized()
		{
			XamlSerializer serializer = new XamlSerializer();
			string xaml = serializer.Serialize(Currency.Dollar);
			Assert.That(() => serializer.Deserialize<Currency>(xaml), Throws.InstanceOf<XamlParseException>());
		}

		[Test]
		public void CanBe_DataContractSerialized()
		{
			Assert.That(Currency.Dollar, Must.Be.DataContractSerializable<Currency>());
		}

		[Test]
		public void CanBe_DataContractDeserializable()
		{
			string serializedDollar =
				"<currency xmlns=\"urn:nmoneys\">" +
				"<isoCode>USD</isoCode>" +
				"</currency>";
			Assert.That(serializedDollar, Must.Be.DataContractDeserializableInto(Currency.Dollar));
		}

		[Test]
		public void CannotBe_DataContractJsonSerialized()
		{
			Assert.That(Currency.Dollar, Must.Not.Be.DataContractJsonSerializable<Currency>());
		}

		[Test]
		public void CanBe_JsonSerialized()
		{
			Assert.That(Currency.Dollar, Must.Be.JsonSerializable<Currency>(Is.SameAs));
		}

		[Test]
		public void CanBe_JsonDeserializable()
		{
			string serializedDollar = "{\"isoCode\":\"USD\"}";
			Assert.That(serializedDollar, Must.Be.JsonDeserializableInto(Currency.Dollar));
		}

		#endregion

		[Test]
		public void Initialize_AllCurrenciesGetInitialized()
		{
			Assert.DoesNotThrow(Currency.InitializeAllCurrencies);
		}

		[Test]
		public void NumericValue_HoldsTheValueOfTheCode()
		{
			Assert.That(Currency.Dollar.NumericCode, Is.EqualTo(840));
			Assert.That(Currency.Euro.NumericCode, Is.EqualTo(978));
		}

		[Test]
		public void PaddedNumericValue_ThreeDigitedValue_NoLeadingZeros()
		{
			Assert.That(Currency.Dollar.PaddedNumericCode, Is.EqualTo("840"));
		}

		[Test]
		public void PaddedNumericValue_TwoDigitedValue_OneLeadingZero()
		{
			Assert.That(Currency.Get(CurrencyIsoCode.BZD).PaddedNumericCode, Is.EqualTo("084"));
		}

		[Test]
		public void PaddedNumericValue_OneDigitedValue_OneLeadingZero()
		{
			Assert.That(Currency.Get(CurrencyIsoCode.ALL).PaddedNumericCode, Is.EqualTo("008"));
		}

		[Test]
		public void CompareTo_NonGeneric_AccordingToSpec()
		{
			object to = null;
			Assert.That(Currency.Xts.CompareTo(to), Is.GreaterThan(0));

			to = Currency.Xts;
			Assert.That(Currency.Xts.CompareTo(to), Is.EqualTo(0));
			to = Currency.Xxx;
			Assert.That(Currency.Xts.CompareTo(to), Is.LessThan(0));
			to = Currency.Aud;
			Assert.That(Currency.Xts.CompareTo(to), Is.GreaterThan(0));

			Exception notACurrency = new Exception();
			Assert.That(() => Currency.Xts.CompareTo(notACurrency), Throws
				.ArgumentException
				.With.Message.StringContaining("Currency"));
		}

		[Test]
		public void CompareTo_Generic_AccordingToSpec()
		{
			Currency to = null;
			Assert.That(Currency.Xts.CompareTo(to), Is.GreaterThan(0));

			to = Currency.Xts;
			Assert.That(Currency.Xts.CompareTo(to), Is.EqualTo(0));
			to = Currency.Xxx;
			Assert.That(Currency.Xts.CompareTo(to), Is.LessThan(0));
			to = Currency.Aud;
			Assert.That(Currency.Xts.CompareTo(to), Is.GreaterThan(0));
		}
	}
}