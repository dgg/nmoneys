using System;
using System.Globalization;
using NMoneys.Support;

namespace NMoneys.Allocations
{
	/// <summary>
	/// Represents an allocation ratio, that is, a fraction.
	/// </summary>
	public struct Ratio : IComparable<Ratio>, IEquatable<Ratio>, IFormattable
	{
		private static readonly Range<decimal> _range = new Range<decimal>(0m.Close(), 1m.Close());

		/// <summary>
		/// Initializes a new instance of <see cref="Ratio"/> to the value of the specified <see cref="decimal"/>.
		/// </summary>
		/// <remarks>The <paramref name="value"/> must represent a value between 0 and 1.</remarks>
		/// <param name="value">The value to represent as a <see cref="Ratio"/>.</param>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="value"/> does not fall in the range [0..1]</exception>
		public Ratio(decimal value): this()
		{
			_range.AssertArgument("value", value);
			Value = value;
		}

		/// <summary>
		/// Fraction
		/// </summary>
		public decimal Value { get; }

		/// <summary>
		/// Converts the numeric value of this instance to its equivalent string representation. 
		/// </summary>
		/// <returns>
		/// A string that represents the value of this instance.
		/// </returns>
		public override string ToString()
		{
			return Value.ToString(CultureInfo.CurrentCulture);
		}

		/// <summary>
		/// Formats the value of the current instance using the specified format.
		/// </summary>
		/// <returns>A <see cref="string"/> containing the value of the current instance in the specified format.</returns>
		/// <param name="format">The <see cref="string"/> specifying the format to use.                   
		/// -or- 
		/// null to use the default format defined for the type of the <see cref="IFormattable"/> implementation.
		/// </param>
		/// <param name="formatProvider">The <see cref="IFormatProvider"/> to use to format the value.
		/// -or- 
		/// null to obtain the numeric format information from the current locale setting of the operating system. 
		/// </param>
		public string ToString(string format, IFormatProvider formatProvider)
		{
			return Value.ToString(format, formatProvider);
		}


		/// <summary>
		/// Indicates whether the current object is equal to another object of the same type.
		/// </summary>
		/// <returns>
		/// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
		/// </returns>
		/// <param name="other">A <see cref="Ratio"/> to compare with this object.</param>
		public bool Equals(Ratio other)
		{
			return other.Value == Value;
		}

		/// <summary>
		/// Indicates whether this instance and a specified object are equal.
		/// </summary>
		/// <returns>
		/// true if <paramref name="obj"/> and this instance are the same type and represent the same value; otherwise, false.
		/// </returns>
		/// <param name="obj">Another object to compare to.</param>
		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (obj.GetType() != typeof(Ratio)) return false;
			return Equals((Ratio)obj);
		}

		/// <summary>
		/// Returns the hash code for this instance.
		/// </summary>
		/// <returns>
		/// A 32-bit signed integer that is the hash code for this instance.
		/// </returns>
		public override int GetHashCode()
		{
			return Value.GetHashCode();
		}

		/// <summary>
		/// Compares the current object with another object of the same type.
		/// </summary>
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
		/// <param name="other">An object to compare with this object.</param>
		public int CompareTo(Ratio other)
		{
			return Value.CompareTo(other.Value);
		}

		/// <summary>
		/// Applies (multiplies) the ratio to the specified <paramref name="amount"/>.
		/// </summary>
		/// <param name="amount">A decimal amount.</param>
		/// <returns>The result of multiplying <paramref name="amount"/> by <see cref="Value"/>.</returns>
		public decimal ApplyTo(decimal amount)
		{
			return Value * amount;
		}
	}
}