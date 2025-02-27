using NMoneys.Support;

namespace NMoneys;

/// <summary>
/// Currency codes as stated by the ISO 4217 standard
/// </summary>
/// <seealso href="http://www.iso.org/iso/support/currency_codes_list-1.htm" />
#pragma warning disable CA1008, CA1028
public enum CurrencyIsoCode : ushort
{
	/// <summary>
	/// UAE Dirham
	/// </summary>
	[CanonicalCulture("ar-AE")]
	[Info(
		englishName: "UAE Dirham", nativeName: "درهم إماراتي", symbol: "د.إ.",
		significantDecimalDigits: 2,
		decimalSeparator: "٫",
		groupSeparator: "٬", groupSizes: new byte[] { 3 },
		positivePattern: 3, negativePattern: 8
	)]
	AED = 784,

	/// <summary>
	/// Afghani
	/// </summary>
	[CanonicalCulture("ps-AF", Overwritten = true)]
	[Info(
		englishName: "Afghani", nativeName: "افغانۍ", symbol: "؋",
		significantDecimalDigits: 2,
		decimalSeparator: "٫",
		groupSeparator: "٬", groupSizes: new byte[] { 3 },
		positivePattern: 3, negativePattern: 8
	)]
	AFN = 971,

	/// <summary>
	/// Lek
	/// </summary>
	[CanonicalCulture("sq-AL", Overwritten = true)]
	[Info(
		englishName: "Lek", nativeName: "Leku shqiptar", symbol: "L",
		significantDecimalDigits: 0,
		decimalSeparator: ",",
		groupSeparator: "\u00a0", groupSizes: new byte[] { 3 },
		positivePattern: 3, negativePattern: 8
	)]
	ALL = 008,

	/// <summary>
	/// Armenian Dram
	/// </summary>
	[CanonicalCulture("hy-AM", Overwritten = true)]
	[Info(
		englishName: "Armenian Dram", nativeName: "դրամ", symbol: "֏",
		significantDecimalDigits: 2,
		decimalSeparator: ",",
		groupSeparator: " ", groupSizes: new byte[] { 3 },
		positivePattern: 3, negativePattern: 8
	)]
	AMD = 051,

	/// <summary>
	/// Netherlands Antillean Guilder
	/// </summary>
	[CanonicalCulture("nl-CW", Overwritten = true)]
	[Info(
		englishName: "Netherlands Antillean Guilder", nativeName: "Antilliaanse gulden", symbol: "NAf.",
		significantDecimalDigits: 2,
		decimalSeparator: ",",
		groupSeparator: ".", groupSizes: new byte[] { 3 },
		positivePattern: 2, negativePattern: 12
	)]
	ANG = 532,

	/// <summary>
	/// Kwanza
	/// </summary>
	[CanonicalCulture("pt-AO")]
	[Info(
		englishName: "Kwanza", nativeName: "kwanza angolano", symbol: "Kz",
		significantDecimalDigits: 2,
		decimalSeparator: ",",
		groupSeparator: "\u00a0", groupSizes: new byte[] { 3 },
		positivePattern: 3, negativePattern: 8
	)]
	AOA = 973,

	/// <summary>
	/// Argentine Peso
	/// </summary>
	[CanonicalCulture("es-AR", Overwritten = true)]
	[Info(
		englishName: "Argentine Peso", nativeName: "Peso", symbol: "$",
		significantDecimalDigits: 2,
		decimalSeparator: ",",
		groupSeparator: ".", groupSizes: new byte[] { 3 },
		positivePattern: 2, negativePattern: 9,
		codePoint: 36, entityName: "dollar"
	)]
	ARS = 032,

	/// <summary>
	/// Australian Dollar
	/// </summary>
	[CanonicalCulture("en-AU")]
	[Info(
		englishName: "Australian Dollar", nativeName: "Australian Dollar", symbol: "$",
		significantDecimalDigits: 2,
		decimalSeparator: ".",
		groupSeparator: ",", groupSizes: new byte[] { 3 },
		positivePattern: 0, negativePattern: 1,
		codePoint: 36, entityName: "dollar"
	)]
	AUD = 036,

	/// <summary>
	/// Aruban Florin
	/// </summary>
	[CanonicalCulture("nl-AW", Overwritten = true)]
	[Info(
		englishName: "Aruban Florin", nativeName: "Arubaanse florin", symbol: "Afl.",
		significantDecimalDigits: 2,
		decimalSeparator: ",",
		groupSeparator: ".", groupSizes: new byte[] { 3 },
		positivePattern: 2, negativePattern: 12
	)]
	AWG = 533,

	/// <summary>
	/// Azerbaijan Manat
	/// </summary>
	[CanonicalCulture("az-Latn-AZ")]
	[Info(
		englishName: "Azerbaijan Manat", nativeName: "Azərbaycan Manatı", symbol: "₼",
		significantDecimalDigits: 2,
		decimalSeparator: ",",
		groupSeparator: ".", groupSizes: new byte[] { 3 },
		positivePattern: 3, negativePattern: 8,
		codePoint: 8380
	)]
	AZN = 944,

	/// <summary>
	/// Convertible Mark
	/// </summary>
	[CanonicalCulture("bs-Latn-BA", Overwritten = true)]
	[Info(
		englishName: "Convertible Mark", nativeName: "Konvertibilna marka", symbol: "KM",
		significantDecimalDigits: 2,
		decimalSeparator: ",",
		groupSeparator: ".", groupSizes: new byte[] { 3 },
		positivePattern: 3, negativePattern: 8
	)]
	BAM = 977,

	/// <summary>
	/// Barbados Dollar
	/// </summary>
	[CanonicalCulture("en-BB")]
	[Info(
		englishName: "Barbados Dollar", nativeName: "Barbadian Dollar", symbol: "$",
		significantDecimalDigits: 2,
		decimalSeparator: ".",
		groupSeparator: ",", groupSizes: new byte[] { 3 },
		positivePattern: 0, negativePattern: 1,
		codePoint: 36, entityName: "dollar"
	)]
	BBD = 052,

	/// <summary>
	/// Taka
	/// </summary>
	[CanonicalCulture("bn-BD", Overwritten = true)]
	[Info(
		englishName: "Taka", nativeName: "টাকা", symbol: "৳",
		significantDecimalDigits: 2,
		decimalSeparator: ".",
		groupSeparator: ",", groupSizes: new byte[] { 3, 2 },
		positivePattern: 1, negativePattern: 5,
		codePoint: 2547
	)]
	BDT = 050,

	/// <summary>
	/// Bulgarian Lev
	/// </summary>
	[CanonicalCulture("bg-BG")]
	[Info(
		englishName: "Bulgarian Lev", nativeName: "Български лев", symbol: "лв.",
		significantDecimalDigits: 2,
		decimalSeparator: ",",
		groupSeparator: "\u00a0", groupSizes: new byte[] { 0 },
		positivePattern: 3, negativePattern: 8
	)]
	BGN = 975,

	/// <summary>
	/// Bahraini Dinar
	/// </summary>
	[CanonicalCulture("ar-BH")]
	[Info(
		englishName: "Bahraini Dinar", nativeName: "دينار بحريني", symbol: "د.ب.‏",
		significantDecimalDigits: 3,
		decimalSeparator: "٫",
		groupSeparator: "٬", groupSizes: new byte[] { 3 },
		positivePattern: 3, negativePattern: 8
	)]
	BHD = 048,

	/// <summary>
	/// Burundi Franc
	/// </summary>
	[CanonicalCulture("fr-BI")]
	[Info(
		englishName: "Burundi Franc", nativeName: "franc burundais", symbol: "FBu",
		significantDecimalDigits: 0,
		decimalSeparator: ",",
		groupSeparator: " ", groupSizes: new byte[] { 3 },
		positivePattern: 3, negativePattern: 8
	)]
	BIF = 108,

	/// <summary>
	/// Bermudian Dollar
	/// </summary>
	[CanonicalCulture("en-BM")]
	[Info(
		englishName: "Bermudian Dollar", nativeName: "Bermudian Dollar", symbol: "$",
		significantDecimalDigits: 2,
		decimalSeparator: ".",
		groupSeparator: ",", groupSizes: new byte[] { 3 },
		positivePattern: 0, negativePattern: 1,
		codePoint: 36, entityName: "dollar"
	)]
	BMD = 060,

	/// <summary>
	/// Brunei Dollar
	/// </summary>
	[CanonicalCulture("ms-BN", Overwritten = true)]
	[Info(
		englishName: "Brunei Dollar", nativeName: "ringgit Brunei", symbol: "$",
		significantDecimalDigits: 2,
		decimalSeparator: ",",
		groupSeparator: ".", groupSizes: new byte[] { 3 },
		positivePattern: 2, negativePattern: 9,
		codePoint: 36, entityName: "dollar"
	)]
	BND = 096,

	/// <summary>
	/// Boliviano
	/// </summary>
	[CanonicalCulture("es-BO")]
	[Info(
		englishName: "Boliviano", nativeName: "boliviano", symbol: "Bs",
		significantDecimalDigits: 2,
		decimalSeparator: ",",
		groupSeparator: ".", groupSizes: new byte[] { 3 },
		positivePattern: 0, negativePattern: 1
	)]
	BOB = 068,

	/// <summary>
	/// Mvdol
	/// </summary>
	[CanonicalCulture("es-BO", Overwritten = true)]
	[Info(
		englishName: "Mvdol", nativeName: "Boliviano con Mantenimiento de Valor respecto al Dólar", symbol: "$b",
		significantDecimalDigits: 2,
		decimalSeparator: ",",
		groupSeparator: ".", groupSizes: new byte[] { 3 },
		positivePattern: 0, negativePattern: 1
	)]
	BOV = 984,

	/// <summary>
	/// Brazilian Real
	/// </summary>
	[CanonicalCulture("pt-BR")]
	[Info(
		englishName: "Brazilian Real", nativeName: "Real brasileiro", symbol: "R$",
		significantDecimalDigits: 2,
		decimalSeparator: ",",
		groupSeparator: ".", groupSizes: new byte[] { 3 },
		positivePattern: 2, negativePattern: 9
	)]
	BRL = 986,

	/// <summary>
	/// Bahamian Dollar
	/// </summary>
	[CanonicalCulture("en-BS")]
	[Info(
		englishName: "Bahamian Dollar", nativeName: "Bahamian Dollar", symbol: "$",
		significantDecimalDigits: 2,
		decimalSeparator: ".",
		groupSeparator: ",", groupSizes: new byte[] { 3 },
		positivePattern: 0, negativePattern: 1,
		codePoint: 36, entityName: "dollar"
	)]
	BSD = 044,

	/// <summary>
	/// Ngultrum
	/// </summary>
	[CanonicalCulture("dz-BT")]
	[Info(
		englishName: "Ngultrum", nativeName: "དངུལ་ཀྲམ", symbol: "Nu.",
		significantDecimalDigits: 2,
		decimalSeparator: ".",
		groupSeparator: ",", groupSizes: new byte[] { 3, 2 },
		positivePattern: 0, negativePattern: 1
	)]
	BTN = 064,

