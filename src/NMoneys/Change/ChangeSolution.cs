using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using NMoneys.Support;

namespace NMoneys.Change
{
	/// <summary>
	/// Represents a possible solution to the problem of finding a way to make change for a particular monetary amount given a set of denominations.
	/// <remarks>The solution might not be optimal in terms of number of denominations used as it is computed using a greedy algorithm.</remarks>
	/// </summary>
	public class ChangeSolution : IEnumerable<QuantifiedDenomination>
	{
		private readonly QuantifiedDenomination[] _denominations;
		internal ChangeSolution(IEnumerable<Denomination> usedDenominations, Money remainder)
		{
			Remainder = remainder.Amount > decimal.Zero ? remainder : default(Money?);
			_denominations = QuantifiedDenomination.Aggregate(usedDenominations)
				.ToArray();
		}

		/// <inheritdoc />
		[Pure]
		public IEnumerator<QuantifiedDenomination> GetEnumerator()
		{
			return _denominations.Cast<QuantifiedDenomination>().GetEnumerator();
		}

		/// <inheritdoc />
		[Pure]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		/// <summary>
		/// Gets the denomination at the specified index.
		/// </summary>
		/// <param name="index">The index of the denomination to get.</param>
		/// <returns>The denomination at the specified index.</returns>
		[Pure]
		public QuantifiedDenomination this[int index] => _denominations[index];

		/// <summary>
		/// Gets the number of different denominations used in the change.
		/// </summary>
		/// <remarks>This is the number of different denominations in the change, not the total number of denominations in the solution, <see cref="TotalCount"/>.</remarks>
		/// <example>If a change solution contains 3 coins of 25 cents and 4 coins of 1 cent, then the <c>Count</c> would be two, since only two different denominations are used.</example>
		[CLSCompliant(false), Pure]
		public uint Count => (uint)_denominations.Length;

		/// <summary>
		/// Represents the part of the monetary amount that could not be changed given the set of denominations.
		/// </summary>
		/// <remarks>It only contains a value for partial change making. <seealso cref="IsPartial"/>.</remarks>
		/// <returns>null if all the monetary amount was changed; the part of that amount if not all of it could be changed.</returns>
		[Pure]
		public Money? Remainder { get; }

		/// <summary>
		/// Gets the total number of denominations used in the change.
		/// </summary>
		/// <remarks>This is the total number of different denominations in the change, not the number of different denominations in the solution, <see cref="Count"/>.</remarks>
		/// <example>If a change solution contains 3 coins of 25 cents and 4 coins of 1 cent, then the <c>TotalCount</c> would be seven, since seven coins were used to make change.</example>
		[CLSCompliant(false), Pure]
		public uint TotalCount => (uint)_denominations.Sum(s => s.Quantity);

		/// <summary>
		/// Indicates whether a change could be made (partial or otherwise).
		/// </summary>
		/// <returns>true if there was a solution to the make change problem; false otherwise.</returns>
		[Pure]
		public bool IsSolution => Count > 0;

		/// <summary>
		/// Indicates whether all the monetary amount could be changed given the set of denominations.
		/// </summary>
		/// <returns>true if all the amount was changed; false otherwise.</returns>
		[Pure]
		public bool IsPartial => IsSolution && Remainder.HasValue;

		/// <inheritdoc />

		[Pure]
		public override string ToString()
		{
			return Stringifier.Default.Stringify(_denominations);
		}
	}
}