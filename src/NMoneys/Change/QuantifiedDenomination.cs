using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Linq;

namespace NMoneys.Change
{
	/// <summary>
	/// Represents a currency denomination that has been used to make change of a monetary amount.
	/// </summary>
	public class QuantifiedDenomination
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

		/// <inheritdoc />
		[Pure]
		public override string ToString()
		{
			return $"{Quantity.ToString(CultureInfo.InvariantCulture)} x {Denomination}";
		}

		// Given a collection of possibly repeated denominations, tallies up the different denominations into a colletion
		[Pure]
		internal static IEnumerable<QuantifiedDenomination> Aggregate(IEnumerable<Denomination> denominations)
		{
			var aggregation = denominations.GroupBy(_ => _)
				.Select(g => new QuantifiedDenomination(g.Key, (uint) g.Count()));
			return aggregation;
		}
	}
}