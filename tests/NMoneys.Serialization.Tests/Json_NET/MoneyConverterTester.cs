using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

using NMoneys.Serialization.Tests.Support;
using NMoneys.Serialization.Json_NET;

namespace NMoneys.Serialization.Tests.Json_NET;

// TODO: test nullable conversions

[TestFixture]
public class MoneyConverterTester
{
	#region serialization
	[Test, Category("exploratory")]
	public void OutOfTheBoxSerialization_BlowsUp()
	{
		var toSerialize = new Money(14.3m, CurrencyIsoCode.XTS);

		Assert.That(()=> JsonConvert.SerializeObject(toSerialize), Throws.InstanceOf<JsonSerializationException>());
	}

	[Test]
	public void DefaultContractResolver_UsesPascalCasedPropertyNamesAndNumericCurrency()
	{
		var toSerialize = new Money(14.3m, CurrencyIsoCode.XTS);

		string actual = JsonConvert.SerializeObject(toSerialize, new MoneyConverter());
		Assert.That(actual, Is.EqualTo("{\"Amount\":14.3,\"Currency\":963}"));
	}

	[Test]
	public void DefaultContractResolver_ForceStringEnum_UsesPascalCasedPropertyNamesAndNumericCurrency()
	{
		var toSerialize = new Money(14.3m, CurrencyIsoCode.XTS);

		string actual = JsonConvert.SerializeObject(toSerialize, new MoneyConverter(forceStringEnum: true));
		Assert.That(actual, Is.EqualTo("{\"Amount\":14.3,\"Currency\":\"XTS\"}"));
	}

	[Test]
	public void CustomContractResolver_AllowsCustomizingCaseAndEnumBehavior()
	{
		var toSerialize = new Money(14.3m, CurrencyIsoCode.XTS);
		var settings = new JsonSerializerSettings
		{
			ContractResolver = new CamelCasePropertyNamesContractResolver(),
			Converters = new JsonConverter[] { new StringEnumConverter(), new MoneyConverter() }
		};
		string actual = JsonConvert.SerializeObject(toSerialize, settings);
		Assert.That(actual, Is.EqualTo("{\"amount\":14.3,\"currency\":\"XTS\"}"));
	}

	[Test]
	public void CustomContractResolver_ForceStringEnum_AllowsCustomizingCaseAndEnumBehavior()
	{
		var toSerialize = new Money(14.3m, CurrencyIsoCode.XTS);
		var settings = new JsonSerializerSettings
		{
			ContractResolver = new CamelCasePropertyNamesContractResolver(),
			// strings as numbers
			Converters = new JsonConverter[] { /*new StringEnumConverter(), */ new MoneyConverter(forceStringEnum: true) }
		};
		string actual = JsonConvert.SerializeObject(toSerialize, settings);
		Assert.That(actual, Is.EqualTo("{\"amount\":14.3,\"currency\":\"XTS\"}"));
	}

	#region nullables

	[Test]
	public void NullDefaultContractResolver_UsesPascalCasedPropertyNamesAndAlphabeticCode()
	{
		Money? @null = default(Money?);

		string actual = JsonConvert.SerializeObject(@null, new MoneyConverter());
		Assert.That(actual, Is.EqualTo("null"));
	}

	#endregion

	#region money containers

	[Test]
	public void Containers_CustomContractResolver_AllowsCustomizingCaseAndEnumBehavior()
	{
		var toSerialize = new Money(14.3m, CurrencyIsoCode.XTS);
		var settings = new JsonSerializerSettings
		{
			ContractResolver = new CamelCasePropertyNamesContractResolver(),
			Converters = new JsonConverter[] { new StringEnumConverter(), new MoneyConverter() }
		};
		string actual = JsonConvert.SerializeObject(new MoneyRecord("P", toSerialize, 1), settings);
		Assert.That(actual, Is.EqualTo("{\"prop\":\"P\",\"money\":{\"amount\":14.3,\"currency\":\"XTS\"}}"));

		actual = JsonConvert.SerializeObject(new MoneyContainer { S = "P", Money = toSerialize}, settings);
		Assert.That(actual, Is.EqualTo("{\"prop\":\"P\",\"money\":{\"amount\":14.3,\"currency\":\"XTS\"}}"));
	}

	[Test]
	public void NullableContainers_CustomContractResolver_AllowsCustomizingCaseAndEnumBehavior()
	{
		var settings = new JsonSerializerSettings
		{
			ContractResolver = new CamelCasePropertyNamesContractResolver(),
			Converters = new JsonConverter[] { new StringEnumConverter(), new MoneyConverter() }
		};
		string actual = JsonConvert.SerializeObject(new NullableMoneyRecord("P", null), settings);
		Assert.That(actual, Is.EqualTo("{\"prop\":\"P\",\"money\":null}"));

		actual = JsonConvert.SerializeObject(new NullableMoneyContainer { Prop = "P", Money = null}, settings);
		Assert.That(actual, Is.EqualTo("{\"prop\":\"P\",\"money\":null}"));
	}

	#endregion

	#endregion

	#region deserialization

	[Test]
	public void DefaultContractResolver_ReadsPascalCasedProperties()
	{
		var expected = new Money(14.3m, CurrencyIsoCode.XTS);
		//var container = new MoneyContainer { Prop = "s", Money = expected };
		string json = "{S:'s',Money: {Amount:14.3,Currency:'XTS'},N:1}";
		var actual = JsonConvert.DeserializeObject<MoneyContainer>(json, new DescriptiveMoneyConverter());

		Assert.That(actual.Money, Is.EqualTo(expected));
	}


	#endregion
}
