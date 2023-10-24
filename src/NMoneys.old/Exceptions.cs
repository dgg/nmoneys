using System;
using System.Runtime.Serialization;

namespace NMoneys
{
	/// <summary>
	/// The exception that is thrown when a currency has not been properly configured.
	/// </summary>
	public partial class MisconfiguredCurrencyException : Exception
	{
		/// <summary>
		/// Initializes a new instance of <see cref="MisconfiguredCurrencyException"/>.
		/// </summary>
		/// <param name="isoCode">The currency which is missconfigured.</param>
		public MisconfiguredCurrencyException(CurrencyIsoCode isoCode) :
			this($"Currency with code {isoCode} was not properly configured") { }

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
	}

	/// <summary>
	/// Currency that is thrown when two instances of <see cref="Money"/> are passed onto an opeation that can only be performed when they have the same currency.
	/// </summary>
	public partial class DifferentCurrencyException : InvalidOperationException
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
		/// Textual template to create the default <see cref="Exception.Message"/> of an instance of <see cref="DifferentCurrencyException"/>
		/// </summary>
		/// <param name="expectedIsoSymbol">Textual representation of a ISO 4217 coden that was expected for the operation to be successful.</param>
		/// <param name="actualIsoSymbol">Textual representation of a ISO 4217 coden that provoked the exception.</param>
		/// <returns>A string that contains the default message for a <see cref="DifferentCurrencyException"/> to be thrown.</returns>
		public static string DefaultMessage(string expectedIsoSymbol, string actualIsoSymbol)
		{
			return
				$"Expected a currency with symbol \"{expectedIsoSymbol}\", but currency with symbol \"{actualIsoSymbol}\" was passed.";
		}
	}
}
