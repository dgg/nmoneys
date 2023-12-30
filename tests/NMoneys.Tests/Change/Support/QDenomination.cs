namespace NMoneys.Tests.Change.Support;

public readonly record struct QDenomination(uint Quantity, decimal Denomination);

internal static class DenominationExtensions
{
	public static QDenomination x(this int quantity, int denominationValue)
	{
		return new QDenomination((uint)quantity, denominationValue);
	}

	public static QDenomination q(this int denominationValue, int quantity)
	{
		return new QDenomination((uint) quantity, denominationValue);
	}
}
