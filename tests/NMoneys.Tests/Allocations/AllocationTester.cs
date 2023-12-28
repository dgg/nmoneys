using NMoneys.Allocations;
using NMoneys.Extensions;

namespace NMoneys.Tests.Allocations;

[TestFixture]
public class AllocationTester
{
	#region ctor

	[Test]
	public void Ctor_AllocationOfDisparateCurrencies_Exception()
	{
		Money allocatableEur = 1m.Eur();
		var allocatedCad = new[] { .5m.Cad(), .5m.Cad() };
		Assert.That(() => new Allocation(allocatableEur, allocatedCad),
			Throws.InstanceOf<DifferentCurrencyException>());
	}

	[Test]
	public void Ctor_AllocatedQuantitiesWithDifferentCurrencies_Exception()
	{
		Money allocatableEur = 1m.Eur();
		var someAreNotEur = new[] { .5m.Eur(), .25m.Cad(), .5m.Eur() };
		Assert.That(() => new Allocation(allocatableEur, someAreNotEur),
			Throws.InstanceOf<DifferentCurrencyException>());
	}

	[Test]
	public void Ctor_AllocatedMoreThanWasPossible_Exception()
	{
		Money allocatable = 1m.Dkk();
		var allocated = new[] { .75m.Dkk(), .75m.Dkk() };

		Assert.That(() => new Allocation(allocatable, allocated), Throws.InstanceOf<ArgumentOutOfRangeException>());
	}

	[TestCaseSource(nameof(allocationResults))]
	public void Ctor_AllocatedEnumerable(Money moreThanAllocated, Money[] allocated)
	{
		var subject = new Allocation(moreThanAllocated, allocated);

		Assert.That(subject, Is.EqualTo(allocated));
	}

	[TestCaseSource(nameof(allocationResults))]
	public void Ctor_AllocatedIndexable(Money moreThanAllocated, Money[] allocated)
	{
		var subject = new Allocation(moreThanAllocated, allocated);

		Assert.That(subject, Has.Count.EqualTo(allocated.Length));

		for (int i = 0; i < allocated.Length; i++)
		{
			Assert.That(subject[i], Is.EqualTo(allocated[i]));
		}
	}

	private static readonly IEnumerable<TestCaseData> allocationResults = new[]
	{
		new TestCaseData(10m.Dkk(), new[] { 8m.Dkk(), 2m.Dkk() }).SetName("All money allocated"),
		new TestCaseData(10m.Dkk(), new[] { 6m.Dkk(), 2m.Dkk() }).SetName("Some money not allocated"),
		new TestCaseData(10m.Dkk(), new[] { 9.99m.Dkk(), .001m.Dkk() }).SetName("Some marginal money not allocated")
	};

	[Test]
	public void Ctor_NoAllocated_Exception()
	{
		Money allocatable = 10m.Usd();
		var empty = new Money[] { };

		Assert.That(()=> new Allocation(allocatable, empty), Throws.ArgumentException);
	}

	[Test]
	public void Ctor_AllMoneyAllocated_Complete()
	{
		Money allocatable = 10m.Usd();
		var allocated = new[] { 5m.Usd(), 5m.Usd() };

		var subject = new Allocation(allocatable, allocated);

		Assert.That(subject.TotalAllocated, Is.EqualTo(10m.Usd()));
		Assert.That(subject.Remainder, Is.EqualTo(Money.Zero(CurrencyIsoCode.USD)));
		Assert.That(subject.IsComplete, Is.True);
		Assert.That(subject.IsQuasiComplete, Is.False);
	}

	[Test]
	public void Ctor_SomeMoneyNotAllocated_Incomplete()
	{
		Money allocatable = 10m.Eur();
		var allocated = new[] { 6m.Eur(), 3m.Eur() };
		var subject = new Allocation(allocatable, allocated);

		Assert.That(subject.IsComplete, Is.False);
		Assert.That(subject.IsQuasiComplete, Is.False);
		Assert.That(subject.TotalAllocated, Is.EqualTo(9m.Eur()));
		Assert.That(subject.Remainder, Is.EqualTo(1m.Eur()));
	}

