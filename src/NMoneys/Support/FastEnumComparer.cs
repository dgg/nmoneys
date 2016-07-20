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

		private static readonly Func<TEnum, TEnum, bool> _equals;
		private static readonly Func<TEnum, int> _getHashCode;

		static FastEnumComparer()
		{
			_getHashCode = generateGetHashCode();
			_equals = generateEquals();
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
			var getHashCodeMethod = underlyingType.GetMethod("GetHashCode");
			var getHashCodeExpression = Expression.Call(convertExpression, getHashCodeMethod);
			return Expression.Lambda<Func<TEnum, int>>(getHashCodeExpression, objParam).Compile();
		}
	}
}
