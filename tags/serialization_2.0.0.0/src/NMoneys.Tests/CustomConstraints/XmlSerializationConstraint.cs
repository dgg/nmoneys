using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using NUnit.Framework.Constraints;

namespace NMoneys.Tests.CustomConstraints
{
	internal class XmlSerializationConstraint<T> : CustomConstraint<T>
	{
		protected override bool matches(T current)
		{
			return (_inner = new EqualConstraint(getDeserializedObject(current)))
				.Matches(current);
		}

		private static T getDeserializedObject(T toSerialize)
		{
			using (MemoryStream str = new MemoryStream())
			{
				try
				{
					var serializer = new XmlSerializer(typeof(T));
					serializer.Serialize(str, toSerialize);

					StringBuilder sb = new StringBuilder();
					XmlWriter xw = XmlWriter.Create(sb);
					if (xw != null)
					{
						serializer.Serialize(xw, toSerialize);
						xw.Flush();
					}

					str.Flush();
					str.Seek(0, SeekOrigin.Begin);

					T deserialized = (T)serializer.Deserialize(str);

					return deserialized;
				}
				finally
				{
					str.Close();
				}
			}
		}
	}
}