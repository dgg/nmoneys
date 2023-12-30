namespace NMoneys.Tests;

public partial class MoneyTester
{
	private static readonly Money fiver = new Money(5, CurrencyIsoCode.GBP),
		tenner = new Money(10, CurrencyIsoCode.GBP),
		benjamin = new Money(100, "USD");

	[Test]
	public void Equality_SameCurrencyAndAmount_True()
	{
		var anotherFiver = new Money(5, Currency.Gbp);

		Assert.That(fiver.Equals(fiver), Is.True);
		Assert.That(fiver.Equals(anotherFiver), Is.True);
		Assert.That(anotherFiver.Equals(fiver), Is.True);
		Assert.That(fiver == anotherFiver, Is.True);
		Assert.That(anotherFiver == fiver, Is.True);
	}

	[Test]
	public void Equality_DifferentAmountOrCurrency_False()
	{
		Assert.That(fiver.Equals(tenner), Is.False);
		Assert.That(tenner.Equals(fiver), Is.False);
		Assert.That(fiver == tenner, Is.False);
		Assert.That(tenner == fiver, Is.False);

		Assert.That(fiver.Equals(benjamin), Is.False);
		Assert.That(benjamin.Equals(fiver), Is.False);
		Assert.That(fiver == benjamin, Is.False);
		Assert.That(benjamin == fiver, Is.False);
	}

	[Test]
	public void Equality_DifferentTypes()
	{
		// ReSharper disable SuspiciousTypeConversion.Global
		Assert.That(fiver.Equals("asd"), Is.False);
		Assert.That("asd".Equals(fiver), Is.False);
		Assert.That(fiver.Equals(5), Is.False);
		Assert.That(5.Equals(fiver), Is.False);
		// ReSharper restore SuspiciousTypeConversion.Global
	}

	[Test]
	public void Inequality_SameCurrencyAndAmount_False()
	{
		var anotherFiver = new Money(5, Currency.Gbp);

		Assert.That(fiver != anotherFiver, Is.False);
		Assert.That(anotherFiver != fiver, Is.False);
	}

	[Test]
	public void Inequality_DifferentAmountOrCurrency_True()
	{
		Assert.That(fiver != tenner, Is.True);
		Assert.That(tenner != fiver, Is.True);

		Assert.That(fiver != benjamin, Is.True);
		Assert.That(benjamin != fiver, Is.True);
	}
}
