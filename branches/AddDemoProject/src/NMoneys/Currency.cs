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
	[Serializable]
	[XmlSchemaProvider("GetSchema")]
	[XmlRoot(Namespace = Serialization.Data.NAMESPACE, ElementName = Serialization.Data.Currency.ROOT_NAME, DataType = Serialization.Data.Currency.DATA_TYPE, IsNullable = false)]
	public sealed class Currency : IFormatProvider, IEquatable<Currency>, ISerializable, IXmlSerializable, IObjectReference
	{
		#region properties

		/// <summary>
		/// The ISO 4217 code of the <see cref="Currency"/>
		/// </summary>
		public CurrencyIsoCode IsoCode { get; private set; }

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
		/// <remarks>For more information about this pattern see <seealso cref="NumberFormatInfo.CurrencyNegativePattern"/>.</remarks>
		[XmlIgnore]
		public int NegativePattern { get; private set; }

		/// <summary>
		/// Gets the format pattern for positive currency values. 
		/// </summary>
		/// <remarks>For more information about this pattern see <seealso cref="NumberFormatInfo.currencyPositivePattern"/>.</remarks>
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

		public bool Equals(Currency other)
		{
			if (ReferenceEquals(null, other)) return false;
			if (ReferenceEquals(this, other)) return true;
			// only IsoCode matters as it cannot be mutated
			return Equals(other.IsoCode, IsoCode);
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != typeof(Currency)) return false;
			return Equals((Currency)obj);
		}

		public override int GetHashCode()
		{
			return IsoCode.GetHashCode();
		}

		public static bool operator ==(Currency left, Currency right)
		{
			return Equals(left, right);
		}

		public static bool operator !=(Currency left, Currency right)
		{
			return !Equals(left, right);
		}

		public object GetFormat(Type formatType)
		{
			return formatType == typeof(NumberFormatInfo) ? FormatInfo : null;
		}

		#region factory methods

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

		public static Currency Get(CultureInfo culture)
		{
			RegionInfo region = new RegionInfo(culture.LCID);
			return Get(region.ISOCurrencySymbol);
		}

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

		public static bool TryGet(string threeLetterIsoSymbol, out Currency currency)
		{
			bool tryGet = _byIsoSymbol.TryGet(threeLetterIsoSymbol, out currency);

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

		public static IEnumerable<Currency> FindAll()
		{
			CurrencyIsoCode[] isoCodes = Enumeration.GetValues<CurrencyIsoCode>();
			using (var initializer = CurrencyInfo.CreateInitializer())
			{
				for (int i = 0; i < isoCodes.Length; i++)
				{
					CurrencyIsoCode isoCode = isoCodes[i];
					Currency maybe;
					if (!_byIsoCode.TryGet(isoCode, out maybe))
					{
						maybe = new Currency(initializer.Get(isoCode));
						fillCaches(maybe);
					}
					yield return maybe;
				}
			}
		}

		#endregion

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

		[Obsolete("deprecated, use SchemaProviders instead")]
		XmlSchema IXmlSerializable.GetSchema()
		{
			return null;
		}

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

		public void WriteXml(XmlWriter writer)
		{
			writer.WriteElementString(Serialization.Data.Currency.ISO_CODE, Serialization.Data.NAMESPACE, IsoSymbol);
		}

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
