using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace NMoneys.Serialization.BSON;

/// <summary>
/// Allows customizing the serialization of monetary quantities
/// </summary>
public class MoneySerializer : StructSerializerBase<Money>
{
	// maps cannot be changed so the looked-up instance is cached
	private readonly Lazy<BsonClassMap<Money>> _map = new(() =>
		(BsonClassMap<Money>)BsonClassMap.LookupClassMap(typeof(Money)));

	/// <inheritdoc />
	public override void Serialize([NotNull]BsonSerializationContext context, BsonSerializationArgs args, Money value)
	{
		context.Writer.WriteStartDocument();

		writeAmount(context.Writer, value);

		writeCurrency(context, args, value);

		context.Writer.WriteEndDocument();
	}

	/// <inheritdoc />
	public override Money Deserialize([NotNull]BsonDeserializationContext context, BsonDeserializationArgs args)
	{
		context.Reader.ReadStartDocument();

		decimal amount = readAmount(context.Reader);
		CurrencyIsoCode currency = readCurrency(context, args);

		context.Reader.ReadEndDocument();

		var deserialized = new Money(amount, currency);
		return deserialized;
	}

	#region write

	private void writeAmount(IBsonWriter writer, Money value)
	{
		string amountName = _map.Value.GetMemberMap(m => m.Amount).ElementName;
		writer.WriteName(amountName);
		writer.WriteDecimal128(value.Amount);
	}

	private void writeCurrency(BsonSerializationContext context, BsonSerializationArgs args, Money value)
	{
		var currencyMap = _map.Value.GetMemberMap(c => c.CurrencyCode);
		string currencyName = char.IsLower(currencyMap.ElementName, 0) ? "currency" : "Currency";
		context.Writer.WriteName(currencyName);
		currencyMap.GetSerializer().Serialize(context, args, value.CurrencyCode);
	}

	#endregion

	#region read

	private decimal readAmount(IBsonReader reader)
	{
		var amountMap = _map.Value.GetMemberMap(m => m.Amount);
		reader.ReadName(amountMap.ElementName);
		BsonType type = reader.GetCurrentBsonType();
		decimal amount;
		switch (type)
		{
			// does not support floating point conversion
			case BsonType.String:
				string str = reader.ReadString();
				amount = decimal.Parse(str, CultureInfo.InvariantCulture);
				break;
			case BsonType.Decimal128:
				Decimal128 dec  = reader.ReadDecimal128();
				amount = Decimal128.ToDecimal(dec);
				break;
			default:
				throw new NotSupportedException($"Cannot extract a monetary amount out of '{type.ToString()}'.");
		}

		return amount;
	}

	private CurrencyIsoCode readCurrency(BsonDeserializationContext context, BsonDeserializationArgs args)
	{
		var currencyMap = _map.Value.GetMemberMap(m => m.CurrencyCode);
		string currencyName = char.IsLower(currencyMap.ElementName, 0) ? "currency" : "Currency";
		context.Reader.ReadName(currencyName);
		var currency = (CurrencyIsoCode)currencyMap.GetSerializer()
			.Deserialize(context, args);
		return currency;
	}

	#endregion
}
