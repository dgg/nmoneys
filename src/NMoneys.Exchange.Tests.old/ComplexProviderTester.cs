using System.Text;
using NUnit.Framework;

namespace NMoneys.Exchange.Tests
{
	[TestFixture]
	public class ComplexProviderTester
	{
		[OneTimeSetUp]
		public void SetupNegatedProvider()
		{
			IExchangeRateProvider provider = new UsdEurGbpAs20110519();
			ExchangeRateProvider.Factory = () => provider;
		}

		[OneTimeTearDown]
		public void ResetProvider()
		{
			ExchangeRateProvider.Factory = ExchangeRateProvider.Default;
		}

		[Test]
		public void METHOD_BEHAVIOR_EXPECTATION()
		{
			
		}
	}
}
