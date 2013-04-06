using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace NMoneys
{
	[Serializable]
	public sealed partial class Currency : ISerializable
	{
		/// <summary>
		/// Initializes a new instace of <see cref="Currency"/> with serialized data
		/// </summary>
		/// <remarks>Only the iso code is serialized, the rest of the state is retrieved from <see cref="Currency"/> obtained by the 
		/// <see cref="Get(NMoneys.CurrencyIsoCode)"/> creation method.</remarks>
		/// <param name="info">The <see cref="SerializationInfo"/> that holds the serialized object data about the <see cref="Currency"/>.</param>
		/// <param name="context">The <see cref="StreamingContext"/> that contains contextual information about the source or destination.</param>
		private Currency(SerializationInfo info, StreamingContext context)
		{
			CurrencyIsoCode isoCode = Code.ParseArgument(
				(string)info.GetValue(Serialization.Data.Currency.ISO_CODE, typeof(string)),
				Serialization.Data.Currency.ISO_CODE);
			// get a paradigm with the most current values
			Currency paradigm = Get(isoCode);
			setAllFields(paradigm.IsoCode, paradigm.EnglishName, paradigm.NativeName, paradigm.Symbol,
				paradigm.SignificantDecimalDigits, paradigm.DecimalSeparator, paradigm.GroupSeparator, paradigm.GroupSizes,
				paradigm.PositivePattern, paradigm.NegativePattern, paradigm.IsObsolete, paradigm.Entity);
		}

		/// <summary>
		/// Sets the <see cref="SerializationInfo"/> with information about the currency. 
		/// </summary>
		/// <remarks>It will only persist information regarding the <see cref="IsoCode"/>.
		/// <para>The rest of the information will be populated from the instance obtained from creation methods.</para>
		/// </remarks>
		/// <param name="info">The <see cref="SerializationInfo"/> that holds the serialized object data about the <see cref="Currency"/>.</param>
		/// <param name="context">The <see cref="StreamingContext"/> that contains contextual information about the source or destination.</param>
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue(Serialization.Data.Currency.ISO_CODE, IsoSymbol);
		}
	}
}
