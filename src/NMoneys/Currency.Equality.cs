using System;
using System.Diagnostics.Contracts;
using NMoneys.Support;

namespace NMoneys
{
	public sealed partial class Currency : IEquatable<Currency>
	{
		/// <summary>
		/// Indicates whether the current <see cref="Currency"/> instance is equal to another instance.
		/// </summary>
		/// <remarks>Only <see cref="IsoCode"/> is checked as the object cannot be mutated. For more thorough comparison use <see cref="CurrencyEqualityComparer"/></remarks>
		/// <returns>
		/// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
		/// </returns>
		/// <param name="other">A <see cref="Currency"/> to compare with this object.</param>
		[Pure]
		public bool Equals(Currency other)
		{
			if (ReferenceEquals(null, other)) return false;
			if (ReferenceEquals(this, other)) return true;
			// only IsoCode matters as it cannot be mutated
			return Code.Comparer.Equals(other.IsoCode, IsoCode);
		}

		/// <summary>
		/// Determines whether the specified <see cref="object"/> is equal to the current <see cref="Currency"/>.
		/// </summary>
		/// <returns>
		/// true if the specified <see cref="object"/> is equal to the current <see cref="Currency"/>; otherwise, false.
		/// </returns>
		/// <param name="obj">The <see cref="object"/> to compare with the current <see cref="Currency"/>.</param> 
		[Pure]
		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != typeof(Currency)) return false;
			return Equals((Currency)obj);
		}

		/// <summary>
		/// Serves as a hash function for a particular type. 
		/// </summary>
		/// <returns>
		/// A hash code for the current <see cref="Currency"/>.
		/// </returns>
		[Pure]
		public override int GetHashCode()
		{
			// it is ok to use writable IsoCode as it is assigned once at initialization
			return IsoCode.GetHashCode();
		}

		///<summary>
		/// Determines whether two specified currencies are equal.
		///</summary>
		///<param name="left">The first <see cref="Currency"/> to compare, or null.</param>
		///<param name="right">The second <see cref="Currency"/> to compare, or null.</param>
		///<returns>true if <paramref name="left"/> is equal to <paramref name="right"/>; otherwise, false.</returns>
		[Pure]
		public static bool operator ==(Currency left, Currency right)
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
}
