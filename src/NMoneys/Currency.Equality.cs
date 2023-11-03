using System.Diagnostics.Contracts;

namespace NMoneys;

public sealed partial class Currency : IEquatable<Currency>
{
	/// <inheritdoc />
	[Pure]
	public bool Equals(Currency? other)
	{
		if (other is null) return false;
		if (ReferenceEquals(this, other)) return true;
		// only IsoCode matters as it cannot be mutated
		return Code.Comparer.Equals(IsoCode, other.IsoCode);
	}

	/// <inheritdoc />
	public override bool Equals(object? obj)
	{
		return ReferenceEquals(this, obj) || obj is Currency other && Equals(other);
	}

	/// <inheritdoc />
	[Pure]
	public override int GetHashCode()
	{
		// ReSharper disable once NonReadonlyMemberInGetHashCode
		return IsoCode.GetHashCode();
	}

	///<summary>
	/// Determines whether two specified currencies are equal.
	///</summary>
	///<param name="left">The first <see cref="Currency"/> to compare, or null.</param>
	///<param name="right">The second <see cref="Currency"/> to compare, or null.</param>
	///<returns>true if <paramref name="left"/> is equal to <paramref name="right"/>; otherwise, false.</returns>
	[Pure]
	public static bool operator ==(Currency? left, Currency? right)
	{
		return Equals(left, right);
	}

	///<summary>
	/// Determines whether two specified currencies are not equal.
	///</summary>
	///<param name="left">The first <see cref="Currency"/> to compare, or null.</param>
	///<param name="right">The second <see cref="Currency"/> to compare, or null.</param>
	///<returns>true if <paramref name="left"/> is not equal to <paramref name="right"/>; otherwise, false.</returns>
	[Pure]
	public static bool operator !=(Currency left, Currency right)
	{
		return !Equals(left, right);
	}
}
