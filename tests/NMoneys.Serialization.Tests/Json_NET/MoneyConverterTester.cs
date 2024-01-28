using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using NMoneys.Extensions;
using Testing.Commons.Serialization;

using NMoneys.Serialization.Tests.Support;
using NMoneys.Serialization.Json_NET;

namespace NMoneys.Serialization.Tests.Json_NET;

[TestFixture]
public class MoneyConverterTester
{
	[Test, Category("exploratory")]
	public void OutOfTheBoxSerialization_BlowsUp()
	{
		var toSerialize = new Money(14.3m, CurrencyIsoCode.XTS);

		Assert.That(()=> JsonConvert.SerializeObject(toSerialize), Throws.InstanceOf<JsonSerializationException>());
	}

	# region serialization

	[Test]
	public void Serialize_DefaultResolver_PascalCasedAndNumericCurrency()
	{
		var toSerialize = new Money(14.3m, CurrencyIsoCode.XTS);

		var subject = new MoneyConverter();
		string actual = JsonConvert.SerializeObject(toSerialize, subject);

		Assert.That(actual, Is.EqualTo("{'Amount':14.3,'Currency':963}".Jsonify()));
	}

	[Test]
	public void Serialize_DefaultResolverWithForcedStringCurrency_PascalCasedAndStringCurrency()
	{
		var toSerialize = new Money(14.3m, CurrencyIsoCode.XTS);

		var subject = new MoneyConverter(true);
		string actual = JsonConvert.SerializeObject(toSerialize, subject);

		Assert.That(actual, Is.EqualTo("{'Amount':14.3,'Currency':'XTS'}".Jsonify()));
	}

	[Test]
	public void Serialize_CustomResolverAndEnumFormat_FollowsCaseAndCurrencyFormat()
	{
		var toSerialize = new Money(14.3m, CurrencyIsoCode.XTS);

		var subject = new MoneyConverter();
		var settings = new JsonSerializerSettings
		{
			ContractResolver = new CamelCasePropertyNamesContractResolver(),
			Converters = new JsonConverter[] { new StringEnumConverter(), subject }
		};
		string actual = JsonConvert.SerializeObject(toSerialize, settings);

		Assert.That(actual, Is.EqualTo("{'amount':14.3,'currency':'XTS'}".Jsonify()));
	}

	[Test]
	public void Serialize_CustomResolverAndForcedString_FollowsCaseAndOverridesCurrencyFormat()
	{
		var toSerialize = new Money(14.3m, CurrencyIsoCode.XTS);

		var subject = new MoneyConverter(true);
		var settings = new JsonSerializerSettings
		{
			ContractResolver = new CamelCasePropertyNamesContractResolver(),
			Converters = new JsonConverter[] { subject }
		};
		string actual = JsonConvert.SerializeObject(toSerialize, settings);

		Assert.That(actual, Is.EqualTo("{'amount':14.3,'currency':'XTS'}".Jsonify()));
	}

	[Test]
	public void Serialize_Null_DefaultNullable()
	{
		Money? @null = default(Money?);

		var subject = new MoneyConverter();

		string actual = JsonConvert.SerializeObject(@null, subject);
		Assert.That(actual, Is.EqualTo("null"));
	}

	[Test]
	public void Serialize_DefaultResolverRecord_PascalCasedAndNumericCurrency()
	{
		var money = new Money(14.3m, CurrencyIsoCode.XTS);
		var toSerialize = new MoneyRecord("s", money, 42);

		var subject = new MoneyConverter();
		string actual = JsonConvert.SerializeObject(toSerialize, subject);

		Assert.That(actual, Is.EqualTo("{'S':'s','M':{'Amount':14.3,'Currency':963},'N':42}".Jsonify()));
	}

	[Test]
	public void Serialize_CustomContainer_FollowsConfiguration()
	{
		var money = new Money(14.3m, CurrencyIsoCode.XTS);
		var toSerialize = new MoneyContainer
		{
			S = "s",
			M = money,
			N = 42
		};

		var subject = new MoneyConverter();
		var settings = new JsonSerializerSettings
		{
			ContractResolver = new CamelCasePropertyNamesContractResolver(),
			Converters = new JsonConverter[] { new StringEnumConverter(), subject }
		};
		string actual = JsonConvert.SerializeObject(toSerialize, settings);

		Assert.That(actual, Is.EqualTo("{'s':'s','m':{'amount':14.3,'currency':'XTS'},'n':42}".Jsonify()));
	}

