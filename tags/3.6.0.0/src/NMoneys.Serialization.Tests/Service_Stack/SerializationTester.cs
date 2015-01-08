using NMoneys.Serialization.Service_Stack;
using NMoneys.Serialization.Tests.Service_Stack.Support;
using NMoneys.Tests.Support;
using NUnit.Framework;
using ServiceStack.Text;

namespace NMoneys.Serialization.Tests.Service_Stack
{
	[TestFixture]
	public class SerializationTester
	{
		[Test, Category("exploratory")]
		public void DefaultSerialization_StringWithToString()
		{
			Money money = new Money(12.3m, CurrencyIsoCode.XTS);
			string s = JsonSerializer.SerializeToString(money);

			Assert.That(s, Is.EqualTo("\"¤12.30\""));
		}

		[Test]
		public void DefaultSerialiation_NotLikeCanonicalJsonSerialization()
		{
			using (var serializer = new OneGoDataContractJsonSerializer<Money>())
			{
				var toSerialize = new Money(14.3m, CurrencyIsoCode.XTS);

				string @default = JsonSerializer.SerializeToString(toSerialize);

				string canonical = serializer.Serialize(toSerialize);
				Assert.That(@default, Is.Not.EqualTo(canonical));
			}
		}

		[Test]
		public void CustomCanonicalConverter_DefaultConfiguration_UsesPascalCasedPropertyNamesAndAlphabeticCode()
		{
			var toSerialize = new Money(14.3m, CurrencyIsoCode.XTS);

			JsConfig<Money>.RawSerializeFn = CanonicalMoneySerializer.Serialize;

			string actual = JsonSerializer.SerializeToString(toSerialize);
			Assert.That(actual, Is.EqualTo("{\"Amount\":14.3,\"Currency\":{\"IsoCode\":\"XTS\"}}"));	
			
			JsConfig<Money>.Reset();
		}

		[Test]
		public void CustomCanonicalConverter_Container_UsesPascalCasedPropertyNamesAndAlphabeticCode()
		{
			var toSerialize = new MoneyContainer {PropName = new Money(14.3m, CurrencyIsoCode.XTS)};

			JsConfig<Money>.RawSerializeFn = CanonicalMoneySerializer.Serialize;

			string actual = JsonSerializer.SerializeToString(toSerialize);
			Assert.That(actual, Is.EqualTo("{\"PropName\":{\"Amount\":14.3,\"Currency\":{\"IsoCode\":\"XTS\"}}}"));

			JsConfig<Money>.Reset();
		}

		[Test]
		public void CustomCanonicalConverter_CamelCase_UsesCamelCasedPropertyNamesAndAlphabeticCode()
		{
			var toSerialize = new Money(14.3m, CurrencyIsoCode.XTS);

			using (new ConfigScope(CanonicalMoneySerializer.Serialize, JsConfig.With(emitCamelCaseNames: true)))
			{
				string actual = JsonSerializer.SerializeToString(toSerialize);
				Assert.That(actual, Is.EqualTo("{\"amount\":14.3,\"currency\":{\"isoCode\":\"XTS\"}}"));
			}
		}

		[Test]
		public void CustomCanonicalConverter_WithCamelCasing_LikeCanonicalJsonSerialization()
		{
			using (var serializer = new OneGoDataContractJsonSerializer<Money>())
			{
				var toSerialize = new Money(14.3m, CurrencyIsoCode.XTS);
				string canonical = serializer.Serialize(toSerialize);

				using (new ConfigScope(CanonicalMoneySerializer.Serialize, JsConfig.With(emitCamelCaseNames: true)))
				{
					string actual = JsonSerializer.SerializeToString(toSerialize);
					Assert.That(actual, Is.EqualTo(canonical));
				}
			}
		}

		[Test]
		public void CustomDefaultConverter_DefaultConfiguration_UsesPascalCasedPropertyNamesAndAlphabeticCode()
		{
			var toSerialize = new Money(14.3m, CurrencyIsoCode.XTS);

			using (new ConfigScope(DefaultMoneySerializer.Serialize))
			{
				string actual = JsonSerializer.SerializeToString(toSerialize);
				Assert.That(actual, Is.EqualTo("{\"Amount\":14.3,\"Currency\":\"XTS\"}"));	
			}
		}

