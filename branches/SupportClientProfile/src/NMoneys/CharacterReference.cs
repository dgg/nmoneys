using System;
using System.Collections.Generic;
using System.Globalization;
using NMoneys.Support;

namespace NMoneys
{
	/// <summary>
	/// Represents a reserved character in an HTML/XML document.
	/// </summary>
	public class CharacterReference : IEquatable<CharacterReference>
	{
		/// <summary>
		/// Creates an empty instance. <see cref="IsEmpty"/> will be true.
		/// </summary>
		/// <remarks>To discourage the use of null, a "null object" pattern is used. "Null objects" are empty.</remarks>
		private CharacterReference()
		{
			EntityName = SimpleName = Character = string.Empty;
			CodePoint = 0;
			EntityNumber = "&#00;";
			IsEmpty = true;
		}

		/// <summary>Empty character reference. <see cref="IsEmpty"/> will be true.</summary>
		/// <remarks>To discourage the use of null, a "null object" pattern is used. "Null objects" are empty.</remarks>
		public static readonly CharacterReference Empty = new CharacterReference();

		private static readonly Dictionary<string, string> _htmlEntities = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
		{
			{"cent", "¢"},
			{"pound", "£"},
			{"curren", "¤"},
			{"yen", "¥"},
			{"fnof", "ƒ"},
			{"euro", "€"},
		};

		/// <summary>
		/// Creates a character reference.
		/// </summary>
		/// <remarks>The <paramref name="entityName"/> can be passed either in its "entity name" form (e.g. <code>&amp;name;</code>)
		/// or in its "simple name" form (e.g. <code>name</code>).
		/// <para>Names (<see cref="EntityName"/> and <see cref="SimpleName"/>) are stored as lower-case, independently of the casing they were provided.</para></remarks>
		/// <param name="entityName">The name of the reference.</param>
		/// <exception cref="ArgumentNullException"><paramref name="entityName"/> is null.</exception>
		internal CharacterReference(string entityName)
		{
			Guard.AgainstNullArgument("entityName", entityName);

			if (IsEntityName(entityName))
			{
				EntityName = lower(entityName);
				SimpleName = toSimpleName(entityName);
			}
			else
			{
				EntityName = toEntityName(entityName);
				SimpleName = lower(entityName);
			}
			Character = _htmlEntities[SimpleName];
			CodePoint = char.ConvertToUtf32(Character, 0);
			EntityNumber = string.Concat(AMP, SHARP, CodePoint.ToString(CultureInfo.InvariantCulture), SEMICOLON);
			IsEmpty = false;
		}

		private static string toEntityName(string simpleEntityName)
		{
			return AMP + lower(simpleEntityName) + SEMICOLON;
		}

		private static string toSimpleName(string complexEntityName)
		{
			return lower(complexEntityName.Substring(1, complexEntityName.Length - 2));
		}

		private static string lower(string s)
		{
			return s.ToLowerInvariant();
		}

		/// <summary>
		/// The "simple name" form of a character reference.
		/// </summary>
		/// <example>new CharacterReference("&amp;pound;").SimpleName --> "pound"</example>
		public string SimpleName { get; private set; }

		/// <summary>
		/// The "entity name" form of a character reference.
		/// </summary>
		/// <example>new CharacterReference("&amp;pound;").EntityName --> "&amp;pound;"</example>
		public string EntityName { get; private set; }

		/// <summary>
		/// The "entity number" form of a character reference.
		/// </summary>
		/// <example>new CharacterReference("&amp;pound;").EntityNumber --> "&amp;163;"</example>
		public string EntityNumber { get; private set; }

		/// <summary>
		/// The unicode code point of a character reference.
		/// </summary>
		/// <example>new CharacterReference("&amp;pound;").CodePoint --> 163</example>
		public int CodePoint { get; private set; }

		/// <summary>
		/// The character of a character reference.
		/// </summary>
		/// <example>new CharacterReference("&amp;pound;").Character --> "£"</example>
		public string Character { get; private set; }

		/// <summary>
		/// Returns wheter the instance is empty or not.
		/// </summary>
		/// <remarks>To discourage the use of null, a "null object" pattern is used. "Null objects" are empty.</remarks>
		public bool IsEmpty { get; private set; }

		/// <summary>
		/// Returns a <see cref="string"/> that represents the current <see cref="CharacterReference"/>.
		/// </summary>
		/// <returns>
		/// The <see cref="SimpleName"/> of the current instance.
		/// </returns>
		public override string ToString()
		{
			return SimpleName;
		}

		private const string AMP = "&", SEMICOLON = ";", SHARP = "#";

		/// <summary>
		/// Returns a value indicating whether <paramref name="entityName"/> starts with the character
		/// <code>'&amp;'</code> and ends with the character <code>';'</code>.
		/// </summary>
		/// <param name="entityName">Name of the entity.</param>
		/// <returns>true if <paramref name="entityName"/> does starts with <code>'&amp;'</code> and ends with <code>';'</code>;
		/// otherwise, false</returns>
		/// <exception cref="ArgumentNullException"><paramref name="entityName"/> is null.</exception>
		public static bool IsEntityName(string entityName)
		{
			Guard.AgainstNullArgument("entityName", entityName);

			return entityName.StartsWith(AMP, StringComparison.Ordinal) &&
				entityName.EndsWith(SEMICOLON, StringComparison.Ordinal);
		}

		/// <summary>
		/// Indicates whether the current object is equal to another object of the same type.
		/// </summary>
		/// <returns>
		/// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
		/// </returns>
		/// <param name="other">A character reference to compare with this character reference.</param>
		public bool Equals(CharacterReference other)
		{
			if (ReferenceEquals(null, other)) return false;
			if (ReferenceEquals(this, other)) return true;
			return other.CodePoint == CodePoint;
		}

		/// <summary>
		/// Determines whether the specified <see cref="object"/> is equal to the current <see cref="object"/>.
		/// </summary>
		/// <returns>
		/// true if the specified <see cref="object"/> is equal to the current <see cref="object"/>; otherwise, false.
		/// </returns>
		/// <param name="obj">The <see cref="T:System.Object"/> to compare with the current <see cref="T:System.Object"/>.</param>
		/// <exception cref="NullReferenceException">The <paramref name="obj"/> parameter is null.</exception>
		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != typeof(CharacterReference)) return false;
			return Equals((CharacterReference)obj);
		}

		/// <summary>
		/// Serves as a hash function for a particular type. 
		/// </summary>
		/// <returns>
		/// A hash code for the current <see cref="CharacterReference"/>.
		/// </returns>
		public override int GetHashCode()
		{
			return CodePoint;
		}

		///<summary>Determines whether two specified character references have the same value.</summary>
		///<param name="left">The first character reference to compare, or null</param>
		///<param name="right">The second character reference to compare, or null</param>
		///<returns>true if the value of <paramref name="left"/> is the same as the value of <paramref name="right"/>; otherwise, false.</returns>
		public static bool operator ==(CharacterReference left, CharacterReference right)
		{
			return Equals(left, right);
		}

		///<summary>Determines whether two specified character references have different values.</summary>
		///<param name="left">The first character reference to compare, or null</param>
		///<param name="right">The second character reference to compare, or null</param>
		///<returns>true if the value of <paramref name="left"/> is is different from the value of <paramref name="right"/>; otherwise, false.</returns>
		public static bool operator !=(CharacterReference left, CharacterReference right)
		{
			return !Equals(left, right);
		}
	}
}
