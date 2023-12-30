using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Text;

namespace NMoneys.Change;

/// <summary>
/// Represents a possible solution to the problem of finding an optimal way to make change for a particular monetary amount given a set of denominations.
/// <remarks>The solution is optimal in terms of number of denominations used.</remarks>
/// </summary>
public record OptimalChangeSolution : IEnumerable<QuantifiedDenomination>
{
	private QuantifiedDenomination[] Denominations { get; }
	internal OptimalChangeSolution(long toChange, Currency operationCurrency, ushort[] table, IntegralDenomination?[] usedDenominations)
	{
		Denominations = Array.Empty<QuantifiedDenomination>();

		ushort possibleSolution = table.Last();
		if (possibleSolution != ushort.MaxValue)
		{
			// at most as many denominations as used
			List<Denomination> denominations = new List<Denomination>(usedDenominations.Length);
			IntegralDenomination defaultDenomination = IntegralDenomination.Default(operationCurrency);

			long denomination = toChange;
			while (denomination > 0)
			{
				IntegralDenomination usedDenomination = usedDenominations[denomination]
					.GetValueOrDefault(defaultDenomination);
				denominations.Add(usedDenomination.Denomination);
				denomination -= usedDenomination.IntegralAmount;
			}
			Denominations = QuantifiedDenomination.Aggregate(denominations.OrderByDescending(d => d.Value))
				.ToArray();
		}
	}

	/// <summary>
	/// Indicates whether a non-partial change could be made.
	/// </summary>
	/// <returns>true if there was a solution to the make optimal change problem; false otherwise.</returns>
	[Pure]
	public bool IsSolution => Count > 0;

	#region collection

	/// <inheritdoc />
	[Pure]
	public IEnumerator<QuantifiedDenomination> GetEnumerator()
	{
		return Denominations.Cast<QuantifiedDenomination>().GetEnumerator();
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
	public QuantifiedDenomination this[int index] => Denominations[index];

	/// <summary>
	/// Gets the number of different denominations used in the change.
	/// </summary>
	/// <remarks>This is the number of different denominations in the change, not the total number of denominations in the solution, <see cref="TotalCount"/>.</remarks>
	/// <example>If a change solution contains 3 coins of 25 cents and 4 coins of 1 cent, then the <c>Count</c> would be two, since only two different denominations are used.</example>
	[CLSCompliant(false), Pure]
	public uint Count => (uint)Denominations.Length;

	/// <summary>
	/// Gets the total number of denominations used in the change.
	/// </summary>
	/// <remarks>This is the total number of different denominations in the change, not the number of different denominations in the solution, <see cref="Count"/>.</remarks>
	/// <example>If a change solution contains 3 coins of 25 cents and 4 coins of 1 cent, then the <c>TotalCount</c> would be seven, since seven coins were used to make change.</example>
	[CLSCompliant(false), Pure]
	public uint TotalCount => (uint)Denominations.Sum(d => d.Quantity);

	#endregion

	/// <summary>
	/// Builds record members representation.
	/// </summary>
	/// <param name="builder">Instance to build the representation into.</param>
	/// <returns><c>true</c></returns>
	protected virtual bool PrintMembers([NotNull] StringBuilder builder)
	{
		builder.Append(nameof(IsSolution)).Append(" = ").Append(IsSolution).Append(' ');

		builder.Append(nameof(Denominations))
			.Append(" = ");
		if (Count == 0)
		{
			builder.Append("[]");
		}
		else
		{
			Denominations.Aggregate(
					builder.Append("[ "),
					(b, d) =>
					{
						d.PrintMember(b).Append( " | ");
						return b;
					})
				.Remove(builder.Length - 3 , 3)
				.Append(" ]");
		}

		return true;
	}
}
