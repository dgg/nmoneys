namespace NMoneys.Serialization.Tests.Support;

public record MoneyRecord(string S, Money Money, int N);

public class MoneyContainer
{
	public string? S { get; set; }
	public Money Money { get; set; }

	public int N { get; set; }
}
