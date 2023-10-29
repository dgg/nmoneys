namespace NMoneys;

/// <summary>
/// Currency codes as stated by the ISO 4217 standard
/// </summary>
/// <seealso href="http://www.iso.org/iso/support/currency_codes_list-1.htm" />
#pragma warning disable CA1008, CA1028
public enum CurrencyIsoCode : ushort
{
	/// <summary>
	/// Test currency
	/// </summary>
	//[EnumMember]
	[Info(
		englishName: "Test currency", nativeName: "Test currency", symbol: "¤",
		significantDecimalDigits: 2,
		decimalSeparator: ".",
		groupSeparator: ",", groupSizes: new byte[] { 3 },
		negativePattern: 0, positivePattern: 0
	)]
	XTS = 963,

	/// <summary>
	/// No currency
	/// </summary>
	//[EnumMember]
	[Info(
		englishName: "No currency", nativeName: "No currency", symbol: "¤",
		significantDecimalDigits: 2,
		decimalSeparator: ".",
		groupSeparator: ",", groupSizes: new byte[] { 3 },
		negativePattern: 0, positivePattern: 0
	)]
	XXX = 999,
}
#pragma warning restore CA1008, CA1028
