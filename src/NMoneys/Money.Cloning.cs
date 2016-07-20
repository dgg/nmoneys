using System;
using System.Diagnostics.Contracts;

namespace NMoneys
{
	public partial struct Money : ICloneable
	{
		/// <summary>
		/// Creates a new object that is a copy of the current instance.
		/// </summary>
		/// <returns>
		/// A new object that is a copy of this instance.
		/// </returns>
		[Pure]
		public object Clone()
		{
			return new Money(Amount, CurrencyCode);
		}
	}
}
