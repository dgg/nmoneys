using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using NMoneys.Support.Ext;

namespace NMoneys.Support
{
	internal static class Enumeration
	{
		private static void assertEnum<TEnum>() where TEnum : struct, IComparable, IFormattable, IConvertible
		{
			Type tEnum = typeof(TEnum);
			if (!Reflect.IsEnum(tEnum))
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
						throw new ArgumentException(message());
					}
				}
			}
			catch (Exception ex)
			{
				throw new ArgumentException(message(), ex);
			}

		}

		public static void AssertDefined<TEnum>(TEnum value) where TEnum : struct, IComparable, IFormattable, IConvertible
		{
			assertEnum<TEnum>();
			Type tEnum = typeof(TEnum);
			if (!Enum.IsDefined(tEnum, value))
			{
				throw new ArgumentException($"Value {value} was not defined for type {tEnum}");
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
			Attribute[] attributes = Reflect.Attributes<TAttr>(field, false);
			if (attributes.Length == 1)
			{
				result = true;
				attribute = (TAttr)attributes[0];
			}
			return result;
		}

		private static FieldInfo fieldOf<TEnum>(TEnum value) where TEnum : struct, IComparable, IFormattable, IConvertible
		{
			return Reflect.Field<TEnum>(value.ToString(CultureInfo.InvariantCulture));
		}

		/* based on: http://www.codeproject.com/KB/cs/EnumComparer.aspx */
		public class Comparer<TEnum> : IEqualityComparer<TEnum> where TEnum : struct, IComparable, IFormattable, IConvertible
		{
			public static readonly IEqualityComparer<TEnum> Instance;

			private static readonly Func<TEnum, TEnum, bool> _equals;
			private static readonly Func<TEnum, int> _getHashCode;

			static Comparer()
			{
				assertEnum<TEnum>();
				_getHashCode = generateGetHashCode();
				_equals = generateEquals();
				Instance = new Comparer<TEnum>();
			}
			/// <summary>
			/// A private constructor to prevent user instantiation.
			/// </summary>
			private Comparer()
			{
				assertUnderlyingTypeIsSupported();
			}
			public bool Equals(TEnum x, TEnum y)
			{
				// call the generated method
				return _equals(x, y);
			}

			public int GetHashCode(TEnum obj)
			{
				// call the generated method
				return _getHashCode(obj);
			}

			private static readonly ICollection<Type> _supportedTypes = new[]
				{
				typeof (byte), typeof (sbyte),
				typeof (short), typeof (ushort),
				typeof (int), typeof (uint),
				typeof (long), typeof (ulong)
			};
			private static void assertUnderlyingTypeIsSupported()
			{
				var underlyingType = Enum.GetUnderlyingType(typeof(TEnum));


				if (!_supportedTypes.Contains(underlyingType))
				{
					throw new NotSupportedException(
						$@"The underlying type of '{typeof(TEnum).Name}' is {underlyingType.Name}. 
Only enums with underlying type of [{_supportedTypes.ToDelimitedString(t => t.Name)}] are supported.");
				}
			}

			private static Func<TEnum, TEnum, bool> generateEquals()
			{
				var xParam = Expression.Parameter(typeof(TEnum), "x");
				var yParam = Expression.Parameter(typeof(TEnum), "y");
				var equalExpression = Expression.Equal(xParam, yParam);
				return Expression.Lambda<Func<TEnum, TEnum, bool>>(equalExpression, xParam, yParam).Compile();
			}

			private static Func<TEnum, int> generateGetHashCode()
			{
				var objParam = Expression.Parameter(typeof(TEnum), "obj");
				var underlyingType = Enum.GetUnderlyingType(typeof(TEnum));
				var convertExpression = Expression.Convert(objParam, underlyingType);
				var getHashCodeMethod = Reflect.Method(underlyingType, nameof(object.GetHashCode));
				var getHashCodeExpression = Expression.Call(convertExpression, getHashCodeMethod);
				return Expression.Lambda<Func<TEnum, int>>(getHashCodeExpression, objParam).Compile();
			}
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
				throw new ArgumentException($"'{value}' is not defined within type {typeof(TEnum).Name}.");
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

		
	}
}
