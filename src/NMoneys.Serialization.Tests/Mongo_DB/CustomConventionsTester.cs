using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using NMoneys.Serialization.Mongo_DB;
using NMoneys.Serialization.Tests.Mongo_DB.Support;
using NMoneys.Tests.Support;
using NUnit.Framework;

namespace NMoneys.Serialization.Tests.Mongo_DB
{
	[TestFixture, Explicit("we cannot change conventions once applied :,-(")]
	public class CustomConventionsTester
	{
		private ProxySerializer<Money> _proxy;

		[TestFixtureSetUp]
		public void Setup()
		{
			new TestingConventions(
					new CamelCaseElementNameConvention(),
					new EnumRepresentationConvention(BsonType.String))
				.Register();

			_proxy = new ProxySerializer<Money>();
			BsonSerializer.RegisterSerializer(typeof(Money), _proxy);
		}

		#region serialization

		#region canonical serializer

		[Test]
		public void Serialization_Canonical_UsesCamelCasedPropertyNamesAndAlphabeticCode()
		{
			var toSerialize = new Money(14.3m, CurrencyIsoCode.XTS);

			_proxy.Serializer = new CanonicalMoneySerializer();

			string actual = toSerialize.ToJson();

			Assert.That(actual, Is.EqualTo("{ 'amount' : 14.3, 'currency' : { 'isoCode' : 'XTS' } }".Jsonify()));
		}

		[Test]
		public void Serialization_Canonical_LikeCanonicalJsonSerialization()
		{
			using (var serializer = new OneGoDataContractJsonSerializer<Money>())
			{
				var toSerialize = new Money(14.3m, CurrencyIsoCode.XTS);
				string canonical = serializer.Serialize(toSerialize);

				_proxy.Serializer = new CanonicalMoneySerializer();

				string actual = toSerialize.ToJson().Replace(" ", string.Empty);
				Assert.That(actual, Is.EqualTo(canonical));
			}
		}

		[Test]
		public void Serialization_CanonicalNotNull_UsesCamelCasedPropertyNamesAndAlphabeticCode()
		{
			Money? notNull = new Money(14.3m, CurrencyIsoCode.XTS);

			_proxy.Serializer = new CanonicalMoneySerializer();

			string actual = notNull.ToJson();

			Assert.That(actual, Is.EqualTo("{ 'amount' : 14.3, 'currency' : { 'isoCode' : 'XTS' } }".Jsonify()));
		}

		[Test]
		public void Serialization_CanonicalNull_Null()
		{
			Money? @null = default(Money?);

			_proxy.Serializer = new CanonicalMoneySerializer();

			string actual = @null.ToJson();

			Assert.That(actual, Is.EqualTo("null"));
		}

		[Test]
		public void Serialization_CanonicalNotNullContainer_UsesCamelCasedPropertyNamesAndAlphabeticCode()
		{
			var notNull = new NullableMoneyContainer { PropName = new Money(14.3m, CurrencyIsoCode.XTS) };

			_proxy.Serializer = new CanonicalMoneySerializer();

			string actual = notNull.ToJson();
			Assert.That(actual, Is.EqualTo("{ 'propName' : { 'amount' : 14.3, 'currency' : { 'isoCode' : 'XTS' } } }".Jsonify()));
		}

		[Test]
		public void Serialization_CanonicalNullContainer_NullProperty()
		{
			var notNull = new NullableMoneyContainer { PropName = default(Money?) };

			_proxy.Serializer = new CanonicalMoneySerializer();

			string actual = notNull.ToJson();

			Assert.That(actual, Is.EqualTo("{ 'propName' : null }".Jsonify()));
		}

		#endregion

		#region default serializer

		[Test]
		public void Serialization_Default_UsesCamelCasedPropertyNamesAndAlphabeticCode()
		{
			var toSerialize = new Money(14.3m, CurrencyIsoCode.XTS);

			_proxy.Serializer = new DefaultMoneySerializer();

			string actual = toSerialize.ToJson();
			Assert.That(actual, Is.EqualTo("{ 'amount' : 14.3, 'currency' : 'XTS' }".Jsonify()));
		}

		[Test]
		public void Serialization_DefaultNotNull_UsesCamelCasedPropertyNamesAndAlphabeticCode()
		{
			Money? notNull = new Money(14.3m, CurrencyIsoCode.XTS);

			_proxy.Serializer = new DefaultMoneySerializer();

			string actual = notNull.ToJson();
			Assert.That(actual, Is.EqualTo("{ 'amount' : 14.3, 'currency' : 'XTS' }".Jsonify()));
		}

