using System;
using NMoneys.Extensions;

namespace NMoneys.Allocation
{
	public class EvenAllocator
	{
		private readonly Money _toAllocate;
		private readonly Currency _currency;

		public EvenAllocator(Money toAllocate)
		{
			_toAllocate = toAllocate;
			_currency = toAllocate.GetCurrency();
		}

		public Money[] Allocate(int numberOfRecipients, out Money allocated)
		{
			var results = new Money[numberOfRecipients];
			allocated = Money.Zero(_currency);
			var each = _toAllocate.Amount / numberOfRecipients;
			each = Math.Round(each - (0.5M * _currency.MinAmount), _currency.SignificantDecimalDigits, MidpointRounding.AwayFromZero);

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
	}
}
