namespace NMoneys.Tests;

[TestFixture]
public class CurrencyCharacterTester
{
	[Test]
	public void ctor_NamedReference_SetsAllProps()
	{
		var namedReference = new CharacterReference(8364, "euro");
		Assert.That(namedReference.CodePoint, Is.EqualTo(8364));
		Assert.That(namedReference.Name, Is.EqualTo("euro"));
		Assert.That(namedReference.DecimalReference, Is.EqualTo("&#8364;"));
		Assert.That(namedReference.HexadecimalReference, Is.EqualTo("&#x20AC;"));
		Assert.That(namedReference.Character, Is.EqualTo("€"));

	}

	[Test]
	public void ctor_UnnamedReference_SetsAllProps()
	{
		var unnamedReference = new CharacterReference(8377);
		Assert.That(unnamedReference.CodePoint, Is.EqualTo(8377));
		Assert.That(unnamedReference.Name, Is.Null);
		Assert.That(unnamedReference.DecimalReference, Is.EqualTo("&#8377;"));
		Assert.That(unnamedReference.HexadecimalReference, Is.EqualTo("&#x20B9;"));
		Assert.That(unnamedReference.Character, Is.EqualTo("₹"));
	}

	[Test]
	public void Eq()
	{
		var rupee = new CharacterReference(8377);
		var other = new CharacterReference(8377);

		Assert.That(rupee.Equals(other), Is.True);
	}

	[Test]
	public void Hash()
	{
		var rupee = new CharacterReference(8377);
		var other = new CharacterReference(8377);

		Assert.That(rupee.GetHashCode(), Is.EqualTo(8377));
	}
}

