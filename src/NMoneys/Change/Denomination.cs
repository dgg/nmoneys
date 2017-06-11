using System;
using System.Globalization;

namespace NMoneys.Change
{
	public struct Denomination : IEquatable<Denomination>
	{
		public Denomination(decimal value)
		{
			Positive.Amounts.AssertArgument(nameof(value), value);
			_value = value;
		}

		private readonly decimal? _value;
		public decimal Value => _value.GetValueOrDefault(1);

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
			return _value == other._value;
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			return obj is Denomination && Equals((Denomination) obj);
		}

		public override int GetHashCode()
		{
			return _value.GetHashCode();
		}
	}
}