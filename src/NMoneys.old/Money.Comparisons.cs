using System;
using System.Diagnostics.Contracts;

namespace NMoneys
{
	public partial struct Money: IComparable, IComparable<Money>
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
		/// <exception cref="T:System.ArgumentException"><paramref name="obj"/> is not a <see cref="Money"/>.</exception>
		[Pure]
		public int CompareTo(object obj)
		{
			if (obj == null)
			{
				return 1;
			}
			if (!(obj is Money))
			{
				throw new ArgumentException(nameof(obj), $"Argument must be of type {typeof(Money).Name}.");
			}
			return CompareTo((Money)obj);
		}

		/// <summary>
		/// Compares the current <see cref="Amount"/> with the one for another <see cref="Money"/>.
		/// </summary>
		/// <remarks>Both instances must have the same <see cref="CurrencyCode"/> in order to be compared, otherwise an exception will be thrown.</remarks>
		/// <param name="other">An <see cref="Money"/> to compare with this object.</param>
		/// <returns>
		/// A 32-bit signed integer that indicates the relative order of the <c>amounts</c> being compared. The return value has the following meanings: 
		/// <list type="table">
		/// <listheader>
		/// <term>Value</term>
		/// <description>Meaning</description>
		/// </listheader>
		/// <item>
		/// <term>Less than zero</term>
		/// <description>This <see cref="Amount"/> is less than <paramref name="other"/>'s.</description>
		/// </item>
		/// <item>
		/// <term>Zero</term>
		/// <description>This <see cref="Amount"/> is equal to <paramref name="other"/>'s.</description>
		/// </item>
		/// <item>
		/// <term>Greater than zero</term>
		/// <description>This <see cref="Amount"/> is greater than <paramref name="other"/>'s.</description>
		/// </item>
		/// </list>
		/// </returns>
		/// <exception cref="DifferentCurrencyException">If <paramref name="other"/> does not have the same <see cref="CurrencyCode"/>.</exception>
		[Pure]
		public int CompareTo(Money other)
		{
			AssertSameCurrency(other);

			return Amount.CompareTo(other.Amount);
		}

		/// <summary>
		/// Returns a value indicating whether a specified <see cref="Money"/> is greater than another specified <see cref="Money"/>.
		/// </summary>
		/// <param name="left">The first value to compare.</param>
		/// <param name="right">The second value to compare.</param>
		/// <returns>true if <paramref name="left"/> is greater than <paramref name="right"/>; otherwise, false.</returns>
		[Pure]
		public static bool operator >(Money left, Money right)
		{
			return left.CompareTo(right) > 0;
		}

		/// <summary>
		/// Returns a value indicating whether a specified <see cref="Money"/> is less than another specified <see cref="Money"/>.
		/// </summary>
		/// <param name="left">The first value to compare.</param>
		/// <param name="right">The second value to compare.</param>
		/// <returns>true if <paramref name="left"/> is less than <paramref name="right"/>; otherwise, false.</returns>
		[Pure]
		public static bool operator <(Money left, Money right)
		{
			return left.CompareTo(right) < 0;
		}

		/// <summary>
		/// Returns a value indicating whether a specified <see cref="Money"/> is greater than or equal to another specified <see cref="Money"/>.
		/// </summary>
		/// <param name="left">The first value to compare.</param>
		/// <param name="right">The second value to compare.</param>
		/// <returns>true if <paramref name="left"/> is greater than or equal to <paramref name="right"/>; otherwise, false.</returns>
		[Pure]
		public static bool operator >=(Money left, Money right)
		{
			return left.CompareTo(right) >= 0;
		}

		/// <summary>
		/// Returns a value indicating whether a specified <see cref="Money"/> is less than or equal to another specified <see cref="Money"/>.
		/// </summary>
		/// <param name="left">The first value to compare.</param>
		/// <param name="right">The second value to compare.</param>
		/// <returns>true if <paramref name="left"/> is less than or equal to <paramref name="right"/>; otherwise, false.</returns>
		[Pure]
		public static bool operator <=(Money left, Money right)
		{
			return left.CompareTo(right) <= 0;
		}

		/// <summary>
		/// Returns a value indicating whether the <see cref="Amount"/> is strictly less than <see cref="decimal.Zero"/>.
		/// </summary>
		/// <returns>true if <see cref="Amount"/> is less than <see cref="decimal.Zero"/>; otherwise, false.</returns>
		[Pure]
		public bool IsNegative()
		{
			return Amount < decimal.Zero;
		}

		/// <summary>
		/// Returns a value indicating whether the <see cref="Amount"/> is stricly greater than <see cref="decimal.Zero"/>.
		/// </summary>
		/// <returns>true if <see cref="Amount"/> is greater than <see cref="decimal.Zero"/>; otherwise, false.</returns>
		[Pure]
		public bool IsPositive()
		{
			return Amount > decimal.Zero;
		}

		/// <summary>
		/// Returns a value indicating whether the <see cref="Amount"/> is stricly equal to <see cref="decimal.Zero"/>.
		/// </summary>
		/// <returns>true if <see cref="Amount"/> is equal to <see cref="decimal.Zero"/>; otherwise, false.</returns>
		[Pure]
		public bool IsZero()
		{
			return Amount.Equals(decimal.Zero);
		}

		/// <summary>
		/// Returns a value indicating whether the <see cref="Amount"/> is less than or equal to <see cref="decimal.Zero"/>.
		/// </summary>
		/// <returns>true if <see cref="Amount"/> is less or equal than <see cref="decimal.Zero"/>; otherwise, false.</returns>
		/// <seealso cref="IsNegative()"/>
		/// <seealso cref="IsZero()"/>
		[Pure]
		public bool IsNegativeOrZero()
		{
			return IsNegative() || IsZero();
		}

		/// <summary>
		/// Returns a value indicating whether the <see cref="Amount"/> is greater than or equal to <see cref="decimal.Zero"/>.
		/// </summary>
		/// <returns>true if <see cref="Amount"/> is greater or equal than <see cref="decimal.Zero"/>; otherwise, false.</returns>
		/// <seealso cref="IsPositive()"/>
		/// <seealso cref="IsZero()"/>
		[Pure]
		public bool IsPositiveOrZero()
		{
			return IsPositive() || IsZero();
		}
	}
}
