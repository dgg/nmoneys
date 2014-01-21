using System;
using System.Globalization;
using NMoneys.Extensions;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace NMoneys.Demo.CodeProject
{
	[TestFixture]
	public class MoneyDemo
	{
		[Test]
		public void a_Money_represents_a_monetary_quantity()
		{
			new Money(10m, Currency.Dollar);		// Money --> tenDollars
			new Money(2.5m, CurrencyIsoCode.EUR);	// Money --> twoFiftyEuros
			new Money(10m, "JPY");					// Money --> tenYen
			new Money();							// Money --> zeroWithNoCurrency
		}

		[Test, SetCulture("da-DK")]
		public void environment_dependencies_are_explicit()
		{
			Money fiveKrona = Money.ForCurrentCulture(5m);
			Assert.That(fiveKrona.CurrencyCode, Is.EqualTo(CurrencyIsoCode.DKK));

			Money currencyLessMoney = new Money(1);
			Assert.That(currencyLessMoney.CurrencyCode, Is.EqualTo(CurrencyIsoCode.XXX));

			Money zeroEuros = Money.ForCulture(0m, CultureInfo.GetCultureInfo("es-ES"));
			Assert.That(zeroEuros.CurrencyCode, Is.EqualTo(CurrencyIsoCode.EUR));
		}

		[Test]
		public void moneys_can_be_quickly_created_for_testing_scenarios_with_extension_methods()
		{
			// Money --> threeNoCurrencies
			3m.Xxx();
			3m.ToMoney();

			// Money --> threeAndAHalfAustralianDollars
			3.5m.Aud();
			3.5m.ToMoney(Currency.Aud);
			3.5m.ToMoney(CurrencyIsoCode.AUD);
			CurrencyIsoCode.AUD.ToMoney(3.5m);
		}

		[Test]
		public void what_is_in_a_money()
		{
			Money threeCads = new Money(3m, "CAD");

			Assert.That(threeCads.Amount, Is.EqualTo(3m));
			Assert.That(threeCads.CurrencyCode, Is.EqualTo(CurrencyIsoCode.CAD));
			Assert.That(threeCads.HasDecimals, Is.False);
			Assert.That(threeCads.IsNegative(), Is.False);
			Assert.That(threeCads.IsNegativeOrZero(), Is.False);
			Assert.That(threeCads.IsPositive(), Is.True);
			Assert.That(threeCads.IsPositiveOrZero(), Is.True);
			Assert.That(threeCads.IsZero(), Is.False);
		}

		[Test]
		public void when_you_money_worths_nothing()
		{
			Money.Zero();
			Money.Zero(Currency.Usd);
			Money.Zero(CurrencyIsoCode.USD);
			Money.Zero("USD");
		}

		[Test]
		public void what_is_with_this_Major_thing()
		{
			Assert.That(Money.ForMajor(234, Currency.Gbp), isMoneyWith(234, CurrencyIsoCode.GBP),
				"instance created from the major units, in this case the Pound");

			Assert.That(3m.Pounds().MajorAmount, Is.EqualTo(3m), "for whole amounts is the quantity");
			Assert.That(3.7m.Pounds().MajorAmount, Is.EqualTo(3m), "for fractional amounts is the number of pounds");
			Assert.That(0.7m.Pounds().MajorAmount, Is.EqualTo(0m), "for fractional amounts is the number of pounds");

			Assert.That(3m.Pounds().MajorIntegralAmount, Is.EqualTo(3L), "for whole amounts is the non-fractional quantity");
			Assert.That(3.7m.Pounds().MajorIntegralAmount, Is.EqualTo(3L), "for fractional amounts is the number of pounds");
			Assert.That(0.7m.Pounds().MajorIntegralAmount, Is.EqualTo(0L), "for fractional amounts is the number of pounds");
		}

		[Test]
		public void what_is_with_this_Minor_thing()
		{
			Assert.That(Currency.Pound.SignificantDecimalDigits, Is.EqualTo(2), "pounds have pence, which is a hundreth of the major unit");

			Assert.That(Money.ForMinor(234, Currency.Gbp), isMoneyWith(2.34m, CurrencyIsoCode.GBP),
				"234 pence is 2.34 pounds");
			Assert.That(Money.ForMinor(50, Currency.Gbp), isMoneyWith(0.5m, CurrencyIsoCode.GBP),
				"fifty pence is half a pound");
			Assert.That(Money.ForMinor(-5, Currency.Gbp), isMoneyWith(-0.05m, CurrencyIsoCode.GBP),
				"you owe me five pence, but keep them");

			Assert.That(3m.Pounds().MinorAmount, Is.EqualTo(300m), "three pounds is 300 pence");
			Assert.That(.07m.Pounds().MinorAmount, Is.EqualTo(7m), "for fractional amounts, the minor unit prevails");
			Assert.That(0.072m.Pounds().MinorAmount, Is.EqualTo(7m), "tenths of pence are discarded");

			Assert.That(3m.Pounds().MinorIntegralAmount, Is.EqualTo(300L), "three pounds is 300 pence");
			Assert.That(.07m.Pounds().MinorIntegralAmount, Is.EqualTo(7L), "for fractional amounts, the minor unit prevails");
			Assert.That(0.072m.Pounds().MinorIntegralAmount, Is.EqualTo(7L), "tenths of pence are discarded");
		}

		[Test]
		public void some_currencies_are_divided_into_more_than_100_units()
		{
			Currency dinar = Currency.Get(CurrencyIsoCode.BHD);
			Assert.That(dinar.SignificantDecimalDigits, Is.EqualTo(3), "one dinar is 1000 fils");

			Assert.That(Money.ForMinor(2340, dinar), isMoneyWith(2.34m, CurrencyIsoCode.BHD),
				"2340 fils is 2.34 dinar");
			Assert.That(Money.ForMinor(500, dinar), isMoneyWith(0.5m, CurrencyIsoCode.BHD),
				"fifty fils is half a dinar");
			Assert.That(Money.ForMinor(-5, dinar), isMoneyWith(-0.005m, CurrencyIsoCode.BHD),
				"you owe me five fils, but keep them");

			Assert.That(3m.ToMoney(dinar).MinorIntegralAmount, Is.EqualTo(3000L), "three dinar is 3000 fils");
			Assert.That(.007m.ToMoney(dinar).MinorIntegralAmount, Is.EqualTo(7L), "for fractional amounts, the minor unit prevails");
			Assert.That(0.0072m.ToMoney(dinar).MinorIntegralAmount, Is.EqualTo(7L), "tenths of fil are discarded");
		}

		[Test]
		public void and_some_currencies_do_not_have_minor_units()
		{
			Currency yen = Currency.Get(CurrencyIsoCode.JPY);
			Assert.That(yen.SignificantDecimalDigits, Is.EqualTo(0), "a yen is a yen is a yen");

			Assert.That(Money.ForMinor(2340, yen), isMoneyWith(2340m, CurrencyIsoCode.JPY),
				"2340 yen are, well, 2340 yen");
			Assert.That(Money.ForMinor(-5, yen), isMoneyWith(-5m, CurrencyIsoCode.JPY),
				"you owe me five yen, but keep them");

			Assert.That(3m.ToMoney(yen).MinorIntegralAmount, Is.EqualTo(3L), "three dinar is three yen");
			Assert.That(3.7m.ToMoney(yen).MinorIntegralAmount, Is.EqualTo(3L), "for fractional amounts, decimals are discarded");
			Assert.That(0.72m.ToMoney(yen).MinorIntegralAmount, Is.EqualTo(0L), "less than a unit is not enough");
		}

		[Test]
		public void moneys_can_be_compared()
		{
			Assert.That(3m.Usd().Equals(CurrencyIsoCode.USD.ToMoney(3m)), Is.True);
			Assert.That(3m.Usd() != CurrencyIsoCode.USD.ToMoney(3m), Is.False);
			Assert.That(3m.Usd().CompareTo(CurrencyIsoCode.USD.ToMoney(5m)), Is.LessThan(0));
			Assert.That(3m.Usd() < CurrencyIsoCode.USD.ToMoney(5m), Is.True);
		}

		[Test]
		public void comparisons_only_possible_if_they_have_the_same_currency()
		{
			Assert.That(3m.Usd().Equals(3m.Gbp()), Is.False);
			Assert.That(3m.Usd() != CurrencyIsoCode.GBP.ToMoney(3m), Is.True);

			Assert.That(() => 3m.Usd().CompareTo(CurrencyIsoCode.GBP.ToMoney(5m)), Throws.InstanceOf<DifferentCurrencyException>());
			Assert.That(() => 3m.Usd() < CurrencyIsoCode.GBP.ToMoney(5m), Throws.InstanceOf<DifferentCurrencyException>());
		}

		[Test]
		public void moneys_are_to_be_displayed()
		{
			Assert.That(10.536m.Eur().ToString(), Is.EqualTo("10,54 €"), "default currency formatting according to instance's currency");
			Assert.That(3.2m.Usd().ToString("N"), Is.EqualTo("3.20"), "alternative formatting according to instance's currency");
		}

		[Test]
		public void using_different_styles_for_currencies_taht_span_multiple_countries()
		{
			Assert.That(3000.5m.Eur().ToString(), Is.EqualTo("3.000,50 €"), "default euro formatting");

			// in French the group separator is neither the dot or the space
			CultureInfo french = CultureInfo.GetCultureInfo("fr-FR");
			string threeThousandAndTheHaldInFrench = string.Format("3{0}000,50 €", french.NumberFormat.CurrencyGroupSeparator);
			Assert.That(3000.5m.Eur().ToString(french), Is.EqualTo(threeThousandAndTheHaldInFrench));
		}

		[Test]
		public void more_complex_formatting()
		{
			Assert.That(3m.Usd().Format("{0:00.00} {2}"), Is.EqualTo("03.00 USD"), "formatting placeholders for code and amount");
			Assert.That(2500m.Eur().Format("> {1} {0:#,#.00}"), Is.EqualTo("> € 2.500,00"), "rich amount formatting");
		}

		[Test]
		public void moneys_are_to_be_operated_with_arithmetic_operators()
		{
			Money fivePounds = 2m.Pounds().Plus(3m.Pounds());
			Assert.That(fivePounds, isMoneyWith(5m, CurrencyIsoCode.GBP));

			Money fiftyPence = 3m.Pounds() - 2.5m.Pounds();
			Assert.That(fiftyPence, isMoneyWith(.5m, CurrencyIsoCode.GBP));

			Money youOweMeThreeEuros = -3m.Eur();
			Assert.That(youOweMeThreeEuros, isMoneyWith(-3m, CurrencyIsoCode.EUR));

			Money nowIHaveThoseThreeEuros = youOweMeThreeEuros.Negate();
			Assert.That(nowIHaveThoseThreeEuros, isMoneyWith(3m, CurrencyIsoCode.EUR));

			Money youOweMeThreeEurosAgain = -nowIHaveThoseThreeEuros;
			Assert.That(youOweMeThreeEurosAgain, isMoneyWith(-3m, CurrencyIsoCode.EUR));
		}

		[Test]
		public void basic_arithmetic_operations_can_be_extended()
		{
			Money halfMyDebt = -60m.Eur().Perform(amt => amt / 2);
			Assert.That(halfMyDebt, isMoneyWith(-30m, CurrencyIsoCode.EUR));

			Money convolutedWayToCancelDebt = (-50m).Eur().Perform(-1m.Eur(),
				(amt1, amt2) => decimal.Multiply(amt1, decimal.Negate(amt2)) - amt1);
			Assert.That(convolutedWayToCancelDebt, isMoneyWith(decimal.Zero, CurrencyIsoCode.EUR));
		}

		[Test]
		public void binary_operations_only_possible_if_they_have_the_same_currency()
		{
			Assert.That(() => 2m.Gbp().Minus(3m.Eur()), Throws.InstanceOf<DifferentCurrencyException>());
			Assert.That(() => 2m.Cad() + 3m.Aud(), Throws.InstanceOf<DifferentCurrencyException>());
			Assert.That(() => 3m.Usd().Perform(3m.Aud(), (x, y) => x + y), Throws.InstanceOf<DifferentCurrencyException>());
		}

		[Test]
		public void several_unary_operations_can_be_performed()
		{
			Assert.That(3m.Xxx().Negate(), isMoneyWith(-3m), "-1 * amount");
			Assert.That((-3m).Xxx().Abs(), isMoneyWith(3m), "|amount|");

			Money twoThirds = new Money(2m / 3);
			Assert.That(twoThirds.Amount, Is.Not.EqualTo(0.66m), "not exactly equals as it has more decimals");
			Assert.That(twoThirds.TruncateToSignificantDecimalDigits().Amount, Is.EqualTo(0.66m), "XXX has two significant decimals");

			Money fractional = 123.456m.ToMoney();
			Assert.That(fractional.Truncate(), isMoneyWith(123m), "whole amount");

			Assert.That(.5m.ToMoney().RoundToNearestInt(), isMoneyWith(0m));
			Assert.That(.599999m.ToMoney().RoundToNearestInt(), isMoneyWith(1m));
			Assert.That(1.5m.ToMoney().RoundToNearestInt(), isMoneyWith(2m));
			Assert.That(1.4999999m.ToMoney().RoundToNearestInt(), isMoneyWith(1m));

			Assert.That(.5m.ToMoney().RoundToNearestInt(MidpointRounding.ToEven), isMoneyWith(0m), "closest even number is 0");
			Assert.That(.5m.ToMoney().RoundToNearestInt(MidpointRounding.AwayFromZero), isMoneyWith(1m), "closest number away from zero is 1");
			Assert.That(1.5m.ToMoney().RoundToNearestInt(MidpointRounding.ToEven), isMoneyWith(2m), "closest even number is 2");
			Assert.That(1.5m.ToMoney().RoundToNearestInt(MidpointRounding.AwayFromZero), isMoneyWith(2m), "closest number away from zero is 2");

			Assert.That(2.345m.Usd().Round(), isMoneyWith(2.34m), "round to two decimals");
			Assert.That(2.345m.Jpy().Round(), isMoneyWith(2m), "round to no decimals");
			Assert.That(2.355m.Usd().Round(), isMoneyWith(2.36m), "round to two decimals");
			Assert.That(2.355m.Jpy().Round(), isMoneyWith(2m), "round to no decimals");

			Assert.That(2.345m.Usd().Round(MidpointRounding.ToEven), isMoneyWith(2.34m));
			Assert.That(2.345m.Usd().Round(MidpointRounding.AwayFromZero), isMoneyWith(2.35m));
			Assert.That(2.345m.Jpy().Round(MidpointRounding.ToEven), isMoneyWith(2m));
			Assert.That(2.345m.Jpy().Round(MidpointRounding.AwayFromZero), isMoneyWith(2m));

			Assert.That(123.456m.ToMoney().Floor(), isMoneyWith(123m));
			Assert.That((-123.456m).ToMoney().Floor(), isMoneyWith(-124m));
		}

		[Test]
		public void moneys_can_be_parsed_to_a_known_currency()
		{
			Assert.That(Money.Parse("$1.5", Currency.Dollar), isMoneyWith(1.5m, CurrencyIsoCode.USD), "one-and-the-half dollars");
			Assert.That(Money.Parse("10 €", Currency.Euro), isMoneyWith(10m, CurrencyIsoCode.EUR), "ten euros");

			Assert.That(Money.Parse("kr -100", Currency.Dkk), isMoneyWith(-100m, CurrencyIsoCode.DKK), "owe hundrede kroner");
			Assert.That(Money.Parse("(¤1.2)", Currency.None), isMoneyWith(-1.2m, CurrencyIsoCode.XXX), "owe one point two, no currency");
		}

		[Test]
		public void number_formatting_is_flexible_for_parsing_purposes()
		{
			Assert.That(Money.Parse("(€1.5)", Currency.Euro), isMoneyWith(-15m, CurrencyIsoCode.EUR), "dollar style");
			Assert.That(Money.Parse("-€5,75", Currency.Euro), isMoneyWith(-5.75m, CurrencyIsoCode.EUR), "pound style");
			Assert.That(Money.Parse("-10", Currency.Euro), isMoneyWith(-10m, CurrencyIsoCode.EUR), "negative euro style without no symbol");
			Assert.That(Money.Parse("(1.2)", Currency.Euro), isMoneyWith(-12m, CurrencyIsoCode.EUR), "negative dollar style with no symbol");
		}

		[Test]
		public void currencies_have_to_be_compatible_for_parsing()
		{
			Assert.That(() => Money.Parse("($1.5)", Currency.Eur), Throws.InstanceOf<FormatException>(), "currency not compatible");
			Assert.That(() => Money.Parse("notAMonetaryQuantity", Currency.Eur), Throws.InstanceOf<FormatException>());
			Assert.That(() => Money.Parse("1-e1", Currency.Eur), Throws.InstanceOf<FormatException>(), "wrong format");
			Assert.That(() => Money.Parse("1--", Currency.Eur), Throws.InstanceOf<FormatException>());
		}

		private IResolveConstraint isMoneyWith(decimal amount, CurrencyIsoCode currency)
		{
			return Has.Property("Amount").EqualTo(amount).And.Property("CurrencyCode").EqualTo(currency);
		}

		private IResolveConstraint isMoneyWith(decimal amount)
		{
			return Has.Property("Amount").EqualTo(amount);
		}
	}
}
