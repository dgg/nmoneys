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
	public class CustomConventionDeserializationTester
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

		[Test]
		public void CustomCanonicalSerializer_WithCamelCaseConventions_ReadsCamelCasedProperties()
		{
			var expected = new Money(14.3m, CurrencyIsoCode.XTS);

			string json = "{'amount':14.3,'currency':{'isoCode':'XTS'}}";

			_proxy.Serializer = new CanonicalMoneySerializer();

			var actual = BsonSerializer.Deserialize<Money>(json);

			Assert.That(actual, Is.EqualTo(expected));
		}

		[Test]
		public void CustomCanonicalConverter_WithCamelCaseContract_LikeCanonicalJsonSerialization()
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
		public void CustomCanonicalSerializer_MoneyContainer_CamelCasedProperties()
		{
			var expected = new Money(14.3m, CurrencyIsoCode.XTS);

			string json = "{'name': 'something', 'propName': {'amount':14.3,'currency':{'isoCode':'XTS'}}}";

			_proxy.Serializer = new CanonicalMoneySerializer();

			var actual = BsonSerializer.Deserialize<MoneyContainer>(json);

			Assert.That(actual.PropName, Is.EqualTo(expected));
		}

		[Test]
		public void CustomDefaultConverter_CamelCaseAndCurrencyString_CamelCasedProperties()
		{
			var expected = new Money(14.3m, CurrencyIsoCode.XTS);

			string json = "{'amount':14.3,'currency':'XTS'}".Jsonify();

			_proxy.Serializer = new DefaultMoneySerializer();

			var actual = BsonSerializer.Deserialize<Money>(json);

			Assert.That(actual, Is.EqualTo(expected));
		}

		[Test]
		public void CustomDefaultSerializer_MoneyContainer_CamelCasedProperties()
		{
			var expected = new Money(14.3m, CurrencyIsoCode.XTS);

			string json = "{'name': 'something', 'propName': {'amount':14.3,'currency':'XTS'}}}";

			_proxy.Serializer = new DefaultMoneySerializer();

			var actual = BsonSerializer.Deserialize<MoneyContainer>(json);

			Assert.That(actual.PropName, Is.EqualTo(expected));
		}

		#region nullable

		[Test]
		public void CustomCanonicalSerializer_NotNull_ReadsCamelCasedProperties()
		{
			var expected = new Money(14.3m, CurrencyIsoCode.XTS);

			string json = "{'amount':14.3,'currency':{'isoCode':'XTS'}}";

			_proxy.Serializer = new CanonicalMoneySerializer();

			var actual = BsonSerializer.Deserialize<Money?>(json);

			Assert.That(actual, Is.EqualTo(expected));
		}

		[Test]
		public void CustomCanonicalSerializer_Null_Null()
		{

			string json = "null";

			_proxy.Serializer = new CanonicalMoneySerializer();

			var actual = BsonSerializer.Deserialize<Money?>(json);

			Assert.That(actual, Is.Null);
		}

		[Test]
		public void CustomCanonicalSerializer_NotNullContainer_ReadsCamelCasedProperties()
		{
			var expected = new Money(14.3m, CurrencyIsoCode.XTS);

			string json = "{'propName':{'amount':14.3,'currency':{'isoCode':'XTS'}}}";

			_proxy.Serializer = new CanonicalMoneySerializer();

			var actual = BsonSerializer.Deserialize<NullableMoneyContainer>(json);

			Assert.That(actual.PropName, Is.EqualTo(expected));
		}

		[Test]
		public void CustomCanonicalConverter_NullContainer_Null()
		{
			string json = "{'propName':null}";

			_proxy.Serializer = new CanonicalMoneySerializer();

			var actual = BsonSerializer.Deserialize<NullableMoneyContainer>(json);

			Assert.That(actual.PropName, Is.Null);
		}

		[Test]
		public void CustomDefaultConverter_NotNull_ReadsCamelCasedProperties()
		{
			var expected = new Money(14.3m, CurrencyIsoCode.XTS);

			string json = "{'amount':14.3,'currency':'XTS'}";

			_proxy.Serializer = new DefaultMoneySerializer();

			var actual = BsonSerializer.Deserialize<Money?>(json);

			Assert.That(actual, Is.EqualTo(expected));
		}

		[Test]
		public void CustomDefaultConverter_Null_Null()
		{
			string json = "null";

			_proxy.Serializer = new DefaultMoneySerializer();

			var actual = BsonSerializer.Deserialize<Money?>(json);

			Assert.That(actual, Is.Null);
		}

		#endregion
	}
}