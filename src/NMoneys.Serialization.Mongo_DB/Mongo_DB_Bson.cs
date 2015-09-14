using System;
using System.Text;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.Serializers;

namespace NMoneys.Serialization.Mongo_DB
{
	#region serializers

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
		public DefaultMoneySerializer() : base(
			m => new DefaultMoneyReader(m),
			m => new DefaultMoneyWriter(m))
		{ }
	}

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
			m => new CanonicalMoneyWriter(m))
		{ }
	}

	public abstract class MoneySerializer : SerializerBase<Money>
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
		/// Deserializes a value.
		/// </summary>
		/// <param name="context">The deserialization context.</param>
		/// <param name="args">The deserialization args.</param>
		/// <returns>
		/// A deserialized value.
		/// </returns>
		public override Money Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
		{
			return _reader.Value.ReadFrom(context, args);
		}

		/// <summary>
		/// Serializes a value.
		/// </summary>
		/// <param name="context">The serialization context.</param>
		/// <param name="args">The serialization args.</param>
		/// <param name="value">The value.</param>
		public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, Money value)
		{
			_writer.Value.Write(value, context, args);
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

	#region support for reading

	internal interface IMoneyReader
	{
		Money ReadFrom(BsonDeserializationContext context, BsonDeserializationArgs args);
	}

	internal abstract class MoneyReader : IMoneyReader
	{
		public Money ReadFrom(BsonDeserializationContext context, BsonDeserializationArgs args)
		{
			context.Reader.ReadStartDocument();
			decimal amount = readAmount(context.Reader);
			CurrencyIsoCode currencyCode = readCurrency(context, args);
			context.Reader.ReadEndDocument();
			return new Money(amount, currencyCode);
		}

		protected abstract decimal readAmount(IBsonReader reader);
		protected abstract CurrencyIsoCode readCurrency(BsonDeserializationContext context, BsonDeserializationArgs args);
	}

	internal class DefaultMoneyReader : MoneyReader
	{
		private readonly BsonClassMap<Money> _map;

		public DefaultMoneyReader(BsonClassMap<Money> map)
		{
			_map = map;
		}

		protected override decimal readAmount(IBsonReader reader)
		{
			var amountMap = _map.GetMemberMap(m => m.Amount);
			double amountRead = reader.ReadDouble(amountMap.ElementName);
			decimal amount = Convert.ToDecimal(amountRead);
			return amount;
		}

		protected override CurrencyIsoCode readCurrency(BsonDeserializationContext context, BsonDeserializationArgs args)
		{
			var currencyMap = _map.GetMemberMap(m => m.CurrencyCode);
			context.Reader.ReadName("Currency".CapitalizeAs(currencyMap));
			var currencyCode = (CurrencyIsoCode)currencyMap.GetSerializer()
				.Deserialize(context, args);
			return currencyCode;
		}
	}

	internal class CanonicalMoneyReader : MoneyReader
	{
		private readonly BsonClassMap<Money> _map;

		public CanonicalMoneyReader(BsonClassMap<Money> map)
		{
			_map = map;
		}

		protected override decimal readAmount(IBsonReader reader)
		{
			BsonMemberMap amountMap = _map.GetMemberMap(m => m.Amount);
			double amountRead = reader.ReadDouble(amountMap.ElementName);
			decimal amount = Convert.ToDecimal(amountRead);
			return amount;
		}

		protected override CurrencyIsoCode readCurrency(BsonDeserializationContext context, BsonDeserializationArgs args)
		{
			context.Reader.ReadStartDocument();

			BsonMemberMap currencyMap = _map.GetMemberMap(m => m.CurrencyCode);
			string currency = context.Reader.ReadString("IsoCode".CapitalizeAs(currencyMap));

			context.Reader.ReadEndDocument();
			CurrencyIsoCode currencyCode = Currency.Code.Parse(currency);
			return currencyCode;
		}
	}

	#endregion

	#region support for writing

	internal interface IMoneyWriter
	{
		void Write(Money value, BsonSerializationContext context, BsonSerializationArgs args);
	}

	internal abstract class MoneyWriter : IMoneyWriter
	{
		protected readonly BsonClassMap<Money> _map;

		protected MoneyWriter(BsonClassMap<Money> map)
		{
			_map = map;
		}

		public void Write(Money value, BsonSerializationContext context, BsonSerializationArgs args)
		{
			context.Writer.WriteStartDocument();
			writeAmount(value, context.Writer);
			writeCurrency(value, context, args);
			context.Writer.WriteEndDocument();
		}

		protected virtual void writeAmount(Money value, IBsonWriter writer)
		{
			BsonMemberMap amountMap = _map.GetMemberMap(m => m.Amount);
			writer.WriteDouble(amountMap.ElementName, Convert.ToDouble(value.Amount));
		}
		protected abstract void writeCurrency(Money value, BsonSerializationContext context, BsonSerializationArgs args);
	}

	internal class CanonicalMoneyWriter : MoneyWriter
	{
		public CanonicalMoneyWriter(BsonClassMap<Money> map) : base(map) { }

		protected override void writeCurrency(Money value, BsonSerializationContext context, BsonSerializationArgs args)
		{
			BsonMemberMap currencyMap = _map.GetMemberMap(m => m.CurrencyCode);
			context.Writer.WriteName("Currency".CapitalizeAs(currencyMap));
			context.Writer.WriteStartDocument();
			context.Writer.WriteString("IsoCode".CapitalizeAs(currencyMap), value.CurrencyCode.AlphabeticCode());
			context.Writer.WriteEndDocument();
		}
	}

	internal class DefaultMoneyWriter : MoneyWriter
	{
		public DefaultMoneyWriter(BsonClassMap<Money> map) : base(map) { }
		protected override void writeCurrency(Money value, BsonSerializationContext context, BsonSerializationArgs args)
		{
			BsonMemberMap currencyMap = _map.GetMemberMap(m => m.CurrencyCode);
			context.Writer.WriteName("Currency".CapitalizeAs(currencyMap));
			currencyMap.GetSerializer().Serialize(context, args, value.CurrencyCode);
		}
	}

	#endregion
}