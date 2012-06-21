using NMoneys.Extensions;
using NMoneys.Support;

namespace NMoneys.Allocations
{
	/// <summary>
	/// Allows allocating an instance of <see cref="Money"/> evenly.
	/// </summary>
	public class EvenAllocator
	{
		private readonly Money _toAllocate;
		private readonly Currency _currency;

		public EvenAllocator(Money toAllocate)
		{
			_toAllocate = toAllocate;
			_currency = toAllocate.GetCurrency();
		}

		/// <summary>
		/// Allocates the sum of money 'fairly', discarding the remainder of an uneven allocation.
		/// </summary>
		/// <remarks>
		/// <para>
		/// A sum of money that can be allocated to each recipient exactly evenly is inherently 'fair'. For example, a US
		/// Dollar split four (4) ways leaves each recipient with 25 cents and everything is allocated.</para>
		/// <para>
		/// A US Dollar split three (3) ways cannot be distributed evenly and is therefore inherently 'unfair'. What will be done
		/// is allocate the maximum fair amount and leave the remainding amount for the caller to decide.</para>
		/// </remarks>
		/// <param name="numberOfRecipients">The number of times to split up the total.</param>
		/// <returns>
		/// The results of the even allocation with a length equal to <paramref name="numberOfRecipients"/>.
		/// <para>In the case of an even allocation, the allocation will be complete.<see cref="Allocation.IsComplete"/>, having a zero <see cref="Allocation.Remainder"/>.</para>
		/// <para>In the case of an uneven allocation, the allocation will not be complete <see cref="EvenAllocator(Money)"/>, having a non-zero <see cref="Allocation.Remainder"/>.</para>
		/// </returns>
		/// <seealso cref="EvenAllocator(Money)"/>
		public Allocation Allocate(int numberOfRecipients)
		{
			// if amount to allocate is too 'scarce' to allocate something to all
			// then effectively go into remainder allocation mode
			if (notEnoughToAllocateEvenly(numberOfRecipients)) return Allocation.Zero(_toAllocate, numberOfRecipients);

			decimal each = amountforEachRecipient(numberOfRecipients);
			Money[] results = Money.Some(each, _currency, numberOfRecipients);
			return new Allocation(_toAllocate, results);
		}

		private bool notEnoughToAllocateEvenly(int numberOfRecipients)
		{
			return (numberOfRecipients*(_toAllocate.MinValue.Amount)) > _toAllocate.Amount;
		}

		private decimal amountforEachRecipient(int numberOfRecipients)
		{
			decimal each = _toAllocate.Amount / numberOfRecipients;
			each = _currency.Round(each);
			return each;
		}

		private static readonly Range<int> _validityRange = new Range<int>(1.Close(), int.MaxValue.Close());
		internal static void AssertNumberOfRecipients(string paramName, int numberOfRecipients)
		{
			_validityRange.AssertArgument(paramName, numberOfRecipients);
		}
	}
}
