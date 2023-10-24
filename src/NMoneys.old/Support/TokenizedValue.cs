namespace NMoneys.Support
{
	internal abstract class TokenizedValue
	{
		public static readonly char Tokenizer = ' ';

		protected static string[] split(string value)
		{
			return value.Split(Tokenizer);
		}

		protected static string join(string[] values)
		{
			return string.Join(Tokenizer.ToString(), values);
		}
	}
}