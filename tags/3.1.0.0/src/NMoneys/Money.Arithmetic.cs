using System;
using System.Globalization;
using NMoneys.Support;

namespace NMoneys
{
	public partial struct Money
	{
		/// <summary>
		/// Adds two specified <see cref="Money"/> values.
		/// </summary>
		/// <remarks><paramref name="money"/> must have the same <see cref="CurrencyCode"/> as this instance, otherwise an exception will be thrown.</remarks>
		/// <param name="money">The value to add.</param>
		/// <returns>A <see cref="Money"/> with <see cref="Amount"/> as the sum of <paramref name="money"/> amount and this <see cref="Amount"/>
		/// and the same <see cref="CurrencyCode"/> this instance.</returns>
		/// <exception cref="DifferentCurrencyException">If <paramref name="money"/> does not have the same <see cref="CurrencyCode"/> as this instance.</exception>
		/// <exception cref="OverflowException">The <see cref="Amount"/> of the result is less than  <see cref="decimal.MinValue"/> or greater than <see cref="decimal.MaxValue"/>.</exception>
		public Money Plus(Money money)
		{
			return this + money;
		}

		/// <summary>
		/// Substracts one specified <see cref="Money"/> from another.
		/// </summary>
		/// <remarks><paramref name="money"/> must have the same <see cref="CurrencyCode"/> as this instance, otherwise an exception will be thrown.</remarks>
		/// <param name="money">The subtrahend.</param>
		/// <returns>A <see cref="Money"/> with <see cref="Amount"/> as the result of substracting <paramref name="money"/>'s <see cref="Amount"/> from this instance's
		/// and the same <see cref="CurrencyCode"/> as this instance.</returns>
		/// <exception cref="DifferentCurrencyException">If <paramref name="money"/> does not have the same <see cref="CurrencyCode"/> as this instance.</exception>
		/// <exception cref="OverflowException">The <see cref="Amount"/> of the result is less than <see cref="decimal.MinValue"/> or greater than <see cref="decimal.MaxValue"/>.</exception>
		public Money Minus(Money money)
		{
			return this - money;
		}

		/// <summary>
		/// Returns the absolute value of a <see cref="Money"/>.
		/// </summary>
		/// <remarks>The absolute value of a <see cref="Money"/> is another <see cref="Money"/> which <see cref="Amount"/> is the numeric value without its sign.
		/// For example, the absolute value of both $1.2 and ($1.2) is $1.2.</remarks>
		/// <returns>A <see cref="Money"/> with <see cref="Amount"/> as the absolute value of this instance's.</returns>
		public Money Abs()
		{
			return new Money(Math.Abs(Amount), CurrencyCode);
		}

		/// <summary>
		/// Returns the result of multiplying this instance of <see cref="Money"/> by negative one.
		/// </summary>
		/// <returns>A <see cref="Money"/> with the <see cref="Amount"/> of this instance's, but the opposite sign.</returns>
		public Money Negate()
		{
			return new Money(decimal.Negate(Amount), CurrencyCode);
		}

		/// <summary>
		/// Truncates the <see cref="Amount"/> to the number of significant decimal digits specified by the <see cref="Currency"/>
		/// identified this <see cref="CurrencyCode"/>.
		/// </summary>
		/// <returns>A <see cref="Money"/> with the <see cref="Amount"/> truncated to the significant number of decimal digits of its currency.</returns>
		public Money TruncateToSignificantDecimalDigits()
		{
			Currency currency = Currency.Get(CurrencyCode);
			return new Money(truncateAmountFor(currency.SignificantDecimalDigits), CurrencyCode);
		}

		/// <summary>
		/// Truncates the <see cref="Amount"/> to the number of significant decimal digits specified by <paramref name="numberFormat"/>.
		/// </summary>
		/// <param name="numberFormat">Specifies the number of significant decimal digits.</param>
		/// <returns>A <see cref="Money"/> with the <see cref="Amount"/> truncated to the significant number of decimal digits of <paramref name="numberFormat"/>.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="numberFormat"/> is null.</exception>
		public Money TruncateToSignificantDecimalDigits(NumberFormatInfo numberFormat)
		{
			Guard.AgainstNullArgument("numberFormat", numberFormat);

			return new Money(truncateAmountFor(numberFormat.CurrencyDecimalDigits), CurrencyCode);
		}

		private decimal truncateAmountFor(int numberOfDecimals)
		{
			uint centFactor = PowerOfTen.Positive(numberOfDecimals);
			decimal truncatedAmount = (decimal)((long)Math.Truncate(Amount * centFactor)) / centFactor;
			return truncatedAmount;
		}

