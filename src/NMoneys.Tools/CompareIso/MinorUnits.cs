using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace NMoneys.Tools.CompareIso
{
	public struct MinorUnits : IXmlSerializable, IEquatable<int>
	{
		public byte? Value { get; private set; }

		public bool HasValue => Value.HasValue;

		public XmlSchema GetSchema()
		{
			return null;
		}

		public void ReadXml(XmlReader reader)
		{
			string read = reader.ReadString();
			reader.ReadEndElement();
			byte parsed;
			Value = byte.TryParse(read, out parsed) ? parsed : default(byte?);
		}

		public void WriteXml(XmlWriter writer)
		{
			throw new NotImplementedException("read only");
		}

		public bool Equals(int other)
		{
			bool equals = Value.HasValue && Convert.ToInt32(Value.Value).Equals(other);
			return equals;
		}

		public override string ToString()
		{
			return Value.ToString();
		}
	}
}