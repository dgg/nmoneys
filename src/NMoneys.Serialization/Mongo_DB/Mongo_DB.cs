using System;
using System.Text;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Options;

namespace NMoneys.Serialization.Mongo_DB
{
	public class CanonicalMoneySerializer : IBsonSerializer
	{
		public object Deserialize(BsonReader bsonReader, Type nominalType, IBsonSerializationOptions options)
		{
			bsonReader.ReadStartDocument();
			double amount = bsonReader.ReadDouble();
			bsonReader.ReadStartDocument();
			string currency = bsonReader.ReadString();
			return new Money(Convert.ToDecimal(amount), currency);
		}

		public object Deserialize(BsonReader bsonReader, Type nominalType, Type actualType, IBsonSerializationOptions options)
		{
			return Deserialize(bsonReader, nominalType, options);
		}

		public IBsonSerializationOptions GetDefaultSerializationOptions()
		{
			return new DocumentSerializationOptions();
		}

		public void Serialize(BsonWriter bsonWriter, Type nominalType, object value, IBsonSerializationOptions options)
		{
			var money = (Money)value;

			var map = (BsonClassMap<Money>)BsonClassMap.LookupClassMap(typeof(Money));
			BsonMemberMap amount = map.GetMemberMap(m => m.Amount);

			bsonWriter.WriteStartDocument();
			bsonWriter.WriteDouble(amount.ElementName, Convert.ToDouble(money.Amount));
			bsonWriter.WriteName(capitalize(amount, "Currency"));
			bsonWriter.WriteStartDocument();
			bsonWriter.WriteString(capitalize(amount, "IsoCode"), money.CurrencyCode.AlphabeticCode());
			bsonWriter.WriteEndDocument();
			bsonWriter.WriteEndDocument();
		}

		private string capitalize(BsonMemberMap map, string pascalCased)
		{
			var sb = new StringBuilder(pascalCased, pascalCased.Length);
			if (char.IsLower(map.ElementName[0]))
			{
				sb[0] = char.ToLowerInvariant(sb[0]);
			}
			return sb.ToString();
		}
	}
}