using NMoneys.Tests.Support;

#pragma warning disable CS0618 // Type or member is obsolete
#pragma warning disable CS0219 // Variable is assigned but its value is never used
namespace NMoneys.Tests;

[TestFixture, Category("Demo"), Category("Sample")]
public class CurrencyIsoCodeDemo
{
	[Test]
	public void currency_codes_are_modeled_as_enums_named_after_its_ISO_alphabetic_code()
	{
		CurrencyIsoCode usDollars = CurrencyIsoCode.USD;
		CurrencyIsoCode euro = CurrencyIsoCode.EUR;
		CurrencyIsoCode danishKrona = CurrencyIsoCode.DKK;
		CurrencyIsoCode noCurrency = CurrencyIsoCode.XXX;
	}

	[Test]
	public void currency_codes_have_their_ISO_numeric_value()
	{
		Assert.That((short)CurrencyIsoCode.USD, Is.EqualTo(840));
		Assert.That((short)CurrencyIsoCode.EUR, Is.EqualTo(978));
		Assert.That((short)CurrencyIsoCode.DKK, Is.EqualTo(208));
		Assert.That((short)CurrencyIsoCode.XXX, Is.EqualTo(999));
	}

	[Test]
	public void less_common_currencies_are_also_modeled_as_long_as_they_are_approved_by_ISO()
	{
		CurrencyIsoCode platinum = CurrencyIsoCode.XPT;
		CurrencyIsoCode yemeniRial = CurrencyIsoCode.YER;
	}

	[Test]
	public void recently_deprecated_currencies_are_also_present()
	{
		var estonianKroon = CurrencyIsoCode.EEK;
		Assert.That(estonianKroon.AsAttributeProvider(), Has.Attribute<ObsoleteAttribute>());
	}
}
