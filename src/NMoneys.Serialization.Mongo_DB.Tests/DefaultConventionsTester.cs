using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using NMoneys.Serialization.Mongo_DB.Tests.Support;
using NUnit.Framework;
using Testing.Commons.NUnit.Constraints;
using Testing.Commons.Serialization;

namespace NMoneys.Serialization.Mongo_DB.Tests
{
	[TestFixture]
	public class DefaultConventionsTester
	{
		private ProxySerializer<Money> _proxy;
		
		[TestFixtureSetUp]
		public void Setup()
		{
			_proxy = new ProxySerializer<Money>()
				.Register();
		}

		#region serialization

		[Test, Category("exploratory")]
		public void Serialization_OutOfTheBox_UsesPascalizedMemberNamesAndStringifiedDecimalAndType()
		{
			_proxy.Serializer = _proxy.Default;

			var toSerialize = new Money(14.3m, CurrencyIsoCode.XTS);
			var doc = new BsonDocument();
			BsonWriter writer = new BsonDocumentWriter(doc, new BsonDocumentWriterSettings());
			BsonSerializer.Serialize(writer, toSerialize);
			string @default = doc.ToJson();


			string expected = "{ '_t' : 'Money', 'CurrencyCode' : 963, 'Amount' : '14.3' }";
			Assert.That(@default, Is.EqualTo(expected).AsJson());
		}

		[Test, Category("exploratory")]
		public void Serialization_OutOfTheBox_NotLikeCanonicalJsonSerialization()
		{
			_proxy.Serializer = _proxy.Default;

			using (var serializer = new DataContractRoundtripSerializer<Money>())
			{
				var toSerialize = new Money(14.3m, CurrencyIsoCode.XTS);

				string @default = toSerialize.ToJson();

				string canonical = serializer.Serialize(toSerialize);
				Assert.That(@default, Is.Not.EqualTo(canonical));
			}
		}

		#region canonical serializer

		[Test]
		public void Serialization_Canonical_UsesPascalCasedPropertyNamesAndAlphabeticCode()
		{
			var toSerialize = new Money(14.3m, CurrencyIsoCode.XTS);

			_proxy.Serializer = new CanonicalMoneySerializer();

			string actual = toSerialize.ToJson();

			Assert.That(actual, Is.EqualTo("{ 'Amount' : 14.3, 'Currency' : { 'IsoCode' : 'XTS' } }").AsJson());
		}

		[Test]
		public void Serialization_CanonicalNotNullInstance_UsesPascalCasedPropertyNamesAndAlphabeticCode()
		{
			Money? notNull = new Money(14.3m, CurrencyIsoCode.XTS);

			_proxy.Serializer = new CanonicalMoneySerializer();

			string actual = notNull.ToJson();

			Assert.That(actual, Is.EqualTo("{ 'Amount' : 14.3, 'Currency' : { 'IsoCode' : 'XTS' } }").AsJson());
		}

		[Test]
		public void Serialization_CanonicalNullInstance_Null()
		{
			Money? @null = default(Money?);

			_proxy.Serializer = new CanonicalMoneySerializer();

			string actual = @null.ToJson();

			Assert.That(actual, Is.EqualTo("null"));
		}

		#endregion

		#region default serializer

		[Test]
		public void Serialization_Default_UsesPascalCasedPropertyNamesAndNumericCode()
		{
			var toSerialize = new Money(14.3m, CurrencyIsoCode.XTS);

			_proxy.Serializer = new DefaultMoneySerializer();

			string actual = toSerialize.ToJson();

			Assert.That(actual, Is.EqualTo("{ 'Amount' : 14.3, 'Currency' : 963 }").AsJson());
		}

		[Test]
		public void Serialization_DefaultNotNullInstance_UsesPascalCasedPropertyNamesAndNumericCode()
		{
			Money? notNull = new Money(14.3m, CurrencyIsoCode.XTS);

			_proxy.Serializer = new DefaultMoneySerializer();

			string actual = notNull.ToJson();

			Assert.That(actual, Is.EqualTo("{ 'Amount' : 14.3, 'Currency' : 963 }").AsJson());
		}

		[Test]
		public void Serialization_DefaultNullInstance_Null()
		{
			Money? @null = default(Money?);

			_proxy.Serializer = new DefaultMoneySerializer();

			string actual = @null.ToJson();

			Assert.That(actual, Is.EqualTo("null"));
		}

