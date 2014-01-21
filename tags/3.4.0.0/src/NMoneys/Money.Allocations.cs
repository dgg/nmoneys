using System;
using NMoneys.Allocations;
using NMoneys.Extensions;
using NMoneys.Support;

namespace NMoneys
{
	public partial struct Money
	{
		/// <summary>
		/// Allocates the sum of money fully and 'fairly', delegating the distribution to a default allocator
		/// </summary>
		/// <remarks>
		/// The default remainder allocation will be performed according to <see cref="RemainderAllocator.FirstToLast"/>.
		/// </remarks>
		/// <param name="numberOfRecipients">The number of times to split up the total.</param>
		/// <returns>The results of the allocation as an array with a length equal to <paramref name="numberOfRecipients"/>.</returns>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="numberOfRecipients"/> is less than 1 or more than <see cref="int.MaxValue"/>.</exception>
		/// <seealso cref="Allocate(int, IRemainderAllocator)"/>
		/// <seealso cref="Allocation"/>
		public Allocation Allocate(int numberOfRecipients)
		{
			return Allocate(numberOfRecipients, RemainderAllocator.FirstToLast);
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
		/// of the currency represented by <see cref="CurrencyCode"/>.</para>
		/// </remarks>
		/// <param name="numberOfRecipients">The number of times to split up the total.</param>
		/// <param name="allocator">The <see cref="IRemainderAllocator"/> that will distribute the remainder after an even split.</param>
		/// <returns>The results of the allocation with a length equal to <paramref name="numberOfRecipients"/>.</returns>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="numberOfRecipients"/> is less than 1 or more than <see cref="int.MaxValue"/>.</exception>
		/// <seealso cref="IRemainderAllocator"/>
		/// <seealso cref="Allocation"/>
		public Allocation Allocate(int numberOfRecipients, IRemainderAllocator allocator)
		{
			EvenAllocator.AssertNumberOfRecipients("numberOfRecipients", numberOfRecipients);

			if (notEnoughToAllocate()) return Allocation.Zero(this, numberOfRecipients);

			Allocation allocated = new EvenAllocator(this)
				.Allocate(numberOfRecipients);

			allocated = allocateRemainderIfNeeded(allocator, allocated);

			return allocated;
		}

		private bool notEnoughToAllocate()
		{
			var currency = this.GetCurrency();
			decimal minimumToAllocate = currency.MinAmount;
			return (Amount < minimumToAllocate);
		}

		private Allocation allocateRemainderIfNeeded(IRemainderAllocator allocator, Allocation allocatedSoFar)
		{
			Money remainder = this - allocatedSoFar.TotalAllocated;
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
		/// <param name="ratios">The ratio collection.</param>
		/// <param name="allocator">The <see cref="IRemainderAllocator"/> that will distribute the remainder after the split.</param>
		/// <returns>The results of the allocation with a length equal to <paramref name="ratios"/>.</returns>
		/// <seealso cref="Allocation"/>
		/// <seealso cref="IRemainderAllocator"/>
		public Allocation Allocate(RatioCollection ratios, IRemainderAllocator allocator)
		{
			Guard.AgainstNullArgument("ratios", ratios);

			if (notEnoughToAllocate()) return Allocation.Zero(this, ratios.Count);

			Allocation allocated = new ProRataAllocator(this)
				.Allocate(ratios);

			allocated = allocateRemainderIfNeeded(allocator, allocated);

			return allocated;
		}

		/// <summary>
		/// Allocates the sum of money as fully and 'fairly' as possible given the collection of ratios passed.
		/// </summary>
		/// <remarks>
		/// The default remainder allocation will be performed according to <see cref="RemainderAllocator.FirstToLast"/>.
		/// </remarks>
		/// <param name="ratios">The ratio collection.</param>
		/// <returns>The results of the allocation with a length equal to <paramref name="ratios"/>.</returns>
		/// <seealso cref="Allocate(RatioCollection, IRemainderAllocator)"/>
		/// <seealso cref="Allocation"/>
		public Allocation Allocate(RatioCollection ratios)
		{
			return Allocate(ratios, RemainderAllocator.FirstToLast);
		}
	}
}
