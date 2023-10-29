using NMoneys;
using NMoneys.Support;


[TestFixture]
public class EnumerationTester
{
	[Test]
	public void Equal_SameValue_True()
	{
		Assert.That(Enumeration.FastComparer.Equals(CurrencyIsoCode.XXX, CurrencyIsoCode.XXX), Is.True);
	}

	[Test]
	public void Equal_DifferentValue_False()
	{
		Assert.That(Enumeration.FastComparer.Equals(CurrencyIsoCode.XXX, CurrencyIsoCode.XTS), Is.False);
	}
}
