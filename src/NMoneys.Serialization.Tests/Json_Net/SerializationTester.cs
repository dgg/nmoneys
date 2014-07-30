using Newtonsoft.Json.Serialization;
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
		#region Money

		[Test, Category("exploratory")]
		public void DefaultSerialization_UsesCustomSerializationForMemberNames()
		{
			var toSerialize = new Money(14.3m, CurrencyIsoCode.XTS);
			string @default = JsonConvert.SerializeObject(toSerialize);

			string expected = "{\"amount\":14.3,\"currency\":963}";
			Assert.That(@default, Is.EqualTo(expected));
		}

		[Test]
		public void DefaultSerialiation_NotLikeCanonicalJsonSerialization()
		{
			using (var serializer = new OneGoDataContractJsonSerializer<Money>())
			{
				var toSerialize = new Money(14.3m, CurrencyIsoCode.XTS);

				string @default = JsonConvert.SerializeObject(toSerialize);

				string canonical = serializer.Serialize(toSerialize);
				Assert.That(@default, Is.Not.EqualTo(canonical));
			}
		}

		[Test]
		public void CustomCanonicalConverter_DefaultContractResolver_UsesPascalCasedPropertyNamesAndAlphabeticCode()
		{
			var toSerialize = new Money(14.3m, CurrencyIsoCode.XTS);

			string actual = JsonConvert.SerializeObject(toSerialize, new CanonicalMoneyConverter());
			Assert.That(actual, Is.EqualTo("{\"Amount\":14.3,\"Currency\":{\"IsoCode\":\"XTS\"}}"));
		}

		[Test]
		public void CustomCanonicalConverter_CamelCaseContractResolver_UsesCamelCasedPropertyNamesAndAlphabeticCode()
		{
			var toSerialize = new Money(14.3m, CurrencyIsoCode.XTS);

			var settings = new JsonSerializerSettings
			{
				ContractResolver = new CamelCasePropertyNamesContractResolver(),
				Converters = new[] { new CanonicalMoneyConverter() }
			};

			string actual = JsonConvert.SerializeObject(toSerialize, settings);
			Assert.That(actual, Is.EqualTo("{\"amount\":14.3,\"currency\":{\"isoCode\":\"XTS\"}}"));
		}

		[Test]
		public void CustomCanonicalConverterWithCamelContract_LikeCanonicalJsonSerialization()
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

				string actual = JsonConvert.SerializeObject(toSerialize, settings);
				Assert.That(actual, Is.EqualTo(canonical));
			}
		}

		[Test]
		public void CustomDefaultConverter_DefaultContractResolver_UsesPascalCasedPropertyNamesAndAlphabeticCode()
		{
			var toSerialize = new Money(14.3m, CurrencyIsoCode.XTS);

			string actual = JsonConvert.SerializeObject(toSerialize, new DefaultMoneyConverter());
			Assert.That(actual, Is.EqualTo("{\"Amount\":14.3,\"Currency\":\"XTS\"}"));
		}

		[Test]
		public void CustomDefaultConverter_CamelCaseContractResolver_UsesCamelCasedPropertyNamesAndAlphabeticCode()
		{
			var toSerialize = new Money(14.3m, CurrencyIsoCode.XTS);

			var settings = new JsonSerializerSettings
			{
				ContractResolver = new CamelCasePropertyNamesContractResolver(),
				Converters = new[] { new DefaultMoneyConverter() }
			};

			string actual = JsonConvert.SerializeObject(toSerialize, settings);
			Assert.That(actual, Is.EqualTo("{\"amount\":14.3,\"currency\":\"XTS\"}"));
		}

		[Test]
		public void CustomDefaultConverter_NumericStyle_UsesNumericCode()
		{
			var toSerialize = new Money(14.3m, CurrencyIsoCode.XTS);

			string actual = JsonConvert.SerializeObject(toSerialize,
				new DefaultMoneyConverter(CurrencyStyle.Numeric));
			Assert.That(actual, Is.EqualTo("{\"Amount\":14.3,\"Currency\":963}"));
		}

		[Test]
		public void CustomDefaultConverter_NumericStyle_TakesPrecedenceOverHowEnumsGetSerialized()
		{
			var toSerialize = new Money(14.3m, CurrencyIsoCode.XTS);

			string actual = JsonConvert.SerializeObject(toSerialize,
				new StringEnumConverter(),
				new DefaultMoneyConverter(CurrencyStyle.Numeric));
			Assert.That(actual, Is.EqualTo("{\"Amount\":14.3,\"Currency\":963}"));
		}

		#endregion
	}
}
