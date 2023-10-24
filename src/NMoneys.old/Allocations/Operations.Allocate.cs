using System;
using System.Diagnostics.Contracts;
using NMoneys.Extensions;
using NMoneys.Support;

namespace NMoneys.Allocations
{
	/// <summary>
	/// Extension class that gives access to extensions methods related to money allocation.
	/// </summary>
	public static class AllocateOperations
	{
		/// <summary>
		/// Allocates the sum of money fully and 'fairly', delegating the distribution to a default allocator
		/// </summary>
		/// <remarks>
		/// The default remainder allocation will be performed according to <see cref="RemainderAllocator.FirstToLast"/>.
		/// </remarks>
		/// <param name="money">The monetary quantity to distribute.</param>
		/// <param name="numberOfRecipients">The number of times to split up the total.</param>
		/// <returns>The results of the allocation as an array with a length equal to <paramref name="numberOfRecipients"/>.</returns>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="numberOfRecipients"/> is less than 1 or more than <see cref="int.MaxValue"/>.</exception>
		/// <seealso cref="Allocate(Money, int, IRemainderAllocator)"/>
		/// <seealso cref="Allocation"/>
		[Pure]
		public static Allocation Allocate(this Money money, int numberOfRecipients)
		{
			return money.Allocate(numberOfRecipients, RemainderAllocator.FirstToLast);
		}

		/// <summary>
		/// Allocates the sum of money fully and 'fairly', delegating the distribution of whichever remainder after
		/// allocating the highest fair amount amongst the recipients to the provided <paramref name="allocator"/>.
		/// </summary>
		/// <remarks>
		/// <para>
		/// A sum of money that can be allocated to each recipient exactly evenly is inherently 'fair'. For example, a US
		/// Dollar split four (4) ways leaves each recipient with 25 cents.</para>
		/// <para>
		/// A US Dollar split three (3) ways cannot be distributed evenly and is therefore inherently 'unfair'. The
		/// best we can do is minimize the amount of the remainder (in this case a cent) and allocate it in a way
		/// that seems random and thus fair to the recipients.</para>
		/// <para>The precision to use for rounding will be the <see cref="Currency.SignificantDecimalDigits"/> 
		/// of the currency represented by <see cref="Money.CurrencyCode"/>.</para>
		/// </remarks>
		/// <param name="money">The monetary quantity to distribute.</param>
		/// <param name="numberOfRecipients">The number of times to split up the total.</param>
		/// <param name="allocator">The <see cref="IRemainderAllocator"/> that will distribute the remainder after an even split.</param>
		/// <returns>The results of the allocation with a length equal to <paramref name="numberOfRecipients"/>.</returns>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="numberOfRecipients"/> is less than 1 or more than <see cref="int.MaxValue"/>.</exception>
		/// <seealso cref="IRemainderAllocator"/>
		/// <seealso cref="Allocation"/>
		[Pure]
		public static Allocation Allocate(this Money money, int numberOfRecipients, IRemainderAllocator allocator)
		{
			EvenAllocator.AssertNumberOfRecipients(nameof(numberOfRecipients), numberOfRecipients);

			if (money.notEnoughToAllocate()) return Allocation.Zero(money, numberOfRecipients);

			Allocation allocated = new EvenAllocator(money)
				.Allocate(numberOfRecipients);

			allocated = money.allocateRemainderIfNeeded(allocator, allocated);

			return allocated;
		}

		private static bool notEnoughToAllocate(this Money money)
		{
			var currency = money.GetCurrency();
			decimal minimumToAllocate = currency.MinAmount;
			return money.Amount < minimumToAllocate;
		}

		private static Allocation allocateRemainderIfNeeded(this Money money, IRemainderAllocator allocator, Allocation allocatedSoFar)
		{
			Money remainder = money - allocatedSoFar.TotalAllocated;
			Allocation beingAllocated = allocatedSoFar;
			if (remainder >= remainder.MinValue)
			{
				beingAllocated = allocator.Allocate(allocatedSoFar);
			}
			return beingAllocated;
		}

		/// <summary>
		/// Allocates the sum of money as fully and 'fairly' as possible given the collection of ratios passed.
		/// </summary>
		/// <param name="money">The monetary quantity to distribute.</param>
		/// <param name="ratios">The ratio collection.</param>
		/// <param name="allocator">The <see cref="IRemainderAllocator"/> that will distribute the remainder after the split.</param>
		/// <returns>The results of the allocation with a length equal to <paramref name="ratios"/>.</returns>
		/// <seealso cref="Allocation"/>
		/// <seealso cref="IRemainderAllocator"/>
		[Pure]
		public static Allocation Allocate(this Money money, RatioCollection ratios, IRemainderAllocator allocator)
		{
			Guard.AgainstNullArgument(nameof(ratios), ratios);

			if (money.notEnoughToAllocate()) return Allocation.Zero(money, ratios.Count);

			Allocation allocated = new ProRataAllocator(money)
				.Allocate(ratios);

			allocated = money.allocateRemainderIfNeeded(allocator, allocated);

			return allocated;
		}

		/// <summary>
		/// Allocates the sum of money as fully and 'fairly' as possible given the collection of ratios passed.
		/// </summary>
		/// <remarks>
		/// The default remainder allocation will be performed according to <see cref="RemainderAllocator.FirstToLast"/>.
		/// </remarks>
		/// <param name="money">The monetary quantity to distribute.</param>
		/// <param name="ratios">The ratio collection.</param>
		/// <returns>The results of the allocation with a length equal to <paramref name="ratios"/>.</returns>
		/// <seealso cref="Allocate(Money, RatioCollection, IRemainderAllocator)"/>
		/// <seealso cref="Allocation"/>
		[Pure]
		public static Allocation Allocate(this Money money, RatioCollection ratios)
		{
			return money.Allocate(ratios, RemainderAllocator.FirstToLast);
		}
	}
}
