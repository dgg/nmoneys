using System;
using NMoneys.Change;
using NMoneys.Extensions;
using NMoneys.Tests.Change.Support;
using NUnit.Framework;
using Testing.Commons;

namespace NMoneys.Tests.Change
{
	[TestFixture]
	public class GetChangeTester
	{
		[Test]
		[TestCase(0), TestCase(-1)]
		public void GetChange_NotPositive_Exception(decimal notPositive)
		{
			Denomination[] any = { new Denomination(1m) };

			Assert.That(()=> new Money(notPositive).GetChange(any), Throws.InstanceOf<ArgumentOutOfRangeException>());
		}

		[Test]
		public void GetChange_SameDenominationAsAmount_OneDenominationAndNoRemainder()
		{
			var fiveX = new Money(5m);
			var oneFiver = fiveX.GetChange(5m);

			Assert.That(oneFiver, Must.Be.CompleteChange(CurrencyIsoCode.XXX, 
				1.x(5)));
		}

		[Test]
		public void GetChange_LessAmountThanMinimalDenomination_AmountRemainder()
		{
			var threeX = new Money(3m);
			var noSolution = threeX.GetChange(5m);

			Assert.That(noSolution, Has.Count.EqualTo(0));
			Assert.That(noSolution.Remainder, Is.EqualTo(threeX));
		}

		[Test]
		public void GetChange_WhenRemainder_RemainderSameCurrencyAsInitial()
		{
			var threeEuro = 3m.Eur();
			var noSolution = threeEuro.GetChange(5m);

			Assert.That(noSolution.Remainder.CurrencyCode, Is.EqualTo(threeEuro.CurrencyCode));
		}

		[Test]
		public void GetChange_ChangePossible_MultipleDenominationsNoRemainder()
		{
			var changeable = new Money(5m);
			var wholeSolution = changeable.GetChange(1m, 3m, 2m);

			Assert.That(wholeSolution, Must.Be.CompleteChange(CurrencyIsoCode.XXX,
				1.x(3), 1.x(2)));
		}

		private static readonly object[] _greedySuboptimal =
		{
			new object[]
			{
				30m, new[] {1m, 15m, 25m}, new[]
				{
					1.x(25), 5.x(1)
				}
			},
			new object[]
			{
				40m, new[] {1m, 5m, 10m, 20m, 25m}, new[]
				{
					1.x(25), 1.x(10), 1.x(5)
				}
			},
			new object[]
			{
				6m, new[] {1m, 3m, 4m}, new[]
				{
					1.x(4), 2.x(1)
				}
			},
			new object[]
			{
				63m, new[] {1m, 5m, 10m, 21m, 25m}, new[]
				{
					2.x(25), 1.x(10), 3.x(1)
				}
			}
		};

		[Test, TestCaseSource(nameof(_greedySuboptimal))]
		public void GetChange_NonOptimalForGreedy_SubOptimalSolution(decimal amount, decimal[] denominationValues, QDenomination[] solution)
		{
			var changeable = new Money(amount);

			var subOptimalSolution = changeable.GetChange(denominationValues);

			Assert.That(subOptimalSolution, Must.Be.CompleteChange(CurrencyIsoCode.XXX, solution));
		}

		[Test]
		public void GetChange_NotCompleteSolution_QuantifiedAndRemainder()
		{
			var notCompletelyChangeable = new Money(7m);

			var incompleteSolution = notCompletelyChangeable.GetChange(4m, 2m);

			Assert.That(incompleteSolution, Must.Not.Be.CompleteChange(1m.Xxx(), 
				1.x(4), 1.x(2)));
		}

		[Test]
		public void GetChange_NoDenominations_EmptySolution()
		{
			var toBeChanged = new Money(5m);

			var emptySolution = toBeChanged.GetChange(new Denomination[0]);

			Assert.That(emptySolution, Is.Empty);
			Assert.That(emptySolution.Count, Is.EqualTo(0));

			Assert.That(emptySolution.Remainder, Is.EqualTo(toBeChanged));
		}
	}
}