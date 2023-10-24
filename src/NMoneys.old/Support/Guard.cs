using System;

namespace NMoneys.Support
{
	internal static class Guard
	{
		public static void AgainstNullArgument<T>(string paramName, T value) where T : class 
		{
			if (value == null) throw new ArgumentNullException(paramName);
		}

		public static U AgainstNullArgument<T, U>(string paramName, T value, Func<T, U> member) where T : class
		{
			AgainstNullArgument(paramName, value);
			return member(value);
		}

		public static void AgainstArgument(string paramName, bool clause)
		{
			if (clause) throw new ArgumentException(null, paramName);
		}

		public static void AgainstArgument(string paramName, bool clause, string message)
		{
			if (clause) throw new ArgumentException(message, paramName);
		}

		public static void AgainstArgument(string paramName, bool clause, string message, Exception inner)
		{
			if (clause) throw new ArgumentException(message, paramName, inner);
		}
	}
}
