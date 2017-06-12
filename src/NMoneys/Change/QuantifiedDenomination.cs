using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace NMoneys.Change
{
	[CLSCompliant(false)]
	public class QuantifiedDenomination
	{
		internal QuantifiedDenomination(Denomination denomination, uint quantity = 0u)
		{
			Denomination = denomination;
			Quantity = quantity;
		}

		public Denomination Denomination { get; }
		public uint Quantity { get; }

		public override string ToString()
		{
			return $"{Quantity.ToString(CultureInfo.InvariantCulture)} x {Denomination}";
		}

		internal static IEnumerable<QuantifiedDenomination> Aggregate(IEnumerable<Denomination> denominations)
		{
			var aggregation = denominations.GroupBy(_ => _)
				.Select(g => new QuantifiedDenomination(g.Key, (uint) g.Count()));
			return aggregation;
		}
	}
}