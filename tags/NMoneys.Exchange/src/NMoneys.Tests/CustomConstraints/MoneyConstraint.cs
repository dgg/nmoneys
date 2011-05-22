using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace NMoneys.Tests.CustomConstraints
{
	internal class MoneyConstraint : CustomConstraint<Money>
	{
		public MoneyConstraint(decimal amount, Currency currency)
		{
			_inner = new AndConstraint(
				new PropertyConstraint<Money>(m => m.Amount, Is.EqualTo(amount)),
				new PropertyConstraint<Money>(m => m.CurrencyCode, Is.EqualTo(currency.IsoCode)));
		}

		protected override bool matches(Money current)
		{
			return _inner.Matches(current);
		}
	}
}
