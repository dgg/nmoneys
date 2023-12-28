using System.Diagnostics.Contracts;
using System.Globalization;
using System.Text;

namespace NMoneys.Change;

/// <summary>
/// Represents a proper description of a currency amount, usually coins or bank notes.
/// </summary>
/// <remarks>Only positive valued denominations make actual sense for the operations they are meant for,
/// so its <see cref="Value"/> is restricted to that range.
/// <para>Denominations are currency-less, taking the currency dimension from the operation they are used for.</para>
/// <para>The value of a denomination is to be expressed in major units (<seealso cref="Money.MajorAmount"/>).</para>
/// </remarks>
/// <example>To represent 50 cents of a dollar, one would create a denomination of value .5
/// <code>var fiftyCents = new Denomination(.5m);</code></example>
public readonly record struct Denomination : IComparable<Denomination>, IComparable
{
	/// <summary>
	/// Creates an instance of <see cref="Denomination"/> with the given value.
	/// </summary>
	/// <remarks></remarks>
	/// <param name="value">Positive amount of the denomination in major units.</param>
	/// <example>To represent 50 cents of a dollar, one would create a denomination of value .5
	/// <code>var fiftyCents = new Denomination(.5m);</code></example>
	/// <exception cref="ArgumentOutOfRangeException">If <paramref name="value"/> is not a positive amount.</exception>
	public Denomination(decimal value)
	{
		Positive.Amounts.AssertArgument(nameof(value), value);
		_value = value;
	}

	// ReSharper disable once InconsistentNaming
	private decimal? _value { get; }

	/// <summary>
	/// Value of the denomination.
	/// </summary>
	[Pure]
	public decimal Value => _value.GetValueOrDefault(1);

	[Pure]
	internal IntegralDenomination ToIntegral(Currency operationCurrency)
	{
		return new IntegralDenomination(this, operationCurrency);
	}


	private bool PrintMembers(StringBuilder builder)
	{
		builder.Append(Value.ToString(CultureInfo.InvariantCulture));
		return true;
	}

	#region comparable

	/// <inheritdoc />
	public int CompareTo(Denomination other)
	{
		return Value.CompareTo(other.Value);
	}

	/// <inheritdoc />
	public int CompareTo(object? obj)
	{
		if (ReferenceEquals(null, obj)) return 1;
		return obj is Denomination other
			? CompareTo(other)
			: throw new ArgumentException($"Object must be of type {nameof(Denomination)}");
	}

	/// <summary>Returns a value indicating whether a specified <see cref="Denomination" /> is less than another specified <see cref="Denomination" />.</summary>
	/// <param name="left">The first value to compare.</param>
	/// <param name="right">The second value to compare.</param>
	/// <returns>
	/// <see langword="true" /> if <paramref name="left" /> is less than <paramref name="right" />; otherwise, <see langword="false" />.</returns>
	public static bool operator <(Denomination left, Denomination right)
	{
		return left.CompareTo(right) < 0;
	}

	/// <summary>Returns a value indicating whether a specified <see cref="Denomination" /> is greater than another specified <see cref="Denomination" />.</summary>
	/// <param name="left">The first value to compare.</param>
	/// <param name="right">The second value to compare.</param>
	/// <returns>
	/// <see langword="true" /> if <paramref name="left" /> is greater than <paramref name="right" />; otherwise, <see langword="false" />.</returns>
	public static bool operator >(Denomination left, Denomination right)
	{
		return left.CompareTo(right) > 0;
	}

	/// <summary>Returns a value indicating whether a specified <see cref="Denomination" /> is less than or equal to another specified <see cref="Denomination" />.</summary>
	/// <param name="left">The first value to compare.</param>
	/// <param name="right">The second value to compare.</param>
	/// <returns>
	/// <see langword="true" /> if <paramref name="left" /> is less than or equal to <paramref name="right" />; otherwise, <see langword="false" />.</returns>
	public static bool operator <=(Denomination left, Denomination right)
	{
		return left.CompareTo(right) <= 0;
	}

	/// <summary>Returns a value indicating whether a specified <see cref="Denomination" /> is greater than or equal to another specified <see cref="Denomination" />.</summary>
	/// <param name="left">The first value to compare.</param>
	/// <param name="right">The second value to compare.</param>
	/// <returns>
	/// <see langword="true" /> if <paramref name="left" /> is greater than or equal to <paramref name="right" />; otherwise, <see langword="false" />.</returns>
	public static bool operator >=(Denomination left, Denomination right)
	{
		return left.CompareTo(right) >= 0;
	}

	#endregion
}
