namespace NMoneys.Serialization.Tests.Support;

public class Dto
{
	public string S { get; set; }
	public MonetaryQuantity M { get; set; }
}

public record Dtr(string S, MonetaryQuantity M);
