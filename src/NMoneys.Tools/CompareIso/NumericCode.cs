using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace NMoneys.Tools.CompareIso
{
	public struct NumericCode : IXmlSerializable
	{
		public short? Value { get; private set; }

		public bool HasValue => Value.HasValue;

		public XmlSchema GetSchema()
		{
			return null;
		}

		public void ReadXml(XmlReader reader)
		{
			string read = reader.ReadString();
			reader.ReadEndElement();
			Value = Int16.TryParse(read, out var parsed) && parsed > 0 ? parsed : default(short?);
		}

		public void WriteXml(XmlWriter writer)
		{
			throw new NotImplementedException("read only");
		}
	}
}