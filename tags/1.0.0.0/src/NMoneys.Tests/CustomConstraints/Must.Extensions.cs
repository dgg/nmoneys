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

		public static Constraint JsonSerializable<T>(this Must.BeEntryPoint entryPoint, Func<T, Constraint> constraintOverDeserialized)
		{
			return new JsonSerializationConstraint<T>(constraintOverDeserialized);
		}

		public static Constraint JsonDeserializableInto<T>(this Must.BeEntryPoint entryPoint, T to)
		{
			return new JsonDeserializationConstraint<T>(to);
		}
	}
}
