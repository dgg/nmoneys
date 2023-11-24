using System.Diagnostics.Contracts;
using NMoneys.Support;

namespace NMoneys;

/// <summary>
/// A monetary quantity in a given currency
/// </summary>
public partial struct Money
{
	#region .ctor

	/// <summary>
	/// Creates an instance of <see cref="Money"/> with the <paramref name="amount"/> provided
	/// and the specified <paramref name="currency"/> (<see cref="CurrencyIsoCode.XXX"/> if not specified).
	/// </summary>
	/// <param name="amount">The <see cref="Amount"/> of the monetary quantity.</param>
	/// <param name="currency">The <see cref="CurrencyCode"/> of the monetary quantity.</param>
	public Money(decimal amount, CurrencyIsoCode currency = CurrencyIsoCode.XXX)
		: this()
	{
		setAllFields(amount, currency);
	}

	/// <summary>
	/// Creates an instance of <see cref="Money"/> with the <paramref name="amount"/> provided
	/// and the specified <paramref name="currency"/>.
	/// </summary>
	/// <param name="amount">The <see cref="Amount"/> of the monetary quantity.</param>
	/// <param name="currency">The incarnation of the <see cref="CurrencyCode"/>.</param>
	/// <exception cref="ArgumentNullException">If <paramref name="currency"/> is null.</exception>
	public Money(decimal amount, Currency currency) : this(amount,
		Ensuring.NotNull(nameof(currency), currency, c => c.IsoCode))
	{
	}

	/// <summary>
	/// Creates an instance of <see cref="Money"/> with the <paramref name="amount"/> provided
	/// and <see cref="Currency"/> the specified <paramref name="threeLetterIsoCode"/>.
	/// </summary>
	/// <param name="amount">The <see cref="Amount"/> of the monetary quantity.</param>
	/// <param name="threeLetterIsoCode">Textual representation of the ISO 4217 <see cref="CurrencyCode"/>.</param>
	public Money(decimal amount, string threeLetterIsoCode) : this(amount,
		Currency.Code.ParseArgument(
			Ensuring.NotNull(nameof(threeLetterIsoCode), threeLetterIsoCode, (t) => t),
			nameof(threeLetterIsoCode)))
	{
	}

	/// <summary>
	/// Creates an instance of <see cref="Money"/> based on the information provided by <paramref name="money"/>.
	/// </summary>
	/// <param name="money">A <see cref="Money"/> instance from which capture the values from.</param>
	public Money(Money money)
		: this()
	{
		setAllFields(money.Amount, money.CurrencyCode);
	}

	private void setAllFields(decimal amount, CurrencyIsoCode currency)
	{
		currency.AssertDefined();
		Currency.RaiseIfObsolete(currency);

		Amount = amount;
		CurrencyCode = currency;
	}

	#endregion

	/// <summary>
	/// DO NOT USE the field directly. Use <see cref="CurrencyCode"/>.
	/// </summary>
	[Obsolete] private CurrencyIsoCode? _currencyCode;

#pragma warning disable 612,618
	/// <summary>
	/// The ISO 4217 code of the currency of a monetary quantity.
	/// </summary>
	public CurrencyIsoCode CurrencyCode
	{
		get => _currencyCode.GetValueOrDefault(CurrencyIsoCode.XXX);
		private set => _currencyCode = value;
	}
#pragma warning restore 612,618

	/// <summary>
	/// The amount of a monetary quantity
	/// </summary>
	public decimal Amount { get; private set; }

	/// <summary>
	/// Gets the <see cref="Currency"/> instance for this monetary quantity.
	/// </summary>
	/// <returns>The instance of <see cref="Currency"/> represented by <see cref="Money.CurrencyCode"/>.</returns>
	public Currency GetCurrency()
	{
		return Currency.Get(CurrencyCode);
	}

	/// <summary>
	/// Gets the amount in major units
	/// </summary>
	/// <remarks>This method returns the monetary amount in terms of the major units of the currency, truncating the <see cref="Amount"/> if necessary.
	/// <para>For example, 'EUR 2.35' will return a major amount of 2, since EUR has 2 significant decimal values.
	/// 'BHD -1.345' will return -1.</para></remarks>
	public decimal MajorAmount => Truncate().Amount;

	/// <summary>
	/// Gets the amount in major units as a <see cref="long"/>.
	/// </summary>
	/// <remarks>This property returns the monetary amount in terms of the major units of the currency, truncating the amount if necessary.
	/// <para>For example, 'EUR 2.35' will return a major amount of 2, since EUR has 2 significant decimal values.
	/// 'BHD -1.345' will return -1.</para></remarks>
	public long MajorIntegralAmount => Convert.ToInt64(MajorAmount);

