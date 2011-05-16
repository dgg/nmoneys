using System.IO;
using System.Runtime.Serialization;
using NUnit.Framework.Constraints;

namespace NMoneys.Tests.CustomConstraints
{
	internal class DataContractSerializationConstraint<T> : CustomConstraint<T>
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
					var serializer = new DataContractSerializer(typeof(T));
					serializer.WriteObject(str, toSerialize);
					
					str.Flush();
					str.Seek(0, SeekOrigin.Begin);
					string serialized = new StreamReader(str).ReadToEnd();
					str.Seek(0, SeekOrigin.Begin);

					T deserialized = (T)serializer.ReadObject(str);

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
