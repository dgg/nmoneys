using System.Collections;
using System.Collections.Generic;
using NMoneys.Extensions;

namespace NMoneys
{
	public class ChangeSolution: IEnumerable<QuantifiedDenomination>
	{
		private List<QuantifiedDenomination> _denominations = new List<QuantifiedDenomination>();

		public QuantifiedDenomination this[int index] => _denominations[index];

		public void Add(QuantifiedDenomination denomination)
		{
			_denominations.Add(denomination);
		}

		public IEnumerator<QuantifiedDenomination> GetEnumerator()
		{
			return _denominations.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public uint Count { get { return (uint)_denominations.Count; } }

		public Money Remainder => 0m.Xxx();
	}
}