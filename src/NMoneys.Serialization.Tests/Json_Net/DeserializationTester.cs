using System.Runtime.Serialization;
using Newtonsoft.Json.Serialization;
using NMoneys.Serialization.Json_NET;
using NMoneys.Tests.CustomConstraints;
using NMoneys.Tests.Support;
using NUnit.Framework;
using Newtonsoft.Json;
using Testing.Commons.Serialization;

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
			var serializer = new DataContractJsonDeserializer();
			Assert.That(() => JsonConvert.DeserializeObject<Money>(customValue), Throws.Nothing);
			Assert.That(() => serializer.Deserialize<Money>(customValue), Throws.InstanceOf<SerializationException>());

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
			using (var serializer = new DataContractJsonRoundtripSerializer<Money>(dataContractSurrogate: new DataContractSurrogate()))
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

		[Test]
		public void CustomCurrencyLessConverter_OnlyMoney_ReadsPascalCasedProperties()
		{
			var expected = new Money(14.3m, CurrencyIsoCode.XXX);

			string json = "{\"what_ever\":14.3}";
			var actual = JsonConvert.DeserializeObject<Money>(json, new CurrencyLessMoneyConverter());
			Assert.That(actual, Is.EqualTo(expected));
		}

		[Test]
		public void CustomCurrencyLessConverter_WithDefaultContract_PascalCasedProperties()
		{
			var expected = new Money(14.3m, CurrencyIsoCode.XXX);

			string json = "{\"Name\": \"something\", \"PropName\":14.3}";
			var actual = JsonConvert.DeserializeObject<MoneyContainer>(json, new CurrencyLessMoneyConverter());

			Assert.That(actual.PropName, Is.EqualTo(expected));
		}

		[Test]
		public void CustomCurrencyLessConverter_WithCamelCaseContract_CamelCasedProperties()
		{
			var expected = new Money(14.3m, CurrencyIsoCode.XXX);

			string json = "{\"name\": \"something\", \"propName\":14.3}";
			var actual = JsonConvert.DeserializeObject<MoneyContainer>(json,
				new JsonSerializerSettings
				{
					ContractResolver = new CamelCasePropertyNamesContractResolver(),
					Converters = new[] { new CurrencyLessMoneyConverter() }
				});

			Assert.That(actual.PropName, Is.EqualTo(expected));
		}

		#endregion

		#region nullable

		[Test]
		public void CustomCanonicalConverter_NotNullWithDefaultContract_ReadsPascalCasedProperties()
		{
			var expected = new Money(14.3m, CurrencyIsoCode.XTS);

			string json = "{\"Amount\":14.3,\"Currency\":{\"IsoCode\":\"XTS\"}}";
			var actual = JsonConvert.DeserializeObject<Money?>(json, new CanonicalNullableMoneyConverter());

			Assert.That(actual, Is.EqualTo(expected));
		}

		[Test]
		public void CustomCanonicalConverter_NullWithDefaultContract_ReadsPascalCasedProperties()
		{
			string json = "null";
			var actual = JsonConvert.DeserializeObject<Money?>(json, new CanonicalNullableMoneyConverter());

			Assert.That(actual, Is.Null);
		}

		[Test]
		public void CustomCanonicalConverter_NotNullWithCamelCaseContract_ReadsCamelCasedProperties()
		{
			var expected = new Money(14.3m, CurrencyIsoCode.XTS);

			string json = "{\"amount\":14.3,\"currency\":{\"isoCode\":\"XTS\"}}";
			var settings = new JsonSerializerSettings
			{
				ContractResolver = new CamelCasePropertyNamesContractResolver(),
				Converters = new[] { new CanonicalNullableMoneyConverter() }
			};
			var actual = JsonConvert.DeserializeObject<Money?>(json, settings);

			Assert.That(actual, Is.EqualTo(expected));
		}

		[Test]
		public void CustomCanonicalConverter_NullWithCamelCaseContract_ReadsCamelCasedProperties()
		{

			string json = "null";
			var settings = new JsonSerializerSettings
			{
				ContractResolver = new CamelCasePropertyNamesContractResolver(),
				Converters = new[] { new CanonicalNullableMoneyConverter() }
			};
			var actual = JsonConvert.DeserializeObject<Money?>(json, settings);

			Assert.That(actual, Is.Null);
		}

		[Test]
		public void CustomCanonicalConverter_NotNullContainerWithCamelCaseContract_ReadsCamelCasedProperties()
		{
			var expected = new Money(14.3m, CurrencyIsoCode.XTS);

			string json = "{\"propName\":{\"amount\":14.3,\"currency\":{\"isoCode\":\"XTS\"}}}";
			var settings = new JsonSerializerSettings
			{
				ContractResolver = new CamelCasePropertyNamesContractResolver(),
				Converters = new[] { new CanonicalNullableMoneyConverter() }
			};
			var actual = JsonConvert.DeserializeObject<NullableMoneyContainer>(json, settings);

			Assert.That(actual.PropName, Is.EqualTo(expected));
		}

		[Test]
		public void CustomCanonicalConverter_NullContainerWithCamelCaseContract_ReadsCamelCasedProperties()
		{
			string json = "{\"propName\":null}";
			var settings = new JsonSerializerSettings
			{
				ContractResolver = new CamelCasePropertyNamesContractResolver(),
				Converters = new[] { new CanonicalNullableMoneyConverter() }
			};
			var actual = JsonConvert.DeserializeObject<NullableMoneyContainer>(json, settings);

			Assert.That(actual.PropName, Is.Null);
		}

		[Test]
		public void CustomDefaultConverter_NotNullWithDefaultContractAndCurrencyStyle_ReadsPascalCasedProperties()
		{
			var expected = new Money(14.3m, CurrencyIsoCode.XTS);

			string json = "{\"Amount\":14.3,\"Currency\":\"XTS\"}";
			var actual = JsonConvert.DeserializeObject<Money?>(json, new DefaultNullableMoneyConverter());

			Assert.That(actual, Is.EqualTo(expected));
		}

		[Test]
		public void CustomDefaultConverter_Null_Null()
		{
			string json = "null";
			var actual = JsonConvert.DeserializeObject<Money?>(json, new DefaultNullableMoneyConverter());

			Assert.That(actual, Is.Null);
		}

		[Test]
		public void CustomDefaultConverter_NotNullWithCamelCaseContractAndCurrencyStyle_ReadsCamelCasedProperties()
		{
			var expected = new Money(14.3m, CurrencyIsoCode.XTS);

			string json = "{\"amount\":14.3,\"currency\":\"XTS\"}";
			var settings = new JsonSerializerSettings
			{
				ContractResolver = new CamelCasePropertyNamesContractResolver(),
				Converters = new[] { new DefaultNullableMoneyConverter() }
			};
			var actual = JsonConvert.DeserializeObject<Money?>(json, settings);

			Assert.That(actual, Is.EqualTo(expected));
		}

		[Test]
		public void CustomDefaultConverter_NullWithCamelCaseContractAndCurrencyStyle_ReadsCamelCasedProperties()
		{
			string json = "null";
			var settings = new JsonSerializerSettings
			{
				ContractResolver = new CamelCasePropertyNamesContractResolver(),
				Converters = new[] { new DefaultNullableMoneyConverter() }
			};
			var actual = JsonConvert.DeserializeObject<Money?>(json, settings);

			Assert.That(actual, Is.Null);
		}

		[Test]
		public void CustomDefaultConverter_NotNullNumericStyle_ReadsCurrencyAsNumber()
		{
			var expected = new Money(14.3m, CurrencyIsoCode.XTS);

			string json = "{\"Amount\":14.3,\"Currency\":963}";

			var actual = JsonConvert.DeserializeObject<Money?>(json, new DefaultNullableMoneyConverter(CurrencyStyle.Numeric));

			Assert.That(actual, Is.EqualTo(expected));
		}

		[Test]
		public void CustomDefaultConverter_NullNumericStyle_ReadsCurrencyAsNumber()
		{
			string json = "null";

			var actual = JsonConvert.DeserializeObject<Money?>(json, new DefaultNullableMoneyConverter(CurrencyStyle.Numeric));

			Assert.That(actual, Is.Null);
		}

		[Test]
		public void NullableDefaultConverterAndNonNullableCanonical_CanCoexist()
		{
			var notNull = new Money(14.3m, CurrencyIsoCode.XTS);

			string actualNotNull = "{\"Name\":null,\"PropName\":{\"Amount\":14.3,\"Currency\":\"XTS\"}}";

			var container = JsonConvert.DeserializeObject<MoneyContainer>(actualNotNull,
				new DefaultMoneyConverter(),
				new CanonicalNullableMoneyConverter());
			Assert.That(container.PropName, Is.EqualTo(notNull));


			string actualNull = "{\"PropName\":null}";
			var nullableContainer = JsonConvert.DeserializeObject<NullableMoneyContainer>(actualNull,
				new DefaultMoneyConverter(),
				new CanonicalNullableMoneyConverter());

			Assert.That(nullableContainer.PropName, Is.Null);
		}

		#endregion
	}
}
