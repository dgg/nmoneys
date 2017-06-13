using System;
using System.Diagnostics.Contracts;
using System.Globalization;

namespace NMoneys.Change
{
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
	public struct Denomination : IEquatable<Denomination>, IFormattable
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

		private readonly decimal? _value;
		/// <summary>
		/// Value of the denomination.
		/// </summary>
		[Pure]
		public decimal Value => _value.GetValueOrDefault(1);


		/// <summary>Returns representation of the value of this instance.</summary>
		/// <returns>A <see cref="string" /> containing a textual representation.</returns>
		[Pure]
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
		[Pure]
		public string ToString(string format, IFormatProvider formatProvider)
		{
			return Value.ToString(format, formatProvider);
		}

		[Pure]
		internal IntegralDenomination ToIntegral(Currency operationCurrency)
		{
			return new IntegralDenomination(this, operationCurrency);
		}

		/// <summary>
		/// Indicates whether the current object is equal to another object of the same type.</summary>
		/// <returns>true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.</returns>
		/// <param name="other">A <see cref="Denomination"/> to compare with this object.</param>
		[Pure]
		public bool Equals(Denomination other)
		{
			return _value == other._value;
		}

		/// <summary>
		/// Indicates whether this instance and a specified object are equal.</summary>
		/// <returns>true if <paramref name="obj" /> and this instance are the same type and represent the same value; otherwise, false. </returns>
		/// <param name="obj">Another object to compare to. </param>
		[Pure]
		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			return obj is Denomination && Equals((Denomination) obj);
		}

		/// <inheritdoc />
		[Pure]
		public override int GetHashCode()
		{
			return _value.GetHashCode();
		}
	}
}