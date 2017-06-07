using Testing.Commons;

namespace NMoneys.Tests.Change.Support
{
	internal static partial class MustExtensions
	{
		public static CompleteChangeConstraint CompleteChange(this Must.BeEntryPoint entry,
			CurrencyIsoCode code, params QDenomination[] denominations)
		{
			return new CompleteChangeConstraint(code, denominations);
		}

		public static IncompleteChangeConstraint CompleteChange(this Must.NotBeEntryPoint entry,
			Money remainder, params QDenomination[] denominations)
		{
			return new IncompleteChangeConstraint(remainder, denominations);
		}

		public static QDenomination x(this int quantity, int denominationValue)
		{
			return new QDenomination((uint) quantity, denominationValue);
		}

		public static QDenomination q(this int denominationValue, int quantity)
		{
			return new QDenomination((uint) quantity, denominationValue);
		}
	}
}