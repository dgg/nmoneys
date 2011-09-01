using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using NMoneys.Demo.CodeProject.Support;
using NUnit.Framework;

namespace NMoneys.Demo.CodeProject
{
	[TestFixture]
	public class CurrencyDemo
	{
		[Test]
		public void popular_currency_instances_can_be_obtained_from_static_accessors()
		{
			Assert.That(Currency.Usd, Is.Not.Null.And.InstanceOf<Currency>());
			Assert.That(Currency.Eur, Is.Not.Null.And.InstanceOf<Currency>());
			Assert.That(Currency.Dkk, Is.Not.Null.And.InstanceOf<Currency>());
			Assert.That(Currency.Xxx, Is.Not.Null.And.InstanceOf<Currency>());
		}

		[Test]
		public void very_popular_currency_instances_can_be_obtained_from_static_aliases()
		{
			Assert.That(Currency.Usd, Is.SameAs(Currency.Dollar));
			Assert.That(Currency.Eur, Is.SameAs(Currency.Euro));
			Assert.That(Currency.Gbp, Is.SameAs(Currency.Pound));
		}

		[Test]
		public void currency_instances_can_be_obtained_from_its_code_enum()
		{
			Assert.That(Currency.Get(CurrencyIsoCode.ZAR), Is.InstanceOf<Currency>());
		}

		[Test]
		public void currency_instances_can_be_obtained_from_its_code_string()
		{
			Assert.That(Currency.Get("eur"), Is.Not.Null);
			Assert.That(Currency.Get("EUR"), Is.Not.Null);
		}

		[Test]
		public void currency_instances_can_be_obtained_from_a_CultureInfo_instance()
		{
			CultureInfo swedish = CultureInfo.GetCultureInfo("sv-SE");
			Assert.That(Currency.Get(swedish), Is.EqualTo(Currency.Sek));
		}

		[Test]
		public void currency_instances_can_be_obtained_from_a_RegionInfo_instance()
		{
			RegionInfo spain = new RegionInfo("es");
			Assert.That(Currency.Get(spain), Is.EqualTo(Currency.Eur));
		}

		[Test]
		public void Get_throws_if_currency_cannot_be_found()
		{
			CurrencyIsoCode notDefined = (CurrencyIsoCode)0;
			Assert.That(()=> Currency.Get(notDefined), Throws.InstanceOf<InvalidEnumArgumentException>());
			Assert.That(()=> Currency.Get("notAnIsoCode"), Throws.InstanceOf<InvalidEnumArgumentException>());
			CultureInfo neutralCulture = CultureInfo.GetCultureInfo("da");
			Assert.That(() => Currency.Get(neutralCulture), Throws.InstanceOf<ArgumentException>());
		}

		[Test]
		public void currency_instances_can_be_obtained_with_a_try_do_pattern()
		{
			Currency currency;
			Assert.That(Currency.TryGet(CurrencyIsoCode.ZAR, out currency), Is.True);
			Assert.That(currency, Is.Not.Null.And.InstanceOf<Currency>());

			Assert.That(Currency.TryGet("zar", out currency), Is.True);
			Assert.That(currency, Is.Not.Null.And.InstanceOf<Currency>());

			Assert.That(Currency.TryGet(CultureInfo.GetCultureInfo("en-ZA"), out currency), Is.True);
			Assert.That(currency, Is.Not.Null.And.InstanceOf<Currency>());

			Assert.That(Currency.TryGet(new RegionInfo("ZA"), out currency), Is.True);
			Assert.That(currency, Is.Not.Null.And.InstanceOf<Currency>());
		}

		[Test]
		public void TryGet_does_not_throw_if_currency_cannot_be_found()
		{
			Currency currency;
			CurrencyIsoCode notDefined = (CurrencyIsoCode)0;
			Assert.That(Currency.TryGet(notDefined, out currency), Is.False);
			Assert.That(currency, Is.Null);

			Assert.That(Currency.TryGet("notAnIsoCode", out currency), Is.False);
			Assert.That(currency, Is.Null);

			CultureInfo neutralCulture = CultureInfo.GetCultureInfo("da");
			Assert.That(Currency.TryGet(neutralCulture, out currency), Is.False);
			Assert.That(currency, Is.Null);
		}

		[Test]
		public void Currency_instances_follow_a_flightweight_like_pattern_being_exactly_equal()
		{
			Assert.That(Currency.Dkk, Is.SameAs(Currency.Get("dkk")));

			Currency pesoUruguayo;
			Currency.TryGet("UYU", out pesoUruguayo);
			RegionInfo uruguay = new RegionInfo("UY");
			Assert.That(Currency.Get(uruguay), Is.SameAs(pesoUruguayo));
		}

