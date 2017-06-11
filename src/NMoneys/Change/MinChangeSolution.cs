using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace NMoneys.Change
{
	public class MinChangeSolution : IEnumerable<QuantifiedDenomination>
	{
		private readonly QuantifiedDenomination[] _denominations;

		internal MinChangeSolution(long toChange, Currency operationCurrency, ushort[] table, IntegralDenomination?[] usedDenominations)
		{
			_denominations = new QuantifiedDenomination[0];

			ushort possibleSolution = table.Last();
			if (possibleSolution != ushort.MaxValue)
			{
				List<Denomination> denominations = new List<Denomination>();
				IntegralDenomination defaultDenomination = IntegralDenomination.Default(operationCurrency);

				long denomination = toChange;
				while (denomination > 0)
				{
					IntegralDenomination usedDenomination = usedDenominations[denomination]
						.GetValueOrDefault(defaultDenomination);
					denominations.Add(usedDenomination.Denomination);
					denomination -= usedDenomination.IntegralAmount;
				}
				_denominations = denominations.GroupBy(_ => _)
					.Select(g => new QuantifiedDenomination(g.Key, (uint) g.Count()))
					.ToArray();
			}
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
	}
}