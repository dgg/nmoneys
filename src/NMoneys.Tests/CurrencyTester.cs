using System;
using System.Linq;
using NMoneys.Support;
using NUnit.Framework;

namespace NMoneys.Tests
{
	[TestFixture]
	public partial class CurrencyTester
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
			new object[] {Currency.Gbp, CurrencyIsoCode.GBP, "GBP", "£", "Pound Sterling"},
			new object[] {Currency.Hkd, CurrencyIsoCode.HKD, "HKD", "HK$", "Hong Kong Dollar"},
			new object[] {Currency.Huf, CurrencyIsoCode.HUF, "HUF", "Ft", "Forint"},
			new object[] {Currency.Inr, CurrencyIsoCode.INR, "INR", "₹", "Indian Rupee"},
			new object[] {Currency.Jpy, CurrencyIsoCode.JPY, "JPY", "¥", "Yen"},
			new object[] {Currency.Mxn, CurrencyIsoCode.MXN, "MXN", "$", "Mexican Peso"},
			// overriden for 2.0 "R"
			new object[] {Currency.Myr, CurrencyIsoCode.MYR, "MYR", "RM", "Malaysian Ringgit"},
			new object[] {Currency.Nok, CurrencyIsoCode.NOK, "NOK", "kr", "Norwegian Krone"},
			new object[] {Currency.Nzd, CurrencyIsoCode.NZD, "NZD", "$", "New Zealand Dollar"},
			new object[] {Currency.Rub, CurrencyIsoCode.RUB, "RUB", "р.", "Russian Ruble"},
			new object[] {Currency.Sek, CurrencyIsoCode.SEK, "SEK", "kr", "Swedish Krona"},
			new object[] {Currency.Sgd, CurrencyIsoCode.SGD, "SGD", "$", "Singapore Dollar"},
			new object[] {Currency.Thb, CurrencyIsoCode.THB, "THB", "฿", "Baht"},
			new object[] {Currency.Usd, CurrencyIsoCode.USD, "USD", "$", "US Dollar"},
			new object[] {Currency.Xts, CurrencyIsoCode.XTS, "XTS", "¤", "Test currency"},
			new object[] {Currency.Xxx, CurrencyIsoCode.XXX, "XXX", "¤", "No currency"},
			new object[] {Currency.Zar, CurrencyIsoCode.ZAR, "ZAR", "R", "Rand"},
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

		#endregion

		#region Issue 29. Allow empty currency symbols

		[Test]
		public void Get_SymbolLessCurrency_NoException()
		{
			Assert.That(() => Currency.Get(CurrencyIsoCode.CVE), Throws.Nothing);
		}

		#endregion
	}
}