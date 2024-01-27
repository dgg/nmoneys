// ReSharper disable PropertyCanBeMadeInitOnly.Global
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
namespace NMoneys.Serialization.Tests.Support;

public class Dto
{

	public string S { get; set; }
	public MonetaryQuantity M { get; set; }
}

public record Dtr(string S, MonetaryQuantity M);
