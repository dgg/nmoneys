using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace NMoneys.Change
{
	public static partial class ChangeOperations
	{
		/// <summary>
		/// Returns a way to make change for a particular amount of money given a set of denominations in
		/// a simple an efficient manner.
		/// </summary>
		/// <remarks>The computation of the change of made using a greedy algorithm, that is, trying to fit bigger denominations in the result.
		/// <para>This way of computing might not yield an optimal (in terms of number of denominations) solution for arbitrary denomination sets.</para>
		/// <para>It is, however, optimal (in terms of number of denminations) for canonical denomination sets (those that minimize the average cost of making change).</para>
		/// <para>For denomination sets that provide no complete way of making change for the given amount, a partial change is calculated.</para>
		/// <para>An practival infinite (or sufficiently large) number of denominations is assumed. That is, we are not going to run out of denominations when making the change.</para>
		/// </remarks>
		/// <param name="money">The monetary quantity to make change of.</param>
		/// <param name="denominations">The monetary denominations for which the change is made.</param>
		/// <returns>A solution with the denominations used for making change.</returns>
		[Pure]
		public static ChangeSolution MakeChange(this Money money, params Denomination[] denominations)
		{
			Positive.Amounts.AssertArgument(nameof(money), money.Amount);

			Denomination[] orderedDenominations = denominations.OrderByDescending(d => d.Value).ToArray();
			decimal remainder = money.Amount;
			bool canContinue = true;

			List<Denomination> usedDenominations = new List<Denomination>();
			
			while (remainder > 0 && canContinue)
			{
				for (int i = 0; i < orderedDenominations.Length; i++)
				{
					// if the denomination can be substracted from what's left
					while (remainder >= orderedDenominations[i].Value)
					{
						// use the denomination
						remainder -= orderedDenominations[i].Value;
						usedDenominations.Add(orderedDenominations[i]);
					}
				}
				// make sure we can finish in case we cannot change
				canContinue = false;
			}
			ChangeSolution solution = new ChangeSolution(usedDenominations, 
				new Money(remainder, money.CurrencyCode));
			return solution;
		}

		/// <summary>
		/// Returns a way to make change for a particular amount of money given a set of denominations in
		/// a simple an efficient manner.
		/// </summary>
		/// <remarks>This overload is a facility method for easier syntax. <see cref="MakeChange(NMoneys.Money,NMoneys.Change.Denomination[])"/>
		/// for more information.</remarks>
		/// <param name="money">The monetary quantity to make change of.</param>
		/// <param name="denominationValues">The monetary denomination values for which the change is made.</param>
		/// <returns>A solution with the denominations used for making change.</returns>
		[Pure]
		public static ChangeSolution MakeChange(this Money money, params decimal[] denominationValues)
		{
			return money.MakeChange(denominationValues.Select(v => new Denomination(v)).ToArray());
		}
	}
}