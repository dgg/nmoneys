using NMoneys.Extensions;

namespace NMoneys.Allocation
{
	internal class ProRataAllocator
	{
		private readonly Money _toAllocate;
		private readonly Currency _currency;

		public ProRataAllocator(Money toAllocate)
		{
			_toAllocate = toAllocate;
			_currency = toAllocate.GetCurrency();
		}

		public Money[] Allocate(RatioBag ratios, out Money allocated)
		{
			var results = initResults(ratios.Count);
			allocated = Money.Zero(_currency);

			for (var i = 0; i < ratios.Count; i++)
			{
				var share = ratios[i].ApplyTo(_toAllocate.Amount);
				share = _currency.Round(share);
				results[i] = new Money(share, _currency);
				allocated += results[i];
			}
			return results;
		}

		public Allocation Allocate(RatioBag ratios)
		{
			Money[] results = Money.Zero(_currency, ratios.Count);
			//allocated = Money.Zero(_currency);

			for (var i = 0; i < ratios.Count; i++)
			{
				var share = ratios[i].ApplyTo(_toAllocate.Amount);
				share = _currency.Round(share);
				results[i] = new Money(share, _currency);
			}
			return new Allocation(_toAllocate, results);
		}

		private Money[] initResults(int numberOfRecipients)
		{
			var results = new Money[numberOfRecipients];
			for (int i = 0; i < results.Length; i++)
			{
				results[i] = Money.Zero(_currency);
			}
			return results;
		}

		protected virtual decimal GetRoundedShare(decimal share, Currency currency)
		{
			return 0m;// return MoneyRounder.CurrencyRoundedShare(share, currency);
		}
	}
}
