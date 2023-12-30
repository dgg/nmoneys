using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Text;

namespace NMoneys.Change;

/// <summary>
/// Represents a possible solution to the problem of finding a way to make change for a particular monetary amount given a set of denominations.
/// <remarks>The solution might not be optimal in terms of number of denominations used as it is computed using a greedy algorithm.</remarks>
/// </summary>
public record ChangeSolution : IEnumerable<QuantifiedDenomination>
{
	private QuantifiedDenomination[] Denominations { get; }

	internal ChangeSolution(IEnumerable<Denomination> usedDenominations, Money remainder)
	{
		Remainder = remainder.Amount > decimal.Zero ? remainder : null;
		CurrencyCode = Remainder?.CurrencyCode;
		Denominations = QuantifiedDenomination.Aggregate(usedDenominations).ToArray();
	}

	/// <summary>
	/// The ISO 4217 code of all monetary quantities of the allocation.
	/// </summary>
	[Pure]
	public CurrencyIsoCode? CurrencyCode { get; }

	/// <summary>
	/// Represents the part of the monetary amount that could not be changed given the set of denominations.
	/// </summary>
	/// <remarks>It only contains a value for partial change making. <seealso cref="IsPartial"/>.</remarks>
	/// <returns>null if all the monetary amount was changed; the part of that amount if not all of it could be changed.</returns>
	[Pure]
	public Money? Remainder { get; }

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
	public uint TotalCount => (uint)Denominations.Sum(s => s.Quantity);

	#endregion

	/// <summary>
	/// Builds record members representation.
	/// </summary>
	/// <param name="builder">Instance to build the representation into.</param>
	/// <returns><c>true</c></returns>
	protected virtual bool PrintMembers([NotNull] StringBuilder builder)
	{
		builder.Append(nameof(IsSolution)).Append(" = ").Append(IsSolution).Append(' ');
		if (IsSolution)
		{
			builder.Append(nameof(IsPartial)).Append(" = ").Append(IsPartial).Append(' ');
		}

		if (Remainder.HasValue)
		{
			IFormatProvider currencyProvider = Remainder.Value.GetCurrency();
			builder.Append(nameof(CurrencyCode))
				.Append(" = ")
				.Append(CurrencyCode)
				.Append(' ');
			builder.Append(nameof(Remainder))
				.Append(" = ")
				.Append(Remainder.Value.Amount.ToString(currencyProvider))
				.Append(' ');
		}


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
					(sb, d) => d.PrintMember(sb).Append(" | ")
				)
				.Remove(builder.Length - 3, 3)
				.Append(" ]");
		}

		return true;
	}
}