	[Test]
	public void Ctor_ATinyAmountMoneyNotAllocated_QuasiComplete()
	{
		var subject = new Allocation(10m.Usd(), new[] { 9.99m.Usd(), .0001m.Usd() });

		Assert.That(subject.IsComplete, Is.False);
		Assert.That(subject.IsQuasiComplete, Is.True);
		Assert.That(subject.TotalAllocated, Is.EqualTo(9.9901m.Usd()));
		Assert.That(subject.Remainder, Is.EqualTo(.0099m.Usd()));
	}

	[Test]
	public void Ctor_Debt_CanBeFullyAllocated()
	{
		var subject = new Allocation(-10m.Usd(), new[] { -3m.Usd(), -7m.Usd() });

		Assert.That(subject.TotalAllocated, Is.EqualTo(-10m.Usd()));
		Assert.That(subject.Remainder, Is.EqualTo(Money.Zero(CurrencyIsoCode.USD)));
		Assert.That(subject.IsComplete, Is.True);
		Assert.That(subject.IsQuasiComplete, Is.False);
	}

	[Test]
	public void Ctor_Debt_CanBePartiallyAllocated()
	{
		var subject = new Allocation(-10m.Usd(), new[] { -3m.Usd(), -6m.Usd() });

		Assert.That(subject.TotalAllocated, Is.EqualTo(-9m.Usd()));
		Assert.That(subject.Remainder, Is.EqualTo(-1m.Usd()));
		Assert.That(subject.IsComplete, Is.False);
		Assert.That(subject.IsQuasiComplete, Is.False);
	}

	[Test]
	public void Ctor_Debt_CanBeAlmostFullyAllocated()
	{
		var subject = new Allocation(-10m.Usd(), new[] { -3m.Usd(), -6.999m.Usd() });

		Assert.That(subject.TotalAllocated, Is.EqualTo(-9.999m.Usd()));
		Assert.That(subject.Remainder, Is.EqualTo(-.001m.Usd()));
		Assert.That(subject.IsComplete, Is.False);
		Assert.That(subject.IsQuasiComplete, Is.True);
	}

	[Test]
	public void Ctor_IdentityAllocation_IsForOneRecipient()
	{
		var tenKroner = 10m.Dkk();
		var subject = new Allocation(tenKroner, new[] { tenKroner });

		Assert.That(subject.TotalAllocated, Is.EqualTo(tenKroner));
		Assert.That(subject.Remainder, Is.EqualTo(Money.Zero(CurrencyIsoCode.DKK)));
		Assert.That(subject.IsComplete, Is.True);
		Assert.That(subject.IsQuasiComplete, Is.False);
	}

	#endregion

	#region Zero

	[Test]
	public void Zero_NoAllocation()
	{
		var allocatable = 10.5m.Cad();
		var zeroCads = Money.Zero(CurrencyIsoCode.CAD);
		var zero = Allocation.Zero(allocatable, 3);

		Assert.That(zero.IsComplete, Is.False);
		Assert.That(zero.IsQuasiComplete, Is.False);
		Assert.That(zero.Allocatable, Is.EqualTo(allocatable));
		Assert.That(zero.TotalAllocated, Is.EqualTo(zeroCads));
		Assert.That(zero.Remainder, Is.EqualTo(zero.Allocatable));

		// three zero allocations
		Assert.That(zero.Count, Is.EqualTo(3));
		Assert.That(zero.Remainder, Is.EqualTo(zero.Allocatable));
		Assert.That(zero, Is.EquivalentTo(new[]
		{
			zeroCads,
			zeroCads,
			zeroCads
		}));
	}

	#endregion

	#region ToString

	[Test]
	public void ToString_StandardRecordTypeName()
	{
		var tenKroner = 10m.Dkk();
		var subject = new Allocation(tenKroner, new[] { tenKroner });
		Assert.That(subject.ToString(), Does.Match(@"^Allocation \{ [\s\S]* \}$"));
	}

	[Test]
	public void ToString_SpaceSeparatedPropertyPairs()
	{
		var tenKroner = 10m.Dkk();
		var subject = new Allocation(tenKroner, new[] { tenKroner });
		Assert.That(subject.ToString(), Does.Not.Contain(","));
	}

	[Test]
	public void ToString_ContainsCurrencyCode()
	{
		var tenKroner = 10m.Dkk();
		var subject = new Allocation(tenKroner, new[] { tenKroner });
		Assert.That(subject.ToString(), Does.Contain("CurrencyCode = DKK"));
	}

