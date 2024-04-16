using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using NMoneys.Extensions;
using Testing.Commons.Serialization;

using NMoneys.Serialization.Tests.Support;
using NMoneys.Serialization.Json_NET;

namespace NMoneys.Serialization.Tests.Json_NET;

[TestFixture]
public class QuantityConverterTester
{
	# region serialization

	[Test]
	public void Serialize_DefaultResolver_QuantityString()
	{
		var toSerialize = new Money(14.3m, CurrencyIsoCode.XTS);

		var subject = new QuantityConverter();
		string actual = JsonConvert.SerializeObject(toSerialize, subject);

		Assert.That(actual, Is.EqualTo("'XTS 14.3'".Jsonify()));
	}

	[Test]
	public void Serialize_CustomResolverAndEnumFormat_ConfigurationDoesNotApply()
	{
		var toSerialize = new Money(14.3m, CurrencyIsoCode.XTS);

		var subject = new QuantityConverter();
		var settings = new JsonSerializerSettings
		{
			ContractResolver = new CamelCasePropertyNamesContractResolver(),
			Converters = [new StringEnumConverter(), subject]
		};
		string actual = JsonConvert.SerializeObject(toSerialize, settings);

		Assert.That(actual, Is.EqualTo("'XTS 14.3'".Jsonify()));
	}

	[Test]
	public void Serialize_Null_DefaultNullable()
	{
		Money? @null = default;

		var subject = new QuantityConverter();

		string actual = JsonConvert.SerializeObject(@null, subject);
		Assert.That(actual, Is.EqualTo("null"));
	}

	[Test]
	public void Serialize_DefaultResolverRecord_QuantityString()
	{
		var money = new Money(14.3m, CurrencyIsoCode.XTS);
		var toSerialize = new MoneyRecord("s", money, 42);

		var subject = new QuantityConverter();
		string actual = JsonConvert.SerializeObject(toSerialize, subject);

		Assert.That(actual, Is.EqualTo("{'S':'s','M':'XTS 14.3','N':42}".Jsonify()));
	}

	[Test]
	public void Serialize_CustomContainer_ConfigurationDoesNotApply()
	{
		var money = new Money(14.3m, CurrencyIsoCode.XTS);
		var toSerialize = new MoneyContainer
		{
			S = "s",
			M = money,
			N = 42
		};

		var subject = new QuantityConverter();
		var settings = new JsonSerializerSettings
		{
			ContractResolver = new CamelCasePropertyNamesContractResolver(),
			Converters = [new StringEnumConverter(), subject]
		};
		string actual = JsonConvert.SerializeObject(toSerialize, settings);

		Assert.That(actual, Is.EqualTo("{'s':'s','m':'XTS 14.3','n':42}".Jsonify()));
	}

	[Test]
	public void Serialize_DefaultResolverNullableContainer_PascalCasedAndNumericCurrency()
	{
		var toSerialize = new NullableMoneyContainer
		{
			N = 42
		};

		var subject = new QuantityConverter();
		string actual = JsonConvert.SerializeObject(toSerialize, subject);

		Assert.That(actual, Is.EqualTo("{'S':null,'M':null,'N':42}".Jsonify()));
	}

	[Test]
	public void Serialize_CustomNullableRecord_FollowsConfiguration()
	{
		var toSerialize = new NullableMoneyRecord(null, null, 42);

		var subject = new QuantityConverter();
		var settings = new JsonSerializerSettings
		{
			ContractResolver = new CamelCasePropertyNamesContractResolver(),
			Converters = [new StringEnumConverter(), subject]
		};
		string actual = JsonConvert.SerializeObject(toSerialize, settings);

		Assert.That(actual, Is.EqualTo("{'s':null,'m':null,'n':42}".Jsonify()));
	}

	#endregion

	#region deserialization

	[Test]
	public void Deserialize_DefaultResolver_QuantityString()
	{
		string json = "'XTS 14.3'".Jsonify();
		var expected = new Money(14.3m, CurrencyIsoCode.XTS);

		var subject = new QuantityConverter();
		Money deserialized = JsonConvert.DeserializeObject<Money>(json, subject);

		Assert.That(deserialized, Is.EqualTo(expected));
	}

	[Test]
	public void Deserialize_DefaultResolver_CanRoundtrip()
	{
		var original = new Money(14.3m, CurrencyIsoCode.XTS);

		var subject = new QuantityConverter();
		string json = JsonConvert.SerializeObject(original, subject);
		var deserialized = JsonConvert.DeserializeObject<Money>(json, subject);

		Assert.That(deserialized, Is.EqualTo(original));
	}

	[Test]
	public void Deserialize_CustomResolverAndEnumFormat_IgnoresConfig()
	{
		string json = "'963 14.3'".Jsonify();
		var expected = new Money(14.3m, CurrencyIsoCode.XTS);

		var subject = new QuantityConverter();
		var settings = new JsonSerializerSettings
		{
			ContractResolver = new CamelCasePropertyNamesContractResolver(),
			Converters = [new StringEnumConverter(), subject]
		};
		var actual = JsonConvert.DeserializeObject<Money>(json, settings);

		Assert.That(actual, Is.EqualTo(expected));
	}

	[Test]
	public void Deserialize_CustomResolverAndEnumFormat_CanRoundtrip()
	{
		var original = new Money(14.3m, CurrencyIsoCode.XTS);

		var subject = new QuantityConverter();
		var settings = new JsonSerializerSettings
		{
			ContractResolver = new CamelCasePropertyNamesContractResolver(),
			Converters = [new StringEnumConverter(), subject]
		};
		string json = JsonConvert.SerializeObject(original, settings);
		var deserialized = JsonConvert.DeserializeObject<Money>(json, settings);

		Assert.That(deserialized, Is.EqualTo(original));
	}

