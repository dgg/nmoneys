using System;
using System.Linq;
using NMoneys.Extensions;

namespace NMoneys.Change
{
	[CLSCompliant(false)]
	public static class CountWaysToMakeChangeOperation
	{
		public static uint CountWaysToMakeChange(this Money money, params Denomination[] denominations)
		{
			Positive.Amounts.AssertArgument(nameof(money), money.Amount);

			long n = money.MinorIntegralAmount;
			int m = denominations.Length;
			Currency operationCurrency = money.GetCurrency();
			long[] integralDenominations = denominations
				.Select(d => IntegralDenomination.CalculateAmount(d, operationCurrency))
				.ToArray();

			//Time complexity of this function: O(m * n)
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
			for (int i = 0; i < m; i++)
			{
				for (long j = integralDenominations[i]; j <= n; j++)
				{
					table[j] += table[j - integralDenominations[i]];
				}
			}

			return table[n];
		}

		public static uint CountWaysToMakeChange(this Money money, params decimal[] denominationValues)
		{
			return money.CountWaysToMakeChange(denominationValues.Select(v => new Denomination(v)).ToArray());
		}
	}
}