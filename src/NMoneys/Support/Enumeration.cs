using System.Linq.Expressions;

namespace NMoneys.Support;

internal static class Enumeration
{

	/*
	 * benchmark results:
	   Apple M2 Max, 1 CPU, 12 logical and 12 physical cores
	   .NET SDK 7.0.306
	   [Host]     : .NET 7.0.9 (7.0.923.32018), Arm64 RyuJIT AdvSIMD
	   DefaultJob : .NET 7.0.9 (7.0.923.32018), Arm64 RyuJIT AdvSIMD

	   | Method             | Mean      | Error     | StdDev    |
	   |------------------- |----------:|----------:|----------:|
	   | Native_False       | 7.2615 ns | 0.0148 ns | 0.0139 ns |
	   | Native_True        | 6.3695 ns | 0.0209 ns | 0.0196 ns |
	   | FastComparer_False | 0.4686 ns | 0.0057 ns | 0.0053 ns |
	   | FastComparer_True  | 0.4799 ns | 0.0123 ns | 0.0115 ns |
	 */
	/* based on: http://www.codeproject.com/KB/cs/EnumComparer.aspx */

	public static readonly IEqualityComparer<CurrencyIsoCode> FastComparer = new Comparer();
	public class Comparer : IEqualityComparer<CurrencyIsoCode>
	{
		private static readonly Func<CurrencyIsoCode, CurrencyIsoCode, bool> _equals = generateEquals();
		private static readonly Func<CurrencyIsoCode, int> _getHashCode = generateGetHashCode();


		private static Func<CurrencyIsoCode, int> generateGetHashCode()
		{
			Type codeType = typeof(CurrencyIsoCode);
			var objParam = Expression.Parameter(codeType, "obj");
			var underlyingType = Enum.GetUnderlyingType(codeType);
			var convertExpression = Expression.Convert(objParam, underlyingType);
			var getHashCodeMethod = underlyingType.GetMethod(nameof(object.GetHashCode));
			var getHashCodeExpression = Expression.Call(convertExpression, getHashCodeMethod!);
			return Expression.Lambda<Func<CurrencyIsoCode, int>>(getHashCodeExpression, objParam).Compile();
		}

		private static Func<CurrencyIsoCode, CurrencyIsoCode, bool> generateEquals()
		{
			Type codeType = typeof(CurrencyIsoCode);
			var xParam = Expression.Parameter(codeType, "x");
			var yParam = Expression.Parameter(codeType, "y");
			var equalExpression = Expression.Equal(xParam, yParam);
			return Expression.Lambda<Func<CurrencyIsoCode, CurrencyIsoCode, bool>>(equalExpression, xParam, yParam)
				.Compile();
		}

		public bool Equals(CurrencyIsoCode x, CurrencyIsoCode y)
		{
			// call the generated method
			return _equals(x, y);
		}

		public int GetHashCode(CurrencyIsoCode obj)
		{
			// call the generated method
			return _getHashCode(obj);
		}
	}
}
