using MongoDB.Bson.Serialization;
using NMoneys.Serialization.Mongo_DB;
using NMoneys.Serialization.Tests.Mongo_DB.Support;
using NMoneys.Tests.Support;
using NUnit.Framework;

namespace NMoneys.Serialization.Tests.Mongo_DB
{
	[TestFixture]
	public class DeserializationTester
	{
		private ProxySerializer<Money> _proxy;

		[TestFixtureSetUp]
		public void Setup()
		{
			_proxy = new ProxySerializer<Money>()
				.Register();
		}

		[Test]
		public void CustomCanonicalSerializer_WithDefaultConventions_ReadsPascalCasedProperties()
		{
			var expected = new Money(14.3m, CurrencyIsoCode.XTS);

			string json = "{'Amount':14.3,'Currency':{'IsoCode':'XTS'}}";

			_proxy.Serializer = new CanonicalMoneySerializer();

			var actual = BsonSerializer.Deserialize<Money>(json);

			Assert.That(actual, Is.EqualTo(expected));
		}

		[Test]
		public void CustomCanonicalSerializer_MoneyContainer_ReadsPascalCasedProperties()
		{
			var expected = new Money(14.3m, CurrencyIsoCode.XTS);

			string json = "{'Name': 'something', 'PropName': {'Amount':14.3,'Currency':{'IsoCode':'XTS'}}}";

			_proxy.Serializer = new CanonicalMoneySerializer();

			var actual = BsonSerializer.Deserialize<MoneyContainer>(json);

			Assert.That(actual.PropName, Is.EqualTo(expected));
		}

		[Test]
		public void CustomDefaultSerializer_WithDefaultConventions_ReadsPascalCasedProperties()
		{
			var expected = new Money(14.3m, CurrencyIsoCode.XTS);

			string json = "{'Amount':14.3,'Currency': 963}";

			_proxy.Serializer = new DefaultMoneySerializer();

			var actual = BsonSerializer.Deserialize<Money>(json);

			Assert.That(actual, Is.EqualTo(expected));
		}

		[Test]
		public void CustomDefaultSerializer_MoneyContainer_ReadsPascalCasedProperties()
		{
			var expected = new Money(14.3m, CurrencyIsoCode.XTS);

			string json = "{'Name': 'something', 'PropName': {'Amount':14.3,'Currency': 963}}";

			_proxy.Serializer = new DefaultMoneySerializer();

			var actual = BsonSerializer.Deserialize<MoneyContainer>(json);

			Assert.That(actual.PropName, Is.EqualTo(expected));
		}

		#region nullable

		[Test]
		public void CustomCanonicalSerializer_NotNullWithDefaultConventions_ReadsPascalCasedProperties()
		{
			var expected = new Money(14.3m, CurrencyIsoCode.XTS);

			string json = "{'Amount':14.3,'Currency':{'IsoCode':'XTS'}}";

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
		public void CustomDefaultSerializer_NotNull_ReadsPascalCasedProperties()
		{
			var expected = new Money(14.3m, CurrencyIsoCode.XTS);

			string json = "{'Amount':14.3,'Currency':'XTS'}";

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