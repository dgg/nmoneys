using System.IO;
using System.Runtime.Serialization.Json;
using NMoneys.Serialization;
using NUnit.Framework.Constraints;

namespace NMoneys.Tests.CustomConstraints
{
	internal class DataContractJsonSerializationConstraint<T> : CustomConstraint<T>
	{
		protected override bool matches(T current)
		{
			return (_inner = new EqualConstraint(getDeserializedObject(current)))
				.Matches(current);
		}

		private static T getDeserializedObject(T toSerialize)
		{
			using (var str = new MemoryStream())
			{
				try
				{
					var surrogate = new DataContractSurrogate();

					var serializer = new DataContractJsonSerializer(
						surrogate.Type<T>(),
						surrogate.KnownTypes<T>(),
						surrogate.MaxItemsInObjectGraph,
						surrogate.IgnoreExtensionDataObject,
						surrogate,
						surrogate.AlwaysEmitTypeInformation);
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