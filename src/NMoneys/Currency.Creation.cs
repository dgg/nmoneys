using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Globalization;

namespace NMoneys;

public partial class Currency
{
	#region Get

	/// <summary>
	/// Obtains the instance of <see cref="Currency"/> associated to the <see cref="CurrencyIsoCode"/> specified.
	/// </summary>
	/// <remarks><see cref="Currency"/> behaves as a singleton, therefore successive calls to <see cref="Currency.Get(CurrencyIsoCode)"/>
	/// will return the same instance of <see cref="Currency"/>.
	/// <para>An internal cache with the information of the currency is maintained.
	/// If it is the first time such currency is obtained within the context of the running application, the information
	/// will be loaded individually.</para>
	/// <para>If many different instances of <see cref="Currency"/> are to be used, it is recommended the use of <see cref="InitializeAllCurrencies"/>
	/// in order to save some initialization time.</para></remarks>
	/// <param name="code">ISO 4217 code.</param>
	/// <returns>The instance of <see cref="Currency"/> represented by the <paramref name="code"/>.</returns>
	/// <exception cref="UndefinedCodeException">The <paramref name="code"/> does not exist in the <see cref="CurrencyIsoCode"/> enumeration.</exception>
	/// <exception cref="MisconfiguredCurrencyException">The currency represented by <paramref name="code"/> has not been properly configured by the library implementor. Please, log a issue.</exception>
	[Pure]
	public static Currency Get(CurrencyIsoCode code)
	{
		code.AssertDefined();

		Currency currency = Currencies.GetOrAdd(code, (c) =>
		{
			InfoAttribute attribute = InfoAttribute.GetFrom(c);
			return new Currency(c, attribute);
		});

		//RaiseIfObsolete(code);
		return currency;
	}

	/// <summary>
	/// Obtains the instance of <see cref="Currency"/> associated to the <paramref name="threeLetterIsoCode"/> specified.
	/// </summary>
	/// <remarks><see cref="Currency"/> behaves as a singleton, therefore successive calls to <see cref="Currency.Get(string)"/>
	/// will return the same instance of <see cref="Currency"/>.
	/// <para>An internal cache with the information of the currency is maintained.
	/// If it is the first time such currency is obtained within the context of the running application, the information
	/// will be loaded individually.</para>
	/// <para>If many different instances of <see cref="Currency"/> are to be used, it is recommended the use of <see cref="InitializeAllCurrencies"/>
	/// in order to save some initialization time.</para></remarks>
	/// <param name="threeLetterIsoCode">A string representing a three-letter ISO 4217 code.</param>
	/// <returns>The instance of <see cref="Currency"/> represented by the <paramref name="threeLetterIsoCode"/>.</returns>
	/// <exception cref="ArgumentException">The <paramref name="threeLetterIsoCode"/> does not exist in the <see cref="CurrencyIsoCode"/> enumeration.</exception>
	/// <exception cref="MisconfiguredCurrencyException">The currency represented by <paramref name="threeLetterIsoCode"/> has not been properly configured by the library implementor. Please, log a issue.</exception>
	[Pure]
	public static Currency Get(string threeLetterIsoCode)
	{
		CurrencyIsoCode code = Enum.Parse<CurrencyIsoCode>(threeLetterIsoCode, ignoreCase: true);
		return Get(code);
	}

	/// <summary>
	/// Obtains the instance of <see cref="Currency"/> associated to the <see cref="CultureInfo"/> specified.
	/// </summary>
	/// <remarks><see cref="Currency"/> behaves as a singleton, therefore successive calls to <see cref="Currency.Get(CultureInfo)"/>
	/// will return the same instance of <see cref="Currency"/>.
	/// <para>An internal cache with the information of the currency is maintained.
	/// If it is the first time such currency is obtained within the context of the running application, the information
	/// will be loaded individually.</para>
	/// <para>If many different instances of <see cref="Currency"/> are to be used, it is recommended the use of <see cref="InitializeAllCurrencies"/>
	/// in order to save some initialization time.</para>
	/// <para>There might be cases that the framework will provide non-standard or out-dated information for
	/// the given <paramref name="culture"/>. In this case it might be possible that an exception is thrown even if the region
	/// corresponding to the <paramref name="culture"/> can be created.</para>
	/// </remarks>
	/// <param name="culture">A <see cref="CultureInfo"/> from which retrieve the associated currency.</param>
	/// <returns>The instance of <see cref="Currency"/> from to the region associated to the <paramref name="culture"/>.</returns>
	/// <exception cref="ArgumentNullException"><paramref name="culture"/> is null.</exception>
	/// <exception cref="ArgumentException"><paramref name="culture"/> is either an invariant, custom or neutral culture, or a <see cref="RegionInfo"/> cannot be instantiated from it.</exception>
	/// <exception cref="ArgumentException">The ISO symbol associated to the <paramref name="culture"/> does not exist in the <see cref="CurrencyIsoCode"/> enumeration.</exception>
	/// <exception cref="MisconfiguredCurrencyException">The currency associated to the <paramref name="culture"/> has not been properly configured by the library implementor. Please, log a issue.</exception>
	[Pure]
	public static Currency Get(CultureInfo culture)
	{
		ArgumentNullException.ThrowIfNull(culture, nameof(culture));
		return Get(new RegionInfo(culture.Name));
	}

