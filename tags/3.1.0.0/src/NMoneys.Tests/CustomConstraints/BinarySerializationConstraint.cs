using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using NUnit.Framework.Constraints;

namespace NMoneys.Tests.CustomConstraints
{
	internal class BinarySerializationConstraint<T> : CustomConstraint<T>
	{
		private readonly Func<T, Constraint> _constraintOverDeserialized;

		public BinarySerializationConstraint(Func<T, Constraint> constraintOverDeserialized)
		{
			_constraintOverDeserialized = constraintOverDeserialized;
		}

		protected override bool matches(T current)
		{
			T deserialized = getDeserializedObject(current);
			_inner = _constraintOverDeserialized(deserialized);
			return _inner.Matches(current);
		}

		private static T getDeserializedObject(T toSerialize)
		{
			using (MemoryStream str = new MemoryStream())
			{
				try
				{
					BinaryFormatter outFormatter = new BinaryFormatter();
					BinaryFormatter inFormatter = new BinaryFormatter();
					outFormatter.Serialize(str, toSerialize);

					str.Flush();
					str.Seek(0, SeekOrigin.Begin);

					// serialized not used, but helps to debug
					string serialized = new StreamReader(str).ReadToEnd();
					str.Seek(0, SeekOrigin.Begin);

					T deserialized = (T)inFormatter.Deserialize(str);

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