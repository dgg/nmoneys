using System.Globalization;
using NMoneys.Tests.Support;

namespace NMoneys.Tests;

public partial class CurrencyTester
{
	#region Get

	[Test]
	public void Get_UndefinedCurrency_Exception()
	{
		var notDefined = (CurrencyIsoCode)5000;
		Assert.That(() => Currency.Get(notDefined), Throws
			.InstanceOf<UndefinedCodeException>()
			.With.Message.Contains("5000")
		);
	}

	[Test]
	public void Get_UndefinedIsoSymbol_Exception()
	{
		string notDefined = "5000";
		Assert.That(() => Currency.Get(notDefined), Throws
			.InstanceOf<UndefinedCodeException>()
			.With.Message.Contains("5000"));
	}

	[Test]
	public void Get_DefinedIsoSymbol_CaseInsensitive()
	{
		Assert.That(Currency.Get(CurrencyIsoCode.XTS), Is
			.SameAs(Currency.Get("XTS"))
			.And.SameAs(Currency.Get("xts"))
			.And.SameAs(Currency.Get("Xts"))
			.And.SameAs(Currency.Get("xTS"))
			.And.SameAs(Currency.Get("xTs")));
	}

	[Test]
	public void Get_DefinedCurrencyForCulture_Currency()
	{
		CultureInfo spanish = CultureInfo.GetCultureInfo("es-ES");
		Assert.That(Currency.Get(spanish), Is.SameAs(Currency.Get("EUR")));
	}

	[Test]
	public void Get_RegionWithUpdatedInformation_Currency()
	{
		var denmark = new RegionInfo("DK");
		Assert.That(Currency.Get(denmark), Is.SameAs(Currency.Get("DKK")));
	}

	[Test]
	public void Get_NotAShortcut_Currency()
	{
		Currency notAShortcut = Currency.Get(CurrencyIsoCode.BZD);
		Assert.That(notAShortcut, Is.SameAs(Currency.Get("BZD")));

		Assert.That(notAShortcut.IsoCode, Is.EqualTo(CurrencyIsoCode.BZD));
		Assert.That(notAShortcut.IsoSymbol, Is.EqualTo("BZD"));
		Assert.That(notAShortcut.Symbol, Is.EqualTo("$"));
		Assert.That(notAShortcut.EnglishName, Is.EqualTo("Belize Dollar"));
	}

	[Test]
	public void Get_Singleton()
	{
		var fromSymbol = Currency.Get("DKK");
		var fromCode = Currency.Get(CurrencyIsoCode.DKK);
		var fromCulture = Currency.Get(CultureInfo.GetCultureInfo("da-DK"));
		var fromRegion = Currency.Get(new RegionInfo("DK"));

		Assert.That(fromSymbol, Is.SameAs(fromCode)
			.And.SameAs(fromCulture)
			.And.SameAs(fromRegion)
		);
	}

	#region Obsolescence

	[Test]
	public void Get_ObsoleteIsoCode_EventRaised()
	{
		CurrencyIsoCode obsolete = Currency.Code.Parse("EEK");
		Action getObsolete = () => Currency.Get(obsolete);
		Assert.That(getObsolete, Doez.Raise.ObsoleteEvent());
	}

	[Test]
	public void Get_ObsoleteCurrencyCode_EventRaised()
	{
		Action getObsolete = () => Currency.Get("EEK");
		Assert.That(getObsolete, Doez.Raise.ObsoleteEvent());
	}

	#endregion

	#endregion

	#region TryGet

	[Test]
	public void TryGet_UndefinedIsoCode_False()
	{
		var notDefined = (CurrencyIsoCode) 5000;

		Assert.That(Currency.TryGet(notDefined, out Currency? tried), Is.False);
		Assert.That(tried, Is.Null);
	}

	[Test]
	public void TryGet_UndefinedIsoSymbol_False()
	{
		string notDefined = "5000";
		Assert.That(Currency.TryGet(notDefined, out var tried), Is.False);
		Assert.That(tried, Is.Null);
	}

	[Test]
	public void TryGet_NullSymbol_False()
	{
		Assert.That(Currency.TryGet((string) null!, out var tried), Is.False);
		Assert.That(tried, Is.Null);
	}

	[Test]
	public void TryGet_DefinedCurrencyForCulture_True()
	{
		CultureInfo spanish = CultureInfo.GetCultureInfo("es-ES");
		Assert.That(Currency.TryGet(spanish, out Currency? tried), Is.True);
		Assert.That(tried, Is.SameAs(Currency.Get("EUR")));
	}

	[Test]
	public void TryGet_NoRegionForCulture_False()
	{
		CultureInfo neutralSpanish = CultureInfo.GetCultureInfo("es");
		Assert.That(Currency.TryGet(neutralSpanish, out Currency? tried), Is.False);
		Assert.That(tried, Is.Null);
	}

	[Test]
	public void TryGet_RegionWithUpdatedInformation_True()
	{
		var denmark = new RegionInfo("DK");

		Assert.That(Currency.TryGet(denmark, out Currency? dkk), Is.True);
		Assert.That(dkk, Is.SameAs(Currency.Get("DKK")));
	}

	[Test]
	public void TryGet_Singleton()
	{
		Currency.TryGet("DKK", out Currency? fromSymbol);
		Currency.TryGet(CurrencyIsoCode.DKK, out Currency? fromCode);
		Currency.TryGet(CultureInfo.GetCultureInfo("da-DK"), out Currency? fromCulture);
		Currency.TryGet(new RegionInfo("DK"), out Currency? fromRegion);

		Assert.That(fromSymbol, Is.SameAs(fromCode)
			.And.SameAs(fromCulture)
			.And.SameAs(fromRegion)
		);
	}

	#region Obsolescence

	[Test]
	public void TryGet_ObsoleteIsoCode_EventRaised()
	{
		CurrencyIsoCode obsolete = Currency.Code.Parse("EEK");
		Action tryGetObsolete = () => Currency.TryGet(obsolete, out var currency);
		Assert.That(tryGetObsolete, Doez.Raise.ObsoleteEvent());
	}

	[Test]
	public void TryGet_ObsoleteCurrencyCode_EventRaised()
	{
		Action tryGetObsolete = () => Currency.TryGet("EEK", out var currency);
		Assert.That(tryGetObsolete, Doez.Raise.ObsoleteEvent());
	}

	#endregion

	#endregion

	[Test]
	public void Initialize_AllCurrenciesGetInitialized()
	{
		Assert.DoesNotThrow(Currency.InitializeAllCurrencies);
	}

	#region FindAll

	[Test]
	public void FindAll_GetsAllCurrencies()
	{
		Currency[] allCurrencies = Currency.FindAll().ToArray();
		CurrencyIsoCode[] allCodes = Enum.GetValues<CurrencyIsoCode>();

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
		Assert.That(Currency.FindAll(), Has.Some.Matches(Has.Property(nameof(Currency.IsObsolete)).True));
	}

	#endregion

	#region change SEK

	[Test]
	public void SEK_GroupSeparator_IsDotAgain()
	{
		Assert.That(Currency.Sek.GroupSeparator, Is.EqualTo(" ").And
			.Not.EqualTo("."));
	}

	#endregion
}
