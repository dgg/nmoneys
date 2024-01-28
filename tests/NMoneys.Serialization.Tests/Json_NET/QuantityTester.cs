using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using NMoneys.Extensions;
using NMoneys.Serialization.Tests.Support;
using Testing.Commons.Serialization;

namespace NMoneys.Serialization.Tests.Json_NET;

[TestFixture]
public class QuantityTester
{
	[Test]
	public void CustomSerializeDto_NoNeedOfCustomSerializer()
	{
		var dto = new Dto { S = "str", M = new MonetaryQuantity(50m, CurrencyIsoCode.EUR) };
		string json = JsonConvert.SerializeObject(dto, new JsonSerializerSettings
		{
			ContractResolver = new CamelCasePropertyNamesContractResolver(),
			Converters = {new StringEnumConverter() }
		});
		Assert.That(json, Is.EqualTo("{'s':'str','m':{'amount':50.0,'currency':'EUR'}}".Jsonify()));
	}

	[Test]
	public void CustomSerializeDtr_NoNeedOfCustomSerializer()
	{
		var dtr = new Dtr("str", new MonetaryQuantity(50m, CurrencyIsoCode.EUR));
		string json = JsonConvert.SerializeObject(dtr, new JsonSerializerSettings
		{
			ContractResolver = new CamelCasePropertyNamesContractResolver(),
			Converters = {new StringEnumConverter() }
		});
		Assert.That(json, Is.EqualTo("{'s':'str','m':{'amount':50.0,'currency':'EUR'}}".Jsonify()));
	}

	[Test]
	public void CustomDeserializeDto_NoNeedOfCustomSerializer()
	{
		string json = "{'s':'str','m':{'amount':50,'currency':'EUR'}}";
		Dto? dto = JsonConvert.DeserializeObject<Dto>(json, new JsonSerializerSettings
		{
			ContractResolver = new CamelCasePropertyNamesContractResolver(),
			Converters = {new StringEnumConverter() }
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
		string json = "{'s':'str','m':{'amount':50,'currency':'EUR'}}";
		Dtr? dtr = JsonConvert.DeserializeObject<Dtr>(json, new JsonSerializerSettings
		{
			ContractResolver = new CamelCasePropertyNamesContractResolver(),
			Converters = {new StringEnumConverter() }
		});
		Assert.That(dtr, Has.Property("S").EqualTo("str"));
		Assert.That(dtr?.M, Has.Property("Amount").EqualTo(50m).And
			.Property("Currency").EqualTo(CurrencyIsoCode.EUR));

		// convert to money for operations
		Money m = (Money)dtr!.M;
		Assert.That(m, Is.EqualTo(50m.Eur()));
	}
}
