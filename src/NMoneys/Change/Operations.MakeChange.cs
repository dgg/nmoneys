using System.Collections.Generic;
using System.Linq;

namespace NMoneys.Change
{
	public static partial class ChangeOperations
	{
		public static ChangeSolution MakeChange(this Money money, params Denomination[] denominations)
		{
			Positive.Amounts.AssertArgument(nameof(money), money.Amount);

			Denomination[] orderedDenominations = denominations.OrderByDescending(d => d.Value).ToArray();
			decimal remainder = money.Amount;
			bool canContinue = true;

			List<Denomination> usedDenominations = new List<Denomination>();
			
			while (remainder > 0 && canContinue)
			{
				for (int i = 0; i < orderedDenominations.Length; i++)
				{
					// if the denomination can be substracted from what's left
					while (remainder >= orderedDenominations[i].Value)
					{
						// use the denomination
						remainder -= orderedDenominations[i].Value;
						usedDenominations.Add(orderedDenominations[i]);
					}
				}
				// make sure we can finish in case we cannot change
				canContinue = false;
			}
			ChangeSolution solution = new ChangeSolution(usedDenominations, 
				new Money(remainder, money.CurrencyCode));
			return solution;
		}

		public static ChangeSolution MakeChange(this Money money, params decimal[] denominationValues)
		{
			return money.MakeChange(denominationValues.Select(v => new Denomination(v)).ToArray());
		}
	}
}