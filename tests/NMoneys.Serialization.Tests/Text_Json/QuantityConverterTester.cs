using System.Text.Json;
using System.Text.Json.Serialization;
using NMoneys.Extensions;
using NMoneys.Serialization.Tests.Support;
using NMoneys.Serialization.Text_Json;
using Testing.Commons.Serialization;

namespace NMoneys.Serialization.Tests.Text_Json;

[TestFixture]
public class QuantityConverterTester
{
	# region serialization

	[Test]
	public void Serialize_DefaultPolicy_QuantityString()
	{
		var toSerialize = new Money(14.3m, CurrencyIsoCode.XTS);

		var subject = new QuantityConverter();
		var options = new JsonSerializerOptions
		{
			Converters = { subject }
		};
		string actual = JsonSerializer.Serialize(toSerialize, options);

		Assert.That(actual, Is.EqualTo("'XTS 14.3'".Jsonify()));
	}


	[Test]
	public void Serialize_CustomPolicyAndEnumFormat_QuantityString()
	{
		var toSerialize = new Money(14.3m, CurrencyIsoCode.XTS);

		var subject = new QuantityConverter();
		var options = new JsonSerializerOptions
		{
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
			Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase), subject }
		};
		string actual = JsonSerializer.Serialize(toSerialize, options);

		Assert.That(actual, Is.EqualTo("'XTS 14.3'".Jsonify()));
	}

	[Test]
	public void Serialize_Null_DefaultNullable()
	{
		Money? @null = default(Money?);

		var subject = new QuantityConverter();
		var options = new JsonSerializerOptions { Converters = { subject } };
		string actual = JsonSerializer.Serialize(@null, options);
		Assert.That(actual, Is.EqualTo("null"));
	}

	[Test]
	public void Serialize_DefaultOptionsRecord_QuantityString()
	{
		var money = new Money(14.3m, CurrencyIsoCode.XTS);
		var toSerialize = new MoneyRecord("s", money, 42);

		var subject = new QuantityConverter();
		var options = new JsonSerializerOptions { Converters = { subject } };
		string actual = JsonSerializer.Serialize(toSerialize, options);

		Assert.That(actual, Is.EqualTo("{'S':'s','M':'XTS 14.3','N':42}".Jsonify()));
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

		var subject = new QuantityConverter();
		var options = new JsonSerializerOptions
		{
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
			Converters = { new JsonStringEnumConverter(), subject }
		};
		string actual = JsonSerializer.Serialize(toSerialize, options);

		Assert.That(actual, Is.EqualTo("{'s':'s','m':'XTS 14.3','n':42}".Jsonify()));
	}

	[Test]
	public void Serialize_DefaultOptionsNullableContainer_PascalCasedAndNumericCurrency()
	{
		var toSerialize = new NullableMoneyContainer
		{
			N = 42
		};

		var subject = new QuantityConverter();
		var options = new JsonSerializerOptions { Converters = { subject } };
		string actual = JsonSerializer.Serialize(toSerialize, options);

		Assert.That(actual, Is.EqualTo("{'S':null,'M':null,'N':42}".Jsonify()));
	}

	[Test]
	public void Serialize_CustomNullableRecord_FollowsConfiguration()
	{
		var toSerialize = new NullableMoneyRecord(null, null, 42);
		;

		var subject = new QuantityConverter();
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
	public void Deserialize_DefaultOptions_QuantityParsedProps()
	{
		string json = "'XTS 14.3'".Jsonify();
		var expected = new Money(14.3m, CurrencyIsoCode.XTS);

		var subject = new QuantityConverter();
		var options = new JsonSerializerOptions { Converters = { subject } };
		Money deserialized = JsonSerializer.Deserialize<Money>(json, options);

		Assert.That(deserialized, Is.EqualTo(expected));
	}

	[Test]
	public void Deserialize_DefaultOptions_CanRoundtrip()
	{
		var original = new Money(14.3m, CurrencyIsoCode.XTS);

		var subject = new QuantityConverter();
		var options = new JsonSerializerOptions { Converters = { subject } };
		string json = JsonSerializer.Serialize(original, options);
		var deserialized = JsonSerializer.Deserialize<Money>(json, options);

		Assert.That(deserialized, Is.EqualTo(original));
	}

	[Test]
	public void Deserialize_CustomPolicyAndEnumFormat_QuantityParsedProps()
	{
		string json = "'XTS 14.3'".Jsonify();
		var expected = new Money(14.3m, CurrencyIsoCode.XTS);

		var subject = new QuantityConverter();
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

		var subject = new QuantityConverter();
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
		var subject = new QuantityConverter();

		var options = new JsonSerializerOptions { Converters = { subject } };
		var actual = JsonSerializer.Deserialize<Money?>("null", options);
		Assert.That(actual, Is.Null);
	}

	[Test]
	public void Deserialize_DefaultOptionsRecord_PascalCasedAndNumericCurrency()
	{
		string json = "{'S':'s','M':'XTS 14.3','N':42}".Jsonify();
		var money = new Money(14.3m, CurrencyIsoCode.XTS);
		var expected = new MoneyRecord("s", money, 42);

		var subject = new QuantityConverter();
		var options = new JsonSerializerOptions { Converters = { subject } };
		MoneyRecord? actual = JsonSerializer.Deserialize<MoneyRecord>(json, options);

		Assert.That(actual, Is.EqualTo(expected));
	}

	[Test]
	public void Deserialize_CustomContainer_FollowsConfiguration()
	{
		string json = "{'s':'s','m':'XTS 14.3','n':42}".Jsonify();
		var money = new Money(14.3m, CurrencyIsoCode.XTS);

		var subject = new QuantityConverter();
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

		var subject = new QuantityConverter();
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

		var subject = new QuantityConverter();
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
	public void Deserialize_DefaultNotAString_Exception()
	{
		string notAJsonObject = "{}".Jsonify();
		var subject = new QuantityConverter();

		var options = new JsonSerializerOptions { Converters = { subject } };
		Assert.That(()=> JsonSerializer.Deserialize<Money>(notAJsonObject, options),
			Throws.InstanceOf<InvalidOperationException>()
				.With.Message.Contains("must be of type")
				.And.Message.Contains("'JsonValue'"));
	}

	[Test]
	public void Deserialize_DefaultMissingAmount_Exception()
	{
		string missingAmount = "'XTS'".Jsonify();
		var subject = new QuantityConverter();
		var options = new JsonSerializerOptions { Converters = { subject } };

		Assert.That(()=> JsonSerializer.Deserialize<Money>(missingAmount, options),
			Throws.InstanceOf<ArgumentOutOfRangeException>());
	}

	[Test]
	public void Deserialize_DefaultNonNumericAmount_Exception()
	{
		string nonNumericAmount = "'XTS lol'}".Jsonify();
		var subject = new QuantityConverter();
		var options = new JsonSerializerOptions { Converters = { subject } };

		Assert.That(()=> JsonSerializer.Deserialize<Money>(nonNumericAmount, options),
			Throws.InstanceOf<FormatException>());
	}

	[Test]
	public void Deserialize_LowercaseCurrency_Exception()
	{
		string caseMismatch = "'xts 1'".Jsonify();
		var subject = new QuantityConverter();
		var options = new JsonSerializerOptions { Converters = { subject } };

		Assert.That(()=> JsonSerializer.Deserialize<Money>( caseMismatch, options),
			Throws.InstanceOf<ArgumentException>());
	}

	[Test]
	public void Deserialize_MissingCurrency_ExceptionIgnoringCasing()
	{
		string missingCurrency = "'1'".Jsonify();
		var subject = new QuantityConverter();
		var options = new JsonSerializerOptions
		{
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
			Converters = { subject }
		};
		Assert.That(()=> JsonSerializer.Deserialize<Money>(missingCurrency, options),
			Throws.InstanceOf<ArgumentOutOfRangeException>());
	}

	[Test]
	public void Deserialize_MismatchCurrencyFormat_DoesNotMatter()
	{
		string currencyFormatMismatch = "'963 1'".Jsonify();
		var subject = new QuantityConverter();

		var options = new JsonSerializerOptions
		{
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
			Converters = { subject }
		};
		Assert.That(()=> JsonSerializer.Deserialize<Money>(currencyFormatMismatch, options),
			Throws.Nothing);

		Assert.That(JsonSerializer.Deserialize<Money>(currencyFormatMismatch, options), Is.EqualTo(1m.Xts()));
	}

	[Test]
	public void Deserialize_FunkyCurrencyValueCasing_Exception()
	{
		string funkyCurrencyValue = "'XtS 1'".Jsonify();
		var subject = new QuantityConverter();

		var options = new JsonSerializerOptions
		{
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
			Converters = { subject }
		};
		Assert.That(()=> JsonSerializer.Deserialize<Money>(funkyCurrencyValue, options),
			Throws.InstanceOf<ArgumentException>()
				.With.Message.Contains("'XtS'"));
	}

	[Test]
	public void Deserialize_UndefinedCurrency_Exception()
	{
		string undefinedCurrency = "'001 1'".Jsonify();
		var subject = new QuantityConverter();
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
		string missingObjectClose = "'XTS 1".Jsonify();
		var subject = new QuantityConverter();
		var options = new JsonSerializerOptions { Converters = { subject } };

		Assert.That(()=> JsonSerializer.Deserialize<Money>(missingObjectClose, options),
			Throws.InstanceOf<JsonException>());
	}

	#endregion

	#endregion

}
