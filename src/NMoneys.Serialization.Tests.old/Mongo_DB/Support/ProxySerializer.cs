using System;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;

namespace NMoneys.Serialization.Tests.Mongo_DB.Support
{
	public class ProxySerializer<T> : IBsonSerializer
	{
		private static readonly IBsonSerializer _default = new BsonClassMapSerializer(
			BsonClassMap.LookupClassMap(typeof (T)));

		public ProxySerializer()
		{
			Serializer = _default;
		}

		public IBsonSerializer Serializer { get; set; }

		public IBsonSerializer Default { get { return _default; } }
		
		public object Deserialize(BsonReader bsonReader, Type nominalType, IBsonSerializationOptions options)
		{
			return Serializer.Deserialize(bsonReader, nominalType, options);
		}

		public object Deserialize(BsonReader bsonReader, Type nominalType, Type actualType, IBsonSerializationOptions options)
		{
			return Serializer.Deserialize(bsonReader, nominalType, actualType, options);
		}

		public IBsonSerializationOptions GetDefaultSerializationOptions()
		{
			return Serializer.GetDefaultSerializationOptions();
		}

		public void Serialize(BsonWriter bsonWriter, Type nominalType, object value, IBsonSerializationOptions options)
		{
			Serializer.Serialize(bsonWriter, nominalType, value, options);
		}

		public ProxySerializer<T> Register()
		{
			BsonSerializer.RegisterSerializer(typeof (T), this);
			return this;
		}
	}
}