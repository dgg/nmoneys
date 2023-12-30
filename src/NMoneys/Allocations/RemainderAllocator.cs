namespace NMoneys.Allocations;

/// <summary>
	/// Factory class for provided implementations of <see cref="IRemainderAllocator"/>
	/// </summary>
	public static class RemainderAllocator
	{
		/// <summary>
		/// Allocate the remainder to the first known recipient and keep going in order towards
		/// the last recipient until the remainder has been fully allocated.
		/// </summary>
		/// <remarks>
		/// In the case of a dollar split three ways the extra penny to the first recipient
		/// and the allocation is done.
		/// </remarks>
		public static readonly IRemainderAllocator FirstToLast = new FirstToLastAllocator();

		/// <summary>
		/// Allocate the remainder to the last known recipient and keep going in order towards
		/// the first recipient until the remainder has been fully allocated.
		/// </summary>
		/// <remarks>
		/// In the case of a dollar split three ways the extra penny to the last recipient
		/// and the allocation is done.
		/// </remarks>
		public static readonly IRemainderAllocator LastToFirst = new LastToFirstAllocator();

		/// <summary>
		/// Allocate the remainder to a randomly selected recipient and keep going using another randomly
		/// selected recipient until the remainder has been fully allocated.
		/// </summary>
		/// <remarks>
		/// In the case of the dollar split three ways randomly, the recipient that gets the extra penny is indeterminate.
		/// <para>
		/// This <see cref="IRemainderAllocator"/> is much slower than either <see cref="FirstToLastAllocator"/>
		/// or <see cref="LastToFirstAllocator"/>. In addition, this particular implementation *could* arguably
		/// be made even more random. See the referenced links for some other implementation ideas that might
		/// enhance perf and/or randomness.
		/// </para>
		/// <seealso href="http://stackoverflow.com/questions/48087/select-a-random-n-elements-from-listt-in-c-sharp"/>
		/// <seealso href="http://www.fsmpi.uni-bayreuth.de/~dun3/archives/return-random-subset-of-n-elements-of-a-given-array/98.html"/>
		/// </remarks>
		public static readonly IRemainderAllocator Random = new RandomAllocator();
	}
