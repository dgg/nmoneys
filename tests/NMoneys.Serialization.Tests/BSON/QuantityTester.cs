using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using NMoneys.Extensions;
using NMoneys.Serialization.Tests.Support;
using Testing.Commons.Serialization;

namespace NMoneys.Serialization.Tests.BSON;

[TestFixture]
public class QuantityTester
{
	[Test]
	public void CustomSerializeDto_NoNeedOfCustomSerializer()
	{
		var dto = new Dto { S = "str", M = new MonetaryQuantity(50m, CurrencyIsoCode.EUR) };
		string json = dto.ToJson().Compact();
		Assert.That(json, Is.EqualTo("{'S':'str','M':{'Amount':'50','Currency':978}}".Jsonify()));
	}

	[Test]
	public void CustomSerializeDtr_NoNeedOfCustomSerializer()
	{
		var dtr = new Dtr("str", new MonetaryQuantity(50m, CurrencyIsoCode.EUR));
		string json = dtr.ToJson().Compact();
		Assert.That(json, Is.EqualTo("{'S':'str','M':{'Amount':'50','Currency':978}}".Jsonify()));
	}

	[Test]
	public void CustomDeserializeDto_NoNeedOfCustomSerializer()
	{
		string json = "{'S':'str','M':{'Amount':'50','Currency':978}}";

		Dto dto = BsonSerializer.Deserialize<Dto>(json);

		Assert.That(dto, Has.Property("S").EqualTo("str"));
		Assert.That(dto?.M, Has.Property("Amount").EqualTo(50m).And
			.Property("Currency").EqualTo(CurrencyIsoCode.EUR));

		// convert to money for operations
		Money m = (Money)dto!.M;
		Assert.That(m, Is.EqualTo(50m.Eur()));
	}

	[Test]
	public void CustomDeserializeDtr_NoNeedOfCustomSerializer()
	{
		string json = "{'S':'str','M':{'Amount':'50','Currency':978}}";
		Dtr? dtr = BsonSerializer.Deserialize<Dtr>(json);
		Assert.That(dtr, Has.Property("S").EqualTo("str"));
		Assert.That(dtr?.M, Has.Property("Amount").EqualTo(50m).And
			.Property("Currency").EqualTo(CurrencyIsoCode.EUR));

		// convert to money for operations
		Money m = (Money)dtr!.M;
		Assert.That(m, Is.EqualTo(50m.Eur()));
	}
}
