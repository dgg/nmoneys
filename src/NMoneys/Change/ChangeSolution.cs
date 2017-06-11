using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace NMoneys.Change
{
	[CLSCompliant(false)]
	public class ChangeSolution : IEnumerable<QuantifiedDenomination>
	{
		private readonly QuantifiedDenomination[] _denominations;
		internal ChangeSolution(IEnumerable<Denomination> usedDenominations, Money remainder)
		{
			Remainder = remainder;
			_denominations = usedDenominations.GroupBy(_ => _)
				.Select(g => new QuantifiedDenomination(g.Key, (uint) g.Count()))
				.ToArray();
		}

		public IEnumerator<QuantifiedDenomination> GetEnumerator()
		{
			return _denominations.Cast<QuantifiedDenomination>().GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public QuantifiedDenomination this[int idx] => _denominations[idx];
		public uint Count => (uint)_denominations.Length;

		public Money Remainder { get; }
	}
}