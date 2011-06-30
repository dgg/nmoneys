using System.Globalization;
using NMoneys.Extensions;
using NUnit.Framework;

namespace NMoneys.Tests
{
	[TestFixture]
	public class Quickstart
	{
		[Test]
		public void Currency_GetInstance_FromStaticAccesor()
		{
			Currency euro = Currency.Eur;
		}

		[Test]
		public void Currency_GetInstance_FromFactoryMethod()
		{
			Currency cad = Currency.Get(CurrencyIsoCode.CAD);
			Currency australianDollar = Currency.Get("AUD");
			Currency euro = Currency.Get(CultureInfo.GetCultureInfo("es-ES"));

			Currency itMightNotBe;
			string isoSymbol = null;
			CurrencyIsoCode isoCode = default(CurrencyIsoCode);
			CultureInfo culture = null;
			bool wasFound = Currency.TryGet(isoSymbol, out itMightNotBe);
			wasFound = Currency.TryGet(isoCode, out itMightNotBe);
			wasFound = Currency.TryGet(culture, out itMightNotBe);
		}

		[Test]
		public void Money_CreateInstance()
		{
			var threeDollars = new Money(3m, Currency.Usd);
			var twoandAHalfPounds = new Money(2.5m, CurrencyIsoCode.GBP);
			var tenEuro = new Money(10m, "EUR");
			var thousandWithMissingCurrency = new Money(1000m);
		}

		[Test]
		public void Money_Comparisons()
		{
			int isPositive = new Money(3m, Currency.Nok).CompareTo(new Money(2m, CurrencyIsoCode.NOK));
			bool isFalse = new Money(2m) > new Money(3m);
			bool areEqual = new Money(1m, Currency.Xxx).Equals(new Money(1m, Currency.Xxx));
			bool areNotEqual = new Money(2m) != new Money(5m);
		}

		[Test]
		public void Money_ArithmeticOperations()
		{
			var three = new Money(3m);
			var two = new Money(2m);
			Money five = three.Plus(two);
			Money oneOwed = two - three;
			Money one = oneOwed.Abs();
		}

		[Test]
		public void Money_Formatting()
		{
			var tenEuro = new Money(10m, CurrencyIsoCode.EUR);
			var threeDollars = new Money(3m, CurrencyIsoCode.USD);

			string s = tenEuro.ToString(); // default formatting "10,00 €"
			s = threeDollars.ToString("N"); // format applied to currency "3.00"
			s = threeDollars.ToString(CultureInfo.GetCultureInfo("es-ES")); // format provider used "3,00 €", better suited for countries with same currency and different number formatting
			s = threeDollars.Format("{1} {0:00.00}"); // formatting with placeholders for currency symbol and amount "$ 03.00"
			s = new Money(1500, Currency.Eur).Format("{1} {0:#,#.00}"); // rich formatting "€ 1.500,00"
		}

		[Test]
		public void Money_CreateInstance_UsingExtensions()
		{
			3m.Gbp();
			1000m.Pounds();
			CurrencyIsoCode.NOK.ToMoney(3m);
		}

		[Test]
		public void CurrencyCode_GetInstance()
		{
			CurrencyIsoCode usd = Currency.Code.Cast(840);
			CurrencyIsoCode eur = Currency.Code.Parse("eur");

			CurrencyIsoCode? maybeAud;
			Currency.Code.TryCast(36, out maybeAud);
			Currency.Code.TryParse("036", out maybeAud);
		}

		[Test]
		public void CurrencyCode_ObtainInformation()
		{
			short thirtySix = CurrencyIsoCode.AUD.NumericCode();
			string USD = CurrencyIsoCode.USD.AlphabeticCode();
			string zeroThreeSix = CurrencyIsoCode.AUD.PaddedNumericCode();
		}
	}
}
