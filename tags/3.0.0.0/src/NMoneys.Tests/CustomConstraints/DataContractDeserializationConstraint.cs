using System.IO;
using System.Runtime.Serialization;
using System.Xml;
using NUnit.Framework.Constraints;

namespace NMoneys.Tests.CustomConstraints
{
	internal class DataContractDeserializationConstraint<T> : CustomConstraint<string>
	{
		public DataContractDeserializationConstraint(T expectedDeserializedObject)
		{
			_inner = new EqualConstraint(expectedDeserializedObject);
		}

		protected override bool matches(string current)
		{
			using (StringReader sr = new StringReader(current))
			{
				XmlReader xr= XmlReader.Create(sr);
				try
				{
					var serializer = new DataContractSerializer(typeof (T));
					T actualDeserialized = (T)serializer.ReadObject(xr);

					return _inner.Matches(actualDeserialized);	
				}
				finally
				{
					xr.Close();
					sr.Close();
				}
			}
		}
	}
}