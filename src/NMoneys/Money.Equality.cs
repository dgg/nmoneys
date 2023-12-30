using System.Diagnostics.Contracts;

namespace NMoneys;

public partial struct Money : IEquatable<Money>
{
	/// <inheritdoc />
	[Pure]
	public bool Equals(Money other)
	{
		return Currency.Code.Comparer.Equals(CurrencyCode, other.CurrencyCode) && Amount == other.Amount;
	}

	/// <inheritdoc />
	[Pure]
	public override bool Equals(object? obj)
	{
		return obj is Money other && Equals(other);
	}

	/// <inheritdoc />
	[Pure]
	public override int GetHashCode()
	{
		return HashCode.Combine(CurrencyCode, Amount);
	}

	/// <summary>
	/// Returns a value indicating whether two instances of <see cref="Money"/> are equal.
	/// </summary>
	/// <param name="left">The first value to compare.</param>
	/// <param name="right">The second value to compare.</param>
	/// <returns>true if <paramref name="left"/> and <paramref name="right"/> are equal; otherwise, false.</returns>
	[Pure]
	public static bool operator ==(Money left, Money right)
	{
		return EqualityComparer<Money>.Default.Equals(left, right);
	}

	/// <summary>
	/// Returns a value indicating whether two instances of <see cref="Money"/> are not equal.
	/// </summary>
	/// <param name="left">The first value to compare.</param>
	/// <param name="right">The second value to compare.</param>
	/// <returns>true if <paramref name="left"/> and <paramref name="right"/> are not equal; otherwise, false.</returns>
	[Pure]
	public static bool operator !=(Money left, Money right)
	{
		return !EqualityComparer<Money>.Default.Equals(left, right);
	}
}
