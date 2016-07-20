using System.Collections.Generic;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using Testing.Commons;
using Testing.Commons.NUnit.Constraints;

namespace NMoneys.Tests.CustomConstraints
{
	internal class CurrencyInfoConstraint : DelegatingConstraint
	{
		private readonly Queue<ComposablePropertyConstraint> _constraints;
		public CurrencyInfoConstraint()
		{
			_constraints = new Queue<ComposablePropertyConstraint>(10);
		}

		public CurrencyInfoConstraint WithCode(CurrencyIsoCode code)
		{
			_constraints.Enqueue(Must.Have.Property(nameof(CurrencyInfo.Code), Is.EqualTo(code)));
			return this;
		}
		public CurrencyInfoConstraint WithEnglishName(string englishName)
		{
			_constraints.Enqueue(Must.Have.Property(nameof(CurrencyInfo.EnglishName), Is.EqualTo(englishName)));
			return this;
		}
		public CurrencyInfoConstraint WithNativeName(string nativeName)
		{
			_constraints.Enqueue(Must.Have.Property(nameof(CurrencyInfo.NativeName), Is.EqualTo(nativeName)));
			return this;
		}
		public CurrencyInfoConstraint WithSymbol(string symbol)
		{
			_constraints.Enqueue(Must.Have.Property(nameof(CurrencyInfo.Symbol),
				Is.EqualTo(symbol)));
			return this;
		}
		public CurrencyInfoConstraint WithSignificantDecimalDigits(int decimalDigits)
		{
			_constraints.Enqueue(Must.Have.Property(nameof(CurrencyInfo.SignificantDecimalDigits), Is.EqualTo(decimalDigits)));
			return this;
		}
		public CurrencyInfoConstraint WithDecimalSeparator(string decimalSeparator)
		{
			_constraints.Enqueue(Must.Have.Property(nameof(CurrencyInfo.DecimalSeparator), Is.EqualTo(decimalSeparator)));
			return this;
		}
		public CurrencyInfoConstraint WithGroupSeparator(string groupSeparator)
		{
			_constraints.Enqueue(Must.Have.Property(nameof(CurrencyInfo.GroupSeparator), Is.EqualTo(groupSeparator)));
			return this;
		}
		public CurrencyInfoConstraint WithGroupSizes(int[] groupSizes)
		{
			_constraints.Enqueue(Must.Have.Property(nameof(CurrencyInfo.GroupSizes), Is.EqualTo(groupSizes)));
			return this;
		}
		public CurrencyInfoConstraint WithPositivePattern(int positivePattern)
		{
			_constraints.Enqueue(Must.Have.Property(nameof(CurrencyInfo.PositivePattern), Is.EqualTo(positivePattern)));
			return this;
		}
		public CurrencyInfoConstraint WithNegativePattern(int negativePattern)
		{
			_constraints.Enqueue(Must.Have.Property(nameof(CurrencyInfo.NegativePattern), Is.EqualTo(negativePattern)));
			return this;
		}

		protected override ConstraintResult matches(object current)
		{
			Delegate = Must.Satisfy.Conjunction(_constraints.ToArray());
			return Delegate.ApplyTo(current);
		}
	}
}
