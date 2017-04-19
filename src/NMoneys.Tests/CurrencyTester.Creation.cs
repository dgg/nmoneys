using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using NMoneys.Support;
using NMoneys.Tests.CustomConstraints;
using NMoneys.Tests.Support;
using NUnit.Framework;
using Testing.Commons;

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
			Assert.That(() => Currency.Get(notDefined), Throws.InstanceOf<InvalidEnumArgumentException>().With.Message.Contains("5000"));
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
			Assert.That(() => Currency.Get(notDefined), Throws.InstanceOf<InvalidEnumArgumentException>().With.Message.Contains("5000"));
		}

		[Test]
		public void Get_DefinedCurrencyForCulture_Currency()
		{
			CultureInfo spanish = CultureInfo.GetCultureInfo("es-ES");
			Assert.That(Currency.Get(spanish), Is.SameAs(Currency.Eur));
		}

		[Test]
		public void Get_RegionWithUpdatedInformation_Currency()
		{
			var denmark = new RegionInfo("DK");
			Assert.That(Currency.Get(denmark), Is.SameAs(Currency.Dkk));
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

		[Test, TestCaseSource(typeof(Obsolete), nameof(Obsolete.ThreeLetterIsoCodes))]
		public void Get_ObsoleteCurrencyIsoCode_EventRaised(string threeLetterIsoCode)
		{
			Action getObsolete = () => Currency.Get(Enumeration.Parse<CurrencyIsoCode>(threeLetterIsoCode));
			Assert.That(getObsolete, Must.Raise.ObsoleteEvent());
		}

		[Test, TestCaseSource(typeof(Obsolete), nameof(Obsolete.ThreeLetterIsoCodes))]
		public void Get_ObsoleteCurrencyCode_EventRaised(string threeLetterIsoCode)
		{
			Action getObsolete = () => Currency.Get(threeLetterIsoCode);
			Assert.That(getObsolete, Must.Raise.ObsoleteEvent());
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

		[Test]
		public void TryGet_RegionWithUpdatedInformation_True()
		{
			var denmark = new RegionInfo("DK");
			Currency dkk;

			Assert.That(Currency.TryGet(denmark, out dkk), Is.True);
			Assert.That(dkk, Is.SameAs(Currency.Dkk));
		}

		[Test, TestCaseSource(typeof(Obsolete), nameof(Obsolete.ThreeLetterIsoCodes))]
		public void TryGet_ObsoleteCurrencyIsoCode_EventRaised(string threeLetterIsoCode)
		{
			Currency c;
			Action tryGetObsolete = () => Currency.TryGet(Enumeration.Parse<CurrencyIsoCode>(threeLetterIsoCode), out c);
			Assert.That(tryGetObsolete, Must.Raise.ObsoleteEvent());
		}

		[Test, TestCaseSource(typeof(Obsolete), nameof(Obsolete.ThreeLetterIsoCodes))]
		public void TryGet_ObsoleteCurrencyCode_EventRaised(string threeLetterIsoCode)
		{
			Currency c;
			Action tryGetObsolete = () => Currency.TryGet(threeLetterIsoCode, out c);
			Assert.That(tryGetObsolete, Must.Raise.ObsoleteEvent());
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
			Assert.That(iterateAllCurrencies, Must.Raise.ObsoleteEvent(ObsoleteCurrencies.Count));
		}

		#endregion

		#region change SEK

		[Test]
		public void SEK_GroupSeparator_IsDotAgain()
		{
			Assert.That(Currency.Sek.GroupSeparator, Is.EqualTo(".").And
				.Not.EqualTo(" "));
		}

		#endregion
	}
}