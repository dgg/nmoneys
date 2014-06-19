using System.Globalization;
using NMoneys.Serialization.Json_NET;
using NMoneys.Tests.Support;
using NUnit.Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace NMoneys.Serialization.Tests.Json_Net
{
	[TestFixture]
	public class SerializationTester
	{
		#region CurrencyIsoCode

		[Test, Category("exploratory")]
		public void CurrencyIsoCode_DefaultSerialization_NumericValue()
		{
			string serializedByDefault = JsonConvert.SerializeObject(CurrencyIsoCode.EGP);
			short numericValue = CurrencyIsoCode.EGP.NumericCode();
			Assert.That(serializedByDefault, Is.EqualTo(numericValue.ToString(CultureInfo.InvariantCulture)));
		}

		[Test]
		public void CurrencyIsoCode_DefaultSerialiation_LikeCanonicalJsonSerialization()
		{
			using (var serializer = new OneGoDataContractJsonSerializer<CurrencyIsoCode>())
			{
				string canonical = serializer.Serialize(CurrencyIsoCode.XTS);
				Assert.That(JsonConvert.SerializeObject(CurrencyIsoCode.XTS), Is.EqualTo(canonical));
			}
		}

		[Test, Category("exploratory")]
		public void CurrencyIsoCode_IncludedCustomConverter_StringValue()
		{
			string serializedByDefault = JsonConvert.SerializeObject(CurrencyIsoCode.EGP, new StringEnumConverter());
			string stringValue = "\"EGP\"";
			Assert.That(serializedByDefault, Is.EqualTo(stringValue));
		}

		#endregion

		#region Currency

		[Test, Category("exploratory")]
		public void Currency_DefaultSerialization_UsesCustomSerialization()
		{
			string serializedByDefault = JsonConvert.SerializeObject(Currency.Get("EGP"));
			string customValue = "{\"isoCode\":\"EGP\"}";
			Assert.That(serializedByDefault, Is.EqualTo(customValue));
		}

		[Test]
		public void Currency_DefaultSerialiation_LikeCanonicalJsonSerialization()
		{
			using (var serializer = new OneGoDataContractJsonSerializer<Currency>())
			{
				string canonical = serializer.Serialize(Currency.Get("XTS"));
				Assert.That(JsonConvert.SerializeObject(Currency.Get("XTS")), Is.EqualTo(canonical));
			}
		}

		#endregion

		#region Money

		[Test, Category("exploratory")]
		public void Money_DefaultSerialization_UsesCustomSerialization()
		{
			var toSerialize = new Money(14.3m, CurrencyIsoCode.XTS);
			string serializedByDefault = JsonConvert.SerializeObject(toSerialize);

			string customValue = "{\"amount\":14.3,\"currency\":963}";
			Assert.That(serializedByDefault, Is.EqualTo(customValue));
		}

		[Test]
		public void Money_DefaultSerialiation_NotLikeCanonicalJsonSerialization()
		{
			using (var serializer = new OneGoDataContractJsonSerializer<Money>())
			{
				var toSerialize = new Money(14.3m, CurrencyIsoCode.XTS);
				string canonical = serializer.Serialize(toSerialize);
				Assert.That(JsonConvert.SerializeObject(toSerialize), Is.Not.EqualTo(canonical));
			}
		}

		[Test, Category("exploratory")]
		public void Money_IncludedCustomConverter_NotLikeCanonicalJsonSerialization()
		{
			using (var serializer = new OneGoDataContractJsonSerializer<Money>())
			{
				var toSerialize = new Money(14.3m, CurrencyIsoCode.XTS);
				string canonical = serializer.Serialize(toSerialize);
				Assert.That(JsonConvert.SerializeObject(toSerialize, new StringEnumConverter()),
					Is.Not.EqualTo(canonical));
			}
		}

		[Test]
		public void Money_CustomConverter_LikeCanonicalJsonSerialization()
		{
			using (var serializer = new OneGoDataContractJsonSerializer<Money>())
			{
				var toSerialize = new Money(14.3m, CurrencyIsoCode.XTS);
				string canonical = serializer.Serialize(toSerialize);
				Assert.That(JsonConvert.SerializeObject(toSerialize, new MoneyConverter()),
					Is.EqualTo(canonical).And.EqualTo("{\"amount\":14.3,\"currency\":{\"isoCode\":\"XTS\"}}"));
			}
		}

		#endregion
	}
}
