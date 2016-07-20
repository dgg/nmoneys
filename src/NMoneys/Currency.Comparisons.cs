using System;
using System.Diagnostics.Contracts;
using NMoneys.Support;

namespace NMoneys
{
	public sealed partial class Currency : IComparable, IComparable<Currency>
	{
		/// <summary>
		/// Compares the current instance with another object of the same type and returns an integer that indicates whether the current instance precedes,
		/// follows, or occurs in the same position in the sort order as the other object.
		/// </summary>
		/// <param name="obj">An object to compare with this instance. </param>
		/// <returns>
		/// A 32-bit signed integer that indicates the relative order of the objects being compared. The return value has these meanings:
		/// <list type="table">
		/// <listheader>
		/// <term>Value</term>
		/// <description>Meaning</description>
		/// </listheader>
		/// <item>
		/// <term>Less than zero</term>
		/// <description>This instance is less than <paramref name="obj"/>.</description>
		/// </item>
		/// <item>
		/// <term>Zero</term>
		/// <description>This instance is equal to <paramref name="obj"/>.</description>
		/// </item>
		/// <item>
		/// <term>Greater than zero</term>
		/// <description>This instance is greater than <paramref name="obj"/>.</description>
		/// </item>
		/// </list>
		/// </returns>
		/// <exception cref="T:System.ArgumentException"><paramref name="obj"/> is not a <see cref="Currency"/>.</exception>
		[Pure]
		public int CompareTo(object obj)
		{
			if (obj == null)
			{
				return 1;
			}
			if (!(obj is Currency))
			{
				throw new ArgumentException("obj", $"Argument must be of type {typeof(Currency).Name}.");
			}
			return CompareTo((Currency)obj);
		}

		/// <summary>
		/// Performs a textual comparison of the Iso symbol
		/// </summary>
		/// <param name="other">An object to compare with this instance.</param>
		/// <returns>
		/// A 32-bit signed integer that indicates the relative order of the objects being compared. The return value has these meanings:
		/// <list type="table">
		/// <listheader>
		/// <term>Value</term>
		/// <description>Meaning</description>
		/// </listheader>
		/// <item>
		/// <term>Less than zero</term>
		/// <description>This instance is less than <paramref name="other"/>.</description>
		/// </item>
		/// <item>
		/// <term>Zero</term>
		/// <description>This instance is equal to <paramref name="other"/>.</description>
		/// </item>
		/// <item>
		/// <term>Greater than zero</term>
		/// <description>This instance is greater than <paramref name="other"/>.</description>
		/// </item>
		/// </list>
		/// </returns>
		[Pure]
		public int CompareTo(Currency other)
		{
			if (other == null) return 1;
			return string.Compare(IsoSymbol, other.IsoSymbol, StringComparison.Ordinal);
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
			Guard.AgainstNullArgument(nameof(left), left);
			return left.CompareTo(right) < 0;
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
			Guard.AgainstNullArgument(nameof(left), left);
			return left.CompareTo(right) > 0;
		}
	}
}
