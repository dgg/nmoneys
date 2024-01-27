using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;

using NMoneys.Serialization.BSON;
using NMoneys.Serialization.Tests.Support;
using Testing.Commons.Serialization;

namespace NMoneys.Serialization.Tests.BSON;

[TestFixture]
public class QuantitySerializerTester
{

	# region serialization

	[Test]
	public void Serialize_DefaultConvention_QuantityString()
	{
		var toSerialize = new Money(14.3m, CurrencyIsoCode.XTS);

		var subject = new QuantitySerializer();
		string json = toSerialize.ToJson(serializer: subject);

		Assert.That(json, Is.EqualTo("'XTS 14.3'".Jsonify()));
	}

	#endregion

	#region deserialization

	[Test]
	public void Deserialize_QuantityString_PropsSet()
	{
		string json = "'XTS 14.3'".Jsonify();
		var expected = new Money(14.3m, CurrencyIsoCode.XTS);

		var subject = new QuantitySerializer();
		Money deserialized = subject.Deserialize(BsonDeserializationContext.CreateRoot(new JsonReader(json)));

		Assert.That(deserialized, Is.EqualTo(expected));
	}

	[Test]
	public void Deserialize_CanRoundtrip()
	{
		var original = new Money(14.3m, CurrencyIsoCode.XTS);

		var subject = new QuantitySerializer();
		var json = original.ToJson(serializer: subject);
		Money deserialized = subject.Deserialize(BsonDeserializationContext.CreateRoot(new JsonReader(json)));

		Assert.That(deserialized, Is.EqualTo(original));
	}

	#endregion

	#region error handling

	[Test]
	public void Deserialize_MissingAmount_Exception()
	{
		string missingAmount = "'XTS'".Jsonify();
		var subject = new QuantitySerializer();

		Assert.That(()=> subject.Deserialize(BsonDeserializationContext.CreateRoot(new JsonReader(missingAmount))),
			Throws.InstanceOf<ArgumentOutOfRangeException>());
	}

	[Test]
	public void Deserialize_NonNumericAmount_Exception()
	{
		string nonNumericAmount = "'XTS lol'".Jsonify();
		var subject = new QuantitySerializer();

		Assert.That(()=> subject.Deserialize(BsonDeserializationContext.CreateRoot(new JsonReader(nonNumericAmount))),
			Throws.InstanceOf<FormatException>());
	}

	[Test]
	public void Deserialize_MissingCurrency_Exception()
	{
		string missingCurrency = "'42'".Jsonify();
		var subject = new QuantitySerializer();

		Assert.That(()=> subject.Deserialize(BsonDeserializationContext.CreateRoot(new JsonReader(missingCurrency))),
			Throws.InstanceOf<ArgumentOutOfRangeException>());
	}

	[Test]
	public void Deserialize_MismatchCurrencyFormat_DoesNotMatter()
	{
		string currencyFormatMismatch = "'963 42'".Jsonify();
		var subject = new QuantitySerializer();

		Assert.That(()=> subject.Deserialize(BsonDeserializationContext.CreateRoot(new JsonReader(currencyFormatMismatch))),
			Throws.Nothing);
	}

	[Test]
	public void Deserialize_FunkyCurrencyValueCasing_Exception()
	{
		string funkyCurrencyValue = "'XtS 42'".Jsonify();
		var subject = new QuantitySerializer();

		Assert.That(()=> subject.Deserialize(BsonDeserializationContext.CreateRoot(new JsonReader(funkyCurrencyValue))),
			Throws.InstanceOf<ArgumentException>());
	}

	[Test]
	public void Deserialize_UndefinedCurrency_Exception()
	{
		string undefinedCurrency = "'LOL 42'}".Jsonify();
		var subject = new QuantitySerializer();

		Assert.That(()=> subject.Deserialize(BsonDeserializationContext.CreateRoot(new JsonReader(undefinedCurrency))),
			Throws.InstanceOf<ArgumentException>());
	}

	[Test]
	public void Deserialize_PoorlyConstructedJson_Exception()
	{
		string missingObjectClose = "'XTS 42".Jsonify();
		var subject = new QuantitySerializer();

		Assert.That(()=> subject.Deserialize(BsonDeserializationContext.CreateRoot(new JsonReader(missingObjectClose))),
			Throws.InstanceOf<FormatException>());
	}

	#endregion
}