		[Test]
		public void instances_of_deprecated_currencies_can_still_be_obtained()
		{
			Currency deprecated = Currency.Get("EEK");
			Assert.That(deprecated, Is.Not.Null.And.InstanceOf<Currency>());
			Assert.That(deprecated.IsObsolete, Is.True);
		}

		[Test]
		public void whenever_a_deprecated_currency_is_obtained_an_event_is_raised()
		{
			bool called = false;
			CurrencyIsoCode obsolete = CurrencyIsoCode.XXX;
			EventHandler<ObsoleteCurrencyEventArgs> callback = (sender, e) =>
			{
				called = true;
				obsolete = e.Code;
			};

			try
			{
				Currency.ObsoleteCurrency += callback;
				Currency.Get("EEK");

				Assert.That(called, Is.True);
				Assert.That(obsolete.ToString(), Is.EqualTo("EEK"));
				Assert.That(obsolete.AsAttributeProvider(), Has.Attribute<ObsoleteAttribute>());
			}
			// DO unsubscribe from global events whenever listening isnot needed anymore
			finally
			{
				Currency.ObsoleteCurrency -= callback;
			}
		}

		[Test]
		public void all_currencies_can_be_obtained_and_linq_operators_applied()
		{
			Assert.That(Currency.FindAll(), Is.Not.Null.And.All.InstanceOf<Currency>());

			var allCurrenciesWithoutMinorUnits = Currency.FindAll().Where(c => c.SignificantDecimalDigits == 0);
			Assert.That(allCurrenciesWithoutMinorUnits, Is.Not.Empty.And.Contains(Currency.Jpy));
		}

		[Test]
		public void currencies_can_be_compared()
		{
			Assert.That(Currency.Usd.Equals(Currency.Gbp), Is.False);

			Assert.That(Currency.Eur.CompareTo(Currency.Usd), Is.LessThan(0), "symbol comparison is used");

			Assert.That(Currency.None > Currency.Dollar, Is.True, "again, only symbols involved");
		}

		[Test]
		public void whats_in_a_currency_anyway()
		{
			Currency euro = Currency.Eur;

			Assert.That(euro.IsObsolete, Is.False);
			Assert.That(euro.IsoCode, Is.EqualTo(CurrencyIsoCode.EUR));
			Assert.That(euro.IsoSymbol, Is.EqualTo("EUR"));
			Assert.That(euro.NativeName, Is.EqualTo("Euro"), "capitalized in the default instance");
			Assert.That(euro.NumericCode, Is.EqualTo(978));
			Assert.That(euro.PaddedNumericCode, Is.EqualTo("978"), "a string of 3 characters containing the numeric code and zeros if needed");
			Assert.That(euro.Symbol, Is.EqualTo("€"));
		}

		[Test]
		public void some_currencies_have_an_character_reference_for_angly_bracket_languages()
		{
			Currency qatariRial = Currency.Get(CurrencyIsoCode.QAR);
			CharacterReference reference = qatariRial.Entity;
			Assert.That(reference, Is.Not.Null.And.Property("IsEmpty").True,
				"the Rial does not have an reference, but a 'null' object");

			Currency euro = Currency.Euro;
			reference = euro.Entity;
			Assert.That(reference, Is.Not.Null.And.Property("IsEmpty").False, "the euro, does");
			Assert.That(reference.Character, Is.EqualTo("€"));
			Assert.That(reference.CodePoint, Is.EqualTo(8364));
			Assert.That(reference.EntityName, Is.EqualTo("&euro;"));
			Assert.That(reference.EntityNumber, Is.EqualTo("&#8364;"));
			Assert.That(reference.SimpleName, Is.EqualTo("euro"));
		}

		[Test]
		public void currencies_have_behavior_that_is_formatting_numbers_as_numbers()
		{
			Assert.That(2.535m.ToString(Currency.Eur), Is.EqualTo("2,535"));
			Assert.That(2.535m.ToString(Currency.Usd), Is.EqualTo("2.535"));
		}

		[Test]
		public void currencies_have_behavior_that_is_formatting_numbers_as_currencies()
		{
			Assert.That(2.535m.ToString("C", Currency.Eur), Is.EqualTo("2,54 €"));
			Assert.That(2.535m.ToString("C", Currency.Usd), Is.EqualTo("$2.54"));
		}
	}
}
