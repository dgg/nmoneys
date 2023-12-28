using NMoneys.Change;

namespace NMoneys.Tests.Change;

[TestFixture]
public class IntegralDenominationTester
{
	[Test]
	public void ctor_Currency_WithMinorAmount()
	{
		var pieceOfEight = new Denomination(8);
		var subject = new IntegralDenomination(pieceOfEight, Currency.Eur);

		Assert.That(subject.Denomination, Is.EqualTo(pieceOfEight));
		// because EUR is divided in 100 cents
		Assert.That(subject.IntegralAmount, Is.EqualTo(800));
	}

	[Test]
	public void ToString_SpaceSeparated_SimplifiedDenomination()
	{
		var pieceOfFive = new Denomination(5m);
		// yens don't have minor
		var subject = new IntegralDenomination(pieceOfFive, Currency.Jpy);

		Assert.That(subject.ToString(), Is.EqualTo("IntegralDenomination { IntegralAmount = 5 Denomination { 5 } }"));
		Console.WriteLine(subject.ToString());
	}
}
