using System;
using System.Collections;
using System.Collections.Generic;
using NMoneys.Support;

namespace NMoneys.Allocations
{
	public class Allocation : IEnumerable<Money>, IFormattable
	{
		private readonly Money[] _results;
		private readonly Money _allocatable, _allocated, _remainder;

		public Allocation(Money allocatable, Money[] results)
		{
			Guard.AgainstNullArgument("results", results);
			allocatable.AssertSameCurrency(results);

			_allocatable = allocatable;
			_results = results;

			_allocated = Money.Total(results);
			_remainder = _allocatable - _allocated;
		}

		public Money Allocatable { get { return _allocatable; } }
		
		public Money TotalAllocated { get { return _allocated; } }

		public Money Remainder { get { return _remainder; } }

		public bool IsComplete { get { return _allocatable.Equals(_allocated); } }

		public bool IsQuasiComplete { get { return _remainder < _allocatable.MinValue; } }

		#region collection-like

		/// <summary>
		/// Returns an enumerator that iterates through the collection.
		/// </summary>
		/// <returns>A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.</returns>
		public IEnumerator<Money> GetEnumerator()
		{
			return ((ICollection<Money>)_results).GetEnumerator();
		}

		/// <summary>
		/// Returns an enumerator that iterates through a collection.
		/// </summary>
		/// <returns>An <see cref="IEnumerator"/> object that can be used to iterate through the collection.</returns>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		/// <summary>
		/// Gets the element at the specified index.
		/// </summary>
		/// <param name="index">The index of the element to get.</param>
		/// <returns>The element at the specified index.</returns>
		public Money this[int index] { get { return _results[index]; } }

		/// <summary>
		/// Gets a 32-bit integer that represents the total number of elements of the <see cref="Allocation"/>. 
		/// </summary>
		/// <returns>A 32-bit integer that represents the total number of elements of the <see cref="Allocation"/>.</returns>
		public int Length { get { return _results.Length; } }

		#endregion

		#region formatting

		/// <summary>
		/// Returns a <see cref="string"/> that represents the current <see cref="Allocation"/>.
		/// </summary>
		/// <returns>
		/// A <see cref="string"/> that represents the current <see cref="Allocation"/>.
		/// </returns>
		public override string ToString()
		{
			return new Stringifier().Stringify(_results);
		}

		/// <summary>
		/// Formats the value of the current instance using the specified format.
		/// </summary>
		/// <remarks><paramref name="format"/> and <paramref name="formatProvider"/> are used to format each allocated monetary quantity.</remarks>
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
			return new Stringifier().Stringify(_results, m => m.ToString(format, formatProvider));
		}

		#endregion

		public static Allocation Zero(Money allocatable, int numberOfRecipients)
		{
			var results = new Money[numberOfRecipients];
			for (int i = 0; i < results.Length; i++)
			{
				results[i] = Money.Zero(allocatable.CurrencyCode);
			}
			return new Allocation(allocatable, results);
		}
	}
}
