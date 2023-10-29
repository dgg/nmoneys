using System.Diagnostics.Contracts;

namespace NMoneys.Support;

internal static class SmallPowerOfTen
{
	private static readonly ushort[] _positive = { 1, 10, 100, 1000, 10000 };
	private static readonly decimal[] _negative = { 1m, .1m, .01m, .001m, .0001m, .00001m };

	/// <summary>
	/// 10 ^ <paramref name="exponent"/>.
	/// </summary>
	[Pure]
	public static ushort Positive(byte exponent)
	{
		if (exponent > _positive.Length - 1)
		{
			throw new ArgumentOutOfRangeException(nameof(exponent), exponent, "Exponent too large.");
		}

		return _positive[exponent];
	}

	/// <summary>
	/// 10 ^ -<paramref name="negativeExponent"/>.
	/// </summary>
	[Pure]
	public static decimal Negative(byte negativeExponent)
	{
		if (negativeExponent > _positive.Length - 1)
		{
			throw new ArgumentOutOfRangeException(nameof(negativeExponent), negativeExponent, "Exponent too large.");
		}

		return _negative[negativeExponent];
	}
}
