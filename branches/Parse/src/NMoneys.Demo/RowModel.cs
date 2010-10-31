using System.Globalization;

namespace NMoneys.Demo
{
	public class RowModel
	{
		public static decimal PositiveAmount = 123.456m;
		public static decimal NegativeAmount = -PositiveAmount;

		public RowModel(Currency currency)
		{
			Currency = currency;
		}

		public Currency Currency { get; private set; }

		public string PositiveDemo { get { return new Money(PositiveAmount, Currency).ToString(); } }
		public string NegativeDemo { get { return new Money(NegativeAmount, Currency).ToString(); } }
		public string DecimalsDemo { get { return new Money(2.534m, Currency).ToString(); } }
		public string GroupsDemo { get { return new Money(1234567m, Currency).ToString(); } }

		public string ToString(decimal amount, NumberFormatInfo nf)
		{
			return new Money(amount, Currency).ToString(nf);
		}
	}
}
