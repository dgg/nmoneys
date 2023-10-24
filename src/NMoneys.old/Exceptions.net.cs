using System;
using System.Runtime.Serialization;

namespace NMoneys
{
	/// <summary>
	/// The exception that is thrown when a currency has not been properly configured.
	/// </summary>
	[Serializable]
	public partial class MisconfiguredCurrencyException : Exception
	{
		/// <summary>
		/// Initializes a new instace of <see cref="MisconfiguredCurrencyException"/> with serialized data
		/// </summary>
		/// <param name="info">The <see cref="SerializationInfo"/> that holds the serialized object data about the <see cref="MisconfiguredCurrencyException"/>.</param>
		/// <param name="context">The <see cref="StreamingContext"/> that contains contextual information about the source or destination.</param>
		protected MisconfiguredCurrencyException(SerializationInfo info, StreamingContext context) : base(info, context) { }
	}

	/// <summary>
	/// Currency that is thrown when two instances of <see cref="Money"/> are passed onto an opeation that can only be performed when they have the same currency.
	/// </summary>
	[Serializable]
	public partial class DifferentCurrencyException : InvalidOperationException
	{
		/// <summary>
		/// Initializes a new instace of <see cref="DifferentCurrencyException"/> with serialized data
		/// </summary>
		/// <param name="info">The <see cref="SerializationInfo"/> that holds the serialized object data about the <see cref="DifferentCurrencyException"/>.</param>
		/// <param name="context">The <see cref="StreamingContext"/> that contains contextual information about the source or destination.</param>
		protected DifferentCurrencyException(SerializationInfo info, StreamingContext context) : base(info, context) { }
	}
}
