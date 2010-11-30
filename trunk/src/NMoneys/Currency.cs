using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using NMoneys.Support;
using NMoneys.Support.Ext;

namespace NMoneys
{
	///<summary>
	/// Represents a currency unit such as Euro or American Dollar.
	///</summary>
	[Serializable]
	[XmlSchemaProvider("GetSchema")]
	[XmlRoot(Namespace = Serialization.Data.NAMESPACE, ElementName = Serialization.Data.Currency.ROOT_NAME, DataType = Serialization.Data.Currency.DATA_TYPE, IsNullable = false)]
	public sealed class Currency : IFormatProvider, IEquatable<Currency>, ISerializable, IXmlSerializable, IObjectReference,IComparable, IComparable<Currency>
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
		[XmlIgnore]
		public string IsoSymbol { get; private set; }

		/// <summary>
		/// Indicates the number of decimal places to use in currency values.
		/// </summary>
		[XmlIgnore]
		public int SignificantDecimalDigits { get; private set; }

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
		/// Gets the default currency symbol.
		/// </summary>
		public static readonly string DefaultSymbol = CultureInfo.InvariantCulture.NumberFormat.CurrencySymbol;

		#endregion

		#region Ctor

		[Obsolete("serialization")]
		private Currency() { }

		private Currency(CurrencyIsoCode isoCode, string englishName, string nativeName, string symbol, int significantDecimalDigits, string decimalSeparator, string groupSeparator, int[] groupSizes, int positivePattern, int negativePattern)
		{
			setAllFields(isoCode, englishName, nativeName, symbol, significantDecimalDigits, decimalSeparator, groupSeparator, groupSizes, positivePattern, negativePattern);
		}

		internal Currency(CurrencyInfo info)
			: this(info.Code, info.EnglishName, info.NativeName,
			info.Symbol, info.SignificantDecimalDigits, info.DecimalSeparator,
			info.GroupSeparator, info.GroupSizes, info.PositivePattern, info.NegativePattern) { }

		/// <summary>
		/// Initializes a new instace of <see cref="Currency"/> with serialized data
		/// </summary>
		/// <remarks>Only the iso code is serialized, the rest of the state is retrieved from <see cref="Currency"/> obtained by the 
		/// <see cref="Get(NMoneys.CurrencyIsoCode)"/> creation method.</remarks>
		/// <param name="info">The <see cref="SerializationInfo"/> that holds the serialized object data about the <see cref="Currency"/>.</param>
		/// <param name="context">The <see cref="StreamingContext"/> that contains contextual information about the source or destination.</param>
		private Currency(SerializationInfo info, StreamingContext context)
		{
			CurrencyIsoCode isoCode = Enumeration.Parse<CurrencyIsoCode>((string)info.GetValue(Serialization.Data.Currency.ISO_CODE, typeof(string)));//(CurrencyIsoCode)info.GetValue(Serialization.Currency.ISO_CODE, typeof(CurrencyIsoCode));
			Currency paradigm = Get(isoCode);
			setAllFields(paradigm.IsoCode,
				paradigm.EnglishName,
				paradigm.NativeName,
				paradigm.Symbol,
				paradigm.SignificantDecimalDigits,
				paradigm.DecimalSeparator,
				paradigm.GroupSeparator,
				paradigm.GroupSizes,
				paradigm.PositivePattern,
				paradigm.NegativePattern);
		}

