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
		private readonly Lazy<IMoneyReader> _reader = new Lazy<IMoneyReader>(() =>
		{
			var map = (BsonClassMap<Money>) BsonClassMap.LookupClassMap(typeof (Money));
			return new CanonicalMoneyReader(map);
		});

		public object Deserialize(BsonReader bsonReader, Type nominalType, IBsonSerializationOptions options)
		{
			Money money = _reader.Value.ReadFrom(bsonReader);
			return money;
		}

		public object Deserialize(BsonReader bsonReader, Type nominalType, Type actualType, IBsonSerializationOptions options)
		{
			return Deserialize(bsonReader, nominalType, options);
		}

		public IBsonSerializationOptions GetDefaultSerializationOptions()
		{
			return new DocumentSerializationOptions();
		}

		private readonly Lazy<IMoneyWriter> _writer = new Lazy<IMoneyWriter>(() =>
		{
			var map = (BsonClassMap<Money>)BsonClassMap.LookupClassMap(typeof(Money));
			return new CanonicalMoneyWriter(map);
		});

		public void Serialize(BsonWriter bsonWriter, Type nominalType, object value, IBsonSerializationOptions options)
		{
			var money = (Money)value;
			_writer.Value.WriteTo(money, bsonWriter);
		}
	}

	public class DefaultMoneySerializer : IBsonSerializer
	{
		private readonly Lazy<IMoneyReader> _reader = new Lazy<IMoneyReader>(() =>
		{
			var map = (BsonClassMap<Money>)BsonClassMap.LookupClassMap(typeof(Money));
			return new DefaultMoneyReader(map);
		}); 

		public object Deserialize(BsonReader bsonReader, Type nominalType, IBsonSerializationOptions options)
		{
			Money money = _reader.Value.ReadFrom(bsonReader);
			return money;
		}

		public object Deserialize(BsonReader bsonReader, Type nominalType, Type actualType, IBsonSerializationOptions options)
		{
			return Deserialize(bsonReader, nominalType, options);
		}

		public IBsonSerializationOptions GetDefaultSerializationOptions()
		{
			return new DocumentSerializationOptions();
		}

		private readonly Lazy<IMoneyWriter> _writer = new Lazy<IMoneyWriter>(() =>
		{
			var map = (BsonClassMap<Money>)BsonClassMap.LookupClassMap(typeof(Money));
			return new DefaultMoneyWriter(map);
		});
		public void Serialize(BsonWriter bsonWriter, Type nominalType, object value, IBsonSerializationOptions options)
		{
			var money = (Money)value;
			_writer.Value.WriteTo(money, bsonWriter);
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
		void WriteTo(Money instance, BsonWriter writer);
	}

	internal abstract class MoneyWriter : IMoneyWriter
	{
		public void WriteTo(Money instance, BsonWriter writer)
		{
			writer.WriteStartDocument();
			writeAmount(instance, writer);
			writeCurrency(instance, writer);
			writer.WriteEndDocument();
		}

		protected abstract void writeAmount(Money instance, BsonWriter writer);
		protected abstract void writeCurrency(Money instance, BsonWriter writer);
	}

	internal class CanonicalMoneyWriter : MoneyWriter
	{
		private readonly BsonClassMap<Money> _map;

		public CanonicalMoneyWriter(BsonClassMap<Money> map)
		{
			_map = map;
		}

		protected override void writeAmount(Money instance, BsonWriter writer)
		{
			BsonMemberMap amount = _map.GetMemberMap(m => m.Amount);
			writer.WriteDouble(amount.ElementName, Convert.ToDouble(instance.Amount));
		}

		protected override void writeCurrency(Money instance, BsonWriter writer)
		{
			BsonMemberMap currency = _map.GetMemberMap(m => m.CurrencyCode);
			writer.WriteName("Currency".CapitalizeAs(currency));
			writer.WriteStartDocument();
			writer.WriteString("IsoCode".CapitalizeAs(currency), instance.CurrencyCode.AlphabeticCode());
			writer.WriteEndDocument();
		}
	}

	internal class DefaultMoneyWriter : MoneyWriter
	{
		private readonly BsonClassMap<Money> _map;

		public DefaultMoneyWriter(BsonClassMap<Money> map)
		{
			_map = map;
		}

		protected override void writeAmount(Money instance, BsonWriter writer)
		{
			BsonMemberMap amount = _map.GetMemberMap(m => m.Amount);
			writer.WriteDouble(amount.ElementName, Convert.ToDouble(instance.Amount));
		}

		protected override void writeCurrency(Money instance, BsonWriter writer)
		{
			BsonMemberMap currency = _map.GetMemberMap(m => m.CurrencyCode);
			writer.WriteName("Currency".CapitalizeAs(currency));
			currency.GetSerializer(typeof(CurrencyIsoCode)).Serialize(writer, typeof(CurrencyIsoCode), instance.CurrencyCode, currency.SerializationOptions);
		}
	}

	#endregion

	#region support for reading

	internal interface IMoneyReader
	{
		Money ReadFrom(BsonReader reader);
	}

	internal abstract class MoneyReader : IMoneyReader
	{
		public Money ReadFrom(BsonReader reader)
		{
			reader.ReadStartDocument();
			var amount = readAmount(reader);
			var currency = readCurrency(reader);
			reader.ReadEndDocument();
			return new Money(amount, currency);
		}

		protected abstract decimal readAmount(BsonReader reader);
		protected abstract CurrencyIsoCode readCurrency(BsonReader reader);
	}

	internal class CanonicalMoneyReader : MoneyReader
	{
		private readonly BsonClassMap<Money> _map;

		public CanonicalMoneyReader(BsonClassMap<Money> map)
		{
			_map = map;
		}

		protected override decimal readAmount(BsonReader reader)
		{
			BsonMemberMap amountMap = _map.GetMemberMap(m => m.Amount);
			double amount = reader.ReadDouble(amountMap.ElementName);
			return Convert.ToDecimal(amount);
		}

		protected override CurrencyIsoCode readCurrency(BsonReader reader)
		{
			reader.ReadStartDocument();
			BsonMemberMap currencyMap = _map.GetMemberMap(m => m.CurrencyCode);
			string currency = reader.ReadString("IsoCode".CapitalizeAs(currencyMap));
			reader.ReadEndDocument();

			return Currency.Code.Parse(currency);
		}
	}

	internal class DefaultMoneyReader : MoneyReader
	{
		private readonly BsonClassMap<Money> _map;

		public DefaultMoneyReader(BsonClassMap<Money> map)
		{
			_map = map;
		}

		protected override decimal readAmount(BsonReader reader)
		{
			var amountMap = _map.GetMemberMap(m => m.Amount);
			double amount = reader.ReadDouble(amountMap.ElementName);
			
			return Convert.ToDecimal(amount);
		}

		protected override CurrencyIsoCode readCurrency(BsonReader reader)
		{
			var currencyMap = _map.GetMemberMap(m => m.CurrencyCode);
			reader.ReadName("Currency".CapitalizeAs(currencyMap));

			var currencyCode = (CurrencyIsoCode)currencyMap.GetSerializer(typeof(CurrencyIsoCode))
				.Deserialize(reader, typeof(CurrencyIsoCode), currencyMap.SerializationOptions);

			return currencyCode;
		}
	}

	#endregion
}