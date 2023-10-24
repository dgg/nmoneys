﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using NMoneys.Support;

namespace NMoneys.Allocations
{
	/// <summary>
	/// Represents the allocated of an allocation operation.
	/// </summary>
	/// <seealso cref="AllocateOperations.Allocate(Money, int, IRemainderAllocator)"/>
	/// <seealso cref="AllocateOperations.Allocate(Money, RatioCollection, IRemainderAllocator)"/>
	/// <seealso cref="EvenAllocator.Allocate(int)"/>
	/// <seealso cref="ProRataAllocator.Allocate(RatioCollection)"/>
	public class Allocation : IEnumerable<Money>, IFormattable
	{
		private readonly Money[] _allocated;
		private readonly Money _allocatable, _totalAllocated, _remainder;

		/// <summary>
		/// Initializes an instance of <see cref="Allocation"/>.
		/// </summary>
		/// <param name="allocatable">The monetary quantity subject of the allocation operation.</param>
		/// <param name="allocated">The raw allocated of an allocation (the quantities allocated).</param>
		/// <exception cref="ArgumentNullException"><paramref name="allocated"/> is null.</exception>
		/// <exception cref="DifferentCurrencyException">At least one of the <paramref name="allocated"/> has a different currency from <paramref name="allocatable"/>'s.</exception>
		public Allocation(Money allocatable, Money[] allocated)
		{
			Guard.AgainstNullArgument(nameof(allocated), allocated);
			allocatable.AssertSameCurrency(allocated);

			_allocatable = allocatable;
			_totalAllocated = Money.Total(allocated);
			
			assertSensibleAllocation(nameof(allocated));
			_allocated = allocated;
			
			_remainder = _allocatable - _totalAllocated;
		}

		// asserts that the allocated amount can be allocated from the allocatable amount
		private void assertSensibleAllocation(string paramName)
		{
			// absolute values is used to simplify the support the case of debt allocation
			var allocatable =_allocatable.Abs();
			var allocated = _totalAllocated.Abs();

			Guard.AgainstArgument(paramName, allocated > allocatable, "One cannot allocate more than the allocatable amount.");
		}

		/// <summary>
		/// The monetary quantity being allocated.
		/// </summary>
		[Pure]
		public Money Allocatable => _allocatable;

		/// <summary>
		/// All the money from <see cref="Allocatable"/> that has been allocated.
		/// </summary>
		[Pure]
		public Money TotalAllocated => _totalAllocated;

		/// <summary>
		/// All the money from <see cref="Allocatable"/> that has not been allocated.
		/// </summary>
		[Pure]
		public Money Remainder => _remainder;

		/// <summary>
		/// true if all the money from <see cref="Allocatable"/> has been allocated; otherwise, false.
		/// </summary>
		[Pure]
		public bool IsComplete => _allocatable.Equals(_totalAllocated);

		/// <summary>
		/// true if almost all the money from <see cref="Allocatable"/> has been allocated; otherwise, false.
		/// </summary>
		/// <remarks>"Almost" is defined by the minimum quantity that can be represented by the currency of <see cref="Allocatable"/>. <see cref="Currency.MinAmount"/></remarks>
		[Pure]
		public bool IsQuasiComplete => !IsComplete && _remainder.Abs() < _allocatable.MinValue.Abs();

		#region collection-like

		/// <summary>
		/// Returns an enumerator that iterates through the collection.
		/// </summary>
		/// <returns>A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.</returns>
		[Pure]
		public IEnumerator<Money> GetEnumerator()
		{
			return ((ICollection<Money>)_allocated).GetEnumerator();
		}

		/// <summary>
		/// Returns an enumerator that iterates through a collection.
		/// </summary>
		/// <returns>An <see cref="IEnumerator"/> object that can be used to iterate through the collection.</returns>
		[Pure]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		/// <summary>
		/// Gets the element at the specified index.
		/// </summary>
		/// <param name="index">The index of the element to get.</param>
		/// <returns>The element at the specified index.</returns>
		[Pure]
		public Money this[int index] => _allocated[index];

		/// <summary>
		/// Gets a 32-bit integer that represents the total number of elements of the <see cref="Allocation"/>. 
		/// </summary>
		/// <returns>A 32-bit integer that represents the total number of elements of the <see cref="Allocation"/>.</returns>
		[Pure]
		public int Length => _allocated.Length;

		#endregion

		#region formatting

		/// <summary>
		/// Returns a <see cref="string"/> that represents the current <see cref="Allocation"/>.
		/// </summary>
		/// <returns>
		/// A <see cref="string"/> that represents the current <see cref="Allocation"/>.
		/// </returns>
		[Pure]
		public override string ToString()
		{
			return Stringifier.Default.Stringify(_allocated);
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
		[Pure]
		public string ToString(string format, IFormatProvider formatProvider)
		{
			return new Stringifier().Stringify(_allocated, m => m.ToString(format, formatProvider));
		}

		#endregion

		/// <summary>
		/// Initializes an "empty" allocation where money could be allocated.
		/// </summary>
		/// <param name="allocatable">The monetary quantity subject of the allocation operation.</param>
		/// <param name="numberOfRecipients">The number of times to split up the total.</param>
		/// <returns>An allocation of <see cref="Length"/> equal to <paramref name="numberOfRecipients"/> and zero <see cref="TotalAllocated"/>.</returns>
		[Pure]
		public static Allocation Zero(Money allocatable, int numberOfRecipients)
		{
			return new Allocation(allocatable, Money.Zero(allocatable.CurrencyCode, numberOfRecipients));
		}
	}
}
