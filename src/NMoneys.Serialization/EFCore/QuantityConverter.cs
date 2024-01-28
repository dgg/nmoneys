using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace NMoneys.Serialization.EFCore;

/// <summary>
/// Defines conversions from an instance of <see cref="Money"/> in a model to a <see cref="string"/> in the store.
/// </summary>
public class QuantityConverter : ValueConverter<Money, string>
{
	/// <summary>
	/// Initializes a new instance of the <see cref="QuantityConverter"/> class.
	/// </summary>
	public QuantityConverter() : base(
		m => m.AsQuantity(),
		str => Money.Parse(str)) { }
}
