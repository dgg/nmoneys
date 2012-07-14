using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using NMoneys.Serialization;

namespace NMoneys.Tests.Support
{
	internal class OneGoBinarySerializer<T> : IDisposable
	{
		private readonly MemoryStream _stream;

		public OneGoBinarySerializer()
		{
			_stream = new MemoryStream();
		}

		public string Serialize(T toSerialize)
		{
			var outFormatter = new BinaryFormatter();
			outFormatter.Serialize(_stream, toSerialize);

			_stream.Flush();
			_stream.Seek(0, SeekOrigin.Begin);

			string serialized = new StreamReader(_stream).ReadToEnd();
			return serialized;
		}

		public T Deserialize()
		{
			var inFormatter = new BinaryFormatter();

			_stream.Seek(0, SeekOrigin.Begin);

			T deserialized = (T)inFormatter.Deserialize(_stream);

			return deserialized;
		}

		public void Dispose()
		{
			_stream.Close();
			_stream.Dispose();
		}
	}

	internal class OneGoXmlSerializer<T> : IDisposable
	{
		private readonly MemoryStream _stream;
		private readonly XmlSerializer _serializer;

		public OneGoXmlSerializer()
		{
			_stream = new MemoryStream();
			_serializer = new XmlSerializer(typeof(T));
		}

		public string Serialize(T toSerialize)
		{
			_serializer.Serialize(_stream, toSerialize);

			var sb = new StringBuilder();
			XmlWriter xw = XmlWriter.Create(sb);
			_serializer.Serialize(xw, toSerialize);
			xw.Flush();
			_stream.Flush();
			_stream.Seek(0, SeekOrigin.Begin);

			return sb.ToString();
		}

		public T Deserialize()
		{
			_stream.Seek(0, SeekOrigin.Begin);
			T deserialized = (T)_serializer.Deserialize(_stream);
			return deserialized;
		}

		public void Dispose()
		{
			_stream.Close();
			_stream.Dispose();
		}
	}

	internal class OneGoDataContractSerializer<T> : IDisposable
	{
		private readonly MemoryStream _stream;
		private readonly DataContractSerializer _serializer;

		public OneGoDataContractSerializer()
		{
			_stream = new MemoryStream();
			_serializer = new DataContractSerializer(typeof(T));
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
			_stream.Seek(0, SeekOrigin.Begin);

			T deserialized = (T)_serializer.ReadObject(_stream);

			return deserialized;
		}

		public void Dispose()
		{
			_stream.Close();
			_stream.Dispose();
		}
	}

	internal class OneGoDataContractJsonSerializer<T> : IDisposable
	{
		private readonly MemoryStream _stream;
		private readonly DataContractJsonSerializer _serializer;

		public OneGoDataContractJsonSerializer()
		{
			_stream = new MemoryStream();
			var surrogate = new DataContractJsonSurrogate();
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
