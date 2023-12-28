using NMoneys.Change;
using NMoneys.Extensions;

namespace NMoneys.Tests.Change;

[TestFixture]
public class CountWaysToMakeChangeTester
{
	[Test]
	[TestCase(0), TestCase(-1)]
	public void CountWaysToMakeChange_NotPositive_Exception(decimal notPositive)
	{
		Denomination[] any = { new (1) };
		Assert.That(() => new Money(notPositive).CountWaysToMakeChange(any), Throws.InstanceOf<ArgumentOutOfRangeException>());
	}

	[Test]
	public void CountWaysToMakeChange_NoDenominations_Zero()
	{
		Assert.That(5m.Usd().CountWaysToMakeChange(Array.Empty<Denomination>()), Is.EqualTo(0));
	}

	[Test]
	public void CountWaysToMakeChange_NullDenominations_Exception()
	{
		Denomination[] @null = null!;
		Assert.That(() => 5m.Usd().CountWaysToMakeChange(@null), Throws.ArgumentNullException);
	}

	[Test]
	public void CountWaysToMakeChange_NotPossibleToMakeChange_Zero()
	{
		Assert.That(7m.Xxx().CountWaysToMakeChange(4m, 2m), Is.EqualTo(0u));
	}

	[Test]
	public void CountWaysToMakeChange_IdentityChange_One()
	{
		Assert.That(7m.Eur().CountWaysToMakeChange(7m), Is.EqualTo(1));
	}

	private static readonly object[] _changeCountSamples =
	{
		new object[] {4m, new decimal[]{1, 2, 3}, 4u},
		new object[] {10m, new decimal[]{2, 5, 3, 6}, 5u},
		new object[] {6m, new decimal[]{1, 3, 4}, 4u},
		new object[] {5m, new decimal[]{1, 2, 3}, 5u},
		new object[] {.63m, new[]{.01m, .05m, .1m, .25m}, 73u},
	};

	[Test, TestCaseSource(nameof(_changeCountSamples))]
	public void CountWaysToMakeChange_PossibleChange_AccordingToSamples(decimal amount, decimal[] denominationValues, uint count)
	{
		var money = new Money(amount);
		Assert.That(money.CountWaysToMakeChange(denominationValues), Is.EqualTo(count));
	}
}
