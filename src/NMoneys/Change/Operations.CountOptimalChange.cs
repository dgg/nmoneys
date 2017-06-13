using System;
using System.Linq;
using NMoneys.Extensions;

namespace NMoneys.Change
{
	public static partial class ChangeOperations
	{
		/// <summary>
		/// Calculates the minimum amount of denominations needed to make change for a particular amount of money given a set of denomination values.
		/// </summary>
		/// <remarks>It is equivalent of calling <see cref="MakeOptimalChange(NMoneys.Money,NMoneys.Change.Denomination[])"/> and checking <see cref="OptimalChangeSolution.TotalCount"/>
		/// but in a more memory-efficient manner since no denominations need to be tracked.
		/// <para>The computation of the optimal change of made using dynamic programming strategies, that is, computing all possible solutions
		/// and selecting an optimal one (in terms of number of denominations used).</para>
		/// <para>This way of computing yields an optimal solution (in terms of number of denominations) even for
		/// arbitrary, non-canonical, denomination sets.</para>
		/// <para>This strategy allows computing the result in O(m*n) time, being n the size of <paramref name="money"/> and m the length of <paramref name="denominations"/>.</para>
		/// <para>An optimal change is only considered for denomination sets that provide a complete (non-partial) 
		/// way of making change for the given amount. Partial change solutions are not considered optimal solutions and thus, not calculated.</para>
		/// <para>A practical infinite (or sufficiently large) number of denominations is assumed. That is, we are not going to run out of denominations when making the change.</para>
		/// </remarks>
		/// <param name="money">The monetary quantity to make optimal change of.</param>
		/// <param name="denominations">The monetary denominations for which the optimal change is made.</param>
		/// <returns>The optimal (minimum) number of denominations used for making change.</returns>
		[CLSCompliant(false)]
		public static ushort CountOptimalChange(this Money money, params Denomination[] denominations)
		{
			Positive.Amounts.AssertArgument(nameof(money), money.Amount);

			long n = money.MinorIntegralAmount;
			int m = denominations.Length;
			Currency currencyOperation = money.GetCurrency();
			IntegralDenomination[] integralDenominations = denominations
				.Select(d => new IntegralDenomination(d, currencyOperation))
				.ToArray();

			// table[i] will be storing the minimum number of denominations required for i value.
			// So table[n] will have result
			ushort[] table = new ushort[n + 1];

			// Base case (If given value n is 0)
			table[0] = 0;
			
			// Compute minimum denominations required for all values from 1 to n
			for (long i = 1; i <= n; i++)
			{
				// Initialize all table values as Infinite
				table[i] = ushort.MaxValue;
				// Go through all denominations smaller than i
				for (int j = 0; j < m; j++)
				{
					if (integralDenominations[j].IntegralAmount <= i)
					{
						uint subResult = table[i - integralDenominations[j].IntegralAmount];
						if (subResult != ushort.MaxValue && subResult + 1u < table[i])
						{
							table[i] = (ushort) (subResult + 1);
						}
					}
				}
			}
			return table[n] == ushort.MaxValue ? default(ushort) : table[n];
		}

		/// <summary>
		/// Calculates the minimum amount of denominations needed to make change for a particular amount of money given a set of denomination values.
		/// </summary>
		/// <remarks>This overload is a facility method for easier syntax. <see cref="CountOptimalChange(NMoneys.Money,NMoneys.Change.Denomination[])"/>
		/// for more information.</remarks>
		/// <param name="money">The monetary quantity to make optimal change of.</param>
		/// <param name="denominationValues">The monetary denomination values for which the optimal change is made.</param>
		/// <returns>The optimal (minimum) number of denominations used for making change.</returns>
		[CLSCompliant(false)]
		public static ushort CountOptimalChange(this Money money, params decimal[] denominationValues)
		{
			return money.CountOptimalChange(denominationValues.Select(v => new Denomination(v)).ToArray());
		}
	}
}