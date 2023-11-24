namespace NMoneys;

public partial struct Money : ICloneable
{
	/// <inheritdoc />
	public object Clone()
	{
		return new Money(Amount, CurrencyCode);
	}
}
