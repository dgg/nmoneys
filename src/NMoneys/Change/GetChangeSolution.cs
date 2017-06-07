using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace NMoneys.Change
{
	[CLSCompliant(false)]
	public class GetChangeSolution : IEnumerable<QuantifiedDenomination>
	{
		private readonly List<QuantifiedDenomination> _denominations = new List<QuantifiedDenomination>();

		public QuantifiedDenomination this[int index] => _denominations[index];

		public IEnumerator<QuantifiedDenomination> GetEnumerator()
		{
			return _denominations.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public uint Count => (uint)_denominations.Count;

		public Money Remainder { get; set; }

		internal void AddOrUpdate(Denomination denomination, Action<QuantifiedDenomination> action)
		{
			QuantifiedDenomination quantified = _denominations.FirstOrDefault(d => d.Denomination.Value == denomination.Value);
			if (quantified == null)
			{
				quantified = new QuantifiedDenomination(denomination);
				_denominations.Add(quantified);
			}
			action(quantified);
		}
	}
}