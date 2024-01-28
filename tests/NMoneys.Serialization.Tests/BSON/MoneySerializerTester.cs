using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;

using NMoneys.Serialization.BSON;
using NMoneys.Serialization.Tests.Support;
using Testing.Commons.Serialization;

namespace NMoneys.Serialization.Tests.BSON;

[TestFixture]
public class MoneySerializerTester
{

	# region serialization

	[Test]
	public void Serialize_DefaultConvention_PascalCasedAndNumericCurrency()
	{
		var toSerialize = new Money(14.3m, CurrencyIsoCode.XTS);

		var subject = new MoneySerializer();
		string json = toSerialize.ToJson(serializer: subject).Compact();

		Assert.That(json, Is.EqualTo("{'Amount':NumberDecimal('14.3'),'Currency':963}".Jsonify()));
	}

	#endregion

	#region deserialization

	[Test]
	public void Deserialize_PascalCasedAndNumericCurrency()
	{
		string json = "{'Amount':NumberDecimal('14.3'),'Currency':963}".Jsonify();
		var expected = new Money(14.3m, CurrencyIsoCode.XTS);

		var subject = new MoneySerializer();
		Money deserialized = subject.Deserialize(BsonDeserializationContext.CreateRoot(new JsonReader(json)));

		Assert.That(deserialized, Is.EqualTo(expected));
	}

	[Test]
	public void Deserialize_CanRoundtrip()
	{
		var original = new Money(14.3m, CurrencyIsoCode.XTS);

		var subject = new MoneySerializer();
		var json = original.ToJson(serializer: subject);
		Money deserialized = subject.Deserialize(BsonDeserializationContext.CreateRoot(new JsonReader(json)));

		Assert.That(deserialized, Is.EqualTo(original));
	}

	#endregion

	#region error handling

	[Test]
	public void Deserialize_NotAnObject_Exception()
	{
		string notAJsonObject = "'str'".Jsonify();
		var subject = new MoneySerializer();

		Assert.That(()=> subject.Deserialize(BsonDeserializationContext.CreateRoot(new JsonReader(notAJsonObject))),
			Throws.InstanceOf<InvalidOperationException>()
				.With.Message.Contains("CurrentBsonType is Document")
				.And.Message.Contains("CurrentBsonType is String"));
	}

	[Test]
	public void Deserialize_MissingAmount_Exception()
	{
		string missingAmount = "{}".Jsonify();
		var subject = new MoneySerializer();

		Assert.That(()=> subject.Deserialize(BsonDeserializationContext.CreateRoot(new JsonReader(missingAmount))),
			Throws.InstanceOf<InvalidOperationException>()
				.With.Message.Contains("when State is Name")
				.And.Message.Contains("not when State is EndOfDocument"));
	}

	[Test]
	public void Deserialize_NonNumericAmount_Exception()
	{
		string nonNumericAmount = "{'Amount':[]}".Jsonify();
		var subject = new MoneySerializer();

		Assert.That(()=> subject.Deserialize(BsonDeserializationContext.CreateRoot(new JsonReader(nonNumericAmount))),
			Throws.InstanceOf<NotSupportedException>()
				.With.Message.Contains("Cannot extract a monetary amount out of")
				.And.Message.Contains("'Array'"));
	}

	[Test]
	public void Deserialize_CaseDoesNotMatch_Exception()
	{
		string caseMismatch = "{'amount':'1','currency':963}".Jsonify();
		var subject = new MoneySerializer();

		Assert.That(()=> subject.Deserialize(BsonDeserializationContext.CreateRoot(new JsonReader(caseMismatch))),
			Throws.InstanceOf<FormatException>()
				.With.Message.Contains("element name to be 'Amount'")
				.And.Message.Contains("not 'amount'"));
	}

	[Test]
	public void Deserialize_MissingCurrency_Exception()
	{
		string missingCurrency = "{'Amount':'1','other':1}".Jsonify();
		var subject = new MoneySerializer();

		Assert.That(()=> subject.Deserialize(BsonDeserializationContext.CreateRoot(new JsonReader(missingCurrency))),
			Throws.InstanceOf<FormatException>()
				.With.Message.Contains("element name to be 'Currency'"));
	}

	[Test]
	public void Deserialize_MismatchCurrencyFormat_DoesNotMatter()
	{
		string currencyFormatMismatch = "{'Amount':'2','Currency':'XTS'}".Jsonify();
		var subject = new MoneySerializer();

		Assert.That(()=> subject.Deserialize(BsonDeserializationContext.CreateRoot(new JsonReader(currencyFormatMismatch))),
			Throws.Nothing);
	}

	[Test]
	public void Deserialize_FunkyCurrencyValueCasing_DoesNotMatter()
	{
		string funkyCurrencyValue = "{'Amount':'1','Currency':'XtS'}".Jsonify();
		var subject = new MoneySerializer();

		Assert.That(()=> subject.Deserialize(BsonDeserializationContext.CreateRoot(new JsonReader(funkyCurrencyValue))),
			Throws.Nothing);
	}

	[Test]
	public void Deserialize_UndefinedCurrency_Exception()
	{
		string undefinedCurrency = "{'Amount':'1','Currency':1}".Jsonify();
		var subject = new MoneySerializer();

		Assert.That(()=> subject.Deserialize(BsonDeserializationContext.CreateRoot(new JsonReader(undefinedCurrency))),
			Throws.InstanceOf<UndefinedCodeException>());
	}

	[Test]
	public void Deserialize_PoorlyConstructedJson_Exception()
	{
		string missingObjectClose = "{'Amount':'1','Currency':1".Jsonify();
		var subject = new MoneySerializer();

		Assert.That(()=> subject.Deserialize(BsonDeserializationContext.CreateRoot(new JsonReader(missingObjectClose))),
			Throws.InstanceOf<FormatException>());
	}

	#endregion
}
