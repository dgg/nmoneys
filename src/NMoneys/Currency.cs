using System.Xml.Serialization;

namespace NMoneys;

///<summary>
/// Represents a currency unit such as Euro or American Dollar.
///</summary>
public sealed partial class Currency
{
	#region properties

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
