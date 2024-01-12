using System.Text.Json;
using System.Text.Json.Serialization;
using NMoneys.Serialization.Tests.Support;
using NMoneys.Serialization.Text_Json;

namespace NMoneys.Serialization.Tests.Text_Json;

[TestFixture]
public class MoneyConverterTester
{
	[Test]
	public void DriveSerialization()
	{
		var m = new Money(13.45m, CurrencyIsoCode.EUR);

		var converter = new MoneyConverter();
		var options = new JsonSerializerOptions
		{
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
			Converters = { new MoneyConverter(), new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
		};
		var r = new MoneyRecord("S", m, 2);
		string serialized = JsonSerializer.Serialize(m, options);
		Console.WriteLine(serialized);

		Console.WriteLine(JsonSerializer.Deserialize<Money>(serialized, options));
	}
}
