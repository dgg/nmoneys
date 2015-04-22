using System;
using System.Globalization;
using System.Xml.Serialization;
using NMoneys.Support;
using NMoneys.Support.Ext;

namespace NMoneys
{
	///<summary>
	/// Represents a currency unit such as Euro or American Dollar.
	///</summary>
	public sealed partial class Currency
	{
		#region properties

		/// <summary>
		/// The ISO 4217 code of the <see cref="Currency"/>
		/// </summary>
		public CurrencyIsoCode IsoCode { get; private set; }

		/// <summary>
		/// The numeric ISO 4217 code of the <see cref="Currency"/>
		/// </summary>
		public short NumericCode { get { return IsoCode.NumericCode(); } }

		/// <summary>
		/// Returns a padded three digit string representation of the <see cref="NumericCode"/>.
		/// </summary>
		public string PaddedNumericCode { get { return IsoCode.PaddedNumericCode(); } }

		/// <summary>
		/// Gets the name, in English, of the <see cref="Currency"/>.
		/// </summary>
		[XmlIgnore]
		public string EnglishName { get; private set; }

		/// <summary>
		/// Gets the currency symbol associated with the <see cref="Currency"/>.
		/// </summary>
		[XmlIgnore]
		public string Symbol { get; private set; }

		/// <summary>
		/// Textual representation of the ISO 4217 code
		/// </summary>
		public string AlphabeticCode { get; private set; }

		/// <summary>
		/// Textual representation of the ISO 4217 code
		/// </summary>
		[XmlIgnore]
		public string IsoSymbol { get; private set; }

		/// <summary>
		/// Indicates the number of decimal places to use in currency values.
		/// </summary>
		[XmlIgnore]
		public int SignificantDecimalDigits { get; private set; }

		/// <summary>
		/// Represents the smalles amount that can be represented for the currency according to its <see cref="SignificantDecimalDigits"/>.
		/// </summary>
		[XmlIgnore]
		public decimal MinAmount { get { return PowerOfTen.Negative(this); } }

		/// <summary>
		/// Gets the name of the currency formatted in the native language of the country/region where the currency is used. 
		/// </summary>
		[XmlIgnore]
		public string NativeName { get; private set; }

		/// <summary>
		/// Gets the string to use as the decimal separator in currency values. 
		/// </summary>
		[XmlIgnore]
		public string DecimalSeparator { get; private set; }

		/// <summary>
		/// Gets the string that separates groups of digits to the left of the decimal in currency values. 
		/// </summary>
		[XmlIgnore]
		public string GroupSeparator { get; private set; }

		/// <summary>
		/// Gets the number of digits in each group to the left of the decimal in currency values. 
		/// </summary>
		[XmlIgnore]
		public int[] GroupSizes { get; private set; }

		/// <summary>
		/// Gets format pattern for negative currency values. 
		/// </summary>
		/// <remarks>For more information about this pattern see <see cref="NumberFormatInfo.CurrencyNegativePattern"/>.</remarks>
		[XmlIgnore]
		public int NegativePattern { get; private set; }

		/// <summary>
		/// Gets the format pattern for positive currency values. 
		/// </summary>
		/// <remarks>For more information about this pattern see <see cref="NumberFormatInfo.CurrencyPositivePattern"/>.</remarks>
		[XmlIgnore]
		public int PositivePattern { get; private set; }

		/// <summary>
		/// Defines how numeric values are formatted and displayed, depending on the culture related to the <see cref="Currency"/>.
		/// </summary>
		[XmlIgnore]
		public NumberFormatInfo FormatInfo { get; private set; }

		/// <summary>
		/// Indicates whether the currency is legal tender or it has been obsoleted
		/// </summary>
		[XmlIgnore]
		public bool IsObsolete { get; private set; }

		/// <summary>
		/// Represents the textual html entity
		/// </summary>
		/// <remarks>Not all currencies have an character reference.
		/// For those who does not have one, a <see cref="CharacterReference.Empty"/> instance is returned.</remarks>
		[XmlIgnore]
		public CharacterReference Entity { get; private set; }

		/// <summary>
		/// Gets the default currency symbol.
		/// </summary>
		public static readonly string DefaultSymbol = CultureInfo.InvariantCulture.NumberFormat.CurrencySymbol;

		#endregion

		#region Ctor

