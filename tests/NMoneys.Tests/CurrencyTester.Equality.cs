namespace NMoneys.Tests;

public partial class CurrencyTester
{
	[Test]
	public void Equals_Currency_SameCode_True()
	{
		Assert.That(Currency.Xxx.Equals(Currency.Get("XXX")), Is.True);
	}

	[Test]
	public void Equals_Currency_DifferentCode_False()
	{
		Assert.That(Currency.Xxx.Equals(Currency.Get("XTS")), Is.False);
	}

	[Test]
	public void Equals_Object_NotCurrency_False()
	{
		object o = new { Prop = "Val" };
		Assert.That(Currency.Xxx.Equals(o), Is.False);
	}

	[Test]
	public void Equals_Object_SameCode_True()
	{
		object o = (object)Currency.Get("XXX");
		Assert.That(Currency.Xxx.Equals(o), Is.True);
	}

	[Test]
	public void Equals_Object_DifferentCode_False()
	{
		object o = (object)Currency.Get("XTS");
		Assert.That(Currency.Xxx.Equals(o), Is.False);
	}

	[Test]
	public void GetHashCode_SameAsCode()
	{
		Assert.That(Currency.Xts.GetHashCode(), Is.EqualTo(CurrencyIsoCode.XTS.NumericCode()));
	}
}
