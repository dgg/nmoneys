using System.Diagnostics.CodeAnalysis;

namespace NMoneys.Tests;

[SuppressMessage("Assertion", "NUnit2009:The same value has been provided as both the actual and the expected argument")]
public partial class CurrencyTester
{
	[Test]
	public void Shortcut_Singletons()
	{
		Assert.That(Currency.Xxx, Is.SameAs(Currency.Xxx));
		Assert.That(Currency.Xxx, Is.SameAs(Currency.Get("XXX")));
	}

	[Test, Explicit]
	public void Shortcut_CanBeConfigured()
	{
		var @override = new CurrencyConfiguration { EnglishName = "triple-X" };
		// we can configure before initialization
		Currency.Configure(CurrencyIsoCode.XXX, @override);
		Assert.That(Currency.Xxx, Is.SameAs(Currency.Xxx));
		Assert.That(Currency.Xxx.EnglishName, Is.EqualTo("triple-X"));
	}

	[Test, Explicit]
	public void Shortcut_InitializesTheCurrency_NoFurtherConfiguration()
	{
		Currency test = Currency.Xts; // triggers currency initialization
		var @override = new CurrencyConfiguration { EnglishName = "triple-X" };
		Assert.That(() => Currency.Configure(CurrencyIsoCode.XTS, @override),
			Throws.InstanceOf<InitializedCurrencyException>());
	}
}
