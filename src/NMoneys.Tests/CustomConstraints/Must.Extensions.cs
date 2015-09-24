using System;
using NUnit.Framework.Constraints;
using Testing.Commons;
using Testing.Commons.NUnit.Constraints;
using Testing.Commons.Serialization;

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

		public static Constraint BinarySerializable<T>(this Must.BeEntryPoint entryPoint, Constraint constraintOverDeserialized)
		{
			return new SerializationConstraint<T>(new BinaryRoundtripSerializer<T>(), constraintOverDeserialized);
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

		public static Constraint DataContractJsonSerializable<T>(this Must.BeEntryPoint entryPoint)
		{
			return new DataContractJsonSerializationConstraint<T>();
		}

		public static Constraint DataContractJsonDeserializableInto<T>(this Must.BeEntryPoint entryPoint, T to)
		{
			return new DataContractJsonDeserializationConstraint<T>(to);
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