	[Test]
	public void Serialize_DefaultResolverNullableContainer_PascalCasedAndNumericCurrency()
	{
		var toSerialize = new NullableMoneyContainer
		{
			N = 42
		};

		var subject = new MoneyConverter();
		string actual = JsonConvert.SerializeObject(toSerialize, subject);

		Assert.That(actual, Is.EqualTo("{'S':null,'M':null,'N':42}".Jsonify()));
	}

	[Test]
	public void Serialize_CustomNullableRecord_FollowsConfiguration()
	{
		var toSerialize = new NullableMoneyRecord(null, null, 42);

		var subject = new MoneyConverter();
		var settings = new JsonSerializerSettings
		{
			ContractResolver = new CamelCasePropertyNamesContractResolver(),
			Converters = new JsonConverter[] { new StringEnumConverter(), subject }
		};
		string actual = JsonConvert.SerializeObject(toSerialize, settings);

		Assert.That(actual, Is.EqualTo("{'s':null,'m':null,'n':42}".Jsonify()));
	}

	#endregion

	#region deserialization

	[Test]
	public void Deserialize_DefaultResolver_PascalCasedAndNumericCurrency()
	{
		string json = "{'Amount':14.3,'Currency':963}".Jsonify();
		var expected = new Money(14.3m, CurrencyIsoCode.XTS);

		var subject = new MoneyConverter();
		Money deserialized = JsonConvert.DeserializeObject<Money>(json, subject);

		Assert.That(deserialized, Is.EqualTo(expected));
	}

	[Test]
	public void Deserialize_DefaultResolver_CanRoundtrip()
	{
		var original = new Money(14.3m, CurrencyIsoCode.XTS);

		var subject = new MoneyConverter();
		string json = JsonConvert.SerializeObject(original, subject);
		var deserialized = JsonConvert.DeserializeObject<Money>(json, subject);

		Assert.That(deserialized, Is.EqualTo(original));
	}

	[Test]
	public void Deserialize_DefaultResolverWithForcedStringCurrency_PascalCasedAndStringCurrency()
	{
		string json = "{'Amount':14.3,'Currency':'XTS'}".Jsonify();
		var expected = new Money(14.3m, CurrencyIsoCode.XTS);

		var subject = new MoneyConverter(true);
		Money deserialized = JsonConvert.DeserializeObject<Money>(json, subject);

		Assert.That(deserialized, Is.EqualTo(expected));
	}

	[Test]
	public void Deserialize_DefaultResolverWithForcedStringCurrency_CanRoundtrip()
	{
		var original = new Money(14.3m, CurrencyIsoCode.XTS);

		var subject = new MoneyConverter(true);
		string json = JsonConvert.SerializeObject(original, subject);
		var deserialized = JsonConvert.DeserializeObject<Money>(json, subject);

		Assert.That(deserialized, Is.EqualTo(original));
	}

	[Test]
	public void Deserialize_CustomResolverAndEnumFormat_FollowsCaseAndCurrencyFormat()
	{
		string json = "{'amount':14.3,'currency':'XTS'}".Jsonify();
		var expected = new Money(14.3m, CurrencyIsoCode.XTS);

		var subject = new MoneyConverter();
		var settings = new JsonSerializerSettings
		{
			ContractResolver = new CamelCasePropertyNamesContractResolver(),
			Converters = new JsonConverter[] { new StringEnumConverter(), subject }
		};
		var actual = JsonConvert.DeserializeObject<Money>(json, settings);

		Assert.That(actual, Is.EqualTo(expected));
	}

	[Test]
	public void Deserialize_CustomResolverAndEnumFormat_CanRoundtrip()
	{
		var original = new Money(14.3m, CurrencyIsoCode.XTS);

		var subject = new MoneyConverter();
		var settings = new JsonSerializerSettings
		{
			ContractResolver = new CamelCasePropertyNamesContractResolver(),
			Converters = new JsonConverter[] { new StringEnumConverter(), subject }
		};
		string json = JsonConvert.SerializeObject(original, settings);
		var deserialized = JsonConvert.DeserializeObject<Money>(json, settings);

		Assert.That(deserialized, Is.EqualTo(original));
	}

