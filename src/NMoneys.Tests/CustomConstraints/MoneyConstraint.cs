using NUnit.Framework;
using NUnit.Framework.Constraints;
using Testing.Commons.NUnit.Constraints;

namespace NMoneys.Tests.CustomConstraints
{
	internal class MoneyConstraint : CustomConstraint<Money>
	{
		public MoneyConstraint(decimal amount, Currency currency)
		{
			_inner = new AndConstraint(
				new LambdaPropertyConstraint<Money>(m => m.Amount, Is.EqualTo(amount)),
				new LambdaPropertyConstraint<Money>(m => m.CurrencyCode, Is.EqualTo(currency.IsoCode)));
		}

		protected override bool matches(Money current)
		{
			return _inner.Matches(current);
		}
	}
}
