using System;

namespace NMoneys.Change
{
	internal struct IntegralDenomination : IEquatable<IntegralDenomination>
	{
		public Denomination Denomination { get; }
		public long IntegralAmount { get; }

		public IntegralDenomination(Denomination denomination, Currency operationCurrency)
		{
			Denomination = denomination;
			IntegralAmount = CalculateAmount(denomination, operationCurrency);
		}

		public static IntegralDenomination Default(Currency operationCurrency)
		{
			return new IntegralDenomination(new Denomination(1), operationCurrency);
		}

		internal static long CalculateAmount(Denomination denomination, Currency operationCurrency)
		{
			long integralAmount = Convert.ToInt64(Money.CalculateMinorAmount(denomination.Value, operationCurrency));
			return integralAmount;
		}


		public bool Equals(IntegralDenomination other)
		{
			return Denomination.Equals(other.Denomination) &&
				IntegralAmount == other.IntegralAmount;
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			return obj is IntegralDenomination denomination && Equals(denomination);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				return (Denomination.GetHashCode() * 397) ^ IntegralAmount.GetHashCode();
			}
		}
	}
}