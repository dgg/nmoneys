using NMoneys.Support;

namespace NMoneys.Change
{
	internal static class Positive
	{
		public static readonly Range<decimal> Amounts = new Range<decimal>(
			decimal.Zero.Open(), decimal.MaxValue.Close());
	}
}