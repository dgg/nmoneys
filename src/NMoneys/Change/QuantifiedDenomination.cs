using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Text;

namespace NMoneys.Change;

/// <summary>
/// Represents a currency denomination that has been used to make change of a monetary amount.
/// </summary>
/// <param name="Denomination">Denomination used to make change.
/// <example>If a change was made using 3 pieces of 10 cents, the <c>Denomination</c> would be 0.1.</example>
/// </param>
/// <param name="Quantity">Number of denomination units used in the operation.
/// <example>If a change was made using 3 pieces of 10 cents, the <c>Quantity</c> would be 3.</example>
/// </param>
public readonly record struct QuantifiedDenomination(Denomination Denomination, uint Quantity = 0u)
{
	private bool PrintMembers(StringBuilder builder)
	{
		PrintMember(builder);
		return true;
	}

	internal StringBuilder PrintMember([NotNull]StringBuilder builder)
	{
		return builder.AppendFormat(
			CultureInfo.InvariantCulture,
			"{0} * {1}",
			Quantity, Denomination.Value
		);
	}

	/// <summary>
	/// Given a collection of possibly repeated denominations, tallies up the different denominations into a collection.
	/// </summary>
	/// <param name="denominations"></param>
	/// <returns></returns>
	[Pure]
	internal static IEnumerable<QuantifiedDenomination> Aggregate(IEnumerable<Denomination> denominations)
	{
		var aggregation = denominations.GroupBy(_ => _)
			.Select(g => new QuantifiedDenomination(g.Key, (uint)g.Count()));
		return aggregation;
	}
}
