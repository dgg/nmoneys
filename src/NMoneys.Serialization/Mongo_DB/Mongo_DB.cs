using System;
using System.Text;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Options;

namespace NMoneys.Serialization.Mongo_DB
{
	#region serializers

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

			IMoneyWriter canonical = new CanonicalMoneyWriter(money, map);
			canonical.WriteTo(bsonWriter);
		}
	}

	public class DefaultMoneySerializer : IBsonSerializer
	{
		public object Deserialize(BsonReader bsonReader, Type nominalType, IBsonSerializationOptions options)
		{
			bsonReader.ReadStartDocument();
			double amount = bsonReader.ReadDouble();
			var map = (BsonClassMap<Money>)BsonClassMap.LookupClassMap(typeof(Money));

			var currency = map.GetMemberMap(m => m.CurrencyCode);
			bsonReader.ReadName("Currency".CapitalizeAs(currency));

			var currencyCode = (CurrencyIsoCode) currency.GetSerializer(typeof (CurrencyIsoCode))
				.Deserialize(bsonReader, typeof (CurrencyIsoCode), currency.SerializationOptions);

			return new Money(Convert.ToDecimal(amount), currencyCode);
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

			IMoneyWriter writer = new DefaultMoneyWriter(money, map);
			writer.WriteTo(bsonWriter);
		}
	}

	#endregion

	internal static class PropertyNameCapitalizer
	{
		public static string CapitalizeAs(this string pascalCasedPropertyName, BsonMemberMap sampleMap)
		{
			var sb = new StringBuilder(pascalCasedPropertyName, pascalCasedPropertyName.Length);
			if (char.IsLower(sampleMap.ElementName[0]))
			{
				sb[0] = char.ToLowerInvariant(sb[0]);
			}
			return sb.ToString();
		}
	}

	#region support for writing

	internal interface IMoneyWriter
	{
		void WriteTo(BsonWriter writer);
	}

	internal abstract class MoneyWriter : IMoneyWriter
	{
		public void WriteTo(BsonWriter writer)
		{
			writer.WriteStartDocument();
			writeAmount(writer);
			writeCurrency(writer);
			writer.WriteEndDocument();
		}

		protected abstract void writeAmount(BsonWriter writer);
		protected abstract void writeCurrency(BsonWriter writer);
	}

	internal class CanonicalMoneyWriter : MoneyWriter
	{
		private readonly Money _instance;
		private readonly BsonClassMap<Money> _map;

		public CanonicalMoneyWriter(Money instance, BsonClassMap<Money> map)
		{
			_instance = instance;
			_map = map;
		}

		protected override void writeAmount(BsonWriter writer)
		{
			BsonMemberMap amount = _map.GetMemberMap(m => m.Amount);
			writer.WriteDouble(amount.ElementName, Convert.ToDouble(_instance.Amount));
		}

		protected override void writeCurrency(BsonWriter writer)
		{
			BsonMemberMap currency = _map.GetMemberMap(m => m.CurrencyCode);
			writer.WriteName("Currency".CapitalizeAs(currency));
			writer.WriteStartDocument();
			writer.WriteString("IsoCode".CapitalizeAs(currency), _instance.CurrencyCode.AlphabeticCode());
			writer.WriteEndDocument();
		}
	}

	internal class DefaultMoneyWriter : MoneyWriter
	{
		private readonly Money _instance;
		private readonly BsonClassMap<Money> _map;

		public DefaultMoneyWriter(Money instance, BsonClassMap<Money> map)
		{
			_instance = instance;
			_map = map;
		}

		protected override void writeAmount(BsonWriter writer)
		{
			BsonMemberMap amount = _map.GetMemberMap(m => m.Amount);
			writer.WriteDouble(amount.ElementName, Convert.ToDouble(_instance.Amount));
		}

		protected override void writeCurrency(BsonWriter writer)
		{
			BsonMemberMap currency = _map.GetMemberMap(m => m.CurrencyCode);
			writer.WriteName("Currency".CapitalizeAs(currency));
			currency.GetSerializer(typeof(CurrencyIsoCode)).Serialize(writer, typeof(CurrencyIsoCode), _instance.CurrencyCode, currency.SerializationOptions);
		}
	}

	#endregion
}