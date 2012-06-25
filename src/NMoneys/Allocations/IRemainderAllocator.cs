using System.Collections.Generic;

namespace NMoneys.Allocations
{
	/// <summary>
	/// Distributes the remainder after allocating the highest fair amount amongst all the recipients.
	/// </summary>
	/// <remarks>
	/// IF the total amount to be allocated cannot be allocated to each recipient evenly, then
	/// this will define the order in which recipients will be allocated the remainder left over
	/// after an equal distribution is made. 
	/// <para>
	/// This is not relevant if the total sum of money can be fully distributed.
	/// </para>
	/// </remarks>
	/// <example>
	/// The classic example is that of a three-way split on a US dollar; after we
	/// allocate each recipient 33 cents we have a cent left over. In this case the <see cref="IRemainderAllocator"/>
	/// will determine which recipient gets that extra penny.
	/// </example>
	public interface IRemainderAllocator
	{
		/// <summary>
		/// Allocates the remainder after allocating the highest fair amount amongst all the recipients.
		/// </summary>
		/// <remarks>
		/// At the end of the distribution, the resulting allocation should, most of the times, be complete (nothing else to allocate).
		/// But it does not have to be like that. If the remaining amount is too small to be allocated, it will remain a remainder.
		/// <para>Implementors will increase the amounts used to generate the returning <see cref="Allocation"/> according to whichever strategy is chosen to distribute the remainder of <paramref name="allocationSoFar"/>.</para>
		/// <para>It may not be called at all if the money can be evenly distributed in <see cref="Money.Allocate(int)"/> or <see cref="Money.Allocate(RatioBag)"/>.</para>
		/// </remarks>
		/// <param name="allocationSoFar">Contains the remainder amount that might need to be allocated or <see cref="Money.Zero()"/> if no remainder is left.
		/// <para>It also contains the evenly allocated amounts (if any).</para>
		/// </param>
		/// <returns>A new instance of <see cref="Allocation"/> whose results are modified according to the strategy chosen to allocate the <see cref="Allocation.Remainder"/>.</returns>
		Allocation Allocate(Allocation allocationSoFar);
	}
}