	/// <summary>
	/// Obtains the instance of <see cref="Currency"/> associated to the <see cref="RegionInfo"/> specified.
	/// </summary>
	/// <remarks><see cref="Currency"/> behaves as a singleton, therefore successive calls to <see cref="Currency.Get(CultureInfo)"/>
	/// will return the same instance of <see cref="Currency"/>.
	/// <para>An internal cache with the information of the currency is maintained.
	/// If it is the first time such currency is obtained within the context of the running application, the information
	/// will be loaded individually.</para>
	/// <para>If many different instances of <see cref="Currency"/> are to be used, it is recommended the use of <see cref="InitializeAllCurrencies"/>
	/// in order to save some initialization time.</para>
	/// <para>There might be cases that the framework will provide non-standard or out-dated information for
	/// the given <paramref name="region"/>. In this case it might be possible that an exception is thrown.</para>
	/// </remarks>
	/// <param name="region">A <see cref="RegionInfo"/> from which retrieve the associated currency.</param>
	/// <returns>The instance of <see cref="Currency"/> corresponding to the region.</returns>
	/// <exception cref="ArgumentNullException"><paramref name="region"/> is null.</exception>
	/// <exception cref="ArgumentException">The ISO symbol associated to the <paramref name="region"/> does not exist in the <see cref="CurrencyIsoCode"/> enumeration.</exception>
	/// <exception cref="MisconfiguredCurrencyException">The currency associated to the <paramref name="region"/> has not been properly configured by the library implementor. Please, log a issue.</exception>
	[Pure]
	public static Currency Get(RegionInfo region)
	{
		ArgumentNullException.ThrowIfNull(region, nameof(region));
		return Get(region.ISOCurrencySymbol);
	}

	#endregion

	#region TryGet

	/// <summary>
	/// Obtains the instance of <see cref="Currency"/> associated to the <see cref="CurrencyIsoCode"/> specified.
	/// A return value indicates whether the lookup succeeded.
	/// </summary>
	/// <remarks><see cref="Currency"/> behaves as a singleton, therefore successive calls to <see cref="Currency.TryGet(CurrencyIsoCode, out Currency)"/>
	/// will obtain the same instance of <see cref="Currency"/>.
	/// <para>An internal cache with the information of the currency is maintained.
	/// If it is the first time such currency is obtained within the context of the running application, the information
	/// will be loaded individually.</para>
	/// <para>If many different instances of <see cref="Currency"/> are to be used, it is recommended the use of <see cref="InitializeAllCurrencies"/>
	/// in order to save some initialization time.</para></remarks>
	/// <param name="code">ISO 4217 code.</param>
	/// <param name="currency">When this method returns, contains the <see cref="Currency"/> instance represented by the <paramref name="code"/> if the
	/// lookup succeeds, or null if the lookup fails.</param>
	/// <returns>true if <paramref name="code"/> was looked up successfully; otherwise, false.</returns>
	/// <exception cref="MisconfiguredCurrencyException">The currency represented by <paramref name="code" />
	/// has not been properly configured by the library implementor. Please, log a issue.</exception>
	[Pure]
	public static bool TryGet(CurrencyIsoCode code, out Currency? currency)
	{
		bool tryGet = false;
		currency = null;

		if (code.CheckDefined())
		{
			tryGet = true;
			currency = Currencies.GetOrAdd(code, (c) =>
			{
				InfoAttribute attribute = InfoAttribute.GetFrom(c);
				return new Currency(c, attribute);
			});
		}

		//RaiseIfObsolete(currency);
		return tryGet;
	}

