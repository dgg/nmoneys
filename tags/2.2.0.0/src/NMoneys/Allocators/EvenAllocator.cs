using System;
using NMoneys.Extensions;

namespace NMoneys.Allocators
{
	internal class EvenAllocator
	{
		private readonly decimal _toAllocate;
		private readonly Currency _currency;

		public EvenAllocator(Money toAllocate)
		{
			_toAllocate = toAllocate.Amount;
			_currency = toAllocate.Currency();
		}

		public decimal[] Allocate(int numberOfRecipients, out Money allocated)
		{
			var results = new decimal[numberOfRecipients];
			allocated = Money.Zero(_currency);

			for (var i = 0; i < numberOfRecipients; i++)
			{
				var each = _toAllocate / numberOfRecipients;

				each = Math.Round(each - (0.5M * _currency.MinAmount), _currency.SignificantDecimalDigits, MidpointRounding.AwayFromZero);
				results[i] = each;
				allocated += new Money(results[i], _currency);
			}
			return results;
		}
	}
}
