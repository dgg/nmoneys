using System.Linq;

namespace NMoneys.Change
{
	public static class Operations
	{
		public static ChangeSolution MinChange(this Money money, Denomination[] denominations)
		{
			if (money.Amount < denominations.Min(d => d.Value)) return new ChangeSolution() { Remainder = money };
			return new ChangeSolution{ new QuantifiedDenomination{Denomination = denominations[0], Quantity = 1}};
		}
	}
}