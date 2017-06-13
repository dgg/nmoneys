using System;
using System.Linq;
using NMoneys.Extensions;

namespace NMoneys.Change
{
	[CLSCompliant(false)]
	public static class MakeOptimalChangeOperation
	{
		public static OptimalChangeSolution MakeOptimalChange(this Money money, params Denomination[] denominations)
		{
			Positive.Amounts.AssertArgument(nameof(money), money.Amount);

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

			// Compute minimum denominations required for all
			// values from 1 to n
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

		public static OptimalChangeSolution MakeOptimalChange(this Money money, params decimal[] denominationValues)
		{
			return money.MakeOptimalChange(denominationValues.Select(v => new Denomination(v)).ToArray());
		}
	}
}