		/// <summary>
		/// Returns the integral digits of this instance of <see cref="Money"/>; any fractional digits are discarded.
		/// </summary>
		/// <remarks>This method rounds <see cref="Amount"/> toward zero, to the nearest whole number, which corresponds to discarding any digits after the decimal point.</remarks>
		/// <returns>A <see cref="Money"/> with <see cref="Amount"/> the result of <see cref="Amount"/> rounded toward zero, to the nearest whole number.</returns>
		public Money Truncate()
		{
			return new Money(decimal.Truncate(Amount), CurrencyCode);
		}

		/// <summary>
		/// Rounds <see cref="Amount"/> to the nearest integer.
		/// </summary>
		/// <returns>
		/// A <see cref="Money"/> with an integer <see cref="Amount"/> that is nearest to the old value.
		/// <para>If <see cref="Amount"/> is halfway between two integers, one of which is even and the other odd, the even number is chosen.</para>
		/// </returns>
		public Money RoundToNearestInt()
		{
			return new Money(decimal.Round(Amount), CurrencyCode);
		}

		/// <summary>
		/// Rounds <see cref="Amount"/> to the nearest integer. A parameter specifies how to round the value if it is midway between two other numbers.
		/// </summary>
		/// <param name="mode">A value that specifies how to round <see cref="Amount"/> if it is midway between two other numbers.</param>
		/// <returns>
		/// A <see cref="Money"/> with an integer <see cref="Amount"/> that is nearest to the previous <see cref="Amount"/> value.
		/// <para>If <see cref="Amount"/> is halfway between two numbers, one of which is even and the other odd, the mode parameter determines which of the two numbers is chosen.</para>
		/// </returns>
		public Money RoundToNearestInt(MidpointRounding mode)
		{
			return new Money(decimal.Round(Amount, mode), CurrencyCode);
		}

		/// <summary>
		/// Rounds <see cref="Amount"/> to a the number of decimal places specified by the <see cref="Currency"/> identified by <see cref="CurrencyCode"/>.
		/// </summary>
		/// <returns>
		/// A <see cref="Money"/> with the <see cref="Amount"/> equivalent to previous <see cref="Amount"/> rounded to the number of decimal places specified by its <see cref="CurrencyCode"/>.
		/// </returns>
		/// <remarks>
		/// When <see cref="Amount"/> is exactly halfway between two rounded values, the resultant <see cref="Amount"/> is the rounded value that has an even digit in the far right decimal position. For example, when rounded to two <see cref="Amount"/>s, the value <c>2.345</c> becomes <c>2.34</c> and the value <c>2.355</c> becomes 2.36. This process is known as rounding toward even, or rounding to nearest.
		/// <para>The behavior of this method follows IEEE Standard 754, section 4. This kind of rounding is sometimes called rounding to nearest or banker's rounding.</para>
		/// </remarks>
		public Money Round()
		{
			Currency currency = Currency.Get(CurrencyCode);
			return Round(currency.SignificantDecimalDigits);
		}

		/// <summary>
		/// Rounds <see cref="Amount"/> to a precision specified by the <see cref="Currency"/> identified by <see cref="CurrencyCode"/>.
		/// <paramref name="mode"/> specifies how to round the value if it is midway between two other numbers.
		/// </summary>
		/// <param name="mode">A value that specifies how to round <see cref="Amount"/> if it is midway between two other numbers.</param>
		/// <returns>
		/// A <see cref="Money"/> with <see cref="Amount"/> that is nearest to the previous <see cref="Amount"/> with a precision equal to <see cref="Currency.SignificantDecimalDigits"/>.
		/// If <see cref="Amount"/> is halfway between two numbers, one of which is even and the other odd, the mode parameter determines which of the two numbers is chosen.
		/// If the precision of <see cref="Amount"/> is less than <see cref="Currency.SignificantDecimalDigits"/>, <see cref="Amount"/> remains unchanged.
		/// </returns>
		/// <remarks>
		/// The <see cref="Currency.SignificantDecimalDigits"/> specifies the number of significant decimal places in the return value and ranges from 0 to 28.
		/// <para>If <see cref="Currency.SignificantDecimalDigits"/> is zero, an integer is returned.</para>
		/// <para>The behavior of this method follows IEEE Standard 754, section 4. This kind of rounding is sometimes called rounding to nearest or banker's rounding.</para>
		/// <para>If <see cref="Currency.SignificantDecimalDigits"/> is zero, this kind of rounding is sometimes called rounding toward zero.</para>
		/// </remarks>
		public Money Round(MidpointRounding mode)
		{
			Currency currency = Currency.Get(CurrencyCode);
			return Round(currency.SignificantDecimalDigits, mode);
		}