	/// <summary>
	/// Pula
	/// </summary>
	[CanonicalCulture("tn-BW", Overwritten = true)]
	[Info(
		englishName: "Pula", nativeName: "Pula", symbol: "P",
		significantDecimalDigits: 2,
		decimalSeparator: ".",
		groupSeparator: " ", groupSizes: new byte[] { 3 },
		positivePattern: 0, negativePattern: 1
	)]
	BWP = 072,

	/// <summary>
	/// Belarusian Ruble
	/// </summary>
	[CanonicalCulture("be-BY")]
	[Info(
		englishName: "Belarusian Ruble", nativeName: "беларускі рубель", symbol: "Br",
		significantDecimalDigits: 2,
		decimalSeparator: ",",
		groupSeparator: "\u00a0", groupSizes: new byte[] { 3 },
		positivePattern: 3, negativePattern: 8
	)]
	BYN = 933,

	/// <summary>
	/// Belarussian Ruble
	/// </summary>
	[CanonicalCulture("be-BY", Overwritten = true), Obsolete("deprecated in favor of BYN")]
	[Info(
		englishName: "Belarussian Ruble", nativeName: "беларускі рубель", symbol: "Br",
		significantDecimalDigits: 0,
		decimalSeparator: ",",
		groupSeparator: "\u00a0", groupSizes: new byte[] { 3 },
		positivePattern: 3, negativePattern: 8,
		isObsolete: true
	)]
	BYR = 974,

	/// <summary>
	/// Belize Dollar
	/// </summary>
	[CanonicalCulture("en-BZ")]
	[Info(
		englishName: "Belize Dollar", nativeName: "Belize Dollar", symbol: "$",
		significantDecimalDigits: 2,
		decimalSeparator: ".",
		groupSeparator: ",", groupSizes: new byte[] { 3 },
		positivePattern: 0, negativePattern: 1,
		codePoint: 36, entityName: "dollar"
	)]
	BZD = 084,

	/// <summary>
	/// Canadian Dollar
	/// </summary>
	[CanonicalCulture("en-CA")]
	[Info(
		englishName: "Canadian Dollar", nativeName: "Canadian Dollar", symbol: "$",
		significantDecimalDigits: 2,
		decimalSeparator: ".",
		groupSeparator: ",", groupSizes: new byte[] { 3 },
		positivePattern: 0, negativePattern: 1,
		codePoint: 36, entityName: "dollar"
	)]
	CAD = 124,

	/// <summary>
	/// Congolese Franc
	/// </summary>
	[CanonicalCulture("fr-CD")]
	[Info(
		englishName: "Congolese Franc", nativeName: "franc congolais", symbol: "FC",
		significantDecimalDigits: 2,
		decimalSeparator: ",",
		groupSeparator: " ", groupSizes: new byte[] { 3 },
		positivePattern: 3, negativePattern: 8
	)]
	CDF = 976,

	/// <summary>
	/// WIR Euro
	/// </summary>
	[CanonicalCulture("de-CH", Overwritten = true)]
	[Info(
		englishName: "WIR Euro", nativeName: "WIR Euro", symbol: "€",
		significantDecimalDigits: 2,
		decimalSeparator: ".",
		groupSeparator: "’", groupSizes: new byte[] { 3 },
		positivePattern: 2, negativePattern: 2,
		codePoint: 8364, entityName: "euro"
	)]
	CHE = 947,

	/// <summary>
	/// Swiss Franc
	/// </summary>
	[CanonicalCulture("de-CH", Overwritten = true)]
	[Info(
		englishName: "Swiss Franc", nativeName: "Schweizer Franken", symbol: "Fr.",
		significantDecimalDigits: 2,
		decimalSeparator: ".",
		groupSeparator: "’", groupSizes: new byte[] { 3 },
		positivePattern: 2, negativePattern: 2
	)]
	CHF = 756,

	/// <summary>
	/// WIR Franc
	/// </summary>
	[CanonicalCulture("de-CH", Overwritten = true)]
	[Info(
		englishName: "WIR Franc", nativeName: "WIR Franken", symbol: "Fr.",
		significantDecimalDigits: 2,
		decimalSeparator: ".",
		groupSeparator: "’", groupSizes: new byte[] { 3 },
		positivePattern: 2, negativePattern: 2
	)]
	CHW = 948,

	/// <summary>
	/// Unidad de Fomento
	/// </summary>
	[CanonicalCulture("es-CL", Overwritten = true)]
	[Info(
		englishName: "Unidad de Fomento", nativeName: "Unidad de Fomento", symbol: "$",
		significantDecimalDigits: 4,
		decimalSeparator: ",",
		groupSeparator: ".", groupSizes: new byte[] { 3 },
		positivePattern: 0, negativePattern: 2,
		codePoint: 36, entityName: "dollar"
	)]
	CLF = 990,

	/// <summary>
	/// Chilean Peso
	/// </summary>
	[CanonicalCulture("es-CL")]
	[Info(
		englishName: "Chilean Peso", nativeName: "Peso chileno", symbol: "$",
		significantDecimalDigits: 0,
		decimalSeparator: ",",
		groupSeparator: ".", groupSizes: new byte[] { 3 },
		positivePattern: 0, negativePattern: 2,
		codePoint: 36, entityName: "dollar"
	)]
	CLP = 152,

	/// <summary>
	/// Yuan Renminbi
	/// </summary>
	[CanonicalCulture("zh-CN")]
	[Info(
		englishName: "Yuan Renminbi", nativeName: "人民币", symbol: "¥",
		significantDecimalDigits: 2,
		decimalSeparator: ".",
		groupSeparator: ",", groupSizes: new byte[] { 3 },
		positivePattern: 0, negativePattern: 1
	)]
	CNY = 156,

	/// <summary>
	/// Colombian Peso
	/// </summary>
	[CanonicalCulture("es-CO")]
	[Info(
		englishName: "Colombian Peso", nativeName: "peso colombiano", symbol: "$",
		significantDecimalDigits: 2,
		decimalSeparator: ",",
		groupSeparator: ".", groupSizes: new byte[] { 3 },
		positivePattern: 2, negativePattern: 9,
		codePoint: 36, entityName: "dollar"
	)]
	COP = 170,

	/// <summary>
	/// Unidad de Valor Real
	/// </summary>
	[CanonicalCulture("es-CO", Overwritten = true)]
	[Info(
		englishName: "Unidad de Valor Real", nativeName: "Unidad de Valor Real", symbol: "$",
		significantDecimalDigits: 2,
		decimalSeparator: ",",
		groupSeparator: ".", groupSizes: new byte[] { 3 },
		positivePattern: 2, negativePattern: 9,
		codePoint: 36, entityName: "dollar"
	)]
	COU = 970,

	/// <summary>
	/// Costa Rican Colon
	/// </summary>
	[CanonicalCulture("es-CR")]
	[Info(
		englishName: "Costa Rican Colon", nativeName: "colón costarricense", symbol: "₡",
		significantDecimalDigits: 2,
		decimalSeparator: ",",
		groupSeparator: " ", groupSizes: new byte[] { 3 },
		positivePattern: 0, negativePattern: 1,
		codePoint: 8353
	)]
	CRC = 188,

	/// <summary>
	/// Peso Convertible
	/// </summary>
	[CanonicalCulture("es-CU", Overwritten = true), Obsolete("withdrawn of circulation")]
	[Info(
		englishName: "Peso Convertible", nativeName: "peso cubano convertible", symbol: "CUC$",
		significantDecimalDigits: 2,
		decimalSeparator: ".",
		groupSeparator: ",", groupSizes: new byte[] { 3 },
		positivePattern: 0, negativePattern: 1,
		codePoint: 36, entityName: "dollar",
		isObsolete: true
	)]
	CUC = 931,

	/// <summary>
	/// Cuban Peso
	/// </summary>
	[CanonicalCulture("es-CU")]
	[Info(
		englishName: "Cuban Peso", nativeName: "peso cubano", symbol: "$",
		significantDecimalDigits: 2,
		decimalSeparator: ".",
		groupSeparator: ",", groupSizes: new byte[] { 3 },
		positivePattern: 0, negativePattern: 1,
		codePoint: 36, entityName: "dollar"
	)]
	CUP = 192,

	/// <summary>
	/// Cape Verde Escudo
	/// </summary>
	[CanonicalCulture("pt-CV")]
	[Info(
		englishName: "Cabo Verde Escudo", nativeName: "escudo cabo-verdiano", symbol: "",
		significantDecimalDigits: 2,
		decimalSeparator: "$",
		groupSeparator: "\u00a0", groupSizes: new byte[] { 3 },
		positivePattern: 3, negativePattern: 8
	)]
	CVE = 132,

	/// <summary>
	/// Czech Koruna
	/// </summary>
	[CanonicalCulture("cs-CZ", Overwritten = true)]
	[Info(
		englishName: "Czech Koruna", nativeName: "koruna česká", symbol: "Kč",
		significantDecimalDigits: 2,
		decimalSeparator: ",",
		groupSeparator: "\u00a0", groupSizes: new byte[] { 3 },
		positivePattern: 3, negativePattern: 8
	)]
	CZK = 203,

	/// <summary>
	/// Djibouti Franc
	/// </summary>
	[CanonicalCulture("fr-DJ", Overwritten = true)]
	[Info(
		englishName: "Djibouti Franc", nativeName: "Franc djiboutien", symbol: "Fdj",
		significantDecimalDigits: 0,
		decimalSeparator: ",",
		groupSeparator: " ", groupSizes: new byte[] { 3 },
		positivePattern: 3, negativePattern: 8
	)]
	DJF = 262,

	/// <summary>
	/// Danish Krone
	/// </summary>
	[CanonicalCulture("da-DK")]
	[Info(
		englishName: "Danish Krone", nativeName: "dansk krone", symbol: "kr.",
		significantDecimalDigits: 2,
		decimalSeparator: ",",
		groupSeparator: ".", groupSizes: new byte[] { 3 },
		positivePattern: 3, negativePattern: 8
	)]
	DKK = 208,

	/// <summary>
	/// Dominican Peso
	/// </summary>
	[CanonicalCulture("es-DO")]
	[Info(
		englishName: "Dominican Peso", nativeName: "peso dominicano", symbol: "RD$",
		significantDecimalDigits: 2,
		decimalSeparator: ".",
		groupSeparator: ",", groupSizes: new byte[] { 3 },
		positivePattern: 0, negativePattern: 1
	)]
	DOP = 214,

	/// <summary>
	/// Algerian Dinar
	/// </summary>
	[CanonicalCulture("ar-DZ")]
	[Info(
		englishName: "Algerian Dinar", nativeName: "دينار جزائري", symbol: "د.ج.",
		significantDecimalDigits: 2,
		decimalSeparator: ",",
		groupSeparator: ".", groupSizes: new byte[] { 3 },
		positivePattern: 2, negativePattern: 9
	)]
	DZD = 012,

	/// <summary>
	/// Estonian Kroon
	/// </summary>
	[CanonicalCulture("et-EE", Overwritten = true), Obsolete("deprecated in favor of EUR")]
	[Info(
		englishName: "Estonian Kroon", nativeName: "Kroon", symbol: "kr",
		significantDecimalDigits: 2,
		decimalSeparator: ".",
		groupSeparator: "\u00a0", groupSizes: new byte[] { 3 },
		positivePattern: 3, negativePattern: 8,
		isObsolete: true
	)]
	EEK = 233,

