using System.Collections;
using System.Collections.Generic;

namespace NMoneys.Tools.CompareIso
{
	// some countries share the same currency, so we normalize them
	internal class IsoCurrenciesCollection : IReadOnlyCollection<IsoCurrency>
	{
		private readonly Dictionary<short, IsoCurrency> _currencies;

		public IsoCurrenciesCollection()
		{
			_currencies = new Dictionary<short, IsoCurrency>();
		}

		private void add(IsoCurrency scrapped)
		{
			if (scrapped != null && scrapped.HasValue) _currencies[scrapped.NumericCode.Value.GetValueOrDefault()] = scrapped;
		}

		public void AddRange(IsoCurrency[] scrapped)
		{
			for (int i = 0; i < scrapped.Length; i++)
			{
				add(scrapped[i]);
			}
		}

		public int Count => _currencies.Count;

		public IEnumerator<IsoCurrency> GetEnumerator()
		{
			return _currencies.Values.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public bool Contains(short numericCode)
		{
			return _currencies.ContainsKey(numericCode);
		}
	}
}