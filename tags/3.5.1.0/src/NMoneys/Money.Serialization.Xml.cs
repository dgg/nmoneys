using System;
using System.IO;
using System.Reflection;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using NMoneys.Support;

namespace NMoneys
{
	[XmlRoot(Namespace = Serialization.Data.NAMESPACE, ElementName = Serialization.Data.Money.ROOT_NAME, DataType = Serialization.Data.Money.DATA_TYPE, IsNullable = false)]
	public partial struct Money : IXmlSerializable
	{
		/// <summary>
		/// This method is reserved and should not be used.
		/// When implementing the <see cref="IXmlSerializable"/> interface, you should return null from this method, and instead, if specifying a custom schema is required, apply the <see cref="T:System.Xml.Serialization.XmlSchemaProviderAttribute"/> to the class.
		/// </summary>
		/// <returns>null</returns>
		[Obsolete("deprecated, use SchemaProviders instead")]
		XmlSchema IXmlSerializable.GetSchema()
		{
			return null;
		}

		/// <summary>
		/// Returns the XML schema applied for serialization.
		/// </summary>
		/// <param name="xs">A cache of XML Schema definition language (XSD) schemas.</param>
		/// <returns>Represents the complexType element from XML Schema as specified by the <paramref name="xs"/>.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="xs"/> is null.</exception>
		public static XmlSchemaComplexType GetSchema(XmlSchemaSet xs)
		{
			Guard.AgainstNullArgument("xs", xs);

			XmlSchemaComplexType complex = null;
			var schemaSerializer = new XmlSerializer(typeof(XmlSchema));
			using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(Serialization.Data.ResourceName))
			{
				if (stream != null)
				{
					var schema = (XmlSchema)schemaSerializer.Deserialize(new XmlTextReader(stream));
					xs.Add(schema);
					var name = new XmlQualifiedName(Serialization.Data.Money.DATA_TYPE, Serialization.Data.NAMESPACE);
					complex = (XmlSchemaComplexType)schema.SchemaTypes[name];
				}
			}
			return complex;
		}

		/// <summary>
		/// Generates an object from its XML representation.
		/// </summary>
		/// <param name="reader">The <see cref="XmlReader"/> stream from which the object is deserialized.</param>
		/// <exception cref="ArgumentNullException"><paramref name="reader"/> is null.</exception>
		public void ReadXml(XmlReader reader)
		{
			Guard.AgainstNullArgument("reader", reader);

			reader.ReadStartElement();
			setAllFields(reader.ReadElementContentAsDecimal(Serialization.Data.Money.AMOUNT, Serialization.Data.NAMESPACE),
				Currency.ReadXmlData(reader));
			reader.ReadEndElement();
		}

		/// <summary>
		/// Converts an object into its XML representation.
		/// </summary>
		/// <param name="writer">The <see cref="XmlWriter"/> stream to which the object is serialized.</param>
		/// <exception cref="ArgumentNullException"><paramref name="writer"/> is null.</exception>
		public void WriteXml(XmlWriter writer)
		{
			Guard.AgainstNullArgument("writer", writer);

			writer.WriteStartElement(Serialization.Data.Money.AMOUNT, Serialization.Data.NAMESPACE);
			writer.WriteValue(Amount);
			writer.WriteEndElement();
			writer.WriteStartElement(Serialization.Data.Currency.ROOT_NAME);
			Currency currency = Currency.Get(CurrencyCode);
			currency.WriteXml(writer);
			writer.WriteEndElement();
		}
	}
}