		[Obsolete("serialization")]
		private Currency() { }

		private Currency(CurrencyIsoCode isoCode, string englishName, string nativeName, string symbol, int significantDecimalDigits, string decimalSeparator, string groupSeparator, int[] groupSizes, int positivePattern, int negativePattern, bool isObsolete, CharacterReference entity)
		{
			setAllFields(isoCode, englishName, nativeName, symbol, significantDecimalDigits, decimalSeparator, groupSeparator, groupSizes, positivePattern, negativePattern, isObsolete, entity);
		}

		internal Currency(CurrencyInfo info)
			: this(info.Code, info.EnglishName, info.NativeName,
			info.Symbol, info.SignificantDecimalDigits, info.DecimalSeparator,
			info.GroupSeparator, info.GroupSizes, info.PositivePattern, info.NegativePattern,
			info.Obsolete, info.Entity) { }

		/// <summary>
		/// Allows setting all field both for constructors and serialization methods.
		/// </summary>
		private void setAllFields(CurrencyIsoCode isoCode, string englishName, string nativeName, string symbol, int significantDecimalDigits, string decimalSeparator, string groupSeparator, int[] groupSizes, int positivePattern, int negativePattern, bool isObsolete, CharacterReference entity)
		{
			IsoCode = isoCode;
			EnglishName = englishName;
			Symbol = symbol;
			AlphabeticCode = IsoSymbol = isoCode.ToString();
			SignificantDecimalDigits = significantDecimalDigits;
			NativeName = nativeName;
			DecimalSeparator = decimalSeparator;
			GroupSeparator = groupSeparator;
			GroupSizes = groupSizes;
			PositivePattern = positivePattern;
			NegativePattern = negativePattern;
			IsObsolete = isObsolete;
			Entity = entity;

			FormatInfo = NumberFormatInfo.ReadOnly(new NumberFormatInfo
			{
				CurrencySymbol = symbol,
				CurrencyDecimalDigits = significantDecimalDigits,
				CurrencyDecimalSeparator = decimalSeparator,
				CurrencyGroupSeparator = groupSeparator,
				CurrencyGroupSizes = groupSizes,
				CurrencyPositivePattern = positivePattern,
				CurrencyNegativePattern = negativePattern,
				NumberDecimalDigits = significantDecimalDigits,
				NumberDecimalSeparator = decimalSeparator,
				NumberGroupSeparator = groupSeparator,
				NumberGroupSizes = groupSizes,
				NumberNegativePattern = negativePattern.TranslateNegativePattern(),
			});
		}

		#endregion

		#region static shortcuts & cache initialization

		// random list of static accessors based on currency exchange most used currencies

		/// <summary>
		/// Australia Dollars
		/// </summary>
		public static readonly Currency Aud;
		/// <summary>
		/// Candada Dollars
		/// </summary>
		public static readonly Currency Cad;
		/// <summary>
		/// Switzerland Francs
		/// </summary>
		public static readonly Currency Chf;
		/// <summary>
		/// China Yuan Renminbi
		/// </summary>
		public static readonly Currency Cny;
		/// <summary>
		/// Denmark Kroner
		/// </summary>
		public static readonly Currency Dkk;
		/// <summary>
		/// Euro
		/// </summary>
		public static readonly Currency Eur;
		/// <summary>
		/// United Kingdom Pounds
		/// </summary>
		public static readonly Currency Gbp;
		/// <summary>
		/// Hong Kong Dollars
		/// </summary>
		public static readonly Currency Hkd;
		/// <summary>
		/// Hungary Forint
		/// </summary>
		public static readonly Currency Huf;
		/// <summary>
		/// India Rupees
		/// </summary>
		public static readonly Currency Inr;
		/// <summary>
		/// Japan Yen
		/// </summary>
		public static readonly Currency Jpy;
		/// <summary>
		/// Mexico Pesos
		/// </summary>
		public static readonly Currency Mxn;
		/// <summary>
		/// Malaysia Ringgits
		/// </summary>
		public static readonly Currency Myr;
		/// <summary>
		/// Norway Kroner
		/// </summary>
		public static readonly Currency Nok;
		/// <summary>
		/// New Zealand Dollars
		/// </summary>
		public static readonly Currency Nzd;
		/// <summary>
		/// Russia Rubles
		/// </summary>
		public static readonly Currency Rub;
		/// <summary>
		/// Sweden Kronor
		/// </summary>
		public static readonly Currency Sek;
		/// <summary>
		/// Singapore Dollars
		/// </summary>
		public static readonly Currency Sgd;
		/// <summary>
		/// Thailand Baht
		/// </summary>
		public static readonly Currency Thb;
		/// <summary>
		/// United States Dollars
		/// </summary>
		public static readonly Currency Usd;
		/// <summary>
		/// South Africa Rand
		/// </summary>
		public static readonly Currency Zar;

