using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Text;

namespace NMoneys.Allocations;

/// <summary>
/// Represents the allocated of an allocation operation.
/// </summary>
/// <seealso cref="MoneyExtensions.Allocate(Money, int, IRemainderAllocator)"/>
/// <seealso cref="MoneyExtensions.Allocate(Money, RatioCollection, IRemainderAllocator)"/>
/// <seealso cref="EvenAllocator.Allocate(int)"/>
/// <seealso cref="ProRataAllocator.Allocate(RatioCollection)"/>
public record Allocation : IEnumerable<Money>
{
	/// <summary>
	/// The monetary quantity being allocated.
	/// </summary>
	[Pure]
	public Money Allocatable { get; init; }

	/// <summary>
	/// All the money from <see cref="Allocatable"/> that has been allocated.
	/// </summary>
	[Pure]
	public Money TotalAllocated { get; init; }

	/// <summary>
	/// All the money from <see cref="Allocatable"/> that has not been allocated.
	/// </summary>
	[Pure]
	public Money Remainder { get; init; }

	/// <summary>
	/// The ISO 4217 code of all monetary quantities of the allocation.
	/// </summary>
	[Pure] public CurrencyIsoCode CurrencyCode { get; init; }

	private Money[] Allocated { get; init; }

	/// <summary>
	/// Initializes an instance of <see cref="Allocation"/>.
	/// </summary>
	/// <param name="allocatable">The monetary quantity subject of the allocation operation.</param>
	/// <param name="allocated">The raw allocated of an allocation (the quantities allocated).</param>
	/// <exception cref="ArgumentNullException"><paramref name="allocated"/> is null.</exception>
	/// <exception cref="ArgumentException"><paramref name="allocated"/> is empty.</exception>
	/// <exception cref="DifferentCurrencyException">At least one of the <paramref name="allocated"/> has a different currency from <paramref name="allocatable"/>'s.</exception>
	public Allocation(Money allocatable, IEnumerable<Money> allocated)
	{
		ArgumentNullException.ThrowIfNull(allocated, nameof(allocated));

		Allocated = allocated as Money[] ?? allocated.ToArray();
		allocatable.AssertSameCurrency(Allocated);
		Allocatable = allocatable;
		CurrencyCode = allocatable.CurrencyCode;

		TotalAllocated = Money.Total(Allocated);

		if (TotalAllocated.Abs() > allocatable.Abs())
		{
			throw new ArgumentOutOfRangeException(nameof(allocatable), allocatable,
				"One cannot allocate more than the allocatable amount.");
		}

		Remainder = Allocatable - TotalAllocated;
	}

	/// <summary>
	/// true if all the money from <see cref="Allocatable"/> has been allocated; otherwise, false.
	/// </summary>
	[Pure]
	public bool IsComplete => Allocatable.Equals(TotalAllocated);

	/// <summary>
	/// true if almost all the money from <see cref="Allocatable"/> has been allocated; otherwise, false.
	/// </summary>
	/// <remarks>"Almost" is defined by the minimum quantity that can be represented by the currency of <see cref="Allocatable"/>. <see cref="Currency.MinAmount"/></remarks>
	[Pure]
	public bool IsQuasiComplete => !IsComplete && Remainder.Abs() < Allocatable.MinValue.Abs();

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

	#region collection-like

	/// <inheritdoc />
	public IEnumerator<Money> GetEnumerator()
	{
		ICollection<Money> collection = Allocated;
		return collection.GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator() => Allocated.GetEnumerator();

	/// <summary>
	/// Gets the element at the specified index.
	/// </summary>
	/// <param name="index">The index of the element to get.</param>
	/// <returns>The element at the specified index.</returns>
	[Pure]
	public Money this[int index] => Allocated[index];

	/// <summary>
	/// Gets a 32-bit integer that represents the total number of elements of the <see cref="Allocation"/>.
	/// </summary>
	/// <returns>A 32-bit integer that represents the total number of elements of the <see cref="Allocation"/>.</returns>
	[Pure]
	public int Length => Allocated.Length;

	#endregion

#pragma warning disable CA1303
	/// <summary>
	///
	/// </summary>
	/// <param name="builder"></param>
	/// <returns></returns>
	protected virtual bool PrintMembers([NotNull] StringBuilder builder)
	{
		IFormatProvider currencyProvider = Allocatable.GetCurrency();
		builder.Append(CultureInfo.InvariantCulture, $"{nameof(CurrencyCode)} = {CurrencyCode} ");
		builder.Append(currencyProvider, $"{nameof(Allocatable)} = {Allocatable.Amount} ");
		builder.Append(currencyProvider, $"{nameof(TotalAllocated)} = {TotalAllocated.Amount} ");
		// no remainder
		if (IsComplete)
		{
			builder.Append(CultureInfo.InvariantCulture, $"{nameof(IsComplete)} = {IsComplete} ");
		}
		// there is a very small remainder
		else if (IsQuasiComplete)
		{
			builder.Append(CultureInfo.InvariantCulture, $"{nameof(IsQuasiComplete)} = {IsQuasiComplete} ");
			builder.Append(currencyProvider, $"{nameof(Remainder)} = {Remainder.Amount} ");
		}
		// there is a remainder
		else
		{
			builder.Append(currencyProvider, $"{nameof(Remainder)} = {Remainder.Amount} ");
		}

		builder.Append(CultureInfo.InvariantCulture, $"{nameof(Allocated)} = ");
		if (Length == 0)
		{
			builder.Append("[]");
		}
		else
		{
			builder.Append("[ ");
			builder.Append(String.Join(" | ", Allocated.Select(allocation => allocation.Amount.ToString(currencyProvider))));
			builder.Append(" ]");
		}

		return true;
	}
#pragma warning restore CA1303
}
