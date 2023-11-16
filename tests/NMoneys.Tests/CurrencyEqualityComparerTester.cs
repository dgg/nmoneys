namespace NMoneys.Tests;

[TestFixture]
public class CurrencyEqualityComparerTester
{
	[Test]
	public void Equals_BothNulls_True()
	{
		Assert.That(CurrencyEqualityComparer.Default.Equals(null, null), Is.True);
	}

	[Test]
	public void Equals_OneNullCurrency_False()
	{
		Assert.That(CurrencyEqualityComparer.Default.Equals(Currency.Xxx, null), Is.False);
		Assert.That(CurrencyEqualityComparer.Default.Equals(null, Currency.Xxx), Is.False);
	}

	[Test]
	public void Equals_SameCurrency_True()
	{
		Assert.That(CurrencyEqualityComparer.Default.Equals(Currency.Get(CurrencyIsoCode.XXX), Currency.Xxx), Is.True);
	}
}
