namespace NMoneys.Tests;

using Iz = Support.Iz;

public partial class MoneyTester
{
	[Test]
	public void Some_ExistingIsoCode_CollectionOfMoneys()
	{
		Assert.That(Money.Some(5m, CurrencyIsoCode.USD, 3), Has.Length.EqualTo(3).And
			.All.Matches(Iz.MoneyWith(5m, Currency.Usd)));
	}

	[Test]
	public void Some_ExistingIsoSymbol_CollectionOfMoneys()
	{
		Assert.That(Money.Some(10m, "EUR", 2), Has.Length.EqualTo(2).And
			.All.Matches(Iz.MoneyWith(10m, Currency.Euro)));
	}

	[Test]
	public void Some_NullSymbol_Exception()
	{
		Assert.That(() => Money.Some(13, (string)null!, 2), Throws.InstanceOf<ArgumentNullException>());
	}

	[Test]
	public void Some_Currency_CollectionOfMoneys()
	{
		Assert.That(Money.Zero(Currency.Gbp), Iz.MoneyWith(decimal.Zero, Currency.Gbp));
	}

	[Test]
	public void Some_NullCurrency_Exception()
	{
		Assert.That(() => Money.Some(13, (Currency)null!, 2), Throws.InstanceOf<ArgumentNullException>());
	}

	[Test]
	public void Some_NonExistingIsoCode_Exception()
	{
		var nonExistingCode = (CurrencyIsoCode)(1);

		Assert.That(() => Money.Some(13, nonExistingCode, 2), Throws.InstanceOf<UndefinedCodeException>().With.Message.Contains("1"));
	}

	[Test]
	public void Some_NonExistingIsoSymbol_Exception()
	{
		string nonExistentIsoSymbol = "XYZ";
		Assert.That(() => Money.Some(13, nonExistentIsoSymbol, 2), Throws.ArgumentException);
	}

	[Test]
	public void Some_InvalidLength_Exception()
	{
		Assert.That(() => Money.Some(13, CurrencyIsoCode.EUR, -1), Throws.InstanceOf<OverflowException>());
	}

	[Test]
	public void Some_ZeroLength_Empty()
	{
		Assert.That(Money.Some(1000, Currency.Hkd, 0), Is.Empty);
	}
}
