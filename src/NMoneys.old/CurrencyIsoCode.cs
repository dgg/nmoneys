﻿using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace NMoneys
{
	/// <summary>
	/// Currency codes as stated by the ISO 4217 standard 
	/// </summary>
	/// <seealso href="http://www.iso.org/iso/support/currency_codes_list-1.htm" />
#if NET
	[XmlRoot(Namespace = Serialization.Data.NAMESPACE, ElementName = Serialization.Data.Currency.ISO_CODE, DataType = Serialization.Data.Currency.DATA_TYPE, IsNullable = false)]
	[DataContract(Namespace = Serialization.Data.NAMESPACE, Name = Serialization.Data.Currency.ISO_CODE)]
#endif
	public enum CurrencyIsoCode : short
	{
		///<summary>
		/// UAE Dirham
		///</summary>
		[EnumMember, CanonicalCulture("ar-AE")]
		AED = 784,
		/// <summary>
		/// Afghani
		/// </summary>
		[EnumMember, CanonicalCulture("ps-AF")]
		AFN = 971,
		/// <summary>
		/// Lek
		/// </summary>
		[EnumMember, CanonicalCulture("sq-AL")]
		ALL = 008,
		/// <summary>
		/// Armenian Dram
		/// </summary>
		[EnumMember, CanonicalCulture("hy-AM")]
		AMD = 051,
		/// <summary>
		/// Netherlands Antillean Guilder
		/// </summary>
		[EnumMember, CanonicalCulture("nl-CW", Overwritten = true)]
		ANG = 532,
		/// <summary>
		/// Kwanza
		/// </summary>
		[EnumMember, CanonicalCulture("pt-AO", Overwritten = true)]
		AOA = 973,
		/// <summary>
		/// Argentine Peso
		/// </summary>
		[EnumMember, CanonicalCulture("es-AR")]
		ARS = 032,
		/// <summary>
		/// Australian Dollar
		/// </summary>
		[EnumMember, CanonicalCulture("en-AU")]
		AUD = 036,
		/// <summary>
		/// Aruban Florin
		/// </summary>
		[EnumMember, CanonicalCulture("nl-AW", Overwritten = true)]
		AWG = 533,
		/// <summary>
		/// Azerbaijan Manat
		/// </summary>
		[EnumMember, CanonicalCulture("az-Latn-AZ")]
		AZN = 944,
		/// <summary>
		/// Convertible Mark
		/// </summary>
		[EnumMember, CanonicalCulture("bs-Latn-BA")]
		BAM = 977,
		/// <summary>
		/// Barbados Dollar
		/// </summary>
		[EnumMember, CanonicalCulture("en-BB", Overwritten = true)]
		BBD = 052,
		/// <summary>
		/// Taka
		/// </summary>
		[EnumMember, CanonicalCulture("bn-BD")]
		BDT = 050,
		/// <summary>
		/// Bulgarian Lev
		/// </summary>
		[EnumMember, CanonicalCulture("bg-BG", Overwritten = true)]
		BGN = 975,
		/// <summary>
		/// Bahraini Dinar
		/// </summary>
		[EnumMember, CanonicalCulture("ar-BH")]
		BHD = 048,
		/// <summary>
		/// Burundi Franc
		/// </summary>
		[EnumMember, CanonicalCulture("fr-BI", Overwritten = true)]
		BIF = 108,
		/// <summary>
		/// Bermudian Dollar
		/// </summary>
		[EnumMember, CanonicalCulture("en-BM", Overwritten = true)]
		BMD = 060,
		/// <summary>
		/// Brunei Dollar
		/// </summary>
		[EnumMember, CanonicalCulture("ms-BN")]
		BND = 096,
		/// <summary>
		/// Boliviano
		/// </summary>
		[EnumMember, CanonicalCulture("es-BO")]
		BOB = 068,
		/// <summary>
		/// Mvdol
		/// </summary>
		[EnumMember, CanonicalCulture("es-BO")]
		BOV = 984,
		/// <summary>
		/// Brazilian Real
		/// </summary>
		[EnumMember, CanonicalCulture("pt-BR")]
		BRL = 986,
		/// <summary>
		/// Bahamian Dollar
		/// </summary>
		[EnumMember, CanonicalCulture("en-BS")]
		BSD = 044,
		/// <summary>
		/// Ngultrum
		/// </summary>
		[EnumMember, CanonicalCulture("dz-BT")]
		BTN = 064,
		/// <summary>
		/// Pula
		/// </summary>
		[EnumMember, CanonicalCulture("tn-BW")]
		BWP = 072,
		/// <summary>
		/// Belarusian Ruble
		/// </summary>
		[EnumMember, CanonicalCulture("be-BY")]
		BYN = 933,
		/// <summary>
		/// Belarussian Ruble
		/// </summary>
		[EnumMember, CanonicalCulture("be-BY"), Obsolete("deprecated in favor of BYN")]
		BYR = 974,
		/// <summary>
		/// Belize Dollar
		/// </summary>
		[EnumMember, CanonicalCulture("en-BZ")]
		BZD = 084,
		/// <summary>
		/// Canadian Dollar
		/// </summary>
		[EnumMember, CanonicalCulture("en-CA")]
		CAD = 124,
		/// <summary>
		/// Congolese Franc
		/// </summary>
		[EnumMember, CanonicalCulture("fr-CD")]
		CDF = 976,
		/// <summary>
		/// WIR Euro
		/// </summary>
		[EnumMember, CanonicalCulture("de-CH")]
		CHE = 947,
		/// <summary>
		/// Swiss Franc
		/// </summary>
		[EnumMember, CanonicalCulture("de-CH", Overwritten = true)]
		CHF = 756,
		/// <summary>
		/// WIR Franc
		/// </summary>
		[EnumMember, CanonicalCulture("de-CH")]
		CHW = 948,
		/// <summary>
		/// Unidad de Fomento
		/// </summary>
		[EnumMember, CanonicalCulture("es-CL")]
		CLF = 990,
		/// <summary>
		/// Chilean Peso
		/// </summary>
		[EnumMember, CanonicalCulture("es-CL")]
		CLP = 152,
		/// <summary>
		/// Yuan Renminbi
		/// </summary>
		[EnumMember, CanonicalCulture("zh-CN")]
		CNY = 156,
		/// <summary>
		/// Colombian Peso
		/// </summary>
		[EnumMember, CanonicalCulture("es-CO")]
		COP = 170,
		/// <summary>
		/// Unidad de Valor Real
		/// </summary>
		[EnumMember, CanonicalCulture("es-CO")]
		COU = 970,
		/// <summary>
		/// Costa Rican Colon
		/// </summary>
		[EnumMember, CanonicalCulture("es-CR")]
		CRC = 188,
		/// <summary>
		/// Peso Convertible
		/// </summary>
		[EnumMember]
		CUC = 931,
		/// <summary>
		/// Cuban Peso
		/// </summary>
		[EnumMember, CanonicalCulture("es-CU", Overwritten = true)]
		CUP = 192,
		/// <summary>
		/// Cape Verde Escudo
		/// </summary>
		[EnumMember, CanonicalCulture("pt-CV", Overwritten = true)]
		CVE = 132,
		/// <summary>
		/// Czech Koruna
		/// </summary>
		[EnumMember, CanonicalCulture("cs-CZ")]
		CZK = 203,
		/// <summary>
		/// Djibouti Franc
		/// </summary>
		[EnumMember, CanonicalCulture("fr-DJ")]
		DJF = 262,
		/// <summary>
		/// Danish Krone
		/// </summary>
		[EnumMember, CanonicalCulture("da-DK", Overwritten = true)]
		DKK = 208,
		/// <summary>
		/// Dominican Peso
		/// </summary>
		[EnumMember, CanonicalCulture("es-DO")]
		DOP = 214,
		/// <summary>
		/// Algerian Dinar
		/// </summary>
		[EnumMember, CanonicalCulture("ar-DZ")]
		DZD = 012,
		/// <summary>
		/// Estonian Kroon
		/// </summary>
		[EnumMember, CanonicalCulture("et-EE"), Obsolete("deprecated in favor of EUR")]
		EEK = 233,
		/// <summary>
		/// Egyptian Pound
		/// </summary>
		[EnumMember, CanonicalCulture("ar-EG")]
		EGP = 818,
		/// <summary>
		/// Nakfa
		/// </summary>
		[EnumMember, CanonicalCulture("ti-ER")]
		ERN = 232,
		/// <summary>
		/// Ethiopian Birr
		/// </summary>
		[EnumMember, CanonicalCulture("am-ET")]
		ETB = 230,
		/// <summary>
		/// Euro
		/// </summary>
		[EnumMember, CanonicalCulture("de-DE")]
		EUR = 978,
		/// <summary>
		/// Fiji Dollar
		/// </summary>
		[EnumMember, CanonicalCulture("en-FJ", Overwritten = true)]
		FJD = 242,
		/// <summary>
		/// Falkland Islands Pound
		/// </summary>
		[EnumMember, CanonicalCulture("en-FK")]
		FKP = 238,
		/// <summary>
		/// Pound Sterling
		/// </summary>
		[EnumMember, CanonicalCulture("en-GB")]
		GBP = 826,
		/// <summary>
		/// Lari
		/// </summary>
		[EnumMember, CanonicalCulture("ka-GE")]
		GEL = 981,
		/// <summary>
		/// Ghana Cedi
		/// </summary>
		[EnumMember, CanonicalCulture("en-GH", Overwritten = true)]
		GHS = 936,
		/// <summary>
		/// Gibraltar Pound
		/// </summary>
		[EnumMember, CanonicalCulture("en-GI")]
		GIP = 292,
		/// <summary>
		/// Dalasi
		/// </summary>
		[EnumMember, CanonicalCulture("en-GM", Overwritten = true)]
		GMD = 270,
		/// <summary>
		/// Guinean Franc
		/// </summary>
		[EnumMember, CanonicalCulture("fr-GN", Overwritten = true)]
		GNF = 324,
		/// <summary>
		/// Quetzal
		/// </summary>
		[EnumMember, CanonicalCulture("es-GT")]
		GTQ = 320,
		/// <summary>
		/// Guyana Dollar
		/// </summary>
		[EnumMember, CanonicalCulture("en-GY", Overwritten = true)]
		GYD = 328,
		/// <summary>
		/// Hong Kong Dollar
		/// </summary>
		[EnumMember, CanonicalCulture("zh-HK")]
		HKD = 344,
		/// <summary>
		/// Lempira
		/// </summary>
		[EnumMember, CanonicalCulture("es-HN")]
		HNL = 340,
		/// <summary>
		/// Croatian Kuna
		/// </summary>
		[EnumMember, CanonicalCulture("hr-HR")]
		HRK = 191,
		/// <summary>
		/// Gourde
		/// </summary>
		[EnumMember, CanonicalCulture("fr-HT")]
		HTG = 332,
		/// <summary>
		/// Forint
		/// </summary>
		[EnumMember, CanonicalCulture("hu-HU")]
		HUF = 348,
		/// <summary>
		/// Rupiah
		/// </summary>
		[EnumMember, CanonicalCulture("id-ID")]
		IDR = 360,
		/// <summary>
		/// New Israeli Sheqel
		/// </summary>
		[EnumMember, CanonicalCulture("he-IL")]
		ILS = 376,
		/// <summary>
		/// Indian Rupee
		/// </summary>
		[EnumMember, CanonicalCulture("hi-IN")]
		INR = 356,
		/// <summary>
		/// Iraqi Dinar
		/// </summary>
		[EnumMember, CanonicalCulture("ar-IQ")]
		IQD = 368,
		/// <summary>
		/// Iranian Rial
		/// </summary>
		[EnumMember, CanonicalCulture("fa-IR")]
		IRR = 364,
		/// <summary>
		/// Iceland Krona
		/// </summary>
		[EnumMember, CanonicalCulture("is-IS")]
		ISK = 352,
		/// <summary>
		/// Jamaican Dollar
		/// </summary>
		[EnumMember, CanonicalCulture("en-JM")]
		JMD = 388,
		/// <summary>
		/// Jordanian Dinar
		/// </summary>
		[EnumMember, CanonicalCulture("ar-JO")]
		JOD = 400,
		/// <summary>
		/// Yen
		/// </summary>
		[EnumMember, CanonicalCulture("ja-JP")]
		JPY = 392,
		/// <summary>
		/// Kenyan Shilling
		/// </summary>
		[EnumMember, CanonicalCulture("sw-KE")]
		KES = 404,
		/// <summary>
		/// Som
		/// </summary>
		[EnumMember, CanonicalCulture("ky-KG")]
		KGS = 417,
		/// <summary>
		/// Comorian Franc
		/// </summary>
		[EnumMember, CanonicalCulture("ar-KM", Overwritten = true)]
		KMF = 174,
		/// <summary>
		/// Riel
		/// </summary>
		[EnumMember, CanonicalCulture("km-KH")]
		KHR = 116,
		/// <summary>
		/// North Korean Won
		/// </summary>
		[EnumMember, CanonicalCulture("ko-KP", Overwritten = true)]
		KPW = 408,
		/// <summary>
		/// Won
		/// </summary>
		[EnumMember, CanonicalCulture("ko-KR")]
		KRW = 410,
		/// <summary>
		/// Kuwaiti Dinar
		/// </summary>
		[EnumMember, CanonicalCulture("ar-KW")]
		KWD = 414,
		/// <summary>
		/// Cayman Islands Dollar
		/// </summary>
		[EnumMember, CanonicalCulture("en-KY")]
		KYD = 136,
		/// <summary>
		/// Tenge
		/// </summary>
		[EnumMember, CanonicalCulture("kk-KZ")]
		KZT = 398,
		/// <summary>
		/// Lao Kip
		/// </summary>
		[EnumMember, CanonicalCulture("lo-LA")]
		LAK = 418,
		/// <summary>
		/// Lebanese Pound
		/// </summary>
		[EnumMember, CanonicalCulture("ar-LB")]
		LBP = 422,
		/// <summary>
		/// Sri Lanka Rupee
		/// </summary>
		[EnumMember, CanonicalCulture("si-LK")]
		LKR = 144,
		/// <summary>
		/// Liberian Dollar
		/// </summary>
		[EnumMember, CanonicalCulture("en-LR")]
		LRD = 430,
		/// <summary>
		/// Loti
		/// </summary>
		[EnumMember]
		LSL = 426,
		/// <summary>
		/// Lithuanian Litas
		/// </summary>
		[EnumMember, CanonicalCulture("lt-LT"), Obsolete("deprecated in favor of EUR")]
		LTL = 440,
		/// <summary>
		/// Latvian Lats
		/// </summary>
		[EnumMember, CanonicalCulture("lv-LV"), Obsolete("deprecated in favor of EUR")]
		LVL = 428,
		/// <summary>
		/// Libyan Dinar
		/// </summary>
		[EnumMember, CanonicalCulture("ar-LY")]
		LYD = 434,
		/// <summary>
		/// Moroccan Dirham
		/// </summary>
		[EnumMember, CanonicalCulture("ar-MA")]
		MAD = 504,
		/// <summary>
		/// Moldovan Leu
		/// </summary>
		[EnumMember, CanonicalCulture("ro-MD")]
		MDL = 498,
		/// <summary>
		/// Malagasy Ariary
		/// </summary>
		[EnumMember, CanonicalCulture("mg-MG", Overwritten = true)]
		MGA = 969,
		/// <summary>
		/// Denar
		/// </summary>
		[EnumMember, CanonicalCulture("mk-MK")]
		MKD = 807,
		/// <summary>
		/// Kyat
		/// </summary>
		[EnumMember, CanonicalCulture("my-MM")]
		MMK = 104,
		/// <summary>
		/// Tugrik
		/// </summary>
		[EnumMember, CanonicalCulture("mn-MN")]
		MNT = 496,
		/// <summary>
		/// Pataca
		/// </summary>
		[EnumMember, CanonicalCulture("zh-MO")]
		MOP = 446,
		/// <summary>
		/// Ouguiya
		/// </summary>
		[EnumMember, Obsolete]
		MRO = 478,
		/// <summary>
		/// Ouguiya
		/// </summary>
		[EnumMember, CanonicalCulture("ar-MR", Overwritten = true)]
		MRU = 929,
		/// <summary>
		/// Mauritius Rupee
		/// </summary>
		[EnumMember, CanonicalCulture("en-MU", Overwritten = true)]
		MUR = 480,
		/// <summary>
		/// Rufiyaa
		/// </summary>
		[EnumMember, CanonicalCulture("dv-MV")]
		MVR = 462,
		/// <summary>
		/// Kwacha
		/// </summary>
		[EnumMember, CanonicalCulture("en-MW", Overwritten = true)]
		MWK = 454,
		/// <summary>
		/// Mexican Peso
		/// </summary>
		[EnumMember, CanonicalCulture("es-MX")]
		MXN = 484,
		/// <summary>
		/// Mexican Unidad de Inversion (UDI)
		/// </summary>
		[EnumMember, CanonicalCulture("es-MX")]
		MXV = 979,
		/// <summary>
		/// Malaysian Ringgit
		/// </summary>
		[EnumMember, CanonicalCulture("ms-MY", Overwritten = true)]
		MYR = 458,
		/// <summary>
		/// Mozambique Metical
		/// </summary>
		[EnumMember, CanonicalCulture("pt-MZ", Overwritten = true)]
		MZN = 943,
		/// <summary>
		/// Namibia Dollar
		/// </summary>
		[EnumMember, CanonicalCulture("en-NA", Overwritten = true)]
		NAD = 516,
		/// <summary>
		/// Naira
		/// </summary>
		[EnumMember, CanonicalCulture("ha-Latn-NG")]
		NGN = 566,
		/// <summary>
		/// Cordoba Oro
		/// </summary>
		[EnumMember, CanonicalCulture("es-NI")]
		NIO = 558,
		/// <summary>
		/// Norwegian Krone
		/// </summary>
		[EnumMember, CanonicalCulture("nn-NO")]
		NOK = 578,
		/// <summary>
		/// Nepalese Rupee
		/// </summary>
		[EnumMember, CanonicalCulture("ne-NP")]
		NPR = 524,
		/// <summary>
		/// New Zealand Dollar
		/// </summary>
		[EnumMember, CanonicalCulture("en-NZ")]
		NZD = 554,
		/// <summary>
		/// Rial Omani
		/// </summary>
		[EnumMember, CanonicalCulture("ar-OM")]
		OMR = 512,
		/// <summary>
		/// Balboa
		/// </summary>
		[EnumMember, CanonicalCulture("es-PA")]
		PAB = 590,
		/// <summary>
		/// Nuevo Sol
		/// </summary>
		[EnumMember, CanonicalCulture("es-PE")]
		PEN = 604,
		/// <summary>
		/// Kina
		/// </summary>
		[EnumMember, CanonicalCulture("en-PG", Overwritten = true)]
		PGK = 598,
		/// <summary>
		/// Philippine Peso
		/// </summary>
		[EnumMember, CanonicalCulture("fil-PH")]
		PHP = 608,
		/// <summary>
		/// Pakistan Rupee
		/// </summary>
		[EnumMember, CanonicalCulture("ur-PK")]
		PKR = 586,
		/// <summary>
		/// Zloty
		/// </summary>
		[EnumMember, CanonicalCulture("pl-PL")]
		PLN = 985,
		/// <summary>
		/// Guarani
		/// </summary>
		[EnumMember, CanonicalCulture("es-PY")]
		PYG = 600,
		/// <summary>
		/// Qatari Rial
		/// </summary>
		[EnumMember, CanonicalCulture("ar-QA")]
		QAR = 634,
		/// <summary>
		/// New Romanian Leu
		/// </summary>
		[EnumMember, CanonicalCulture("ro-RO")]
		RON = 946,
		/// <summary>
		/// Serbian Dinar
		/// </summary>
		[EnumMember, CanonicalCulture("sr-Latn-RS", Overwritten = true)]
		RSD = 941,
		/// <summary>
		/// Russian Ruble
		/// </summary>
		[EnumMember, CanonicalCulture("ru-RU")]
		RUB = 643,
		/// <summary>
		/// Rwanda Franc
		/// </summary>
		[EnumMember, CanonicalCulture("rw-RW")]
		RWF = 646,
		/// <summary>
		/// Saudi Riyal
		/// </summary>
		[EnumMember, CanonicalCulture("ar-SA")]
		SAR = 682,
		/// <summary>
		/// Solomon Islands Dollar
		/// </summary>
		[EnumMember, CanonicalCulture("en-SB")]
		SBD = 090,
		/// <summary>
		/// Seychelles Rupee
		/// </summary>
		[EnumMember, CanonicalCulture("en-SC", Overwritten = true)]
		SCR = 690,
		/// <summary>
		/// Sudanese Pound
		/// </summary>
		[EnumMember, CanonicalCulture("ar-SD")]
		SDG = 938,
		/// <summary>
		/// Swedish Krona
		/// </summary>
		[EnumMember, CanonicalCulture("sv-SE")]
		SEK = 752,
		/// <summary>
		/// Singapore Dollar
		/// </summary>
		[EnumMember, CanonicalCulture("zh-SG")]
		SGD = 702,
		/// <summary>
		/// Saint Helena Pound
		/// </summary>
		[EnumMember, CanonicalCulture("en-SH")]
		SHP = 654,
		/// <summary>
		/// Leone
		/// </summary>
		[EnumMember, CanonicalCulture("en-SL")]
		SLL = 694,
		/// <summary>
		/// Somali Shilling
		/// </summary>
		[EnumMember, CanonicalCulture("so-SO")]
		SOS = 706,
		/// <summary>
		/// Surinam Dollar
		/// </summary>
		[EnumMember, CanonicalCulture("nl-SR")]
		SRD = 968,
		/// <summary>
		/// South Sudanese Pound
		/// </summary>
		[EnumMember, CanonicalCulture("en-SS")]
		SSP = 728,
		/// <summary>
		/// Dobra
		/// </summary>
		[EnumMember, Obsolete]
		STD = 678,
		/// <summary>
		/// Dobra
		/// </summary>
		[EnumMember, CanonicalCulture("pt-ST", Overwritten = true)]
		STN = 930,
		/// <summary>
		/// El Salvador Colon
		/// </summary>
		[EnumMember, CanonicalCulture("es-SV")]
		SVC = 222,
		/// <summary>
		/// Syrian Pound
		/// </summary>
		[EnumMember, CanonicalCulture("ar-SY")]
		SYP = 760,
		/// <summary>
		/// Lilangeni
		/// </summary>
		[EnumMember, CanonicalCulture("ss-SZ")]
		SZL = 748,
		/// <summary>
		/// Baht
		/// </summary>
		[EnumMember, CanonicalCulture("th-TH")]
		THB = 764,
		/// <summary>
		/// Somoni
		/// </summary>
		[EnumMember, CanonicalCulture("tg-Cyrl-TJ")]
		TJS = 972,
		/// <summary>
		/// Turkmenistan New Manat
		/// </summary>
		[EnumMember, CanonicalCulture("tk-TM")]
		TMT = 934,
		/// <summary>
		/// Tunisian Dinar
		/// </summary>
		[EnumMember, CanonicalCulture("ar-TN")]
		TND = 788,
		/// <summary>
		/// Paʻanga
		/// </summary>
		[EnumMember, CanonicalCulture("to-TO", Overwritten = true)]
		TOP = 776,
		/// <summary>
		/// Turkish Lira
		/// </summary>
		[EnumMember, CanonicalCulture("tr-TR", Overwritten = true)]
		TRY = 949,
		/// <summary>
		/// Trinidad and Tobago Dollar
		/// </summary>
		[EnumMember, CanonicalCulture("en-TT")]
		TTD = 780,
		/// <summary>
		/// New Taiwan Dollar
		/// </summary>
		[EnumMember, CanonicalCulture("zh-TW")]
		TWD = 901,
		/// <summary>
		/// Tanzanian Shilling
		/// </summary>
		[EnumMember, CanonicalCulture("sw-TZ", Overwritten = true)]
		TZS = 834,
		/// <summary>
		/// Hryvnia
		/// </summary>
		[EnumMember, CanonicalCulture("uk-UA")]
		UAH = 980,
		/// <summary>
		/// Uganda Shilling
		/// </summary>
		[EnumMember, CanonicalCulture("sw-UG", Overwritten = true)]
		UGX = 800,
		/// <summary>
		/// US Dollar
		/// </summary>
		[EnumMember, CanonicalCulture("en-US")]
		USD = 840,
		/// <summary>
		/// US Dollar (Next day)
		/// </summary>
		[EnumMember]
		USN = 997,
		/// <summary>
		/// US Dollar (Same day)
		/// </summary>
		[EnumMember, Obsolete("No longer in use")]
		USS = 998,
		/// <summary>
		/// >Uruguay Peso en Unidades Indexadas (UI)
		/// </summary>
		[EnumMember, CanonicalCulture("es-UY")]
		UYI = 940,
		/// <summary>
		/// Peso Uruguayo
		/// </summary>
		[EnumMember, CanonicalCulture("es-UY")]
		UYU = 858,
		/// <summary>
		/// Unidad Previsional
		/// </summary>
		[EnumMember, CanonicalCulture("es-UY")]
		UYW = 927,
		/// <summary>
		/// Uzbekistan Sum
		/// </summary>
		[EnumMember, CanonicalCulture("uz-Latn-UZ")]
		UZS = 860,
		/// <summary>
		/// Bolivar
		/// </summary>
		[EnumMember, CanonicalCulture("es-VE"), Obsolete]
		VEF = 937,
		/// <summary>
		/// Bolívar Soberano
		/// </summary>
		[EnumMember, CanonicalCulture("es-VE")]
		VES = 928,
		/// <summary>
		/// Dong
		/// </summary>
		[EnumMember, CanonicalCulture("vi-VN")]
		VND = 704,
		/// <summary>
		/// Vatu
		/// </summary>
		[EnumMember, CanonicalCulture("fr-VU", Overwritten = true)]
		VUV = 548,
		/// <summary>
		/// Tala
		/// </summary>
		[EnumMember, CanonicalCulture("en-WS", Overwritten = true)]
		WST = 882,
		/// <summary>
		/// CFA Franc BEAC
		/// </summary>
		[EnumMember, CanonicalCulture("fr-CM")]
		XAF = 950,
		/// <summary>
		/// Silver
		/// </summary>
		[EnumMember]
		XAG = 961,
		/// <summary>
		/// Gold
		/// </summary>
		[EnumMember]
		XAU = 959,
		/// <summary>
		/// European Composite Unit (EURCO)
		/// </summary>
		[EnumMember]
		XBA = 955,
		/// <summary>
		/// European Monetary Unit (E.M.U.-6)
		/// </summary>
		[EnumMember]
		XBB = 956,
		/// <summary>
		/// European Unit of Account 9(E.U.A.-9)
		/// </summary>
		[EnumMember]
		XBC = 957,
		/// <summary>
		/// European Unit of Account 17(E.U.A.-17)
		/// </summary>
		[EnumMember]
		XBD = 958,
		/// <summary>
		/// East Caribbean Dollar
		/// </summary>
		[EnumMember, CanonicalCulture("en-029")]
		XCD = 951,
		/// <summary>
		/// SDR (Special Drawing Right)
		/// </summary>
		[EnumMember, CanonicalCulture("es-419")]
		XDR = 960,
		/// <summary>
		/// CFA Franc BCEAO
		/// </summary>
		[EnumMember, CanonicalCulture("wo-SN")]
		XOF = 952,
		/// <summary>
		/// Palladium
		/// </summary>
		[EnumMember]
		XPD = 964,
		/// <summary>
		/// CFP Franc
		/// </summary>
		[EnumMember, CanonicalCulture("fr-NC", Overwritten = true)]
		XPF = 953,
		/// <summary>
		/// Platinum
		/// </summary>
		[EnumMember]
		XPT = 962,
		/// <summary>
		/// Sucre
		/// </summary>
		[EnumMember]
		XSU = 994,
		/// <summary>
		/// Test currency
		/// </summary>
		[EnumMember]
		XTS = 963,
		/// <summary>
		/// ADB Unit of Account
		/// </summary>
		[EnumMember]
		XUA = 965,
		/// <summary>
		/// No currency
		/// </summary>
		[EnumMember]
		XXX = 999,
		/// <summary>
		/// Yemeni Rial
		/// </summary>
		[EnumMember, CanonicalCulture("ar-YE")]
		YER = 886,
		/// <summary>
		/// Rand
		/// </summary>
		[EnumMember, CanonicalCulture("en-ZA")]
		ZAR = 710,
		/// <summary>
		/// Zambian Kwacha
		/// </summary>
		[EnumMember, Obsolete("deprecated in favor of ZMW")]
		ZMK = 894,
		/// <summary>
		/// Zambian Kwacha
		/// </summary>
		[EnumMember, CanonicalCulture("en-ZM", Overwritten = true)]
		ZMW = 967,
		/// <summary>
		/// Zimbabwe Dollar
		/// </summary>
		[EnumMember, CanonicalCulture("en-ZW")]
		ZWL = 932
	}
}