		/// <summary>
		/// Allows setting all field both for constructors and serialization methods.
		/// </summary>
		private void setAllFields(CurrencyIsoCode isoCode, string englishName, string nativeName, string symbol, int significantDecimalDigits, string decimalSeparator, string groupSeparator, int[] groupSizes, int positivePattern, int negativePattern)
		{
			IsoCode = isoCode;
			EnglishName = englishName;
			Symbol = symbol;
			IsoSymbol = isoCode.ToString();
			SignificantDecimalDigits = significantDecimalDigits;
			NativeName = nativeName;
			DecimalSeparator = decimalSeparator;
			GroupSeparator = groupSeparator;
			GroupSizes = groupSizes;
			PositivePattern = positivePattern;
			NegativePattern = negativePattern;

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

		private static readonly ThreadSafeCache<string, Currency> _byIsoSymbol;
		private static readonly ThreadSafeCache<CurrencyIsoCode, Currency> _byIsoCode;

		private static readonly ICurrencyInfoProvider _provider;

		/// <summary>
		/// Initialized static shortcuts and caches
		/// </summary>
		static Currency()
		{
			int cacheCapacity = Enum.GetNames(typeof(CurrencyIsoCode)).Length;
			_byIsoSymbol = new ThreadSafeCache<string, Currency>(cacheCapacity, StringComparer.OrdinalIgnoreCase);
			// we use a fast comparer as we have quite a few enum keys
			_byIsoCode = new ThreadSafeCache<CurrencyIsoCode, Currency>(cacheCapacity, Enumeration.Comparer<CurrencyIsoCode>());
			_provider = CurrencyInfo.CreateProvider();

			using (var initializer = CurrencyInfo.CreateInitializer())
			{
				Aud = init(CurrencyIsoCode.AUD, initializer.Get);
				fillCaches(Aud);

				Cad = init(CurrencyIsoCode.CAD, initializer.Get);
				fillCaches(Cad);

				Chf = init(CurrencyIsoCode.CHF, initializer.Get);
				fillCaches(Chf);

				Cny = init(CurrencyIsoCode.CNY, initializer.Get);
				fillCaches(Cny);

				Dkk = init(CurrencyIsoCode.DKK, initializer.Get);
				fillCaches(Dkk);

				Eur = init(CurrencyIsoCode.EUR, initializer.Get);
				fillCaches(Eur);

				Gbp = init(CurrencyIsoCode.GBP, initializer.Get);
				fillCaches(Gbp);

				Hkd = init(CurrencyIsoCode.HKD, initializer.Get);
				fillCaches(Hkd);

				Huf = init(CurrencyIsoCode.HUF, initializer.Get);
				fillCaches(Huf);

				Inr = init(CurrencyIsoCode.INR, initializer.Get);
				fillCaches(Inr);

				Jpy = init(CurrencyIsoCode.JPY, initializer.Get);
				fillCaches(Jpy);

				Mxn = init(CurrencyIsoCode.MXN, initializer.Get);
				fillCaches(Mxn);

				Myr = init(CurrencyIsoCode.MYR, initializer.Get);
				fillCaches(Myr);

				Nok = init(CurrencyIsoCode.NOK, initializer.Get);
				fillCaches(Nok);

				Nzd = init(CurrencyIsoCode.NZD, initializer.Get);
				fillCaches(Nzd);

				Rub = init(CurrencyIsoCode.RUB, initializer.Get);
				fillCaches(Rub);

				Sek = init(CurrencyIsoCode.SEK, initializer.Get);
				fillCaches(Sek);

				Sgd = init(CurrencyIsoCode.SGD, initializer.Get);
				fillCaches(Sgd);

				Thb = init(CurrencyIsoCode.THB, initializer.Get);
				fillCaches(Thb);

				Usd = init(CurrencyIsoCode.USD, initializer.Get);
				fillCaches(Usd);

				Zar = init(CurrencyIsoCode.ZAR, initializer.Get);
				fillCaches(Zar);

				Xxx = init(CurrencyIsoCode.XXX, initializer.Get);
				fillCaches(Xxx);

				Xts = init(CurrencyIsoCode.XTS, initializer.Get);
				fillCaches(Xts);
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
		private static void fillCaches(Currency currency)
		{
			Guard.AgainstNullArgument("currency", currency);

			_byIsoCode.Add(currency.IsoCode, currency);
			_byIsoSymbol.Add(currency.IsoSymbol, currency);
		}

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
					if (!_byIsoCode.Contains(isoCode))
					{
						Currency initialized = new Currency(initializer.Get(isoCode));
						fillCaches(initialized);
					}
				}
			}
		}

		#endregion

		#region equality

		/// <summary>
		/// Indicates whether the current <see cref="Currency"/> instance is equal to another instance.
		/// </summary>
		/// <returns>
		/// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
		/// </returns>
		/// <param name="other">A <see cref="Currency"/> to compare with this object.</param>
		public bool Equals(Currency other)
		{
			if (ReferenceEquals(null, other)) return false;
			if (ReferenceEquals(this, other)) return true;
			// only IsoCode matters as it cannot be mutated
			return Equals(other.IsoCode, IsoCode);
		}

