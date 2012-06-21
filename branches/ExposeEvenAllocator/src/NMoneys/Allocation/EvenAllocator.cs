using System;
using NMoneys.Extensions;
using NMoneys.Support;

namespace NMoneys.Allocation
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
		/// <param name="allocated">The sum of money evenly allocated.
		/// <para>In the case of an even allocation, that sum would equal the quantity passed to the constructor <see cref="EvenAllocator(Money)"/>.</para>
		/// <para>In the case of an uneven allocation, that sum would be less than the quantity amount passed to the constructor <see cref="EvenAllocator(Money)"/>.</para>
		/// </param>
		/// <returns>The results of the even allocation as an array with a length equal to <paramref name="numberOfRecipients"/>.</returns>
		public Money[] Allocate(int numberOfRecipients, out Money allocated)
		{
			Money[] results = Money.Zero(_currency, numberOfRecipients);
			allocated = Money.Zero(_currency);
			decimal each = _toAllocate.Amount / numberOfRecipients;
			each = _currency.Round(each);

			// if amount to allocate is too 'scarce' to allocate something to all
			// then effectively go into remainder allocation mode
			var notEnough = (numberOfRecipients * (_toAllocate.MinValue.Amount)) > _toAllocate.Amount;
			if (notEnough) return results;

			for (var i = 0; i < numberOfRecipients; i++)
			{
				results[i] = new Money(each, _currency);
				allocated += results[i];
			}
			return results;
		}

		public Allocation Allocate(int numberOfRecipients)
		{
			Money[] results = Money.Zero(_currency, numberOfRecipients);
			decimal each = _toAllocate.Amount / numberOfRecipients;
			each = _currency.Round(each);

			// if amount to allocate is too 'scarce' to allocate something to all
			// then effectively go into remainder allocation mode
			var notEnough = (numberOfRecipients * (_toAllocate.MinValue.Amount)) > _toAllocate.Amount;
			if (notEnough) return Allocation.Zero(_toAllocate, numberOfRecipients);

			for (var i = 0; i < numberOfRecipients; i++)
			{
				results[i] = new Money(each, _currency);
			}
			return new Allocation(_toAllocate, results);
		}

		private static readonly Range<int> _validityRange = new Range<int>(1.Close(), int.MaxValue.Close());
		internal static void AssertNumberOfRecipients(string paramName, int numberOfRecipients)
		{
			_validityRange.AssertArgument(paramName, numberOfRecipients);
		}
	}
}
