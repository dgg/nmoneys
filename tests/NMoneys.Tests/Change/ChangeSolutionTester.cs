using NMoneys.Change;
using NMoneys.Extensions;

namespace NMoneys.Tests.Change;

[TestFixture]
public class ChangeSolutionTester
{
	[Test]
	public void ctor_NoSolution_FalseWithRemainderAndEmptyDenominations()
	{
		var noSolution = new ChangeSolution(Array.Empty<Denomination>(), 3.5m.Usd());

		Assert.That(noSolution.IsSolution, Is.False);
		Assert.That(noSolution.IsPartial, Is.False);
		Assert.That(noSolution.Remainder, Is.EqualTo(3.5m.Usd()));
		Assert.That(noSolution, Is.Empty);
	}

	[Test]
	public void ctor_CompleteSolution_TrueWithNoRemainderAndQuantifiedDenominations()
	{
		var pieceOfThree = new Denomination(3m);
		var pieceOfTwo = new Denomination(2m);
		var onePieceOfThree = new QuantifiedDenomination(pieceOfThree, 1);
		var onePieceOfTwo = new QuantifiedDenomination(pieceOfTwo, 1);

		var wholeSolution = new ChangeSolution(new[] { pieceOfThree, pieceOfTwo }, Money.Zero());

		Assert.That(wholeSolution.IsSolution, Is.True);
		Assert.That(wholeSolution.IsPartial, Is.False);
		Assert.That(wholeSolution.Remainder, Is.Null);
		Assert.That(wholeSolution, Is.EqualTo(new[] { onePieceOfThree, onePieceOfTwo }));
	}

	[Test]
	public void ctor_PartialSolution_TrueWithRemainderAndQuantifiedDenominations()
	{
		var pieceOfFour = new Denomination(4m);
		var pieceOfTwo = new Denomination(2m);
		var onePieceOfFour = new QuantifiedDenomination(pieceOfFour, 1);
		var onePieceOfTwo = new QuantifiedDenomination(pieceOfTwo, 1);

		var incompleteSolution = new ChangeSolution(new[] { pieceOfFour, pieceOfTwo }, new Money(1m));

		Assert.That(incompleteSolution.IsSolution, Is.True);
		Assert.That(incompleteSolution.IsPartial, Is.True);
		Assert.That(incompleteSolution.Remainder, Is.Not.Null);
		Assert.That(incompleteSolution, Is.EqualTo(new[] { onePieceOfFour, onePieceOfTwo }));
	}

	#region ToString

	[Test]
	public void ToString_NoSolution_FalseWithRemainderAndEmptyDenominations()
	{
		var noSolution = new ChangeSolution(Array.Empty<Denomination>(), 3.5m.Dkk());

		Assert.That(noSolution.ToString(), Is.EqualTo(
			"ChangeSolution { IsSolution = False CurrencyCode = DKK Remainder = 3,5 Denominations = [] }"
		));
	}

	[Test]
	public void ToString_CompleteSolution_TrueWithNoRemainderAndQuantifiedDenominations()
	{
		var pieceOfThree = new Denomination(3m);
		var pieceOfTwo = new Denomination(2m);

		var wholeSolution = new ChangeSolution(new[] { pieceOfThree, pieceOfTwo }, Money.Zero());

		Assert.That(wholeSolution.ToString(), Is.EqualTo(
			"ChangeSolution { IsSolution = True IsPartial = False Denominations = [ 1 * 3 | 1 * 2 ] }"
		));
	}

	[Test]
	public void ToString_PartialSolution_TrueWithRemainderAndQuantifiedDenominations()
	{
		var pieceOfFour = new Denomination(4m);
		var pieceOfTwo = new Denomination(2m);

		var incompleteSolution = new ChangeSolution(new[] { pieceOfFour, pieceOfTwo }, new Money(1m));

		Assert.That(incompleteSolution.ToString(), Is.EqualTo(
			"ChangeSolution { IsSolution = True IsPartial = True CurrencyCode = XXX Remainder = 1 Denominations = [ 1 * 4 | 1 * 2 ] }"
		));
	}

	#endregion
}
