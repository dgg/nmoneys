using Testing.Commons;

namespace NMoneys.Tests.Change.Support
{
	internal static partial class MustExtensions
	{
		public static CompleteChangeConstraint CompleteChange(this Must.BeEntryPoint entry, uint totalCount,
			params QDenomination[] denominations)
		{
			return new CompleteChangeConstraint(totalCount, denominations);
		}

		public static NoChangeConstraint NoChange(this Must.BeEntryPoint entry,
			Money remainder)
		{
			return new NoChangeConstraint(remainder);
		}

		public static IncompleteChangeConstraint IncompleteChange(this Must.NotBeEntryPoint entry,
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

		public static OptimalChangeConstraint OptimalChange(this Must.BeEntryPoint entry,
			params QDenomination[] denominations)
		{
			return new OptimalChangeConstraint(denominations);
		}

		public static NoOptimalChangeConstraint NoChange(this Must.BeEntryPoint entry)
		{
			return new NoOptimalChangeConstraint();
		}
	}
}