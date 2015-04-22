using System;
using System.Collections.Concurrent;
using NMoneys.Support;

namespace NMoneys
{
	internal class CurrencyCache
	{
		private static readonly ConcurrentDictionary<string, Currency> _byIsoSymbol;
		private static readonly ConcurrentDictionary<CurrencyIsoCode, Currency> _byIsoCode;

		static CurrencyCache()
		{
			int cacheCapacity = Enum.GetNames(typeof(CurrencyIsoCode)).Length;
			int concurrency = Environment.ProcessorCount * 4;
			_byIsoSymbol = new ConcurrentDictionary<string, Currency>(concurrency, cacheCapacity, StringComparer.OrdinalIgnoreCase);
			// we use a fast comparer as we have quite a few enum keys
			_byIsoCode = new ConcurrentDictionary<CurrencyIsoCode, Currency>(concurrency, cacheCapacity, Enumeration.Comparer<CurrencyIsoCode>());
		}

		public Currency Add(Currency currency)
		{
			_byIsoCode.TryAdd(currency.IsoCode, currency);
			_byIsoSymbol.TryAdd(currency.IsoSymbol, currency);

			return currency;
		}

		public Currency GetOrAdd(string key, Func<Currency> add)
		{
			return _byIsoSymbol.GetOrAdd(key, k => Add(add()));
		}

		public Currency GetOrAdd(CurrencyIsoCode key, Func<Currency> add)
		{
			return _byIsoCode.GetOrAdd(key, k => Add(add()));
		}
	}
}