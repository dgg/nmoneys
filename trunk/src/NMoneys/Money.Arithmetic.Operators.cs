using System;
using System.Collections.Generic;
using System.Linq;
using NMoneys.Support;

namespace NMoneys
{
	public partial struct Money
	{
		/// <summary>
		/// Negates the value of the specified <see cref="Money"/> operand.
		/// </summary>
		/// <param name="money">The value to negate.</param>
		/// <returns>A <see cref="Money"/> with the <see cref="Amount"/> of <paramref name="money"/>, but multiplied by negative one (-1).</returns>
		public static Money operator -(Money money)
		{
			return new Money(-(money.Amount), money.CurrencyCode);
		}

		/// <summary>
		/// Adds two specified <see cref="Money"/> values.
		/// </summary>
		/// <remarks>Both instances must have the same <see cref="CurrencyCode"/> in order to be added, otherwise an exception will be thrown.</remarks>
		/// <param name="first">The first value to add.</param>
		/// <param name="second">The second value to add.</param>
		/// <returns>A <see cref="Money"/> with <see cref="Amount"/> as the sum of <paramref name="first"/> and <paramref name="second"/> amounts
		/// and the same <see cref="CurrencyCode"/> as any of the arguments.</returns>
		/// <exception cref="DifferentCurrencyException">If <paramref name="first"/> does not have the same <see cref="CurrencyCode"/>
		/// as <paramref name="second"/>.</exception>
		/// <exception cref="OverflowException">The <see cref="Amount"/> of the result is less than
		/// <see cref="decimal.MinValue"/> or greater than <see cref="decimal.MaxValue"/>.</exception>
		public static Money operator +(Money first, Money second)
		{
			first.AssertSameCurrency(second);
			return new Money(first.Amount + second.Amount, first.CurrencyCode);
		}

		/// <summary>
		/// Adds two specified <see cref="Money"/> values.
		/// </summary>
		/// <remarks>Both instances must have the same <see cref="CurrencyCode"/> in order to be added, otherwise an exception will be thrown.</remarks>
		/// <param name="first">The first value to add.</param>
		/// <param name="second">The second value to add.</param>
		/// <returns>A <see cref="Money"/> with <see cref="Amount"/> as the sum of <paramref name="first"/> and <paramref name="second"/> amounts
		/// and the same <see cref="CurrencyCode"/> as any of the arguments.</returns>
		/// <exception cref="DifferentCurrencyException">If <paramref name="first"/> does not have the same <see cref="CurrencyCode"/>
		/// as <paramref name="second"/>.</exception>
		/// <exception cref="OverflowException">The <see cref="Amount"/> of the result is less than
		/// <see cref="decimal.MinValue"/> or greater than <see cref="decimal.MaxValue"/>.</exception>
		public static Money Add(Money first, Money second)
		{
			return first + second;
		}

		/// <summary>
		/// Substracts one specified <see cref="Money"/> from another.
		/// </summary>
		/// <remarks>Both instances must have the same <see cref="CurrencyCode"/> in order to be substracted, otherwise an exception will be thrown.</remarks>
		/// <param name="first">The minuend.</param>
		/// <param name="second">The subtrahend.</param>
		/// <returns>A <see cref="Money"/> with <see cref="Amount"/> as the result of substracting <paramref name="second"/> from <paramref name="first"/> amounts
		/// and the same <see cref="CurrencyCode"/> as any of the arguments.</returns>
		/// <exception cref="DifferentCurrencyException">If <paramref name="first"/> does not have the same <see cref="CurrencyCode"/>
		/// as <paramref name="second"/>.</exception>
		/// <exception cref="OverflowException">The <see cref="Amount"/> of the result is less than
		/// <see cref="decimal.MinValue"/> or greater than <see cref="decimal.MaxValue"/>.</exception>
		public static Money operator -(Money first, Money second)
		{
			first.AssertSameCurrency(second);
			return new Money(first.Amount - second.Amount, first.CurrencyCode);
		}

		/// <summary>
		/// Substracts one specified <see cref="Money"/> from another.
		/// </summary>
		/// <remarks>Both instances must have the same <see cref="CurrencyCode"/> in order to be substracted, otherwise an exception will be thrown.</remarks>
		/// <param name="first">The minuend.</param>
		/// <param name="second">The subtrahend.</param>
		/// <returns>A <see cref="Money"/> with <see cref="Amount"/> as the result of substracting <paramref name="second"/> from <paramref name="first"/> amounts
		/// and the same <see cref="CurrencyCode"/> as any of the arguments.</returns>
		/// <exception cref="DifferentCurrencyException">If <paramref name="first"/> does not have the same <see cref="CurrencyCode"/>
		/// as <paramref name="second"/>.</exception>
		/// <exception cref="OverflowException">The <see cref="Amount"/> of the result is less than
		/// <see cref="decimal.MinValue"/> or greater than <see cref="decimal.MaxValue"/>.</exception>
		public static Money Subtract(Money first, Money second)
		{
			return first - second;
		}

		/// <summary>
		/// Creates an instance of <see cref="Money"/> witht the total value of an array.
		/// </summary>
		/// <remarks>All moneys have to have the same currency, otherwise and exception will be thrown.</remarks>
		/// <param name="moneys">A not null and not empty array of moneys.</param>
		/// <returns>An <see cref="Money"/> instance which <see cref="Amount"/> is the sum of all amounts of the moneys in the array,
		/// and <see cref="Currency"/> the same as all the moneys in the array.</returns>
		/// <exception cref="ArgumentNullException">If <paramref name="moneys"/> is null.</exception>
		/// <exception cref="ArgumentException">If <paramref name="moneys"/> is empty.</exception>
		/// <exception cref="DifferentCurrencyException">If any of the currencies of <paramref name="moneys"/> differ.</exception>
		public static Money Total(params Money[] moneys)
		{
			return Total((IEnumerable<Money>)moneys);
		}

		/// <summary>
		/// Creates an instance of <see cref="Money"/> witht the total value of an collection of moneys.
		/// </summary>
		/// <remarks></remarks>
		/// <param name="moneys">A not null and not empty collection of moneys.</param>
		/// <returns>An <see cref="Money"/> instance which <see cref="Amount"/> is the sum of all amounts of the moneys in the collection,
		/// and <see cref="Currency"/> the same as all the moneys in the collection.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="moneys"/> is null.</exception>
		/// <exception cref="ArgumentException"><paramref name="moneys"/> is empty.</exception>
		/// <exception cref="DifferentCurrencyException">If any of the currencies of <paramref name="moneys"/> differ.</exception>
		public static Money Total(IEnumerable<Money> moneys)
		{
			Guard.AgainstNullArgument("moneys", moneys);
			Guard.AgainstArgument("moneys", !moneys.Any(), "The collection of moneys cannot be empty.");

			return moneys.Aggregate((a, b) => a + b);
		}
	}
}
