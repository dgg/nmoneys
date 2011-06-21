using System;
using System.Collections.Generic;
using NMoneys.Support;

namespace NMoneys.Exchange
{
	public class TabulatedExchangeRateProvider : IExchangeRateProvider
	{
		private readonly Func<CurrencyIsoCode, CurrencyIsoCode, decimal, ExchangeRate> _rateBuilder;

		public sealed class ExchangeRatePair
		{
			public ExchangeRatePair(ExchangeRate direct, ExchangeRate inverse)
			{
				Direct = direct;
				Inverse = inverse;
			}

			public ExchangeRate Direct { get; private set; }
			public ExchangeRate Inverse { get; private set; }
		}

		private readonly Dictionary<CurrencyIsoCode, Dictionary<CurrencyIsoCode, ExchangeRate>> _rows;
		public TabulatedExchangeRateProvider() : this((from, to, rate)=> new ExchangeRate(from, to, rate)) { }

		public TabulatedExchangeRateProvider(Func<CurrencyIsoCode, CurrencyIsoCode, decimal, ExchangeRate> rateBuilder)
		{
			_rateBuilder = rateBuilder;
			_rows = new Dictionary<CurrencyIsoCode, Dictionary<CurrencyIsoCode, ExchangeRate>>(FastEnumComparer<CurrencyIsoCode>.Instance);
		}

		public ExchangeRate Add(CurrencyIsoCode from, CurrencyIsoCode to, decimal rate)
		{
			Dictionary<CurrencyIsoCode, ExchangeRate> columns;
			if (!_rows.TryGetValue(from, out columns))
			{
				columns = new Dictionary<CurrencyIsoCode, ExchangeRate>(FastEnumComparer<CurrencyIsoCode>.Instance);
				_rows.Add(from, columns);
			}
			ExchangeRate added = _rateBuilder(from, to, rate);
			columns[to] = added;
			return added;
		}

		public ExchangeRatePair MultiAdd(CurrencyIsoCode from, CurrencyIsoCode to, decimal rate)
		{
			Add(to, to, 1m);
			Add(from, from, 1m);

			ExchangeRate direct = Add(from, to, rate);
			ExchangeRate inverse = Add(to, from, direct.Invert().Rate);
			return new ExchangeRatePair(direct, inverse);
		}

		public ExchangeRate Get(CurrencyIsoCode from, CurrencyIsoCode to)
		{
			return _rows[from][to];
		}

		public bool TryGet(CurrencyIsoCode from, CurrencyIsoCode to, out ExchangeRate rate)
		{
			bool isThere = false;
			rate = null;
			Dictionary<CurrencyIsoCode, ExchangeRate> colum;
			if (_rows.TryGetValue(from, out colum))
			{
				isThere = colum.TryGetValue(to, out rate);
			}
			return isThere;
		}
	}
}