		/// <summary>
		/// Determines whether the specified <see cref="object"/> is equal to the current <see cref="Currency"/>.
		/// </summary>
		/// <returns>
		/// true if the specified <see cref="object"/> is equal to the current <see cref="Currency"/>; otherwise, false.
		/// </returns>
		/// <param name="obj">The <see cref="object"/> to compare with the current <see cref="Currency"/>.</param> 
		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != typeof(Currency)) return false;
			return Equals((Currency)obj);
		}

		/// <summary>
		/// Serves as a hash function for a particular type. 
		/// </summary>
		/// <returns>
		/// A hash code for the current <see cref="Currency"/>.
		/// </returns>
		public override int GetHashCode()
		{
			return IsoCode.GetHashCode();
		}

		///<summary>
		/// Determines whether two specified currencies are equal.
		///</summary>
		///<param name="left">The first <see cref="Currency"/> to compare, or null.</param>
		///<param name="right">The second <see cref="Currency"/> to compare, or null.</param>
		///<returns>true if <paramref name="left"/> is equal to <paramref name="right"/>; otherwise, false.</returns>
		public static bool operator ==(Currency left, Currency right)
		{
			return Equals(left, right);
		}

		///<summary>
		/// Determines whether two specified currencies are not equal.
		///</summary>
		///<param name="left">The first <see cref="Currency"/> to compare, or null.</param>
		///<param name="right">The second <see cref="Currency"/> to compare, or null.</param>
		///<returns>true if <paramref name="left"/> is not equal to <paramref name="right"/>; otherwise, false.</returns>
		public static bool operator !=(Currency left, Currency right)
		{
			return !Equals(left, right);
		}

		#endregion

		#region comparison

		/// <summary>
		/// Compares the current instance with another object of the same type and returns an integer that indicates whether the current instance precedes,
		/// follows, or occurs in the same position in the sort order as the other object.
		/// </summary>
		/// <param name="obj">An object to compare with this instance. </param>
		/// <returns>
		/// A 32-bit signed integer that indicates the relative order of the objects being compared. The return value has these meanings:
		/// <list type="table">
		/// <listheader>
		/// <term>Value</term>
		/// <description>Meaning</description>
		/// </listheader>
		/// <item>
		/// <term>Less than zero</term>
		/// <description>This instance is less than <paramref name="obj"/>.</description>
		/// </item>
		/// <item>
		/// <term>Zero</term>
		/// <description>This instance is equal to <paramref name="obj"/>.</description>
		/// </item>
		/// <item>
		/// <term>Greater than zero</term>
		/// <description>This instance is greater than <paramref name="obj"/>.</description>
		/// </item>
		/// </list>
		/// </returns>
		/// <exception cref="T:System.ArgumentException"><paramref name="obj"/> is not a <see cref="Currency"/>.</exception>
		public int CompareTo(object obj)
		{
			if (obj == null)
			{
				return 1;
			}
			if (!(obj is Currency))
			{
				throw new ArgumentException("obj", string.Format("Argument must be of type {0}.", typeof(Currency).Name));
			}
			return CompareTo((Currency)obj);
		}

		/// <summary>
		/// Performs a textual comparison of the Iso symbol
		/// </summary>
		/// <param name="other">An object to compare with this instance.</param>
		/// <returns>
		/// A 32-bit signed integer that indicates the relative order of the objects being compared. The return value has these meanings:
		/// <list type="table">
		/// <listheader>
		/// <term>Value</term>
		/// <description>Meaning</description>
		/// </listheader>
		/// <item>
		/// <term>Less than zero</term>
		/// <description>This instance is less than <paramref name="other"/>.</description>
		/// </item>
		/// <item>
		/// <term>Zero</term>
		/// <description>This instance is equal to <paramref name="other"/>.</description>
		/// </item>
		/// <item>
		/// <term>Greater than zero</term>
		/// <description>This instance is greater than <paramref name="other"/>.</description>
		/// </item>
		/// </list>
		/// </returns>
		public int CompareTo(Currency other)
		{
			if (other == null) return 1;
			return string.Compare(IsoSymbol, other.IsoSymbol, StringComparison.Ordinal);
		}

		#endregion

		/// <summary>
		/// Returns an object that provides formatting services for the specified type.
		/// </summary>
		/// <returns>
		/// An instance of the object specified by <paramref name="formatType"/>,
		/// if the <see cref="IFormatProvider"/> implementation is <see cref="NumberFormatInfo"/>; otherwise, null.
		/// </returns>
		/// <param name="formatType">An object that specifies the type of format object to return.</param>
		public object GetFormat(Type formatType)
		{
			return formatType == typeof(NumberFormatInfo) ? FormatInfo : null;
		}

		#region factory methods

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
		/// <exception cref="MissconfiguredCurrencyException">The currency represented by <paramref name="isoCode"/> has not been properly configured by the library implementor. Please, log a issue.</exception>
		public static Currency Get(CurrencyIsoCode isoCode)
		{
			Enumeration.AssertDefined(isoCode);

			Currency currency;
			if (!_byIsoCode.TryGet(isoCode, out currency))
			{
				currency = init(isoCode, _provider.Get);
				if (currency == null) throw new MissconfiguredCurrencyException(isoCode);
				fillCaches(currency);
			}

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
		/// <exception cref="MissconfiguredCurrencyException">The currency represented by <paramref name="threeLetterIsoCode"/> has not been properly configured by the library implementor. Please, log a issue.</exception>
		public static Currency Get(string threeLetterIsoCode)
		{
			Currency currency;
			if (!_byIsoSymbol.TryGet(threeLetterIsoCode, out currency))
			{
				var isoCode = Enumeration.Parse<CurrencyIsoCode>(threeLetterIsoCode);
				currency = init(isoCode, _provider.Get);
				if (currency == null) throw new MissconfiguredCurrencyException(isoCode);
				fillCaches(currency);
			}
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
		/// <exception cref="ArgumentNullException">The <paramref name="culture"/> is null.</exception>
		/// <exception cref="ArgumentException">The <paramref name="culture"/> is either an invariant, custom or neutral culture, or a <see cref="RegionInfo"/> cannot be instantiated from it.</exception>
		/// <exception cref="System.ComponentModel.InvalidEnumArgumentException">The ISO symbol associated to the <paramref name="culture"/> does not exist in the <see cref="CurrencyIsoCode"/> enumeration.</exception>
		/// <exception cref="MissconfiguredCurrencyException">The currency associated to the <paramref name="culture"/> has not been properly configured by the library implementor. Please, log a issue.</exception>
		public static Currency Get(CultureInfo culture)
		{
			Guard.AgainstNullArgument("culture", culture); 
			RegionInfo region = new RegionInfo(culture.LCID);
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

			if (Enum.IsDefined(typeof(CurrencyIsoCode), isoCode))
			{
				tryGet = _byIsoCode.TryGet(isoCode, out currency);
				if (!tryGet)
				{
					currency = init(isoCode, _provider.Get);
					if (currency != null)
					{
						tryGet = true;
						fillCaches(currency);
					}
				}
			}
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
			currency = null;
			bool tryGet = threeLetterIsoSymbol == null ? false : _byIsoSymbol.TryGet(threeLetterIsoSymbol, out currency);

			if (!tryGet)
			{
				CurrencyIsoCode? isoCode;
				if (Enumeration.TryParse(threeLetterIsoSymbol, out isoCode))
				{
					currency = init(isoCode.Value, _provider.Get);
					if (currency != null)
					{
						tryGet = true;
						fillCaches(currency);
					}
				}
			}
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
		/// the given <paramref name="culture"/>. In this case it might be possible that the lookup is not successfull even if the region
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

			if (!culture.IsNeutralCulture && !culture.Equals(CultureInfo.InvariantCulture))
			{
				RegionInfo region = new RegionInfo(culture.LCID);
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
					Currency maybeInCache;
					if (!_byIsoCode.TryGet(isoCode, out maybeInCache))
					{
						maybeInCache = new Currency(initializer.Get(isoCode));
						fillCaches(maybeInCache);
					}
					yield return maybeInCache;
				}
			}
		}

		#endregion

		/// <summary>
		/// Returns a <see cref="string"/> that represents the current <see cref="Currency"/>.
		/// </summary>
		/// <remarks>It actually is a representation of the <see cref="IsoCode"/>.</remarks>
		/// <returns>
		/// A <see cref="string"/> that represents the current <see cref="Currency"/>.
		/// </returns>
		public override string ToString()
		{
			return IsoCode.ToString();
		}

		#region serialization

		/// <summary>
		/// Sets the <see cref="SerializationInfo"/> with information about the currency. 
		/// </summary>
		/// <remarks>It will only persist information regarding the <see cref="IsoCode"/>.
		/// <para>The rest of the information will be populated from the instance obtained from creation methods.</para>
		/// </remarks>
		/// <param name="info">The <see cref="SerializationInfo"/> that holds the serialized object data about the <see cref="Currency"/>.</param>
		/// <param name="context">The <see cref="StreamingContext"/> that contains contextual information about the source or destination.</param>
		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue(Serialization.Data.Currency.ISO_CODE, IsoSymbol);
		}

		/// <summary>
		/// This method is reserved and should not be used.
		/// When implementing the <see cref="IXmlSerializable"/> interface, you should return null from this method, and instead,
		/// if specifying a custom schema is required, apply the <see cref="XmlSchemaProviderAttribute"/> to the class.
		/// </summary>
		/// <returns>null</returns>
		[Obsolete("deprecated, use SchemaProviders instead")]
		XmlSchema IXmlSerializable.GetSchema()
		{
			return null;
		}

		/// <summary>
		/// Returns the XML schema applied for serialization.
		/// </summary>
		/// <param name="xs">A cache of XML Schema definition language (XSD) schemas.</param>
		/// <returns>Represents the complexType element from XML Schema as specified by the <paramref name="xs"/>.</returns>
		public static XmlSchemaComplexType GetSchema(XmlSchemaSet xs)
		{
			XmlSchemaComplexType complex = null;
			XmlSerializer schemaSerializer = new XmlSerializer(typeof(XmlSchema));
			using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(Serialization.Data.ResourceName))
			{
				if (stream != null)
				{
					try
					{
						XmlSchema schema = (XmlSchema)schemaSerializer.Deserialize(new XmlTextReader(stream));
						xs.Add(schema);
						XmlQualifiedName name = new XmlQualifiedName(Serialization.Data.Currency.DATA_TYPE, Serialization.Data.NAMESPACE);
						complex = (XmlSchemaComplexType)schema.SchemaTypes[name];
					}
					finally
					{
						stream.Close();
					}
				}
			}
			return complex;
		}

		/// <summary>
		/// Generates an object from its XML representation.
		/// </summary>
		/// <param name="reader">The <see cref="XmlReader"/> stream from which the object is deserialized.</param>
		public void ReadXml(XmlReader reader)
		{
			var isoCode = ReadXmlData(reader);

			Currency paradigm = Get(isoCode);
			setAllFields(paradigm.IsoCode,
			             paradigm.EnglishName,
			             paradigm.NativeName,
			             paradigm.Symbol,
			             paradigm.SignificantDecimalDigits,
			             paradigm.DecimalSeparator,
			             paradigm.GroupSeparator,
			             paradigm.GroupSizes,
			             paradigm.PositivePattern,
			             paradigm.NegativePattern);
		}

		/// <summary>
		/// Converts an object into its XML representation.
		/// </summary>
		/// <param name="writer">The <see cref="XmlWriter"/> stream to which the object is serialized.</param>
		public void WriteXml(XmlWriter writer)
		{
			writer.WriteElementString(Serialization.Data.Currency.ISO_CODE, Serialization.Data.NAMESPACE, IsoSymbol);
		}

		/// <summary>
		/// Returns the real object that should be deserialized, rather than the object that the serialized stream specifies.
		/// </summary>
		/// <returns>
		/// Returns the actual object that is put into the graph.
		/// </returns>
		/// <param name="context">The <see cref="StreamingContext"/> from which the current object is deserialized.</param>
		/// <exception cref="System.Security.SecurityException">The caller does not have the required permission. The call will not work on a medium trusted server.</exception>
		public object GetRealObject(StreamingContext context)
		{
			return Get(IsoCode);
		}

		internal static CurrencyIsoCode ReadXmlData(XmlReader reader)
		{
			reader.ReadStartElement();
			CurrencyIsoCode isoCode = Enumeration.Parse<CurrencyIsoCode>(reader.ReadElementContentAsString(Serialization.Data.Currency.ISO_CODE, Serialization.Data.NAMESPACE));
			reader.ReadEndElement();
			return isoCode;
		}

		#endregion
	}
}
