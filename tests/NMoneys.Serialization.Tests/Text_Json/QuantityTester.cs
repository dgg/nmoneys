using System.Text.Json;
using System.Text.Json.Serialization;
using NMoneys.Extensions;
using NMoneys.Serialization.Tests.Support;
using Testing.Commons.Serialization;

namespace NMoneys.Serialization.Tests.Text_Json;

[TestFixture]
public class QuantityTester
{
	[Test]
	public void CustomSerializeDto_NoNeedOfCustomSerializer()
	{
		var dto = new Dto { S = "str", M = new MonetaryQuantity(50m, CurrencyIsoCode.EUR) };

		string json = JsonSerializer.Serialize(dto, new JsonSerializerOptions()
		{
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
			Converters = { new JsonStringEnumConverter() }
		});
		Assert.That(json, Is.EqualTo("{'s':'str','m':{'amount':50,'currency':'EUR'}}".Jsonify()));
	}

	[Test]
	public void CustomSerializeDtr_NoNeedOfCustomSerializer()
	{
		var dtr = new Dtr("str", new MonetaryQuantity(50m, CurrencyIsoCode.EUR));
		string json = JsonSerializer.Serialize(dtr, new JsonSerializerOptions()
		{
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
			Converters = { new JsonStringEnumConverter() }
		});
		Assert.That(json, Is.EqualTo("{'s':'str','m':{'amount':50,'currency':'EUR'}}".Jsonify()));
	}

	[Test]
	public void CustomDeserializeDto_NoNeedOfCustomSerializer()
	{
		string json = "{'s':'str','m':{'amount':50,'currency':'EUR'}}".Jsonify();
		Dto? dto = JsonSerializer.Deserialize<Dto>(json, new JsonSerializerOptions()
		{
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
			Converters = { new JsonStringEnumConverter() }
		});
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
		string json = "{'s':'str','m':{'amount':50,'currency':'EUR'}}".Jsonify();
		Dtr? dtr = JsonSerializer.Deserialize<Dtr>(json, new JsonSerializerOptions()
		{
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
			Converters = { new JsonStringEnumConverter() }
		});
		Assert.That(dtr, Has.Property("S").EqualTo("str"));
		Assert.That(dtr?.M, Has.Property("Amount").EqualTo(50m).And
			.Property("Currency").EqualTo(CurrencyIsoCode.EUR));

		// convert to money for operations
		Money m = (Money)dtr!.M;
		Assert.That(m, Is.EqualTo(50m.Eur()));
	}
}
