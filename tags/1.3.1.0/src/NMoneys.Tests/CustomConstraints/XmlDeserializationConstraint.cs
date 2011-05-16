using System.IO;
using System.Xml.Serialization;
using NUnit.Framework.Constraints;

namespace NMoneys.Tests.CustomConstraints
{
	internal class XmlDeserializationConstraint<T> : CustomConstraint<string>
	{
		public XmlDeserializationConstraint(T expectedDeserializedObject)
		{
			_inner = new EqualConstraint(expectedDeserializedObject);
		}

		protected override bool matches(string current)
		{
			var serializer = new XmlSerializer(typeof(T));
			using (StringReader sr = new StringReader(current))
			{
				
				T actualDeserialized = (T)serializer.Deserialize(sr);
				bool result = _inner.Matches(actualDeserialized);
				sr.Close();
				return result;
			}
		}
	}
}
