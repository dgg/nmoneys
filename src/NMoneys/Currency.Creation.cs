using System;
using System.Collections.Generic;
using System.Globalization;
using NMoneys.Support;

namespace NMoneys
{
	public sealed partial class Currency
	{
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
		/// <param name="isoCode">ISO 4217 code.</param>
		/// <returns>The instance of <see cref="Currency"/> represented by the <paramref name="isoCode"/>.</returns>
		/// <exception cref="System.ComponentModel.InvalidEnumArgumentException">The <paramref name="isoCode"/> does not exist in the <see cref="CurrencyIsoCode"/> enumeration.</exception>
		/// <exception cref="MisconfiguredCurrencyException">The currency represented by <paramref name="isoCode"/> has not been properly configured by the library implementor. Please, log a issue.</exception>
		public static Currency Get(CurrencyIsoCode isoCode)
		{
			Enumeration.AssertDefined(isoCode);

			Currency currency = _cache.GetOrAdd(isoCode, () =>
			{
				var built = init(isoCode, _provider.Get);
				if (built == null) throw new MisconfiguredCurrencyException(isoCode);
				return built;
			});

			RaiseIfObsolete(isoCode);
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
		/// <exception cref="System.ComponentModel.InvalidEnumArgumentException">The <paramref name="threeLetterIsoCode"/> does not exist in the <see cref="CurrencyIsoCode"/> enumeration.</exception>
		/// <exception cref="MisconfiguredCurrencyException">The currency represented by <paramref name="threeLetterIsoCode"/> has not been properly configured by the library implementor. Please, log a issue.</exception>
		public static Currency Get(string threeLetterIsoCode)
		{
			Currency currency = _cache.GetOrAdd(threeLetterIsoCode, () =>
			{
				var isoCode = Code.ParseArgument(threeLetterIsoCode, "threeLetterIsoCode");
				var built = init(isoCode, _provider.Get);
				if (built == null) throw new MisconfiguredCurrencyException(isoCode);
				return built;
			});
			RaiseIfObsolete(currency);
			return currency;
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
		/// <exception cref="System.ComponentModel.InvalidEnumArgumentException">The ISO symbol associated to the <paramref name="culture"/> does not exist in the <see cref="CurrencyIsoCode"/> enumeration.</exception>
		/// <exception cref="MisconfiguredCurrencyException">The currency associated to the <paramref name="culture"/> has not been properly configured by the library implementor. Please, log a issue.</exception>
		public static Currency Get(CultureInfo culture)
		{
			Guard.AgainstNullArgument("culture", culture);
			return Get(new RegionInfo(culture.LCID));
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
		/// <exception cref="System.ComponentModel.InvalidEnumArgumentException">The ISO symbol associated to the <paramref name="region"/> does not exist in the <see cref="CurrencyIsoCode"/> enumeration.</exception>
		/// <exception cref="MisconfiguredCurrencyException">The currency associated to the <paramref name="region"/> has not been properly configured by the library implementor. Please, log a issue.</exception>
		public static Currency Get(RegionInfo region)
		{
			Guard.AgainstNullArgument("region", region);
			return Get(region.ISOCurrencySymbol);
		}

		/// <summary>
		/// Obtains the instance of <see cref="Currency"/> associated to the <see cref="CurrencyIsoCode"/> specified.
		/// A return value indicates wheter the lookup succeeded.
		/// </summary>
		/// <remarks><see cref="Currency"/> behaves as a singleton, therefore successive calls to <see cref="Currency.TryGet(CurrencyIsoCode, out Currency)"/>
		/// will obtain the same instance of <see cref="Currency"/>.
		/// <para>An internal cache with the information of the currency is maintained.
		/// If it is the first time such currency is obtained within the context of the running application, the information
		/// will be loaded individually.</para>
		/// <para>If many different instances of <see cref="Currency"/> are to be used, it is recommended the use of <see cref="InitializeAllCurrencies"/>
		/// in order to save some initialization time.</para></remarks>
		/// <param name="isoCode">ISO 4217 code.</param>
		/// <param name="currency">When this method returns, contains the <see cref="Currency"/> instance represented by the <paramref name="isoCode"/> if the
		/// lookup suceeds, or null if the lookup fails.</param>
		/// <returns>true if <paramref name="isoCode"/> was looked up successfully; otherwise, false.</returns>
		public static bool TryGet(CurrencyIsoCode isoCode, out Currency currency)
		{
			bool tryGet = false;
			currency = null;

			if (Enumeration.CheckDefined(isoCode))
			{
				tryGet = true;
				currency = _cache.GetOrAdd(isoCode, () => init(isoCode, _provider.Get));
			}
			RaiseIfObsolete(currency);
			return tryGet;
		}

		/// <summary>
		/// Obtains the instance of <see cref="Currency"/> associated to the ISO symbol specified.
		/// A return value indicates wheter the lookup succeeded.
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
		/// lookup suceeds, or null if the lookup fails.</param>
		/// <returns>true if <paramref name="threeLetterIsoSymbol"/> was looked up successfully; otherwise, false.</returns>
		public static bool TryGet(string threeLetterIsoSymbol, out Currency currency)
		{
			bool tryGet = false;
			currency = null;
			CurrencyIsoCode? isoCode;

			if (threeLetterIsoSymbol != null && Enumeration.TryParse(threeLetterIsoSymbol.ToUpperInvariant(), out isoCode))
			{
				tryGet = true;
				currency = _cache.GetOrAdd(isoCode.GetValueOrDefault(), () =>
					init(isoCode.GetValueOrDefault(), _provider.Get));
			}
			RaiseIfObsolete(currency);
			return tryGet;
		}

		/// <summary>
		/// Obtains the instance of <see cref="Currency"/> associated to the <see cref="CultureInfo"/> specified.
		/// A return value indicates wheter the lookup succeeded.
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
		/// the <paramref name="culture"/> if the lookup suceeds, or null if the lookup fails.</param>
		/// <returns>true if <paramref name="currency"/> was looked up successfully; otherwise, false.</returns>
		public static bool TryGet(CultureInfo culture, out Currency currency)
		{
			bool tryGet = false;
			currency = null;

			if (culture != null && !culture.IsNeutralCulture && !culture.Equals(CultureInfo.InvariantCulture))
			{
				var region = new RegionInfo(culture.LCID);
				tryGet = TryGet(region, out currency);
			}
			return tryGet;
		}

		/// <summary>
		/// Obtains the instance of <see cref="Currency"/> associated to the <see cref="RegionInfo"/> specified.
		/// A return value indicates wheter the lookup succeeded.
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
		/// the <paramref name="region"/> if the lookup suceeds, or null if the lookup fails.</param>
		/// <returns>true if <paramref name="currency"/> was looked up successfully; otherwise, false.</returns>
		public static bool TryGet(RegionInfo region, out Currency currency)
		{
			bool tryGet = false;
			currency = null;

			if (region != null)
			{
				tryGet = TryGet(region.ISOCurrencySymbol, out currency);
			}
			return tryGet;
		}

		/// <summary>
		/// Retrieves all currencies.
		/// </summary>
		/// <remarks>Since all currencies are visited, caches are initialized with all values.</remarks>
		/// <returns>List of all currencies defined.</returns>
		public static IEnumerable<Currency> FindAll()
		{
			CurrencyIsoCode[] isoCodes = Enumeration.GetValues<CurrencyIsoCode>();
			using (var initializer = CurrencyInfo.CreateInitializer())
			{
				for (int i = 0; i < isoCodes.Length; i++)
				{
					CurrencyIsoCode isoCode = isoCodes[i];
					var copy = initializer;
					Currency currency = _cache.GetOrAdd(isoCode, () => init(isoCode, copy.Get));
					RaiseIfObsolete(isoCode);
					yield return currency;
				}
			}
		}
	}
}
