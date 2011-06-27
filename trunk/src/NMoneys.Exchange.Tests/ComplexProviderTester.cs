using System.Text;
using NUnit.Framework;

namespace NMoneys.Exchange.Tests
{
	[TestFixture]
	public class ComplexProviderTester
	{
		[TestFixtureSetUp]
		public void setupNegatedProvider()
		{
			IExchangeRateProvider provider = new UsdEurGbpAs20110519();
			ExchangeRateProvider.Provider = () => provider;
		}

		[TestFixtureTearDown]
		public void resetProvider()
		{
			ExchangeRateProvider.Provider = ExchangeRateProvider.Default;
		}

		[Test]
		public void METHOD_BEHAVIOR_EXPECTATION()
		{
			
		}
	}
}
