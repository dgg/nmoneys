using NMoneys.Support;

namespace NMoneys.Change
{
	public class Denomination
	{
		private static readonly Range<decimal> _range = new Range<decimal>(
			decimal.Zero.Close(), decimal.MaxValue.Close());
		public Denomination(decimal value)
		{
			_range.AssertArgument(nameof(value), value);
			Value = value;
		}

		public decimal Value { get; }
	}
}