namespace NMoneys.Serialization.Tests.Support;

public record NullableMoneyRecord(string Prop, Money? Money);

public class NullableMoneyContainer
{
	public string? Prop { get; set; }
	public Money? Money { get; set; }
}
