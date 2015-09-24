using System.Collections.Generic;
using NUnit.Framework;
using Testing.Commons.NUnit.Constraints;

namespace NMoneys.Tests.CustomConstraints
{
	internal class CurrencyInfoConstraint : CustomConstraint<CurrencyInfo>
	{
		class AlwaysPassing : CustomConstraint<CurrencyInfo>
		{
			protected override bool matches(CurrencyInfo current)
			{
				return true;
			}
		}

		private readonly Queue<LambdaPropertyConstraint<CurrencyInfo>> _constraints;
		public CurrencyInfoConstraint()
		{
			_constraints = new Queue<LambdaPropertyConstraint<CurrencyInfo>>(10);
		}

		protected override bool matches(CurrencyInfo current)
		{
			_inner = null;
			foreach (var constraint in _constraints)
			{
				if (_inner == null) _inner = constraint;
				else
				{
					_inner = _inner & constraint;
				}
			}
			return _inner.Matches(current);
		}

		public CurrencyInfoConstraint WithCode(CurrencyIsoCode code)
		{
			_constraints.Enqueue(new LambdaPropertyConstraint<CurrencyInfo>(ci => ci.Code, Is.EqualTo(code)));
			return this;
		}
		public CurrencyInfoConstraint WithEnglishName(string englishName)
		{
			_constraints.Enqueue(new LambdaPropertyConstraint<CurrencyInfo>(ci => ci.EnglishName, Is.EqualTo(englishName)));
			return this;
		}
		public CurrencyInfoConstraint WithNativeName(string nativeName)
		{
			_constraints.Enqueue(new LambdaPropertyConstraint<CurrencyInfo>(ci => ci.NativeName, Is.EqualTo(nativeName)));
			return this;
		}
		public CurrencyInfoConstraint WithSymbol(string symbol)
		{
			_constraints.Enqueue(new LambdaPropertyConstraint<CurrencyInfo>(ci => ci.Symbol,
				Is.EqualTo(symbol)));
			return this;
		}
		public CurrencyInfoConstraint WithSignificantDecimalDigits(int decimalDigits)
		{
			_constraints.Enqueue(new LambdaPropertyConstraint<CurrencyInfo>(ci => ci.SignificantDecimalDigits, Is.EqualTo(decimalDigits)));
			return this;
		}
		public CurrencyInfoConstraint WithDecimalSeparator(string decimalSeparator)
		{
			_constraints.Enqueue(new LambdaPropertyConstraint<CurrencyInfo>(ci => ci.DecimalSeparator, Is.EqualTo(decimalSeparator)));
			return this;
		}
		public CurrencyInfoConstraint WithGroupSeparator(string groupSeparator)
		{
			_constraints.Enqueue(new LambdaPropertyConstraint<CurrencyInfo>(ci => ci.GroupSeparator, Is.EqualTo(groupSeparator)));
			return this;
		}
		public CurrencyInfoConstraint WithGroupSizes(int[] groupSizes)
		{
			_constraints.Enqueue(new LambdaPropertyConstraint<CurrencyInfo>(ci => ci.GroupSizes, Is.EqualTo(groupSizes)));
			return this;
		}
		public CurrencyInfoConstraint WithPositivePattern(int positivePattern)
		{
			_constraints.Enqueue(new LambdaPropertyConstraint<CurrencyInfo>(ci => ci.PositivePattern, Is.EqualTo(positivePattern)));
			return this;
		}
		public CurrencyInfoConstraint WithNegativePattern(int negativePattern)
		{
			_constraints.Enqueue(new LambdaPropertyConstraint<CurrencyInfo>(ci => ci.NegativePattern, Is.EqualTo(negativePattern)));
			return this;
		}
	}
}
