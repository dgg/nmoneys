using System;

namespace NMoneys.Change
{
	[CLSCompliant(false)]
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