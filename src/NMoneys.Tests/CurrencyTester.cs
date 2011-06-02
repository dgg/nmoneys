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
			Assert.That(shortcut.AlphabeticCode, Is.EqualTo(isoSymbol));
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
			RegionInfo SerbiaAndMontenegro = new RegionInfo("CS");
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
			Assert.That(notAShortcut.EnglishName, Is.EqualTo("Namibian Dollar"));
		}

		[Test]
		public void Get_ObsoleteCurrencyIsoCode_EventRaised()
		{
			Action getObsolete = () => Currency.Get(Enumeration.Parse<CurrencyIsoCode>("EEK"));
			Assert.That(getObsolete, Must.RaiseObsoleteEvent.Once());
		}

		[Test]
		public void Get_ObsoleteCurrencyCode_EventRaised()
		{
			Action getObsolete = () => Currency.Get("EEK");
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
			RegionInfo SerbiaAndMontenegro = new RegionInfo("CS");
			Currency rsd;
			Assert.That(Currency.TryGet(SerbiaAndMontenegro, out rsd), Is.False);
			Assert.That(rsd, Is.Null);
		}

		[Test]
		public void TryGet_ObsoleteCurrencyIsoCode_EventRaised()
		{
			Currency c;
			Action tryGetObsolete = () => Currency.TryGet(Enumeration.Parse<CurrencyIsoCode>("EEK"), out c);
			Assert.That(tryGetObsolete, Must.RaiseObsoleteEvent.Once());
		}

		[Test]
		public void TryGet_ObsoleteCurrencyCode_EventRaised()
		{
			Currency c;
			Action tryGetObsolete = () => Currency.TryGet("EEK", out c);
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
			Assert.That(iterateAllCurrencies, Must.RaiseObsoleteEvent.Once());
		}

		#region serialization

		[Test]
		public void CanBe_BinarySerialized()
		{
			Assert.That(Currency.Dollar, Must.Be.BinarySerializable<Currency>(Is.EqualTo));
		}

		[Test]
		public void BinaryDeserialization_OfObsoleteCurrency_RaisesEvent()
		{
			using (var serializer = new OneGoBinarySerializer<Currency>())
			{
				var obsolete = Currency.Get("EEK");
				serializer.Serialize(obsolete);
				Action deserializeObsolete = () => serializer.Deserialize();
				Assert.That(deserializeObsolete, Must.RaiseObsoleteEvent.Once());
			}
		}

		[Test]
		public void BinaryDeserialization_DoesNotPreservesInstanceUniqueness()
		{
			using (var serializer = new OneGoBinarySerializer<Currency>())
			{
				Currency usd = Currency.Get("USD");
				serializer.Serialize(usd);
				Assert.That(serializer.Deserialize(), Is.EqualTo(usd));
			}
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
		public void XmlDeserialization_OfObsoleteCurrency_RaisesEvent()
		{
			using (var serializer = new OneGoXmlSerializer<Currency>())
			{
				var obsolete = Currency.Get("EEK");
				serializer.Serialize(obsolete);
				Action deserializeObsolete = () => serializer.Deserialize();
				Assert.That(deserializeObsolete, Must.RaiseObsoleteEvent.Once());
			}
		}

		[Test]
		public void XmlDeserialization_DoesNotPreserveInstanceUniqueness()
		{
			using (var serializer = new OneGoXmlSerializer<Currency>())
			{
				Currency usd = Currency.Get("USD");
				serializer.Serialize(usd);
				Assert.That(serializer.Deserialize(), Is.Not.SameAs(usd)
					.And.EqualTo(usd));
			}
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
		public void DataContractDeserialization_OfObsoleteCurrency_RaisesEvent()
		{
			using (var serializer = new OneGoDataContractSerializer<Currency>())
			{
				var obsolete = Currency.Get("EEK");
				serializer.Serialize(obsolete);
				Action deserializeObsolete = () => serializer.Deserialize();
				Assert.That(deserializeObsolete, Must.RaiseObsoleteEvent.Once());
			}
		}

		[Test]
		public void DataContractDeserialization_DoesNotPreserveInstanceUniqueness()
		{
			using (var serializer = new OneGoDataContractSerializer<Currency>())
			{
				Currency usd = Currency.Get("USD");
				serializer.Serialize(usd);
				Assert.That(serializer.Deserialize(), Is.Not.SameAs(usd)
					.And.EqualTo(usd));
			}
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

		[Test]
		public void JsonDeserialization_OfObsoleteCurrency_RaisesEvent()
		{
			using (var serializer = new OneGoJsonSerializer<Currency>())
			{
				var obsolete = Currency.Get("EEK");
				serializer.Serialize(obsolete);
				Action deserializeObsolete = () => serializer.Deserialize();
				Assert.That(deserializeObsolete, Must.RaiseObsoleteEvent.Once());
			}
		}

		[Test]
		public void JsonDeserialization_DoesPreserveInstanceUniqueness()
		{
			using (var serializer = new OneGoJsonSerializer<Currency>())
			{
				Currency usd = Currency.Get("USD");
				serializer.Serialize(usd);
				Assert.That(serializer.Deserialize(), Is.SameAs(usd));
			}
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

		[Test]
		public void GreaterThan_Generic_AccordingToSpec()
		{
			Currency to = null;
			Assert.That(Currency.Xts > to, Is.True);

			to = Currency.Xts;
			Assert.That(Currency.Xts > to, Is.False);
			to = Currency.Xxx;
			Assert.That(Currency.Xts > to, Is.False);
			to = Currency.Aud;
			Assert.That(Currency.Xts > to, Is.True);
		}

		[Test]
		public void LessThan_Generic_AccordingToSpec()
		{
			Currency to = null;
			Assert.That(Currency.Xts < to, Is.False);

			to = Currency.Xts;
			Assert.That(Currency.Xts < to, Is.False);
			to = Currency.Xxx;
			Assert.That(Currency.Xts < to, Is.True);
			to = Currency.Aud;
			Assert.That(Currency.Xts < to, Is.False);
		}

		[Test]
		public void ObsoleteCurrencies_AreConsistent()
		{
			Currency[] obsoleteCurrencies = Currency.FindAll().Where(c => c.IsObsolete).ToArray();
			CurrencyIsoCode[] obsoleteCodes = obsoleteCurrencies.Select(c => c.IsoCode).ToArray();

			// all currencies are in the cache of obsolete currencies
			Assert.That(obsoleteCurrencies, Has.All.Matches<Currency>(ObsoleteCurrencies.IsObsolete));

			// there no more obsolete currencies than the ones in the cache
			Currency[] obsoleteCurrenciesInCache = Currency.FindAll().Where(ObsoleteCurrencies.IsObsolete).ToArray();
			Assert.That(obsoleteCurrenciesInCache, Is.EquivalentTo(obsoleteCurrencies));

			// all currency codes are marked as obsolete
			Assert.That(obsoleteCodes, Has.All.Matches<CurrencyIsoCode>(Enumeration.HasAttribute<CurrencyIsoCode, ObsoleteAttribute>));

			// there no more currency codes marked as obsolete than obsolete currencies
			Assert.That(obsoleteCodes, Is.EquivalentTo(
				Enumeration.GetValues<CurrencyIsoCode>()
					.Where(Enumeration.HasAttribute<CurrencyIsoCode, ObsoleteAttribute>)));
		}

		[Test, TestCaseSource("HtmlEntitySpec")]
		public void SomeCurrencies_HaveAnHtmlEntity(string isoSymbol, string entityName, string entityNumber)
		{
			Currency withHtmlEntity = Currency.Get(isoSymbol);
			Assert.That(withHtmlEntity.Entity, Must.Be.EntityWith(entityName, entityNumber));
		}

		[Test]
		public void MostCurrencies_DoNotHaveAnHtmlEntity()
		{
			Assert.That(Currency.Dkk.Entity, Is.SameAs(CharacterReference.Empty));
			Assert.That(Currency.Nok.Entity, Is.SameAs(CharacterReference.Empty));
		}

#pragma warning disable 169
		private static object[] HtmlEntitySpec = new[]
		{
			new object[]{"ANG", "&fnof;", "&#402;" },
			new object[]{"AWG", "&fnof;", "&#402;" },
			
			new object[]{"GBP", "&pound;", "&#163;" },
			new object[]{"SHP", "&pound;", "&#163;" },
			new object[]{"FKP", "&pound;", "&#163;" },

			new object[]{"JPY", "&yen;", "&#165;" },
			new object[]{"CNY", "&yen;", "&#165;" },

			new object[]{"EUR", "&euro;", "&#8364;" },
			new object[]{"CHE", "&euro;", "&#8364;" },

			new object[]{"XXX", "&curren;", "&#164;" },
			new object[]{"XBA", "&curren;", "&#164;" },
			new object[]{"XBB", "&curren;", "&#164;" },
			new object[]{"XBC", "&curren;", "&#164;" },
			new object[]{"XBD", "&curren;", "&#164;" },
			new object[]{"XDR", "&curren;", "&#164;" },
			new object[]{"XPT", "&curren;", "&#164;" },
			new object[]{"XTS", "&curren;", "&#164;" },

			new object[]{"GHS", "&cent;", "&#162;" },
		};
#pragma warning restore 169

		#region Issue 16. Case sensitivity. Currency instances can be obtained by any casing of the IsoCode (Alphbetic code)

		[Test]
		public void Get_ByIsoCode_IsCaseInsensitive()
		{
			Assert.That(Currency.Get("XBA"), Is.SameAs(Currency.Get("xBa")));
		}

		[Test]
		public void TryGet_ByIsoCode_IsCaseInsensitive()
		{
			Currency upper, mixed;
			Assert.That(Currency.TryGet("XBA", out upper), Is.True);
			Assert.That(Currency.TryGet("xBa", out mixed), Is.True);

			Assert.That(upper, Is.SameAs(mixed));
		}

		[Test]
		public void XmlDeserialization_IsCaseInsentive()
		{
			string serializedDollar =
				"<currency xmlns=\"urn:nmoneys\">" +
				"<isoCode>uSd</isoCode>" +
				"</currency>";
			Assert.That(serializedDollar, Must.Be.XmlDeserializableInto(Currency.Dollar));
		}

		[Test]
		public void DataContractDeserialization_IsCaseInsensitive()
		{
			string serializedDollar =
				"<currency xmlns=\"urn:nmoneys\">" +
				"<isoCode>uSd</isoCode>" +
				"</currency>";
			Assert.That(serializedDollar, Must.Be.DataContractDeserializableInto(Currency.Dollar));
		}

		[Test]
		public void JsonDeserialization_IsCaseInsensitive()
		{
			string serializedDollar = "{\"isoCode\":\"uSd\"}";
			Assert.That(serializedDollar, Must.Be.JsonDeserializableInto(Currency.Dollar));
		}

		#endregion

		#region ParseCode

		[Test]
		public void ParseCode_Defined_UpperCased_AlphabeticCode_CodeParsed()
		{
			Assert.That(Currency.ParseCode("USD"), Is.EqualTo(CurrencyIsoCode.USD));
		}

		[Test]
		public void ParseCode_Defined_LowerCased_AlphabeticCode_CodeParsed()
		{
			Assert.That(Currency.ParseCode("eur"), Is.EqualTo(CurrencyIsoCode.EUR));
		}

		[Test]
		public void ParseCode_Defined_MixedCased_AlphabeticCode_CodeParsed()
		{
			Assert.That(Currency.ParseCode("NoK"), Is.EqualTo(CurrencyIsoCode.NOK));
		}

		[Test]
		public void ParseCode_Defined_NumericCode_CodeParsed()
		{
			Assert.That(Currency.ParseCode("999"), Is.EqualTo(CurrencyIsoCode.XXX));
		}

		[Test]
		public void ParseCode_Null_Exception()
		{
			Assert.That(() => Currency.ParseCode(null), Throws.InstanceOf<ArgumentNullException>());
		}

		[Test]
		public void ParseCode_Undefined_AlphabeticCode_Exception()
		{
			Assert.That(() => Currency.ParseCode("notAnIsoCode"), Throws.InstanceOf<InvalidEnumArgumentException>());
		}

		[Test]
		public void ParseCode_Undefined_NumericCode_Exception()
		{
			Assert.That(() => Currency.ParseCode("0"), Throws.InstanceOf<InvalidEnumArgumentException>());
		}

		[Test]
		public void ParseCode_Overflowing_NumericCode_Exception()
		{
			long overflowingCode = short.MinValue + 1L;
			Assert.That(() => Currency.ParseCode(overflowingCode.ToString()), Throws.InstanceOf<InvalidEnumArgumentException>());
		}

		#endregion

		#region TryParseCode

		[Test]
		public void TryParseCode_Defined_UpperCased_AlphabeticCode_CodeParsed()
		{
			CurrencyIsoCode? parsed;
			Assert.That(Currency.TryParseCode("USD", out parsed), Is.True);
			Assert.That(parsed, Is.EqualTo(CurrencyIsoCode.USD));

		}

		[Test]
		public void TryParseCode_Defined_LowerCased_AlphabeticCode_CodeParsed()
		{
			CurrencyIsoCode? parsed;
			Assert.That(Currency.TryParseCode("eur", out parsed), Is.True);
			Assert.That(parsed, Is.EqualTo(CurrencyIsoCode.EUR));
		}

		[Test]
		public void TryParseCode_Defined_MixedCased_AlphabeticCode_CodeParsed()
		{
			CurrencyIsoCode? parsed;
			Assert.That(Currency.TryParseCode("NoK", out parsed), Is.True);
			Assert.That(parsed, Is.EqualTo(CurrencyIsoCode.NOK));
		}

		[Test]
		public void TryParseCode_Defined_NumericCode_CodeParsed()
		{
			CurrencyIsoCode? parsed;
			Assert.That(Currency.TryParseCode("999", out parsed), Is.True);
			Assert.That(parsed, Is.EqualTo(CurrencyIsoCode.XXX));
		}

		[Test]
		public void TryParseCode_Null_Exception()
		{
			CurrencyIsoCode? parsed;
			Assert.That(Currency.TryParseCode(null, out parsed), Is.False);
			Assert.That(parsed, Is.Null);
		}

		[Test]
		public void TryParseCode_Undefined_AlphabeticCode_Exception()
		{
			CurrencyIsoCode? parsed;
			Assert.That(Currency.TryParseCode("notAnIsoCode", out parsed), Is.False);
			Assert.That(parsed, Is.Null);
		}

		[Test]
		public void TryParseCode_Undefined_NumericCode_Exception()
		{
			CurrencyIsoCode? parsed;
			Assert.That(Currency.TryParseCode("0", out parsed), Is.False);
			Assert.That(parsed, Is.Null);
		}

		[Test]
		public void TryParseCode_Overflowing_NumericCode_Exception()
		{
			CurrencyIsoCode? parsed;
			long overflowingCode = short.MinValue + 1L;
			Assert.That(Currency.TryParseCode(overflowingCode.ToString(), out parsed), Is.False);
			Assert.That(parsed, Is.Null);
		}

		#endregion
	}
}