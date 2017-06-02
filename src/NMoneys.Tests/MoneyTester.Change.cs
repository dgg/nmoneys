using NMoneys.Change;
using NMoneys.Extensions;
using NUnit.Framework;

namespace NMoneys.Tests
{
	[TestFixture]
	public partial class MoneyTester
	{
		[Test]
		public void MinChange_SameDenominationAsAmount_OneDenominationAndNoRemainder()
		{
			var fiveX = new Money(5m);
			var fiver = new Denomination(5m);
			var oneFiver = fiveX.MinChange(new []{ fiver });

			Assert.That(oneFiver, Has.Count.EqualTo(1));
			Assert.That(oneFiver[0].Quantity, Is.EqualTo(1u));
			Assert.That(oneFiver[0].Denomination, Is.EqualTo(fiver));
			Assert.That(oneFiver.Remainder, Is.EqualTo(0m.Xxx()));
		}

		[Test]
		public void MinChange_LessAmountThanMinimalDenomination_AmountRemainder()
		{
			var threeX = new Money(3m);
			var fiver = new Denomination(5m);
			var noSolution = threeX.MinChange(new[] { fiver });

			Assert.That(noSolution, Has.Count.EqualTo(0));
			Assert.That(noSolution.Remainder, Is.EqualTo(threeX));
		}

		[Test]
		public void MinChange_WhenRemainder_RemainderSameCurrencyAsInitial()
		{
			var threeEuro = 3m.Eur();
			var fiver = new Denomination(5m);
			var noSolution = threeEuro.MinChange(new[] {fiver});

			Assert.That(noSolution.Remainder.CurrencyCode, Is.EqualTo(threeEuro.CurrencyCode));
		}
	}
}