	[Test]
	public void Deserialize_CustomResolverAndForcedString_FollowsCaseAndOverridesCurrencyFormat()
	{
		string json = "{'amount':14.3,'currency':'XTS'}".Jsonify();
		var expected = new Money(14.3m, CurrencyIsoCode.XTS);

		var subject = new MoneyConverter(true);
		var settings = new JsonSerializerSettings
		{
			ContractResolver = new CamelCasePropertyNamesContractResolver(),
			Converters = new JsonConverter[] { subject }
		};
		var actual = JsonConvert.DeserializeObject<Money>(json,  settings);

		Assert.That(actual, Is.EqualTo(expected));
	}

	[Test]
	public void Deserialize_CustomResolverAndForcedString_CanRoundtrip()
	{
		var original = new Money(14.3m, CurrencyIsoCode.XTS);

		var subject = new MoneyConverter(true);
		var settings = new JsonSerializerSettings
		{
			ContractResolver = new CamelCasePropertyNamesContractResolver(),
			Converters = new JsonConverter[] { subject }
		};
		var json = JsonConvert.SerializeObject(original,  settings);
		var deserialized = JsonConvert.DeserializeObject<Money>(json, settings);

		Assert.That(deserialized, Is.EqualTo(original));
	}

	[Test]
	public void Deserialize_Null_DefaultNullable()
	{
		var subject = new MoneyConverter();

		var actual = JsonConvert.DeserializeObject<Money?>("null", subject);
		Assert.That(actual, Is.Null);
	}

	[Test]
	public void Deserialize_DefaultResolverRecord_PascalCasedAndNumericCurrency()
	{
		string json = "{'S':'s','M':{'Amount':14.3,'Currency':963},'N':42}".Jsonify();
		var money = new Money(14.3m, CurrencyIsoCode.XTS);
		var expected = new MoneyRecord("s", money, 42);

		var subject = new MoneyConverter();
		MoneyRecord? actual = JsonConvert.DeserializeObject<MoneyRecord>(json, subject);

		Assert.That(actual, Is.EqualTo(expected));
	}

	[Test]
	public void Deserialize_CustomContainer_FollowsConfiguration()
	{
		string json = "{'s':'s','m':{'amount':14.3,'currency':'XTS'},'n':42}".Jsonify();
		var money = new Money(14.3m, CurrencyIsoCode.XTS);

		var subject = new MoneyConverter();
		var settings = new JsonSerializerSettings
		{
			ContractResolver = new CamelCasePropertyNamesContractResolver(),
			Converters = new JsonConverter[] { new StringEnumConverter(), subject }
		};
		var actual = JsonConvert.DeserializeObject<MoneyContainer>(json, settings);

		Assert.That(actual, Has.Property("S").EqualTo("s"));
		Assert.That(actual, Has.Property("M").EqualTo(money));
		Assert.That(actual, Has.Property("N").EqualTo(42));

	}

	[Test]
	public void Deserialize_DefaultResolverNullableContainer_PascalCasedAndNumericCurrency()
	{
		string json = "{'S':null,'M':null,'N':42}".Jsonify();

		var subject = new MoneyConverter();
		var actual = JsonConvert.DeserializeObject<NullableMoneyContainer>(json, subject);

		Assert.That(actual, Has.Property("S").Null);
		Assert.That(actual, Has.Property("M").Null);
		Assert.That(actual, Has.Property("N").EqualTo(42));
	}

	[Test]
	public void Deserialize_CustomNullableRecord_FollowsConfiguration()
	{
		string json = "{'s':null,'m':null,'n':42}".Jsonify();
		var expected = new NullableMoneyRecord(null, null, 42);

		var subject = new MoneyConverter();
		var settings = new JsonSerializerSettings
		{
			ContractResolver = new CamelCasePropertyNamesContractResolver(),
			Converters = new JsonConverter[] { new StringEnumConverter(), subject }
		};
		var actual = JsonConvert.DeserializeObject<NullableMoneyRecord>(json, settings);

		Assert.That(actual, Is.EqualTo(expected));
	}

	#region error handling

	[Test]
	public void Deserialize_DefaultNotAnObject_Exception()
	{
		string notAJsonObject = "'str'".Jsonify();
		var subject = new MoneyConverter();

		Assert.That(()=> JsonConvert.DeserializeObject<Money>(notAJsonObject, subject),
			Throws.InstanceOf<JsonSerializationException>()
				.With.Message.Contains("token 'Object'")
				.And.Message.Contains("'String'"));
	}

