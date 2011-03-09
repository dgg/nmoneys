using System;
using System.ComponentModel;
using System.Globalization;

namespace NMoneys
{
	/// <summary>
	/// Currency codes as stated by the ISO 4217 standard 
	/// </summary>
	/// <seealso href="http://www.iso.org/iso/support/currency_codes_list-1.htm" />
	public enum CurrencyIsoCode : short
	{
		///<summary>
		/// UAE Dirham
		///</summary>
		[CanonicalCulture("ar-AE")]
		AED = 784,
		/// <summary>
		/// Afghani
		/// </summary>
		[CanonicalCulture("ps-AF")]
		AFN = 971,
		/// <summary>
		/// Albanian Lek
		/// </summary>
		[CanonicalCulture("sq-AL")]
		ALL = 008,
		/// <summary>
		/// Armenian Dram
		/// </summary>
		[CanonicalCulture("hy-AM")]
		AMD = 051,
		/// <summary>
		/// Netherlands Antillian Guilder
		/// </summary>
		ANG = 532,
		/// <summary>
		/// Kwanza
		/// </summary>
		AOA = 973,
		/// <summary>
		/// Argentine Peso
		/// </summary>
		[CanonicalCulture("es-AR")]
		ARS = 032,
		/// <summary>
		/// Australian Dollar
		/// </summary>
		[CanonicalCulture("en-AU")]
		AUD = 036,
		/// <summary>
		/// Aruban Guilder
		/// </summary>
		AWG = 533,
		/// <summary>
		/// Azerbaijanian Manat
		/// </summary>
		[CanonicalCulture("az-Latn-AZ")]
		AZN = 944,
		/// <summary>
		/// Convertible Marks
		/// </summary>
		[CanonicalCulture("bs-Latn-BA")]
		BAM = 977,
		/// <summary>
		/// Barbados Dollar
		/// </summary>
		BBD = 052,
		/// <summary>
		/// Bangladeshi Taka
		/// </summary>
		[CanonicalCulture("bn-BD")]
		BDT = 050,
		/// <summary>
		/// Bulgarian Lev
		/// </summary>
		[CanonicalCulture("bg-BG", Overwritten = true)]
		BGN = 975,
		/// <summary>
		/// Bahraini Dinar
		/// </summary>
		[CanonicalCulture("ar-BH")]
		BHD = 048,
		/// <summary>
		/// Burundi Franc
		/// </summary>
		BIF = 108,
		/// <summary>
		/// Bermudian Dollar
		/// </summary>
		BMD = 060,
		/// <summary>
		/// Brunei Dollar
		/// </summary>
		[CanonicalCulture("ms-BN")]
		BND = 096,
		/// <summary>
		/// Boliviano
		/// </summary>
		[CanonicalCulture("es-BO")]
		BOB = 068,
		/// <summary>
		/// Boliviano
		/// </summary>
		[CanonicalCulture("es-BO")]
		BOV = 984,
		/// <summary>
		/// Real
		/// </summary>
		[CanonicalCulture("pt-BR")]
		BRL = 986,
		/// <summary>
		/// Bahamian Dollar
		/// </summary>
		BSD = 044,
		/// <summary>
		/// Ngultrum
		/// </summary>
		BTN = 064,
		/// <summary>
		/// Pula
		/// </summary>
		BWP = 072,
		/// <summary>
		/// Belarusian Ruble
		/// </summary>
		[CanonicalCulture("be-BY")]
		BYR = 974,
		/// <summary>
		/// Belize Dollar
		/// </summary>
		[CanonicalCulture("en-BZ")]
		BZD = 084,
		/// <summary>
		/// Canadian Dollar
		/// </summary>
		[CanonicalCulture("en-CA")]
		CAD = 124,
		/// <summary>
		/// Congolese Franc
		/// </summary>
		CDF = 976,
		/// <summary>
		/// WIR Euro
		/// </summary>
		[CanonicalCulture("de-CH")]
		CHE = 947,
		/// <summary>
		/// Swiss Franc
		/// </summary>
		[CanonicalCulture("de-CH", Overwritten = true)]
		CHF = 756,
		/// <summary>
		/// WIR Franc
		/// </summary>
		[CanonicalCulture("de-CH")]
		CHW = 948,
		/// <summary>
		/// Chilean Peso
		/// </summary>
		[CanonicalCulture("es-CL")]
		CLF = 990,
		/// <summary>
		/// Chilean Peso
		/// </summary>
		[CanonicalCulture("es-CL")]
		CLP = 152,
		/// <summary>
		/// Yuan Renminbi
		/// </summary>
		[CanonicalCulture("zh-CN")]
		CNY = 156,
		/// <summary>
		/// Colombian Peso
		/// </summary>
		[CanonicalCulture("es-CO")]
		COP = 170,
		/// <summary>
		/// Colombian Peso
		/// </summary>
		[CanonicalCulture("es-CO")]
		COU = 970,
		/// <summary>
		/// Costa Rican Colon
		/// </summary>
		[CanonicalCulture("es-CR")]
		CRC = 188,
		/// <summary>
		/// Peso Convertible
		/// </summary>
		CUC = 931,
		/// <summary>
		/// Cuban Peso
		/// </summary>
		CUP = 192,
		/// <summary>
		/// Cape Verde Escudo
		/// </summary>
		CVE = 132,
		/// <summary>
		/// Czech Koruna
		/// </summary>
		[CanonicalCulture("cs-CZ")]
		CZK = 203,
		/// <summary>
		/// Djibouti Franc
		/// </summary>
		DJF = 262,
		/// <summary>
		/// Danish Krone
		/// </summary>
		[CanonicalCulture("da-DK", Overwritten = true)]
		DKK = 208,
		/// <summary>
		/// Dominican Peso
		/// </summary>
		[CanonicalCulture("es-DO")]
		DOP = 214,
		/// <summary>
		/// Algerian Dinar
		/// </summary>
		[CanonicalCulture("ar-DZ")]
		DZD = 012,
		/// <summary>
		/// Estonian Kroon
		/// </summary>
		[CanonicalCulture("et-EE"), Obsolete("deprecated")]
		EEK = 233,
		/// <summary>
		/// Egyptian Pound
		/// </summary>
		[CanonicalCulture("ar-EG")]
		EGP = 818,
		/// <summary>
		/// Nakfa
		/// </summary>
		ERN = 232,
		/// <summary>
		/// Ethiopian Birr
		/// </summary>
		[CanonicalCulture("am-ET")]
		ETB = 230,
		/// <summary>
		/// Euro
		/// </summary>
		[CanonicalCulture("de-DE")]
		EUR = 978,
		/// <summary>
		/// Fiji Dollar
		/// </summary>
		FJD = 242,
		/// <summary>
		/// Falkland Islands Pound
		/// </summary>
		FKP = 238,
		/// <summary>
		/// UK Pound Sterling
		/// </summary>
		[CanonicalCulture("en-GB")]
		GBP = 826,
		/// <summary>
		/// Lari
		/// </summary>
		[CanonicalCulture("ka-GE")]
		GEL = 981,
		/// <summary>
		/// Cedi
		/// </summary>
		GHS = 936,
		/// <summary>
		/// Gibraltar Pound
		/// </summary>
		GIP = 292,
		/// <summary>
		/// Dalasi
		/// </summary>
		GMD = 270,
		/// <summary>
		/// Guinea Franc
		/// </summary>
		GNF = 324,
		/// <summary>
		/// Guatemalan Quetzal
		/// </summary>
		[CanonicalCulture("es-GT")]
		GTQ = 320,
		/// <summary>
		/// Guyana Dollar
		/// </summary>
		GYD = 328,
		/// <summary>
		/// Hong Kong Dollar
		/// </summary>
		[CanonicalCulture("zh-HK")]
		HKD = 344,
		/// <summary>
		/// Honduran Lempira
		/// </summary>
		[CanonicalCulture("es-HN")]
		HNL = 340,
		/// <summary>
		/// Croatian Kuna
		/// </summary>
		[CanonicalCulture("hr-HR")]
		HRK = 191,
		/// <summary>
		/// Gourde
		/// </summary>
		HTG = 332,
		/// <summary>
		/// Hungarian Forint
		/// </summary>
		[CanonicalCulture("hu-HU")]
		HUF = 348,
		/// <summary>
		/// Indonesian Rupiah
		/// </summary>
		[CanonicalCulture("id-ID")]
		IDR = 360,
		/// <summary>
		/// Israeli New Shekel
		/// </summary>
		[CanonicalCulture("he-IL")]
		ILS = 376,
		/// <summary>
		/// Indian Rupee
		/// </summary>
		[CanonicalCulture("hi-IN")]
		INR = 356,
		/// <summary>
		/// Iraqi Dinar
		/// </summary>
		[CanonicalCulture("ar-IQ")]
		IQD = 368,
		/// <summary>
		/// Iranian Rial
		/// </summary>
		[CanonicalCulture("fa-IR")]
		IRR = 364,
		/// <summary>
		/// Icelandic Krona
		/// </summary>
		[CanonicalCulture("is-IS")]
		ISK = 352,
		/// <summary>
		/// Jamaican Dollar
		/// </summary>
		[CanonicalCulture("en-JM")]
		JMD = 388,
		/// <summary>
		/// Jordanian Dinar
		/// </summary>
		[CanonicalCulture("ar-JO")]
		JOD = 400,
		/// <summary>
		/// Japanese Yen
		/// </summary>
		[CanonicalCulture("ja-JP")]
		JPY = 392,
		/// <summary>
		/// Kenyan Shilling
		/// </summary>
		[CanonicalCulture("sw-KE")]
		KES = 404,
		/// <summary>
		/// som
		/// </summary>
		[CanonicalCulture("ky-KG")]
		KGS = 417,
		/// <summary>
		/// Comoro Franc
		/// </summary>
		KMF = 174,
		/// <summary>
		/// Riel
		/// </summary>
		[CanonicalCulture("km-KH")]
		KHR = 116,
		/// <summary>
		/// North Korean Won
		/// </summary>
		KPW = 408,
		/// <summary>
		/// Korean Won
		/// </summary>
		[CanonicalCulture("ko-KR")]
		KRW = 410,
		/// <summary>
		/// Kuwaiti Dinar
		/// </summary>
		[CanonicalCulture("ar-KW")]
		KWD = 414,
		/// <summary>
		/// Cayman Islands Dollar
		/// </summary>
		KYD = 136,
		/// <summary>
		/// Tenge
		/// </summary>
		[CanonicalCulture("kk-KZ")]
		KZT = 398,
		/// <summary>
		/// Kip
		/// </summary>
		[CanonicalCulture("lo-LA")]
		LAK = 418,
		/// <summary>
		/// Lebanese Pound
		/// </summary>
		[CanonicalCulture("ar-LB")]
		LBP = 422,
		/// <summary>
		/// Sri Lanka Rupee
		/// </summary>
		[CanonicalCulture("si-LK")]
		LKR = 144,
		/// <summary>
		/// Liberian Dollar
		/// </summary>
		LRD = 430,
		/// <summary>
		/// Loti
		/// </summary>
		LSL = 426,
		/// <summary>
		/// Lithuanian Litas
		/// </summary>
		[CanonicalCulture("lt-LT")]
		LTL = 440,
		/// <summary>
		/// Latvian Lats
		/// </summary>
		[CanonicalCulture("lv-LV")]
		LVL = 428,
		/// <summary>
		/// Libyan Dinar
		/// </summary>
		[CanonicalCulture("ar-LY")]
		LYD = 434,
		/// <summary>
		/// Moroccan Dirham
		/// </summary>
		[CanonicalCulture("ar-MA")]
		MAD = 504,
		/// <summary>
		/// Moldovan Leu
		/// </summary>
		MDL = 498,
		/// <summary>
		/// Malagasy Ariary
		/// </summary>
		MGA = 969,
		/// <summary>
		/// Macedonian Denar
		/// </summary>
		[CanonicalCulture("mk-MK")]
		MKD = 807,
		/// <summary>
		/// Kyat
		/// </summary>
		MMK = 104,
		/// <summary>
		/// Tugrik
		/// </summary>
		[CanonicalCulture("mn-MN")]
		MNT = 496,
		/// <summary>
		/// Macao Pataca
		/// </summary>
		[CanonicalCulture("zh-MO")]
		MOP = 446,
		/// <summary>
		/// Ouguiya
		/// </summary>
		MRO = 478,
		/// <summary>
		/// Mauritius Rupee
		/// </summary>
		MUR = 480,
		/// <summary>
		/// Rufiyaa
		/// </summary>
		[CanonicalCulture("dv-MV")]
		MVR = 462,
		/// <summary>
		/// Kwacha
		/// </summary>
		MWK = 454,
		/// <summary>
		/// Mexican Peso
		/// </summary>
		[CanonicalCulture("es-MX")]
		MXN = 484,
		/// <summary>
		/// Mexican Peso
		/// </summary>
		[CanonicalCulture("es-MX")]
		MXV = 979,
		/// <summary>
		/// Malaysian Ringgit
		/// </summary>
		[CanonicalCulture("ms-MY", Overwritten = true)]
		MYR = 458,
		/// <summary>
		/// Metical
		/// </summary>
		MZN = 943,
		/// <summary>
		/// Namibian Dollar
		/// </summary>
		NAD = 516,
		/// <summary>
		/// Nigerian Naira
		/// </summary>
		[CanonicalCulture("ha-Latn-NG")]
		NGN = 566,
		/// <summary>
		/// Nicaraguan Cordoba Oro
		/// </summary>
		[CanonicalCulture("es-NI")]
		NIO = 558,
		/// <summary>
		/// Norwegian Krone
		/// </summary>
		[CanonicalCulture("nn-NO")]
		NOK = 578,
		/// <summary>
		/// Nepalese Rupees
		/// </summary>
		[CanonicalCulture("ne-NP")]
		NPR = 524,
		/// <summary>
		/// New Zealand Dollar
		/// </summary>
		[CanonicalCulture("en-NZ")]
		NZD = 554,
		/// <summary>
		/// Omani Rial
		/// </summary>
		[CanonicalCulture("ar-OM")]
		OMR = 512,
		/// <summary>
		/// Panamanian Balboa
		/// </summary>
		[CanonicalCulture("es-PA")]
		PAB = 590,
		/// <summary>
		/// Peruvian Nuevo Sol
		/// </summary>
		[CanonicalCulture("es-PE")]
		PEN = 604,
		/// <summary>
		/// Kina
		/// </summary>
		PGK = 598,
		/// <summary>
		/// Philippine Peso
		/// </summary>
		[CanonicalCulture("fil-PH")]
		PHP = 608,
		/// <summary>
		/// Pakistan Rupee
		/// </summary>
		[CanonicalCulture("ur-PK")]
		PKR = 586,
		/// <summary>
		/// Polish Zloty
		/// </summary>
		[CanonicalCulture("pl-PL")]
		PLN = 985,
		/// <summary>
		/// Paraguay Guarani
		/// </summary>
		[CanonicalCulture("es-PY")]
		PYG = 600,
		/// <summary>
		/// Qatari Rial
		/// </summary>
		[CanonicalCulture("ar-QA")]
		QAR = 634,
		/// <summary>
		/// Romanian Leu
		/// </summary>
		[CanonicalCulture("ro-RO")]
		RON = 946,
		/// <summary>
		/// Serbian Dinar
		/// </summary>
		[CanonicalCulture("sr-Latn-RS", Overwritten = true)]
		RSD = 941,
		/// <summary>
		/// Russian Ruble
		/// </summary>
		[CanonicalCulture("ru-RU")]
		RUB = 643,
		/// <summary>
		/// Rwandan Franc
		/// </summary>
		[CanonicalCulture("rw-RW")]
		RWF = 646,
		/// <summary>
		/// Saudi Riyal
		/// </summary>
		[CanonicalCulture("ar-SA")]
		SAR = 682,
		/// <summary>
		/// Solomon Islands Dollar
		/// </summary>
		SBD = 090,
		/// <summary>
		/// Seychelles Rupee
		/// </summary>
		SCR = 690,
		/// <summary>
		/// Sudanese Pound
		/// </summary>
		SDG = 938,
		/// <summary>
		/// Swedish Krona
		/// </summary>
		[CanonicalCulture("sv-SE")]
		SEK = 752,
		/// <summary>
		/// Singapore Dollar
		/// </summary>
		[CanonicalCulture("zh-SG")]
		SGD = 702,
		/// <summary>
		/// Saint Helena Pound
		/// </summary>
		SHP = 654,
		/// <summary>
		/// Leone
		/// </summary>
		SLL = 694,
		/// <summary>
		/// Somali Shilling
		/// </summary>
		SOS = 706,
		/// <summary>
		/// Surinam Dollar
		/// </summary>
		SRD = 968,
		/// <summary>
		/// Dobra
		/// </summary>
		STD = 678,
		/// <summary>
		/// US Dollar
		/// </summary>
		[CanonicalCulture("es-SV")]
		SVC = 222,
		/// <summary>
		/// Syrian Pound
		/// </summary>
		[CanonicalCulture("ar-SY")]
		SYP = 760,
		/// <summary>
		/// Lilangeni
		/// </summary>
		SZL = 748,
		/// <summary>
		/// Thai Baht
		/// </summary>
		[CanonicalCulture("th-TH")]
		THB = 764,
		/// <summary>
		/// Ruble
		/// </summary>
		[CanonicalCulture("tg-Cyrl-TJ")]
		TJS = 972,
		/// <summary>
		/// Turkmen manat
		/// </summary>
		[CanonicalCulture("tk-TM")]
		TMT = 934,
		/// <summary>
		/// Tunisian Dinar
		/// </summary>
		[CanonicalCulture("ar-TN")]
		TND = 788,
		/// <summary>
		/// Pa'anga
		/// </summary>
		TOP = 776,
		/// <summary>
		/// Turkish Lira
		/// </summary>
		[CanonicalCulture("tr-TR", Overwritten = true)]
		TRY = 949,
		/// <summary>
		/// Trinidad Dollar
		/// </summary>
		[CanonicalCulture("en-TT")]
		TTD = 780,
		/// <summary>
		/// New Taiwan Dollar
		/// </summary>
		[CanonicalCulture("zh-TW")]
		TWD = 901,
		/// <summary>
		/// Tanzanian Shilling
		/// </summary>
		TZS = 834,
		/// <summary>
		/// Ukrainian Grivna
		/// </summary>
		[CanonicalCulture("uk-UA")]
		UAH = 980,
		/// <summary>
		/// Uganda Shilling
		/// </summary>
		UGX = 800,
		/// <summary>
		/// US Dollar
		/// </summary>
		[CanonicalCulture("en-US")]
		USD = 840,
		/// <summary>
		/// US Dollar (Next day)
		/// </summary>
		USN = 997,
		/// <summary>
		/// US Dollar (Same day)
		/// </summary>
		USS = 998,
		/// <summary>
		/// Peso Uruguayo
		/// </summary>
		[CanonicalCulture("es-UY")]
		UYI = 940,
		/// <summary>
		/// Peso Uruguayo
		/// </summary>
		[CanonicalCulture("es-UY")]
		UYU = 858,
		/// <summary>
		/// Uzbekistan Som
		/// </summary>
		[CanonicalCulture("uz-Latn-UZ")]
		UZS = 860,
		/// <summary>
		/// Venezuelan Bolivar
		/// </summary>
		[CanonicalCulture("es-VE")]
		VEF = 937,
		/// <summary>
		/// Vietnamese Dong
		/// </summary>
		[CanonicalCulture("vi-VN")]
		VND = 704,
		/// <summary>
		/// Vatu
		/// </summary>
		VUV = 548,
		/// <summary>
		/// Tala
		/// </summary>
		WST = 882,
		/// <summary>
		/// CFA Franc BEAC
		/// </summary>
		XAF = 950,
		/// <summary>
		/// Silver
		/// </summary>
		XAG = 961,
		/// <summary>
		/// Gold
		/// </summary>
		XAU = 959,
		/// <summary>
		/// Bond Markets Units European Composite Unit (EURCO)
		/// </summary>
		XBA = 955,
		/// <summary>
		/// European Monetary Unit (E.M.U.-6)
		/// </summary>
		XBB = 956,
		/// <summary>
		/// European Unit of Account 9(E.U.A.-9)
		/// </summary>
		XBC = 957,
		/// <summary>
		/// European Unit of Account 17(E.U.A.-17)
		/// </summary>
		XBD = 958,
		/// <summary>
		/// East Caribbean Dollar
		/// </summary>
		XCD = 951,
		/// <summary>
		/// SDR
		/// </summary>
		XDR = 960,
		/// <summary>
		/// CFA Franc BCEAO
		/// </summary>
		[CanonicalCulture("wo-SN")]
		XOF = 952,
		/// <summary>
		/// Palladium
		/// </summary>
		XPD = 964,
		/// <summary>
		/// CFP Franc
		/// </summary>
		XPF = 953,
		/// <summary>
		/// Platinum
		/// </summary>
		XPT = 962,
		/// <summary>
		/// Sucre
		/// </summary>
		XSU = 994,
		/// <summary>
		/// Test currency
		/// </summary>
		[Description("Test currency")]
		XTS = 963,
		/// <summary>
		/// No currency
		/// </summary>
		[Description("No currency")]
		XXX = 999,
		/// <summary>
		/// Yemeni Rial
		/// </summary>
		[CanonicalCulture("ar-YE")]
		YER = 886,
		/// <summary>
		/// South African Rand
		/// </summary>
		[CanonicalCulture("en-ZA")]
		ZAR = 710,
		/// <summary>
		/// Zambian Kwacha
		/// </summary>
		ZMK = 894,
		/// <summary>
		/// Zimbabwe Dollar
		/// </summary>
		[CanonicalCulture("en-ZW")]
		ZWL = 932
	}

	/// <summary>
	/// Contains extension to the type <see cref="CurrencyIsoCode"/>
	/// </summary>
	public static class IsoCodeExtensions
	{
		private const string EQUAL = " = ";

		/// <summary>
		/// Returns a combination of the ISO 4217 code and its numeric value, separated by the equals sign '<code>=</code>'.
		/// </summary>
		public static string AsValuePair(this CurrencyIsoCode isoCode)
		{
			return isoCode + EQUAL + isoCode.NumericCode();
		}

		/// <summary>
		/// The numeric ISO 4217 code of the <see cref="CurrencyIsoCode"/>
		/// </summary>
		public static short NumericCode(this CurrencyIsoCode isoCode)
		{
			return (short)isoCode;
		}

		/// <summary>
		/// Returns a padded three digit string representation of the <see cref="NumericCode"/>.
		/// </summary>
		public static string PaddedNumericCode(this CurrencyIsoCode isoCode)
		{
			return isoCode.NumericCode().ToString("000", CultureInfo.InvariantCulture);
		}
	}
}
