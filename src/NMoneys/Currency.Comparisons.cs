using System.Diagnostics.Contracts;

namespace NMoneys;

public sealed partial class Currency : IComparable, IComparable<Currency>
{
	/// <inheritdoc />
	[Pure]
	public int CompareTo(object? obj)
	{
		if (obj == null)
		{
			return 1;
		}

		if (obj is not Currency currency)
		{
			throw new ArgumentException($"Argument must be of type {nameof(Currency)}.", nameof(obj));
		}
		return CompareTo(currency);
	}

	/// <inheritdoc />
	[Pure]
	public int CompareTo(Currency? other)
	{
		if (other == null) return 1;
		return StringComparer.Ordinal.Compare(IsoSymbol, other.IsoSymbol);
	}

	/// <summary>
	/// Returns a value indicating whether a specified <see cref=" Currency"/> is less than another specified <see cref="Currency"/>.
	/// </summary>
	/// <param name="left">The first value to compare.</param>
	/// <param name="right">The second value to compare.</param>
	/// <returns>true if <paramref name="left"/> is less than <paramref name="right"/>; otherwise, false.</returns>
	/// <exception cref="ArgumentNullException"><paramref name="left"/> is null.</exception>
	[Pure]
	public static bool operator <(Currency left, Currency right)
	{
		return left?.CompareTo(right) < 0;
	}

	/// <summary>
	/// Returns a value indicating whether a specified <see cref="Currency"/> is greater than or equal to another specified <see cref="Currency"/>.
	/// </summary>
	/// <param name="left">The first value to compare.</param>
	/// <param name="right">The second value to compare.</param>
	/// <returns>true if <paramref name="left"/> is greater than or equal to <paramref name="right"/>; otherwise, false.</returns>
	/// <exception cref="ArgumentNullException"><paramref name="left"/> is null.</exception>
	[Pure]
	public static bool operator >(Currency left, Currency right)
	{
		return left?.CompareTo(right) > 0;
	}

	/// <summary>
	/// Returns a value indicating whether a specified <see cref=" Currency"/> is less than another specified <see cref="Currency"/>.
	/// </summary>
	/// <param name="left">The first value to compare.</param>
	/// <param name="right">The second value to compare.</param>
	/// <returns>true if <paramref name="left"/> is less than <paramref name="right"/>; otherwise, false.</returns>
	/// <exception cref="ArgumentNullException"><paramref name="left"/> is null.</exception>
	[Pure]
	public static bool operator <=(Currency left, Currency right)
	{
		return left?.CompareTo(right) <= 0;
	}

	/// <summary>
	/// Returns a value indicating whether a specified <see cref="Currency"/> is greater than or equal to another specified <see cref="Currency"/>.
	/// </summary>
	/// <param name="left">The first value to compare.</param>
	/// <param name="right">The second value to compare.</param>
	/// <returns>true if <paramref name="left"/> is greater than or equal to <paramref name="right"/>; otherwise, false.</returns>
	/// <exception cref="ArgumentNullException"><paramref name="left"/> is null.</exception>
	[Pure]
	public static bool operator >=(Currency left, Currency right)
	{
		return left?.CompareTo(right) >= 0;
	}
}