	/// <summary>
	/// Obtains the instance of <see cref="Currency"/> associated to the ISO symbol specified.
	/// A return value indicates whether the lookup succeeded.
	/// </summary>
	/// <remarks><see cref="Currency"/> behaves as a singleton, therefore successive calls to <see cref="Currency.TryGet(string, out Currency)"/>
	/// will obtain the same instance of <see cref="Currency"/>.
	/// <para>An internal cache with the information of the currency is maintained.
	/// If it is the first time such currency is obtained within the context of the running application, the information
	/// will be loaded individually.</para>
	/// <para>If many different instances of <see cref="Currency"/> are to be used, it is recommended the use of <see cref="InitializeAllCurrencies"/>
	/// in order to save some initialization time.</para></remarks>
	/// <param name="threeLetterIsoSymbol">A string representing a three-letter ISO 4217 code.</param>
	/// <param name="currency">When this method returns, contains the <see cref="Currency"/> instance represented by the <paramref name="threeLetterIsoSymbol"/> if the
	/// lookup succeeds, or null if the lookup fails.</param>
	/// <returns>true if <paramref name="threeLetterIsoSymbol"/> was looked up successfully; otherwise, false.</returns>
	[Pure]
	public static bool TryGet(string threeLetterIsoSymbol, out Currency? currency)
	{
		bool tryGet = false;
		currency = null;

		if (Enum.TryParse(threeLetterIsoSymbol, ignoreCase: false, out CurrencyIsoCode code))
		{
			return TryGet(code, out currency);
		}

		//RaiseIfObsolete(currency);
		return tryGet;
	}

	/// <summary>
	/// Obtains the instance of <see cref="Currency"/> associated to the <see cref="CultureInfo"/> specified.
	/// A return value indicates whether the lookup succeeded.
	/// </summary>
	/// <remarks><see cref="Currency"/> behaves as a singleton, therefore successive calls to <see cref="Currency.TryGet(CultureInfo, out Currency)"/>
	/// will obtain the same instance of <see cref="Currency"/>.
	/// <para>An internal cache with the information of the currency is maintained.
	/// If it is the first time such currency is obtained within the context of the running application, the information
	/// will be loaded individually.</para>
	/// <para>If many different instances of <see cref="Currency"/> are to be used, it is recommended the use of <see cref="InitializeAllCurrencies"/>
	/// in order to save some initialization time.</para>
	/// <para>There might be cases that the framework will provide non-standard or out-dated information for
	/// the given <paramref name="culture"/>. In this case it might be possible that the lookup is not successful even if the region
	/// corresponding to the <paramref name="culture"/> can be created.</para>
	/// </remarks>
	/// <param name="culture">A <see cref="CultureInfo"/> from which retrieve the associated currency.</param>
	/// <param name="currency">When this method returns, contains the <see cref="Currency"/> instance from the region associated to
	/// the <paramref name="culture"/> if the lookup succeeds, or null if the lookup fails.</param>
	/// <returns>true if <paramref name="currency"/> was looked up successfully; otherwise, false.</returns>
	[Pure]
	public static bool TryGet([NotNull] CultureInfo culture, out Currency? currency)
	{
		bool tryGet = false;
		currency = null;

		if (!culture.IsNeutralCulture && !culture.Equals(CultureInfo.InvariantCulture))
		{
			var region = new RegionInfo(culture.Name);
			tryGet = TryGet(region, out currency);
		}

		return tryGet;
	}

	/// <summary>
	/// Obtains the instance of <see cref="Currency"/> associated to the <see cref="RegionInfo"/> specified.
	/// A return value indicates whether the lookup succeeded.
	/// </summary>
	/// <remarks><see cref="Currency"/> behaves as a singleton, therefore successive calls to <see cref="Currency.TryGet(RegionInfo, out Currency)"/>
	/// will obtain the same instance of <see cref="Currency"/>.
	/// <para>An internal cache with the information of the currency is maintained.
	/// If it is the first time such currency is obtained within the context of the running application, the information
	/// will be loaded individually.</para>
	/// <para>If many different instances of <see cref="Currency"/> are to be used, it is recommended the use of <see cref="InitializeAllCurrencies"/>
	/// in order to save some initialization time.</para>
	/// <para>There might be cases that the framework will provide non-standard or out-dated information for
	/// the given <paramref name="region"/>. In this case it might be possible that the lookup is not successful.</para>
	/// </remarks>
	/// <param name="region">A <see cref="RegionInfo"/> from which retrieve the associated currency.</param>
	/// <param name="currency">When this method returns, contains the <see cref="Currency"/> instance from the region associated to
	/// the <paramref name="region"/> if the lookup succeeds, or null if the lookup fails.</param>
	/// <returns>true if <paramref name="currency"/> was looked up successfully; otherwise, false.</returns>
	[Pure]
	public static bool TryGet([NotNull] RegionInfo region, out Currency? currency)
	{
		return TryGet(region.ISOCurrencySymbol, out currency);
	}

	#endregion
}
