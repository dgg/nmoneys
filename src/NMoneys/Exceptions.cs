namespace NMoneys;

/// <summary>
/// The exception that is thrown when an argument of type <see cref="CurrencyIsoCode"/> is not defined within the enumeration.
/// </summary>
public class UndefinedCodeException : ArgumentException
{
	/// <inheritdoc />
	public UndefinedCodeException()
	{
	}

	/// <inheritdoc />
	public UndefinedCodeException(string message) : base(message)
	{
	}

	/// <inheritdoc />
	public UndefinedCodeException(string message, string paramName) : base(message, paramName)
	{
	}

	/// <inheritdoc />
	public UndefinedCodeException(string message, Exception inner) : base(message, inner)
	{
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="UndefinedCodeException"/> class with a descriptive error message
	/// when the name of the parameter is <b>code</b>.
	/// </summary>
	/// <param name="code">Code</param>
	public static UndefinedCodeException ForCode(CurrencyIsoCode code)
	{
		return new UndefinedCodeException($"Value {code} is not defined for {nameof(CurrencyIsoCode)}.", nameof(code));
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="UndefinedCodeException"/> class with a descriptive error message
	/// when the name of the parameter is <b>threeLetterIsoCode</b>.
	/// </summary>
	/// <param name="threeLetterIsoCode">Three-letter ISO code.</param>
	public static UndefinedCodeException ForCode(string threeLetterIsoCode)
	{
		return new UndefinedCodeException($"Value {threeLetterIsoCode} is not defined for {nameof(CurrencyIsoCode)}.",
			nameof(threeLetterIsoCode));
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="UndefinedCodeException"/> class with a descriptive error message
	/// when the name of the parameter is <b>code</b>.
	/// </summary>
	/// <param name="code">Code</param>
	public static UndefinedCodeException ForCode(ushort code)
	{
		return new UndefinedCodeException($"Value {code} is not defined for {nameof(CurrencyIsoCode)}.", nameof(code));
	}
}

/// <summary>
/// The exception that is thrown when a currency has not been properly configured in code.
/// </summary>
public class MisconfiguredCurrencyException : Exception
{
	/// <summary>
	/// Initializes a new instance of <see cref="MisconfiguredCurrencyException"/>.
	/// </summary>
	public MisconfiguredCurrencyException()
	{
	}

	/// <summary>
	/// Initializes a new instance of <see cref="MisconfiguredCurrencyException"/>.
	/// </summary>
	/// <param name="message">A message that describes why this exception was thrown.</param>
	public MisconfiguredCurrencyException(string message) : base(message)
	{
	}

	/// <summary>
	/// Initializes a new instance of <see cref="MisconfiguredCurrencyException"/>.
	/// </summary>
	/// <param name="message">A message that describes why this exception was thrown.</param>
	/// <param name="inner">The exception that caused this exception to be thrown.</param>
	public MisconfiguredCurrencyException(string message, Exception inner) : base(message, inner)
	{
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="MisconfiguredCurrencyException"/> class with a descriptive error message.
	/// </summary>
	/// <param name="code">Code</param>
	public static MisconfiguredCurrencyException ForCode(CurrencyIsoCode code)
	{
		return new MisconfiguredCurrencyException($"Currency with code {code} was not properly configured.");
	}
}

/// <summary>
/// The exception that is thrown when a currency has already been initialized.
/// </summary>
public class InitializedCurrencyException : Exception
{
	/// <summary>
	/// Initializes a new instance of <see cref="InitializedCurrencyException"/>.
	/// </summary>
	public InitializedCurrencyException()
	{
	}

	/// <summary>
	/// Initializes a new instance of <see cref="InitializedCurrencyException"/>.
	/// </summary>
	/// <param name="message">A message that describes why this exception was thrown.</param>
	public InitializedCurrencyException(string message) : base(message)
	{
	}

	/// <summary>
	/// Initializes a new instance of <see cref="InitializedCurrencyException"/>.
	/// </summary>
	/// <param name="message">A message that describes why this exception was thrown.</param>
	/// <param name="inner">The exception that caused this exception to be thrown.</param>
	public InitializedCurrencyException(string message, Exception inner) : base(message, inner)
	{
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="InitializedCurrencyException"/> class with a descriptive error message.
	/// </summary>
	/// <param name="code">Code</param>
	public static InitializedCurrencyException ForCode(CurrencyIsoCode code)
	{
		return new InitializedCurrencyException($"Currency with code {code} was already initialized.");
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="InitializedCurrencyException"/> class with a descriptive error message.
	/// </summary>
	/// <param name="threeLetterIsoCode">Code</param>
	public static InitializedCurrencyException ForCode(string threeLetterIsoCode)
	{
		return new InitializedCurrencyException($"Currency with code {threeLetterIsoCode} was already initialized.");
	}
}

/// <summary>
/// Currency that is thrown when two instances of <see cref="Money"/> are passed into an operation that can only be performed when they have the same currency.
/// </summary>
public class DifferentCurrencyException : Exception
{
	/// <summary>
	/// Initializes a new instance of <see cref="DifferentCurrencyException"/>.
	/// </summary>
	[Obsolete("Serialization")]
	public DifferentCurrencyException()
	{
	}

	/// <summary>
	/// Initializes a new instance of <see cref="DifferentCurrencyException"/>.
	/// </summary>
	/// <param name="message">A message that describes why this exception was thrown.</param>
	public DifferentCurrencyException(string message) : base(message)
	{
	}

	/// <summary>
	/// Initializes a new instance of <see cref="DifferentCurrencyException"/>.
	/// </summary>
	/// <param name="message">A message that describes why this exception was thrown.</param>
	/// <param name="inner">The exception that caused this exception to be thrown.</param>
	public DifferentCurrencyException(string message, Exception inner) : base(message, inner)
	{
	}

	/// <summary>
	/// Initializes a new instance of <see cref="DifferentCurrencyException"/>.
	/// </summary>
	/// <param name="expectedIsoSymbol">Textual representation of a ISO 4217 code that was expected for the operation to be successful.</param>
	/// <param name="actualIsoSymbol">Textual representation of a ISO 4217 code that provoked the exception.</param>
	public static DifferentCurrencyException ForCodes(string expectedIsoSymbol, string actualIsoSymbol)
	{
		string message =
			$"Expected a currency with symbol \"{expectedIsoSymbol}\", but currency with symbol \"{actualIsoSymbol}\" was passed.";
		return new DifferentCurrencyException(message);
	}
}
