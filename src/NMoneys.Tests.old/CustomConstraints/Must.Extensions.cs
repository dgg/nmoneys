using System;
using NUnit.Framework.Constraints;
using Testing.Commons;

namespace NMoneys.Tests.CustomConstraints
{
	internal static partial class MustExtensions
	{
		public static Constraint MoneyWith(this Must.BeEntryPoint entryPoint, decimal amount, Currency currency)
		{
			return new MoneyConstraint(amount, currency);
		}

		public static CurrencyInfoConstraint CurrencyInfo(this Must.BeEntryPoint entryPoint)
		{
			return new CurrencyInfoConstraint();
		}

		public static Constraint ObsoleteEvent(this Must.RaiseEntryPoint entryPoint)
		{
			return new ObsoleteCurrencyRaisedConstraint(1);
		}

		public static Constraint ObsoleteEvent(this Must.RaiseEntryPoint entryPoint, uint times)
		{
			return new ObsoleteCurrencyRaisedConstraint(times);
		}

		public static Constraint ObsoleteEvent(this Must.NotRaiseEntryPoint entryPoint)
		{
			return new ObsoleteCurrencyRaisedConstraint(0);
		}

		public static Constraint EntityWith(this Must.BeEntryPoint entry, string entityName, string entityNumber)
		{
			return new CharacterReferenceConstraint(entityName, entityNumber);
		}

		internal static Constraint Incomplete(this Must.BeEntryPoint entry, Action<IncompleteAllocationConstraint> setup)
		{
			var incomplete = new IncompleteAllocationConstraint();
			setup(incomplete);
			return incomplete;
		}

		internal static Constraint Complete(this Must.BeEntryPoint entry, Action<CompleteAllocationConstraint> setup)
		{
			var complete = new CompleteAllocationConstraint();
			setup(complete);
			return complete;
		}

		internal static Constraint NoAllocation(this Must.BeEntryPoint entry, Action<NoAllocationConstraint> setup)
		{
			var no = new NoAllocationConstraint();
			setup(no);
			return no;
		}

		internal static Constraint QuasiComplete(this Must.BeEntryPoint entry, Action<QuasiCompleteAllocationConstraint> setup)
		{
			var quasiComplete = new QuasiCompleteAllocationConstraint();
			setup(quasiComplete);
			return quasiComplete;
		}
	}
}