	/// <summary>
	/// Egyptian Pound
	/// </summary>
	[CanonicalCulture("ar-EG")]
	[Info(
		englishName: "Egyptian Pound", nativeName: "جنيه مصري", symbol: "ج.م.",
		significantDecimalDigits: 2,
		decimalSeparator: "٫",
		groupSeparator: "٬", groupSizes: new byte[] { 3 },
		positivePattern: 3, negativePattern: 8
	)]
	EGP = 818,

	/// <summary>
	/// Nakfa
	/// </summary>
	[CanonicalCulture("ti-ER", Overwritten = true)]
	[Info(
		englishName: "Nakfa", nativeName: "ናቕፋ", symbol: "Nfk",
		significantDecimalDigits: 2,
		decimalSeparator: ".",
		groupSeparator: ",", groupSizes: new byte[] { 3 },
		positivePattern: 0, negativePattern: 1
	)]
	ERN = 232,

	/// <summary>
	/// Ethiopian Birr
	/// </summary>
	[CanonicalCulture("am-ET", Overwritten = true)]
	[Info(
		englishName: "Ethiopian Birr", nativeName: "የኢትዮጵያ ብር", symbol: "ብር",
		significantDecimalDigits: 2,
		decimalSeparator: ".",
		groupSeparator: ",", groupSizes: new byte[] { 3 },
		positivePattern: 0, negativePattern: 1
	)]
	ETB = 230,

	/// <summary>
	/// Euro
	/// </summary>
	[CanonicalCulture("de-DE")]
	[Info(
		englishName: "Euro", nativeName: "Euro", symbol: "€",
		significantDecimalDigits: 2,
		decimalSeparator: ",",
		groupSeparator: ".", groupSizes: new byte[] { 3 },
		positivePattern: 3, negativePattern: 8,
		codePoint: 8364, entityName: "euro"
	)]
	EUR = 978,

	/// <summary>
	/// Fiji Dollar
	/// </summary>
	[CanonicalCulture("en-FJ")]
	[Info(
		englishName: "Fiji Dollar", nativeName: "Fijian Dollar", symbol: "$",
		significantDecimalDigits: 2,
		decimalSeparator: ".",
		groupSeparator: ",", groupSizes: new byte[] { 3 },
		positivePattern: 0, negativePattern: 1,
		codePoint: 36, entityName: "dollar"
	)]
	FJD = 242,

	/// <summary>
	/// Falkland Islands Pound
	/// </summary>
	[CanonicalCulture("en-FK")]
	[Info(
		englishName: "Falkland Islands Pound", nativeName: "Falkland Islands Pound", symbol: "£",
		significantDecimalDigits: 2,
		decimalSeparator: ".",
		groupSeparator: ",", groupSizes: new byte[] { 3 },
		positivePattern: 0, negativePattern: 1,
		codePoint: 163, entityName: "pound"
	)]
	FKP = 238,

	/// <summary>
	/// Pound Sterling
	/// </summary>
	[CanonicalCulture("en-GB", Overwritten = true)]
	[Info(
		englishName: "Pound Sterling", nativeName: "Pound Sterling", symbol: "£",
		significantDecimalDigits: 2,
		decimalSeparator: ".",
		groupSeparator: ",", groupSizes: new byte[] { 3 },
		positivePattern: 0, negativePattern: 1,
		codePoint: 163, entityName: "pound"
	)]
	GBP = 826,

	/// <summary>
	/// Lari
	/// </summary>
	[CanonicalCulture("ka-GE")]
	[Info(
		englishName: "Lari", nativeName: "ქართული ლარი", symbol: "₾",
		significantDecimalDigits: 2,
		decimalSeparator: ",",
		groupSeparator: "\u00a0", groupSizes: new byte[] { 3 },
		positivePattern: 3, negativePattern: 8
	)]
	GEL = 981,

	/// <summary>
	/// Ghana Cedi
	/// </summary>
	[CanonicalCulture("en-GH")]
	[Info(
		englishName: "Ghana Cedi", nativeName: "Ghanaian cedi", symbol: "GH₵",
		significantDecimalDigits: 2,
		decimalSeparator: ".",
		groupSeparator: ",", groupSizes: new byte[] { 3 },
		positivePattern: 0, negativePattern: 1,
		codePoint: 8373
	)]
	GHS = 936,

	/// <summary>
	/// Gibraltar Pound
	/// </summary>
	[CanonicalCulture("en-GI")]
	[Info(
		englishName: "Gibraltar Pound", nativeName: "Gibraltar Pound", symbol: "£",
		significantDecimalDigits: 2,
		decimalSeparator: ".",
		groupSeparator: ",", groupSizes: new byte[] { 3 },
		positivePattern: 0, negativePattern: 1,
		codePoint: 163, entityName: "pound"
	)]
	GIP = 292,

	/// <summary>
	/// Dalasi
	/// </summary>
	[CanonicalCulture("en-GM")]
	[Info(
		englishName: "Dalasi", nativeName: "Gambian Dalasi", symbol: "D",
		significantDecimalDigits: 2,
		decimalSeparator: ".",
		groupSeparator: ",", groupSizes: new byte[] { 3 },
		positivePattern: 0, negativePattern: 1
	)]
	GMD = 270,

	/// <summary>
	/// Guinean Franc
	/// </summary>
	[CanonicalCulture("fr-GN")]
	[Info(
		englishName: "Guinean Franc", nativeName: "franc guinéen", symbol: "FG",
		significantDecimalDigits: 0,
		decimalSeparator: ",",
		groupSeparator: " ", groupSizes: new byte[] { 3 },
		positivePattern: 3, negativePattern: 8
	)]
	GNF = 324,

	/// <summary>
	/// Quetzal
	/// </summary>
	[CanonicalCulture("es-GT", Overwritten = true)]
	[Info(
		englishName: "Quetzal", nativeName: "quetzal guatemalteco", symbol: "Q",
		significantDecimalDigits: 2,
		decimalSeparator: ".",
		groupSeparator: ",", groupSizes: new byte[] { 3 },
		positivePattern: 0, negativePattern: 1
	)]
	GTQ = 320,

	/// <summary>
	/// Guyana Dollar
	/// </summary>
	[CanonicalCulture("en-GY")]
	[Info(
		englishName: "Guyana Dollar", nativeName: "Guyanaese Dollar", symbol: "$",
		significantDecimalDigits: 2,
		decimalSeparator: ".",
		groupSeparator: ",", groupSizes: new byte[] { 3 },
		positivePattern: 0, negativePattern: 1,
		codePoint: 36, entityName: "dollar"
	)]
	GYD = 328,

	/// <summary>
	/// Hong Kong Dollar
	/// </summary>
	[CanonicalCulture("zh-HK")]
	[Info(
		englishName: "Hong Kong Dollar", nativeName: "港元", symbol: "HK$",
		significantDecimalDigits: 2,
		decimalSeparator: ".",
		groupSeparator: ",", groupSizes: new byte[] { 3 },
		positivePattern: 0, negativePattern: 1
	)]
	HKD = 344,

	/// <summary>
	/// Lempira
	/// </summary>
	[CanonicalCulture("es-HN")]
	[Info(
		englishName: "Lempira", nativeName: "lempira hondureño", symbol: "L",
		significantDecimalDigits: 2,
		decimalSeparator: ".",
		groupSeparator: ",", groupSizes: new byte[] { 3 },
		positivePattern: 0, negativePattern: 1
	)]
	HNL = 340,

	/// <summary>
	/// Croatian Kuna
	/// </summary>
	[CanonicalCulture("hr-HR", Overwritten = true), Obsolete("deprecated in favor of EUR")]
	[Info(
		englishName: "Kuna", nativeName: "hrvatska kuna", symbol: "kn",
		significantDecimalDigits: 2,
		decimalSeparator: ",",
		groupSeparator: ".", groupSizes: new byte[] { 3 },
		positivePattern: 3, negativePattern: 8,
		isObsolete: true
	)]
	HRK = 191,

	/// <summary>
	/// Gourde
	/// </summary>
	[CanonicalCulture("fr-HT")]
	[Info(
		englishName: "Gourde", nativeName: "gourde haïtienne", symbol: "G",
		significantDecimalDigits: 2,
		decimalSeparator: ",",
		groupSeparator: " ", groupSizes: new byte[] { 3 },
		positivePattern: 3, negativePattern: 8
	)]
	HTG = 332,

	/// <summary>
	/// Forint
	/// </summary>
	[CanonicalCulture("hu-HU", Overwritten = true)]
	[Info(
		englishName: "Forint", nativeName: "Magyar forint", symbol: "Ft",
		significantDecimalDigits: 2,
		decimalSeparator: ",",
		groupSeparator: "\u00a0", groupSizes: new byte[] { 3 },
		positivePattern: 3, negativePattern: 8
	)]
	HUF = 348,

	/// <summary>
	/// Rupiah
	/// </summary>
	[CanonicalCulture("id-ID")]
	[Info(
		englishName: "Rupiah", nativeName: "Rupiah Indonesia", symbol: "Rp",
		significantDecimalDigits: 2,
		decimalSeparator: ",",
		groupSeparator: ".", groupSizes: new byte[] { 3 },
		positivePattern: 0, negativePattern: 1,
		codePoint: 8377
	)]
	IDR = 360,

	/// <summary>
	/// New Israeli Sheqel
	/// </summary>
	[CanonicalCulture("he-IL")]
	[Info(
		englishName: "New Israeli Sheqel", nativeName: "שקל חדש", symbol: "₪",
		significantDecimalDigits: 2,
		decimalSeparator: ".",
		groupSeparator: ",", groupSizes: new byte[] { 3 },
		positivePattern: 3, negativePattern: 8,
		codePoint: 8362
	)]
	ILS = 376,

	/// <summary>
	/// Indian Rupee
	/// </summary>
	[CanonicalCulture("hi-IN")]
	[Info(
		englishName: "Indian Rupee", nativeName: "भारतीय रुपया", symbol: "₹",
		significantDecimalDigits: 2,
		decimalSeparator: ".",
		groupSeparator: ",", groupSizes: new byte[] { 3, 2 },
		positivePattern: 0, negativePattern: 1,
		codePoint: 8377
	)]
	INR = 356,

	/// <summary>
	/// Iraqi Dinar
	/// </summary>
	[CanonicalCulture("ar-IQ", Overwritten = true)]
	[Info(
		englishName: "Iraqi Dinar", nativeName: "دينار عراقي", symbol: "د.ع.",
		significantDecimalDigits: 3,
		decimalSeparator: "٫",
		groupSeparator: "٬", groupSizes: new byte[] { 3 },
		positivePattern: 3, negativePattern: 8
	)]
	IQD = 368,

	/// <summary>
	/// Iranian Rial
	/// </summary>
	[CanonicalCulture("fa-IR", Overwritten = true)]
	[Info(
		englishName: "Iranian Rial", nativeName: "ریال ایران", symbol: "ريال",
		significantDecimalDigits: 2,
		decimalSeparator: "٫",
		groupSeparator: "٬", groupSizes: new byte[] { 3 },
		positivePattern: 0, negativePattern: 1
	)]
	IRR = 364,

