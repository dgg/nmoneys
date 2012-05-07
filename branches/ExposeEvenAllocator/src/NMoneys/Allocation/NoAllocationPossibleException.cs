using System;
using System.Runtime.Serialization;

namespace NMoneys.Allocation
{
	[Serializable]
	public class NoAllocationPossibleException : NotSupportedException
	{
		public NoAllocationPossibleException(Money insufficientAmount, decimal minimumToAllocate) :
			this(string.Format("'{0}' is not enough to be allocated. Only quantities above '{1}' can be allocated", insufficientAmount.Format("{0} {2}"), minimumToAllocate)) { }

		public NoAllocationPossibleException() { }

		public NoAllocationPossibleException(string message) : base(message) { }

		public NoAllocationPossibleException(string message, Exception inner) : base(message, inner){ }

		protected NoAllocationPossibleException(SerializationInfo info, StreamingContext context) : base(info, context) { }
	}
}
