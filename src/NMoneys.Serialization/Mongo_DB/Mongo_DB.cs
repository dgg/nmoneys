using System;
using System.Text;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.Options;

namespace NMoneys.Serialization.Mongo_DB
{
	#region serializers


	/// <summary>
	/// Converts a <see cref="Money"/> (or <see cref="Nullable{T}"/>) instance to and from JSON in the canonical way.
	/// </summary>
	/// <remarks>The canonical way is the one implemented in NMoneys itself, with an <c>Amount</c>
	/// numeric property and a <c>Currency</c> object with a three-letter code <c>IsoCode"</c> property.
	/// <para>
	/// Property casing must be configured apart from this serializer using, for instance, another set of
	/// <see cref="IConvention"/>
	/// </para>
	/// </remarks>
	/// <example>
	/// <code>{"Amount" : 123.4, "Currency" : {"IsoCode" : "XXX"}}</code>
	/// <code>{"amount" : 123.4, "currency" : {"isoCode" : "XXX"}}</code>
	/// </example>
	public class CanonicalMoneySerializer : MoneySerializer
	{
		public CanonicalMoneySerializer() : base(
			m => new CanonicalMoneyReader(m),
			m => new CanonicalMoneyWriter(m)) { }
	}

	/// <summary>
	/// Converts a <see cref="Money"/> (or <see cref="Nullable{Money}"/>) instance to and from JSON in a default (standard) way.
	/// </summary>
	/// <remarks>The default (standard) way is the one that a normal serializer would do for a money instance,
	/// with an <c>Amount</c> numeric property and a <c>Currency</c> code. The <c>Currency</c> property can be
	/// serialized either as a number (the default or using a <see cref="EnumRepresentationConvention"/> with <see cref="BsonType.Int32"/>) or as
	/// a string (using a <see cref="EnumRepresentationConvention"/> with <see cref="BsonType.String"/>).
	/// <para>
	/// Property casing must be configured apart from this serializer using, for instance, another set of
	/// <see cref="IConvention"/>
	/// </para>
	/// </remarks>
	/// <example>
	/// <code>{"Amount" : 123.4, "Currency" : "XXX"}</code>
	/// <code>{"amount" : 123.4, "currency" : 999}</code>
	/// </example>
	public class DefaultMoneySerializer : MoneySerializer
	{
		public DefaultMoneySerializer()
			: base(
				m => new DefaultMoneyReader(m),
				m => new DefaultMoneyWriter(m)) { }
	}

	/// <summary>
	/// Base class for custom <see cref="Money"/> serializers
	/// </summary>
	public abstract class MoneySerializer : IBsonSerializer
	{
		private readonly Lazy<IMoneyReader> _reader;
		private readonly Lazy<IMoneyWriter> _writer;

		internal MoneySerializer(Func<BsonClassMap<Money>, IMoneyReader> reader,
			Func<BsonClassMap<Money>, IMoneyWriter> writer)
		{
			_reader = new Lazy<IMoneyReader>(() =>
			{
				var map = (BsonClassMap<Money>)BsonClassMap.LookupClassMap(typeof(Money));
				return reader(map);
			});

			_writer = new Lazy<IMoneyWriter>(() =>
			{
				var map = (BsonClassMap<Money>)BsonClassMap.LookupClassMap(typeof(Money));
				return writer(map);
			});
		}


		/// <summary>
		/// Deserializes an object from a BsonReader.
		/// </summary>
		/// <param name="bsonReader">The BsonReader.</param>
		/// <param name="nominalType">The nominal type of the object.</param>
		/// <param name="options">The serialization options.</param>
		/// <returns>An object.</returns>
		public object Deserialize(BsonReader bsonReader, Type nominalType, IBsonSerializationOptions options)
		{
			Money money = _reader.Value.ReadFrom(bsonReader);
			return money;
		}

		/// <summary>
		/// Deserializes an object from a BsonReader.
		/// </summary>
		/// <param name="bsonReader">The BsonReader.</param>
		/// <param name="nominalType">The nominal type of the object.</param>
		/// <param name="actualType">The actual type of the object.</param>
		/// <param name="options">The serialization options.</param>
		/// <returns>An object.</returns>
		public object Deserialize(BsonReader bsonReader, Type nominalType, Type actualType, IBsonSerializationOptions options)
		{
			return Deserialize(bsonReader, nominalType, options);
		}

		/// <summary>
		/// Gets the default serialization options for this serializer.
		/// </summary>
		/// <returns>
		/// The default serialization options for this serializer.
		/// </returns>
		public IBsonSerializationOptions GetDefaultSerializationOptions()
		{
			return new DocumentSerializationOptions();
		}

		/// <summary>
		/// Serializes an object to a BsonWriter.
		/// </summary>
		/// <param name="bsonWriter">The BsonWriter.</param>
		/// <param name="nominalType">The nominal type.</param>
		/// <param name="value">The object.</param>
		/// <param name="options">The serialization options.</param>
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