	[Test]
	public void Deserialize_Null_DefaultNullable()
	{
		var subject = new QuantityConverter();

		var actual = JsonConvert.DeserializeObject<Money?>("null", subject);
		Assert.That(actual, Is.Null);
	}

	[Test]
	public void Deserialize_DefaultResolverRecord_PascalCasedAndNumericCurrency()
	{
		string json = "{'S':'s','M':'XTS 14.3','N':42}".Jsonify();
		var money = new Money(14.3m, CurrencyIsoCode.XTS);
		var expected = new MoneyRecord("s", money, 42);

		var subject = new QuantityConverter();
		MoneyRecord? actual = JsonConvert.DeserializeObject<MoneyRecord>(json, subject);

		Assert.That(actual, Is.EqualTo(expected));
	}

	[Test]
	public void Deserialize_CustomContainer_FollowsConfiguration()
	{
		string json = "{'s':'s','m':'XTS 14.3','n':42}".Jsonify();
		var money = new Money(14.3m, CurrencyIsoCode.XTS);

		var subject = new QuantityConverter();
		var settings = new JsonSerializerSettings
		{
			ContractResolver = new CamelCasePropertyNamesContractResolver(),
			Converters = [new StringEnumConverter(), subject]
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

		var subject = new QuantityConverter();
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

		var subject = new QuantityConverter();
		var settings = new JsonSerializerSettings
		{
			ContractResolver = new CamelCasePropertyNamesContractResolver(),
			Converters = [new StringEnumConverter(), subject]
		};
		var actual = JsonConvert.DeserializeObject<NullableMoneyRecord>(json, settings);

		Assert.That(actual, Is.EqualTo(expected));
	}

	#region error handling

	[Test]
	public void Deserialize_DefaultNotAnObject_Exception()
	{
		string notAJsonObject = "'str'".Jsonify();
		var subject = new QuantityConverter();

		Assert.That(()=> JsonConvert.DeserializeObject<Money>(notAJsonObject, subject),
			Throws.ArgumentException);
	}

	[Test]
	public void Deserialize_DefaultMissingAmount_Exception()
	{
		string missingAmount = "'XTS '".Jsonify();
		var subject = new QuantityConverter();

		Assert.That(()=> JsonConvert.DeserializeObject<Money>(missingAmount, subject),
			Throws.InstanceOf<FormatException>());
	}

	[Test]
	public void Deserialize_DefaultNonNumericAmount_Exception()
	{
		string nonNumericAmount = "'XTS 'lol''".Jsonify();
		var subject = new QuantityConverter();

		Assert.That(()=> JsonConvert.DeserializeObject<Money>(nonNumericAmount, subject),
			Throws.InstanceOf<FormatException>());
	}

	[Test]
	public void Deserialize_CaseDoesNotMatchContract_DoesntMatter()
	{
		string caseMismatch = "'963 1'".Jsonify();
		var subject = new QuantityConverter();

		Assert.That(()=> JsonConvert.DeserializeObject<Money>(caseMismatch, subject),
			Throws.Nothing);
		var deserialized = JsonConvert.DeserializeObject<Money>(caseMismatch, subject);
		Assert.That(deserialized, Is.EqualTo(1m.Xts()));
	}

	[Test]
	public void Deserialize_MissingCurrency_Exception()
	{
		string missingCurrency = "'1'".Jsonify();
		var subject = new QuantityConverter();

		var settings = new JsonSerializerSettings
		{
			ContractResolver = new CamelCasePropertyNamesContractResolver(),
			Converters = { subject }
		};
		Assert.That(()=> JsonConvert.DeserializeObject<Money>(missingCurrency, settings),
			Throws.InstanceOf<ArgumentOutOfRangeException>());
	}

	[Test]
	public void Deserialize_MismatchCurrencyFormat_MattersMuch()
	{
		string currencyFormatMismatch = "'xts 1'".Jsonify();
		var subject = new QuantityConverter();

		var settings = new JsonSerializerSettings
		{
			ContractResolver = new CamelCasePropertyNamesContractResolver(),
			Converters = { subject }
		};
		Assert.That(()=> JsonConvert.DeserializeObject<Money>(currencyFormatMismatch, settings),
			Throws.ArgumentException);
	}

	[Test]
	public void Deserialize_FunkyCurrencyValueCasing_DoesNotMatter()
	{
		string funkyCurrencyValue = "'XtS 1'".Jsonify();
		var subject = new QuantityConverter();

		var settings = new JsonSerializerSettings
		{
			ContractResolver = new CamelCasePropertyNamesContractResolver(),
			Converters = { subject }
		};
		Assert.That(()=> JsonConvert.DeserializeObject<Money>(funkyCurrencyValue, settings),
			Throws.ArgumentException);
	}

	[Test]
	public void Deserialize_UndefinedCurrency_Exception()
	{
		string undefinedCurrency = "'AAA 1'".Jsonify();
		var subject = new QuantityConverter();

		var settings = new JsonSerializerSettings
		{
			ContractResolver = new CamelCasePropertyNamesContractResolver(),
			Converters = { subject }
		};
		Assert.That(()=> JsonConvert.DeserializeObject<Money>(undefinedCurrency, settings),
			Throws.ArgumentException);
	}

	[Test]
	public void Deserialize_PoorlyConstructedJson_Exception()
	{
		string missingObjectClose = "'XTS 1".Jsonify();
		var subject = new QuantityConverter();

		Assert.That(()=> JsonConvert.DeserializeObject<Money>(missingObjectClose, subject),
			Throws.InstanceOf<JsonReaderException>());
	}

	#endregion

	#endregion
}
