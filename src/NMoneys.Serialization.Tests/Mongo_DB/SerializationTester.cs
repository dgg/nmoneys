using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using NMoneys.Serialization.Mongo_DB;
using NMoneys.Serialization.Tests.Mongo_DB.Support;
using NMoneys.Tests.Support;
using NUnit.Framework;

namespace NMoneys.Serialization.Tests.Mongo_DB
{
	[TestFixture]
	public class SerializationTester
	{
		private ProxySerializer<Money> _proxy;
		
		[TestFixtureSetUp]
		public void Setup()
		{
			_proxy = new ProxySerializer<Money>()
				.Register();
		}

		[Test, Category("exploratory")]
		public void DefaultSerialization_UsesPascalizedMemberNamesAndStringifiedDecimal()
		{
			_proxy.Serializer = _proxy.Default;

			var toSerialize = new Money(14.3m, CurrencyIsoCode.XTS);
			var doc = new BsonDocument();
			BsonWriter writer = new BsonDocumentWriter(doc, new BsonDocumentWriterSettings());
			BsonSerializer.Serialize(writer, toSerialize);
			string @default = doc.ToJson();

			
			string expected = "{ 'CurrencyCode' : 963, 'Amount' : '14.3' }".Jsonify();
			Assert.That(@default, Is.EqualTo(expected));
		}

		[Test]
		public void DefaultSerialization_NotLikeCanonicalJsonSerialization()
		{
			_proxy.Serializer = _proxy.Default;

			using (var serializer = new OneGoDataContractJsonSerializer<Money>())
			{
				var toSerialize = new Money(14.3m, CurrencyIsoCode.XTS);

				string @default = toSerialize.ToJson();

				string canonical = serializer.Serialize(toSerialize);
				Assert.That(@default, Is.Not.EqualTo(canonical));
			}
		}

		[Test]
		public void CustomCanonicalSerializer_DefaultConventions_UsesPascalCasedPropertyNamesAndAlphabeticCode()
		{
			var toSerialize = new Money(14.3m, CurrencyIsoCode.XTS);

			_proxy.Serializer = new CanonicalMoneySerializer();

			string actual = toSerialize.ToJson();
			
			Assert.That(actual, Is.EqualTo("{ 'Amount' : 14.3, 'Currency' : { 'IsoCode' : 'XTS' } }".Jsonify()));
		}

		[Test]
		public void CustomDefaultSerializer_DefaultConventions_UsesPascalCasedPropertyNamesAndNumericCode()
		{
			var toSerialize = new Money(14.3m, CurrencyIsoCode.XTS);

			_proxy.Serializer = new DefaultMoneySerializer();

			string actual = toSerialize.ToJson();

			Assert.That(actual, Is.EqualTo("{ 'Amount' : 14.3, 'Currency' : 963 }".Jsonify()));
		}

		#region nullables

		[Test]
		public void CustomCanonicalSerializer_NotNullDefaultConventions_UsesPascalCasedPropertyNamesAndAlphabeticCode()
		{
			Money? notNull = new Money(14.3m, CurrencyIsoCode.XTS);

			_proxy.Serializer = new CanonicalMoneySerializer();

			string actual = notNull.ToJson();

			Assert.That(actual, Is.EqualTo("{ 'Amount' : 14.3, 'Currency' : { 'IsoCode' : 'XTS' } }".Jsonify()));
		}
		
		[Test]
		public void CustomCanonicalSerializer_NullDefaultConventions_UsesPascalCasedPropertyNamesAndAlphabeticCode()
		{
			Money? @null = default(Money?);

			_proxy.Serializer = new CanonicalMoneySerializer();

			string actual = @null.ToJson();

			Assert.That(actual, Is.EqualTo("null"));
		}

		[Test]
		public void CustomDefaultSerializer_NotNullDefaultConventions_UsesPascalCasedPropertyNamesAndNumericCode()
		{
			Money? notNull = new Money(14.3m, CurrencyIsoCode.XTS);

			_proxy.Serializer = new DefaultMoneySerializer();

			string actual = notNull.ToJson();

			Assert.That(actual, Is.EqualTo("{ 'Amount' : 14.3, 'Currency' : 963 }".Jsonify()));
		}

		[Test]
		public void CustomDefaultSerializer_NullDefaultConventions_Null()
		{
			Money? @null = default(Money?);

			_proxy.Serializer = new DefaultMoneySerializer();

			string actual = @null.ToJson();

			Assert.That(actual, Is.EqualTo("null"));
		}

		[Test]
		public void CustomDefaultSerializer_NotNullContainer_NotNullProperty()
		{
			var notNull = new NullableMoneyContainer { PropName = new Money(14.3m, CurrencyIsoCode.XTS) };

			_proxy.Serializer = new DefaultMoneySerializer();

			string actual = notNull.ToJson();

			Assert.That(actual, Is.EqualTo("{ 'PropName' : { 'Amount' : 14.3, 'Currency' : 963 } }".Jsonify()));
		}

		[Test]
		public void CustomDefaultSerializer_NullContainer_NullProperty()
		{
			var @null = new NullableMoneyContainer { PropName = default(Money?) };

			_proxy.Serializer = new DefaultMoneySerializer();

			string actual = @null.ToJson();

			Assert.That(actual, Is.EqualTo("{ 'PropName' : null }".Jsonify()));
		}

		#endregion
	}
}