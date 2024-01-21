using MongoDB.Bson;

namespace NMoneys.Serialization.Tests.Support;

public record MoneyRecord(string S, Money M, int N);

public record MoneyRecordDoc(ObjectId Id, string S, Money M, int N);

public class MoneyContainer
{
	public string? S { get; set; }
	public Money M { get; set; }

	public int N { get; set; }
}

public class MoneyDoc : MoneyContainer
{
	public ObjectId Id { get; set; }
}
