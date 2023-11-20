namespace NMoneys.Support;

internal static class Ensuring
{
	public static U NotNull<T, U>(string paramName, T value, Func<T, U> member) where T : class
	{
		ArgumentNullException.ThrowIfNull(value, paramName);
		return member(value);
	}
}
