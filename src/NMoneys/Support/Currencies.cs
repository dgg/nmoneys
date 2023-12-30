using System.Collections.Concurrent;

namespace NMoneys.Support;

internal class Currencies
{
	private static readonly int CAPACITY = Enum.GetNames<CurrencyIsoCode>().Length;
	private static readonly int CONCURRENCY = Environment.ProcessorCount * 4;

	private readonly ConcurrentDictionary<string, Currency> _bySymbol;

	// ReSharper disable once ConvertConstructorToMemberInitializers
	public Currencies()
	{
		_bySymbol = new ConcurrentDictionary<string, Currency>(CONCURRENCY, CAPACITY, StringComparer.OrdinalIgnoreCase);
	}

	public Currency GetOrAdd(string isoSymbol, Func<string, Currency> add)
	{
		return _bySymbol.GetOrAdd(isoSymbol, add);
	}

	public Currency GetOrAdd(CurrencyIsoCode code, Func<CurrencyIsoCode, Currency> add)
	{
		return _bySymbol.GetOrAdd(code.ToString(), _ => add(code));
	}

	public Currency AddOrThrow(CurrencyIsoCode code, Func<CurrencyIsoCode, Currency> add)
	{
		return _bySymbol.AddOrUpdate(code.ToString(),
			_ => add(code),
			(k, old) => throw InitializedCurrencyException.ForCode(code)
		);
	}

	public Currency AddOrThrow(string isoSymbol, Func<string, Currency> add)
	{
		return _bySymbol.AddOrUpdate(isoSymbol,
			add,
			(k, old) => throw InitializedCurrencyException.ForCode(k)
		);
	}
}
