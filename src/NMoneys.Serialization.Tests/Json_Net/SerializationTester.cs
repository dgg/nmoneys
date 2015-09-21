using System;
using Newtonsoft.Json.Serialization;
using NMoneys.Serialization.Json_NET;
using NMoneys.Tests.Support;
using NUnit.Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Testing.Commons.Serialization;

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
			using (var serializer = new DataContractJsonRoundtripSerializer<Money>(dataContractSurrogate: new DataContractSurrogate()))
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
			using (var serializer = new DataContractJsonRoundtripSerializer<Money>(dataContractSurrogate: new DataContractSurrogate()))
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

		[Test]
		public void CustomCurrencyLessConverter_ContainerObject_Exception()
		{
			var toSerialize = new MoneyContainer { PropName = new Money(14.3m, CurrencyIsoCode.XTS) };

			Assert.That(() => JsonConvert.SerializeObject(toSerialize, new CurrencyLessMoneyConverter()),
				Throws.InstanceOf<NotImplementedException>());
		}

		[Test]
		public void CustomCurrencyLessConverter_MoneyInstance_Exception()
		{
			var toSerialize = new Money(14.3m, CurrencyIsoCode.XTS);

			Assert.That(() => JsonConvert.SerializeObject(toSerialize, new CurrencyLessMoneyConverter()),
				Throws.InstanceOf<NotImplementedException>());
		}

		#endregion

		#region nullables

		[Test]
		public void CustomCanonicalConverter_NotNullDefaultContractResolver_UsesPascalCasedPropertyNamesAndAlphabeticCode()
		{
			Money? notNull = new Money(14.3m, CurrencyIsoCode.XTS);

			string actual = JsonConvert.SerializeObject(notNull, new CanonicalNullableMoneyConverter());
			Assert.That(actual, Is.EqualTo("{\"Amount\":14.3,\"Currency\":{\"IsoCode\":\"XTS\"}}"));
		}

		[Test]
		public void CustomCanonicalConverter_NullDefaultContractResolver_UsesPascalCasedPropertyNamesAndAlphabeticCode()
		{
			Money? @null = default(Money?);

			string actual = JsonConvert.SerializeObject(@null, new CanonicalNullableMoneyConverter());
			Assert.That(actual, Is.EqualTo("null"));
		}

		[Test]
		public void CustomCanonicalConverter_NotNullCamelCaseContractResolver_UsesCamelCasedPropertyNamesAndAlphabeticCode()
		{
			Money? notNull = new Money(14.3m, CurrencyIsoCode.XTS);

			var settings = new JsonSerializerSettings
			{
				ContractResolver = new CamelCasePropertyNamesContractResolver(),
				Converters = new[] { new CanonicalNullableMoneyConverter() }
			};

			string actual = JsonConvert.SerializeObject(notNull, settings);
			Assert.That(actual, Is.EqualTo("{\"amount\":14.3,\"currency\":{\"isoCode\":\"XTS\"}}"));
		}

		[Test]
		public void CustomCanonicalConverter_NullCamelCaseContractResolver_Null()
		{
			Money? @null = default(Money?);

			var settings = new JsonSerializerSettings
			{
				ContractResolver = new CamelCasePropertyNamesContractResolver(),
				Converters = new[] { new CanonicalNullableMoneyConverter() }
			};

			string actual = JsonConvert.SerializeObject(@null, settings);
			Assert.That(actual, Is.EqualTo("null"));
		}

		[Test]
		public void CustomCanonicalConverter_NotNullContainer_UsesCamelCasedPropertyNamesAndAlphabeticCode()
		{
			var notNull = new NullableMoneyContainer { PropName = new Money(14.3m, CurrencyIsoCode.XTS) };

			var settings = new JsonSerializerSettings
			{
				ContractResolver = new CamelCasePropertyNamesContractResolver(),
				Converters = new[] { new CanonicalNullableMoneyConverter() }
			};

			string actual = JsonConvert.SerializeObject(notNull, settings);
			Assert.That(actual, Is.EqualTo("{\"propName\":{\"amount\":14.3,\"currency\":{\"isoCode\":\"XTS\"}}}"));
		}

		[Test]
		public void CustomCanonicalConverter_NullContainer_Null()
		{
			var notNull = new NullableMoneyContainer { PropName = default(Money?) };

			var settings = new JsonSerializerSettings
			{
				ContractResolver = new CamelCasePropertyNamesContractResolver(),
				Converters = new[] { new CanonicalNullableMoneyConverter() }
			};

			string actual = JsonConvert.SerializeObject(notNull, settings);
			Assert.That(actual, Is.EqualTo("{\"propName\":null}"));
		}

		[Test]
		public void CustomDefaultConverter_NotNullDefaultContractResolver_UsesPascalCasedPropertyNamesAndAlphabeticCode()
		{
			Money? notNull = new Money(14.3m, CurrencyIsoCode.XTS);

			string actual = JsonConvert.SerializeObject(notNull, new DefaultNullableMoneyConverter());
			Assert.That(actual, Is.EqualTo("{\"Amount\":14.3,\"Currency\":\"XTS\"}"));
		}

		[Test]
		public void CustomDefaultConverter_NullDefaultContractResolver_Null()
		{
			Money? @null = default(Money?);

			string actual = JsonConvert.SerializeObject(@null, new DefaultNullableMoneyConverter());
			Assert.That(actual, Is.EqualTo("null"));
		}

		[Test]
		public void CustomDefaultConverter_NotNullCamelCaseContractResolver_UsesCamelCasedPropertyNamesAndAlphabeticCode()
		{
			Money? notNull = new Money(14.3m, CurrencyIsoCode.XTS);

			var settings = new JsonSerializerSettings
			{
				ContractResolver = new CamelCasePropertyNamesContractResolver(),
				Converters = new[] { new DefaultNullableMoneyConverter() }
			};

			string actual = JsonConvert.SerializeObject(notNull, settings);
			Assert.That(actual, Is.EqualTo("{\"amount\":14.3,\"currency\":\"XTS\"}"));
		}

		[Test]
		public void CustomDefaultConverter_NullCamelCaseContractResolver_Null()
		{
			Money? @null = default(Money?);

			var settings = new JsonSerializerSettings
			{
				ContractResolver = new CamelCasePropertyNamesContractResolver(),
				Converters = new[] { new DefaultNullableMoneyConverter() }
			};

			string actual = JsonConvert.SerializeObject(@null, settings);
			Assert.That(actual, Is.EqualTo("null"));
		}

		[Test]
		public void CustomDefaultConverter_NotNullNumericStyle_UsesNumericCode()
		{
			Money? notNull = new Money(14.3m, CurrencyIsoCode.XTS);

			string actual = JsonConvert.SerializeObject(notNull,
				new DefaultNullableMoneyConverter(CurrencyStyle.Numeric));
			Assert.That(actual, Is.EqualTo("{\"Amount\":14.3,\"Currency\":963}"));
		}

		[Test]
		public void CustomDefaultConverter_NotNullContainer_NotNullProperty()
		{
			var notNull = new NullableMoneyContainer { PropName = new Money(14.3m, CurrencyIsoCode.XTS) };

			string actual = JsonConvert.SerializeObject(notNull,
				new DefaultNullableMoneyConverter(CurrencyStyle.Numeric));
			Assert.That(actual, Is.EqualTo("{\"PropName\":{\"Amount\":14.3,\"Currency\":963}}"));
		}

		[Test]
		public void CustomDefaultConverter_NullContainer_NullProperty()
		{
			var @null = new NullableMoneyContainer { PropName = default(Money?) };

			string actual = JsonConvert.SerializeObject(@null,
				new DefaultNullableMoneyConverter(CurrencyStyle.Numeric));
			Assert.That(actual, Is.EqualTo("{\"PropName\":null}"));
		}

		[Test]
		public void NullableDefaultConverterAndNonNullableCanonical_CanCoexist()
		{
			var notNull = new MoneyContainer { PropName = new Money(14.3m, CurrencyIsoCode.XTS) };
			var @null = new NullableMoneyContainer { PropName = default(Money?) };

			string actualNull = JsonConvert.SerializeObject(@null,
				new DefaultMoneyConverter(),
				new CanonicalNullableMoneyConverter());

			string actualNotNull = JsonConvert.SerializeObject(notNull,
				new DefaultMoneyConverter(),
				new CanonicalNullableMoneyConverter());

			Assert.That(actualNotNull, Is.EqualTo("{\"Name\":null,\"PropName\":{\"Amount\":14.3,\"Currency\":\"XTS\"}}"));

			Assert.That(actualNull, Is.EqualTo("{\"PropName\":null}"));
		}

		#endregion
	}
}
