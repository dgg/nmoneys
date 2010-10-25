using System;
using System.Configuration;
using System.Runtime.Serialization;

namespace NMoneys
{
	[Serializable]
	public class MissconfiguredCurrencyException : ConfigurationException
	{
		public MissconfiguredCurrencyException(CurrencyIsoCode isoCode) :
			this(string.Format("Currency with code {0} was not properly configured", isoCode)) { }

		[Obsolete("Serialization")]
		public MissconfiguredCurrencyException() { }

		public MissconfiguredCurrencyException(string message) : base(message) { }

		public MissconfiguredCurrencyException(string message, Exception inner) : base(message, inner) { }

		protected MissconfiguredCurrencyException(SerializationInfo info, StreamingContext context) : base(info, context) { }
	}

	[Serializable]
	public class DifferentCurrencyException : InvalidOperationException
	{
		[Obsolete("Serialization")]
		public DifferentCurrencyException() { }

		public DifferentCurrencyException(string expectedIsoSymbol, string actualIsoSymbol) :
			this(DefaultMessage(expectedIsoSymbol, actualIsoSymbol)) { }

		public DifferentCurrencyException(string message) : base(message) { }

		public DifferentCurrencyException(string message, Exception innerException) : base(message, innerException) { }

		protected DifferentCurrencyException(SerializationInfo info, StreamingContext context) : base(info, context) { }

		public static string DefaultMessage(string expectedIsoSymbol, string actualIsoSymbol)
		{
			return string.Format("Expected a currency with symbol \"{0}\", but currency with symbol \"{1}\" was passed.", expectedIsoSymbol, actualIsoSymbol);
		}
	}
}
