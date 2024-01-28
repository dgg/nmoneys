using NMoneys.Extensions;

namespace NMoneys.Tests;

[TestFixture]
public class MonetaryRecordTester
{
	#region ctors

	[Test]
	public void DefaultCtor_ZeroAmountAndNoCurrency()
	{
		var subject = new MonetaryRecord();

		Assert.That(subject.Amount, Is.EqualTo(decimal.Zero));
		Assert.That(subject.Currency, Is.EqualTo(CurrencyIsoCode.XXX));
	}

	[Test]
	public void CompleteCtor_SetsAmountAndCurrency()
	{
		decimal amount = 42.375m;
		var subject = new MonetaryRecord(amount, CurrencyIsoCode.XTS);

		Assert.That(subject.Amount, Is.EqualTo(amount));
		Assert.That(subject.Currency, Is.EqualTo(CurrencyIsoCode.XTS));
	}

	#endregion

	[Test]
	public void ToString_CompactMonetaryRepresentation()
	{
		var subject = new MonetaryRecord(-42.375m, CurrencyIsoCode.XTS);

		Assert.That(subject.ToString(), Is.EqualTo("XTS -42.375"));
	}

	#region conversions

	#region from money

	[Test]
	public void FromMoneyOperator_SetsAmountAndCurrency()
	{
		var subject = (MonetaryRecord)52.75m.Xts();

		Assert.That(subject.Amount, Is.EqualTo(52.75m));
		Assert.That(subject.Currency, Is.EqualTo(CurrencyIsoCode.XTS));
	}

	[Test]
	public void FromMoney_SetsAmountAndCurrency()
	{
		var subject = MonetaryRecord.FromMoney(52.75m.Xts());

		Assert.That(subject.Amount, Is.EqualTo(52.75m));
		Assert.That(subject.Currency, Is.EqualTo(CurrencyIsoCode.XTS));
	}

	#endregion

	#region to money

	[Test]
	public void ToMoneyOperator_SetsAmountAndCurrency()
	{
		var money = (Money)new MonetaryRecord(52.75m, CurrencyIsoCode.XTS);

		Assert.That(money, Is.EqualTo(52.75m.Xts()));
	}

	[Test]
	public void StaticToMoney_SetsAmountAndCurrency()
	{
		var subject = new MonetaryRecord(52.75m, CurrencyIsoCode.XTS);

		Assert.That(MonetaryRecord.ToMoney(subject), Is.EqualTo(52.75m.Xts()));
	}

	[Test]
	public void InstanceToMoney_SetsAmountAndCurrency()
	{
		var subject = new MonetaryRecord(52.75m, CurrencyIsoCode.XTS);

		Assert.That(subject.ToMoney(), Is.EqualTo(52.75m.Xts()));
	}

	#endregion

	#endregion
}
