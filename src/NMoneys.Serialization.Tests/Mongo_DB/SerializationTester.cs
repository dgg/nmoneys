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

			string actual = toSerialize.ToJson()
				;
			Assert.That(actual, Is.EqualTo("{ 'Amount' : 14.3, 'Currency' : 963 }".Jsonify()));
		}
	}
}