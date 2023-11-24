namespace NMoneys.Tests.Support;

public partial class Iz : Is
{
	public static MoneyConstraint MoneyWith(decimal amount, Currency currency)
	{
		return new MoneyConstraint(amount, currency);
	}
}
