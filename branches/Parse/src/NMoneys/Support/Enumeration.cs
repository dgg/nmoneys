using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using NMoneys.Support.Ext;

namespace NMoneys.Support
{
	internal static class Enumeration
	{
		private static void assertEnum<TEnum>() where TEnum : struct, IComparable, IFormattable, IConvertible
		{
			Type tEnum = typeof (TEnum);
			if (!tEnum.IsEnum)
			{
				throw new ArgumentException(
					string.Format("The type {0} is not an enumeration", tEnum.Name),
					"TEnum");
			}
		}

		public static void AssertDefined<TEnum>(string text) where TEnum : struct, IComparable, IFormattable, IConvertible
		{
			assertEnum<TEnum>();
			Type tEnum = typeof (TEnum);
			if (!Enum.IsDefined(tEnum, text))
			{
				throw new InvalidEnumArgumentException(string.Format("Value {0} was not defined for type {1}", text, tEnum));
			}
		}

		public static void AssertDefined<TEnum>(TEnum value) where TEnum : struct, IComparable, IFormattable, IConvertible
		{
			assertEnum<TEnum>();
			Type tEnum = typeof(TEnum);
			if (!Enum.IsDefined(tEnum, value))
			{
				throw new InvalidEnumArgumentException(string.Format("Value {0} was not defined for type {1}", value, tEnum));
			}
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
			if (input.IsEmpty() || !Enum.IsDefined(enumType, input)) return false;

			result = (TEnum)Enum.Parse(enumType, input);
			return true;
		}

		public static TAttr GetAttribute<TEnum, TAttr>(TEnum value)
			where TEnum : struct, IComparable, IFormattable, IConvertible
			where TAttr : Attribute
		{
			assertEnum<TEnum>();
			FieldInfo field = fieldOf(value);
			return (TAttr)field.GetCustomAttributes(typeof (TAttr), false).Single();
		}

		public static bool TryGetAttribute<TEnum, TAttr>(TEnum value, out TAttr attribute)
			where TEnum : struct, IComparable, IFormattable, IConvertible
			where TAttr : Attribute
		{
			assertEnum<TEnum>();
			bool result = false;
			attribute = null;
			FieldInfo field = fieldOf(value);
			object[] attributes= field.GetCustomAttributes(typeof (TAttr), false);
			if (attributes.Length  == 1)
			{
				result = true;
				attribute = (TAttr) attributes[0];
			}
			return result;
		}

		private static FieldInfo fieldOf<TEnum>(TEnum value) where TEnum : struct, IComparable, IFormattable, IConvertible
		{
			return typeof(TEnum).GetField(value.ToString());
		}

		public static IEqualityComparer<TEnum> Comparer<TEnum>() where TEnum : struct, IComparable, IFormattable, IConvertible
		{
			assertEnum<TEnum>();
			return FastEnumComparer<TEnum>.Instance;
		}

		public static TEnum[] GetValues<TEnum>()where TEnum : struct, IComparable, IFormattable, IConvertible
		{
			assertEnum<TEnum>();
			return (TEnum[])Enum.GetValues(typeof (TEnum));
		}
	}
}
