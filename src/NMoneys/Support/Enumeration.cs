using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using NMoneys.Support.Ext;

namespace NMoneys.Support
{
	internal static class Enumeration
	{
		private static void assertEnum<TEnum>() where TEnum : struct, IComparable, IFormattable, IConvertible
		{
			Type tEnum = typeof(TEnum);
			if (!tEnum.IsEnum)
			{
				throw new ArgumentException(
					$"The type {tEnum.Name} is not an enumeration",
					nameof(TEnum));
			}
		}

		public static void AssertDefined<TEnum>(string text) where TEnum : struct, IComparable, IFormattable, IConvertible
		{
			Guard.AgainstNullArgument("text", text);
			assertEnum<TEnum>();
			Type tEnum = typeof(TEnum);
			Func<string> message = () => $"Value {text} was not defined for type {tEnum}";
			try
			{
				if (!Enum.IsDefined(tEnum, text))
				{
					//check whether text represents a value of TEnum
					Type underlying = Enum.GetUnderlyingType(tEnum);
					object converted = Convert.ChangeType(text, underlying);
					if (converted == null || !Enum.IsDefined(tEnum, converted))
					{
						throw invalidEnumException(message());
					}
				}
			}
			catch (Exception ex)
			{
				throw invalidEnumException(message(), ex);
			}

		}

		public static void AssertDefined<TEnum>(TEnum value) where TEnum : struct, IComparable, IFormattable, IConvertible
		{
			assertEnum<TEnum>();
			Type tEnum = typeof(TEnum);
			if (!Enum.IsDefined(tEnum, value))
			{
				throw invalidEnumException($"Value {value} was not defined for type {tEnum}");
			}
		}

		private static bool checkDefined<TEnum>(string text) where TEnum : struct, IComparable, IFormattable, IConvertible
		{
			bool defined = false;
			if (!text.IsEmpty())
			{
				Type tEnum = typeof(TEnum);
				try
				{
					if (Enum.IsDefined(tEnum, text))
					{
						defined = true;
					}
					//check whether text represents a value of TEnum
					else
					{
						Type underlying = Enum.GetUnderlyingType(tEnum);
						object converted = Convert.ChangeType(text, underlying);
						defined = converted != null && Enum.IsDefined(tEnum, converted);
					}
				}
				// swallow exceptions on purpose
				catch { }
			}

			return defined;

		}

		public static bool CheckDefined<TEnum>(TEnum value) where TEnum : struct , IComparable, IFormattable, IConvertible
		{
			assertEnum<TEnum>();
			Type tEnum = typeof(TEnum);
			return Enum.IsDefined(tEnum, value);
		}

		public static bool CheckDefined<TEnum, U>(U value) where TEnum : struct, IComparable, IFormattable, IConvertible
		{
			assertEnum<TEnum>();
			return Enum.IsDefined(typeof(TEnum), value);
		}

		public static TEnum Parse<TEnum>(string text) where TEnum : struct, IComparable, IFormattable, IConvertible
		{
			assertEnum<TEnum>();
			AssertDefined<TEnum>(text);
			return (TEnum)Enum.Parse(typeof(TEnum), text);
		}

		public static bool TryParse<TEnum>(string input, out TEnum? result) where TEnum : struct, IComparable, IFormattable, IConvertible
		{
			assertEnum<TEnum>();
			Type enumType = typeof(TEnum);
			result = null;
			if (!checkDefined<TEnum>(input)) return false;
			result = (TEnum)Enum.Parse(enumType, input);
			return true;
		}

		public static TAttr GetAttribute<TEnum, TAttr>(TEnum value)
			where TEnum : struct, IComparable, IFormattable, IConvertible
			where TAttr : Attribute
		{
			assertEnum<TEnum>();
			FieldInfo field = fieldOf(value);
			return (TAttr)field.GetCustomAttributes(typeof(TAttr), false).Single();
		}

		public static bool HasAttribute<TEnum, TAttr>(TEnum value)
			where TEnum : struct, IComparable, IFormattable, IConvertible
			where TAttr : Attribute
		{
			assertEnum<TEnum>();
			FieldInfo field = fieldOf(value);
			return field.GetCustomAttributes(typeof(TAttr), false).Any();
		}

		public static bool TryGetAttribute<TEnum, TAttr>(TEnum value, out TAttr attribute)
			where TEnum : struct, IComparable, IFormattable, IConvertible
			where TAttr : Attribute
		{
			assertEnum<TEnum>();
			bool result = false;
			attribute = null;
			FieldInfo field = fieldOf(value);
			object[] attributes = field.GetCustomAttributes(typeof(TAttr), false);
			if (attributes.Length == 1)
			{
				result = true;
				attribute = (TAttr)attributes[0];
			}
			return result;
		}

		private static FieldInfo fieldOf<TEnum>(TEnum value) where TEnum : struct, IComparable, IFormattable, IConvertible
		{
			return typeof(TEnum).GetField(value.ToString(CultureInfo.InvariantCulture));
		}

		public static IEqualityComparer<TEnum> Comparer<TEnum>() where TEnum : struct, IComparable, IFormattable, IConvertible
		{
			assertEnum<TEnum>();
			return FastEnumComparer<TEnum>.Instance;
		}

		public static TEnum[] GetValues<TEnum>() where TEnum : struct, IComparable, IFormattable, IConvertible
		{
			assertEnum<TEnum>();
			return (TEnum[])Enum.GetValues(typeof(TEnum));
		}

		public static TEnum Cast<TEnum>(short value) where TEnum : struct, IComparable, IFormattable, IConvertible
		{
			TEnum? casted;
			bool result = TryCast(value, out casted);
			if (!result)
			{
				throw invalidEnumException($"'{value}' is not defined within type {typeof(TEnum).Name}.");
			}
			return casted.Value;
		}

		public static bool TryCast<TEnum>(short value, out TEnum? casted) where TEnum : struct, IComparable, IFormattable, IConvertible
		{
			casted = null;
			bool success = CheckDefined<TEnum, short>(value);
			if (success) casted = (TEnum)Enum.ToObject(typeof(TEnum), value);
			return success;
		}

		private static Exception invalidEnumException(string message)
		{
#if NET
			return new InvalidEnumArgumentException(message);
#else
			// InvalidArgumentException does not exist in netstandard, the superclass is used
			return new ArgumentException(message);
#endif
		}
		private static Exception invalidEnumException(string message, Exception inner)
		{
#if NET
			return new InvalidEnumArgumentException(message, inner);
#else
			// InvalidArgumentException does not exist in netstandard, the superclass is used
			return new ArgumentException(message, inner);
#endif
		}
	}
}
