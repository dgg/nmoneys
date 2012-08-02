using System.IO;
using System.Text;
using NMoneys.Serialization;
using NUnit.Framework.Constraints;

namespace NMoneys.Tests.CustomConstraints
{
	internal class DataContractJsonDeserializationConstraint<T> : CustomConstraint<string>
	{
		public DataContractJsonDeserializationConstraint(T expectedDeserializedObject)
		{
			_inner = new EqualConstraint(expectedDeserializedObject);
		}

		protected override bool matches(string current)
		{
			using (var ms = new MemoryStream(Encoding.Default.GetBytes(current)))
			{
				try
				{
					var surrogate = new DataContractSurrogate();
					var serializer = surrogate.BuildSerializer<T>();
					T actualDeserialized = (T)serializer.ReadObject(ms);

					return _inner.Matches(actualDeserialized);
				}
				finally
				{
					ms.Close();
				}
			}
		}
	}
}