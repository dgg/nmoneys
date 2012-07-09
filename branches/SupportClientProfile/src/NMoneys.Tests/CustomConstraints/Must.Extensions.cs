using System;
using NUnit.Framework.Constraints;

namespace NMoneys.Tests.CustomConstraints
{
	internal static class MustExtensions
	{
		public static Constraint MoneyWith(this Must.BeEntryPoint entryPoint, decimal amount, Currency currency)
		{
			return new MoneyConstraint(amount, currency);
		}

		public static CurrencyInfoConstraint CurrencyInfo(this Must.BeEntryPoint entryPoint)
		{
			return new CurrencyInfoConstraint();
		}

		public static Constraint BinarySerializable<T>(this Must.BeEntryPoint entryPoint, Func<T, Constraint> constraintOverDeserialized)
		{
			return new BinarySerializationConstraint<T>(constraintOverDeserialized);
		}

		public static Constraint XmlSerializable<T>(this Must.BeEntryPoint entryPoint)
		{
			return new XmlSerializationConstraint<T>();
		}

		public static Constraint XmlDeserializableInto<T>(this Must.BeEntryPoint entryPoint, T to)
		{
			return new XmlDeserializationConstraint<T>(to);
		}

		public static Constraint DataContractSerializable<T>(this Must.BeEntryPoint entryPoint)
		{
			return new DataContractSerializationConstraint<T>();
		}

		public static Constraint DataContractDeserializableInto<T>(this Must.BeEntryPoint entryPoint, T to)
		{
			return new DataContractDeserializationConstraint<T>(to);
		}

		public static Constraint DataContractJsonSerializable<T>(this Must.NotBeEntryPoint entryPoint)
		{
			return new DataContractJsonSerializationConstraint<T>();
		}

		public static Constraint Once(this Must.RaiseObsoleteEventEntryPoint entryPoint)
		{
			return new ObsoleteCurrencyRaisedConstraint(1);
		}

		public static Constraint None(this Must.RaiseObsoleteEventEntryPoint entryPoint)
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
