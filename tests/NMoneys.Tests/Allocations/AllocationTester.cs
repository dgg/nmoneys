using NMoneys.Allocations;
using NMoneys.Extensions;

namespace NMoneys.Tests.Allocations;

[TestFixture]
public class AllocationTester
{
	[Test]
		public void Ctor_AllocationOfDisparateCurrencies_Exception()
		{
			Money allocatableEur = 1m.Eur();
			var allocatedCad = new[] { .5m.Cad(), .5m.Cad() };
			Assert.That(() => new Allocation(allocatableEur, allocatedCad), Throws.InstanceOf<DifferentCurrencyException>());
		}

		[Test]
		public void Ctor_AllocatedQuantitiesWithDifferentCurrencies_Exception()
		{
			Money allocatableEur = 1m.Eur();
			var someAreNotEur = new[] { .5m.Eur(), .25m.Cad(), .5m.Eur() };
			Assert.That(() => new Allocation(allocatableEur, someAreNotEur), Throws.InstanceOf<DifferentCurrencyException>());
		}

		[Test]
		public void Ctor_AllocatedMoreThanWasPossible_Exception()
		{
			Money allocatable = 1m.Dkk();
			var allocated = new[] { .75m.Dkk(), .75m.Dkk() };

			Assert.That(() => new Allocation(allocatable, allocated), Throws.InstanceOf<ArgumentOutOfRangeException>());

		}

		[TestCaseSource(nameof(allocationResults))]
		public void Ctor_AllocatedIsWrappedIntoAnEnumerable(Money moreThanAllocated, Money[] allocated)
		{
			var subject = new Allocation(moreThanAllocated, allocated);

			Assert.That(subject, Is.EqualTo(allocated));
		}

		[TestCaseSource(nameof(allocationResults))]
		public void Ctor_AllocatedIsWrappedIntoAnIndexable(Money moreThanAllocated, Money[] allocated)
		{
			var subject = new Allocation(moreThanAllocated, allocated);

			Assert.That(subject, Has.Length.EqualTo(allocated.Length));

			for (int i = 0; i < allocated.Length; i++)
			{
				Assert.That(subject[i], Is.EqualTo(allocated[i]));
			}
		}

		private static readonly IEnumerable<TestCaseData> allocationResults = new[]
		{
			new TestCaseData(10m.Dkk(), new[]{8m.Dkk(), 2m.Dkk()}).SetName("All money allocated"),
			new TestCaseData(10m.Dkk(), new[]{6m.Dkk(), 2m.Dkk()}).SetName("Some money not allocated"),
			new TestCaseData(10m.Dkk(), new[]{9.99m.Dkk(), .001m.Dkk()}).SetName("Some marginal money not allocated")
		};

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
}
