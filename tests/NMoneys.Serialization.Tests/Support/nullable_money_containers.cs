using MongoDB.Bson;

namespace NMoneys.Serialization.Tests.Support;

public record NullableMoneyRecord(string? S, Money? M, int N);

public record NullableMoneyRecordDoc(ObjectId Id, string? S, Money? M, int N);

public class NullableMoneyContainer
{
	public string? S { get; set; }
	public Money? M { get; set; }

	public int N { get; set; }
}

public class NullableMoneyDoc : NullableMoneyContainer
{
	public ObjectId Id { get; set; }
}

public class NullableMoneyModel : NullableMoneyContainer
{
	public Guid Id { get; set; }
}
