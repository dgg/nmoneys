using System;
using NMoneys.Allocations;
using NMoneys.Extensions;
using NUnit.Framework;

namespace NMoneys.Tests
{
	[TestFixture]
	public partial class MoneyTester
	{
		[Test]
		public void Allocate_NullProRata_Exception()
		{
			Assert.That(() => 1m.Usd().Allocate(null), Throws.InstanceOf<ArgumentNullException>());
			Assert.That(() => 1m.Usd().Allocate(null, new Support.RemainderAllocatorSpy()), Throws.InstanceOf<ArgumentNullException>());
		}

		#region scenarios where the sum to allocate can be split exactly even.

		[Test]
		public void Allocate_ProRated_OneZeroSplit()
		{
			var easyEven = 10m.Usd();

			var ratios = new RatioCollection(1m, 0m);
			Allocation allocated = easyEven.Allocate(ratios);
			Assert.That(allocated, Is.EqualTo(new[]
			{
				10m.Usd(),
				0m.Usd()
			}));
		}

		[Test]
		public void Allocate_ProRated_EasyEvenSplit()
		{
			var easyEven = 10m.Usd();

			var ratios = new RatioCollection(.5m, .2m, .3m);
			Allocation allocated = easyEven.Allocate(ratios);
			Assert.That(allocated, Is.EqualTo(new[]
			{
				5m.Usd(),
				2m.Usd(),
				3m.Usd()
			}));
		}

		[Test]
		public void Allocate_ProRated_BigEvenSplit()
		{
			var bigEven = 6000000m.Usd();

			var ratios = new RatioCollection(.5725m, .4275m);
			Allocation allocated = bigEven.Allocate(ratios);
			Assert.That(allocated, Is.EqualTo(new[]
			{
				3435000m.Usd(),
				2565000m.Usd()
			}));
		}

		[Test]
		public void Allocate_ProRated_StillEasyEvenSplit()
		{
			var stillEasy = 100m.Usd();

			var ratios = new RatioCollection(.412m, .495m, .093m);
			Allocation allocated = stillEasy.Allocate(ratios);
			Assert.That(allocated, Is.EqualTo(new[]
			{
				41.2m.Usd(),
				49.5m.Usd(),
				9.3m.Usd()
			}));
		}

		[Test]
		public void Allocate_ProRated_StillEasyHightToLowSplit()
		{
			var stillEasy = 100m.Usd();

			var ratios = new RatioCollection(.495m, .412m, .093m);
			var allocated = stillEasy.Allocate(ratios);
			Assert.That(allocated, Is.EqualTo(new[]
			{
				49.5m.Usd(),
				41.2m.Usd(),
				9.3m.Usd()
			}));
		}

		#endregion

		#region scenarios where there is not enough of the sum to allocate to go around (default remainder allocator)

		[Test]
		public void Allocate_ProRated_ScarceResources()
		{
			var scarce = .05m.Usd();
			var ratios = new RatioCollection(.412m, .093m, .495m);

			Allocation allocated = scarce.Allocate(ratios);
			Assert.That(allocated, Is.EqualTo(new[]
			{
				.03m.Usd(),
				.0m.Usd(),
				.02m.Usd()
			}));
		}

		[Test]
		public void Allocate_ProRated_ScarceResources_LowToHigh()
		{
			var scarce = .05m.Usd();
			var ratios = new RatioCollection(.093m, .412m, .495m);

			Allocation allocated = scarce.Allocate(ratios);
			Assert.That(allocated, Is.EqualTo(new[]
			{
				.01m.Usd(),
				.02m.Usd(),
				.02m.Usd()
			}));
		}

		[Test]
		public void Allocate_ProRated_DefaultRemainder_LowToHighScarceResources()
		{
			var scarce = .05m.Usd();
			var ratios = new RatioCollection(.3m, .7m);

			var allocated = scarce.Allocate(ratios);
			Assert.That(allocated, Is.EqualTo(new[]
			{
				.02m.Usd(),
				.03m.Usd()
			}));
		}

		[Test]
		public void Allocate_ProRated_DefaultRemainder_HighToLowScarceResources()
		{
			var scarce = .05m.Usd();
			var ratios = new RatioCollection(.7m, .3m);

			var allocated = scarce.Allocate(ratios);
			Assert.That(allocated, Is.EqualTo(new[]
			{
				.04m.Usd(),
				.01m.Usd()
			}));
		}
		
		#endregion

		#region scenarios where there is not enough of the sum to allocate to go around (specific remainder allocator)

		[Test]
		public void Allocate_ProRated_FirstToLast_LowToHighScarceResources()
		{
			var scarce = .05m.Usd();
			var ratios = new RatioCollection(.3m, .7m);

			var allocated = scarce.Allocate(ratios, RemainderAllocator.FirstToLast);
			Assert.That(allocated, Is.EqualTo(new[]
			{
				.02m.Usd(),
				.03m.Usd()
			}));
		}

		[Test]
		public void Allocate_ProRated_LastToFirst_LowToHighScarceResources()
		{
			var scarce = .05m.Usd();
			var ratios = new RatioCollection(.3m, .7m);

			var allocated = scarce.Allocate(ratios, RemainderAllocator.LastToFirst);
			Assert.That(allocated, Is.EqualTo(new[]
			{
				.01m.Usd(),
				.04m.Usd()
			}));
		}

		[Test]
		public void Allocate_ProRated_FirstToLast_HighToLowScarceResources()
		{
			var scarce = .05m.Usd();
			var ratios = new RatioCollection(.7m, .3m);

			var allocated = scarce.Allocate(ratios, RemainderAllocator.FirstToLast);
			Assert.That(allocated, Is.EqualTo(new[]
			{
				.04m.Usd(),
				.01m.Usd()
			}));
		}

		[Test]
		public void Allocate_ProRated_FirstToLast_HighToLowNotSoScarceResources()
		{
			var notSoScarce = 17m.Usd();
			var ratios = new RatioCollection(.412m, .093m, .495m);

			var allocated = notSoScarce.Allocate(ratios, RemainderAllocator.FirstToLast);
			Assert.That(allocated, Is.EqualTo(new[]
			{
				7.01m.Usd(),
				1.58m.Usd(),
				8.41m.Usd()
			}));
		}

		#endregion

		[Test]
		public void Allocate_ProRated_NotEvenTheMinimumForEveryone_QuasiCompleteAllocation()
		{
			var notAllocatable = 0.001m.Usd();

			var result = notAllocatable.Allocate(new RatioCollection(.7m, .3m), RemainderAllocator.FirstToLast);

			Assert.That(result.IsComplete, Is.False);
			Assert.That(result.IsQuasiComplete, Is.True);
			Assert.That(result.TotalAllocated, Is.EqualTo(Money.Zero(CurrencyIsoCode.USD)));
		}

		[Test]
		public void Allocate_ProRated_ResidualRemainders_QuasiComplete()
		{
			var residualRemainder = 100.001m.Usd();

			var result = residualRemainder.Allocate(new RatioCollection(.7m, .3m), RemainderAllocator.FirstToLast);
			Assert.That(result.IsComplete, Is.False);
			Assert.That(result.IsQuasiComplete, Is.True);
			Assert.That(result.TotalAllocated, Is.EqualTo(100m.Usd()));
		}
	}
}