		/// <summary>
		/// Euro
		/// </summary>
		public static readonly Currency Euro;
		/// <summary>
		/// United States Dollars
		/// </summary>
		public static readonly Currency Dollar;
		/// <summary>
		/// United Kingdom Pounds
		/// </summary>
		public static readonly Currency Pound;
		/// <summary>
		/// Non-Existing currency
		/// </summary>
		public static readonly Currency Xxx;
		/// <summary>
		/// Testing currency
		/// </summary>
		public static readonly Currency Xts;
		/// <summary>
		/// Non-Existing currency
		/// </summary>
		public static readonly Currency None;
		/// <summary>
		/// Testing currency
		/// </summary>
		public static readonly Currency Test;

		/*private static readonly ThreadSafeCache<string, Currency> _byIsoSymbol;
		private static readonly ThreadSafeCache<CurrencyIsoCode, Currency> _byIsoCode;*/
		private static readonly CurrencyCache _cache;

		private static readonly ICurrencyInfoProvider _provider;

		/// <summary>
		/// Initialized static shortcuts and caches
		/// </summary>
		static Currency()
		{
			_cache = new CurrencyCache();
			_provider = CurrencyInfo.CreateProvider();

			using (var initializer = CurrencyInfo.CreateInitializer())
			{
				Aud = init(CurrencyIsoCode.AUD, initializer.Get);
				_cache.Add(Aud);

				Cad = init(CurrencyIsoCode.CAD, initializer.Get);
				_cache.Add(Cad);

				Chf = init(CurrencyIsoCode.CHF, initializer.Get);
				_cache.Add(Chf);

				Cny = init(CurrencyIsoCode.CNY, initializer.Get);
				_cache.Add(Cny);

				Dkk = init(CurrencyIsoCode.DKK, initializer.Get);
				_cache.Add(Dkk);

				Eur = init(CurrencyIsoCode.EUR, initializer.Get);
				_cache.Add(Eur);

				Gbp = init(CurrencyIsoCode.GBP, initializer.Get);
				_cache.Add(Gbp);

				Hkd = init(CurrencyIsoCode.HKD, initializer.Get);
				_cache.Add(Hkd);

				Huf = init(CurrencyIsoCode.HUF, initializer.Get);
				_cache.Add(Huf);

				Inr = init(CurrencyIsoCode.INR, initializer.Get);
				_cache.Add(Inr);

				Jpy = init(CurrencyIsoCode.JPY, initializer.Get);
				_cache.Add(Jpy);

				Mxn = init(CurrencyIsoCode.MXN, initializer.Get);
				_cache.Add(Mxn);

				Myr = init(CurrencyIsoCode.MYR, initializer.Get);
				_cache.Add(Myr);

				Nok = init(CurrencyIsoCode.NOK, initializer.Get);
				_cache.Add(Nok);

				Nzd = init(CurrencyIsoCode.NZD, initializer.Get);
				_cache.Add(Nzd);

				Rub = init(CurrencyIsoCode.RUB, initializer.Get);
				_cache.Add(Rub);

				Sek = init(CurrencyIsoCode.SEK, initializer.Get);
				_cache.Add(Sek);

				Sgd = init(CurrencyIsoCode.SGD, initializer.Get);
				_cache.Add(Sgd);

				Thb = init(CurrencyIsoCode.THB, initializer.Get);
				_cache.Add(Thb);

				Usd = init(CurrencyIsoCode.USD, initializer.Get);
				_cache.Add(Usd);

				Zar = init(CurrencyIsoCode.ZAR, initializer.Get);
				_cache.Add(Zar);

				Xxx = init(CurrencyIsoCode.XXX, initializer.Get);
				_cache.Add(Xxx);

				Xts = init(CurrencyIsoCode.XTS, initializer.Get);
				_cache.Add(Xts);
			}

			Euro = Eur;
			Dollar = Usd;
			Pound = Gbp;
			None = Xxx;
			Test = Xts;
		}

