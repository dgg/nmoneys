namespace NMoneys.Change;

using NMoneys.Support;

internal static class Positive
{
	public static readonly Range<decimal> Amounts = new Range<decimal>(
		decimal.Zero.Open(), decimal.MaxValue.Close());
}
