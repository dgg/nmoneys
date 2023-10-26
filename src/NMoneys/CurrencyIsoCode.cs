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
	XTS = 963,
	/// <summary>
	/// No currency
	/// </summary>
	//[EnumMember]
	XXX = 999,
}
	#pragma  warning restore CA1008, CA1028
