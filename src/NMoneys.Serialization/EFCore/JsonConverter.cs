using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Text.Json;

namespace NMoneys.Serialization.EFCore;

/// <summary>
/// Defines conversions from an instance of <see cref="Money"/> in a model to a <see cref="string"/> in the store.
/// </summary>
public class JsonConverter : ValueConverter<Money, string>
{
	/// <summary>
	/// Initializes a new instance of the <see cref="JsonConverter"/> class.
	/// </summary>
	public JsonConverter(JsonSerializerOptions options) : base(
		m => JsonSerializer.Serialize(m, options),
		str => JsonSerializer.Deserialize<Money>(str, options)) { }
}