	/// <summary>
	/// Iceland Krona
	/// </summary>
	[CanonicalCulture("is-IS", Overwritten = true)]
	[Info(
		englishName: "Iceland Krona", nativeName: "króna", symbol: "kr",
		significantDecimalDigits: 0,
		decimalSeparator: ",",
		groupSeparator: ".", groupSizes: new byte[] { 3 },
		positivePattern: 3, negativePattern: 8
	)]
	ISK = 352,

	/// <summary>
	/// Jamaican Dollar
	/// </summary>
	[CanonicalCulture("en-JM")]
	[Info(
		englishName: "Jamaican Dollar", nativeName: "Jamaican Dollar", symbol: "$",
		significantDecimalDigits: 2,
		decimalSeparator: ".",
		groupSeparator: ",", groupSizes: new byte[] { 3 },
		positivePattern: 0, negativePattern: 1,
		codePoint: 36, entityName: "dollar"
	)]
	JMD = 388,

	/// <summary>
	/// Jordanian Dinar
	/// </summary>
	[CanonicalCulture("ar-JO")]
	[Info(
		englishName: "Jordanian Dinar", nativeName: "دينار أردني", symbol: "د.أ.",
		significantDecimalDigits: 3,
		decimalSeparator: "٫",
		groupSeparator: "٬", groupSizes: new byte[] { 3 },
		positivePattern: 3, negativePattern: 8
	)]
	JOD = 400,

	/// <summary>
	/// Yen
	/// </summary>
	[CanonicalCulture("ja-JP")]
	[Info(
		englishName: "Yen", nativeName: "日本円", symbol: "¥",
		significantDecimalDigits: 0,
		decimalSeparator: ".",
		groupSeparator: ",", groupSizes: new byte[] { 3 },
		positivePattern: 0, negativePattern: 1,
		codePoint: 165, entityName: "yen"
	)]
	JPY = 392,

	/// <summary>
	/// Kenyan Shilling
	/// </summary>
	[CanonicalCulture("sw-KE", Overwritten = true)]
	[Info(
		englishName: "Kenyan Shilling", nativeName: "Shilingi ya Kenya", symbol: "KSh",
		significantDecimalDigits: 2,
		decimalSeparator: ".",
		groupSeparator: ",", groupSizes: new byte[] { 3 },
		positivePattern: 2, negativePattern: 9
	)]
	KES = 404,

	/// <summary>
	/// Som
	/// </summary>
	[CanonicalCulture("ky-KG")]
	[Info(
		englishName: "Som", nativeName: "Кыргызстан сому", symbol: "сом",
		significantDecimalDigits: 2,
		decimalSeparator: ",",
		groupSeparator: "\u00a0", groupSizes: new byte[] { 3 },
		positivePattern: 3, negativePattern: 8
	)]
	KGS = 417,

	/// <summary>
	/// Riel
	/// </summary>
	[CanonicalCulture("km-KH")]
	[Info(
		englishName: "Riel", nativeName: "រៀលកម្ពុជា", symbol: "៛",
		significantDecimalDigits: 2,
		decimalSeparator: ",",
		groupSeparator: ".", groupSizes: new byte[] { 3 },
		positivePattern: 1, negativePattern: 5,
		codePoint: 6107
	)]
	KHR = 116,

	/// <summary>
	/// Comorian Franc
	/// </summary>
	[CanonicalCulture("ar-KM", Overwritten = true)]
	[Info(
		englishName: "Comorian Franc", nativeName: "فرنك قمري", symbol: "CF",
		significantDecimalDigits: 0,
		decimalSeparator: "٫",
		groupSeparator: "٬", groupSizes: new byte[] { 3 },
		positivePattern: 3, negativePattern: 8
	)]
	KMF = 174,

	/// <summary>
	/// North Korean Won
	/// </summary>
	[CanonicalCulture("ko-KP", Overwritten = true)]
	[Info(
		englishName: "North Korean Won", nativeName: "조선 민주주의 인민 공화국 원", symbol: "₩",
		significantDecimalDigits: 2,
		decimalSeparator: ".",
		groupSeparator: ",", groupSizes: new byte[] { 3 },
		positivePattern: 0, negativePattern: 1,
		codePoint: 8361
	)]
	KPW = 408,

	/// <summary>
	/// Won
	/// </summary>
	[CanonicalCulture("ko-KR")]
	[Info(
		englishName: "Won", nativeName: "대한민국 원", symbol: "₩",
		significantDecimalDigits: 0,
		decimalSeparator: ".",
		groupSeparator: ",", groupSizes: new byte[] { 3 },
		positivePattern: 0, negativePattern: 1,
		codePoint: 8361
	)]
	KRW = 410,

	/// <summary>
	/// Kuwaiti Dinar
	/// </summary>
	[CanonicalCulture("ar-KW")]
	[Info(
		englishName: "Kuwaiti Dinar", nativeName: "دينار كويتي", symbol: "د.ك.",
		significantDecimalDigits: 3,
		decimalSeparator: "٫",
		groupSeparator: "٬", groupSizes: new byte[] { 3 },
		positivePattern: 3, negativePattern: 8
	)]
	KWD = 414,

	/// <summary>
	/// Cayman Islands Dollar
	/// </summary>
	[CanonicalCulture("en-KY")]
	[Info(
		englishName: "Cayman Islands Dollar", nativeName: "Cayman Islands Dollar", symbol: "$",
		significantDecimalDigits: 2,
		decimalSeparator: ".",
		groupSeparator: ",", groupSizes: new byte[] { 3 },
		positivePattern: 0, negativePattern: 1,
		codePoint: 36, entityName: "dollar"
	)]
	KYD = 136,

	/// <summary>
	/// Tenge
	/// </summary>
	[CanonicalCulture("kk-KZ", Overwritten = true)]
	[Info(
		englishName: "Tenge", nativeName: "Қазақстан теңгесі", symbol: "₸",
		significantDecimalDigits: 2,
		decimalSeparator: ",",
		groupSeparator: "\u00a0", groupSizes: new byte[] { 3 },
		positivePattern: 3, negativePattern: 8,
		codePoint: 8376
	)]
	KZT = 398,

	/// <summary>
	/// Lao Kip
	/// </summary>
	[CanonicalCulture("lo-LA", Overwritten = true)]
	[Info(
		englishName: "Lao Kip", nativeName: "ເງີນກີບລາວ", symbol: "₭",
		significantDecimalDigits: 2,
		decimalSeparator: ",",
		groupSeparator: ".", groupSizes: new byte[] { 3 },
		positivePattern: 0, negativePattern: 2,
		codePoint: 8365
	)]
	LAK = 418,

	/// <summary>
	/// Lebanese Pound
	/// </summary>
	[CanonicalCulture("ar-LB", Overwritten = true)]
	[Info(
		englishName: "Lebanese Pound", nativeName: "ليرة لبناني", symbol: "ل.ل.",
		significantDecimalDigits: 2,
		decimalSeparator: "٫",
		groupSeparator: "٬", groupSizes: new byte[] { 3 },
		positivePattern: 3, negativePattern: 8
	)]
	LBP = 422,

	/// <summary>
	/// Sri Lanka Rupee
	/// </summary>
	[CanonicalCulture("si-LK")]
	[Info(
		englishName: "Sri Lanka Rupee", nativeName: "ශ්‍රී ලංකා රුපියල", symbol: "රු.",
		significantDecimalDigits: 2,
		decimalSeparator: ".",
		groupSeparator: ",", groupSizes: new byte[] { 3 },
		positivePattern: 0, negativePattern: 1
	)]
	LKR = 144,

	/// <summary>
	/// Liberian Dollar
	/// </summary>
	[CanonicalCulture("en-LR")]
	[Info(
		englishName: "Liberian Dollar", nativeName: "Liberian Dollar", symbol: "$",
		significantDecimalDigits: 2,
		decimalSeparator: ".",
		groupSeparator: ",", groupSizes: new byte[] { 3 },
		positivePattern: 0, negativePattern: 1,
		codePoint: 36, entityName: "dollar"
	)]
	LRD = 430,

	/// <summary>
	/// Loti
	/// </summary>
	[CanonicalCulture("en-LS", Overwritten = true)]
	[Info(
		englishName: "Loti", nativeName: "Loti", symbol: "L",
		significantDecimalDigits: 2,
		decimalSeparator: ".",
		groupSeparator: ",", groupSizes: new byte[] { 3 },
		positivePattern: 0, negativePattern: 0
	)]
	LSL = 426,

	/// <summary>
	/// Lithuanian Litas
	/// </summary>
	[CanonicalCulture("lt-LT", Overwritten = true), Obsolete("deprecated in favor of EUR")]
	[Info(
		englishName: "Lithuanian Litas", nativeName: "Litas", symbol: "Lt",
		significantDecimalDigits: 2,
		decimalSeparator: ",",
		groupSeparator: "\u00a0", groupSizes: new byte[] { 3 },
		positivePattern: 3, negativePattern: 8,
		isObsolete: true
	)]
	LTL = 440,

	/// <summary>
	/// Latvian Lats
	/// </summary>
	[CanonicalCulture("lv-LV", Overwritten = true), Obsolete("deprecated in favor of EUR")]
	[Info(
		englishName: "Latvian Lats", nativeName: "Lats", symbol: "Ls",
		significantDecimalDigits: 2,
		decimalSeparator: ",",
		groupSeparator: "\u00a0", groupSizes: new byte[] { 3 },
		positivePattern: 2, negativePattern: 9,
		isObsolete: true
	)]
	LVL = 428,

	/// <summary>
	/// Libyan Dinar
	/// </summary>
	[CanonicalCulture("ar-LY")]
	[Info(
		englishName: "Libyan Dinar", nativeName: "دينار ليبي", symbol: "د.ل.",
		significantDecimalDigits: 3,
		decimalSeparator: ",",
		groupSeparator: ".", groupSizes: new byte[] { 3 },
		positivePattern: 2, negativePattern: 9
	)]
	LYD = 434,

	/// <summary>
	/// Moroccan Dirham
	/// </summary>
	[CanonicalCulture("ar-MA")]
	[Info(
		englishName: "Moroccan Dirham", nativeName: "درهم مغربي", symbol: "د.م.",
		significantDecimalDigits: 2,
		decimalSeparator: ",",
		groupSeparator: ".", groupSizes: new byte[] { 3 },
		positivePattern: 2, negativePattern: 9
	)]
	MAD = 504,

	/// <summary>
	/// Moldovan Leu
	/// </summary>
	[CanonicalCulture("ro-MD")]
	[Info(
		englishName: "Moldovan Leu", nativeName: "leu moldovenesc", symbol: "L",
		significantDecimalDigits: 2,
		decimalSeparator: ",",
		groupSeparator: ".", groupSizes: new byte[] { 3 },
		positivePattern: 3, negativePattern: 8
	)]
	MDL = 498,

	/// <summary>
	/// Malagasy Ariary
	/// </summary>
	[CanonicalCulture("mg-MG", Overwritten = true)]
	[Info(
		englishName: "Malagasy Ariary", nativeName: "Ariary", symbol: "Ar",
		significantDecimalDigits: 2,
		decimalSeparator: ".",
		groupSeparator: ",", groupSizes: new byte[] { 3 },
		positivePattern: 2, negativePattern: 9
	)]
	MGA = 969,

