using System;
using System.Web;
using NMoneys.Support;

namespace NMoneys
{
	/// <summary>
	/// Represents a reserved character in an HTML/XML document.
	/// </summary>
	public class CharacterReference
	{
		/// <summary>
		/// Creates a character reference.
		/// </summary>
		/// <remarks>The <paramref name="entityName"/> can be passed either in its "entity name" form (e.g. <code>&name;</code>)
		/// or in its "simple name" form (e.g. <code>name</code>)</remarks>
		/// <param name="entityName">The name of the reference.</param>
		public CharacterReference(string entityName)
		{
			Guard.AgainstNullArgument("entityName", entityName);

			if (IsEntityName(entityName))
			{
				EntityName = entityName;
				SimpleName = toSimpleName(entityName);
			}
			else
			{
				EntityName = toEntityName(entityName);
				SimpleName = entityName;
			}
			EntityName = IsEntityName(entityName) ? entityName : toEntityName(entityName);
			Character = HttpUtility.HtmlDecode(EntityName);
			CodePoint = char.ConvertToUtf32(Character, 0);
			EntityNumber = string.Concat(AMP, SHARP, CodePoint.ToString(), SEMICOLON);
		}

		private string toEntityName(string simpleEntityName)
		{
			return AMP + simpleEntityName + SEMICOLON;
		}

		private string toSimpleName(string complexEntityName)
		{
			return complexEntityName.Substring(1, complexEntityName.Length - 2);
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
		/// <example>new CharacterReference("&amp;pound;").Character --> "pound"</example>
		public string Character { get; private set; }


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
		public static bool IsEntityName(string entityName)
		{
			return entityName.StartsWith(AMP, StringComparison.Ordinal) &&
				entityName.EndsWith(SEMICOLON, StringComparison.Ordinal);
		}
	}
}
