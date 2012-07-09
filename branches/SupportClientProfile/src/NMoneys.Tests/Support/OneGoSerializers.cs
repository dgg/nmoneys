﻿using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Web.Script.Serialization;
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
			BinaryFormatter outFormatter = new BinaryFormatter();
			outFormatter.Serialize(_stream, toSerialize);

			_stream.Flush();
			_stream.Seek(0, SeekOrigin.Begin);

			string serialized = new StreamReader(_stream).ReadToEnd();
			return serialized;
		}

		public T Deserialize()
		{
			BinaryFormatter inFormatter = new BinaryFormatter();

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

			StringBuilder sb = new StringBuilder();
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

	internal class OneGoJsonSerializer<T> : IDisposable
	{
		private readonly JavaScriptSerializer _serializer;
		private readonly StringBuilder _sb;

		public OneGoJsonSerializer()
		{
			_serializer = new JavaScriptSerializer();
			_serializer.RegisterConverters(new JavaScriptConverter[] { new MoneyConverter(), new CurrencyConverter(), new CurrencyCodeConverter() });

			_sb = new StringBuilder();
		}

		public string Serialize(T toSerialize)
		{
			_serializer.Serialize(toSerialize, _sb);
			return _sb.ToString();
		}

		public T Deserialize()
		{
			T deserialized = _serializer.Deserialize<T>(_sb.ToString());
			return deserialized;
		}

		public void Dispose() { }
	}
}
