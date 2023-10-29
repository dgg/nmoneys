using NMoneys.Support;

namespace NMoneys;

public partial class Currency
{
	/// <summary>
	/// Allows overriding how a <see cref="Currency"/> behaves.
	/// </summary>
	/// <remarks>This method needs to be called at the beginning of the application as configuring already initialized currencies is NOT supported.</remarks>
	/// <param name="code">ISO 4217 code identifying the currency</param>
	/// <param name="configuration">Configuration override.</param>
	/// <exception cref="UndefinedCodeException">The currency identified by <paramref name="code"/> is not defined in the enumeration.</exception>
	/// <exception cref="InitializedCurrencyException">The currency has already been initialized (or configured).</exception>
	public static void Configure(CurrencyIsoCode code, CurrencyConfiguration configuration)
	{
		code.AssertDefined();

		Currency currency = Currencies.AddOrThrow(code, (c) =>
		{
			InfoAttribute attribute = InfoAttribute.GetFrom(c);
			CurrencyInfo merged = attribute.MergeWith(configuration);
			return new Currency(c, merged);
		});
	}

	/// <summary>
	/// Allows overriding how multiple <see cref="Currency"/> behave.
	/// </summary>
	/// <remarks>This method needs to be called at the beginning of the application as configuring already initialized currencies is NOT supported.</remarks>
	/// <param name="configurations">Configuration overrides.</param>
	/// <exception cref="UndefinedCodeException">A currency code of any of the <paramref name="configurations"/> is not defined in the enumeration.</exception>
	/// <exception cref="InitializedCurrencyException">A currency of the <paramref name="configurations"/> has already been initialized (or configured).</exception>
	public static void Configure(ValueTuple<CurrencyIsoCode, CurrencyConfiguration>[] configurations)
	{
		ArgumentNullException.ThrowIfNull(configurations, nameof(configurations));
		foreach (var tuple in configurations)
		{
			Configure(tuple.Item1, tuple.Item2);
		}
	}

	/// <summary>
	/// Actively initializes the information for all currencies.
	/// </summary>
	/// <remarks>Use this method if you plan to use a lot of currencies in your program.
	/// <para>When most of currencies are expected to be used, it is recommended to initialize the information for all of them,
	/// saving time each time the first instance is accessed.</para></remarks>
	public static void InitializeAllCurrencies()
	{
		CurrencyIsoCode[] isoCodes = Enum.GetValues<CurrencyIsoCode>();

		for (int i = 0; i < isoCodes.Length; i++)
		{
			CurrencyIsoCode code = isoCodes[i];
			_ = Get(code);
		}
	}
}
