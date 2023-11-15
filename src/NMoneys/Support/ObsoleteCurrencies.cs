using System.Diagnostics.Contracts;

namespace NMoneys.Support;

/// <summary>
/// Maintains a list of obsolete currencies
/// </summary>
internal static class ObsoleteCurrencies
{
	private static readonly HashSet<CurrencyIsoCode> _set = new(
		Enum.GetValues<CurrencyIsoCode>()
			.Where(c => c.HasAttribute<ObsoleteAttribute>()),
	Currency.Code.Comparer);

	[Pure]
	public static bool IsObsolete(CurrencyIsoCode code)
	{
		return _set.Contains(code);
	}

	[Pure]
	public static bool IsObsolete(Currency currency)
	{
		return IsObsolete(currency.IsoCode);
	}

	[Pure] public static ushort Count => Convert.ToUInt16(_set.Count);
}
