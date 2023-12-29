using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using NMoneys.Support;

namespace NMoneys.Exchange
{
	/// <summary>
	/// Provides a way to obtain exhange rates to be used in conversion operations
	/// </summary>
	public class TabulatedExchangeRateProvider : IExchangeRateProvider
	{
		private readonly Func<CurrencyIsoCode, CurrencyIsoCode, decimal, ExchangeRate> _rateBuilder;

		/// <summary>
		/// Represents a rate to exchange a currency X into another Y and a rate to convert Y into X
		/// </summary>
		public sealed class ExchangeRatePair
		{
			/// <summary>
			/// Creates a <see cref="ExchangeRatePair"/> instance.
			/// </summary>
			/// <param name="direct">Direct conversion from currency X into currency Y.</param>
			/// <param name="inverse">Inverse conversion from currency Y into currency X.</param>
			public ExchangeRatePair(ExchangeRate direct, ExchangeRate inverse)
			{
				AssertConsistentcy(direct, inverse);
				Direct = direct;
				Inverse = inverse;
			}

			/// <summary>
			/// Check the constistency of a pair of rates.
			/// </summary>
			/// <remarks>Two rates as consistent as a pair if the base currency on one is the quote currency of the other and viceversa.</remarks>
			/// <param name="direct">One of the rates to be checked.</param>
			/// <param name="inverse">Another of the rates to be checked.</param>
			/// <exception cref="ArgumentNullException"><paramref name="direct"/> or <paramref name="inverse"/> are null.</exception>
			/// <exception cref="DifferentCurrencyException">The rates are not consistent.</exception>
			public static void AssertConsistentcy(ExchangeRate direct, ExchangeRate inverse)
			{
				Guard.AgainstNullArgument(nameof(direct), direct);
				Guard.AgainstNullArgument(nameof(inverse), inverse);

				if (direct.From != inverse.To) throw new DifferentCurrencyException(direct.From.AlphabeticCode(), inverse.To.AlphabeticCode());
				if (direct.To != inverse.From) throw new DifferentCurrencyException(direct.To.AlphabeticCode(), inverse.From.AlphabeticCode());
			}

			/// <summary>
			/// Conversion from currency X into currency Y.
			/// </summary>
			[Pure]
			public ExchangeRate Direct { get; }

			/// <summary>
			/// Conversion from currency Y into currency X.
			/// </summary>
			[Pure]
			public ExchangeRate Inverse { get; }


			/// <summary>
			/// Converts a rate pair to its equivalent string representation.
			/// </summary>
			/// <returns>The string representation of the value of this instance, consisting of the string representation of the <see cref="Direct"/> rate and the
			/// string representation of the <see cref="Inverse"/> rate.</returns>
			[Pure]
			public override string ToString()
			{
				return Direct + " " + Inverse;
			}
		}

		private readonly Dictionary<CurrencyIsoCode, Dictionary<CurrencyIsoCode, ExchangeRate>> _rows;

		/// <summary>
		/// Creates an instance of <see cref="TabulatedExchangeRateProvider"/> with the standard way to build a <see cref="ExchangeRate"/>.
		/// </summary>
		public TabulatedExchangeRateProvider() : this((from, to, rate)=> new ExchangeRate(from, to, rate)) { }

		/// <summary>
		/// Creates an instance of <see cref="TabulatedExchangeRateProvider"/> with the custom way to build a <see cref="ExchangeRate"/>.
		/// </summary>
		/// <param name="rateBuilder">Custom way of building a <see cref="ExchangeRate"/> implementation.</param>
		public TabulatedExchangeRateProvider(Func<CurrencyIsoCode, CurrencyIsoCode, decimal, ExchangeRate> rateBuilder)
		{
			_rateBuilder = rateBuilder;
			_rows = new Dictionary<CurrencyIsoCode, Dictionary<CurrencyIsoCode, ExchangeRate>>(Currency.Code.Comparer);
		}

		/// <summary>
		/// Adds a rate for its later retrieval.
		/// </summary>
		/// <abstract>It will try to populate its inverse rate if this has not been added before.</abstract>
		/// <param name="from">Base currency, the currency from which the conversion is performed.</param>
		/// <param name="to">Quote currency, the currency which the conversion is performed to.</param>
		/// <param name="rate">A non-negative <see cref="decimal"/> instance representing the relative vaue of <paramref name="from"/> against <paramref name="to"/>.</param>
		/// <returns>The <see cref="ExchangeRate"/> just added as per the rules specified in the constructor.</returns>
		public ExchangeRate Add(CurrencyIsoCode from, CurrencyIsoCode to, decimal rate)
		{
			if (!_rows.TryGetValue(from, out var columns))
			{
				columns = new Dictionary<CurrencyIsoCode, ExchangeRate>(Currency.Code.Comparer);
				_rows.Add(from, columns);
			}
			ExchangeRate added = _rateBuilder(from, to, rate);
			columns[to] = added;
			return added;
		}

		/// <summary>
		/// Adds a rate, its inverse and identity rates for both base and quote currency for its later retrieval.
		/// </summary>
		/// <param name="from">Base currency, the currency from which the conversion is performed.</param>
		/// <param name="to">Quote currency, the currency which the conversion is performed to.</param>
		/// <param name="rate">A non-negative <see cref="decimal"/> instance representing the relative vaue of <paramref name="from"/> against <paramref name="to"/>.</param>
		/// <returns>The <see cref="ExchangeRatePair"/> just added as per the rules specified in the constructor.</returns>
		public ExchangeRatePair MultiAdd(CurrencyIsoCode from, CurrencyIsoCode to, decimal rate)
		{
			Add(to, to, 1m);
			Add(from, from, 1m);

			ExchangeRate direct = Add(from, to, rate);
			ExchangeRate inverse = Add(to, from, direct.Invert().Rate);
			return new ExchangeRatePair(direct, inverse);
		}

		/// <summary>
		/// Provides an applicable rate for exchange operations.
		/// </summary>
		/// <param name="from">Base currency, the currency from which the conversion is performed.</param>
		/// <param name="to">Quote currency, the currency which the conversion is performed to.</param>
		/// <returns>A rate at which one currency will be exchanged for another.</returns>
		/// <exception cref="KeyNotFoundException">A rate converting <paramref name="from"/> into <paramref name="to"/> could not be provided.</exception>
		[Pure]
		public ExchangeRate Get(CurrencyIsoCode from, CurrencyIsoCode to)
		{
			return _rows[from][to];
		}

		/// <summary>
		/// Provides an applicable rate for exchange operations.
		/// </summary>
		/// <param name="from">Base currency, the currency from which the conversion is performed.</param>
		/// <param name="to">Quote currency, the currency which the conversion is performed to.</param>
		/// <param name="rate">A rate at which one currency will be exchanged for another or null if one cannot be provided.</param>
		/// <returns>true if an applicable rate can be provided; otherwise, false.</returns>
		[Pure]
		public bool TryGet(CurrencyIsoCode from, CurrencyIsoCode to, out ExchangeRate rate)
		{
			bool isThere = false;
			rate = null;
			if (_rows.TryGetValue(from, out var colum))
			{
				isThere = colum.TryGetValue(to, out rate);
			}
			return isThere;
		}
	}
}
