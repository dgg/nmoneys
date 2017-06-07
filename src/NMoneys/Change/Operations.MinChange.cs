using System;
using System.Linq;

namespace NMoneys.Change
{
	[CLSCompliant(false)]
	public static class MinChangeOperation
	{
		public static MinChangeSolution MinChange(this Money money, params Denomination[] denominations)
		{
			Positive.Amounts.AssertArgument(nameof(money), money.Amount);

			Denomination[] orderedDenominations = denominations.OrderByDescending(d => d.Value).ToArray();
			decimal remainder = money.Amount;
			bool canContinue = true;

			MinChangeSolution solution = new MinChangeSolution();
			while (remainder > 0 && canContinue)
			{
				for (int i = 0; i < orderedDenominations.Length; i++)
				{
					while (orderedDenominations[i].CanBeSubstractedFrom(remainder))
					{
						orderedDenominations[i].SubstractFrom(ref remainder);
						solution.AddOrUpdate(orderedDenominations[i], 
							d => d.Increase());
					}
				}
				canContinue = false;
			}
			solution.Remainder = new Money(remainder, money.CurrencyCode);

			return solution;
		}

		public static MinChangeSolution MinChange(this Money money, params decimal[] denominationValues)
		{
			return money.MinChange(denominationValues.Select(v => new Denomination(v)).ToArray());
		}
	}
}