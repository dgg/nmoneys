using NMoneys.Extensions;
using NMoneys.Tests.CustomConstraints;
using NUnit.Framework;

namespace NMoneys.Tests
{
	[TestFixture]
	public class ExtensionsTester
	{
		private readonly decimal five = 5m;

		[Test]
		public void ShortcutExtensions_CreateMoneyFromDecimal()
		{
			Assert.That(five.Aud(), Must.Be.MoneyWith(five, Currency.Aud));
			Assert.That(five.Cad(), Must.Be.MoneyWith(five, Currency.Cad));
			Assert.That(five.Chf(), Must.Be.MoneyWith(five, Currency.Chf));
			Assert.That(five.Cny(), Must.Be.MoneyWith(five, Currency.Cny));
			Assert.That(five.Dkk(), Must.Be.MoneyWith(five, Currency.Dkk));
			Assert.That(five.Eur(), Must.Be.MoneyWith(five, Currency.Eur));
			Assert.That(five.Gbp(), Must.Be.MoneyWith(five, Currency.Gbp));
			Assert.That(five.Hkd(), Must.Be.MoneyWith(five, Currency.Hkd));
			Assert.That(five.Huf(), Must.Be.MoneyWith(five, Currency.Huf));
			Assert.That(five.Inr(), Must.Be.MoneyWith(five, Currency.Inr));
			Assert.That(five.Jpy(), Must.Be.MoneyWith(five, Currency.Jpy));
			Assert.That(five.Mxn(), Must.Be.MoneyWith(five, Currency.Mxn));
			Assert.That(five.Myr(), Must.Be.MoneyWith(five, Currency.Myr));
			Assert.That(five.Nok(), Must.Be.MoneyWith(five, Currency.Nok));
			Assert.That(five.Nzd(), Must.Be.MoneyWith(five, Currency.Nzd));
			Assert.That(five.Rub(), Must.Be.MoneyWith(five, Currency.Rub));
			Assert.That(five.Sek(), Must.Be.MoneyWith(five, Currency.Sek));
			Assert.That(five.Sgd(), Must.Be.MoneyWith(five, Currency.Sgd));
			Assert.That(five.Thb(), Must.Be.MoneyWith(five, Currency.Thb));
			Assert.That(five.Usd(), Must.Be.MoneyWith(five, Currency.Usd));
			Assert.That(five.Zar(), Must.Be.MoneyWith(five, Currency.Zar));
			Assert.That(five.Xts(), Must.Be.MoneyWith(five, Currency.Xts));
			Assert.That(five.Xxx(), Must.Be.MoneyWith(five, Currency.Xxx));
		}

		[Test]
		public void AliasExtensions_CreateMoneyFromDecimal()
		{
			Assert.That(five.Euros(), Must.Be.MoneyWith(five, Currency.Eur));
			Assert.That(five.Dollars(), Must.Be.MoneyWith(five, Currency.Usd));
			Assert.That(five.Pounds(), Must.Be.MoneyWith(five, Currency.Gbp));
		}

		[Test]
		public void SlangExtensions_CreateMoneyFromDecimal()
		{
			Assert.That(five.Lerus(), Must.Be.MoneyWith(five, Currency.Eur));
			Assert.That(five.Bucks(), Must.Be.MoneyWith(five, Currency.Usd));
			Assert.That(five.Quid(), Must.Be.MoneyWith(five, Currency.Gbp));
		}

		[Test]
		public void ToMoney_CreateMoneyFromCurrencyCode()
		{
			Assert.That(CurrencyIsoCode.DKK.ToMoney(100), Must.Be.MoneyWith(100, Currency.Dkk));
		}

		[Test]
		public void ToMoney_CreateMoneyFromDecimal_NoCurrency()
		{
			Assert.That(five.ToMoney(), Must.Be.MoneyWith(5, Currency.Xxx));
		}

		[Test]
		public void ToMoney_CreateMoneyFromDecimal_WithCode()
		{
			Assert.That(five.ToMoney(CurrencyIsoCode.AUD), Must.Be.MoneyWith(5, Currency.Aud));
		}

		[Test]
		public void ToMoney_CreateMoneyFromDecimal_WithCurrency()
		{
			Assert.That(five.ToMoney(Currency.Aud), Must.Be.MoneyWith(5, Currency.Aud));
		}
	}
}