	/// <summary>
	/// Denar
	/// </summary>
	[CanonicalCulture("mk-MK", Overwritten = true)]
	[Info(
		englishName: "Denar", nativeName: "денар", symbol: "ден.",
		significantDecimalDigits: 2,
		decimalSeparator: ",",
		groupSeparator: ".", groupSizes: new byte[] { 3 },
		positivePattern: 3, negativePattern: 8
	)]
	MKD = 807,

	/// <summary>
	/// Kyat
	/// </summary>
	[CanonicalCulture("my-MM", Overwritten = true)]
	[Info(
		englishName: "Kyat", nativeName: "ကျပ်ငွေ", symbol: "K",
		significantDecimalDigits: 0,
		decimalSeparator: ".",
		groupSeparator: ",", groupSizes: new byte[] { 3 },
		positivePattern: 3, negativePattern: 8
	)]
	MMK = 104,

	/// <summary>
	/// Tugrik
	/// </summary>
	[CanonicalCulture("mn-MN")]
	[Info(
		englishName: "Tugrik", nativeName: "Монгол төгрөг", symbol: "₮",
		significantDecimalDigits: 2,
		decimalSeparator: ".",
		groupSeparator: ",", groupSizes: new byte[] { 3 },
		positivePattern: 2, negativePattern: 9,
		codePoint: 8366
	)]
	MNT = 496,

	/// <summary>
	/// Pataca
	/// </summary>
	[CanonicalCulture("zh-MO", Overwritten = true)]
	[Info(
		englishName: "Pataca", nativeName: "澳門元", symbol: "$",
		significantDecimalDigits: 2,
		decimalSeparator: ".",
		groupSeparator: ",", groupSizes: new byte[] { 3 },
		positivePattern: 0, negativePattern: 1
	)]
	MOP = 446,

	/// <summary>
	/// Ouguiya
	/// </summary>
	[CanonicalCulture("ar-MR", Overwritten = true), Obsolete("deprecated in favor of MRU")]
	[Info(
		englishName: "Ouguiya", nativeName: "أوقية موريتانية", symbol: "أ.م.",
		significantDecimalDigits: 2,
		decimalSeparator: "٫",
		groupSeparator: "٬", groupSizes: new byte[] { 3 },
		positivePattern: 3, negativePattern: 8,
		isObsolete: true
	)]
	MRO = 478,

	/// <summary>
	/// Ouguiya
	/// </summary>
	[CanonicalCulture("ar-MR")]
	[Info(
		englishName: "Ouguiya", nativeName: "أوقية موريتانية", symbol: "أ.م.",
		significantDecimalDigits: 2,
		decimalSeparator: "٫",
		groupSeparator: "٬", groupSizes: new byte[] { 3 },
		positivePattern: 3, negativePattern: 8
	)]
	MRU = 929,

	/// <summary>
	/// Mauritius Rupee
	/// </summary>
	[CanonicalCulture("en-MU")]
	[Info(
		englishName: "Mauritius Rupee", nativeName: "Mauritian Rupee", symbol: "Rs",
		significantDecimalDigits: 2,
		decimalSeparator: ".",
		groupSeparator: ",", groupSizes: new byte[] { 3 },
		positivePattern: 0, negativePattern: 1
	)]
	MUR = 480,

	/// <summary>
	/// Rufiyaa
	/// </summary>
	[CanonicalCulture("dv-MV", Overwritten = true)]
	[Info(
		englishName: "Rufiyaa", nativeName: "ދިވެހި ރުފިޔާ", symbol: "ރ.",
		significantDecimalDigits: 2,
		decimalSeparator: ".",
		groupSeparator: ",", groupSizes: new byte[] { 3 },
		positivePattern: 2, negativePattern: 9
	)]
	MVR = 462,

	/// <summary>
	/// Kwacha
	/// </summary>
	[CanonicalCulture("en-MW")]
	[Info(
		englishName: "Malawi Kwacha", nativeName: "Malawian Kwacha", symbol: "MK",
		significantDecimalDigits: 2,
		decimalSeparator: ".",
		groupSeparator: ",", groupSizes: new byte[] { 3 },
		positivePattern: 0, negativePattern: 1
	)]
	MWK = 454,

	/// <summary>
	/// Mexican Peso
	/// </summary>
	[CanonicalCulture("es-MX", Overwritten = true)]
	[Info(
		englishName: "Mexican Peso", nativeName: "Peso Mexicano", symbol: "$",
		significantDecimalDigits: 2,
		decimalSeparator: ".",
		groupSeparator: ",", groupSizes: new byte[] { 3 },
		positivePattern: 0, negativePattern: 1,
		codePoint: 36, entityName: "dollar"
	)]
	MXN = 484,

	/// <summary>
	/// Mexican Unidad de Inversion (UDI)
	/// </summary>
	[CanonicalCulture("es-MX", Overwritten = true)]
	[Info(
		englishName: "Mexican Unidad de Inversion (UDI)", nativeName: "Unidad de inversión (UDI)", symbol: "$",
		significantDecimalDigits: 2,
		decimalSeparator: ".",
		groupSeparator: ",", groupSizes: new byte[] { 3 },
		positivePattern: 0, negativePattern: 1,
		codePoint: 36, entityName: "dollar"
	)]
	MXV = 979,

	/// <summary>
	/// Malaysian Ringgit
	/// </summary>
	[CanonicalCulture("ms-MY")]
	[Info(
		englishName: "Malaysian Ringgit", nativeName: "Ringgit Malaysia", symbol: "RM",
		significantDecimalDigits: 2,
		decimalSeparator: ".",
		groupSeparator: ",", groupSizes: new byte[] { 3 },
		positivePattern: 0, negativePattern: 1
	)]
	MYR = 458,

	/// <summary>
	/// Mozambique Metical
	/// </summary>
	[CanonicalCulture("pt-MZ")]
	[Info(
		englishName: "Mozambique Metical", nativeName: "metical moçambicano", symbol: "MTn",
		significantDecimalDigits: 2,
		decimalSeparator: ",",
		groupSeparator: "\u00a0", groupSizes: new byte[] { 3 },
		positivePattern: 3, negativePattern: 8
	)]
	MZN = 943,

	/// <summary>
	/// Namibia Dollar
	/// </summary>
	[CanonicalCulture("en-NA")]
	[Info(
		englishName: "Namibia Dollar", nativeName: "Namibian Dollar", symbol: "$",
		significantDecimalDigits: 2,
		decimalSeparator: ".",
		groupSeparator: ",", groupSizes: new byte[] { 3 },
		positivePattern: 0, negativePattern: 1,
		codePoint: 36, entityName: "dollar"
	)]
	NAD = 516,

	/// <summary>
	/// Naira
	/// </summary>
	[CanonicalCulture("ha-Latn-NG")]
	[Info(
		englishName: "Naira", nativeName: "Nairar Najeriya", symbol: "₦",
		significantDecimalDigits: 2,
		decimalSeparator: ".",
		groupSeparator: ",", groupSizes: new byte[] { 3 },
		positivePattern: 2, negativePattern: 9,
		codePoint: 8358
	)]
	NGN = 566,

	/// <summary>
	/// Cordoba Oro
	/// </summary>
	[CanonicalCulture("es-NI")]
	[Info(
		englishName: "Cordoba Oro", nativeName: "córdoba nicaragüense", symbol: "C$",
		significantDecimalDigits: 2,
		decimalSeparator: ".",
		groupSeparator: ",", groupSizes: new byte[] { 3 },
		positivePattern: 0, negativePattern: 1
	)]
	NIO = 558,

	/// <summary>
	/// Norwegian Krone
	/// </summary>
	[CanonicalCulture("nn-NO", Overwritten = true)]
	[Info(
		englishName: "Norwegian Krone", nativeName: "Norsk krone", symbol: "kr",
		significantDecimalDigits: 2,
		decimalSeparator: ",",
		groupSeparator: "\u00a0", groupSizes: new byte[] { 3 },
		positivePattern: 3, negativePattern: 8
	)]
	NOK = 578,

	/// <summary>
	/// Nepalese Rupee
	/// </summary>
	[CanonicalCulture("ne-NP", Overwritten = true)]
	[Info(
		englishName: "Nepalese Rupee", nativeName: "रुपैयाँ", symbol: "	रु.",
		significantDecimalDigits: 2,
		decimalSeparator: ".",
		groupSeparator: ",", groupSizes: new byte[] { 3, 2 },
		positivePattern: 2, negativePattern: 9
	)]
	NPR = 524,

	/// <summary>
	/// New Zealand Dollar
	/// </summary>
	[CanonicalCulture("en-NZ")]
	[Info(
		englishName: "New Zealand Dollar", nativeName: "New Zealand Dollar", symbol: "$",
		significantDecimalDigits: 2,
		decimalSeparator: ".",
		groupSeparator: ",", groupSizes: new byte[] { 3 },
		positivePattern: 0, negativePattern: 1,
		codePoint: 36, entityName: "dollar"
	)]
	NZD = 554,

	/// <summary>
	/// Rial Omani
	/// </summary>
	[CanonicalCulture("ar-OM")]
	[Info(
		englishName: "Rial Omani", nativeName: "ريال عماني", symbol: "ر.ع.",
		significantDecimalDigits: 3,
		decimalSeparator: "٫",
		groupSeparator: "٬", groupSizes: new byte[] { 3 },
		positivePattern: 3, negativePattern: 8
	)]
	OMR = 512,

	/// <summary>
	/// Balboa
	/// </summary>
	[CanonicalCulture("es-PA")]
	[Info(
		englishName: "Balboa", nativeName: "balboa panameño", symbol: "B/.",
		significantDecimalDigits: 2,
		decimalSeparator: ".",
		groupSeparator: ",", groupSizes: new byte[] { 3 },
		positivePattern: 0, negativePattern: 1
	)]
	PAB = 590,

	/// <summary>
	/// Nuevo Sol
	/// </summary>
	[CanonicalCulture("es-PE")]
	[Info(
		englishName: "Sol", nativeName: "sol peruano", symbol: "S/",
		significantDecimalDigits: 2,
		decimalSeparator: ".",
		groupSeparator: ",", groupSizes: new byte[] { 3 },
		positivePattern: 2, negativePattern: 9
	)]
	PEN = 604,

	/// <summary>
	/// Kina
	/// </summary>
	[CanonicalCulture("en-PG")]
	[Info(
		englishName: "Kina", nativeName: "Papua New Guinean Kina", symbol: "K",
		significantDecimalDigits: 2,
		decimalSeparator: ".",
		groupSeparator: ",", groupSizes: new byte[] { 3 },
		positivePattern: 0, negativePattern: 1
	)]
	PGK = 598,

	/// <summary>
	/// Philippine Peso
	/// </summary>
	[CanonicalCulture("fil-PH")]
	[Info(
		englishName: "Philippine Peso", nativeName: "Piso ng Pilipinas", symbol: "₱",
		significantDecimalDigits: 2,
		decimalSeparator: ".",
		groupSeparator: ",", groupSizes: new byte[] { 3 },
		positivePattern: 0, negativePattern: 1,
		codePoint: 8369
	)]
	PHP = 608,