	[Test]
	public void Deserialize_DefaultMissingAmount_Exception()
	{
		string missingAmount = "{}".Jsonify();
		var subject = new MoneyConverter();

		Assert.That(()=> JsonConvert.DeserializeObject<Money>(missingAmount, subject),
			Throws.InstanceOf<JsonSerializationException>()
				.With.Message.Contains("Missing property")
				.And.Message.Contains("'Amount'"));
	}

	[Test]
	public void Deserialize_DefaultNonNumericAmount_Exception()
	{
		string nonNumericAmount = "{'Amount':'lol'}".Jsonify();
		var subject = new MoneyConverter();

		Assert.That(()=> JsonConvert.DeserializeObject<Money>(nonNumericAmount, subject),
			Throws.InstanceOf<FormatException>()
				.With.Message.Contains("input string 'lol'"));
	}

	[Test]
	public void Deserialize_CaseDoesNotMatchContract_DoesntMatter()
	{
		string caseMismatch = "{'Amount':1,'currency':963}".Jsonify();
		var subject = new MoneyConverter();

		Assert.That(()=> JsonConvert.DeserializeObject<Money>(caseMismatch, subject),
			Throws.Nothing);
		var deserialized = JsonConvert.DeserializeObject<Money>(caseMismatch, subject);
		Assert.That(deserialized, Is.EqualTo(1m.Xts()));
	}

	[Test]
	public void Deserialize_MissingCurrency_ExceptionIgnoringCasing()
	{
		string missingCurrency = "{'amount':1}".Jsonify();
		var subject = new MoneyConverter();

		var settings = new JsonSerializerSettings
		{
			ContractResolver = new CamelCasePropertyNamesContractResolver(),
			Converters = { subject }
		};
		Assert.That(()=> JsonConvert.DeserializeObject<Money>(missingCurrency, settings),
			Throws.InstanceOf<JsonSerializationException>()
				.With.Message.Contains("Missing property")
				.And.Message.Contains("'Currency'"), "no camel-casing");
	}

	[Test]
	public void Deserialize_MismatchCurrencyFormat_DoesNotMatter()
	{
		string currencyFormatMismatch = "{'amount':1,'currency':'XTS'}".Jsonify();
		var subject = new MoneyConverter();

		var settings = new JsonSerializerSettings
		{
			ContractResolver = new CamelCasePropertyNamesContractResolver(),
			Converters = { subject }
		};
		Assert.That(()=> JsonConvert.DeserializeObject<Money>(currencyFormatMismatch, settings),
			Throws.Nothing);

		Money deserialized = JsonConvert.DeserializeObject<Money>(currencyFormatMismatch, settings);
		Assert.That(deserialized, Is.EqualTo(1m.Xts()));
	}

	[Test]
	public void Deserialize_FunkyCurrencyValueCasing_DoesNotMatter()
	{
		string funkyCurrencyValue = "{'amount':1,'currency':'XtS'}".Jsonify();
		var subject = new MoneyConverter();

		var settings = new JsonSerializerSettings
		{
			ContractResolver = new CamelCasePropertyNamesContractResolver(),
			Converters = { subject }
		};
		Assert.That(()=> JsonConvert.DeserializeObject<Money>(funkyCurrencyValue, settings),
			Throws.Nothing);

		Money deserialized = JsonConvert.DeserializeObject<Money>(funkyCurrencyValue, settings);
		Assert.That(deserialized, Is.EqualTo(1m.Xts()));
	}

	[Test]
	public void Deserialize_UndefinedCurrency_Exception()
	{
		string undefinedCurrency = "{'amount':1,'currency':1}".Jsonify();
		var subject = new MoneyConverter();

		var settings = new JsonSerializerSettings
		{
			ContractResolver = new CamelCasePropertyNamesContractResolver(),
			Converters = { subject }
		};
		Assert.That(()=> JsonConvert.DeserializeObject<Money>(undefinedCurrency, settings),
			Throws.InstanceOf<UndefinedCodeException>());
	}

	[Test]
	public void Deserialize_PoorlyConstructedJson_Exception()
	{
		string missingObjectClose = "{'amount':1,'currency':1".Jsonify();
		var subject = new MoneyConverter();

		Assert.That(()=> JsonConvert.DeserializeObject<Money>(missingObjectClose, subject),
			Throws.InstanceOf<JsonReaderException>());
	}

	#endregion

	#endregion
}