		/// <summary>
		/// Rounds <see cref="Amount"/> to a specified number of decimal places.
		/// </summary>
		/// <param name="decimals">A value from 0 to 28 that specifies the number of decimal places to round to.</param>
		/// <returns>
		/// A <see cref="Money"/> with <see cref="Amount"/> rounded to decimals number of decimal places.
		/// </returns>
		/// <remarks>
		/// The behavior of this method follows IEEE Standard 754, section 4. This kind of rounding is sometimes called rounding to nearest or banker's rounding.
		/// </remarks>
		public Money Round(int decimals)
		{
			return new Money(decimal.Round(Amount, decimals), CurrencyCode);
		}

		/// <summary>
		/// Rounds <see cref="Amount"/> to a specified precision. A parameter specifies how to round the value if it is midway between two other numbers.
		/// </summary>
		/// <param name="decimals">A value from 0 to 28 that specifies the number of decimal places to round to.</param>
		/// <param name="mode">A value that specifies how to round <see cref="Amount"/> if it is midway between two other numbers.</param>
		/// <returns>
		/// A <see cref="Money"/> with <see cref="Amount"/> that is nearest to the previous <see cref="Amount"/> with a precision equal to the <paramref name="decimals"/> parameter.
		/// <para>If previous <see cref="Amount"/> is halfway between two numbers, one of which is even and the other odd, the mode parameter determines which of the two numbers is returned.</para>
		/// <para>If the precision of d  is less than decimals, d is returned unchanged.</para>
		/// </returns>
		/// <remarks>
		/// The <paramref name="decimals"/> specifies the number of significant decimal places in the return value and ranges from 0 to 28.
		/// <para>If <paramref name="decimals"/> is zero, an integer is returned.</para>
		/// <para>The behavior of this method follows IEEE Standard 754, section 4. This kind of rounding is sometimes called rounding to nearest or banker's rounding.</para>
		/// <para>If <paramref name="decimals"/> is zero, this kind of rounding is sometimes called rounding toward zero.</para>
		/// </remarks>
		public Money Round(int decimals, MidpointRounding mode)
		{
			return new Money(decimal.Round(Amount, decimals, mode), CurrencyCode);
		}

		/// <summary>
		/// Rounds <see cref="Amount"/> to the closest integer toward negative infinity.
		/// </summary>
		/// <returns>A <see cref="Money"/> which <see cref="Amount"/> is:
		/// <para>if <see cref="Amount"/> has a fractional part, the next whole decimal number toward negative infinity that is less than <see cref="Amount"/>.</para>
		///<para>-or-</para>
		///<para>If <see cref="Amount"/> doesn't have a fractional part, is remains unchanged.</para></returns>
		public Money Floor()
		{
			return new Money(decimal.Floor(Amount), CurrencyCode);
		}

		/// <summary>
		/// Performs the arithmetial operation <paramref name="binaryOperation"/> on <see cref="Amount"/>.
		/// </summary>
		/// <param name="operand">Value which <see cref="Amount"/> serves as the second argument to <paramref name="binaryOperation"/>.</param>
		/// <param name="binaryOperation">Arithmetical operation to perform.</param>
		/// <returns>A <see cref="Money"/> with <see cref="Amount"/> as the result of applying <paramref name="binaryOperation"/> to he old amount and
		/// <paramref name="operand"/>'s amount.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="binaryOperation"/> is null.</exception>
		public Money Perform(Money operand, Func<decimal, decimal, decimal> binaryOperation)
		{
			Guard.AgainstNullArgument("binaryOperation", binaryOperation);

			AssertSameCurrency(operand);
			return new Money(binaryOperation(Amount, operand.Amount), CurrencyCode);
		}

		/// <summary>
		/// Performs the arithmetial operation <paramref name="unaryOperation"/> on <see cref="Amount"/>.
		/// </summary>
		/// <param name="unaryOperation">Arithmetical operation to perform.</param>
		/// <returns>a <see cref="Money"/> with <see cref="Amount"/> as the result of applying <paramref name="unaryOperation"/> to the previous <see cref="Amount"/>.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="unaryOperation"/> is null.</exception>
		public Money Perform(Func<decimal, decimal> unaryOperation)
		{
			Guard.AgainstNullArgument("unaryOperation", unaryOperation);

			return new Money(unaryOperation(Amount), CurrencyCode);
		}
	}
}