		[Test]
		public void Serialization_DefaultNull_Null()
		{
			Money? @null = default(Money?);

			_proxy.Serializer = new DefaultMoneySerializer();

			string actual = @null.ToJson();
			Assert.That(actual, Is.EqualTo("null"));
		}

		#endregion

		#endregion

		#region deserialization

		#region canonical serializer

		[Test]
		public void Deserialization_Canonical_ReadsCamelCasedProperties()
		{
			var expected = new Money(14.3m, CurrencyIsoCode.XTS);

			string json = "{'amount':14.3,'currency':{'isoCode':'XTS'}}";

			_proxy.Serializer = new CanonicalMoneySerializer();

			var actual = BsonSerializer.Deserialize<Money>(json);

			Assert.That(actual, Is.EqualTo(expected));
		}

		[Test]
		public void Deserialization_Canonical_LikeCanonicalJsonSerialization()
		{
			using (var serializer = new OneGoDataContractJsonSerializer<Money>())
			{
				var toSerialize = new Money(14.3m, CurrencyIsoCode.XTS);
				string canonical = serializer.Serialize(toSerialize);

				_proxy.Serializer = new CanonicalMoneySerializer();

				Assert.That(BsonSerializer.Deserialize<Money>(canonical),
					Is.EqualTo(toSerialize));
			}
		}

		[Test]
		public void Deserialization_CanonicalContainer_CamelCasedProperties()
		{
			var expected = new Money(14.3m, CurrencyIsoCode.XTS);

			string json = "{'name': 'something', 'propName': {'amount':14.3,'currency':{'isoCode':'XTS'}}}";

			_proxy.Serializer = new CanonicalMoneySerializer();

			var actual = BsonSerializer.Deserialize<MoneyContainer>(json);

			Assert.That(actual.PropName, Is.EqualTo(expected));
		}

		[Test]
		public void Deserialization_CanonicalNotNull_ReadsCamelCasedProperties()
		{
			var expected = new Money(14.3m, CurrencyIsoCode.XTS);

			string json = "{'amount':14.3,'currency':{'isoCode':'XTS'}}";

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

		[Test]
		public void Deserialization_CanonicalNotNullContainer_ReadsCamelCasedProperties()
		{
			var expected = new Money(14.3m, CurrencyIsoCode.XTS);

			string json = "{'propName':{'amount':14.3,'currency':{'isoCode':'XTS'}}}";

			_proxy.Serializer = new CanonicalMoneySerializer();

			var actual = BsonSerializer.Deserialize<NullableMoneyContainer>(json);

			Assert.That(actual.PropName, Is.EqualTo(expected));
		}

		[Test]
		public void Deserialization_CanonicalNullContainer_Null()
		{
			string json = "{'propName':null}";

			_proxy.Serializer = new CanonicalMoneySerializer();

			var actual = BsonSerializer.Deserialize<NullableMoneyContainer>(json);

			Assert.That(actual.PropName, Is.Null);
		}

		#endregion

		#region default serializer

		[Test]
		public void Deserialization_Default_CamelCasedProperties()
		{
			var expected = new Money(14.3m, CurrencyIsoCode.XTS);

			string json = "{'amount':14.3,'currency':'XTS'}".Jsonify();

			_proxy.Serializer = new DefaultMoneySerializer();

			var actual = BsonSerializer.Deserialize<Money>(json);

			Assert.That(actual, Is.EqualTo(expected));
		}

		[Test]
		public void Deserialization_DefaultContainer_CamelCasedProperties()
		{
			var expected = new Money(14.3m, CurrencyIsoCode.XTS);

			string json = "{'name': 'something', 'propName': {'amount':14.3,'currency':'XTS'}}}";

			_proxy.Serializer = new DefaultMoneySerializer();

			var actual = BsonSerializer.Deserialize<MoneyContainer>(json);

			Assert.That(actual.PropName, Is.EqualTo(expected));
		}

		[Test]
		public void Deserialization_DefaultNotNull_ReadsCamelCasedProperties()
		{
			var expected = new Money(14.3m, CurrencyIsoCode.XTS);

			string json = "{'amount':14.3,'currency':'XTS'}";

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