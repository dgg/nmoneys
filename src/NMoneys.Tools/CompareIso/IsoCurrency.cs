using System.Xml.Serialization;

namespace NMoneys.Tools.CompareIso
{
	public class IsoCurrency
	{
		[XmlIgnore]
		public bool HasValue => NumericCode.HasValue;

		[XmlElement("Ccy")]
		public string AlphabeticalCode { get; set; }

		[XmlElement("CcyNm")]
		public string Name { get; set; }

		[XmlElement("CcyNbr")]
		public NumericCode NumericCode { get; set; }

		[XmlElement("CcyMnrUnts")]
		public MinorUnits MinorUnits { get; set; }
	}
}