using System;
using System.Globalization;

namespace NMoneys
{
	/// <summary>
	/// Represents that a .NET Framework culture is suitable for representing an ISO currency
	/// </summary>
	/// <remarks>For internal verification purposes, it does not have any effect on currency runtime.</remarks>
	[AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
	internal sealed class CanonicalCultureAttribute : Attribute
	{
		public CanonicalCultureAttribute(string cultureName)
		{
			_name = cultureName;
		}

		private readonly string _name;
		/// <summary>
		/// Name of the <see cref="CultureInfo"/> that is source of the information
		/// </summary>
		public string Name { get { return _name; } }

		/// <summary>
		/// Indicates whether one or more values of the currency are different from ones specified by the <see cref="CultureInfo"/>.
		/// </summary>
		public bool Overwritten { get; set; }

		/// <summary>
		/// Source of the information
		/// </summary>
		/// <remarks>It is a delegate as we do not want to use their value within the progrma but just as a support
		/// to the tools that compare the information in the .NET Framework with the information in the Xml.</remarks>
		public Func<CultureInfo> Culture { get { return () => CultureInfo.GetCultureInfo(Name); } }
	}
}
