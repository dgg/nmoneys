using System;
using System.Globalization;

namespace NMoneys.Change
{
	// class because default(decimal) is not a valid value for Value
	// TODO: convert to struct
	public class Denomination : IEquatable<Denomination>
	{
		public Denomination(decimal value)
		{
			Positive.Amounts.AssertArgument(nameof(value), value);
			Value = value;
		}

		public decimal Value { get; }

		internal bool CanBeSubstractedFrom(decimal remainder)
		{
			return remainder >= Value;
		}

		internal void SubstractFrom(ref decimal remainder)
		{
			remainder -= Value;
		}

		public override string ToString()
		{
			return Value.ToString(CultureInfo.InvariantCulture);
		}

		internal IntegralDenomination ToIntegral(Currency operationCurrency)
		{
			return new IntegralDenomination(this, operationCurrency);
		}

		public bool Equals(Denomination other)
		{
			if (ReferenceEquals(null, other)) return false;
			if (ReferenceEquals(this, other)) return true;
			return Value == other.Value;
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != GetType()) return false;
			return Equals((Denomination) obj);
		}

		public override int GetHashCode()
		{
			return Value.GetHashCode();
		}
	}
}