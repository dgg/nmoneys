using NMoneys.Support;

namespace NMoneys.Tests;

[TestFixture]
public partial class CurrencyTester
{
	[Test, Explicit]
	public void Configure_Single_OverridesInformationSubset()
	{
		string englishName = "override";
		Currency.Configure(CurrencyIsoCode.XXX, new CurrencyConfiguration{ EnglishName = englishName});
		Currency overriden = Currency.Get(CurrencyIsoCode.XXX);
		Assert.That(overriden.EnglishName, Is.EqualTo(englishName).And.Not.EqualTo(overriden.NativeName));
	}

	[Test, Explicit]
	public void Configure_Multiple_OverridesInformationSubset()
	{
		string englishName = "override";
		Currency.Configure(new []
		{
			(CurrencyIsoCode.XXX, new CurrencyConfiguration{ EnglishName = englishName}),
			(CurrencyIsoCode.XTS, new CurrencyConfiguration{ EnglishName = englishName})
		});
		Currency xxx = Currency.Get(CurrencyIsoCode.XXX);
		Assert.That(xxx.EnglishName, Is.EqualTo(englishName).And.Not.EqualTo(xxx.NativeName));
		Currency xts = Currency.Get(CurrencyIsoCode.XTS);
		Assert.That(xts.EnglishName, Is.EqualTo(englishName).And.Not.EqualTo(xts.NativeName));
	}

	[Test]
	public void Configure_AfterInitialization_Exception()
	{
		Currency xxx = Currency.Get(CurrencyIsoCode.XXX); // initializes the currency

		Assert.That(()=>Currency.Configure(CurrencyIsoCode.XXX, new CurrencyConfiguration()),
			Throws.InstanceOf<InitializedCurrencyException>());
	}
}
