using NMoneys.Extensions;
using NUnit.Framework;

namespace NMoneys.Exchange.Demo.CodeProject
{
	public static class ApiExtensions
	{
		public static UsingImplementor Using(this IExchangeConversion conversion, decimal rate)
		{
			return new UsingImplementor(conversion.From, rate);
		}
	}

	public class UsingImplementor
	{
		private readonly Money _from;
		private readonly decimal _rate;
		public UsingImplementor(Money from, decimal rate)
		{
			_from = from;
			_rate = rate;
		}
		public Money To(CurrencyIsoCode to)
		{
			var rateCalculator = new ExchangeRate(_from.CurrencyCode, to, _rate);
			return rateCalculator.Apply(_from);
		}
	}

	[TestFixture]
	public class RedefineAPI
	{
		[Test]
		public void Creating_New_ConversionOperations()
		{
			var hundredDollars = new Money(100m, CurrencyIsoCode.USD);

			var twoHundredEuros = hundredDollars.Convert().Using(2m).To(CurrencyIsoCode.EUR);

			Assert.That(twoHundredEuros, Is.EqualTo(200m.Eur()));
		}
	}
}
