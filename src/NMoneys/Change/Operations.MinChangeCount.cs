using System;
using System.Linq;

namespace NMoneys.Change
{
	[CLSCompliant(false)]
	public static class MinChangeCountOperation
	{
		public static ushort MinChangeCount(this Money money, params Denomination[] denominations)
		{
			long n = money.MinorIntegralAmount;
			int m = denominations.Length;
			long[] integralDenominations = denominations
				.Select(d => new Money(d.Value, money.CurrencyCode))
				.Select(mm => mm.MinorIntegralAmount)
				.ToArray();
				
			// table[i] will be storing the minimum number of denominations
			// required for i value.  So table[n] will have result
			ushort[] table = new ushort[n + 1];

			// Base case (If given value n is 0)
			table[0] = 0;

			// Initialize all table values as Infinite
			for (int i = 1; i <= n; i++)
			{
				table[i] = ushort.MaxValue;
			}

			// Compute minimum denominations required for all
			// values from 1 to n
			for (long i = 1; i <= n; i++)
			{
				// Go through all denominations smaller than i
				for (int j = 0; j < m; j++)
					if (integralDenominations[j] <= i)
					{
						uint subResult = table[i - integralDenominations[j]];
						if (subResult != ushort.MaxValue && subResult + 1u < table[i])
						{
							table[i] = (ushort) (subResult + 1);
						}
					}
			}
			return table[n] == ushort.MaxValue ? default(ushort) : table[n];
		}

		public static ushort MinChangeCount(this Money money, params decimal[] denominationValues)
		{
			return money.MinChangeCount(denominationValues.Select(v => new Denomination(v)).ToArray());
		}
	}
}