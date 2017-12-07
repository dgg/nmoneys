using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using NMoneys.Support;

namespace NMoneys
{
	public partial struct Money: IEquatable<Money>
	{
		/// <summary>
		/// Indicates whether the current <see cref="Money"/> is equal to another <see cref="Money"/>.
		/// </summary>
		/// <returns>
		/// true if the current instance has equal <see cref="Amount"/> and <see cref="Currency"/>as the <paramref name="other"/> parameter;
		/// otherwise, false.
		/// </returns>
		/// <param name="other">An money to compare with this instance.</param>
		[Pure]
		public bool Equals(Money other)
		{
			return Enumeration.Comparer<CurrencyIsoCode>().Equals(other.CurrencyCode, CurrencyCode) && 
				other.Amount == Amount;
		}

		/// <summary>
		/// Indicates whether this instance and a specified object are equal.
		/// </summary>
		/// <returns>
		/// true if <paramref name="obj"/> and this instance are the same type and represent the same value; otherwise, false.
		/// </returns>
		/// <param name="obj">Another object to compare to.</param>
		[Pure]
		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (obj.GetType() != typeof(Money)) return false;
			return Equals((Money)obj);
		}

		/// <summary>
		/// Returns the hash code for this instance.
		/// </summary>
		/// <returns>
		/// A 32-bit signed integer that is the hash code for this instance.
		/// </returns>
		[Pure]
		public override int GetHashCode()
		{
			unchecked
			{
				return (CurrencyCode.GetHashCode() * 397) ^ Amount.GetHashCode();
			}
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
}
