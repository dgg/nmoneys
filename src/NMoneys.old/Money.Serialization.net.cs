using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace NMoneys
{
	[Serializable]
	public partial struct Money : ISerializable
	{
		/// <summary>
		/// Initializes a new instace of <see cref="Money"/> with serialized data
		/// </summary>
		/// <param name="info">The <see cref="SerializationInfo"/> that holds the serialized object data about the <see cref="Money"/>.</param>
		/// <param name="context">The <see cref="StreamingContext"/> that contains contextual information about the source or destination.</param>
		private Money(SerializationInfo info, StreamingContext context) :
			this(info.GetDecimal(Serialization.Data.Money.AMOUNT), (CurrencyIsoCode)info.GetValue(Serialization.Data.Money.CURRENCY, typeof(CurrencyIsoCode))) { }

		/// <summary>
		/// Populates a <see cref="SerializationInfo"/> with the data needed to serialize the target object.
		/// </summary>
		/// <param name="info">The <see cref="System.Runtime.Serialization.SerializationInfo"/> to populate with data.</param>
		/// <param name="context">The destination (see <see cref="T:System.Runtime.Serialization.StreamingContext"/>) for this serialization.</param>
		/// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue(Serialization.Data.Money.AMOUNT, Amount);
			info.AddValue(Serialization.Data.Money.CURRENCY, CurrencyCode, typeof(CurrencyIsoCode));
		}
	}
}
