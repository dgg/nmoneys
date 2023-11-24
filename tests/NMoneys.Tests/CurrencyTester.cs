using System.Reflection;
using NMoneys.Support;

namespace NMoneys.Tests;

[TestFixture]
public partial class CurrencyTester
{
	[Test]
	public void NumericValue_HoldsTheValueOfTheCode()
	{
		Assert.That(Currency.Get("XXX").NumericCode, Is.EqualTo(999));
		Assert.That(Currency.Get("XTS").NumericCode, Is.EqualTo(963));
	}

	[Test]
	public void PaddedNumericValue_ThreeDigitedValue_NoLeadingZeros()
	{
		Assert.That(Currency.Get("XTS").PaddedNumericCode, Is.EqualTo("963"));
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

	#region Entity

	[Test]
	public void Entity_NoEntity_Null()
	{
		var noEntity = Currency.Dkk;
		Assert.That(noEntity.Entity, Is.Null);
	}

	[Test]
	public void Entity_NamedReference_SetsAllProps()
	{
		var namedReference = Currency.Eur.Entity;
		Assert.That(namedReference, Is.Not.Null);

		Assert.That(namedReference?.CodePoint, Is.EqualTo(8364));
		Assert.That(namedReference?.Name, Is.EqualTo("euro"));
		Assert.That(namedReference?.DecimalReference, Is.EqualTo("&#8364;"));
		Assert.That(namedReference?.HexadecimalReference, Is.EqualTo("&#x20AC;"));
		Assert.That(namedReference?.Character, Is.EqualTo("€"));
	}

	[Test]
	public void Entity_UnnamedReference_SetsSomeProps()
	{
		var unnamedReference = Currency.Get(CurrencyIsoCode.IDR).Entity;
		Assert.That(unnamedReference, Is.Not.Null);

		Assert.That(unnamedReference?.CodePoint, Is.EqualTo(8377));
		Assert.That(unnamedReference?.Name, Is.Null);
		Assert.That(unnamedReference?.DecimalReference, Is.EqualTo("&#8377;"));
		Assert.That(unnamedReference?.HexadecimalReference, Is.EqualTo("&#x20B9;"));
		Assert.That(unnamedReference?.Character, Is.EqualTo("₹"));
	}

	[Test]
	public void Entity_CodesDecoratedWithCodePoint_SetsProps()
	{
		var currenciesWithEntity = Enum.GetValues<CurrencyIsoCode>()
			.Select(c => new { Code = c, Attribute = InfoAttribute.GetFrom(c) })
			.Where(a => a.Attribute.CodePoint != null)
			.Select(a => Currency.Get(a.Code));

		Assert.That(currenciesWithEntity, Has.All.Matches(
			Has.Property(nameof(Currency.Entity)).Not.Null
		));
	}

	#endregion

	#region Obsolete

	[Test]
	public void IsObsolete_TrueForSome()
	{
		var obsolete = Currency.Get("EEK");
		Assert.That(obsolete.IsObsolete, Is.True);
	}

	[Test]
	public void IsObsolete_True_CodeIsMarkedAsObsolete()
	{
		var obsolete = Currency.Get("EEK");

		Assert.That(obsolete.IsoCode.HasAttribute<ObsoleteAttribute>(), Is.True);
	}

	[Test]
	public void IsObsolete_ISConsistentWithAttributeDecoration()
	{
		Currency[] obsoleteCurrencies = Currency.FindAll().Where(c => c.IsObsolete).ToArray();

		Assert.That(obsoleteCurrencies,
			Has.All.Matches<Currency>(ObsoleteCurrencies.IsObsolete),
			"all obsolete currencies are in obsolete cache");

		Currency[] obsoleteCurrenciesInCache = Currency.FindAll().Where(ObsoleteCurrencies.IsObsolete).ToArray();
		Assert.That(obsoleteCurrenciesInCache,
			Is.EquivalentTo(obsoleteCurrencies),
			"no more obsolete currencies than the ones in the cache");

		CurrencyIsoCode[] obsoleteCodes = obsoleteCurrencies.Select(c => c.IsoCode).ToArray();
		Assert.That(obsoleteCodes, Has.All.Matches<CurrencyIsoCode>(c => c.HasAttribute<ObsoleteAttribute>()),
			"all currency codes are marked as obsolete");
		Assert.That(obsoleteCodes, Has.All.Matches<CurrencyIsoCode>(c => c.IsObsolete()),
			"all currency codes are obsolete");
	}

	#endregion

	#region Issue 16. Case sensitivity. Currency instances can be obtained by any casing of the IsoCode (Alphbetic code)

	[Test]
	public void Get_ByIsoCode_IsCaseInsensitive()
	{
		Assert.That(Currency.Get("XBA"), Is.SameAs(Currency.Get("xBa")));
	}

	[Test]
	public void TryGet_ByIsoCode_IsCaseInsensitive()
	{
		Assert.That(Currency.TryGet("XBA", out Currency? upper), Is.True);
		Assert.That(Currency.TryGet("xBa", out Currency? mixed), Is.True);

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