	[Test]
	public void ToString_AllocatableAmount_FormattedAsPerCurrency()
	{
		var tenCommaFiveKroner = 10.5m.Dkk();
		var dkkSubject = new Allocation(tenCommaFiveKroner, new[] { tenCommaFiveKroner });
		Assert.That(dkkSubject.Allocatable, Is.EqualTo(tenCommaFiveKroner));
		Assert.That(dkkSubject.ToString(), Does.Contain("Allocatable = 10,5"));

		var tenCommaFiveDollars = 10.5m.Usd();
		var usdSubject = new Allocation(tenCommaFiveDollars, new[] { tenCommaFiveDollars });
		Assert.That(usdSubject.Allocatable, Is.EqualTo(tenCommaFiveDollars));
		Assert.That(usdSubject.ToString(), Does.Contain("Allocatable = 10.5"));
	}

	[Test]
	public void ToString_TotalAllocatedAmount_FormattedAsPerCurrency()
	{
		var tenCommaFiveKroner = 10.5m.Dkk();
		var dkkSubject = new Allocation(tenCommaFiveKroner, new[] { tenCommaFiveKroner });
		Assert.That(dkkSubject.TotalAllocated, Is.EqualTo(tenCommaFiveKroner));
		Assert.That(dkkSubject.ToString(), Does.Contain("TotalAllocated = 10,5"));

		var tenCommaFiveDollars = 10.5m.Usd();
		var usdSubject = new Allocation(tenCommaFiveDollars, new[] { tenCommaFiveDollars });
		Assert.That(usdSubject.TotalAllocated, Is.EqualTo(tenCommaFiveDollars));
		Assert.That(usdSubject.ToString(), Does.Contain("TotalAllocated = 10.5"));
	}

	[Test]
	public void ToString_CompleteAllocation_IsCompleteAndNoRemainder()
	{
		Money allocatable = 10m.Usd();
		var allocated = new[] { 5m.Usd(), 5m.Usd() };

		var complete = new Allocation(allocatable, allocated);
		Assert.That(complete.IsComplete, Is.True);
		Assert.That(complete.ToString(), Does.Contain("IsComplete = True").And
			.Not.Contain("Remainder").And
			.Not.Contain("IsQuasiComplete"));
	}

	[Test]
	public void ToString_QuasiCompleteAllocation_IsQuasiCompleteAndFormattedRemainder()
	{
		var quasiComplete = new Allocation(10m.Usd(), new[] { 9.99m.Usd(), .0001m.Usd() });

		Assert.That(quasiComplete.IsQuasiComplete, Is.True);
		Assert.That(quasiComplete.Remainder, Is.EqualTo(.0099m.Usd()));
		Assert.That(quasiComplete.ToString(), Does.Contain("IsQuasiComplete = True").And
			.Contain("Remainder = 0.0099").And
			.Not.Contain("IsComplete"));
	}

	[Test]
	public void ToString_IncompleteAllocation_OnlyFormattedRemainder()
	{
		Money allocatable = 10.3m.Eur();
		var allocated = new[] { 6m.Eur(), 3m.Eur() };
		var incomplete = new Allocation(allocatable, allocated);

		Assert.That(incomplete.IsComplete, Is.False);
		Assert.That(incomplete.IsQuasiComplete, Is.False);
		Assert.That(incomplete.ToString(), Does.Contain("Remainder = 1,3").And
			.Not.Contain("IsQuasiComplete").And
			.Not.Contain("IsComplete"));
	}

	[Test]
	public void ToString_ZeroAllocation_OnlyFormattedRemainder()
	{
		Money allocatable = 10.3m.Eur();
		var zero = Allocation.Zero(allocatable, 2);

		Assert.That(zero.ToString(), Does.Contain("Remainder = 10,3").And
			.Contain("TotalAllocated = 0").And
			.Not.Contain("IsQuasiComplete").And
			.Not.Contain("IsComplete"));
	}

	[Test]
	public void ToString_PipeSeparatedCurrencyFormattedAllocatedMembers()
	{
		Money allocatable = 10m.Usd();
		var allocated = new[] { 5.5m.Usd(), 3.62m.Usd() };

		var complete = new Allocation(allocatable, allocated);

		Assert.That(complete.ToString(), Does.Contain("Allocated = [ 5.5 | 3.62 ]"));
	}

	#endregion
}
