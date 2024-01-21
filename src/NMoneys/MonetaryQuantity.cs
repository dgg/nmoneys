using System.Diagnostics.CodeAnalysis;

namespace NMoneys;

/// <summary>
/// Represents the data of a monetary quantity.
/// </summary>
/// <remarks>Useful when serialization platform cannot be configured or it is very complex.
/// <para>In that scenario, one can use this class in the Data-Transfer Objects.</para>
/// </remarks>
public class MonetaryQuantity
{
	/// <summary>
	/// Initializes an instance of <see cref="MonetaryQuantity"/> with an amount of <c>0</c> and no currency (<see cref="CurrencyIsoCode.XXX"/>).
	/// </summary>
	public MonetaryQuantity()
	{
		Currency = CurrencyIsoCode.XXX;
	}

	/// <summary>
	/// Initializes an instance of <see cref="MonetaryQuantity"/> with the provided amount and currency code (<see cref="CurrencyIsoCode"/>).
	/// </summary>
	/// <param name="amount">The amount of a monetary quantity data.</param>
	/// <param name="currency">The ISO 4217 code of the currency of a monetary quantity data.</param>
	public MonetaryQuantity(decimal amount, CurrencyIsoCode currency)
	{
		Amount = amount;
		Currency = currency;
	}

	/// <summary>
	/// The amount of a monetary quantity data.
	/// </summary>
	public decimal Amount { get; set; }
	/// <summary>
	/// The ISO 4217 code of the currency of a monetary quantity data.
	/// </summary>
	public CurrencyIsoCode Currency { get; set; }

	/// <summary>
	/// Defines an explicit conversion of a monetary quantity data (<see cref="MonetaryQuantity"/>) to a monetary quantity (<see cref="Money"/>).
	/// </summary>
	/// <param name="quantity">The monetary quantity data to convert.</param>
	/// <returns>The converted monetary quantity.</returns>
	public static explicit operator Money([NotNull]MonetaryQuantity quantity) => new(quantity.Amount, quantity.Currency);
	/// <summary>
	/// Defines a conversion of a monetary quantity data (<see cref="MonetaryQuantity"/>) to a monetary quantity (<see cref="Money"/>).
	/// </summary>
	/// <param name="quantity">The monetary quantity data to convert.</param>
	/// <returns>The converted monetary quantity.</returns>
	public static Money ToMoney(MonetaryQuantity quantity)
	{
		return (Money)quantity;
	}
	/// <summary>
	/// Defines a conversion of the current monetary quantity data (<see cref="MonetaryQuantity"/>) to a monetary quantity (<see cref="Money"/>).
	/// </summary>
	/// <returns>The converted monetary quantity.</returns>
	public Money ToMoney()
	{
		return (Money)this;
	}

	/// <summary>
	/// Defines an explicit conversion of a monetary quantity (<see cref="Money"/>) to a monetary quantity data (<see cref="MonetaryQuantity"/>).
	/// </summary>
	/// <param name="money">The monetary quantity to convert.</param>
	/// <returns>The converted monetary quantity data.</returns>
	public static explicit operator MonetaryQuantity(Money money) => new(money.Amount, money.CurrencyCode);
	/// <summary>
	/// Defines a conversion of a monetary quantity (<see cref="Money"/>) to a monetary quantity data (<see cref="MonetaryQuantity"/>).
	/// </summary>
	/// <param name="money">The monetary quantity to convert.</param>
	/// <returns>The converted monetary quantity data.</returns>
	public static MonetaryQuantity FromMoney(Money money)
	{
		return (MonetaryQuantity)money;
	}

	/// <inheritdoc />
	/// <remarks>A string with the format '<c>{three_letter_currency_code} {invariant_numeric_amount}</c>' is used due to ease of parsing and compactness.</remarks>
	public override string ToString()
	{
		return Money.AsQuantity(Amount, Currency);
	}
}
