namespace NMoneys.Serialization.Tests.Support;

internal static class TestExtensions
{
	/// <summary>
	/// "Compacts" the resulting JSON from Bson serialization.
	/// </summary>
	/// <remarks>Removes spaces from a non-indented JSON.</remarks>
	/// <param name="json"></param>
	/// <returns>JSON without spaces.</returns>
	public static string Compact(this string json)
	{
		return json.Replace(" ", string.Empty);
	}
}
