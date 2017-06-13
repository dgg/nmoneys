using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NMoneys.Support;

namespace NMoneys.Allocations
{
	/// <summary>
	/// Maintains a list of ratios suitable for use in an allocation when the
	/// sum of all items is exactly equal to one (100%).
	/// </summary>
	public class RatioCollection : IEnumerable<Ratio>, IFormattable
	{
		private readonly Ratio[] _ratios;

		#region Creation

		/// <summary>
		/// Initializes a new instance of <see cref="RatioCollection"/> with the specified ratio values.
		/// </summary>
		/// <remarks>This is a helper constructor due to the verbosity of <see cref="Ratio"/> construction.</remarks>
		/// <param name="ratioValues">A collection of ratio values.</param>
		/// <exception cref="ArgumentNullException"><paramref name="ratioValues"/> is null.</exception>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="ratioValues"/> do not sum up one
		/// -or-
		/// any of the values does not fall in the range [0..1].
		/// .</exception>
		public RatioCollection(params decimal[] ratioValues) : this(toRatios(ratioValues)) { }

		private static Ratio[] toRatios(decimal[] ratioValues)
		{
			Guard.AgainstNullArgument(nameof(ratioValues), ratioValues);
			return ratioValues.Select(r => new Ratio(r)).ToArray();
		}

		/// <summary>
		/// Initializes a new instance of <see cref="RatioCollection"/> with the specified <paramref name="ratios"/>.
		/// </summary>
		/// <param name="ratios">A collection of ratios</param>
		/// <exception cref="ArgumentNullException"><paramref name="ratios"/> is null.</exception>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="ratios"/> do not sum up one.</exception>
		public RatioCollection(params Ratio[] ratios)
		{
			Guard.AgainstNullArgument(nameof(ratios), ratios);
			assertAllocatable(ratios);

			_ratios = ratios;
		}

		private void assertAllocatable(IEnumerable<Ratio> ratios)
		{
			decimal sum = ratios.Select(r => r.Value).Sum();
			if (!sum.Equals(decimal.One))
			{
				throw new ArgumentOutOfRangeException(nameof(ratios), sum, "Ratios have to sum up 1.0.");
			}
		}

		#endregion

		#region collection-like methods

		/// <summary>
		/// Returns an enumerator that iterates through the collection.
		/// </summary>
		/// <returns>A <see cref="IEnumerator{Ratio}"/> that can be used to iterate through the collection.</returns>
		public IEnumerator<Ratio> GetEnumerator() { return ((IEnumerable<Ratio>)_ratios).GetEnumerator(); }


		/// <summary>
		/// Returns an enumerator that iterates through a collection.
		/// </summary>
		/// <returns>An <see cref="IEnumerator"/> object that can be used to iterate through the collection.</returns>
		IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }

		/// <summary>
		/// Gets a 32-bit integer that represents the total number of ratios in the <see cref="RatioCollection"/>.
		/// </summary>
		/// <returns>A 32-bit integer that represents the total number of ratios in the <see cref="RatioCollection"/>.</returns>
		public int Count => _ratios.Length;

		/// <summary>
		/// Gets the ratio at the specified index.
		/// </summary>
		/// <param name="index">The index of the ratio to get.</param>
		/// <returns>The ratio at the specified index.</returns>
		/// <exception cref="ArgumentOutOfRangeException">index is less than zero.
		/// -or-
		/// index is equal to or greater than <see cref="Count"/>.</exception>
		public Ratio this[int index] => _ratios[index];

		#endregion

		/// <summary>
		/// Returns a <see cref="string"/> that represents the current <see cref="RatioCollection"/>.
		/// </summary>
		/// <returns>
		/// A <see cref="string"/> that represents the current <see cref="RatioCollection"/>.
		/// </returns>
		public override string ToString()
		{
			return Stringifier.Default.Stringify(_ratios);
		}

		/// <summary>
		/// Formats the value of the current instance using the specified format.
		/// </summary>
		/// <remarks><paramref name="format"/> and <paramref name="formatProvider"/> are used to format each ratio in the bag.</remarks>
		/// <returns>A <see cref="string"/> containing the value of the current instance in the specified format.</returns>
		/// <param name="format">The <see cref="string"/> specifying the format to use.                   
		/// -or- 
		/// null to use the default format defined for the type of the <see cref="IFormattable"/> implementation.
		/// </param>
		/// <param name="formatProvider">The <see cref="IFormatProvider"/> to use to format the value.
		/// -or- 
		/// null to obtain the numeric format information from the current locale setting of the operating system. 
		/// </param>
		public string ToString(string format, IFormatProvider formatProvider)
		{
			return Stringifier.Default.Stringify(_ratios, r => r.ToString(format, formatProvider));
		}
	}
}
