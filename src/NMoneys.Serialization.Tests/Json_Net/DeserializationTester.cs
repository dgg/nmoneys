using System.Runtime.Serialization;
using Newtonsoft.Json.Serialization;
using NMoneys.Serialization.Json_NET;
using NMoneys.Tests.CustomConstraints;
using NMoneys.Tests.Support;
using NUnit.Framework;
using Newtonsoft.Json;

namespace NMoneys.Serialization.Tests.Json_Net
{
	[TestFixture]
	public class DeserializationTester
	{
		#region Money

		[Test, Category("exploratory")]
		public void DefaultDeserialization_UsesCustomSerialization()
		{
			string customValue = "{\"amount\":14.3,\"currency\":963}";
			var deserialized = JsonConvert.DeserializeObject<Money>(customValue);

			Assert.That(deserialized, Must.Be.MoneyWith(14.3m, Currency.Get("XTS")));
		}

		[Test]
		public void DefaultDeserialiation_NotLikeCanonicalJsonSerialization()
		{
			string customValue = "{\"amount\":14.3,\"currency\":963}";
			using (var serializer = new OneGoDataContractJsonSerializer<Money>())
			{
				Assert.That(() => JsonConvert.DeserializeObject<Money>(customValue), Throws.Nothing);
				Assert.That(() => serializer.Deserialize(customValue), Throws.InstanceOf<SerializationException>());
			}
		}

		[Test]
		public void CustomCanonicalConverter_WithDefaultContract_ReadsPascalCasedProperties()
		{
			var expected = new Money(14.3m, CurrencyIsoCode.XTS);

			string json = "{\"Amount\":14.3,\"Currency\":{\"IsoCode\":\"XTS\"}}";
			var actual = JsonConvert.DeserializeObject<Money>(json, new CanonicalMoneyConverter());

			Assert.That(actual, Is.EqualTo(expected));
		}

		[Test]
		public void CustomCanonicalConverter_WithCamelCaseContract_ReadsCamelCasedProperties()
		{
			var expected = new Money(14.3m, CurrencyIsoCode.XTS);

			string json = "{\"amount\":14.3,\"currency\":{\"isoCode\":\"XTS\"}}";
			var settings = new JsonSerializerSettings
			{
				ContractResolver = new CamelCasePropertyNamesContractResolver(),
				Converters = new[] { new CanonicalMoneyConverter() }
			};
			var actual = JsonConvert.DeserializeObject<Money>(json, settings);

			Assert.That(actual, Is.EqualTo(expected));
		}

		[Test]
		public void CustomCanonicalConverter_WithCamelCaseContract_LikeCanonicalJsonSerialization()
		{
			using (var serializer = new OneGoDataContractJsonSerializer<Money>())
			{
				var toSerialize = new Money(14.3m, CurrencyIsoCode.XTS);
				string canonical = serializer.Serialize(toSerialize);

				var settings = new JsonSerializerSettings
				{
					ContractResolver = new CamelCasePropertyNamesContractResolver(),
					Converters = new[] { new CanonicalMoneyConverter() }
				};
				Assert.That(JsonConvert.DeserializeObject<Money>(canonical, settings),
					Is.EqualTo(toSerialize));
			}
		}

		[Test]
		public void CustomDefaultConverter_WithDefaultContractAndCurrencyStyle_ReadsPascalCasedProperties()
		{
			var expected = new Money(14.3m, CurrencyIsoCode.XTS);

			string json = "{\"Amount\":14.3,\"Currency\":\"XTS\"}";
			var actual = JsonConvert.DeserializeObject<Money>(json, new DefaultMoneyConverter());

			Assert.That(actual, Is.EqualTo(expected));
		}

		[Test]
		public void CustomDefaultConverter_WithCamelCaseContractAndCurrencyStyle_ReadsCamelCasedProperties()
		{
			var expected = new Money(14.3m, CurrencyIsoCode.XTS);

			string json = "{\"amount\":14.3,\"currency\":\"XTS\"}";
			var settings = new JsonSerializerSettings
			{
				ContractResolver = new CamelCasePropertyNamesContractResolver(),
				Converters = new[] { new DefaultMoneyConverter() }
			};
			var actual = JsonConvert.DeserializeObject<Money>(json, settings);

			Assert.That(actual, Is.EqualTo(expected));
		}

		[Test]
		public void CustomDefaultConverter_NumericStyle_ReadsCurrencyAsNumber()
		{
			var expected = new Money(14.3m, CurrencyIsoCode.XTS);

			string json = "{\"Amount\":14.3,\"Currency\":963}";
			
			var actual = JsonConvert.DeserializeObject<Money>(json, new DefaultMoneyConverter(CurrencyStyle.Numeric));

			Assert.That(actual, Is.EqualTo(expected));
		}

		#endregion
	}
}
