using System.Diagnostics.Contracts;

namespace NMoneys.Change;

public static partial class ChangeOperations
{
	/// <summary>
		/// Returns a way to make an optimal change (in terms of number of denominations) for a particular amount of money given a regardless of the set of denominations in
		/// an efficient manner.
		/// </summary>
		/// <remarks>The computation of the change of made using dynamic programming strategies, that is, computing all possible solutions
		/// and selecting an optimal one (in terms of number of denominations used).
		/// <para>This way of computing yields an optimal solution (in terms of number of denominations) even for
		/// arbitrary, non-canonical, denomination sets.</para>
		/// <para>This strategy allows computing the result in O(m*n) time, being n the size of <paramref name="money"/> and m the length of <paramref name="denominations"/>.</para>
		/// <para>An optimal change is only considered for denomination sets that provide a complete (non-partial)
		/// way of making change for the given amount. Partial change solutions are not considered optimal solutions and thus, not calculated.</para>
		/// <para>A practical infinite (or sufficiently large) number of denominations is assumed. That is, we are not going to run out of denominations when making the change.</para>
		/// </remarks>
		/// <param name="money">The monetary quantity to make optimal change of.</param>
		/// <param name="denominations">The monetary denominations for which the optimal change is made.</param>
		/// <returns>A solution with the denominations used for making optimal change.</returns>
		/// <exception cref="ArgumentOutOfRangeException">If <paramref name="money"/> is not positive.</exception>
		/// <exception cref="ArgumentNullException">If <paramref name="denominations"/> is null.</exception>
		[Pure]
		public static OptimalChangeSolution MakeOptimalChange(this Money money, params Denomination[] denominations)
		{
			Positive.Amounts.AssertArgument(nameof(money), money.Amount);
			ArgumentNullException.ThrowIfNull(denominations, nameof(denominations));

			long n = money.MinorIntegralAmount;
			int m = denominations.Length;
			Currency operationCurrency = money.GetCurrency();
			IntegralDenomination[] integralDenominations = denominations
				// denominations are ordered because the algorithm is sensitive to order
				.OrderBy(d => d.Value)
				.Select(d => d.ToIntegral(operationCurrency))
				.ToArray();

			var table = new ushort[n + 1];
			var usedDenominations = new IntegralDenomination?[n + 1];

			// Base case (If given value n is 0)
			table[0] = 0;

			// Compute minimum denominations required for all values from 1 to n
			for (long i = 1; i <= n; i++)
			{
				// Initialize all table values as Infinite
				table[i] = ushort.MaxValue;

				// Go through all denominations smaller than i
				for (int j = 0; j < m && integralDenominations[j].IntegralAmount <= i; j++)
				{
					uint subResult = table[i - integralDenominations[j].IntegralAmount];
					if (subResult != ushort.MaxValue && subResult + 1u < table[i])
					{
						table[i] = (ushort)(subResult + 1);
						usedDenominations[i] = integralDenominations[j];
					}
				}
			}

			var solution = new OptimalChangeSolution(n, operationCurrency, table, usedDenominations);
			return solution;
		}

		/// <summary>
		/// Returns a way to make an optimal change (in terms of number of denominations) for a particular amount of money given a regardless of the set of denominations in
		/// an efficient manner.
		/// </summary>
		/// <remarks>This overload is a facility method for easier syntax. <see cref="MakeOptimalChange(NMoneys.Money,NMoneys.Change.Denomination[])"/>
		/// for more information.</remarks>
		/// <param name="money">The monetary quantity to make optimal change of.</param>
		/// <param name="denominationValues">The monetary denomination values for which the optimal change is made.</param>
		/// <returns>A solution with the denominations used for making optimal change.</returns>
		/// <exception cref="ArgumentOutOfRangeException">If <paramref name="money"/> is not positive.</exception>
		/// <exception cref="ArgumentNullException">If <paramref name="denominationValues"/> is null.</exception>
		[Pure]
		public static OptimalChangeSolution MakeOptimalChange(this Money money, params decimal[] denominationValues)
		{
			ArgumentNullException.ThrowIfNull(denominationValues, nameof(denominationValues));

			return money.MakeOptimalChange(denominationValues.Select(v => new Denomination(v)).ToArray());
		}
}
