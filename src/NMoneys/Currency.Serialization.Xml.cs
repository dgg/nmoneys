using System;
using System.Diagnostics.Contracts;
using System.IO;
using System.Reflection;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using NMoneys.Support;

namespace NMoneys
{
	[XmlSchemaProvider("GetSchema")]
	[XmlRoot(Namespace = Serialization.Data.NAMESPACE, ElementName = Serialization.Data.Currency.ROOT_NAME, DataType = Serialization.Data.Currency.DATA_TYPE, IsNullable = false)]
	public sealed partial class Currency : IXmlSerializable
	{
		/// <summary>
		/// This method is reserved and should not be used.
		/// When implementing the <see cref="IXmlSerializable"/> interface, you should return null from this method, and instead,
		/// if specifying a custom schema is required, apply the <see cref="XmlSchemaProviderAttribute"/> to the class.
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
		[Pure]
		public static XmlSchemaComplexType GetSchema(XmlSchemaSet xs)
		{
			Guard.AgainstNullArgument(nameof(xs), xs);

			XmlSchemaComplexType complex = null;
			var schemaSerializer = new XmlSerializer(typeof(XmlSchema));
			using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(Serialization.Data.ResourceName))
			{
				if (stream != null)
				{
					var schema = (XmlSchema)schemaSerializer.Deserialize(new XmlTextReader(stream));
					xs.Add(schema);
					var name = new XmlQualifiedName(Serialization.Data.Currency.DATA_TYPE, Serialization.Data.NAMESPACE);
					complex = (XmlSchemaComplexType)schema.SchemaTypes[name];
				}
			}
			return complex;
		}

		/// <summary>
		/// Generates an object from its XML representation.
		/// </summary>
		/// <param name="reader">The <see cref="XmlReader"/> stream from which the object is deserialized.</param>
		public void ReadXml(XmlReader reader)
		{
			var isoCode = ReadXmlData(reader);

			Currency paradigm = Get(isoCode);
			setAllFields(paradigm.IsoCode,
						 paradigm.EnglishName,
						 paradigm.NativeName,
						 paradigm.Symbol,
						 paradigm.SignificantDecimalDigits,
						 paradigm.DecimalSeparator,
						 paradigm.GroupSeparator,
						 paradigm.GroupSizes,
						 paradigm.PositivePattern,
						 paradigm.NegativePattern,
						 paradigm.IsObsolete,
						 paradigm.Entity);
		}

		/// <summary>
		/// Converts an object into its XML representation.
		/// </summary>
		/// <param name="writer">The <see cref="XmlWriter"/> stream to which the object is serialized.</param>
		/// <exception cref="ArgumentNullException"><paramref name="writer"/> is null.</exception>
		public void WriteXml(XmlWriter writer)
		{
			Guard.AgainstNullArgument(nameof(writer), writer);
			writer.WriteElementString(Serialization.Data.Currency.ISO_CODE, Serialization.Data.NAMESPACE, IsoSymbol);
		}

		internal static CurrencyIsoCode ReadXmlData(XmlReader reader)
		{
			reader.ReadStartElement();
			CurrencyIsoCode isoCode = Code.ParseArgument(
				reader.ReadElementContentAsString(Serialization.Data.Currency.ISO_CODE, Serialization.Data.NAMESPACE),
				Serialization.Data.Currency.ISO_CODE);
			reader.ReadEndElement();
			return isoCode;
		}
	}
}
