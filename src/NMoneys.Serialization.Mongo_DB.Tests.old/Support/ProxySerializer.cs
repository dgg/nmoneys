using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace NMoneys.Serialization.Mongo_DB.Tests.Support
{
	public class ProxySerializer<T> : SerializerBase<T>
	{
		private static readonly IBsonSerializer<T> _default = new BsonClassMapSerializer<T>(
			BsonClassMap.RegisterClassMap<T>());

		public ProxySerializer()
		{
			Serializer = _default;
		}

		public IBsonSerializer<T> Serializer { get; set; }

		public IBsonSerializer<T> Default { get { return _default; } }

		public override T Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
		{
			return Serializer.Deserialize(context, args);
		}

		public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, T value)
		{
			Serializer.Serialize(context, args, value);
		}

		public ProxySerializer<T> Register()
		{
			BsonSerializer.RegisterSerializer(typeof (T), this);
			return this;
		}
	}
}