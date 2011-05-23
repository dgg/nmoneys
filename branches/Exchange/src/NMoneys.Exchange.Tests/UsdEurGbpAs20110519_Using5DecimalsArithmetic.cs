using System.Collections.Generic;

namespace NMoneys.Exchange.Tests
{
	internal class UsdEurGbpAs20110519_Using5DecimalsArithmetic
	{
		private readonly IDictionary<CurrencyIsoCode, FiveDecimalsArithmetic> _usdBase, _eurBase, _gbpBase;
		public UsdEurGbpAs20110519_Using5DecimalsArithmetic()
		{
			_usdBase = new Dictionary<CurrencyIsoCode, FiveDecimalsArithmetic>(3)
			{
				{CurrencyIsoCode.USD, new FiveDecimalsArithmetic(CurrencyIsoCode.USD, CurrencyIsoCode.USD, 1m)},
				{CurrencyIsoCode.EUR, new FiveDecimalsArithmetic(CurrencyIsoCode.USD, CurrencyIsoCode.EUR, 0.70155m)},
				{CurrencyIsoCode.GBP, new FiveDecimalsArithmetic(CurrencyIsoCode.USD, CurrencyIsoCode.GBP, 0.61860m)}
			};

			_eurBase = new Dictionary<CurrencyIsoCode, FiveDecimalsArithmetic>(3)
			{
				{CurrencyIsoCode.EUR, new FiveDecimalsArithmetic(CurrencyIsoCode.EUR, CurrencyIsoCode.EUR, 1m)},
				{CurrencyIsoCode.USD, new FiveDecimalsArithmetic(CurrencyIsoCode.EUR, CurrencyIsoCode.USD, 1.42542m)},
				{CurrencyIsoCode.GBP, new FiveDecimalsArithmetic(CurrencyIsoCode.EUR, CurrencyIsoCode.GBP, 0.88176m)}
			};

			_gbpBase = new Dictionary<CurrencyIsoCode, FiveDecimalsArithmetic>(3)
			{
				{CurrencyIsoCode.GBP, new FiveDecimalsArithmetic(CurrencyIsoCode.GBP, CurrencyIsoCode.GBP, 1m)},
				{CurrencyIsoCode.EUR, new FiveDecimalsArithmetic(CurrencyIsoCode.GBP, CurrencyIsoCode.EUR, 1.13409m)},
				{CurrencyIsoCode.USD, new FiveDecimalsArithmetic(CurrencyIsoCode.GBP, CurrencyIsoCode.USD, 1.61656m)}
			};
		}

		public ExchangeRate Get(CurrencyIsoCode from, CurrencyIsoCode to)
		{
			switch (from)
			{
				case CurrencyIsoCode.EUR:
					return _eurBase[to];
				case CurrencyIsoCode.USD:
					return _usdBase[to];
				case CurrencyIsoCode.GBP:
					return _gbpBase[to];
				default:
					throw new KeyNotFoundException("Only conversions from/to EUR-GBP-USD allowed");
			}
		}

		public bool TryGet(CurrencyIsoCode from, CurrencyIsoCode to, out ExchangeRate rate)
		{
			bool found = false;
			FiveDecimalsArithmetic value = null;
			switch (from)
			{
				case CurrencyIsoCode.EUR:
					found = _eurBase.TryGetValue(to, out value);
					break;
				case CurrencyIsoCode.USD:
					found = _usdBase.TryGetValue(to, out value);
					break;
				case CurrencyIsoCode.GBP:
					found = _gbpBase.TryGetValue(to, out value);
					break;
			}
			rate = value;
			return found;
		}
	}

	public class FiveDecimalsArithmetic : ExchangeRate
	{
		public FiveDecimalsArithmetic(CurrencyIsoCode from, CurrencyIsoCode to, decimal rate)
			: base(from, to, rate) { }

		public override ExchangeRate Inverse()
		{
			return new FiveDecimalsArithmetic(To, From, decimal.Round(1m / Rate, 5));
		}

		public override Money Apply(Money from)
		{
			Money applied = base.Apply(from);
			return applied.Round(5);
		}
	}
}
