using System;
using System.Runtime.Serialization;

namespace NMoneys
{
	/// <summary>
	/// The exception that is thrown when a currency has not been properly configured.
	/// </summary>
	[Serializable]
	public class MisconfiguredCurrencyException : Exception
	{
		/// <summary>
		/// Initializes a new instance of <see cref="MisconfiguredCurrencyException"/>.
		/// </summary>
		/// <param name="isoCode">The currency which is missconfigured.</param>
		public MisconfiguredCurrencyException(CurrencyIsoCode isoCode) :
			this(string.Format("Currency with code {0} was not properly configured", isoCode)) { }

		/// <summary>
		/// Initializes a new instance of <see cref="MisconfiguredCurrencyException"/>.
		/// </summary>
		[Obsolete("Serialization")]
		public MisconfiguredCurrencyException() { }

		/// <summary>
		/// Initializes a new instance of <see cref="MisconfiguredCurrencyException"/>.
		/// </summary>
		/// <param name="message">A message that describes why this exception was thrown.</param>
		public MisconfiguredCurrencyException(string message) : base(message) { }

		/// <summary>
		/// Initializes a new instance of <see cref="MisconfiguredCurrencyException"/>.
		/// </summary>
		/// <param name="message">A message that describes why this exception was thrown.</param>
		/// <param name="inner">The exception that caused this exception to be thrown.</param>
		public MisconfiguredCurrencyException(string message, Exception inner) : base(message, inner) { }

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
	public class DifferentCurrencyException : InvalidOperationException
	{
		/// <summary>
		/// Initializes a new instance of <see cref="DifferentCurrencyException"/>.
		/// </summary>
		[Obsolete("Serialization")]
		public DifferentCurrencyException() { }

		/// <summary>
		/// Initializes a new instance of <see cref="DifferentCurrencyException"/>.
		/// </summary>
		/// <param name="expectedIsoSymbol">Textual representation of a ISO 4217 coden that was expected for the operation to be successful.</param>
		/// <param name="actualIsoSymbol">Textual representation of a ISO 4217 coden that provoked the exception.</param>
		public DifferentCurrencyException(string expectedIsoSymbol, string actualIsoSymbol) :
			this(DefaultMessage(expectedIsoSymbol, actualIsoSymbol)) { }

		/// <summary>
		/// Initializes a new instance of <see cref="DifferentCurrencyException"/>.
		/// </summary>
		/// <param name="message">A message that describes why this exception was thrown.</param>
		public DifferentCurrencyException(string message) : base(message) { }

		/// <summary>
		/// Initializes a new instance of <see cref="DifferentCurrencyException"/>.
		/// </summary>
		/// <param name="message">A message that describes why this exception was thrown.</param>
		/// <param name="inner">The exception that caused this exception to be thrown.</param>
		public DifferentCurrencyException(string message, Exception inner) : base(message, inner) { }

		/// <summary>
		/// Initializes a new instace of <see cref="DifferentCurrencyException"/> with serialized data
		/// </summary>
		/// <param name="info">The <see cref="SerializationInfo"/> that holds the serialized object data about the <see cref="DifferentCurrencyException"/>.</param>
		/// <param name="context">The <see cref="StreamingContext"/> that contains contextual information about the source or destination.</param>
		protected DifferentCurrencyException(SerializationInfo info, StreamingContext context) : base(info, context) { }

		/// <summary>
		/// Textual template to create the default <see cref="Exception.Message"/> of an instance of <see cref="DifferentCurrencyException"/>
		/// </summary>
		/// <param name="expectedIsoSymbol">Textual representation of a ISO 4217 coden that was expected for the operation to be successful.</param>
		/// <param name="actualIsoSymbol">Textual representation of a ISO 4217 coden that provoked the exception.</param>
		/// <returns>A string that contains the default message for a <see cref="DifferentCurrencyException"/> to be thrown.</returns>
		public static string DefaultMessage(string expectedIsoSymbol, string actualIsoSymbol)
		{
			return string.Format("Expected a currency with symbol \"{0}\", but currency with symbol \"{1}\" was passed.", expectedIsoSymbol, actualIsoSymbol);
		}
	}
}