	/// <summary>
	/// Pakistan Rupee
	/// </summary>
	[CanonicalCulture("ur-PK")]
	[Info(
		englishName: "Pakistan Rupee", nativeName: "پاکستانی روپیہ", symbol: "Rs",
		significantDecimalDigits: 2,
		decimalSeparator: ".",
		groupSeparator: ",", groupSizes: new byte[] { 3 },
		positivePattern: 2, negativePattern: 9
	)]
	PKR = 586,

	/// <summary>
	/// Zloty
	/// </summary>
	[CanonicalCulture("pl-PL")]
	[Info(
		englishName: "Zloty", nativeName: "złoty polski", symbol: "zł",
		significantDecimalDigits: 2,
		decimalSeparator: ",",
		groupSeparator: "\u00a0", groupSizes: new byte[] { 3 },
		positivePattern: 3, negativePattern: 8
	)]
	PLN = 985,

	/// <summary>
	/// Guarani
	/// </summary>
	[CanonicalCulture("es-PY", Overwritten = true)]
	[Info(
		englishName: "Guarani", nativeName: "guaraní paraguayo", symbol: "₲",
		significantDecimalDigits: 0,
		decimalSeparator: ",",
		groupSeparator: ".", groupSizes: new byte[] { 3 },
		positivePattern: 2, negativePattern: 12,
		codePoint: 8370
	)]
	PYG = 600,

	/// <summary>
	/// Qatari Rial
	/// </summary>
	[CanonicalCulture("ar-QA")]
	[Info(
		englishName: "Qatari Rial", nativeName: "ريال قطري", symbol: "ر.ق.",
		significantDecimalDigits: 2,
		decimalSeparator: "٫",
		groupSeparator: "٬", groupSizes: new byte[] { 3 },
		positivePattern: 3, negativePattern: 8
	)]
	QAR = 634,

	/// <summary>
	/// New Romanian Leu
	/// </summary>
	[CanonicalCulture("ro-RO", Overwritten = true)]
	[Info(
		englishName: "Romanian Leu", nativeName: "Leu românesc", symbol: "lei",
		significantDecimalDigits: 2,
		decimalSeparator: ",",
		groupSeparator: ".", groupSizes: new byte[] { 3 },
		positivePattern: 3, negativePattern: 8
	)]
	RON = 946,

	/// <summary>
	/// Serbian Dinar
	/// </summary>
	[CanonicalCulture("sr-Latn-RS", Overwritten = true)]
	[Info(
		englishName: "Serbian Dinar", nativeName: "Srpski dinar", symbol: "RSD",
		significantDecimalDigits: 2,
		decimalSeparator: ",",
		groupSeparator: ".", groupSizes: new byte[] { 3 },
		positivePattern: 3, negativePattern: 8
	)]
	RSD = 941,

	/// <summary>
	/// Russian Ruble
	/// </summary>
	[CanonicalCulture("ru-RU")]
	[Info(
		englishName: "Russian Ruble", nativeName: "российский рубль", symbol: "₽",
		significantDecimalDigits: 2,
		decimalSeparator: ",",
		groupSeparator: "\u00a0", groupSizes: new byte[] { 3 },
		positivePattern: 3, negativePattern: 8,
		codePoint: 8381
	)]
	RUB = 643,

	/// <summary>
	/// Rwanda Franc
	/// </summary>
	[CanonicalCulture("rw-RW", Overwritten = true)]
	[Info(
		englishName: "Rwanda Franc", nativeName: "Ifaranga ry'u Rwanda", symbol: "RF",
		significantDecimalDigits: 0,
		decimalSeparator: ",",
		groupSeparator: ".", groupSizes: new byte[] { 3 },
		positivePattern: 2, negativePattern: 9
	)]
	RWF = 646,

	/// <summary>
	/// Saudi Riyal
	/// </summary>
	[CanonicalCulture("ar-SA")]
	[Info(
		englishName: "Saudi Riyal", nativeName: "ريال سعودي", symbol: "ر.س.",
		significantDecimalDigits: 2,
		decimalSeparator: "٫",
		groupSeparator: "٬", groupSizes: new byte[] { 3 },
		positivePattern: 3, negativePattern: 8
	)]
	SAR = 682,

	/// <summary>
	/// Solomon Islands Dollar
	/// </summary>
	[CanonicalCulture("en-SB")]
	[Info(
		englishName: "Solomon Islands Dollar", nativeName: "Solomon Islands Dollar", symbol: "$",
		significantDecimalDigits: 2,
		decimalSeparator: ".",
		groupSeparator: ",", groupSizes: new byte[] { 3 },
		positivePattern: 0, negativePattern: 1,
		codePoint: 36, entityName: "dollar"

	)]
	SBD = 090,

	/// <summary>
	/// Seychelles Rupee
	/// </summary>
	[CanonicalCulture("en-SC")]
	[Info(
		englishName: "Seychelles Rupee", nativeName: "Seychellois Rupee", symbol: "SR",
		significantDecimalDigits: 2,
		decimalSeparator: ".",
		groupSeparator: ",", groupSizes: new byte[] { 3 },
		positivePattern: 0, negativePattern: 1
	)]
	SCR = 690,

	/// <summary>
	/// Sudanese Pound
	/// </summary>
	[CanonicalCulture("ar-SD")]
	[Info(
		englishName: "Sudanese Pound", nativeName: "جنيه سوداني", symbol: "ج.س.",
		significantDecimalDigits: 2,
		decimalSeparator: "٫",
		groupSeparator: "٬", groupSizes: new byte[] { 3 },
		positivePattern: 3, negativePattern: 8
	)]
	SDG = 938,

	/// <summary>
	/// Swedish Krona
	/// </summary>
	[CanonicalCulture("sv-SE")]
	[Info(
		englishName: "Swedish Krona", nativeName: "svensk krona", symbol: "kr",
		significantDecimalDigits: 2,
		decimalSeparator: ",",
		groupSeparator: " ", groupSizes: new byte[] { 3 },
		positivePattern: 3, negativePattern: 8
	)]
	SEK = 752,

	/// <summary>
	/// Singapore Dollar
	/// </summary>
	[CanonicalCulture("zh-SG")]
	[Info(
		englishName: "Singapore Dollar", nativeName: "新加坡元", symbol: "$",
		significantDecimalDigits: 2,
		decimalSeparator: ".",
		groupSeparator: ",", groupSizes: new byte[] { 3 },
		positivePattern: 0, negativePattern: 1,
		codePoint: 36, entityName: "dollar"
	)]
	SGD = 702,

	/// <summary>
	/// Saint Helena Pound
	/// </summary>
	[CanonicalCulture("en-SH", Overwritten = true)]
	[Info(
		englishName: "Saint Helena Pound", nativeName: "Saint Helena Pound", symbol: "£",
		significantDecimalDigits: 2,
		decimalSeparator: ".",
		groupSeparator: ",", groupSizes: new byte[] { 3 },
		positivePattern: 0, negativePattern: 1,
		codePoint: 163, entityName: "pound"
	)]
	SHP = 654,

	/// <summary>
	/// Leone
	/// </summary>
	[CanonicalCulture("en-SL", Overwritten = true)]
	[Info(
		englishName: "Leone", nativeName: "Leone", symbol: "Le",
		significantDecimalDigits: 2,
		decimalSeparator: ".",
		groupSeparator: ",", groupSizes: new byte[] { 3 },
		positivePattern: 0, negativePattern: 1
	)]
	SLE = 925,

	/// <summary>
	/// Leone
	/// </summary>
	[CanonicalCulture("en-SL", Overwritten = true), Obsolete("deprecated in favor of SLE")]
	[Info(
		englishName: "Leone", nativeName: "Leone", symbol: "Le",
		significantDecimalDigits: 2,
		decimalSeparator: ".",
		groupSeparator: ",", groupSizes: new byte[] { 3 },
		positivePattern: 0, negativePattern: 1,
		isObsolete: true
	)]
	SLL = 694,

	/// <summary>
	/// Somali Shilling
	/// </summary>
	[CanonicalCulture("so-SO", Overwritten = true)]
	[Info(
		englishName: "Somali Shilling", nativeName: "Shilin Soomaali", symbol: "S",
		significantDecimalDigits: 2,
		decimalSeparator: ".",
		groupSeparator: ",", groupSizes: new byte[] { 3 },
		positivePattern: 0, negativePattern: 1
	)]
	SOS = 706,

	/// <summary>
	/// Surinam Dollar
	/// </summary>
	[CanonicalCulture("nl-SR")]
	[Info(
		englishName: "Surinam Dollar", nativeName: "Surinaamse dollar", symbol: "$",
		significantDecimalDigits: 2,
		decimalSeparator: ",",
		groupSeparator: ".", groupSizes: new byte[] { 3 },
		positivePattern: 2, negativePattern: 12,
		codePoint: 36, entityName: "dollar"
	)]
	SRD = 968,

	/// <summary>
	/// South Sudanese Pound
	/// </summary>
	[CanonicalCulture("en-SS")]
	[Info(
		englishName: "South Sudanese Pound", nativeName: "South Sudanese Pound", symbol: "£",
		significantDecimalDigits: 2,
		decimalSeparator: ".",
		groupSeparator: ",", groupSizes: new byte[] { 3 },
		positivePattern: 0, negativePattern: 1,
		codePoint: 163, entityName: "pound"
	)]
	SSP = 728,

	/// <summary>
	/// Dobra
	/// </summary>
	[CanonicalCulture("pt-ST", Overwritten = true), Obsolete("deprecated in favor of STN")]
	[Info(
		englishName: "Dobra", nativeName: "dobra de São Tomé e Príncipe", symbol: "Db",
		significantDecimalDigits: 2,
		decimalSeparator: ",",
		groupSeparator: "\u00a0", groupSizes: new byte[] { 3 },
		positivePattern: 3, negativePattern: 8,
		isObsolete: true
	)]
	STD = 678,

	/// <summary>
	/// Dobra
	/// </summary>
	[CanonicalCulture("pt-ST")]
	[Info(
		englishName: "Dobra", nativeName: "dobra de São Tomé e Príncipe", symbol: "Db",
		significantDecimalDigits: 2,
		decimalSeparator: ",",
		groupSeparator: "\u00a0", groupSizes: new byte[] { 3 },
		positivePattern: 3, negativePattern: 8
	)]
	STN = 930,

	/// <summary>
	/// El Salvador Colon
	/// </summary>
	[CanonicalCulture("es-SV", Overwritten = true), Obsolete("deprecated in favor of USD")]
	[Info(
		englishName: "El Salvador Colon", nativeName: "colón salvadoreño", symbol: "₡",
		significantDecimalDigits: 2,
		decimalSeparator: ".",
		groupSeparator: ",", groupSizes: new byte[] { 3, 0 },
		positivePattern: 0, negativePattern: 0,
		isObsolete: true
	)]
	SVC = 222,

	/// <summary>
	/// Syrian Pound
	/// </summary>
	[CanonicalCulture("ar-SY", Overwritten = true)]
	[Info(
		englishName: "Syrian Pound", nativeName: "ليرة سورية", symbol: "ل.س.",
		significantDecimalDigits: 2,
		decimalSeparator: "٫",
		groupSeparator: "٬", groupSizes: new byte[] { 3 },
		positivePattern: 3, negativePattern: 8
	)]
	SYP = 760,

