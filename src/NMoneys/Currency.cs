using System.Xml.Serialization;

namespace NMoneys;

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
	/// Textual representation of the ISO 4217 code
	/// </summary>
	[XmlIgnore]
	public string IsoSymbol { get; private set; }

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

	#endregion
}
