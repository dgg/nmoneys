using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using NMoneys.Support.Ext;

namespace NMoneys.Support
{
	/* based on: http://www.codeproject.com/KB/cs/EnumComparer.aspx */
	internal class FastEnumComparer<TEnum> : IEqualityComparer<TEnum> where TEnum : struct, IComparable, IFormattable, IConvertible
	{
		public static readonly IEqualityComparer<TEnum> Instance;

		private static readonly Func<TEnum, TEnum, bool> equals;
		private static readonly Func<TEnum, int> getHashCode;

		static FastEnumComparer()
		{
			getHashCode = generateGetHashCode();
			equals = generateEquals();
			Instance = new FastEnumComparer<TEnum>();
		}
		/// <summary>
		/// A private constructor to prevent user instantiation.
		/// </summary>
		private FastEnumComparer()
		{
			assertUnderlyingTypeIsSupported();
		}
		public bool Equals(TEnum x, TEnum y)
		{
			// call the generated method
			return equals(x, y);
		}

		public int GetHashCode(TEnum obj)
		{
			// call the generated method
			return getHashCode(obj);
		}

		private static readonly ICollection<Type> supportedTypes = new[]
			{
				typeof (byte), typeof (sbyte),
				typeof (short), typeof (ushort),
				typeof (int), typeof (uint),
				typeof (long), typeof (ulong)
			};
		private static void assertUnderlyingTypeIsSupported()
		{
			var underlyingType = Enum.GetUnderlyingType(typeof(TEnum));
			

			if (!supportedTypes.Contains(underlyingType))
			{
				throw new NotSupportedException(
					string.Format("The underlying type of '{0}' is {1}. Only enums with underlying type of [{2}] are supported.",
					typeof(TEnum).Name,
					underlyingType.Name,
					supportedTypes.ToDelimitedString(t => t.Name)));
			}
		}

		private static Func<TEnum, TEnum, bool> generateEquals()
		{
			var xParam = Expression.Parameter(typeof(TEnum), "x");
			var yParam = Expression.Parameter(typeof(TEnum), "y");
			var equalExpression = Expression.Equal(xParam, yParam);
			return Expression.Lambda<Func<TEnum, TEnum, bool>>(equalExpression, new[] { xParam, yParam }).Compile();
		}

		private static Func<TEnum, int> generateGetHashCode()
		{
			var objParam = Expression.Parameter(typeof(TEnum), "obj");
			var underlyingType = Enum.GetUnderlyingType(typeof(TEnum));
			var convertExpression = Expression.Convert(objParam, underlyingType);
			var getHashCodeMethod = underlyingType.GetMethod("GetHashCode");
			var getHashCodeExpression = Expression.Call(convertExpression, getHashCodeMethod);
			return Expression.Lambda<Func<TEnum, int>>(getHashCodeExpression, new[] { objParam }).Compile();
		}
	}
}
