using NUnit.Framework;

namespace NMoneys.Exchange.Demo.CodeProject
{
	[TestFixture]
	public class DefaultConversions
	{
		[Test]
		public void Default_Conversions_DoNotBlowUpButAreNotTerriblyUseful()
		{
			var tenEuro = new Money(10m, CurrencyIsoCode.EUR);

			var tenDollars = tenEuro.Convert().To(CurrencyIsoCode.USD);
			Assert.That(tenDollars.Amount, Is.EqualTo(10m));
			
			var tenPounds = tenEuro.Convert().To(Currency.Gbp);
			Assert.That(tenPounds.Amount, Is.EqualTo(10m));
		}
	}
}