		[Test]
		public void CustomDefaultConverter_CamelCasing_UsesCamelCasedPropertyNamesAndAlphabeticCode()
		{
			var toSerialize = new Money(14.3m, CurrencyIsoCode.XTS);

			using (new ConfigScope(DefaultMoneySerializer.Serialize, JsConfig.With(emitCamelCaseNames: true)))
			{
				string actual = JsonSerializer.SerializeToString(toSerialize);
				Assert.That(actual, Is.EqualTo("{\"amount\":14.3,\"currency\":\"XTS\"}"));	
			}
		}

		[Test]
		public void CustomDefaultConverter_NumericStyle_UsesNumericCode()
		{
			var toSerialize = new Money(14.3m, CurrencyIsoCode.XTS);

			using (new ConfigScope(DefaultMoneySerializer.Numeric.Serialize))
			{
				string actual = JsonSerializer.SerializeToString(toSerialize);
				Assert.That(actual, Is.EqualTo("{\"Amount\":14.3,\"Currency\":963}"));
			}
		}

		[Test]
		public void CustomDefaultConverter_NumericStyle_TakesPrecedenceOverHowEnumsGetSerialized()
		{
			var toSerialize = new Money(14.3m, CurrencyIsoCode.XTS);

			using (new ConfigScope(DefaultMoneySerializer.Numeric.Serialize, JsConfig.With(treatEnumAsInteger: false)))
			{
				string actual = JsonSerializer.SerializeToString(toSerialize);
				Assert.That(actual, Is.EqualTo("{\"Amount\":14.3,\"Currency\":963}"));
			}

			using (new ConfigScope(DefaultMoneySerializer.Serialize, JsConfig.With(treatEnumAsInteger: true)))
			{
				string actual = JsonSerializer.SerializeToString(toSerialize);
				Assert.That(actual, Is.EqualTo("{\"Amount\":14.3,\"Currency\":\"XTS\"}"));
			}
		}

		#region nullable

		[Test]
		public void CustomCanonicalConverter_NotNullDefaultConfiguration_UsesPascalCasedPropertyNamesAndAlphabeticCode()
		{
			Money? notNull = new Money(14.3m, CurrencyIsoCode.XTS);

			JsConfig<Money?>.RawSerializeFn = CanonicalNullableMoneySerializer.Serialize;

			string actual = JsonSerializer.SerializeToString(notNull);
			Assert.That(actual, Is.EqualTo("{\"Amount\":14.3,\"Currency\":{\"IsoCode\":\"XTS\"}}"));

			JsConfig<Money?>.Reset();
		}

		[Test]
		public void CustomCanonicalConverter_NullDefaultConfiguration_UsesPascalCasedPropertyNamesAndAlphabeticCode()
		{
			Money? @null = default(Money?);

			JsConfig<Money?>.RawSerializeFn = CanonicalNullableMoneySerializer.Serialize;

			string actual = JsonSerializer.SerializeToString(@null);
			Assert.That(actual, Is.Null);

			JsConfig<Money?>.Reset();
		}

		[Test]
		public void CustomCanonicalConverter_NotNullContainer_UsesPascalCasedPropertyNamesAndAlphabeticCode()
		{
			var notNull = new NullableMoneyContainer { PropName = new Money(14.3m, CurrencyIsoCode.XTS) };

			JsConfig<Money?>.RawSerializeFn = CanonicalNullableMoneySerializer.Serialize;

			string actual = JsonSerializer.SerializeToString(notNull);
			Assert.That(actual, Is.EqualTo("{\"PropName\":{\"Amount\":14.3,\"Currency\":{\"IsoCode\":\"XTS\"}}}"));

			JsConfig<Money?>.Reset();
		}

		[Test]
		public void CustomCanonicalConverter_NullContainer_UsesPascalCasedPropertyNamesAndAlphabeticCode()
		{
			var @null = new NullableMoneyContainer { PropName = default(Money?) };

			JsConfig<Money?>.RawSerializeFn = CanonicalNullableMoneySerializer.Serialize;

			string actual = JsonSerializer.SerializeToString(@null);
			Assert.That(actual, Is.EqualTo("{}"));

			JsConfig<Money?>.Reset();
		}

		[Test]
		public void CustomCanonicalConverter_NotNullCamelCase_UsesCamelCasedPropertyNamesAndAlphabeticCode()
		{
			Money? notNull = new Money(14.3m, CurrencyIsoCode.XTS);

			using (new NullableConfigScope(CanonicalNullableMoneySerializer.Serialize, JsConfig.With(emitCamelCaseNames: true)))
			{
				string actual = JsonSerializer.SerializeToString(notNull);
				Assert.That(actual, Is.EqualTo("{\"amount\":14.3,\"currency\":{\"isoCode\":\"XTS\"}}"));
			}
		}

