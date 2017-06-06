using System.Collections.Generic;
using System.Linq;

namespace NMoneys.Change
{
	public static class CountWaysOperation
	{
		public static uint ChangeCount(this Money money, params Denomination[] denominations)
		{
			long n = money.MinorIntegralAmount;
			long[] integralDenominations = denominations
				.Select(d => new Money(d.Value, money.CurrencyCode))
				.Select(m => m.MinorIntegralAmount)
				.ToArray();
			
			//Time complexity of this function: O(mn)
			//Space Complexity of this function: O(n)

			// table[i] will be storing the number of solutions
			// for value i. We need n+1 rows as the table is
			// constructed in bottom up manner using the base
			// case (n = 0)
			uint[] table = new uint[n + 1];

			// Base case (If given value is 0)
			table[0] = 1u;

			// Pick all coins one by one and update the table[]
			// values after the index greater than or equal to
			// the value of the picked coin
			for (int i = 0; i < denominations.Length; i++)
			{
				for (long j = integralDenominations[i]; j <= n; j++)
				{
					table[j] += table[j - integralDenominations[i]];
				}
			}

			return table[n];
		}
	}
}