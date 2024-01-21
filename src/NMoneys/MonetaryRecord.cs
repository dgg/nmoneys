using System.Diagnostics.CodeAnalysis;

namespace NMoneys;

/// <summary>
/// Represents the data of a monetary quantity.
/// </summary>
/// <remarks>Useful when serialization platform cannot be configured or it is very complex.
/// <para>In that scenario, one can use this record in the Data-Transfer Objects.</para>
/// </remarks>
/// <param name="Amount">The amount of a monetary quantity data.</param>
/// <param name="Currency">The ISO 4217 code of the currency of a monetary quantity data.</param>
public record MonetaryRecord(decimal Amount = Decimal.Zero, CurrencyIsoCode Currency = CurrencyIsoCode.XXX)
{
	/// <summary>
	/// Defines an explicit conversion of a monetary quantity data (<see cref="MonetaryRecord"/>) to a monetary quantity (<see cref="Money"/>).
	/// </summary>
	/// <param name="quantity">The monetary quantity data to convert.</param>
	/// <returns>The converted monetary quantity.</returns>
	public static explicit operator Money([NotNull]MonetaryRecord quantity) => new(quantity.Amount, quantity.Currency);
	/// <summary>
	/// Defines a conversion of a monetary quantity data (<see cref="MonetaryRecord"/>) to a monetary quantity (<see cref="Money"/>).
	/// </summary>
	/// <param name="quantity">The monetary quantity data to convert.</param>
	/// <returns>The converted monetary quantity.</returns>
	public static Money ToMoney(MonetaryRecord quantity)
	{
		return (Money)quantity;
	}
	/// <summary>
	/// Defines a conversion of the current monetary quantity data (<see cref="MonetaryRecord"/>) to a monetary quantity (<see cref="Money"/>).
	/// </summary>
	/// <returns>The converted monetary quantity.</returns>
	public Money ToMoney()
	{
		return (Money)this;
	}

	/// <summary>
	/// Defines an explicit conversion of a monetary quantity (<see cref="Money"/>) to a monetary quantity data (<see cref="MonetaryRecord"/>).
	/// </summary>
	/// <param name="money">The monetary quantity to convert.</param>
	/// <returns>The converted monetary quantity data.</returns>
	public static explicit operator MonetaryRecord(Money money) => new(money.Amount, money.CurrencyCode);
	/// <summary>
	/// Defines a conversion of a monetary quantity (<see cref="Money"/>) to a monetary quantity data (<see cref="MonetaryRecord"/>).
	/// </summary>
	/// <param name="money">The monetary quantity to convert.</param>
	/// <returns>The converted monetary quantity data.</returns>
	public static MonetaryRecord FromMoney(Money money)
	{
		return (MonetaryRecord)money;
	}

	/// <inheritdoc />
	/// <remarks>A string with the format '<c>{three_letter_currency_code} {invariant_numeric_amount}</c>' is used due to ease of parsing and compactness.</remarks>
	public override string ToString()
	{
		return Money.AsQuantity(Amount, Currency);
	}
};
