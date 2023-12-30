using System.Diagnostics.Contracts;

namespace NMoneys;

public sealed partial class Currency
{
	[Pure]
	internal decimal Round(decimal share)
	{
		decimal sign = Math.Sign(share);
		decimal raw = share - (sign * (.5m * MinAmount));
		decimal rounded = Math.Round(raw, SignificantDecimalDigits, MidpointRounding.AwayFromZero);
		return rounded;
	}
}
