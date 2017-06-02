namespace NMoneys.Change
{
	// class because default(decimal) is not a valid value for Value
	public class Denomination
	{
		public Denomination(decimal value)
		{
			Positive.Amounts.AssertArgument(nameof(value), value);
			Value = value;
		}

		public decimal Value { get; }

		internal bool CanBeSubstractedFrom(decimal remainder)
		{
			return remainder >= Value;
		}
	}
}