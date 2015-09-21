using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using NMoneys.Serialization;

namespace NMoneys.Tests.Support
{
	public class OneGoDataContractJsonSerializer<T> : IDisposable
	{
		private readonly MemoryStream _stream;
		private readonly DataContractJsonSerializer _serializer;

		public OneGoDataContractJsonSerializer()
		{
			_stream = new MemoryStream();
			var surrogate = new DataContractSurrogate();
			_serializer = surrogate.BuildSerializer<T>();
		}

		public string Serialize(T toSerialize)
		{
			_serializer.WriteObject(_stream, toSerialize);

			_stream.Flush();
			_stream.Seek(0, SeekOrigin.Begin);
			string serialized = new StreamReader(_stream).ReadToEnd();
			return serialized;
		}

		public T Deserialize()
		{
			return deserialize(_stream);
		}

		private T deserialize(Stream s)
		{
			s.Seek(0, SeekOrigin.Begin);

			T deserialized = (T)_serializer.ReadObject(_stream);

			return deserialized;
		}

		public T Deserialize(string serialized)
		{
			using (var ms = new MemoryStream(Encoding.Default.GetBytes(serialized)))
			{
				try
				{
					return deserialize(ms);
				}
				finally
				{
					ms.Close();
				}
			}
		}

		public void Dispose()
		{
			_stream.Close();
			_stream.Dispose();
		}
	}
}
