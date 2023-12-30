using NMoneys.Change;

namespace NMoneys.Tests.Change;

[TestFixture]
public class QuantifiedDenominationTester
{
	#region ToString()

	[Test]
	public void ToString_TimesDenomination_FormattedValue()
	{
		var subject = new QuantifiedDenomination(new Denomination(3m), 2u);

		Assert.That(subject.ToString(), Is.EqualTo("QuantifiedDenomination { 2 * 3 }"));
	}

	[Test, SetCulture("da-DK")]
	public void ToString_TimesDenomination_FormatIndependent()
	{
		var subject = new QuantifiedDenomination(new Denomination(3.5m), 2u);

		// DK decimals with ,
		Assert.That(subject.ToString(), Is.EqualTo("QuantifiedDenomination { 2 * 3.5 }"));
	}

	#endregion

	#region Aggregate

	[Test]
	public void Aggregate_NotOverlappingDenominations_SingleOnes()
	{
		var different = new[] { new Denomination(1), new Denomination(2), new Denomination(5) };

		var aggregated = QuantifiedDenomination.Aggregate(different);

		var quantityOfOne = Has.Property(nameof(QuantifiedDenomination.Quantity)).EqualTo(1);
		Assert.That(aggregated, Has.All.Matches(quantityOfOne));
	}

	[Test]
	public void Aggregate_OverlappingDenominations_SumsQuantities()
	{
		var pieceOfTwo = new Denomination(2);
		var anotherPieceOfTwo = new Denomination(2);
		var pieceOfFive = new Denomination(5);
		var someOverlap = new[] { pieceOfTwo, anotherPieceOfTwo, pieceOfFive };

		var aggregated = QuantifiedDenomination.Aggregate(someOverlap);

		Assert.That(aggregated.ToArray(), Is.EqualTo(new[]
		{
			new QuantifiedDenomination(pieceOfTwo, 2),
			new QuantifiedDenomination(pieceOfFive, 1)
		}));
	}

	#endregion
}
