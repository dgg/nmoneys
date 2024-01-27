using System.Diagnostics.CodeAnalysis;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace NMoneys.Serialization.BSON;

public class QuantitySerializer : StructSerializerBase<Money>
{
	/// <inheritdoc />
	public override void Serialize([NotNull]BsonSerializationContext context, BsonSerializationArgs args, Money value)
	{
		context.Writer.WriteString(value.AsQuantity());
	}

	/// <inheritdoc />
	public override Money Deserialize([NotNull]BsonDeserializationContext context, BsonDeserializationArgs args)
	{
		string quantity = context.Reader.ReadString();

		Money deserialized = Money.Parse(quantity);
		return deserialized;
	}
}
