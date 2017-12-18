using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace NMoneys.Tools.CompareIso
{
	internal class ImplementedCurrenciesCollection : IReadOnlyCollection<Currency>
	{
		private readonly Dictionary<short, Currency> _currencies;

		public ImplementedCurrenciesCollection()
		{
			_currencies = new Dictionary<short, Currency>();
		}

		private void add(Currency implemented)
		{
			_currencies.Add(implemented.NumericCode, implemented);
		}

		public ImplementedCurrenciesCollection AddRange(IEnumerable<Currency> implemented)
		{
			foreach (var i in implemented ?? Enumerable.Empty<Currency>())
			{
				add(i);
			}
			return this;
		}

		public int Count => _currencies.Count;

		public IEnumerator<Currency> GetEnumerator()
		{
			return _currencies.Values.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public Currency this[short numericCode] => _currencies[numericCode];

		public IEnumerable<Currency> Except(IsoCurrenciesCollection scrapped)
		{
			var except = _currencies.Values.Select(c => !scrapped.Contains(c.NumericCode) ? c : null)
				.Where(c => c != null);
			return except;
		}
	}
}