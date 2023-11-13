namespace NMoneys.Tests;

[TestFixture]
public partial class CurrencyTester
{
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
}
