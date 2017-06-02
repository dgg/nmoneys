using NMoneys.Support;

namespace NMoneys.Change
{
	// class because default(decimal) is not a valid value for Value
	public class Denomination
	{
		private static readonly Range<decimal> _range = new Range<decimal>(
			decimal.Zero.Open(), decimal.MaxValue.Close());
		public Denomination(decimal value)
		{
			_range.AssertArgument(nameof(value), value);
			Value = value;
		}

		public decimal Value { get; }
	}
}