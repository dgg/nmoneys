using System.Xml.Serialization;

namespace NMoneys.Tools.CompareIso
{
	[XmlRoot("ISO_4217")]
	public class Iso4217Element
	{
		[XmlElement("CcyTbl")]
		public CountryTableElement CountryTable { get; set; }
	}
}