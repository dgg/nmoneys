namespace NMoneys.Change
{
	public class QuantifiedDenomination
	{
		public QuantifiedDenomination(Denomination denomination)
		{
			Denomination = denomination;
		}

		public Denomination Denomination { get; }
		public uint Quantity { get; private set; }

		internal void Increase()
		{
			Quantity++;
		}
	}
}