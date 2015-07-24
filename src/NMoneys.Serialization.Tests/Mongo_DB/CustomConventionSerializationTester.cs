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
	public class CustomConventionSerializationTester
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
		public void CustomCanonicalConverter_CamelCaseConventions_UsesCamelCasedPropertyNamesAndAlphabeticCode()
		{
			var toSerialize = new Money(14.3m, CurrencyIsoCode.XTS);

			_proxy.Serializer = new CanonicalMoneySerializer();

			string actual = toSerialize.ToJson();

			Assert.That(actual, Is.EqualTo("{ 'amount' : 14.3, 'currency' : { 'isoCode' : 'XTS' } }".Jsonify()));
		}

		[Test]
		public void CustomCanonicalConverterWithCamelContract_LikeCanonicalJsonSerialization()
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
	}
}