		/// <summary>
		/// Stores the currency in both symbol and code caches
		/// </summary>
		/*private static void fillCaches(Currency currency)
		{
			_byIsoCode.Add(currency.IsoCode, currency);
			_byIsoSymbol.Add(currency.IsoSymbol, currency);
		}*/

		private static Currency init(CurrencyIsoCode isoCode, Func<CurrencyIsoCode, CurrencyInfo> infoReading)
		{
			return new Currency(infoReading(isoCode));
		}

		/// <summary>
		/// Actively initializes the information for all currencies.
		/// </summary>
		/// <remarks>Use this method if you plan to use a lot of currencies in your program.
		/// <para>When most of currencies are expected to be used, it is recommeneded to initialize the information for all of them,
		/// saving time each time the first instance is accessed.</para></remarks>
		public static void InitializeAllCurrencies()
		{
			CurrencyIsoCode[] isoCodes = Enumeration.GetValues<CurrencyIsoCode>();
			using (var initializer = CurrencyInfo.CreateInitializer())
			{
				for (int i = 0; i < isoCodes.Length; i++)
				{
					CurrencyIsoCode isoCode = isoCodes[i];
					var copy = initializer;
					_cache.GetOrAdd(isoCode, () => init(isoCode, copy.Get));
				}
			}
		}

		#endregion

		///<summary>
		/// Occurs when an obsolete currency is created.
		///</summary>
		/// <remarks>
		/// Do remember to unsubscribe from this event when you are no longer insterested it its ocurrence.
		/// Failing to do so can prevent your objects from being garbage collected and result in a memory leak.
		/// <para>By its static nature, the notification is available even when no instance of the class is existing yet.
		/// This very same nature will cause that subscribers are notified for ocurrences that are "far" from the code that is likely to raise an event in concurrent systems.
		/// For example, another thread could make the event to raise and a totally unrelated code will get the notification. This may well be the desired effect,
		/// but awareness need to be raised for when it is not the desired effect.</para>
		/// <para>Currencies are transient entities in the real world, getting deprecated and/or substituted.
		/// When a currency that is no longer current is created this event will be raised. This can happen in a number of cases:
		/// <list type="bullet">
		/// <item><description>A <see cref="Currency"/> factory method is used.</description></item>
		/// <item><description>A <see cref="Currency"/> instance gets deserialized.</description></item>
		/// <item><description>A <see cref="Money"/> instance gets created.</description></item>
		/// <item><description>A <see cref="Money"/> instance gets deserialized.</description></item>
		/// </list>
		/// </para>
		/// </remarks>
		/// <seealso cref="Get(CurrencyIsoCode)"/>
		/// <seealso cref="Get(string)"/>
		/// <seealso cref="Get(CultureInfo)"/>
		/// <seealso cref="Get(RegionInfo)"/>
		/// <seealso cref="TryGet(CurrencyIsoCode, out Currency)"/>
		/// <seealso cref="TryGet(string, out Currency)"/>
		/// <seealso cref="TryGet(CultureInfo, out Currency)"/>
		/// <seealso cref="TryGet(RegionInfo, out Currency)"/>
		public static event EventHandler<ObsoleteCurrencyEventArgs> ObsoleteCurrency;

		internal static void RaiseIfObsolete(CurrencyIsoCode code)
		{
			if (ObsoleteCurrencies.IsObsolete(code))
			{
				EventHandler<ObsoleteCurrencyEventArgs> handler = ObsoleteCurrency;
				if (handler != null) handler(null, new ObsoleteCurrencyEventArgs(code));
			}
		}

		internal static void RaiseIfObsolete(Currency currency)
		{
			if (ObsoleteCurrencies.IsObsolete(currency))
			{
				EventHandler<ObsoleteCurrencyEventArgs> handler = ObsoleteCurrency;
				if (handler != null) handler(null, new ObsoleteCurrencyEventArgs(currency.IsoCode));
			}
		}

		internal decimal Round(decimal share)
		{
			decimal raw = share - (.5m * MinAmount);
			decimal rounded = decimal.Round(raw, SignificantDecimalDigits, MidpointRounding.AwayFromZero);
			return rounded;
		}
	}
}
