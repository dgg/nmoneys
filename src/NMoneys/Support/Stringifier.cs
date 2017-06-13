using System;
using System.Collections.Generic;
using System.Linq;

namespace NMoneys.Support
{
	internal class Stringifier
	{
		private readonly string _startToken;
		private readonly string _endToken;
		private readonly string _separatorToken;

		public Stringifier() : this("< ", " >", " | ") { }

		public Stringifier(string startToken, string endToken, string separatorToken)
		{
			_startToken = startToken;
			_endToken = endToken;
			_separatorToken = separatorToken;
		}

		public string Stringify<T>(IEnumerable<T> collection)
		{
			return Stringify(collection, each => each.ToString());
		}

		public string Stringify<T>(IEnumerable<T> collection, Func<T, string> stringifier)
		{
			return _startToken + string.Join(_separatorToken, collection.Select(stringifier).ToArray()) + _endToken;
		}

		public static readonly  Stringifier Default = new Stringifier();
	}
}
