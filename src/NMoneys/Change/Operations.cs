using System.Linq;

namespace NMoneys.Change
{
	public static class Operations
	{
		public static ChangeSolution MinChange(this Money money, Denomination[] denominations)
		{
			Positive.Amounts.AssertArgument(nameof(money), money.Amount);

			Denomination[] orderedDenominations = denominations.OrderByDescending(d => d.Value).ToArray();
			decimal remainder = money.Amount;
			bool canContinue = true;

			ChangeSolution solution = new ChangeSolution();
			while (remainder > 0 && canContinue)
			{
				for (int i = 0; i < orderedDenominations.Length; i++)
				{
					while (orderedDenominations[i].CanBeSubstractedFrom(remainder))
					{
						remainder -= orderedDenominations[i].Value;
						var quantified = solution.FirstOrDefault(d => d.Denomination.Value == orderedDenominations[i].Value);
						// already in solution
						if (quantified != null)
						{
							quantified.Quantity++;
						}
						// first ocurrence
						else
						{
							solution.Add(new QuantifiedDenomination{ Denomination = orderedDenominations[i], Quantity = 1u});
						}
					}
				}
				canContinue = false;
			}
			solution.Remainder = new Money(remainder, money.CurrencyCode);

			return solution;
		}
	}
}