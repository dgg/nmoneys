using System.Linq;
using NMoneys.Extensions;

namespace NMoneys.Change
{
	public static partial class ChangeOperations
	{
		public static ushort CountOptimalChange(this Money money, params Denomination[] denominations)
		{
			Positive.Amounts.AssertArgument(nameof(money), money.Amount);

			long n = money.MinorIntegralAmount;
			int m = denominations.Length;
			Currency currencyOperation = money.GetCurrency();
			IntegralDenomination[] integralDenominations = denominations
				.Select(d => new IntegralDenomination(d, currencyOperation))
				.ToArray();
				
			// table[i] will be storing the minimum number of denominations
			// required for i value.  So table[n] will have result
			ushort[] table = new ushort[n + 1];

			// Base case (If given value n is 0)
			table[0] = 0;
			
			// Compute minimum denominations required for all
			// values from 1 to n
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

		public static ushort CountOptimalChange(this Money money, params decimal[] denominationValues)
		{
			return money.CountOptimalChange(denominationValues.Select(v => new Denomination(v)).ToArray());
		}
	}
}