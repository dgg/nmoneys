using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace NMoneys.Change
{
	public class MinChangeSolution : IEnumerable<QuantifiedDenomination>
	{
		private IEnumerable<QuantifiedDenomination> _denominations;

		internal MinChangeSolution(long toChange, Currency operationCurrency, ushort[] table, IntegralDenomination?[] usedDenominations)
		{
			_denominations = Enumerable.Empty<QuantifiedDenomination>();

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
					.Select(g => new QuantifiedDenomination(g.Key, (uint) g.Count()));
			}
		}

		public IEnumerator<QuantifiedDenomination> GetEnumerator()
		{
			return _denominations.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}