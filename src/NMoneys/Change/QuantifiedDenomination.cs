using System;
using System.Globalization;

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

		public override string ToString()
		{
			return $"{Quantity.ToString(CultureInfo.InvariantCulture)} x {Denomination}";
		}
	}
}