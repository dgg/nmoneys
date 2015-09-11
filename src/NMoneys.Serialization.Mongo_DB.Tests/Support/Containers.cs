namespace NMoneys.Serialization.Mongo_DB.Tests.Support
{
	public class MoneyContainer
	{
		public string Name { get; set; }
		public Money PropName { get; set; }
	}

	public class NullableMoneyContainer
	{
		public Money? PropName { get; set; }
	}
}