namespace NMoneys.Tests.Change.Support
{
	// public as it is part of public test signature
	public struct QDenomination
	{
		public QDenomination(uint quantity, decimal denomination)
		{
			Quantity = quantity;
			Denomination = denomination;
		}

		public uint Quantity { get; }
		public decimal Denomination { get; }
	}
}