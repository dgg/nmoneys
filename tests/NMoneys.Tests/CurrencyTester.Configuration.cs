using System.Globalization;

namespace NMoneys.Tests;

public partial class CurrencyTester
{
	[Test, Explicit]
	public void Configure_Single_OverridesInformationSubset()
	{
		string englishName = "override";
		Currency.Configure(CurrencyIsoCode.XXX, new CurrencyConfiguration { EnglishName = englishName });
		Currency overriden = Currency.Get(CurrencyIsoCode.XXX);
		Assert.That(overriden.EnglishName, Is.EqualTo(englishName).And.Not.EqualTo(overriden.NativeName));
	}

	[Test, Explicit]
	public void Configure_SingleCulture_OverridesInformationSubset()
	{
		CultureInfo paraguaySpanish = CultureInfo.GetCultureInfo("es-PY");

		Currency.Configure(CurrencyIsoCode.PYG, CurrencyConfiguration.From(paraguaySpanish));
		Currency overriden = Currency.Get(CurrencyIsoCode.PYG);
		Assert.That(overriden.Symbol, Is.EqualTo(paraguaySpanish.NumberFormat.CurrencySymbol).And
			.Not.EqualTo(@"₲"));
	}

	[Test, Explicit]
	public void Configure_CultureAndRegion_OverridesInformationSubset()
	{
		CultureInfo somali = CultureInfo.GetCultureInfo("so-SO");
		RegionInfo djibuti = new("so-DJ");

		Currency.Configure(CurrencyIsoCode.SOS, CurrencyConfiguration.From(somali, djibuti));
		Currency overriden = Currency.Get(CurrencyIsoCode.SOS);
		// according to .NET, there are no decimals in shillings, but according to ISO there actually are
		// (although inflation turns them useless and are not in the denominations)
		Assert.That(overriden.SignificantDecimalDigits, Is.EqualTo(0).And
			.Not.EqualTo(2));
		// even though somali is spoken in Djibuti, they do not have "shillings", but "francs"
		Assert.That(overriden.EnglishName, Does.Contain("Franc").And
			.Not.Contains("Shilling"));
	}

	[Test, Explicit]
	public void Configure_Multiple_OverridesInformationSubset()
	{
		string englishName = "override";
		Currency.Configure([
			(CurrencyIsoCode.XXX, new CurrencyConfiguration { EnglishName = englishName }),
			(CurrencyIsoCode.XTS, new CurrencyConfiguration { EnglishName = englishName })
		]);
		Currency xxx = Currency.Get(CurrencyIsoCode.XXX);
		Assert.That(xxx.EnglishName, Is.EqualTo(englishName).And.Not.EqualTo(xxx.NativeName));
		Currency xts = Currency.Get(CurrencyIsoCode.XTS);
		Assert.That(xts.EnglishName, Is.EqualTo(englishName).And.Not.EqualTo(xts.NativeName));
	}

	[Test]
	public void Configure_AfterInitialization_Exception()
	{
		Currency xxx = Currency.Get(CurrencyIsoCode.XXX); // initializes the currency

		Assert.That(() => Currency.Configure(CurrencyIsoCode.XXX, new CurrencyConfiguration()),
			Throws.InstanceOf<InitializedCurrencyException>());
	}
}
