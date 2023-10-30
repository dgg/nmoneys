using System.Runtime.Serialization;
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
	/// Lek
	/// </summary>
	[EnumMember, CanonicalCulture("sq-AL")]
	[Info(
		englishName: "Lek", nativeName: "Leku Shqiptar", symbol: "L",
		significantDecimalDigits: 2,
		decimalSeparator: ".",
		groupSeparator: "\u00a0", groupSizes: new byte[] { 3 },
		positivePattern: 3, negativePattern: 8
	)]
	ALL = 008,
	/// <summary>
	/// Belize Dollar
	/// </summary>
	[EnumMember, CanonicalCulture("en-BZ")]
	[Info(
		englishName: "Belize Dollar", nativeName: "Belize Dollar", symbol: "BZ$",
		significantDecimalDigits: 2,
		decimalSeparator: ".",
		groupSeparator: ",", groupSizes: new byte[] { 3 },
		positivePattern: 0, negativePattern: 1
	)]
	BZD = 084,
	/// <summary>
	/// Test currency
	/// </summary>
	[EnumMember]
	[Info(
		englishName: "Test currency", nativeName: "Test currency", symbol: "¤",
		significantDecimalDigits: 2,
		decimalSeparator: ".",
		groupSeparator: ",", groupSizes: new byte[] { 3 },
		positivePattern: 0, negativePattern: 0
	)]
	XTS = 963,

	/// <summary>
	/// No currency
	/// </summary>
	[EnumMember]
	[Info(
		englishName: "No currency", nativeName: "No currency", symbol: "¤",
		significantDecimalDigits: 2,
		decimalSeparator: ".",
		groupSeparator: ",", groupSizes: new byte[] { 3 },
		positivePattern: 0, negativePattern: 0
	)]
	XXX = 999,
}
#pragma warning restore CA1008, CA1028