		[Test]
		public void Serialization_DefaultNotNullContainer_NotNullProperty()
		{
			var notNull = new NullableMoneyContainer { PropName = new Money(14.3m, CurrencyIsoCode.XTS) };

			_proxy.Serializer = new DefaultMoneySerializer();

			string actual = notNull.ToJson();

			Assert.That(actual, Is.EqualTo("{ 'PropName' : { 'Amount' : 14.3, 'Currency' : 963 } }").AsJson());
		}

		[Test]
		public void Serialization_DefaultNullContainer_NullProperty()
		{
			var @null = new NullableMoneyContainer { PropName = default(Money?) };

			_proxy.Serializer = new DefaultMoneySerializer();

			string actual = @null.ToJson();

			Assert.That(actual, Is.EqualTo("{ 'PropName' : null }").AsJson());
		}

		#endregion

		#endregion

		#region deserialization

		#region canonical serializer

		[Test]
		public void Deserialization_Canonical_ReadsPascalCasedProperties()
		{
			var expected = new Money(14.3m, CurrencyIsoCode.XTS);

			string json = "{'Amount':14.3,'Currency':{'IsoCode':'XTS'}}";

			_proxy.Serializer = new CanonicalMoneySerializer();

			var actual = BsonSerializer.Deserialize<Money>(json);

			Assert.That(actual, Is.EqualTo(expected));
		}

		[Test]
		public void Deserialization_CanonicalContainer_ReadsPascalCasedProperties()
		{
			var expected = new Money(14.3m, CurrencyIsoCode.XTS);

			string json = "{'Name': 'something', 'PropName': {'Amount':14.3,'Currency':{'IsoCode':'XTS'}}}";

			_proxy.Serializer = new CanonicalMoneySerializer();

			var actual = BsonSerializer.Deserialize<MoneyContainer>(json);

			Assert.That(actual.PropName, Is.EqualTo(expected));
		}

		[Test]
		public void Deserialization_CanonicalNotNull_ReadsPascalCasedProperties()
		{
			var expected = new Money(14.3m, CurrencyIsoCode.XTS);

			string json = "{'Amount':14.3,'Currency':{'IsoCode':'XTS'}}";

			_proxy.Serializer = new CanonicalMoneySerializer();

			var actual = BsonSerializer.Deserialize<Money?>(json);

			Assert.That(actual, Is.EqualTo(expected));
		}

		[Test]
		public void Deserialization_CanonicalNull_Null()
		{
			string json = "null";

			_proxy.Serializer = new CanonicalMoneySerializer();

			var actual = BsonSerializer.Deserialize<Money?>(json);

			Assert.That(actual, Is.Null);
		}

		#endregion

		#region default serializer

		[Test]
		public void Deserialization_Default_ReadsPascalCasedProperties()
		{
			var expected = new Money(14.3m, CurrencyIsoCode.XTS);

			string json = "{'Amount':14.3,'Currency': 963}";

			_proxy.Serializer = new DefaultMoneySerializer();

			var actual = BsonSerializer.Deserialize<Money>(json);

			Assert.That(actual, Is.EqualTo(expected));
		}

		[Test]
		public void Deserialization_DefaultContainer_ReadsPascalCasedProperties()
		{
			var expected = new Money(14.3m, CurrencyIsoCode.XTS);

			string json = "{'Name': 'something', 'PropName': {'Amount':14.3,'Currency': 963}}";

			_proxy.Serializer = new DefaultMoneySerializer();

			var actual = BsonSerializer.Deserialize<MoneyContainer>(json);

			Assert.That(actual.PropName, Is.EqualTo(expected));
		}

		[Test]
		public void Deserialization_DefaultNotNull_ReadsPascalCasedProperties()
		{
			var expected = new Money(14.3m, CurrencyIsoCode.XTS);

			string json = "{'Amount':14.3,'Currency':'XTS'}";

			_proxy.Serializer = new DefaultMoneySerializer();

			var actual = BsonSerializer.Deserialize<Money?>(json);

			Assert.That(actual, Is.EqualTo(expected));
		}

		[Test]
		public void Deserialization_DefaultNull_Null()
		{
			string json = "null";
			_proxy.Serializer = new DefaultMoneySerializer();

			var actual = BsonSerializer.Deserialize<Money?>(json);

			Assert.That(actual, Is.Null);
		}

		#endregion

		#endregion
	}
}