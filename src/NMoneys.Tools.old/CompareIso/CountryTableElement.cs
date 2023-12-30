using System.Xml.Serialization;

namespace NMoneys.Tools.CompareIso
{
	public class CountryTableElement
	{
		[XmlElement("CcyNtry")]
		public IsoCurrency[] Countries { get; set; }
	}
}