using NMoneys.Extensions;
using NUnit.Framework;
using Testing.Commons.NUnit.Constraints.Support;

namespace NMoneys.Tests.CustomConstraints
{
	[TestFixture]
	public class MoneyConstraintTester : ConstraintTesterBase
	{
		private static readonly MoneyConstraint _threeTests = new MoneyConstraint(3m, Currency.Test);

		[Test]
		public void Matches_MatchingMoney_True()
		{
			Money matching = 3m.Xts();
			Assert.That(matches(_threeTests, matching), Is.True);
		}

		[TestCaseSource(nameof(nonMatching))]
		public void Matches_NonMatchingMoney_False(decimal amount, Currency currency)
		{
			Money notMatching = new Money(amount, currency);
			Assert.That(matches(_threeTests, notMatching), Is.False);
		}

		private static readonly object[] nonMatching =
		{
			new object[] { 3m, Currency.Euro },
			new object[] { 5m , Currency.Test },
			new object[] { 5m, Currency.Pound }
		};

		[Test]
		public void WriteMessage_NonMatchingCurrency_ContainsUsefulInformation()
		{
			Assert.That(getMessage(_threeTests, 3m.Euros()), Does.Contain("Currency")
				.And.Contain("EUR")
				.And.Contain("XTS"));
		}

		[Test]
		public void WriteMessage_NonMatchingAmount_ContainsUsefulInformation()
		{
			Assert.That(getMessage(_threeTests, 5m.Xts()), Does.Contain("Amount")
				.And.Contain("3m")
				.And.Contain("5m"));
		}

		[Test]
		public void WriteMessage_NonMatchingBothMembers_ContainsUsefulInformation()
		{
			Assert.That(getMessage(_threeTests, 11.5m.Usd()), Does.Contain("Amount")
				.And.Contain("3m")
				.And.Contain("1.5m")
				.And.Contain("Currency")
				.And.Contain("XTS")
				.And.Not.Contains("USD"),
				"AndConstraint() does not include all information in the message");
		}
	}
}
