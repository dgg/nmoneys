using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Linq;
using NMoneys.Support;

namespace NMoneys.Change
{
	/// <summary>
	/// Represents a currency denomination that has been used to make change of a monetary amount.
	/// </summary>
	public class QuantifiedDenomination : IFormattable
	{
		internal QuantifiedDenomination(Denomination denomination, uint quantity = 0u)
		{
			Denomination = denomination;
			Quantity = quantity;
		}

		/// <summary>
		/// Denomination used to make change.
		/// </summary>
		/// <example>If a change was made using 3 pieces of 10 cents, the <c>Denomination</c> would be 0.1.</example>
		[Pure]
		public Denomination Denomination { get; }

		/// <summary>
		/// Number of denomination units used in the operation.
		/// </summary>
		/// <example>If a change was made using 3 pieces of 10 cents, the <c>Quantity</c> would be 3.</example>
		[CLSCompliant(false), Pure]
		public uint Quantity { get; }

		private static readonly string _formatPattern = "{0} * {1}";
		/// <summary>
		/// Converts the numeric value of this instance to its equivalent string representation. 
		/// </summary>
		/// <returns>
		/// A string that represents the value of this instance.
		/// </returns>
		[Pure]
		public override string ToString()
		{
			string representation = string.Format(_formatPattern,
				Quantity.ToString(CultureInfo.InvariantCulture),
				Denomination);
			return Stringifier.Default.StringifyIt(representation);
		}

		/// <summary>
		/// Formats the value of the current instance using the specified format.
		/// </summary>
		/// <returns>A <see cref="string"/> containing the value of the current instance based on the specified format.</returns>
		/// <param name="format">The <see cref="string"/> specifying the format to  apply to <see cref="Denomination"/>.
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
			string representation = string.Format(_formatPattern,
				Quantity.ToString(CultureInfo.InvariantCulture),
				Denomination.ToString(format, formatProvider));
			return Stringifier.Default.StringifyIt(representation);
		}

		// Given a collection of possibly repeated denominations, tallies up the different denominations into a colletion
		[Pure]
		internal static IEnumerable<QuantifiedDenomination> Aggregate(IEnumerable<Denomination> denominations)
		{
			var aggregation = denominations.GroupBy(_ => _)
				.Select(g => new QuantifiedDenomination(g.Key, (uint)g.Count()));
			return aggregation;
		}
	}
}