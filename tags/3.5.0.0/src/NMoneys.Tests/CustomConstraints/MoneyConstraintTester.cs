using System;
using NMoneys.Extensions;
using NUnit.Framework;

namespace NMoneys.Tests.CustomConstraints
{
	[TestFixture]
	public class MoneyConstraintTester
	{
		private static readonly MoneyConstraint _threeTests = new MoneyConstraint(3m, Currency.Test);

		[Test]
		public void Matches_DifferentType_Exception()
		{
			var subject = new MoneyConstraint(decimal.Zero, Currency.Test);
			decimal notAMoney = 3m;
			Assert.That(() => subject.Matches(notAMoney), Throws.InstanceOf<InvalidCastException>());
		}

		[Test]
		public void Matches_MatchingMoney_True()
		{
			Money matching = 3m.Xts();
			Assert.That(_threeTests.Matches(matching), Is.True);
		}

		[TestCaseSource("nonMatching")]
		public void Matches_NonMatchingMoney_False(decimal amount, Currency currency)
		{
			Money notMatching = new Money(amount, currency);
			Assert.That(_threeTests.Matches(notMatching), Is.False);
		}

		private static readonly object[] nonMatching = new object[]
		{
			new object[] { 3m, Currency.Euro },
			new object[] { 5m , Currency.Test },
			new object[] { 5m, Currency.Pound }
		};

		[Test]
		public void WriteMessage_NonMatchingCurrency_ContainsUsefulInformation()
		{
			TextMessageWriter writer = new TextMessageWriter();

			_threeTests.Matches(3m.Euros());
			_threeTests.WriteMessageTo(writer);

			Assert.That(writer.ToString(), Is.StringContaining("Currency").And.StringContaining("EUR").And.StringContaining("XTS"));
		}

		[Test]
		public void WriteMessage_NonMatchingAmount_ContainsUsefulInformation()
		{
			TextMessageWriter writer = new TextMessageWriter();

			_threeTests.Matches(5m.Xts());
			_threeTests.WriteMessageTo(writer);

			Assert.That(writer.ToString(), Is.StringContaining("Amount").And.StringContaining("3m").And.StringContaining("5m"));
		}

		[Test]
		public void WriteMessage_NonMatchingBothMembers_ContainsUsefulInformation()
		{
			TextMessageWriter writer = new TextMessageWriter();

			_threeTests.Matches(1.5m.Usd());
			_threeTests.WriteMessageTo(writer);

			Assert.That(writer.ToString(), Is
				.StringContaining("Amount").And.StringContaining("3m").And.StringContaining("1.5m")
				.And.StringContaining("Currency").And.StringContaining("XTS")
				.And.Not.StringContaining("USD"),
				"AndConstraint() does not include all information in the message");
		}
	}
}
