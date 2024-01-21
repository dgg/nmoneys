using System.Text.Json;
using System.Text.Json.Serialization;
using NMoneys.Extensions;
using NMoneys.Serialization.Tests.Support;
using NMoneys.Serialization.Text_Json;
using Testing.Commons.Serialization;

namespace NMoneys.Serialization.Tests.Text_Json;

[TestFixture]
public class MoneyConverterTester
{
	[Test, Category("exploratory")]
	public void OutOfTheBoxSerialization_BlowsUp()
	{
		var toSerialize = new Money(14.3m, CurrencyIsoCode.XTS);

		Assert.That(() => JsonSerializer.Serialize(toSerialize), Throws.InstanceOf<JsonException>());
	}

	# region serialization

	[Test]
	public void Serialize_DefaultPolicy_PascalCasedAndNumericCurrency()
	{
		var toSerialize = new Money(14.3m, CurrencyIsoCode.XTS);

		var subject = new MoneyConverter();
		var options = new JsonSerializerOptions
		{
			Converters = { subject }
		};
		string actual = JsonSerializer.Serialize(toSerialize, options);

		Assert.That(actual, Is.EqualTo("{'Amount':14.3,'Currency':963}".Jsonify()));
	}


	[Test]
	public void Serialize_CustomPolicyAndEnumFormat_FollowsCaseAndCurrencyFormat()
	{
		var toSerialize = new Money(14.3m, CurrencyIsoCode.XTS);

		var subject = new MoneyConverter();
		var options = new JsonSerializerOptions
		{
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
			Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase), subject }
		};
		string actual = JsonSerializer.Serialize(toSerialize, options);

		Assert.That(actual, Is.EqualTo("{'amount':14.3,'currency':'xts'}".Jsonify()));
	}

	[Test]
	public void Serialize_Null_DefaultNullable()
	{
		Money? @null = default(Money?);

		var subject = new MoneyConverter();
		var options = new JsonSerializerOptions { Converters = { subject } };
		string actual = JsonSerializer.Serialize(@null, options);
		Assert.That(actual, Is.EqualTo("null"));
	}

	[Test]
	public void Serialize_DefaultOptionsRecord_PascalCasedAndNumericCurrency()
	{
		var money = new Money(14.3m, CurrencyIsoCode.XTS);
		var toSerialize = new MoneyRecord("s", money, 42);

		var subject = new MoneyConverter();
		var options = new JsonSerializerOptions { Converters = { subject } };
		string actual = JsonSerializer.Serialize(toSerialize, options);

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
		var options = new JsonSerializerOptions
		{
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
			Converters = { new JsonStringEnumConverter(), subject }
		};
		string actual = JsonSerializer.Serialize(toSerialize, options);

		Assert.That(actual, Is.EqualTo("{'s':'s','m':{'amount':14.3,'currency':'XTS'},'n':42}".Jsonify()));
	}

	[Test]
	public void Serialize_DefaultOptionsNullableContainer_PascalCasedAndNumericCurrency()
	{
		var toSerialize = new NullableMoneyContainer
		{
			N = 42
		};

		var subject = new MoneyConverter();
		var options = new JsonSerializerOptions { Converters = { subject } };
		string actual = JsonSerializer.Serialize(toSerialize, options);

		Assert.That(actual, Is.EqualTo("{'S':null,'M':null,'N':42}".Jsonify()));
	}

	[Test]
	public void Serialize_CustomNullableRecord_FollowsConfiguration()
	{
		var toSerialize = new NullableMoneyRecord(null, null, 42);
		;

		var subject = new MoneyConverter();
		var options = new JsonSerializerOptions
		{
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
			Converters = { new JsonStringEnumConverter(), subject }
		};
		string actual = JsonSerializer.Serialize(toSerialize, options);

		Assert.That(actual, Is.EqualTo("{'s':null,'m':null,'n':42}".Jsonify()));
	}

	#endregion

	#region deserialization

	[Test]
	public void Deserialize_DefaultOptions_PascalCasedAndNumericCurrency()
	{
		string json = "{'Amount':14.3,'Currency':963}".Jsonify();
		var expected = new Money(14.3m, CurrencyIsoCode.XTS);

		var subject = new MoneyConverter();
		var options = new JsonSerializerOptions { Converters = { subject } };
		Money deserialized = JsonSerializer.Deserialize<Money>(json, options);

		Assert.That(deserialized, Is.EqualTo(expected));
	}

	[Test]
	public void Deserialize_DefaultOptions_CanRoundtrip()
	{
		var original = new Money(14.3m, CurrencyIsoCode.XTS);

		var subject = new MoneyConverter();
		var options = new JsonSerializerOptions { Converters = { subject } };
		string json = JsonSerializer.Serialize(original, options);
		var deserialized = JsonSerializer.Deserialize<Money>(json, options);

		Assert.That(deserialized, Is.EqualTo(original));
	}

	[Test]
	public void Deserialize_CustomPolicyAndEnumFormat_FollowsCaseAndCurrencyFormat()
	{
		string json = "{'amount':14.3,'currency':'xts'}".Jsonify();
		var expected = new Money(14.3m, CurrencyIsoCode.XTS);

		var subject = new MoneyConverter();
		var options = new JsonSerializerOptions
		{
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
			Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase), subject }
		};
		var actual = JsonSerializer.Deserialize<Money>(json, options);

		Assert.That(actual, Is.EqualTo(expected));
	}

	[Test]
	public void Deserialize_CustomPolicyAndEnumFormat_CanRoundtrip()
	{
		var original = new Money(14.3m, CurrencyIsoCode.XTS);

		var subject = new MoneyConverter();
		var options = new JsonSerializerOptions
		{
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
			Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase), subject }
		};
		string json = JsonSerializer.Serialize(original, options);
		var deserialized = JsonSerializer.Deserialize<Money>(json, options);

		Assert.That(deserialized, Is.EqualTo(original));
	}


	[Test]
	public void Deserialize_Null_DefaultNullable()
	{
		var subject = new MoneyConverter();

		var options = new JsonSerializerOptions { Converters = { subject } };
		var actual = JsonSerializer.Deserialize<Money?>("null", options);
		Assert.That(actual, Is.Null);
	}

	[Test]
	public void Deserialize_DefaultOptionsRecord_PascalCasedAndNumericCurrency()
	{
		string json = "{'S':'s','M':{'Amount':14.3,'Currency':963},'N':42}".Jsonify();
		var money = new Money(14.3m, CurrencyIsoCode.XTS);
		var expected = new MoneyRecord("s", money, 42);

		var subject = new MoneyConverter();
		var options = new JsonSerializerOptions { Converters = { subject } };
		MoneyRecord? actual = JsonSerializer.Deserialize<MoneyRecord>(json, options);

		Assert.That(actual, Is.EqualTo(expected));
	}

	[Test]
	public void Deserialize_CustomContainer_FollowsConfiguration()
	{
		string json = "{'s':'s','m':{'amount':14.3,'currency':'XTS'},'n':42}".Jsonify();
		var money = new Money(14.3m, CurrencyIsoCode.XTS);

		var subject = new MoneyConverter();
		var settings = new JsonSerializerOptions
		{
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
			Converters = { new JsonStringEnumConverter(), subject }
		};
		var actual = JsonSerializer.Deserialize<MoneyContainer>(json, settings);

		Assert.That(actual, Has.Property("S").EqualTo("s"));
		Assert.That(actual, Has.Property("M").EqualTo(money));
		Assert.That(actual, Has.Property("N").EqualTo(42));
	}

	[Test]
	public void Deserialize_DefaultOptionsNullableContainer_PascalCasedAndNumericCurrency()
	{
		string json = "{'S':null,'M':null,'N':42}".Jsonify();

		var subject = new MoneyConverter();
		var options = new JsonSerializerOptions { Converters = { subject } };
		var actual = JsonSerializer.Deserialize<NullableMoneyContainer>(json, options);

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
		var settings = new JsonSerializerOptions
		{
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
			Converters ={ new JsonStringEnumConverter(), subject }
		};
		var actual = JsonSerializer.Deserialize<NullableMoneyRecord>(json, settings);

		Assert.That(actual, Is.EqualTo(expected));
	}

	#region error handling

	[Test]
	public void Deserialize_DefaultNotAnObject_Exception()
	{
		string notAJsonObject = "'str'".Jsonify();
		var subject = new MoneyConverter();

		var options = new JsonSerializerOptions { Converters = { subject } };
		Assert.That(()=> JsonSerializer.Deserialize<Money>(notAJsonObject, options),
			Throws.InstanceOf<InvalidOperationException>()
				.With.Message.Contains("must be of type")
				.And.Message.Contains("'JsonObject'"));
	}

	[Test]
	public void Deserialize_DefaultMissingAmount_Exception()
	{
		string missingAmount = "{}".Jsonify();
		var subject = new MoneyConverter();
		var options = new JsonSerializerOptions { Converters = { subject } };

		Assert.That(()=> JsonSerializer.Deserialize<Money>(missingAmount, options),
			Throws.InstanceOf<JsonException>()
				.With.Message.Contains("Missing property")
				.And.Message.Contains("'Amount'"));
	}

	[Test]
	public void Deserialize_DefaultNonNumericAmount_Exception()
	{
		string nonNumericAmount = "{'Amount':'lol'}".Jsonify();
		var subject = new MoneyConverter();
		var options = new JsonSerializerOptions { Converters = { subject } };

		Assert.That(()=> JsonSerializer.Deserialize<Money>(nonNumericAmount, options),
			Throws.InstanceOf<InvalidOperationException>()
				.With.Message.Contains("cannot be converted")
				.And.Message.Contains("'String'")
				.And.Message.Contains("'System.Decimal'"));
	}

	[Test]
	public void Deserialize_CaseDoesNotMatchContract_DoesntMatter()
	{
		string caseMismatch = "{'Amount':1,'currency':963}".Jsonify();
		var subject = new MoneyConverter();
		var options = new JsonSerializerOptions { Converters = { subject } };

		Assert.That(()=> JsonSerializer.Deserialize<Money>(caseMismatch, options),
			Throws.Nothing);
		var deserialized = JsonSerializer.Deserialize<Money>(caseMismatch, options);
		Assert.That(deserialized, Is.EqualTo(1m.Xts()));
	}

	[Test]
	public void Deserialize_MissingCurrency_ExceptionIgnoringCasing()
	{
		string missingCurrency = "{'amount':1}".Jsonify();
		var subject = new MoneyConverter();
		var options = new JsonSerializerOptions
		{
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
			Converters = { subject }
		};
		Assert.That(()=> JsonSerializer.Deserialize<Money>(missingCurrency, options),
			Throws.InstanceOf<JsonException>()
				.With.Message.Contains("Missing property")
				.And.Message.Contains("'Currency'"), "no camel-casing");
	}

	[Test]
	public void Deserialize_MismatchCurrencyFormat_Exception()
	{
		string currencyFormatMismatch = "{'amount':1,'currency':'XTS'}".Jsonify();
		var subject = new MoneyConverter();

		var options = new JsonSerializerOptions
		{
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
			Converters = { subject }
		};
		Assert.That(()=> JsonSerializer.Deserialize<Money>(currencyFormatMismatch, options),
			Throws.InstanceOf<JsonException>()
				.With.Message.Contains("could not be converted to")
				.And.Message.Contains("NMoneys.CurrencyIsoCode"));
	}

	[Test]
	public void Deserialize_FunkyCurrencyValueCasing_Exception()
	{
		string funkyCurrencyValue = "{'amount':1,'currency':'XtS'}".Jsonify();
		var subject = new MoneyConverter();

		var options = new JsonSerializerOptions
		{
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
			Converters = { subject }
		};
		Assert.That(()=> JsonSerializer.Deserialize<Money>(funkyCurrencyValue, options),
			Throws.InstanceOf<JsonException>()
				.With.Message.Contains("could not be converted to")
				.And.Message.Contains("NMoneys.CurrencyIsoCode"));
	}

	[Test]
	public void Deserialize_UndefinedCurrency_Exception()
	{
		string undefinedCurrency = "{'amount':1,'currency':1}".Jsonify();
		var subject = new MoneyConverter();
		var options = new JsonSerializerOptions
		{
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
			Converters = { subject }
		};
		Assert.That(()=> JsonSerializer.Deserialize<Money>(undefinedCurrency, options),
			Throws.InstanceOf<UndefinedCodeException>());
	}

	[Test]
	public void Deserialize_PoorlyConstructedJson_Exception()
	{
		string missingObjectClose = "{'amount':1,'currency':1".Jsonify();
		var subject = new MoneyConverter();
		var options = new JsonSerializerOptions { Converters = { subject } };

		Assert.That(()=> JsonSerializer.Deserialize<Money>(missingObjectClose, options),
			Throws.InstanceOf<JsonException>());
	}

	#endregion

	#endregion
}
