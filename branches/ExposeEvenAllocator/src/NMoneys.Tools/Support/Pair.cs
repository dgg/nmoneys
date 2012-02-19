namespace NMoneys.Tools.Support
{
	public class Pair
	{
		public Pair(object first, object second)
		{
			First = first;
			Second = second;
		}

		public object First { get; private set; }
		public object Second { get; private set; }
	}
}
