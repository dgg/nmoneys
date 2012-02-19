using System.Collections.Generic;

namespace NMoneys.Allocation
{
	/// <summary>
	/// Distributes the remainder after allocating the highest fair amount amongst all the recipients.
	/// </summary>
	/// <remarks>
	/// IF the total amount to be allocated cannot be allocated to each recipient evenly, then
	/// this will define the order in which recipients will be allocated the remainder left over
	/// after an equal distribution is made. 
	/// <para>
	/// This is not relevant if the total sum of money can be evenly distributed.
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
		/// At the end of the distribution, the sum of the amounts of <paramref name="alreadyAllocated"/> must be equal to the original amount to allocate.
		/// <para>Implementors will increase the amounts of <paramref name="alreadyAllocated"/> according to whichever strategy is chosen to distribute the <paramref name="remainder"/>.</para>
		/// <para>It may not be called at all if the money can be evently distributed in <see cref="Money.Allocate"/>.</para>
		/// </remarks>
		/// <param name="remainder">The remainder amount that might need to be allocated or <see cref="Money.Zero()"/> if no remainder.</param>
		/// <param name="alreadyAllocated">An array representing the evenly allocated amounts.
		/// Its amounts will be modified according to the strategy chosen to allocate the <paramref name="remainder"/>.</param>
		void Allocate(Money remainder, IList<Money> alreadyAllocated);
	}
}