	/// <summary>
	/// Gets the amount in minor units.
	/// </summary>
	/// <remarks>This property return the monetary amount in terms of the minor units of the currency, truncating the amount if necessary.
	/// <para>For example, 'EUR 2.35' will return a minor amount of 235, since EUR has 2 significant decimal values.
	/// 'BHD -1.345' will return -1345.</para></remarks>
	public decimal MinorAmount
	{
		get
		{
			Currency currency = Currency.Get(CurrencyCode);
			return CalculateMinorAmount(Amount, currency);
		}
	}

	internal static decimal CalculateMinorAmount(decimal amount, Currency currency)
	{
		decimal minorAmount =
			decimal.Truncate(decimal.Multiply(amount, SmallPowerOfTen.Positive(currency.SignificantDecimalDigits)));
		return minorAmount;
	}

	/// <summary>
	/// Gets the amount in minor units as a <see cref="long"/>.
	/// </summary>
	/// <remarks>This property return the monetary amount in terms of the minor units of the currency, truncating the amount if necessary.
	/// <para>For example, 'EUR 2.35' will return a minor amount of 235, since EUR has 2 significant decimal values.
	/// 'BHD -1.345' will return -1345.</para></remarks>
	public long MinorIntegralAmount => Convert.ToInt64(MinorAmount);

	/// <summary>
	/// Represents the smallest quantity that could be represented using the currency corresponding to <see cref="CurrencyCode"/>.
	/// <seealso cref="Currency.MinAmount"/>
	/// <seealso cref="Currency.SignificantDecimalDigits"/>
	/// </summary>
	public Money MinValue
	{
		get
		{
			Currency currency = Currency.Get(CurrencyCode);
			return new Money(currency.MinAmount, currency);
		}
	}

	/// <summary>
	/// Specifies whether the <see cref="Amount"/> is not a whole number.
	/// </summary>
	public bool HasDecimals
	{
		get
		{
			decimal truncated = decimal.Truncate(Amount);
			return Amount - truncated != decimal.Zero;
		}
	}

	#region currency checking

	/// <summary>
	/// Returns a value indicating whether the <paramref name="money"/> has the same currency as the instance.
	/// </summary>
	/// <param name="money"><see cref="Money"/> instance to check against.</param>
	/// <returns>true if <see cref="CurrencyCode"/> is equal to <paramref name="money"/>'s; otherwise, false.</returns>
	public bool HasSameCurrencyAs(Money money)
	{
		return money.CurrencyCode.Equals(CurrencyCode);
	}

	/// <summary>
	/// Returns a value indicating whether each of the <paramref name="moneys"/> has the same currency as the instance.
	/// </summary>
	/// <param name="moneys">Collection of <see cref="Money"/> instances to check against.</param>
	/// <returns>true if <see cref="CurrencyCode"/> is equal to each of <paramref name="moneys"/>'s; otherwise, false.</returns>
	/// <exception cref="ArgumentNullException"><paramref name="moneys"/> is null.</exception>
	[Pure]
	public bool HasSameCurrencyAs(IEnumerable<Money> moneys)
	{
		ArgumentNullException.ThrowIfNull(moneys, nameof(moneys));
		var self = this;
		return moneys.All(self.HasSameCurrencyAs);
	}

	/// <summary>
	/// Checks whether the <paramref name="money"/> has the same currency as the instance, throwing an exception if that is not the case.
	/// </summary>
	/// <param name="money"><see cref="Money"/> instance to check against.</param>
	/// <exception cref="DifferentCurrencyException"><paramref name="money"/> has a different currency from the instance's.</exception>
	public void AssertSameCurrency(Money money)
	{
		if (!HasSameCurrencyAs(money))
			throw DifferentCurrencyException.ForCodes(CurrencyCode.ToString(), money.CurrencyCode.ToString());
	}

	/// <summary>
	/// Checks whether each of the <paramref name="moneys"/> has the same currency as the instance, throwing an exception if that is not the case.
	/// </summary>
	/// <param name="moneys">Collection of <see cref="Money"/> instance to check against.</param>
	/// <exception cref="DifferentCurrencyException">At least one of the <paramref name="moneys"/> has a different currency from the instance's.</exception>
	public void AssertSameCurrency(IEnumerable<Money> moneys)
	{
		ArgumentNullException.ThrowIfNull(moneys, nameof(moneys));
		foreach (var money in moneys)
		{
			if (!HasSameCurrencyAs(money))
				throw DifferentCurrencyException.ForCodes(CurrencyCode.ToString(), money.CurrencyCode.ToString());
		}
	}

	#endregion
}
