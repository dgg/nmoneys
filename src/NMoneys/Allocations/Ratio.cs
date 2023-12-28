using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text;
using NMoneys.Support;

namespace NMoneys.Allocations;

/// <summary>
/// Represents an allocation ratio, that is, a fraction.
/// </summary>
public readonly record struct Ratio: IComparable<Ratio>, IComparable
{
	private static readonly Range<decimal> _range = new(0m.Close(), 1m.Close());
	/// <summary>
	/// Initializes a new instance of <see cref="Ratio"/> to the value of the specified <see cref="decimal"/>.
	/// </summary>
	/// <remarks>The <paramref name="value"/> must represent a value between 0 and 1.</remarks>
	/// <param name="value">The value to represent as a <see cref="Ratio"/>.</param>
	/// <exception cref="ArgumentOutOfRangeException"><paramref name="value"/> does not fall in the range [0..1]</exception>
	public Ratio(decimal value) : this()
	{
		_range.AssertArgument(nameof(value), value);
		Value = value;
	}

	/// <summary>
	/// Fraction
	/// </summary>
	public decimal Value { get; }

	/// <summary>
	/// Applies (multiplies) the ratio to the specified <paramref name="amount"/>.
	/// </summary>
	/// <param name="amount">A decimal amount.</param>
	/// <returns>The result of multiplying <paramref name="amount"/> by <see cref="Value"/>.</returns>
	public decimal ApplyTo(decimal amount)
	{
		return Value * amount;
	}

	private bool PrintMembers(StringBuilder builder)
	{
		PrintMember(builder);
		return true;
	}

	internal StringBuilder PrintMember([NotNull]StringBuilder builder)
	{
		return builder.Append(Value.ToString(CultureInfo.InvariantCulture));
	}

	#region comparable

	/// <inheritdoc />
	public int CompareTo(Ratio other)
	{
		return Value.CompareTo(other.Value);
	}

	/// <inheritdoc />
	public int CompareTo(object? obj)
	{
		if (ReferenceEquals(null, obj)) return 1;
		return obj is Ratio other ? CompareTo(other) : throw new ArgumentException($"Object must be of type {nameof(Ratio)}");
	}

	/// <summary>Returns a value indicating whether a specified <see cref="Ratio" /> is less than another specified <see cref="Ratio" />.</summary>
	/// <param name="left">The first value to compare.</param>
	/// <param name="right">The second value to compare.</param>
	/// <returns>
	/// <see langword="true" /> if <paramref name="left" /> is less than <paramref name="right" />; otherwise, <see langword="false" />.</returns>
	public static bool operator <(Ratio left, Ratio right)
	{
		return left.CompareTo(right) < 0;
	}

	/// <summary>Returns a value indicating whether a specified <see cref="Ratio" /> is less than or equal to another specified <see cref="Ratio" />.</summary>
	/// <param name="left">The first value to compare.</param>
	/// <param name="right">The second value to compare.</param>
	/// <returns>
	/// <see langword="true" /> if <paramref name="left" /> is less than or equal to <paramref name="right" />; otherwise, <see langword="false" />.</returns>
	public static bool operator <=(Ratio left, Ratio right)
	{
		return left.CompareTo(right) <= 0;
	}

	/// <summary>Returns a value indicating whether a specified <see cref="Ratio" /> is greater than another specified <see cref="Ratio" />.</summary>
	/// <param name="left">The first value to compare.</param>
	/// <param name="right">The second value to compare.</param>
	/// <returns>
	/// <see langword="true" /> if <paramref name="left" /> is greater than <paramref name="right" />; otherwise, <see langword="false" />.</returns>
	public static bool operator >(Ratio left, Ratio right)
	{
		return left.CompareTo(right) > 0;
	}

	/// <summary>Returns a value indicating whether a specified <see cref="Ratio" /> is greater than or equal to another specified <see cref="Ratio" />.</summary>
	/// <param name="left">The first value to compare.</param>
	/// <param name="right">The second value to compare.</param>
	/// <returns>
	/// <see langword="true" /> if <paramref name="left" /> is greater than or equal to <paramref name="right" />; otherwise, <see langword="false" />.</returns>
	public static bool operator >=(Ratio left, Ratio right)
	{
		return left.CompareTo(right) >= 0;
	}

	#endregion
}

