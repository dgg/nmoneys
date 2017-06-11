using System;
using System.Collections.Generic;
using System.Linq;

namespace NMoneys.Change
{
	[CLSCompliant(false)]
	public static class GetChangeOperation
	{
		public static ChangeSolution GetChange(this Money money, params Denomination[] denominations)
		{
			Positive.Amounts.AssertArgument(nameof(money), money.Amount);

			Denomination[] orderedDenominations = denominations.OrderByDescending(d => d.Value).ToArray();
			decimal remainder = money.Amount;
			bool canContinue = true;

			List<Denomination> usedDenominations = new List<Denomination>(denominations.Length);
			
			while (remainder > 0 && canContinue)
			{
				for (int i = 0; i < orderedDenominations.Length; i++)
				{
					while (orderedDenominations[i].CanBeSubstractedFrom(remainder))
					{
						orderedDenominations[i].SubstractFrom(ref remainder);
						usedDenominations.Add(orderedDenominations[i]);
					}
				}
				canContinue = false;
			}
			ChangeSolution solution = new ChangeSolution(usedDenominations, new Money(remainder, money.CurrencyCode));
			return solution;
		}

		public static ChangeSolution GetChange(this Money money, params decimal[] denominationValues)
		{
			return money.GetChange(denominationValues.Select(v => new Denomination(v)).ToArray());
		}
	}
}