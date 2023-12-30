using System.Globalization;

namespace NMoneys;

/// <summary>
/// Represents a reserved currency character in an HTML/XML document.
/// </summary>
/// <param name="CodePoint">The unicode code point of a currency character reference.</param>
/// <param name="Name">The name of a "named" currency character reference, e.g. "euro".</param>
public sealed record CharacterReference(ushort CodePoint, string? Name = null)
{
	/// <summary>
	/// The decimal code.
	/// </summary>
	/// <example>&amp;#8364;</example>
	public string DecimalReference => $"&#{CodePoint};";
	/// <summary>
	/// The hexadecimal code.
	/// </summary>
	/// <example>&amp;#x20AC;</example>
	public string HexadecimalReference => $"&#x{CodePoint.ToString("X", CultureInfo.InvariantCulture)};";
	/// <summary>
	/// If the reference can be named, the entity; <c>null</c> otherwise.
	/// </summary>
	/// <example>&amp;euro;</example>
	public string? NamedReference => Name != null ? $"&{Name};" : null;
	/// <summary>
	/// Character representation of the reference.
	/// </summary>
	/// <example>€</example>
	public string Character => char.ConvertFromUtf32(CodePoint);

	/// <inheritdoc />
	public bool Equals(CharacterReference? other) => other?.CodePoint == CodePoint;

	/// <inheritdoc />
	public override int GetHashCode() => CodePoint;
}
