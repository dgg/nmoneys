using NMoneys.Serialization.Service_Stack;
using NMoneys.Serialization.Tests.Service_Stack.Support;
using NMoneys.Tests.Support;
using NUnit.Framework;
using ServiceStack.Text;

namespace NMoneys.Serialization.Tests.Service_Stack
{
	[TestFixture]
	public class DeserializationTester
	{
		[Test, Category("exploratory")]
		public void DefaultDeserialization_IsDangerous()
		{
			Assert.Ignore("One cannot get a reliable conversion from string currency representation");
		}

		[Test]
		public void CustomCanonicalConverter_WithDefaultConfiguration_ReadsPascalCasedProperties()
		{
			var expected = new Money(14.3m, CurrencyIsoCode.XTS);

			string json = "{\"Amount\":14.3,\"Currency\":{\"IsoCode\":\"XTS\"}}";

			JsConfig<Money>.RawDeserializeFn = CanonicalMoneySerializer.Deserialize;

			var actual = JsonSerializer.DeserializeFromString<Money>(json);

			JsConfig<Money>.Reset();
			Assert.That(actual, Is.EqualTo(expected));
		}

		[Test]
		public void CustomCanonicalConverter_WithCamelCasing_ReadsCamelCasedProperties()
		{
			var expected = new Money(14.3m, CurrencyIsoCode.XTS);

			string json = "{\"amount\":14.3,\"currency\":{\"isoCode\":\"XTS\"}}";

			using (new ConfigScope(CanonicalMoneySerializer.Deserialize, JsConfig.With(emitCamelCaseNames: true)))
			{
				var actual = JsonSerializer.DeserializeFromString<Money>(json);

				Assert.That(actual, Is.EqualTo(expected));
			}
		}

		[Test]
		public void CustomCanonicalConverter_WithCamelCase_LikeCanonicalJsonSerialization()
		{
			using (var serializer = new OneGoDataContractJsonSerializer<Money>())
			{
				var toSerialize = new Money(14.3m, CurrencyIsoCode.XTS);
				string canonical = serializer.Serialize(toSerialize);

				using (new ConfigScope(CanonicalMoneySerializer.Deserialize, JsConfig.With(emitCamelCaseNames: true)))
				{
					Assert.That(JsonSerializer.DeserializeFromString<Money>(canonical),
						Is.EqualTo(toSerialize));
				}
			}
		}

		[Test]
		public void CustomDefaultConverter_WithDefaultConfiguration_ReadsPascalCasedProperties()
		{
			var expected = new Money(14.3m, CurrencyIsoCode.XTS);

			string json = "{\"Amount\":14.3,\"Currency\":\"XTS\"}";

			using (new ConfigScope(DefaultMoneySerializer.Deserialize))
			{
				var actual = JsonSerializer.DeserializeFromString<Money>(json);

				Assert.That(actual, Is.EqualTo(expected));
			}
		}

		[Test]
		public void CustomDefaultConverter_WithCamelCase_ReadsCamelCasedProperties()
		{
			var expected = new Money(14.3m, CurrencyIsoCode.XTS);

			string json = "{\"amount\":14.3,\"currency\":\"XTS\"}";
			using (new ConfigScope(DefaultMoneySerializer.Deserialize, JsConfig.With(emitCamelCaseNames: true)))
			{
				var actual = JsonSerializer.DeserializeFromString<Money>(json);

				Assert.That(actual, Is.EqualTo(expected));
			}
		}

		[Test]
		public void CustomDefaultConverter_NumericStyle_ReadsCurrencyAsNumber()
		{
			var expected = new Money(14.3m, CurrencyIsoCode.XTS);

			string json = "{\"Amount\":14.3,\"Currency\":963}";

			using (new ConfigScope(DefaultMoneySerializer.Numeric.Deserialize, JsConfig.With(emitCamelCaseNames: false)))
			{
				var actual = JsonSerializer.DeserializeFromString<Money>(json);

				Assert.That(actual, Is.EqualTo(expected));
			}
		}

		[Test]
		public void CustomCurrencyLessConverter_OnlyMoney_ReadsPascalCasedProperties()
		{
			var expected = new Money(14.3m, CurrencyIsoCode.XXX);

			string json = "{\"what_ever\":14.3}";

			using (new ConfigScope(CurrencyLessMoneySerializer.Deserialize, JsConfig.With(emitCamelCaseNames: false)))
			{
				var actual = JsonSerializer.DeserializeFromString<Money>(json);
				Assert.That(actual, Is.EqualTo(expected));
			}
		}

		[Test]
		public void CustomCurrencyLessConverter_WithDefaultConfiguration_ReadsPascalCasedProperties()
		{
			var expected = new Money(14.3m, CurrencyIsoCode.XXX);

			string json = "{\"Name\": \"something\", \"PropName\":14.3}";

			using (new ConfigScope(CurrencyLessMoneySerializer.Deserialize, JsConfig.With(emitCamelCaseNames: false)))
			{
				var actual = JsonSerializer.DeserializeFromString<MoneyContainer>(json);

				Assert.That(actual.PropName, Is.EqualTo(expected));
			}
		}

		[Test]
		public void CustomCurrencyLessConverter_WithCamelCase_ReadsCamelCasedProperties()
		{
			var expected = new Money(14.3m, CurrencyIsoCode.XXX);

			string json = "{\"Name\": \"something\", \"PropName\":14.3}";
			using (new ConfigScope(CurrencyLessMoneySerializer.Deserialize, JsConfig.With(emitCamelCaseNames: true)))
			{
				var actual = JsonSerializer.DeserializeFromString<MoneyContainer>(json);

				Assert.That(actual.PropName, Is.EqualTo(expected));
			}
		}
	}
}