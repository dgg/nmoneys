using System;
using System.Collections.Generic;
using NMoneys.Support;

namespace NMoneys
{
	///<summary>
	/// Represents all reserved currency characters in an HTML/XML document.
	///</summary>
	public static class CurrencyCharacterReferences
	{
		/// <summary>
		/// Gets a <see cref="CharacterReference"/> instance from its simple name.
		/// </summary>
		/// <example>CharacterReference cent = CurrencyCharacterReferences.Get("cent");</example>
		/// <param name="entityName">Simple name of the entity to retrieve. Is case-insensitive.</param>
		/// <returns>The instance of the <see cref="CharacterReference"/> corresponding to the <paramref name="entityName"/>.</returns>
		/// <seealso cref="CharacterReference.SimpleName"/>
		/// <exception cref="ArgumentNullException"><paramref name="entityName"/> is null.</exception>
		/// <exception cref="KeyNotFoundException"><paramref name="entityName"/> is not a currency character reference.</exception>
		/// <seealso cref="Cent"/>
		/// <seealso cref="Pound"/>
		/// <seealso cref="Curren"/>
		/// <seealso cref="Yen"/>
		/// <seealso cref="Fnof"/>
		public static CharacterReference Get(string entityName)
		{
			Guard.AgainstNullArgument("entityName", entityName);
			return _currencyReferences[entityName];
		}

		private static readonly IDictionary<string, CharacterReference> _currencyReferences;
		static CurrencyCharacterReferences()
		{
			_currencyReferences = new Dictionary<string, CharacterReference>(6, StringComparer.OrdinalIgnoreCase)
			{
				{"cent", Cent = new CharacterReference("cent")},
				{"pound", Pound = new CharacterReference("pound")},
				{"curren", Curren = new CharacterReference("curren")},
				{"yen", Yen = new CharacterReference("yen")},
				{"fnof", Fnof = new CharacterReference("fnof")},
				{"euro", Euro = new CharacterReference("euro")}
			};
		}

		/// <summary>
		/// Cent character reference: &#162;
		/// </summary>
		public static readonly CharacterReference Cent;

		/// <summary>
		/// Pound character reference: &#163;
		/// </summary>
		public static readonly CharacterReference Pound;

		/// <summary>
		/// Generic currency character reference: &#164;
		/// </summary>
		public static readonly CharacterReference Curren;

		/// <summary>
		/// Yen character reference: &#165;
		/// </summary>
		public static readonly CharacterReference Yen;

		/// <summary>
		/// Florin character reference: &#402;
		/// </summary>
		public static readonly CharacterReference Fnof;

		/// <summary>
		/// Florin character reference: &#8364;
		/// </summary>
		public static readonly CharacterReference Euro;

	}
}