	/// <summary>
	/// Lilangeni
	/// </summary>
	[CanonicalCulture("ss-SZ", Overwritten = true)]
	[Info(
		englishName: "Lilangeni", nativeName: "Swazi lilangeni", symbol: "E",
		significantDecimalDigits: 2,
		decimalSeparator: ",",
		groupSeparator: "\u00a0", groupSizes: new byte[] { 3 },
		positivePattern: 0, negativePattern: 1
	)]
	SZL = 748,

	/// <summary>
	/// Baht
	/// </summary>
	[CanonicalCulture("th-TH")]
	[Info(
		englishName: "Baht", nativeName: "บาท", symbol: "฿",
		significantDecimalDigits: 2,
		decimalSeparator: ".",
		groupSeparator: ",", groupSizes: new byte[] { 3 },
		positivePattern: 0, negativePattern: 1,
		codePoint: 3647
	)]
	THB = 764,

	/// <summary>
	/// Somoni
	/// </summary>
	[CanonicalCulture("tg-Cyrl-TJ", Overwritten = true)]
	[Info(
		englishName: "Somoni", nativeName: "Сомонӣ", symbol: "SM",
		significantDecimalDigits: 2,
		decimalSeparator: ",",
		groupSeparator: "\u00a0", groupSizes: new byte[] { 3 },
		positivePattern: 3, negativePattern: 8
	)]
	TJS = 972,

	/// <summary>
	/// Turkmenistan New Manat
	/// </summary>
	[CanonicalCulture("tk-TM", Overwritten = true)]
	[Info(
		englishName: "Turkmenistan New Manat", nativeName: "Türkmen manady", symbol: "m",
		significantDecimalDigits: 2,
		decimalSeparator: ",",
		groupSeparator: "\u00a0", groupSizes: new byte[] { 3 },
		positivePattern: 3, negativePattern: 8
	)]
	TMT = 934,

	/// <summary>
	/// Tunisian Dinar
	/// </summary>
	[CanonicalCulture("ar-TN")]
	[Info(
		englishName: "Tunisian Dinar", nativeName: "دينار تونسي", symbol: "د.ت.",
		significantDecimalDigits: 3,
		decimalSeparator: ",",
		groupSeparator: ".", groupSizes: new byte[] { 3 },
		positivePattern: 2, negativePattern: 9
	)]
	TND = 788,

	/// <summary>
	/// Paʻanga
	/// </summary>
	[CanonicalCulture("to-TO", Overwritten = true)]
	[Info(
		englishName: "Pa’anga", nativeName: "Tonga paʻanga", symbol: "T$",
		significantDecimalDigits: 2,
		decimalSeparator: ".",
		groupSeparator: ",", groupSizes: new byte[] { 3 },
		positivePattern: 2, negativePattern: 9
	)]
	TOP = 776,

	/// <summary>
	/// Turkish Lira
	/// </summary>
	[CanonicalCulture("tr-TR")]
	[Info(
		englishName: "Turkish Lira", nativeName: "Türk Lirası", symbol: "₺",
		significantDecimalDigits: 2,
		decimalSeparator: ",",
		groupSeparator: ".", groupSizes: new byte[] { 3 },
		positivePattern: 0, negativePattern: 1,
		codePoint: 8378
	)]
	TRY = 949,

	/// <summary>
	/// Trinidad and Tobago Dollar
	/// </summary>
	[CanonicalCulture("en-TT", Overwritten = true)]
	[Info(
		englishName: "Trinidad and Tobago Dollar", nativeName: "Trinidad and Tobago Dollar", symbol: "TT$",
		significantDecimalDigits: 2,
		decimalSeparator: ".",
		groupSeparator: ",", groupSizes: new byte[] { 3 },
		positivePattern: 0, negativePattern: 1
	)]
	TTD = 780,

	/// <summary>
	/// New Taiwan Dollar
	/// </summary>
	[CanonicalCulture("zh-TW", Overwritten = true)]
	[Info(
		englishName: "New Taiwan Dollar", nativeName: "新台幣", symbol: "NT$",
		significantDecimalDigits: 2,
		decimalSeparator: ".",
		groupSeparator: ",", groupSizes: new byte[] { 3 },
		positivePattern: 0, negativePattern: 1
	)]
	TWD = 901,

	/// <summary>
	/// Tanzanian Shilling
	/// </summary>
	[CanonicalCulture("sw-TZ")]
	[Info(
		englishName: "Tanzanian Shilling", nativeName: "Shilingi ya Tanzania", symbol: "TSh",
		significantDecimalDigits: 2,
		decimalSeparator: ".",
		groupSeparator: ",", groupSizes: new byte[] { 3 },
		positivePattern: 2, negativePattern: 9
	)]
	TZS = 834,

	/// <summary>
	/// Hryvnia
	/// </summary>
	[CanonicalCulture("uk-UA")]
	[Info(
		englishName: "Hryvnia", nativeName: "українська гривня", symbol: "₴",
		significantDecimalDigits: 2,
		decimalSeparator: ",",
		groupSeparator: "\u00a0", groupSizes: new byte[] { 3 },
		positivePattern: 3, negativePattern: 8,
		codePoint: 8372
	)]
	UAH = 980,

	/// <summary>
	/// Uganda Shilling
	/// </summary>
	[CanonicalCulture("sw-UG")]
	[Info(
		englishName: "Uganda Shilling", nativeName: "Shilingi ya Uganda", symbol: "USh",
		significantDecimalDigits: 0,
		decimalSeparator: ".",
		groupSeparator: ",", groupSizes: new byte[] { 3 },
		positivePattern: 2, negativePattern: 9
	)]
	UGX = 800,

	/// <summary>
	/// US Dollar
	/// </summary>
	[CanonicalCulture("en-US")]
	[Info(
		englishName: "US Dollar", nativeName: "US Dollar", symbol: "$",
		significantDecimalDigits: 2,
		decimalSeparator: ".",
		groupSeparator: ",", groupSizes: new byte[] { 3 },
		positivePattern: 0, negativePattern: 1,
		codePoint: 36, entityName: "dollar"
	)]
	USD = 840,

	/// <summary>
	/// US Dollar (Next day)
	/// </summary>
	[CanonicalCulture("en-US", Overwritten = true)]
	[Info(
		englishName: "US Dollar (Next day)", nativeName: "US Dollar (Next day)", symbol: "$",
		significantDecimalDigits: 2,
		decimalSeparator: ".",
		groupSeparator: ",", groupSizes: new byte[] { 3 },
		positivePattern: 0, negativePattern: 1,
		codePoint: 36, entityName: "dollar"
	)]
	USN = 997,

	/// <summary>
	/// US Dollar (Same day)
	/// </summary>
	[CanonicalCulture("en-US", Overwritten = true), Obsolete("No longer in use")]
	[Info(
		englishName: "US Dollar (Same day)", nativeName: "US Dollar (Same day)", symbol: "$",
		significantDecimalDigits: 2,
		decimalSeparator: ".",
		groupSeparator: ",", groupSizes: new byte[] { 3 },
		positivePattern: 0, negativePattern: 1,
		codePoint: 36, entityName: "dollar",
		isObsolete: true
	)]
	USS = 998,

	/// <summary>
	/// >Uruguay Peso en Unidades Indexadas (UI)
	/// </summary>
	[Info(
		englishName: "Uruguay Peso en Unidades Indexadas (UI)",
		nativeName: "Uruguay Peso en Unidades Indexadas (UI)", symbol: "$U",
		significantDecimalDigits: 0,
		decimalSeparator: ",",
		groupSeparator: ".", groupSizes: new byte[] { 3 },
		positivePattern: 2, negativePattern: 9
	)]
	UYI = 940,

	/// <summary>
	/// Peso Uruguayo
	/// </summary>
	[CanonicalCulture("es-UY", Overwritten = true)]
	[Info(
		englishName: "Uruguayan Peso", nativeName: "peso uruguayo", symbol: "$U",
		significantDecimalDigits: 2,
		decimalSeparator: ",",
		groupSeparator: ".", groupSizes: new byte[] { 3 },
		positivePattern: 2, negativePattern: 9
	)]
	UYU = 858,

	/// <summary>
	/// Unidad Previsional
	/// </summary>
	[Info(
		englishName: "Unidad Previsional", nativeName: "Unidad Previsional", symbol: "$U",
		significantDecimalDigits: 4,
		decimalSeparator: ",",
		groupSeparator: ".", groupSizes: new byte[] { 3 },
		positivePattern: 2, negativePattern: 9
	)]
	UYW = 927,

	/// <summary>
	/// Uzbekistan Sum
	/// </summary>
	[CanonicalCulture("uz-Latn-UZ", Overwritten = true)]
	[Info(
		englishName: "Uzbekistan Sum", nativeName: "Oʻzbek soʻmi", symbol: "soʻm",
		significantDecimalDigits: 2,
		decimalSeparator: ",",
		groupSeparator: "\u00a0", groupSizes: new byte[] { 3 },
		positivePattern: 3, negativePattern: 8
	)]
	UZS = 860,

	/// <summary>
	/// Bolívar Soberano
	/// </summary>
	[CanonicalCulture("es-VE", Overwritten = true)]
	[Info(
		englishName: "Bolívar Soberano", nativeName: "bolívar digital soberano", symbol: "Bs.D",
		significantDecimalDigits: 2,
		decimalSeparator: ",",
		groupSeparator: ".", groupSizes: new byte[] { 3 },
		positivePattern: 0, negativePattern: 2
	)]
	VED = 926,

	/// <summary>
	/// Bolivar
	/// </summary>
	[CanonicalCulture("es-VE", Overwritten = true), Obsolete("deprecated in favor of VES")]
	[Info(
		englishName: "Bolívar", nativeName: "bolívar", symbol: "Bs.",
		significantDecimalDigits: 2,
		decimalSeparator: ",",
		groupSeparator: ".", groupSizes: new byte[] { 3 },
		positivePattern: 0, negativePattern: 2,
		isObsolete: true
	)]
	VEF = 937,

	/// <summary>
	/// Bolívar Soberano
	/// </summary>
	[CanonicalCulture("es-VE", Overwritten = true)]
	[Info(
		englishName: "Bolívar Soberano", nativeName: "bolívar soberano", symbol: "Bs.",
		significantDecimalDigits: 2,
		decimalSeparator: ",",
		groupSeparator: ".", groupSizes: new byte[] { 3 },
		positivePattern: 0, negativePattern: 2
	)]
	VES = 928,

	/// <summary>
	/// Dong
	/// </summary>
	[CanonicalCulture("vi-VN")]
	[Info(
		englishName: "Dong", nativeName: "Đồng Việt Nam", symbol: "₫",
		significantDecimalDigits: 0,
		decimalSeparator: ",",
		groupSeparator: ".", groupSizes: new byte[] { 3 },
		positivePattern: 3, negativePattern: 8,
		codePoint: 8363
	)]
	VND = 704,

	/// <summary>
	/// Vatu
	/// </summary>
	[CanonicalCulture("fr-VU")]
	[Info(
		englishName: "Vatu", nativeName: "vatu vanuatuan", symbol: "VT",
		significantDecimalDigits: 0,
		decimalSeparator: ",",
		groupSeparator: " ", groupSizes: new byte[] { 3 },
		positivePattern: 3, negativePattern: 8
	)]
	VUV = 548,

