using System;
using System.Linq;
using NMoneys.Extensions;
using NMoneys.Support;

namespace NMoneys.Change
{
	/// <summary>
	/// Extension class that gives access to extensions methods related to money change making.
	/// </summary>
	[CLSCompliant(false)]
	public static partial class ChangeOperations
	{
		/// <summary>
		/// Counts the number of ways of making changes for a particular amount of money given a particular set of denominations.
		/// </summary>
		/// <remarks>Instead of using a brute force, recursive algorithm, it takes a faster and computational cheaper approach
		/// using dynamic programming strategy.
		/// <para>Such strategy allow computing the result in O(n*m) time, given n the size of <paramref name="money"/> and m the length of <paramref name="denominations"/>.</para>
		/// <para>In terms of size, we need to store O(n) memory to store temporary results.</para></remarks>
		/// <param name="money">The monetary quantity to make change of.</param>
		/// <param name="denominations">The monetary denominations for which we make the change.</param>
		/// <returns>The number of ways one can make change for a given amount of money or zero if no change can be made.</returns>
		/// <exception cref="ArgumentOutOfRangeException">If <paramref name="money"/> is not positive.</exception>
		/// <exception cref="ArgumentNullException">If <paramref name="denominations"/> is null.</exception>
		public static uint CountWaysToMakeChange(this Money money, params Denomination[] denominations)
		{
			Positive.Amounts.AssertArgument(nameof(money), money.Amount);
			Guard.AgainstNullArgument(nameof(denominations), denominations);

			long n = money.MinorIntegralAmount;
			int m = denominations.Length;
			Currency operationCurrency = money.GetCurrency();
			long[] integralDenominations = denominations
				.Select(d => IntegralDenomination.CalculateAmount(d, operationCurrency))
				.ToArray();


			// table[i] will be storing the number of solutions for value i.
			// n+1 rows are needed since the table is constructed in bottom up manner using the base case (n = 0)
			uint[] table = new uint[n + 1];

			// Base case (If given value is 0)
			table[0] = 1u;

			// Pick all denominations one by one and update the table[] values after the index 
			// greater than or equal to the value of the picked coin
			for (int i = 0; i < m; i++)
			{
				for (long j = integralDenominations[i]; j <= n; j++)
				{
					table[j] += table[j - integralDenominations[i]];
				}
			}

			return table[n];
		}

		/// <summary>
		/// Counts the number of ways of making changes for a particular amount of money given a particular set of denomination values.
		/// </summary>
		/// <remarks>Instead of using a brute force, recursive algorithm, it takes a faster and computational cheaper approach
		/// using dynamic programming strategy.
		/// <para>Such strategy allow computing the result in O(n*m) time, given n the size of <paramref name="money"/> and m the length of <paramref name="denominations"/>.</para>
		/// <para>In terms of size, we need to store O(n) memory to store temporary results.</para></remarks>
		/// <param name="money">The monetary quantity to make change of.</param>
		/// <param name="denominationValues">The monetary denomination values for which we make the change.</param>
		/// <returns>The number of ways one can make change for a given amount of money or zero if no change can be made.</returns>
		/// <exception cref="ArgumentOutOfRangeException">If <paramref name="money"/> is not positive.</exception>
		/// <exception cref="ArgumentNullException">If <paramref name="denominationValues"/> is null.</exception>
		public static uint CountWaysToMakeChange(this Money money, params decimal[] denominationValues)
		{
			Guard.AgainstNullArgument(nameof(denominationValues), denominationValues);

			return money.CountWaysToMakeChange(denominationValues.Select(v => new Denomination(v)).ToArray());
		}
	}
}