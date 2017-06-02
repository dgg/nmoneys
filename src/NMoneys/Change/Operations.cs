namespace NMoneys.Change
{
	public static class Operations
	{
		public static ChangeSolution MinChange(this Money money, Denomination[] denominations)
		{
			return new ChangeSolution{ new QuantifiedDenomination{Denomination = denominations[0], Quantity = 1}};
		}
	}
}