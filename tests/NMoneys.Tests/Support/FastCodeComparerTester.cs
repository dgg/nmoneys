using NMoneys;
using NMoneys.Support;


[TestFixture]
public class FastCodeComparerTester
{
	[Test]
	public void Equal_SameValue_True()
	{
		Assert.That(FastCodeComparer.Instance.Equals(CurrencyIsoCode.XXX, CurrencyIsoCode.XXX), Is.True);
	}

	[Test]
	public void Equal_DifferentValue_False()
	{
		Assert.That(FastCodeComparer.Instance.Equals(CurrencyIsoCode.XXX, CurrencyIsoCode.XTS), Is.False);
	}
}
