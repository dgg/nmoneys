using System;
using System.Diagnostics.Contracts;
using System.Globalization;

namespace NMoneys
{
	public sealed partial class Currency : IFormatProvider
	{
		/// <summary>
		/// Returns a <see cref="string"/> that represents the current <see cref="Currency"/>.
		/// </summary>
		/// <remarks>It actually is a representation of the <see cref="IsoCode"/>.</remarks>
		/// <returns>
		/// A <see cref="string"/> that represents the current <see cref="Currency"/>.
		/// </returns>
		[Pure]
		public override string ToString()
		{
			return IsoCode.ToString();
		}

		/// <summary>
		/// Returns an object that provides formatting services for the specified type.
		/// </summary>
		/// <returns>
		/// An instance of the object specified by <paramref name="formatType"/>,
		/// if the <see cref="IFormatProvider"/> implementation is <see cref="NumberFormatInfo"/>; otherwise, null.
		/// </returns>
		/// <param name="formatType">An object that specifies the type of format object to return.</param>
		[Pure]
		public object GetFormat(Type formatType)
		{
			return formatType == typeof(NumberFormatInfo) ? FormatInfo : null;
		}
	}
}
