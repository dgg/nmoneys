using System.Globalization;
using System.Runtime.Serialization;
using NMoneys.Serialization.Json_NET;
using NMoneys.Tests.CustomConstraints;
using NMoneys.Tests.Support;
using NUnit.Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace NMoneys.Serialization.Tests.Json_Net
{
	[TestFixture]
	public class DeserializationTester
	{
		#region CurrencyIsoCode

		[Test, Category("exploratory")]
		public void CurrencyIsoCode_DefaultOverNumericValue_Currency()
		{
			string numericValue = CurrencyIsoCode.EGP.NumericCode().ToString(CultureInfo.InvariantCulture);

			Assert.That(JsonConvert.DeserializeObject<CurrencyIsoCode>(numericValue),
				Is.EqualTo(CurrencyIsoCode.EGP));
		}

		[Test]
		public void CurrencyIsoCode_DefaultDeserialiation_LikeCanonicalJsonSerialization()
		{
			string serializedDollar = "840";
			Assert.That(serializedDollar, Must.Be.DataContractJsonDeserializableInto(CurrencyIsoCode.USD));
			Assert.That(JsonConvert.DeserializeObject<CurrencyIsoCode>(serializedDollar), Is.EqualTo(CurrencyIsoCode.USD));
		}

		[Test, Category("exploratory")]
		public void CurrencyIsoCode_IncludedCustomConverterOverStringValue_Currency()
		{
			string stringValue = "\"EGP\"";
			var deserialized = JsonConvert.DeserializeObject<CurrencyIsoCode>(stringValue, new StringEnumConverter());
			Assert.That(deserialized, Is.EqualTo(CurrencyIsoCode.EGP));
		}

		#endregion

		#region Currency

		[Test, Category("exploratory")]
		public void Currency_DefaultDeserialization_UsesCustomSerialization()
		{
			string customValue = "{\"isoCode\":\"EGP\"}";
			var deserialized = JsonConvert.DeserializeObject<Currency>(customValue);

			Assert.That(deserialized, Is.EqualTo(Currency.Get("EGP")));
		}

		[Test]
		public void Currency_DefaultSerialiation_LikeCanonicalJsonSerialization()
		{
			string serializedDollar = "{\"isoCode\":\"USD\"}";
			Assert.That(serializedDollar, Must.Be.DataContractJsonDeserializableInto(Currency.Usd));
			Assert.That(JsonConvert.DeserializeObject<Currency>(serializedDollar), Is.EqualTo(Currency.Usd));
		}

		#endregion

		#region Money

		[Test, Category("exploratory")]
		public void Money_DefaultDeserialization_UsesCustomSerialization()
		{
			string customValue = "{\"amount\":14.3,\"currency\":963}";
			var deserialized = JsonConvert.DeserializeObject<Money>(customValue);

			Assert.That(deserialized, Must.Be.MoneyWith(14.3m, Currency.Get("XTS")));
		}

		[Test]
		public void Money_DefaultDeserialiation_NotLikeCanonicalJsonSerialization()
		{
			string customValue = "{\"amount\":14.3,\"currency\":963}";
			using (var serializer = new OneGoDataContractJsonSerializer<Money>())
			{
				Assert.That(() => JsonConvert.DeserializeObject<Money>(customValue), Throws.Nothing);
				Assert.That(() => serializer.Deserialize(customValue), Throws.InstanceOf<SerializationException>());
			}
		}

		[Test]
		public void Money_CustomConverter_LikeCanonicalJsonSerialization()
		{
			using (var serializer = new OneGoDataContractJsonSerializer<Money>())
			{
				var toSerialize = new Money(14.3m, CurrencyIsoCode.XTS);
				string canonical = serializer.Serialize(toSerialize);
				Assert.That(JsonConvert.DeserializeObject<Money>(canonical, new MoneyConverter()),
					Is.EqualTo(toSerialize));
			}
		}

		#endregion
	}
}
