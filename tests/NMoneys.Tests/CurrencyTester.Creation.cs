namespace NMoneys.Tests;

[TestFixture]
public partial class CurrencyTester
{
	[Test]
	public void Get_UndefinedCurrency_Exception()
	{
		var notDefined = (CurrencyIsoCode)5000;
		Assert.That(() => Currency.Get(notDefined), Throws
			.InstanceOf<UndefinedCodeException>()
			.With.Message.Contains("5000")
		);
	}
}