	/// <summary>
	/// Tala
	/// </summary>
	[CanonicalCulture("en-WS", Overwritten = true)]
	[Info(
		englishName: "Tala", nativeName: "Samoa tālā", symbol: "WS$",
		significantDecimalDigits: 2,
		decimalSeparator: ".",
		groupSeparator: ",", groupSizes: new byte[] { 3 },
		positivePattern: 0, negativePattern: 1
	)]
	WST = 882,

	/// <summary>
	/// CFA Franc BEAC
	/// </summary>
	[CanonicalCulture("fr-CM")]
	[Info(
		englishName: "CFA Franc BEAC", nativeName: "franc CFA (BEAC)", symbol: "FCFA",
		significantDecimalDigits: 0,
		decimalSeparator: ",",
		groupSeparator: " ", groupSizes: new byte[] { 3 },
		positivePattern: 3, negativePattern: 8
	)]
	XAF = 950,

	/// <summary>
	/// Silver
	/// </summary>
	[Info(
		englishName: "Silver", nativeName: "Silver", symbol: "Ag",
		significantDecimalDigits: 2,
		decimalSeparator: ".",
		groupSeparator: ",", groupSizes: new byte[] { 3 },
		positivePattern: 0, negativePattern: 0
	)]
	XAG = 961,

	/// <summary>
	/// Gold
	/// </summary>
	[Info(
		englishName: "Gold", nativeName: "Gold", symbol: "Au",
		significantDecimalDigits: 2,
		decimalSeparator: ".",
		groupSeparator: ",", groupSizes: new byte[] { 3 },
		positivePattern: 0, negativePattern: 0
	)]
	XAU = 959,

	/// <summary>
	/// European Composite Unit (EURCO)
	/// </summary>
	[Info(
		englishName: "European Composite Unit (EURCO)", nativeName: "European Composite Unit (EURCO)", symbol: "¤",
		significantDecimalDigits: 2,
		decimalSeparator: ".",
		groupSeparator: ",", groupSizes: new byte[] { 3 },
		positivePattern: 0, negativePattern: 0,
		codePoint: 164, entityName: "curren"
	)]
	XBA = 955,

	/// <summary>
	/// European Monetary Unit (E.M.U.-6)
	/// </summary>
	[Info(
		englishName: "European Monetary Unit (E.M.U.-6)", nativeName: "European Monetary Unit (E.M.U.-6)", symbol: "¤",
		significantDecimalDigits: 2,
		decimalSeparator: ".",
		groupSeparator: ",", groupSizes: new byte[] { 3 },
		positivePattern: 0, negativePattern: 0,
		codePoint: 164, entityName: "curren"
	)]
	XBB = 956,

	/// <summary>
	/// European Unit of Account 9 (E.U.A.-9)
	/// </summary>
	[Info(
		englishName: "European Unit of Account 9 (E.U.A.-9)", nativeName: "European Unit of Account 9 (E.U.A.-9)",
		symbol: "¤",
		significantDecimalDigits: 2,
		decimalSeparator: ".",
		groupSeparator: ",", groupSizes: new byte[] { 3 },
		positivePattern: 0, negativePattern: 0,
		codePoint: 164, entityName: "curren"
	)]
	XBC = 957,

	/// <summary>
	/// European Unit of Account 17 (E.U.A.-17)
	/// </summary>
	[Info(
		englishName: "European Unit of Account 17 (E.U.A.-17)", nativeName: "European Unit of Account 17 (E.U.A.-17)",
		symbol: "¤",
		significantDecimalDigits: 2,
		decimalSeparator: ".",
		groupSeparator: ",", groupSizes: new byte[] { 3 },
		positivePattern: 0, negativePattern: 0,
		codePoint: 164, entityName: "curren"
	)]
	XBD = 958,

	/// <summary>
	/// East Caribbean Dollar
	/// </summary>
	[CanonicalCulture("en-029")]
	[Info(
		englishName: "East Caribbean Dollar", nativeName: "East Caribbean Dollar", symbol: "EC$",
		significantDecimalDigits: 2,
		decimalSeparator: ".",
		groupSeparator: ",", groupSizes: new byte[] { 3 },
		positivePattern: 0, negativePattern: 1
	)]
	XCD = 951,

	/// <summary>
	/// SDR (Special Drawing Right)
	/// </summary>
	[Info(
		englishName: "SDR (Special Drawing Right)", nativeName: "SDR (Special Drawing Right)", symbol: "¤",
		significantDecimalDigits: 2,
		decimalSeparator: ".",
		groupSeparator: ",", groupSizes: new byte[] { 3 },
		positivePattern: 0, negativePattern: 0,
		codePoint: 164, entityName: "curren"
	)]
	XDR = 960,

	/// <summary>
	/// CFA Franc BCEAO
	/// </summary>
	[CanonicalCulture("wo-SN")]
	[Info(
		englishName: "CFA Franc BCEAO", nativeName: "Franc CFA bu Afrik Sowwu-jant", symbol: "CFA",
		significantDecimalDigits: 0,
		decimalSeparator: ",",
		groupSeparator: ".", groupSizes: new byte[] { 3 },
		positivePattern: 2, negativePattern: 9
	)]
	XOF = 952,

	/// <summary>
	/// Palladium
	/// </summary>
	[Info(
		englishName: "Palladium", nativeName: "Palladium", symbol: "Pd",
		significantDecimalDigits: 2,
		decimalSeparator: ".",
		groupSeparator: ",", groupSizes: new byte[] { 3 },
		positivePattern: 0, negativePattern: 0
	)]
	XPD = 964,

	/// <summary>
	/// CFP Franc
	/// </summary>
	[CanonicalCulture("fr-NC", Overwritten = true)]
	[Info(
		englishName: "CFP Franc", nativeName: "franc pacifique", symbol: "F",
		significantDecimalDigits: 0,
		decimalSeparator: ",",
		groupSeparator: " ", groupSizes: new byte[] { 3 },
		positivePattern: 3, negativePattern: 8
	)]
	XPF = 953,

	/// <summary>
	/// Platinum
	/// </summary>
	[Info(
		englishName: "Platinum", nativeName: "Platinum", symbol: "¤",
		significantDecimalDigits: 2,
		decimalSeparator: ".",
		groupSeparator: ",", groupSizes: new byte[] { 3 },
		positivePattern: 0, negativePattern: 0,
		codePoint: 164, entityName: "curren"
	)]
	XPT = 962,

	/// <summary>
	/// Sucre
	/// </summary>
	[Info(
		englishName: "Sucre", nativeName: "Sucre", symbol: "¤",
		significantDecimalDigits: 2,
		decimalSeparator: ".",
		groupSeparator: ",", groupSizes: new byte[] { 3 },
		positivePattern: 0, negativePattern: 0,
		codePoint: 164, entityName: "curren"
	)]
	XSU = 994,

	/// <summary>
	/// Test currency
	/// </summary>
	[Info(
		englishName: "Test currency", nativeName: "Test currency", symbol: "¤",
		significantDecimalDigits: 2,
		decimalSeparator: ".",
		groupSeparator: ",", groupSizes: new byte[] { 3 },
		positivePattern: 0, negativePattern: 0,
		codePoint: 164, entityName: "curren"
	)]
	XTS = 963,

	/// <summary>
	/// ADB Unit of Account
	/// </summary>
	[Info(
		englishName: "ADB Unit of Account", nativeName: "ADB Unit of Account", symbol: "¤",
		significantDecimalDigits: 2,
		decimalSeparator: ".",
		groupSeparator: ",", groupSizes: new byte[] { 3 },
		positivePattern: 0, negativePattern: 0,
		codePoint: 164, entityName: "curren"
	)]
	XUA = 965,

	/// <summary>
	/// No currency
	/// </summary>
	[Info(
		englishName: "No currency", nativeName: "No currency", symbol: "¤",
		significantDecimalDigits: 2,
		decimalSeparator: ".",
		groupSeparator: ",", groupSizes: new byte[] { 3 },
		positivePattern: 0, negativePattern: 0,
		codePoint: 164, entityName: "curren"
	)]
	XXX = 999,

	/// <summary>
	/// Yemeni Rial
	/// </summary>
	[CanonicalCulture("ar-YE", Overwritten = true)]
	[Info(
		englishName: "Yemeni Rial", nativeName: "ريال يمني", symbol: "ر.ي.",
		significantDecimalDigits: 2,
		decimalSeparator: "٫",
		groupSeparator: "٬", groupSizes: new byte[] { 3 },
		positivePattern: 3, negativePattern: 8
	)]
	YER = 886,

	/// <summary>
	/// Rand
	/// </summary>
	[CanonicalCulture("en-ZA")]
	[Info(
		englishName: "Rand", nativeName: "South African Rand", symbol: "R",
		significantDecimalDigits: 2,
		decimalSeparator: ",",
		groupSeparator: "\u00a0", groupSizes: new byte[] { 3 },
		positivePattern: 0, negativePattern: 1
	)]
	ZAR = 710,

	/// <summary>
	/// Zambian Kwacha
	/// </summary>
	[CanonicalCulture("en-ZM", Overwritten = true), Obsolete("deprecated in favor of ZMW")]
	[Info(
		englishName: "Zambian Kwacha", nativeName: "Zambian Kwacha", symbol: "ZK",
		significantDecimalDigits: 2,
		decimalSeparator: ".",
		groupSeparator: ",", groupSizes: new byte[] { 3 },
		positivePattern: 0, negativePattern: 1,
		isObsolete: true
	)]
	ZMK = 894,

	/// <summary>
	/// Zambian Kwacha
	/// </summary>
	[CanonicalCulture("en-ZM")]
	[Info(
		englishName: "Zambian Kwacha", nativeName: "Zambian Kwacha", symbol: "K",
		significantDecimalDigits: 2,
		decimalSeparator: ".",
		groupSeparator: ",", groupSizes: new byte[] { 3 },
		positivePattern: 0, negativePattern: 1
	)]
	ZMW = 967,

	/// <summary>
	/// Zimbabwe Gold
	/// </summary>
	[CanonicalCulture("en-ZW", Overwritten = true)]
	[Info(
		englishName: "Zimbabwe Gold", nativeName: "Zimbabwe Gold", symbol: "Z$",
		significantDecimalDigits: 2,
		decimalSeparator: ".",
		groupSeparator: ",", groupSizes: new byte[] { 3 },
		positivePattern: 0, negativePattern: 1
	)]
	ZWG = 924,

	/// <summary>
	/// Zimbabwe Dollar
	/// </summary>
	[CanonicalCulture("en-ZW", Overwritten = true), Obsolete("deprecated in favor of ZWG")]
	[Info(
		englishName: "Zimbabwe Dollar", nativeName: "Zimbabwean dollar", symbol: "Z$",
		significantDecimalDigits: 2,
		decimalSeparator: ".",
		groupSeparator: ",", groupSizes: new byte[] { 3 },
		positivePattern: 0, negativePattern: 1,
		isObsolete: true
	)]
	ZWL = 932
}
#pragma warning restore CA1008, CA1028
