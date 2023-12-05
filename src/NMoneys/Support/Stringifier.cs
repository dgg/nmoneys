namespace NMoneys.Support;

internal class Stringifier
{
	private string _startToken { get; init; }
	private string _endToken { get; init; }
	private string _separatorToken { get; init; }


	public Stringifier(string startToken = "< ", string endToken = " >", string separatorToken = " | ")
	{
		_startToken = startToken;
		_endToken = endToken;
		_separatorToken = separatorToken;
	}

	public string Stringify<T>(IEnumerable<T> collection)
	{
		return Stringify(collection, each => each?.ToString() ?? "'null'");
	}

	public string Stringify<T>(IEnumerable<T> collection, Func<T, string> stringifier)
	{
		return _startToken + string.Join(_separatorToken, collection.Select(stringifier).ToArray()) + _endToken;
	}

	public string StringifyIt<T>(T obj)
	{
		return StringifyIt(obj, each => each?.ToString() ?? "'null'");
	}

	public string StringifyIt<T>(T obj, Func<T, string> stringifier)
	{
		return _startToken + stringifier(obj) + _endToken;
	}

	public static readonly  Stringifier Default = new ();
}
