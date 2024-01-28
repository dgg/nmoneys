using NMoneys.Extensions;

namespace NMoneys.Tests;

[TestFixture]
public partial class MoneyTester
{
	#region AsQuantity

	[Test]
	public void AsQuantity_InvariantCurrency_SpaceSeparatedCapitalizedCurrencyCodeAndInvariantAmount()
	{
		string quantity = 13.45m.Xts().AsQuantity();
		Assert.That(quantity, Is.EqualTo("XTS 13.45"));
	}

	[Test]
	public void AsQuantity_NonInvariantCurrency_SpaceSeparatedCapitalizedCurrencyCodeAndInvariantAmount()
	{
		string quantity = (-13.45m).Eur().AsQuantity();
		Assert.That(quantity, Is.EqualTo("EUR -13.45"));
	}

	[Test, TestCaseSource(nameof(quantities))]
	public void AsQuantity_QuantityWrittenAsPerSpec(string expected, CurrencyIsoCode currency, decimal amount)
	{
		var money = new Money(amount, currency);
		string actual = money.AsQuantity();

		Assert.That(actual, Is.EqualTo(expected));
	}

	#endregion
}
