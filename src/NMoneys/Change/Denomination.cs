namespace NMoneys.Change
{
	public class Denomination
	{
		public Denomination(decimal value)
		{
			Value = value;
		}

		public decimal Value { get; }
	}
}