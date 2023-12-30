using System.Text;

namespace NMoneys.Change;

internal readonly record struct IntegralDenomination(Denomination Denomination, long IntegralAmount)
{
	internal IntegralDenomination(Denomination denomination, Currency operationCurrency) :
		this(denomination, CalculateAmount(denomination, operationCurrency))
	{
	}

	private bool PrintMembers(StringBuilder builder)
	{
		builder.Append(nameof(IntegralAmount)).Append(" = ").Append(IntegralAmount).Append(' ');
		builder.Append(Denomination);
		return true;
	}

	internal static long CalculateAmount(Denomination denomination, Currency operationCurrency)
	{
		long integralAmount = Convert.ToInt64(Money.CalculateMinorAmount(denomination.Value, operationCurrency));
		return integralAmount;
	}

	internal static IntegralDenomination Default(Currency operationCurrency)
	{
		return new IntegralDenomination(new Denomination(1), operationCurrency);
	}
}
