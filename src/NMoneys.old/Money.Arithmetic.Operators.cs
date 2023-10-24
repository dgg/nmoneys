using System;
using System.Diagnostics.Contracts;

namespace NMoneys
{
	public partial struct Money
	{
		/// <summary>
		/// Negates the value of the specified <see cref="Money"/> operand.
		/// </summary>
		/// <param name="money">The value to negate.</param>
		/// <returns>A <see cref="Money"/> with the <see cref="Amount"/> of <paramref name="money"/>, but multiplied by negative one (-1).</returns>
		[Pure]
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
		[Pure]
		public static Money operator +(Money first, Money second)
		{
			first.AssertSameCurrency(second);
			return new Money(first.Amount + second.Amount, first.CurrencyCode);
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
		[Pure]
		public static Money operator -(Money first, Money second)
		{
			first.AssertSameCurrency(second);
			return new Money(first.Amount - second.Amount, first.CurrencyCode);
		}

		/// <summary>
		/// Multiplies the specified <see cref="Money"/> by an integral factor.
		/// </summary>
		/// <remarks>Be careful when using very large factors (in the order of <see cref="ulong.MaxValue"/>)
		/// since they cannot be converted to <see cref="long"/>.</remarks>
		/// <param name="money">The instance which amount will be multiplied.</param>
		/// <param name="factor">The factor to multiply by.</param>
		/// <returns>A <see cref="Money"/> with <see cref="Amount"/> as the product of <see cref="Amount"/> and <paramref name="factor"/>
		/// and the same <see cref="CurrencyCode"/> as <paramref name="money"/>.</returns>
		/// <exception cref="OverflowException">The <see cref="Amount"/> of the result is less than
		/// <see cref="decimal.MinValue"/> or greater than <see cref="decimal.MaxValue"/>.</exception>
		[Pure]
		public static Money operator *(Money money, long factor)
		{
			return new Money(money.Amount * factor, money.CurrencyCode);
		}
	}
}
