using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using NMoneys.Serialization.BSON;
using NMoneys.Serialization.Tests.Support;
using Testing.Commons.Serialization;

namespace NMoneys.Serialization.Tests.BSON;

[TestFixture, Explicit("cannot change conventions mid-test :-(")]
public class MoneySerializerWithCustomConventionsTester
{
	[OneTimeSetUp]
	public void RegisterSerializer()
	{
		var subject = new MoneySerializer();
		BsonSerializer.RegisterSerializer(subject);
		ConventionPack custom = new()
		{
			new CamelCaseElementNameConvention(),
			new EnumRepresentationConvention(BsonType.String)
		};
		ConventionRegistry.Register(nameof(custom), custom, _ => true);
	}

	#region serialize

	[Test]
	public void Serialize_CustomPolicyAndEnumFormat_FollowsCaseAndCurrencyFormat()
	{
		var toSerialize = new Money(14.3m, CurrencyIsoCode.XTS);

		string json = toSerialize.ToJson().Compact();

		Assert.That(json, Is.EqualTo("{'amount':NumberDecimal('14.3'),'currency':'XTS'}".Jsonify()));
	}

	[Test]
	public void Serialize_Null_DefaultNullable()
	{
		Money? @null = default(Money?);

		string json = @null.ToJson().Compact();
		Assert.That(json, Is.EqualTo("null"));
	}

	[Test]
	public void Serialize_Record_FollowsConfiguration()
	{
		var money = new Money(14.3m, CurrencyIsoCode.XTS);
		var toSerialize = new MoneyRecord("s", money, 42);

		string json = toSerialize.ToJson().Compact();

		Assert.That(json, Is.EqualTo("{'s':'s','m':{'amount':NumberDecimal('14.3'),'currency':'XTS'},'n':42}".Jsonify()));
	}

	[Test]
	public void Serialize_Container_FollowsConfiguration()
	{
		var money = new Money(14.3m, CurrencyIsoCode.XTS);
		var toSerialize = new MoneyContainer
		{
			S = "s",
			M = money,
			N = 42
		};

		string json = toSerialize.ToJson().Compact();

		Assert.That(json, Is.EqualTo("{'s':'s','m':{'amount':NumberDecimal('14.3'),'currency':'XTS'},'n':42}".Jsonify()));
	}

	[Test]
	public void Serialize_NullableContainer_FollowsConfiguration()
	{
		var toSerialize = new NullableMoneyContainer
		{
			N = 42
		};

		string json = toSerialize.ToJson().Compact();

		Assert.That(json, Is.EqualTo("{'s':null,'m':null,'n':42}".Jsonify()));
	}

	[Test]
	public void Serialize_NullableRecord_FollowsConfiguration()
	{
		var toSerialize = new NullableMoneyRecord(null, null, 42);


		string json = toSerialize.ToJson().Compact();

		Assert.That(json, Is.EqualTo("{'s':null,'m':null,'n':42}".Jsonify()));
	}

	#endregion

	#region deserialize

	[Test]
	public void Deserialize_CustomPolicyAndEnumFormat_FollowsCaseAndCurrencyFormat()
	{
		string json = "{'amount':NumberDecimal('14.3'),'currency':'xts'}".Jsonify();
		var expected = new Money(14.3m, CurrencyIsoCode.XTS);

		Money actual = BsonSerializer.Deserialize<Money>(json);

		Assert.That(actual, Is.EqualTo(expected));
	}

	[Test]
	public void Deserialize_CustomPolicyAndEnumFormat_CanRoundtrip()
	{
		var original = new Money(14.3m, CurrencyIsoCode.XTS);

		string json = original.ToJson().Compact();

		var deserialized = BsonSerializer.Deserialize<Money>(json);

		Assert.That(deserialized, Is.EqualTo(original));
	}

	[Test]
	public void Deserialize_Record()
	{
		string json = "{'s':'s','m':{'amount':NumberDecimal('14.3'),'currency':'XTS'},'n':42}".Jsonify();
		var money = new Money(14.3m, CurrencyIsoCode.XTS);
		var expected = new MoneyRecord("s", money, 42);

		MoneyRecord actual = BsonSerializer.Deserialize<MoneyRecord>(json);

		Assert.That(actual, Is.EqualTo(expected));
	}

	[Test]
	public void Deserialize_Container()
	{
		string json = "{'s':'s','m':{'amount':NumberDecimal('14.3'),'currency':'XTS'},'n':42}".Jsonify();
		var money = new Money(14.3m, CurrencyIsoCode.XTS);

		var actual = BsonSerializer.Deserialize<MoneyContainer>(json);

		Assert.That(actual, Has.Property("S").EqualTo("s"));
		Assert.That(actual, Has.Property("M").EqualTo(money));
		Assert.That(actual, Has.Property("N").EqualTo(42));
	}

	[Test]
	public void Deserialize_NullableRecord()
	{
		string json = "{'s':null,'m':null,'n':42}".Jsonify();
		var expected = new NullableMoneyRecord(null, null, 42);

		var actual = BsonSerializer.Deserialize<NullableMoneyRecord>(json);

		Assert.That(actual, Is.EqualTo(expected));
	}

	#endregion
}