		[Test]
		public void CustomCanonicalConverter_NullCamelCase_UsesCamelCasedPropertyNamesAndAlphabeticCode()
		{
			Money? @null = default(Money?);

			using (new NullableConfigScope(CanonicalNullableMoneySerializer.Serialize, JsConfig.With(emitCamelCaseNames: true)))
			{
				string actual = JsonSerializer.SerializeToString(@null);
				Assert.That(actual, Is.Null);
			}
		}

		[Test]
		public void CustomDefaultConverter_NotNullDefaultConfiguration_UsesPascalCasedPropertyNamesAndAlphabeticCode()
		{
			Money? notNull = new Money(14.3m, CurrencyIsoCode.XTS);

			using (new NullableConfigScope(DefaultNullableMoneySerializer.Serialize))
			{
				string actual = JsonSerializer.SerializeToString(notNull);
				Assert.That(actual, Is.EqualTo("{\"Amount\":14.3,\"Currency\":\"XTS\"}"));
			}
		}

		[Test]
		public void CustomDefaultConverter_NullDefaultConfiguration_UsesPascalCasedPropertyNamesAndAlphabeticCode()
		{
			Money? @null = default(Money?);

			using (new NullableConfigScope(DefaultNullableMoneySerializer.Serialize))
			{
				string actual = JsonSerializer.SerializeToString(@null);
				Assert.That(actual, Is.Null);
			}
		}

		[Test]
		public void CustomDefaultConverter_NotNullCamelCasing_UsesCamelCasedPropertyNamesAndAlphabeticCode()
		{
			Money? notNull = new Money(14.3m, CurrencyIsoCode.XTS);

			using (new NullableConfigScope(DefaultNullableMoneySerializer.Serialize, JsConfig.With(emitCamelCaseNames: true)))
			{
				string actual = JsonSerializer.SerializeToString(notNull);
				Assert.That(actual, Is.EqualTo("{\"amount\":14.3,\"currency\":\"XTS\"}"));
			}
		}

		[Test]
		public void CustomDefaultConverter_NullCamelCasing_UsesCamelCasedPropertyNamesAndAlphabeticCode()
		{
			Money? @null = default(Money?);

			using (new NullableConfigScope(DefaultNullableMoneySerializer.Serialize, JsConfig.With(emitCamelCaseNames: true)))
			{
				string actual = JsonSerializer.SerializeToString(@null);
				Assert.That(actual, Is.Null);
			}
		}

		[Test]
		public void CustomDefaultConverter_NotNullNumericStyle_UsesNumericCode()
		{
			Money? notNull = new Money(14.3m, CurrencyIsoCode.XTS);

			using (new NullableConfigScope(DefaultNullableMoneySerializer.Numeric.Serialize))
			{
				string actual = JsonSerializer.SerializeToString(notNull);
				Assert.That(actual, Is.EqualTo("{\"Amount\":14.3,\"Currency\":963}"));
			}
		}

		[Test]
		public void CustomDefaultConverter_NullNumericStyle_UsesNumericCode()
		{
			Money? @null = default(Money?);

			using (new NullableConfigScope(DefaultNullableMoneySerializer.Numeric.Serialize))
			{
				string actual = JsonSerializer.SerializeToString(@null);
				Assert.That(actual, Is.Null);
			}
		}

		[Test]
		public void CustomDefaultConverter_NotNullContainer_UsesPascalCasedPropertyNamesAndAlphabeticCode()
		{
			var notNull = new NullableMoneyContainer { PropName = new Money(14.3m, CurrencyIsoCode.XTS) };

			JsConfig<Money?>.RawSerializeFn = DefaultNullableMoneySerializer.Serialize;

			string actual = JsonSerializer.SerializeToString(notNull);
			Assert.That(actual, Is.EqualTo("{\"PropName\":{\"Amount\":14.3,\"Currency\":\"XTS\"}}"));

			JsConfig<Money?>.Reset();
		}

		[Test]
		public void CustomDefaultConverter_NullContainer_UsesPascalCasedPropertyNamesAndAlphabeticCode()
		{
			var @null = new NullableMoneyContainer { PropName = default(Money?) };

			JsConfig<Money?>.RawSerializeFn = DefaultNullableMoneySerializer.Serialize;

			string actual = JsonSerializer.SerializeToString(@null);
			Assert.That(actual, Is.EqualTo("{}"));

			JsConfig<Money?>.Reset();
		}

		#endregion
	}
}